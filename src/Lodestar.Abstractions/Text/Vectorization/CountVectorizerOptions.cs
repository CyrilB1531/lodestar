namespace Lodestar.Text.Vectorization;

/// <summary>Configuration for <c>CountVectorizer</c> (and, via composition, TF-IDF).</summary>
/// <remarks>Defaults mirror <c>sklearn.feature_extraction.text.CountVectorizer</c>.</remarks>
public sealed record CountVectorizerOptions
{
    /// <summary>Lowercase documents before tokenizing. Default <c>true</c>.</summary>
    public bool Lowercase { get; init; } = true;

    /// <summary>Strip accents via NFKD decomposition (like sklearn <c>strip_accents="unicode"</c>). Default <c>false</c>.</summary>
    public bool StripAccents { get; init; }

    /// <summary>Which analyzer to use. Default <see cref="AnalyzerKind.Word"/>.</summary>
    public AnalyzerKind Analyzer { get; init; } = AnalyzerKind.Word;

    /// <summary>Inclusive (min, max) n-gram sizes. Default <c>(1, 1)</c>.</summary>
    public (int Min, int Max) NgramRange { get; init; } = (1, 1);

    /// <summary>
    /// Minimum document frequency. A value in <c>(0, 1)</c> is a proportion of documents;
    /// a value <c>&gt;= 1</c> is an absolute count. Default <c>1</c>.
    /// </summary>
    public double MinDf { get; init; } = 1.0;

    /// <summary>
    /// Maximum document frequency. A value in <c>[0, 1]</c> is a proportion; a value
    /// <c>&gt; 1</c> is an absolute count. Default <c>1.0</c> (all documents).
    /// </summary>
    public double MaxDf { get; init; } = 1.0;

    /// <summary>If true, all non-zero counts are set to 1. Default <c>false</c>.</summary>
    public bool Binary { get; init; }

    /// <summary>Stop words to remove after tokenizing (word analyzer only). Default none.</summary>
    public IReadOnlyCollection<string>? StopWords { get; init; }

    /// <summary>Regex selecting word tokens. Default matches runs of two or more word characters.</summary>
    public string TokenPattern { get; init; } = @"\b\w\w+\b";

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated equality
    /// would otherwise treat two lists of the same words as unequal. <see cref="TfidfVectorizerOptions"/>
    /// embeds this record, so it inherits the fix.
    /// </remarks>
    public bool Equals(CountVectorizerOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Lowercase != other.Lowercase
            || StripAccents != other.StripAccents
            || Analyzer != other.Analyzer
            || NgramRange != other.NgramRange
            // SonarLint S1244 warns against comparing floating point for exact
            // equality, which is right for arithmetic and wrong here: this is value
            // equality between two configurations, where "the same threshold" means
            // the same bits. double.Equals also treats NaN as equal to NaN, which is
            // what a record's equality needs and what == would get wrong.
#pragma warning disable S1244
            || !MinDf.Equals(other.MinDf)
            || !MaxDf.Equals(other.MaxDf)
#pragma warning restore S1244
            || Binary != other.Binary
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present. Its
    /// <em>count</em> cannot be used: <see cref="Equals(CountVectorizerOptions)"/>
    /// compares as a set, so <c>["the", "the"]</c> equals <c>["the"]</c> while the
    /// counts differ — and equal objects are required to hash alike. Hashing the
    /// words themselves would mean hashing all of them, order-independently, which
    /// is the O(n) this exists to avoid. Unequal options are allowed to collide.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + (Lowercase ? 1 : 0);
            hash = (hash * 31) + (StripAccents ? 1 : 0);
            hash = (hash * 31) + (int)Analyzer;
            hash = (hash * 31) + NgramRange.GetHashCode();
            hash = (hash * 31) + MinDf.GetHashCode();
            hash = (hash * 31) + MaxDf.GetHashCode();
            hash = (hash * 31) + (Binary ? 1 : 0);
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
}
