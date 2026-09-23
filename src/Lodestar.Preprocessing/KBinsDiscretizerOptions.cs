namespace Lodestar.Preprocessing;

/// <summary>How <see cref="KBinsDiscretizer"/> places its bins, and what it emits.</summary>
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

/// <summary>Where <see cref="KBinsDiscretizer"/> places its bin edges.</summary>
public enum BinStrategy
{
    /// <summary>Equal widths between the feature's smallest and largest value. scikit-learn's <c>'uniform'</c>.</summary>
    Uniform,

    /// <summary>Equal counts, read off the feature's percentiles. scikit-learn's <c>'quantile'</c>, and the default.</summary>
    Quantile,

    /// <summary>Midway between one-dimensional k-means centres. scikit-learn's <c>'kmeans'</c>.</summary>
    KMeans,
}

/// <summary>What a row transformed by <see cref="KBinsDiscretizer"/> carries.</summary>
public enum BinEncoding
{
    /// <summary>One column per feature, holding its bin's index. scikit-learn's <c>'ordinal'</c>.</summary>
    Ordinal,

    /// <summary>One column per bin of each feature, one of them set. scikit-learn's <c>'onehot-dense'</c>, and the default shape.</summary>
    OneHot,
}

/// <summary>Which percentile convention a quantile-strategy fit reads.</summary>
/// <remarks>
/// numpy publishes both under <c>numpy.percentile</c>'s <c>method</c>. They are different
/// definitions of the same word, not different approximations of one answer.
/// </remarks>
public enum QuantileMethod
{
    /// <summary>Linear interpolation between the two neighbouring order statistics; numpy's <c>'linear'</c>.</summary>
    Linear,

    /// <summary>The inverted empirical CDF, averaged where it jumps; numpy's <c>'averaged_inverted_cdf'</c>.</summary>
    AveragedInvertedCdf,
}
