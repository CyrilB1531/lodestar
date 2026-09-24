namespace Lodestar.Metrics;

/// <summary>
/// How <c>ConfusionMatrix.ToArray</c> scales the cells —
/// the equivalent of <c>normalize=</c> on <c>sklearn.metrics.confusion_matrix</c>.
/// </summary>
/// <remarks>
/// A projection, not a state: a <c>ConfusionMatrix</c> is never normalized, and
/// <c>ConfusionMatrix.ToArray</c> is what scales its cells.
/// </remarks>
public enum Normalization
{
    /// <summary>Raw counts, or weights when the matrix is weighted (<c>normalize=None</c>).</summary>
    None,

    /// <summary>Each row divided by its own sum: recall per true class (<c>normalize="true"</c>).</summary>
    True,

    /// <summary>Each column divided by its own sum: precision per predicted class (<c>normalize="pred"</c>).</summary>
    Pred,

    /// <summary>Every cell divided by the total (<c>normalize="all"</c>).</summary>
    All,
}
