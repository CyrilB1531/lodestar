namespace Lodestar.Survival;

/// <summary>One step of a survival or cumulative-hazard curve.</summary>
/// <param name="Time">The duration at which the step sits.</param>
/// <param name="AtRisk">How many subjects were still at risk immediately before it.</param>
/// <param name="Events">How many events were observed at it.</param>
/// <param name="Censored">How many subjects were censored at it.</param>
/// <remarks>
/// A step exists for every distinct duration in the sample, censorings included, so a
/// time that removes subjects without moving the estimate is still visible. The first
/// step is always time zero, where nothing has happened yet — the shape lifelines' own
/// event table has.
/// </remarks>
public sealed record SurvivalStep(double Time, int AtRisk, int Events, int Censored);

/// <summary>A Kaplan-Meier survival curve with its Greenwood variance and interval.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="Survival">The survival estimate at each step.</param>
/// <param name="Lower">The lower confidence bound at each step.</param>
/// <param name="Upper">The upper confidence bound at each step.</param>
/// <param name="ConfidenceLevel">The level the bounds were built at.</param>
/// <remarks>
/// The four arrays share one index with <paramref name="Steps"/>. Bounds are built on the
/// <strong>log-log transform</strong> of the estimate, which is what lifelines reports by
/// default and is not the same as the estimate plus or minus its own standard error — the
/// reference page has the two numbers side by side.
/// </remarks>
#pragma warning disable CA1819
public sealed record KaplanMeierCurve(
    SurvivalStep[] Steps,
    double[] Survival,
    double[] Lower,
    double[] Upper,
    double ConfidenceLevel)
{
    /// <summary>Compares the steps and all three curves, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare the four arrays by reference, so two curves fitted
    /// from the same data would be unequal. Decision 0113 has the rule.
    /// </remarks>
    public bool Equals(KaplanMeierCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        // S1244: the level is a stored configuration, compared by bits so NaN stays reflexive.
#pragma warning disable S1244
        if (other is null || !ConfidenceLevel.Equals(other.ConfidenceLevel))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(Survival, other.Survival)
            && ValueEquality.Same(Lower, other.Lower)
            && ValueEquality.Same(Upper, other.Upper);
    }

    /// <summary>Hashes the level and the step count, which is O(1).</summary>
    /// <remarks>
    /// The four arrays share one index, so the step count stands for all of them. A curve can
    /// hold thousands of steps, which is the walk this avoids.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ConfidenceLevel.GetHashCode();
            return (hash * 31) + ValueEquality.CountOf(Steps);
        }
    }
}

/// <summary>A Nelson-Aalen cumulative-hazard curve.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="CumulativeHazard">The cumulative hazard at each step.</param>
/// <remarks>
/// The hazard accumulates <c>d / n</c> at each step rather than multiplying survival
/// fractions, so it keeps rising where a Kaplan-Meier curve that has reached zero can
/// no longer move.
/// </remarks>
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard)
{
    /// <summary>Compares the steps and the hazard, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare both arrays by reference, so two curves fitted from
    /// the same data would be unequal. Decision 0113 has the rule.
    /// </remarks>
    public bool Equals(NelsonAalenCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return other is not null
            && ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(CumulativeHazard, other.CumulativeHazard);
    }

    /// <summary>Hashes the step count, which is O(1).</summary>
    /// <remarks>
    /// Both arrays share one index, so the step count stands for both. Equal curves agree on it;
    /// unequal ones are allowed to collide.
    /// </remarks>
    public override int GetHashCode() => ValueEquality.CountOf(Steps);
}
#pragma warning restore CA1819

/// <summary>The outcome of a two-sample log-rank test.</summary>
/// <param name="Statistic">The log-rank statistic.</param>
/// <param name="PValue">Its upper-tail chi-squared p-value.</param>
/// <param name="DegreesOfFreedom">One, for a two-sample comparison.</param>
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom);
