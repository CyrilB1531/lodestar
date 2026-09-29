namespace Lodestar.Embeddings.Persistence;

/// <summary>A float block read from a <c>.npy</c> file, with the shape it was stored under.</summary>
/// <remarks>
/// <see cref="Values"/> and <see cref="Shape"/> are taken and exposed as they are, not copied: writing to the
/// caller's memory or list changes this block and what it equals, and every holder of it, a <c>with</c> copy
/// included (#1305).
/// </remarks>
/// <param name="Values">The elements, in C order.</param>
/// <param name="Shape">The dimensions; one entry for a vector, two for a matrix.</param>
public readonly record struct NpyBlock(ReadOnlyMemory<float> Values, IReadOnlyList<int> Shape)
{
    /// <summary>The array this block owns, or <see langword="null"/> when it borrows.</summary>
    /// <remarks>
    /// A reader sets it to the array <see cref="Values"/> covers when it allocated that array itself: the stream and
    /// path reads, and a read of bytes on a big-endian host. A little-endian read of bytes borrows them and leaves it
    /// null. Nothing here reads it; it is the array a caller may hand to <c>EmbeddingIndex.FromOwnedBlock</c>.
    /// It is a public <c>init</c>: a caller may set it to any array, which the block then neither checks nor uses,
    /// and a <c>with</c> copy that replaces <see cref="Values"/> keeps the old one, no longer the block's elements.
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
    public bool Equals(NpyBlock other) =>
        ValueEquality.Same(Shape, other.Shape) && Values.Span.SequenceEqual(other.Values.Span);

    /// <summary>Hashes the element and dimension counts, which is O(1) and consistent with equality.</summary>
    public override int GetHashCode() => unchecked((Values.Length * 31) + (Shape?.Count ?? -1));
}
