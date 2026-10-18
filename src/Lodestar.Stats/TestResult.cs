namespace Lodestar.Stats;

/// <summary>A t-test's result: the statistic, the p-value and the degrees of freedom.</summary>
/// <param name="Statistic">The t statistic.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
/// <param name="DegreesOfFreedom">
/// The degrees of freedom. Integral for Student and for the paired and
/// one-sample tests; fractional for Welch, whose Satterthwaite denominator is
/// not a count of anything.
/// </param>
public sealed record TTestResult(double Statistic, double PValue, double DegreesOfFreedom)
{
    /// <summary>The quantity the test compared: a mean, or a difference of means.</summary>
    /// <remarks>
    /// Internal rather than public. It exists so a later confidence-interval
    /// method can be added to the result instead of a second call that
    /// re-derives everything, and scipy keeps it hidden on its own result for
    /// the same reason.
    /// </remarks>
    internal double Estimate { get; init; }

    /// <summary>The standard error of <see cref="Estimate"/>. Internal, as above.</summary>
    internal double StandardError { get; init; }

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the difference this test measured.</summary>
    /// <remarks>
    /// A method, not a property, because it takes a level -- scipy exposes it
    /// the same way via <c>TtestResult.confidence_interval</c>; a named tuple
    /// avoids a public record for what is only a two-double carrier. A
    /// one-sided interval is half-open, not narrower: it says nothing about how
    /// far the difference could be, so the far bound is an infinity rather than a number.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>.
    /// </exception>
    public (double Low, double High) ConfidenceInterval(double level = 0.95)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // A NaN kept in the input, or no degree of freedom, leaves nothing to bound: scipy's interval is NaN.
        if (!(DegreesOfFreedom > 0.0))
        {
            return (double.NaN, double.NaN);
        }

        // A one-sided test spends its whole error budget on one side, so the tail
        // is 1 - level rather than half of it, and the other bound is infinite: StudentMultiplier takes it.
        // S1244: a zero standard error spans nothing, even where the quantile is past every double (#1690's review),
        // and scipy's finite quantile there gives the estimate as both bounds.
#pragma warning disable S1244
        double half = StandardError == 0.0 ? 0.0 : StudentMultiplier(level) * StandardError;
#pragma warning restore S1244

        // The open bound is scipy's estimate plus an infinite quantile times the standard error, NaN where that is zero.
        double open = double.PositiveInfinity * StandardError;
        return Alternative switch
        {
            Alternative.TwoSided => (Estimate - half, Estimate + half),
            Alternative.Greater => (Estimate - half, Estimate + open),
            Alternative.Less => (Estimate - open, Estimate + half),
            // CA2208: Alternative is a field, not a parameter here -- a bad
            // value stored on the result is a broken invariant, not a bad call.
            _ => throw new InvalidOperationException($"Unrecognised alternative: {Alternative}."),
        };
    }

    /// <summary>The upper Student quantile of what lies outside <paramref name="level"/>, half of it two-sided: one-sided
    /// below one half by symmetry from the level itself, where <c>1 − level</c> rounded to one below 2^-53 and was
    /// refused under a parameter the caller never passed, and kept six digits at 1e-10 (#1729's Review A).</summary>
    private double StudentMultiplier(double level)
    {
        // The inversion's own root, finite at its cap as main read it, not the public quantile's ±∞ there.
        if (Alternative == Alternative.TwoSided)
        {
            return Internal.Beta.StudentQuantile((1.0 - level) / 2.0, DegreesOfFreedom);
        }
        return level < 0.5
            ? -Internal.Beta.StudentQuantile(level, DegreesOfFreedom)
            : Internal.Beta.StudentQuantile(1.0 - level, DegreesOfFreedom);
    }
}

/// <summary>A Pearson correlation and the p-value that goes with it.</summary>
/// <remarks>
/// Its own record rather than <see cref="TestResult"/> for the reason
/// <see cref="TTestResult"/> is: the interval needs the sample size and the tail that were
/// used, and a second call that re-derived them would be a second chance to disagree.
/// scipy carries the same two on its own <c>PearsonRResult</c>.
/// </remarks>
/// <param name="Statistic">The correlation coefficient, in <c>[-1, 1]</c>.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
public sealed record PearsonResult(double Statistic, double PValue)
{
    /// <summary>The number of pairs the correlation was computed over; -1 on a record a caller built, which carries none.</summary>
    internal int N { get; init; } = -1;

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the correlation, through the Fisher z transform.</summary>
    /// <remarks>
    /// A method rather than a property because it takes a level, as <c>scipy.stats.pearsonr(...).confidence_interval()</c>
    /// does. A one-sided interval is half-open, its far bound the correlation's own limit of <c>-1</c> or <c>1</c>. Below
    /// four pairs <c>1 / sqrt(n - 3)</c> is undefined and the whole range is returned, as scipy returns it; below two
    /// there is no correlation to bound, the computed bound is <c>NaN</c>, and a one-sided interval keeps its fixed one.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>.
    /// </exception>
    public (double Low, double High) ConfidenceInterval(double level = 0.95)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // Below two pairs there is no correlation to bound, NaN as decision 0010 rules; from two to three, or a record built
        // without a count, no standard error, and the whole range as scipy answers it. Past that a NaN reaches the bound alone.
        if (N is 0 or 1)
        {
            // The fixed bound of a one-sided interval is still true of no correlation, as it is of a NaN one below.
            return Alternative switch
            {
                Alternative.Less => (-1.0, double.NaN),
                Alternative.Greater => (double.NaN, 1.0),
                _ => (double.NaN, double.NaN),
            };
        }
        if (N <= 3)
        {
            return (-1.0, 1.0);
        }

        double z = Internal.Fisher.Atanh(Statistic);
        double error = Math.Sqrt(1.0 / (N - 3.0));

        // A one-sided level spends its whole error budget on one side, so the quantile is
        // taken at the level itself rather than at half of what is left outside it.
        return Alternative switch
        {
            Alternative.TwoSided => Bounds(z, error, CriticalValue.TwoSided(level)),
            Alternative.Less => (-1.0, Math.Tanh(z + (Distributions.NormalQuantile(level) * error))),
            Alternative.Greater => (Math.Tanh(z - (Distributions.NormalQuantile(level) * error)), 1.0),
            // CA2208: Alternative is a field, not a parameter here -- a bad value stored on
            // the result is a broken invariant, not a bad call.
            _ => throw new InvalidOperationException($"Unrecognised alternative: {Alternative}."),
        };
    }

    private static (double Low, double High) Bounds(double z, double error, double quantile) =>
        (Math.Tanh(z - (quantile * error)), Math.Tanh(z + (quantile * error)));
}

/// <summary>A binomial test's result: the observed proportion and the p-value.</summary>
/// <remarks>
/// Its own record because the interval needs the counts and the tail that were tested, the same
/// reason <see cref="TTestResult"/> and <see cref="PearsonResult"/> carry theirs. scipy keeps
/// <c>k</c>, <c>n</c> and <c>alternative</c> on <c>BinomTestResult</c> for the same purpose.
/// </remarks>
/// <param name="Statistic">The observed proportion of successes, <c>successes / trials</c>.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
public sealed record BinomialResult(double Statistic, double PValue)
{
    /// <summary>The successes the test was given.</summary>
    internal int Successes { get; init; }

    /// <summary>The trials the test was given.</summary>
    internal int Trials { get; init; }

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the proportion.</summary>
    /// <remarks>
    /// A method rather than a property because it takes a level and a shape, as
    /// <c>BinomTestResult.proportion_ci</c> does. A one-sided test's interval is half-open at
    /// <c>0</c> or <c>1</c> — the proportion's own limits, so there is no wider bound to give —
    /// and so is a two-sided interval when every trial succeeded or none did.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <param name="method">Which interval to compute.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>, or <paramref name="method"/> is
    /// not one of the three.
    /// </exception>
    public (double Low, double High) ProportionConfidenceInterval(
        double level = 0.95, ProportionInterval method = ProportionInterval.Exact)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // Refused before the answer at no trial, which would otherwise never read it, as scipy's refuses it there.
        Internal.Options.Require(method);

        // No trial bounds no proportion: NaN, but for the limit a one-sided interval fixes, as scipy's keeps it.
        if (Trials == 0)
        {
            return Alternative switch
            {
                Alternative.Less => (0.0, double.NaN),
                Alternative.Greater => (double.NaN, 1.0),
                _ => (double.NaN, double.NaN),
            };
        }

        return method switch
        {
            ProportionInterval.Exact => ClopperPearson(level),
            ProportionInterval.Wilson => Internal.WilsonInterval.Bounds(this, level, corrected: false),
            ProportionInterval.WilsonCorrected => Internal.WilsonInterval.Bounds(this, level, corrected: true),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        };
    }

    /// <summary>The exact interval, inverted from the binomial tails through the beta they are.</summary>
    /// <remarks>
    /// A one-sided interval spends its whole error budget on one side, so its tail is 1 − level rather than half of it,
    /// and the other bound is the proportion's own limit. Each bound's probability is passed with its complement, the smaller
    /// exact, and the bound solved from it past one half: <c>1 − level</c> rounded to one below 2^-53, and read (0, 0) at 1e-20.
    /// </remarks>
    private (double Low, double High) ClopperPearson(double level)
    {
        bool twoSided = Alternative == Alternative.TwoSided;
        double outside = twoSided ? (1.0 - level) / 2.0 : 1.0 - level;
        // The low bound's beta probability is what lies outside below it, the high bound's what lies inside below it.
        double inside = twoSided ? 1.0 - outside : level;
        double low = Successes == 0 || Alternative == Alternative.Less
            ? 0.0
            : BetaAt(outside, inside, Successes, Trials - Successes + 1.0);
        double high = Successes == Trials || Alternative == Alternative.Greater
            ? 1.0
            : BetaAt(inside, outside, Successes + 1.0, Trials - (double)Successes);

        return (low, high);
    }

    /// <summary>The x with <c>I_x(a, b) = p</c>, from <paramref name="p"/> itself up to one half, past it from both tails:
    /// the inversion reflects there and answers one less a root near one, which kept a few digits of a small bound,
    /// 6e-9 of 9.9e-9 at no success in 1e9 trials, at 0.95 as at 0.9999.</summary>
    private static double BetaAt(double p, double complement, double a, double b)
    {
        return p > 0.5
            ? Internal.BetaQuantile.InvertBoth(p, complement, a, b).X
            : Internal.BetaQuantile.Invert(p, a, b);
    }
}

