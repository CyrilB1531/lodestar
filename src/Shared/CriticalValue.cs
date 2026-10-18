using Lodestar.Stats;

namespace Lodestar.Internal;

/// <summary>The two-sided normal and Student multipliers of a confidence interval at a level, from <c>(1 − level)/2</c>,
/// exact from a level of one half: <c>1 − (1 − level)/2</c> rounds, and read +∞ at a level of 1 − 2^-53 (#1729's Review A).
/// A one-sided one is the level's own quantile, as main took it.</summary>
internal static class CriticalValue
{
    /// <summary>The two-sided normal multiplier, the upper quantile of <c>(1 − level)/2</c>.</summary>
    internal static double TwoSided(double level) => Distributions.NormalIsf((1.0 - level) / 2.0);

    /// <summary>The two-sided Student multiplier at <paramref name="df"/>, the upper quantile of <c>(1 − level)/2</c>.</summary>
    internal static double TwoSidedStudent(double level, double df) => Distributions.StudentIsf((1.0 - level) / 2.0, df);
}
