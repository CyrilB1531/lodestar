namespace Lodestar.Preprocessing;

/// <summary>Which steps <see cref="RobustScaler"/> applies, and between which percentiles it scales.</summary>
/// <remarks>
/// <c>sklearn.preprocessing.RobustScaler</c>'s <c>with_centering</c>, <c>with_scaling</c> and
/// <c>quantile_range</c>, same defaults. Unlike <see cref="StandardScalerOptions"/>, each switch decides
/// exactly one statistic: turning centring off leaves <see cref="RobustScaler.Centre"/> null, and nothing
/// else moves.
/// </remarks>
public sealed record RobustScalerOptions
{
    /// <summary>Whether to subtract each feature's median.</summary>
    public bool WithCentring { get; init; } = true;

    /// <summary>Whether to divide each feature by its interpercentile range.</summary>
    public bool WithScaling { get; init; } = true;

    /// <summary>The bottom percentile of the range scaled by, in <c>[0, 100]</c>.</summary>
    public double LowerPercentile { get; init; } = 25.0;

    /// <summary>The top percentile of the range scaled by, at least <see cref="LowerPercentile"/> and at most 100.</summary>
    public double UpperPercentile { get; init; } = 75.0;

    /// <summary>Whether to scale the range so that normally distributed data comes out with unit variance.</summary>
    /// <remarks>
    /// <c>unit_variance</c>, off by default as it is in the reference. It divides the range by
    /// <c>Φ⁻¹(upper/100) − Φ⁻¹(lower/100)</c> — 1.3489795 at the quartiles — so that a normal column
    /// scales to a standard deviation of 1 rather than to an interquartile range of 1.
    /// </remarks>
    public bool UnitVariance { get; init; }
}
