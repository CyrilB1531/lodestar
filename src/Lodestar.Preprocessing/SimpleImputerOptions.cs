namespace Lodestar.Preprocessing;

/// <summary>How <see cref="SimpleImputer"/> fills a missing value, and what to do with an empty feature.</summary>
/// <remarks><c>sklearn.impute.SimpleImputer</c>'s <c>strategy</c>, <c>fill_value</c> and <c>keep_empty_features</c>.</remarks>
public sealed record SimpleImputerOptions
{
    /// <summary>Which statistic to fill with; the mean by default, as the reference's is.</summary>
    public ImputationStrategy Strategy { get; init; }

    /// <summary>What <see cref="ImputationStrategy.Constant"/> fills with; zero by default, the reference's own.</summary>
    public double FillValue { get; init; }

    /// <summary>
    /// Whether a feature with no value at all is kept, rather than refused: filled with zero, or with
    /// <see cref="FillValue"/> under <see cref="ImputationStrategy.Constant"/>.
    /// </summary>
    /// <remarks>
    /// The reference's <c>keep_empty_features=True</c>. Its <c>False</c> — the default there —
    /// <strong>drops the feature from the output</strong>, which this package refuses instead: a
    /// transform whose output width depends on the fitted data rather than on the input shape is a
    /// trap, and the refusal names the feature and this option.
    /// </remarks>
    public bool KeepEmptyFeatures { get; init; }
}
