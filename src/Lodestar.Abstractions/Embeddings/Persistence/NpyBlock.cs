namespace Lodestar.Embeddings.Persistence;

/// <summary>A float block read from a <c>.npy</c> file, with the shape it was stored under.</summary>
/// <param name="Values">The elements, in C order.</param>
/// <param name="Shape">The dimensions; one entry for a vector, two for a matrix.</param>
public readonly record struct NpyBlock(ReadOnlyMemory<float> Values, IReadOnlyList<int> Shape)
{
    /// <summary>The array this block owns, or <see langword="null"/> when it borrows.</summary>
    /// <remarks>
    /// The stream reader fills it, and the path overload through it, because only that
    /// route allocates an array nobody else holds. A block over a caller's bytes leaves
    /// it null, and so does one built by hand — which is what stops the equality
    /// ownership transfer being reached without the method that documents it.
    /// </remarks>
    // CA1819: handing the array out is the contract -- FromOwnedBlock adopts it and the
    // block stops using it, so the defensive copy the rule wants would defeat the feature.
#pragma warning disable CA1819
    public float[]? OwnedArray { get; init; }
#pragma warning restore CA1819

    /// <summary>Whether two blocks hold the same elements under the same shape.</summary>
    /// <remarks>
    /// The generated equality compared <see cref="Values"/> and <see cref="Shape"/> by reference, so
    /// two reads of one file were unequal (#902). Elements compare as
    /// <c>float.Equals</c> does, <c>NaN</c> equal to <c>NaN</c>; who owns the array is not part of
    /// the value, so <see cref="OwnedArray"/> is not compared.
    /// </remarks>
    public bool Equals(NpyBlock other)
    {
        if (Shape is null || other.Shape is null)
        {
            return Shape is null && other.Shape is null && Values.Span.SequenceEqual(other.Values.Span);
        }
        if (Shape.Count != other.Shape.Count || !Values.Span.SequenceEqual(other.Values.Span))
        {
            return false;
        }
        for (int i = 0; i < Shape.Count; i++)
        {
            if (Shape[i] != other.Shape[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Hashes the element and dimension counts, which is O(1) and consistent with equality.</summary>
    public override int GetHashCode() => unchecked((Values.Length * 31) + (Shape?.Count ?? -1));
}
