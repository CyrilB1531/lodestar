using Lodestar.Abstractions;

namespace Lodestar.Text.Vectorization;

/// <summary>Options for <c>HashingVectorizer</c>.</summary>
/// <remarks>Defaults mirror <c>sklearn.feature_extraction.text.HashingVectorizer</c>.</remarks>
public sealed record HashingVectorizerOptions
{
    /// <summary>Tokenization / analysis options (vocabulary-related fields are ignored — hashing is stateless).</summary>
    public CountVectorizerOptions Count { get; init; } = new();

    /// <summary>Number of hash buckets (feature dimensions). Default 2^20.</summary>
    public int NumFeatures { get; init; } = 1 << 20;

    /// <summary>Add a sign from the hash so collisions can cancel (reduces bias). Default <c>true</c>.</summary>
    public bool AlternateSign { get; init; } = true;

    /// <summary>Row normalization, or <c>null</c> for none. Default <see cref="SparseNorm.L2"/>.</summary>
    public SparseNorm? Norm { get; init; } = SparseNorm.L2;
}
