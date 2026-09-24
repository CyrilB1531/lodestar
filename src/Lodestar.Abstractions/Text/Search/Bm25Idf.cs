namespace Lodestar.Text.Search;

/// <summary>Which inverse document frequency a <c>Bm25Index</c> weights terms by.</summary>
/// <remarks>
/// The three published forms are different numbers, not different spellings of one, so the
/// choice is stated rather than inherited. A term in most of the corpus is where they part.
/// </remarks>
public enum Bm25Idf
{
    /// <summary>
    /// Robertson's <c>log((N - n + 0.5) / (n + 0.5))</c>, with negatives floored.
    /// </summary>
    /// <remarks>
    /// The raw form goes <strong>negative</strong> once a term appears in more than half the
    /// corpus, which would let a match subtract from a score. `rank_bm25` replaces every
    /// negative with <c>Epsilon × (mean of the raw values)</c>, and this reproduces that —
    /// it is the reference this package is checked against, and the floor is the single
    /// place it departs from a plain reading of Robertson and Zaragoza.
    /// </remarks>
    RobertsonFloored,

    /// <summary>Lucene's <c>log(1 + (N - n + 0.5) / (n + 0.5))</c>.</summary>
    /// <remarks>
    /// Non-negative by construction, so it needs no floor and no epsilon. Different numbers
    /// from <see cref="RobertsonFloored"/>, not a rescaling of them: the ordering of two
    /// documents can differ when a query mixes a common and a rare term.
    /// </remarks>
    Lucene,
}
