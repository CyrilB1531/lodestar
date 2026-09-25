namespace Lodestar.Stats.Regression;

/// <summary>A test statistic with its p-value and the degrees of freedom it is read against.</summary>
/// <remarks>Shared by the instrumental-variables and panel estimators, whose model, first-stage, poolability and
/// overidentification tests are each a χ² or an F.</remarks>
/// <param name="Statistic">The statistic.</param>
/// <param name="PValue">Its upper-tail probability.</param>
/// <param name="DegreesOfFreedom">The numerator degrees of freedom.</param>
/// <param name="DenominatorDegreesOfFreedom">The denominator's, when the statistic is an F; <see langword="null"/> for a χ².</param>
public sealed record WaldTest(double Statistic, double PValue, int DegreesOfFreedom, int? DenominatorDegreesOfFreedom);
