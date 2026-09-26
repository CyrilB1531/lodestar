namespace Lodestar.Survival;

/// <summary>The restricted mean survival time of a curve, with its variance.</summary>
/// <param name="Mean">The area under the survival curve up to the horizon; lifelines' RMST.</param>
/// <param name="Variance">The restricted second moment less the squared mean, as lifelines defines it.</param>
public sealed record RestrictedMeanResult(double Mean, double Variance);
