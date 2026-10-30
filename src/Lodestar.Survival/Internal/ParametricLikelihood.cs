namespace Lodestar.Survival.Internal;

/// <summary>How the observations are censored, which picks lifelines' likelihood.</summary>
internal enum Censoring
{
    Right,
    Left,
    Interval,
}

/// <summary>A censored sample: the times, the event flags, the weights and the entry times, validated.</summary>
/// <param name="Censoring">Which likelihood the sample takes.</param>
/// <param name="Lower">The durations, or the lower bounds of interval-censored times, clipped to <c>1e-20</c> as lifelines clips them.</param>
/// <param name="Upper">The upper bounds of interval-censored times, clipped to <c>1e25</c>; the durations otherwise.</param>
/// <param name="Events">Whether each time is an observed event; for interval censoring, whether its bounds coincide.</param>
/// <param name="Weights">One positive weight per observation.</param>
/// <param name="Entries">Each observation's entry time, zero for none.</param>
internal sealed record CensoredSample(
    Censoring Censoring, double[] Lower, double[] Upper, bool[] Events, double[] Weights, double[] Entries)
{
    public int Count => Lower.Length;

    public double TotalWeight => Weights.Sum();

    /// <summary>The times a model starts from: the durations, or the upper bounds where they are finite.</summary>
    public double[] StartTimes => Censoring == Censoring.Interval ? [.. Upper.Select((u, i) => u < 1e25 ? u : Lower[i])] : Upper;
}

/// <summary>lifelines' log-likelihoods, summed rather than averaged: <c>log_likelihood_</c> is the sum, the objective its mean.</summary>
internal static class ParametricLikelihood
{
    /// <summary>The log-likelihood of <paramref name="sample"/> with each observation's hazards from <paramref name="model"/>.</summary>
    /// <param name="sample">The sample.</param>
    /// <param name="model">For observation <c>i</c> and time <c>t</c>: its cumulative hazard, log hazard and <c>log(1 − S)</c>.</param>
    /// <param name="size">How many variables the jets carry.</param>
    /// <remarks>
    /// Right-censored, <c>Σ w (e log h(T) − H(T))</c>; left-censored, <c>Σ w (e (log h − H − log(1 − S)) + log(1 − S))</c> at
    /// <c>T</c>; interval-censored, <c>log h − H</c> at the time for an event and <c>log(S(L) − S(U))</c>, clipped to
    /// <c>[1e-25, 1 − 1e-25]</c>, for an interval; and every entry after zero adds <c>w H(entry)</c>.
    /// </remarks>
    public static Jet LogLikelihood(CensoredSample sample, IObservationModel model, int size)
    {
        Jet total = Jet.Constant(0.0, size);
        for (int i = 0; i < sample.Count; i++)
        {
            total += sample.Weights[i] * Term(sample, model, i);
        }

        return total;
    }

    /// <summary>Refuses a fit whose every term is a probability and whose likelihood has reached their supremum, zero, as
    /// it does only in a limit: twenty intervals sharing [1.19, 1.5] took a Weibull ρ to 783 at a standard error of 6e7,
    /// a point mass the doubles round to, read as a maximum (#1756).</summary>
    public static void RequireMaximum(CensoredSample sample, double logLikelihood)
    {
        // Against the total weight, which scales the likelihood: weights of 1e-11 left a proper maximum within 1e-9 of one.
        if (!sample.Events.Any(e => e) && logLikelihood > -1e-9 * sample.TotalWeight)
        {
            throw new InvalidOperationException(
                "Every observation is a probability, and the likelihood reaches their supremum of one only in the limit of a "
                + "point mass: the fit has no maximum on this sample.");
        }
    }

    /// <summary>Observation <paramref name="i"/>'s unweighted term, its delayed entry included: what lifelines' sandwich differentiates.</summary>
    public static Jet Term(CensoredSample sample, IObservationModel model, int i)
    {
        Jet term = Observation(sample, model, i);
        return sample.Entries[i] > 0.0 ? term + model.CumulativeHazard(i, sample.Entries[i]) : term;
    }

    private static Jet Observation(CensoredSample sample, IObservationModel model, int i)
    {
        switch (sample.Censoring)
        {
            case Censoring.Right:
                double t = sample.Upper[i];
                return sample.Events[i] ? model.LogDensity(i, t) : -model.CumulativeHazard(i, t);
            case Censoring.Left:
                double time = sample.Upper[i];
                return sample.Events[i] ? model.LogDensity(i, time) : model.LogOneMinusSurvival(i, time);
            default:
                if (sample.Events[i])
                {
                    return model.LogDensity(i, sample.Upper[i]);
                }

                return IntervalTerm(model.CumulativeHazard(i, sample.Lower[i]), model.CumulativeHazard(i, sample.Upper[i]));
        }
    }

    /// <summary><c>log(S(L) − S(U))</c> as <c>−H(L) + log(1 − e^(H(L) − H(U)))</c>, clipped to <c>[1e-25, 1 − 1e-25]</c>
    /// as lifelines clips the difference: taken as one, two survivals near one left 0.01 of rounding in an exponential
    /// fit's log-likelihood, and Newton stalled where main and lifelines converge (Review A, pass 35).</summary>
    private static Jet IntervalTerm(Jet lower, Jet upper)
    {
        double gap = upper.Value - lower.Value;
        double probability = gap > 0.0 ? Math.Exp(-lower.Value) * -NearZero.Expm1(-gap) : 0.0;
        if (probability < 1e-25)
        {
            return Jet.Constant(Math.Log(1e-25), lower.Size);
        }
        if (probability > 1.0 - 1e-25)
        {
            return Jet.Constant(NearZero.Log1P(-1e-25), lower.Size);
        }

        // At an infinite upper end the probability is the survival at the lower one.
        return double.IsPositiveInfinity(upper.Value) ? -lower : Jet.Log1mExp(upper - lower) - lower;
    }
}

/// <summary>An observation's hazards as jets: the bridge from a univariate or a regression model to the likelihood.</summary>
internal interface IObservationModel
{
    Jet CumulativeHazard(int observation, double t);

    Jet LogHazard(int observation, double t);

    /// <summary><c>log h − H</c>, an event's term.</summary>
    Jet LogDensity(int observation, double t);

    Jet LogOneMinusSurvival(int observation, double t);
}
