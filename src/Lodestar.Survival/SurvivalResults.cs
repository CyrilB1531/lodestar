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
    double ConfidenceLevel);

/// <summary>A Nelson-Aalen cumulative-hazard curve.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="CumulativeHazard">The cumulative hazard at each step.</param>
/// <remarks>
/// The hazard accumulates <c>d / n</c> at each step rather than multiplying survival
/// fractions, so it keeps rising where a Kaplan-Meier curve that has reached zero can
/// no longer move.
/// </remarks>
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard);
#pragma warning restore CA1819

/// <summary>The outcome of a two-sample log-rank test.</summary>
/// <param name="Statistic">The log-rank statistic.</param>
/// <param name="PValue">Its upper-tail chi-squared p-value.</param>
/// <param name="DegreesOfFreedom">One, for a two-sample comparison.</param>
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom);
