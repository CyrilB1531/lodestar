namespace Lodestar.Text.Search;

/// <summary>How a <c>Bm25Index</c> saturates term frequency and normalizes length.</summary>
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
