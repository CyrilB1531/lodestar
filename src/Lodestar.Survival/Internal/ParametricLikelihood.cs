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
                Jet cumulative = model.CumulativeHazard(i, t);
                return sample.Events[i] ? model.LogHazard(i, t) - cumulative : -cumulative;
            case Censoring.Left:
                double time = sample.Upper[i];
                Jet logCdf = model.LogOneMinusSurvival(i, time);
                return sample.Events[i] ? model.LogHazard(i, time) - model.CumulativeHazard(i, time) : logCdf;
            default:
                if (sample.Events[i])
                {
                    return model.LogHazard(i, sample.Upper[i]) - model.CumulativeHazard(i, sample.Upper[i]);
                }

                Jet difference = Jet.Exp(-model.CumulativeHazard(i, sample.Lower[i])) - Jet.Exp(-model.CumulativeHazard(i, sample.Upper[i]));
                if (difference.Value < 1e-25)
                {
                    return Jet.Constant(Math.Log(1e-25), difference.Size);
                }

                return difference.Value > 1.0 - 1e-25 ? Jet.Constant(Special.Log1p(-1e-25), difference.Size) : Jet.Log(difference);
        }
    }
}

/// <summary>An observation's hazards as jets: the bridge from a univariate or a regression model to the likelihood.</summary>
internal interface IObservationModel
{
    Jet CumulativeHazard(int observation, double t);

    Jet LogHazard(int observation, double t);

    Jet LogOneMinusSurvival(int observation, double t);
}
