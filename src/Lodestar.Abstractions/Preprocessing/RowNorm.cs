namespace Lodestar.Preprocessing;

/// <summary>Which norm <c>Normalizer</c> scales each row to one by.</summary>
/// <remarks>
/// scikit-learn spells this <c>norm</c> on <c>Normalizer</c> and defaults it to <c>'l2'</c>. Its
/// own enum rather than <c>Lodestar.Abstractions</c>' <c>SparseNorm</c>, on purpose (#1232):
/// <c>SparseNorm</c> is what the tf-idf and hashing vectorizers take, and scikit-learn refuses
/// <c>'max'</c> there, so a <c>Max</c> member would be a value every one of them rejects; and
/// <c>CsrMatrix.NormalizeRows</c> mutates in place where every member here leaves its input alone.
/// </remarks>
public enum RowNorm
{
    /// <summary>Sum of absolute values. scikit-learn's <c>'l1'</c>.</summary>
    L1,

    /// <summary>Euclidean length. scikit-learn's <c>'l2'</c>, and the default on both sides.</summary>
    L2,

    /// <summary>Largest absolute value. scikit-learn's <c>'max'</c>.</summary>
    Max,
}
