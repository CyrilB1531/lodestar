namespace Lodestar.Embeddings.Tokenization;

/// <summary>What to do with a sequence longer than the model accepts.</summary>
/// <remarks>
/// The names follow HuggingFace's <c>truncation</c> argument. Lodestar encodes one
/// sequence at a time (never a pair), so <see cref="LongestFirst"/> and
/// <see cref="Right"/> both drop tokens from the end — the distinction only bites
/// on a pair, which this pipeline does not build.
/// </remarks>
public enum TruncationStrategy
{
    /// <summary>Refuse: a sequence over the limit raises <see cref="ArgumentException"/>.</summary>
    /// <remarks>
    /// Matches <c>truncation=False</c>. Silently dropping the tail of a document is
    /// a decision, so this makes it possible to insist on being told instead.
    /// </remarks>
    None = 0,

    /// <summary>Drop tokens from the end until the sequence fits. Matches <c>truncation="longest_first"</c>.</summary>
    LongestFirst = 1,

    /// <summary>Drop tokens from the end until the sequence fits. Matches <c>truncation="only_first"</c>.</summary>
    Right = 2,
}
