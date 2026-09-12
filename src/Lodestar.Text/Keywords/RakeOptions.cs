namespace Lodestar.Text.Keywords;

/// <summary>How RAKE scores a word before the phrase sums it.</summary>
public enum RakeMetric
{
    /// <summary><c>deg(w) / freq(w)</c>. The paper's, and the reference implementation's default.</summary>
    DegreeToFrequencyRatio,

    /// <summary><c>deg(w)</c>: how many words it shares a candidate with, itself included, counted per occurrence.</summary>
    WordDegree,

    /// <summary><c>freq(w)</c>: how often it occurs at all.</summary>
    WordFrequency,
}

/// <summary>What <see cref="Rake"/> is built with.</summary>
public sealed record RakeOptions
{
    /// <summary>The stop words that delimit candidates. Null takes <c>StopWords.English</c>.</summary>
    public IReadOnlyCollection<string>? StopWords { get; init; }

    /// <summary>Which per-word score the phrase sums.</summary>
    public RakeMetric Metric { get; init; } = RakeMetric.DegreeToFrequencyRatio;

    /// <summary>Shortest candidate kept, in words, inclusive.</summary>
    public int MinLength { get; init; } = 1;

    /// <summary>Longest candidate kept, in words, inclusive.</summary>
    public int MaxLength { get; init; } = 100_000;

    /// <summary>When false, a candidate that occurs twice is reported once.</summary>
    public bool IncludeRepeatedPhrases { get; init; } = true;

    /// <summary>
    /// What counts as a word.
    /// </summary>
    /// <remarks>
    /// <c>\b\w+\b</c>, not the vectorizers' <c>\b\w\w+\b</c>: a one-letter word neighbours a
    /// boundary rather than being a stop word, and dropping it would merge two candidates.
    /// </remarks>
    public string TokenPattern { get; init; } = @"\b\w+\b";

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated
    /// equality would otherwise treat two lists of the same words as unequal. Decision 0113 has
    /// the rule, and <see cref="Lodestar.Text.Vectorization.CountVectorizerOptions"/> the
    /// same member.
    /// </remarks>
    public bool Equals(RakeOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Metric != other.Metric
            || MinLength != other.MinLength
            || MaxLength != other.MaxLength
            || IncludeRepeatedPhrases != other.IncludeRepeatedPhrases
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present. Its <em>count</em> cannot
    /// be used: equality compares as a set, so <c>["the", "the"]</c> equals <c>["the"]</c> while
    /// the counts differ, and equal objects must hash alike.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + (int)Metric;
            hash = (hash * 31) + MinLength;
            hash = (hash * 31) + MaxLength;
            hash = (hash * 31) + (IncludeRepeatedPhrases ? 1 : 0);
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
}
