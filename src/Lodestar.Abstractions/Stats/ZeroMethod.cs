namespace Lodestar.Stats;

/// <summary>How the Wilcoxon signed-rank test treats pairs whose difference is zero.</summary>
/// <remarks>
/// scipy's <c>zero_method</c>, default <c>'wilcox'</c>. The three rules give
/// three different statistics on the same data, so this is part of the test's
/// definition rather than a tuning knob.
/// </remarks>
public enum ZeroMethod
{
    /// <summary>Discard the zero-difference pairs before ranking. scipy's <c>'wilcox'</c>.</summary>
    Wilcox,

    /// <summary>Rank the zeros, then drop their ranks from the sums. scipy's <c>'pratt'</c>.</summary>
    Pratt,

    /// <summary>Rank the zeros and split their ranks evenly between the two sums. scipy's <c>'zsplit'</c>.</summary>
    ZSplit,
}
