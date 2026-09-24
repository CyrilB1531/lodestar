namespace Lodestar.Stats;

/// <summary>Which normalisation Kendall's tau divides the concordance excess by.</summary>
/// <remarks>
/// scipy spells this <c>variant</c> and defaults it to <c>'b'</c>. The two answer the same
/// question and share a p-value — scipy's own words, and replayed into the corpus: they
/// "differ only in how they are normalized to lie within the range -1 to 1; the hypothesis
/// tests (their p-values) are identical". Kendall's original tau-a has no member of its own
/// here for the reason scipy gives it none: both variants reduce to it when nothing is tied.
/// </remarks>
public enum KendallVariant
{
    /// <summary>Divide by the tied-pair counts of each sample separately. scipy's <c>'b'</c>.</summary>
    TauB,

    /// <summary>Stuart's tau-c, scaled by the smaller number of distinct values. scipy's <c>'c'</c>.</summary>
    TauC,
}
