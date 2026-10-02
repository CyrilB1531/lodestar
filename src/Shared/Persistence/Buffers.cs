using System.Buffers;

namespace Lodestar.Internal.Persistence;

/// <summary>Allocation for the two blocks an artifact load fills whole.</summary>
/// <remarks>
/// The runtime zeroes an array before handing it over, and both of a load's large
/// buffers are overwritten immediately after — the payload by the stream, the vector
/// block by the decoder. At an embedding index's size that is tens of megabytes of
/// large-object-heap writes doing nothing, which #324 measured as the larger half of
/// the load. Only <see langword="struct"/> element types, which is what
/// <c>GC.AllocateUninitializedArray</c> can skip the zeroing for at all.
/// </remarks>
internal static class Buffers
{
    /// <summary>An array whose contents are undefined until the caller writes them.</summary>
    /// <typeparam name="T">The element type, which must hold no references.</typeparam>
    /// <param name="length">How many elements.</param>
    /// <remarks>
    /// <b>Every element the caller then exposes must be written first.</b> A partly
    /// filled buffer has to be sliced to what was written, the way
    /// <see cref="JsonArtifact.ReadAllBytes"/> returns only the bytes the stream gave:
    /// past that, this hands back whatever the heap held, where <c>new T[]</c> handed
    /// back zeroes.
    /// </remarks>
    public static T[] AllocateUninitialized<T>(int length)
        where T : struct =>
#if NET5_0_OR_GREATER
        GC.AllocateUninitializedArray<T>(length);
#else
        new T[length];
#endif

    /// <summary>A payload buffer and the exact bytes that were read into it.</summary>
    /// <remarks>
    /// The rent and the return live together so no call site can hold one without the
    /// other, the way <c>ArtifactIo.SaveWithBlock</c> owns the writer sequence.
    /// <see cref="Memory"/> is sliced to what was read, never to the buffer: a rented array
    /// is at least as long as asked and <b>its tail holds the previous artifact</b>.
    /// A payload that could not be rented against is carried here too, owning nothing, so
    /// the call site has one shape rather than a branch it could get wrong.
    /// </remarks>
    internal readonly struct RentedPayload : IDisposable
    {
        private readonly byte[]? _rented;
        private readonly List<byte[]>? _segments;

        private RentedPayload(byte[]? rented, ReadOnlyMemory<byte> memory)
        {
            _rented = rented;
            Memory = memory;
            Sequence = new ReadOnlySequence<byte>(memory);
        }

        private RentedPayload(List<byte[]> segments, ReadOnlySequence<byte> sequence)
        {
            _segments = segments;
            Sequence = sequence;
        }

        /// <summary>The bytes read, and only those; empty when they are <see cref="IsSegmented"/>.</summary>
        public ReadOnlyMemory<byte> Memory { get; }

        /// <summary>The bytes read as a sequence: one segment, <see cref="Memory"/>, unless <see cref="IsSegmented"/>.</summary>
        public ReadOnlySequence<byte> Sequence { get; }

        /// <summary>Whether the bytes passed one array, and are read through <see cref="Sequence"/> alone (#1618).</summary>
        public bool IsSegmented => _segments is not null;

        /// <summary>Takes ownership of <paramref name="segments"/>, every one full but the last, which holds <paramref name="inLast"/> bytes.</summary>
        public static RentedPayload RentedSegments(List<byte[]> segments, int inLast)
        {
            Link? head = null;
            Link? tail = null;
            for (int i = 0; i < segments.Count; i++)
            {
                var memory = new ReadOnlyMemory<byte>(segments[i], 0, i == segments.Count - 1 ? inLast : segments[i].Length);
                tail = tail is null ? new Link(memory, 0) : tail.Append(memory);
                head ??= tail;
            }

            return new RentedPayload(segments, new ReadOnlySequence<byte>(head!, 0, tail!, tail!.Memory.Length));
        }

        /// <summary>Takes ownership of <paramref name="buffer"/>, exposing its first <paramref name="filled"/> bytes.</summary>
        public static RentedPayload Rented(byte[] buffer, int filled) =>
            new(buffer, new ReadOnlyMemory<byte>(buffer, 0, filled));

        /// <summary>Carries memory this does not own; <see cref="Dispose"/> is then a no-op.</summary>
        public static RentedPayload Borrowed(ReadOnlyMemory<byte> memory) => new(null, memory);

        /// <summary>Returns the buffer to the pool, if there was one.</summary>
        public void Dispose()
        {
            // Not cleared. Returning 20 MB zeroed costs the memset this lot exists to avoid,
            // and the pool is process-local: the bytes were already the caller's own artifact.
            if (_rented is not null)
            {
                ArrayPool<byte>.Shared.Return(_rented);
            }

            foreach (byte[] segment in _segments ?? [])
            {
                ArrayPool<byte>.Shared.Return(segment);
            }
        }

        /// <summary>One link of a segmented payload, which is all <see cref="ReadOnlySequence{T}"/> asks for.</summary>
        private sealed class Link : ReadOnlySequenceSegment<byte>
        {
            public Link(ReadOnlyMemory<byte> memory, long runningIndex)
            {
                Memory = memory;
                RunningIndex = runningIndex;
            }

            public Link Append(ReadOnlyMemory<byte> memory)
            {
                var next = new Link(memory, RunningIndex + Memory.Length);
                Next = next;
                return next;
            }
        }
    }
}
