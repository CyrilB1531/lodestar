namespace Lodestar.Preprocessing;

/// <summary>Where <c>KBinsDiscretizer</c> places its bin edges.</summary>
public enum BinStrategy
{
    /// <summary>Equal widths between the feature's smallest and largest value. scikit-learn's <c>'uniform'</c>.</summary>
    Uniform,

    /// <summary>Equal counts, read off the feature's percentiles. scikit-learn's <c>'quantile'</c>, and the default.</summary>
    Quantile,

    /// <summary>Midway between one-dimensional k-means centres. scikit-learn's <c>'kmeans'</c>.</summary>
    KMeans,
}
