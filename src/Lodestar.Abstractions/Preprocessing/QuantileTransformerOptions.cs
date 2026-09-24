namespace Lodestar.Preprocessing;

/// <summary>How many quantiles <c>QuantileTransformer</c> fits, and what it maps onto.</summary>
/// <remarks>
/// scikit-learn's <c>n_quantiles</c> and <c>output_distribution</c>, with the same defaults.
/// <strong><c>subsample</c> is deliberately absent</strong>: at its own default of 10,000 the
/// reference draws a subsample from numpy's generator, and two seeds were measured moving a
/// quantile by <c>0.19</c> on twenty thousand rows. This transformer always reads every row,
/// which is the reference's <c>subsample=None</c> — a narrower parameter set that is provable,
/// rather than a wider one that is not.
/// </remarks>
public sealed record QuantileTransformerOptions
{
    /// <summary>
    /// How many quantiles to fit; scikit-learn's <c>n_quantiles</c>, default 1000, and clamped to
    /// the number of rows on both sides.
    /// </summary>
    public int QuantileCount { get; init; } = 1000;

    /// <summary>What the values are mapped onto; scikit-learn's <c>output_distribution</c>.</summary>
    public QuantileOutput Output { get; init; } = QuantileOutput.Uniform;
}
