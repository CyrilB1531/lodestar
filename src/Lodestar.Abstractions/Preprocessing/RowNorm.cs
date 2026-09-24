namespace Lodestar.Preprocessing;

/// <summary>Which norm <c>Normalizer</c> scales each row to one by.</summary>
/// <remarks>
/// scikit-learn spells this <c>norm</c> on <c>Normalizer</c> and defaults it to <c>'l2'</c>. Its
/// own enum rather than <c>Lodestar.Abstractions</c>' <c>SparseNorm</c>, which carries
/// <c>L1</c> and <c>L2</c> and no maximum: adding a member there would be a release of that
/// package ahead of this one (CONTRIBUTING.md's <em>Working across two packages</em>), and
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
