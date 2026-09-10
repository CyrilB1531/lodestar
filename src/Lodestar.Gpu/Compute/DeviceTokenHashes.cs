using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>One document's token hashes per row, held flat on the accelerator.</summary>
/// <remarks>
/// Hashes rather than tokens: a kernel parameter must be blittable, and taking the hashes keeps
/// this package free of an edge in either direction (decisions 0101 and 0103).
/// <strong>The hash has to be the one the CPU path uses, or the signatures will not match</strong>
/// — <c>Lodestar.Text.Similarity.MinHash</c> takes the first four bytes of a token's SHA-1,
/// little-endian, which is what <c>datasketch</c> exports as <c>sha1_hash32</c>. Anything else is
/// a different sketch: a legitimate thing to want, and not parity.
/// </remarks>
public sealed class DeviceTokenHashes : IDisposable
{
    internal MemoryBuffer1D<uint, Stride1D.Dense> Hashes { get; }

    internal MemoryBuffer1D<int, Stride1D.Dense> Offsets { get; }

    /// <summary>How many documents the block holds.</summary>
    public int Count { get; }

    private DeviceTokenHashes(
        MemoryBuffer1D<uint, Stride1D.Dense> hashes,
        MemoryBuffer1D<int, Stride1D.Dense> offsets,
        int count)
    {
        Hashes = hashes;
        Offsets = offsets;
        Count = count;
    }

    /// <summary>Flattens a batch of documents and uploads it.</summary>
    /// <param name="context">The accelerator to upload to.</param>
    /// <param name="documents">
    /// One sequence of token hashes per document. A document may be empty, and repeats are
    /// harmless: a minimum is idempotent, which is what makes this a set sketch.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/>, <paramref name="documents"/>, or one of them, is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> is empty.</exception>
    public static DeviceTokenHashes Upload(
        GpuContext context, IReadOnlyList<IReadOnlyList<uint>> documents)
    {
        Guard.NotNull(context);
        Guard.NotNull(documents);
        if (documents.Count == 0)
        {
            throw new ArgumentException("A batch holds at least one document.", nameof(documents));
        }

        int total = 0;
        foreach (IReadOnlyList<uint> document in documents)
        {
            Guard.NotNull(document);
            total += document.Count;
        }

        uint[] flat = new uint[total];
        int[] offsets = new int[documents.Count + 1];
        int at = 0;
        for (int row = 0; row < documents.Count; row++)
        {
            offsets[row] = at;
            foreach (uint hash in documents[row])
            {
                flat[at++] = hash;
            }
        }

        offsets[documents.Count] = at;
        Accelerator accelerator = context.Accelerator;
        return new DeviceTokenHashes(Upload(accelerator, flat), accelerator.Allocate1D(offsets),
            documents.Count);
    }

    /// <summary>Allocates a buffer for a host array, tolerating an empty one.</summary>
    /// <remarks>
    /// Measured: ILGPU's array overload throws a <see cref="NullReferenceException"/> on a
    /// zero-length array, while the length overload returns an empty buffer. A batch of nothing
    /// but empty documents reaches the first, so the two cases are separated here rather than
    /// papered over with a one-element array nothing reads.
    /// </remarks>
    private static MemoryBuffer1D<uint, Stride1D.Dense> Upload(Accelerator accelerator, uint[] values) =>
        values.Length == 0 ? accelerator.Allocate1D<uint>(0) : accelerator.Allocate1D(values);

    /// <summary>Frees the two device buffers.</summary>
    public void Dispose()
    {
        Hashes.Dispose();
        Offsets.Dispose();
    }
}
