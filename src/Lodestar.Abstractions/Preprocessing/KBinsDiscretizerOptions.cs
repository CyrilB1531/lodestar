namespace Lodestar.Preprocessing;

/// <summary>How <c>KBinsDiscretizer</c> places its bins, and what it emits.</summary>
public sealed record KBinsDiscretizerOptions
{
    /// <summary>How many bins each feature is cut into; scikit-learn's <c>n_bins</c>, default 5.</summary>
    public int BinCount { get; init; } = 5;

    /// <summary>Where the edges are placed; scikit-learn's <c>strategy</c>, default <see cref="BinStrategy.Quantile"/>.</summary>
    public BinStrategy Strategy { get; init; } = BinStrategy.Quantile;

    /// <summary>What a transformed row carries; scikit-learn's <c>encode</c>, default <see cref="BinEncoding.OneHot"/>.</summary>
    public BinEncoding Encoding { get; init; } = BinEncoding.OneHot;

    /// <summary>
    /// Which percentile convention <see cref="BinStrategy.Quantile"/> reads; scikit-learn's
    /// <c>quantile_method</c>, default <see cref="QuantileMethod.AveragedInvertedCdf"/> since 1.9.
    /// </summary>
    /// <remarks>
    /// It changes the edges, not merely how they are reached: over <c>1..6</c> cut into three,
    /// this default gives <c>2.5</c> and <c>4.5</c> where <see cref="QuantileMethod.Linear"/>
    /// gives <c>2.667</c> and <c>4.333</c>.
    /// </remarks>
    public QuantileMethod QuantileMethod { get; init; } = QuantileMethod.AveragedInvertedCdf;
}
