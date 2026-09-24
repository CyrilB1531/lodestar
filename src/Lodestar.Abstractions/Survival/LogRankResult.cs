namespace Lodestar.Survival;

/// <summary>The outcome of a two-sample log-rank test.</summary>
/// <param name="Statistic">The log-rank statistic.</param>
/// <param name="PValue">Its upper-tail chi-squared p-value.</param>
/// <param name="DegreesOfFreedom">One, for a two-sample comparison.</param>
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom);
