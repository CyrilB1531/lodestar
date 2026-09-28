namespace Lodestar.Survival;

/// <summary>The restricted mean survival time of a curve, with its variance.</summary>
/// <param name="Mean">The area under the survival curve up to the horizon; lifelines' RMST.</param>
/// <param name="Variance">The restricted second moment less the squared mean, as lifelines defines it.</param>
public sealed record RestrictedMeanResult(double Mean, double Variance)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOf(Mean);
            return (hash * 31) + ValueEquality.HashOf(Variance);
        }
    }
}
