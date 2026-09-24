namespace Lodestar.Text.Vectorization;

/// <summary>Options for <c>TfidfVectorizer</c>: tokenization plus TF-IDF weighting.</summary>
public sealed record TfidfVectorizerOptions
{
    /// <summary>Tokenization / vocabulary options (shared with <c>CountVectorizer</c>).</summary>
    public CountVectorizerOptions Count { get; init; } = new();

    /// <summary>TF-IDF weighting options.</summary>
    public TfidfOptions Tfidf { get; init; } = new();
}
