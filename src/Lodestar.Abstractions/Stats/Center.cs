namespace Lodestar.Stats;

/// <summary>Which centre Levene's test measures each group's spread around.</summary>
/// <remarks>
/// scipy spells this <c>center</c> and defaults it to <c>'median'</c>, which is Brown and
/// Forsythe's variant rather than Levene's own — the median is what makes the test hold up on a
/// skewed sample, and it is the default here for the same reason. The choice changes the
/// statistic, not merely how it is reached.
/// </remarks>
public enum Center
{
    /// <summary>The group's median. scipy's <c>'median'</c>, and Brown-Forsythe.</summary>
    Median,

    /// <summary>The group's mean. scipy's <c>'mean'</c>, and Levene's original test.</summary>
    Mean,

    /// <summary>The mean of what is left after trimming each end. scipy's <c>'trimmed'</c>.</summary>
    Trimmed,
}
