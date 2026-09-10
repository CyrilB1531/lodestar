namespace Lodestar.Text.Search;

/// <summary>Which inverse document frequency a <see cref="Bm25Index"/> weights terms by.</summary>
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

/// <summary>How a <see cref="Bm25Index"/> saturates term frequency and normalizes length.</summary>
/// <param name="K1">Term-frequency saturation. Higher lets repetition keep paying; 0 makes a
/// term's presence binary. Must be non-negative.</param>
/// <param name="B">Length normalization, in <c>[0, 1]</c>. 0 ignores document length entirely;
/// 1 divides fully by it.</param>
/// <param name="Idf">Which inverse document frequency to weight by.</param>
/// <param name="Epsilon">The floor multiplier <see cref="Bm25Idf.RobertsonFloored"/> applies to
/// a negative IDF, as a fraction of the mean raw IDF. Ignored by
/// <see cref="Bm25Idf.Lucene"/>.</param>
/// <remarks>
/// The defaults are <c>rank_bm25</c>'s, so the documented default is the tested one.
/// <strong>K1 is 1.5, not the 1.2 Lucene defaults to</strong> — a caller porting numbers from
/// a Lucene index has to say so.
/// </remarks>
public sealed record Bm25Options(
    double K1 = 1.5,
    double B = 0.75,
    Bm25Idf Idf = Bm25Idf.RobertsonFloored,
    double Epsilon = 0.25);
