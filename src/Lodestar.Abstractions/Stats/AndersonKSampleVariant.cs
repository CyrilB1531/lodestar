namespace Lodestar.Stats;

/// <summary>Which form of the k-sample Anderson-Darling statistic <c>AndersonDarling.KSample</c> computes.</summary>
/// <remarks>
/// scipy 1.18's <c>variant</c> of <c>anderson_ksamp</c>, which replaces its <c>midrank</c> flag: <see cref="Midrank"/>
/// is <c>midrank=True</c>, the default there and here, and <see cref="Right"/> is <c>midrank=False</c>.
/// </remarks>
public enum AndersonKSampleVariant
{
    /// <summary>Scholz and Stephens' equation 7, with mid-ranks for ties: for continuous and discrete samples alike. The default.</summary>
    Midrank,

    /// <summary>Their equation 6, the right-continuous empirical distribution, for discrete samples.</summary>
    Right,

    /// <summary>Their equation 3, which assumes no ties, for continuous samples.</summary>
    Continuous,
}
