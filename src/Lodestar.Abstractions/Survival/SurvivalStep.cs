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
public sealed record SurvivalStep(double Time, int AtRisk, int Events, int Censored)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOf(Time);
            hash = (hash * 31) + AtRisk;
            hash = (hash * 31) + Events;
            return (hash * 31) + Censored;
        }
    }
}
