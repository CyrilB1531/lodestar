namespace Lodestar.Stats;

/// <summary>Whether a discrete statistic's normal approximation gets the half-unit correction.</summary>
/// <remarks>
/// One idea, three spellings in scipy: <c>use_continuity</c> on
/// <c>mannwhitneyu</c> (default true), <c>correction</c> on <c>wilcoxon</c>
/// (default false) and <c>correction</c> on <c>chi2_contingency</c> (default
/// true, and applied to 2x2 tables only). The three defaults disagree, which is
/// exactly why this is a named argument here rather than a bool nobody reads.
/// </remarks>
public enum Continuity
{
    /// <summary>Shift the statistic half a unit toward the mean before the normal tail.</summary>
    Applied,

    /// <summary>Take the statistic as it stands.</summary>
    None,
}
