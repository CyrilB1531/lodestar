namespace Lodestar.Stats.Internal;

/// <summary>The rule the k-sample tests share for a group with nothing in it.</summary>
internal static class Groups
{
    /// <summary>Whether a group is null or empty, given or left by omission: NaN in the k-sample tests that read it.</summary>
    /// <remarks>
    /// scipy's kruskal, levene, fligner and f_oneway answer NaN for an empty group under every nan_policy, and a null one has
    /// no scipy counterpart; four of the five threw while Fligner answered NaN (#1712's Review A). Bartlett keeps its own,
    /// wider rule, a group of one value included.
    /// </remarks>
    internal static bool AnyEmpty(double[][] groups) => Array.Exists(groups, group => group is not { Length: > 0 });
}
