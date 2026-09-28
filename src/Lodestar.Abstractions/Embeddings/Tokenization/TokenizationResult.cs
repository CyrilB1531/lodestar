namespace Lodestar.Embeddings.Tokenization;

/// <summary>The result of tokenizing a piece of text: the sub-word tokens and their vocabulary ids.</summary>
public sealed record TokenizationResult(IReadOnlyList<string> Tokens, IReadOnlyList<int> Ids)
{
    /// <summary>Compares the tokens and ids element by element.</summary>
    /// <remarks>
    /// The generated equality would compare <see cref="Tokens"/> and <see cref="Ids"/>
    /// by reference, so two results holding the same tokens would be unequal — in
    /// the one place a caller has every reason to compare: asserting an encoding
    /// against the result written out by hand.
    /// </remarks>
    public bool Equals(TokenizationResult? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return other is not null
            && ValueEquality.Same(Tokens, other.Tokens)
            && ValueEquality.Same(Ids, other.Ids);
    }

    /// <summary>Hashes the lengths only, which is O(1) and still consistent with equality.</summary>
    /// <remarks>
    /// Equal results necessarily agree on both counts; unequal ones are allowed to
    /// share a hash. Hashing every token would make the cheap operation the
    /// expensive one on a long encoding.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            return (((17 * 31) + ValueEquality.LengthOf(Tokens)) * 31) + ValueEquality.LengthOf(Ids);
        }
    }
}
