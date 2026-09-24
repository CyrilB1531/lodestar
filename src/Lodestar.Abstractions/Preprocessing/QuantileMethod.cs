namespace Lodestar.Preprocessing;

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
