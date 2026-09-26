namespace Lodestar.Survival;

/// <summary>The outcome of a log-rank test, two groups or more.</summary>
/// <param name="Statistic">The log-rank statistic.</param>
/// <param name="PValue">Its upper-tail chi-squared p-value.</param>
/// <param name="DegreesOfFreedom">One for two groups, and one fewer than the groups compared for more.</param>
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom);
