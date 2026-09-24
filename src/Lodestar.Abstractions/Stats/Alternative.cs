namespace Lodestar.Stats;

/// <summary>Which tail of the null distribution a test's p-value covers.</summary>
/// <remarks>
/// scipy spells this <c>alternative</c> and defaults it to <c>'two-sided'</c>
/// everywhere. It is not a presentation choice: a one-sided p-value is a
/// different number, not half of the two-sided one, once the null distribution
/// is asymmetric or discrete.
/// </remarks>
public enum Alternative
{
    /// <summary>The samples differ, in either direction. scipy's <c>'two-sided'</c>.</summary>
    TwoSided,

    /// <summary>The first sample's distribution is shifted below the second's. scipy's <c>'less'</c>.</summary>
    Less,

    /// <summary>The first sample's distribution is shifted above the second's. scipy's <c>'greater'</c>.</summary>
    Greater,
}
