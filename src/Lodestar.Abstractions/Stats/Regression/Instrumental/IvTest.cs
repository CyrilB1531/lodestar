namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>A test statistic with its p-value and the degrees of freedom it is read against.</summary>
/// <param name="Statistic">The statistic.</param>
/// <param name="PValue">Its upper-tail probability.</param>
/// <param name="DegreesOfFreedom">The numerator degrees of freedom.</param>
/// <param name="DenominatorDegreesOfFreedom">The denominator's, when the statistic is an F; <see langword="null"/> for a χ².</param>
public sealed record IvTest(double Statistic, double PValue, int DegreesOfFreedom, int? DenominatorDegreesOfFreedom);
