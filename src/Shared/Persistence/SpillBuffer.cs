namespace Lodestar.Internal.Persistence;

/// <summary>A write-only stream an asynchronous save composes into, in chunks, before copying them out.</summary>
/// <remarks>
/// <see cref="MemoryStream"/> refuses past <c>int.MaxValue</c> bytes, "Stream was too long", where a save writing its
/// lists a mebibyte at a time writes any length: chunks have no such ceiling. The last grows as that stream's buffer
/// did — to what a write needs, at least twice its length and 256 bytes — while it holds less than a mebibyte; past
/// that a new one starts at what the next write needs. An artifact under a mebibyte is then one chunk and one write,
/// and a longer one costs no more than that stream did, a write a mebibyte or so where it made one (#1623, #1633).
/// </remarks>
internal sealed class SpillBuffer : Stream
{
    private const int FirstChunkBytes = 256;

    private const int LargestChunkBytes = 1 << 20;

    private readonly List<byte[]> _chunks = [];
    private int _inLast;
    private long _length;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => _length;

    public override long Position
    {
        get => _length;
        set => throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count) => Write(buffer.AsSpan(offset, count));

#if NETSTANDARD2_0
    public void Write(ReadOnlySpan<byte> buffer)
#else
    public override void Write(ReadOnlySpan<byte> buffer)
#endif
    {
        while (!buffer.IsEmpty)
        {
            if (_chunks.Count == 0 || _inLast == _chunks[_chunks.Count - 1].Length)
            {
                Grow(buffer.Length);
            }

            byte[] last = _chunks[_chunks.Count - 1];
            int taken = Math.Min(buffer.Length, last.Length - _inLast);
            buffer.Slice(0, taken).CopyTo(last.AsSpan(_inLast));
            _inLast += taken;
            _length += taken;
            buffer = buffer.Slice(taken);
        }
    }

    /// <summary>Writes every byte held to <paramref name="destination"/>, a chunk a write: one write under a mebibyte.</summary>
    /// <remarks>
    /// The token goes to every write, as main handed it to its one write of the whole payload: a stream that honours it
    /// stops where it is and raises what it raised on main, and one that ignores it completes, as on main (#1618).
    /// </remarks>
    public async Task CopyOutAsync(Stream destination, CancellationToken cancellationToken)
    {
        for (int i = 0; i < _chunks.Count; i++)
        {
            await WriteAsync(destination, _chunks[i], Count(i), cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Makes room for <paramref name="pending"/> more bytes once the last chunk is full. While it holds less than a
    /// mebibyte it grows as <see cref="MemoryStream"/>'s buffer does, to the larger of what the write needs, twice its
    /// length and 256 bytes, up to one array; past that, a new chunk starts, sized the same way from nothing.
    /// </summary>
    private void Grow(int pending)
    {
        int last = _chunks.Count - 1;
        if (last >= 0 && _chunks[last].Length < LargestChunkBytes)
        {
            byte[] grown = _chunks[last];
            Array.Resize(ref grown, Sized(_inLast + (long)pending, grown.Length));
            _chunks[last] = grown;
            return;
        }

        _chunks.Add(new byte[Sized(pending, 0)]);
        _inLast = 0;
    }

    /// <summary><see cref="MemoryStream"/>'s capacity rule: what is needed, at least twice the length and 256 bytes, up to one array.</summary>
    private static int Sized(long needed, int length) =>
        (int)Math.Min(Math.Max(Math.Max(needed, 2L * length), FirstChunkBytes), TableLength.MaxByteLength);

    /// <summary>The bytes chunk <paramref name="index"/> holds: all of it, but for the last.</summary>
    private int Count(int index) => index == _chunks.Count - 1 ? _inLast : _chunks[index].Length;

    private static ValueTask WriteAsync(Stream destination, byte[] buffer, int count, CancellationToken cancellationToken) =>
#if NETSTANDARD2_0
        new(destination.WriteAsync(buffer, 0, count, cancellationToken));
#else
        destination.WriteAsync(buffer.AsMemory(0, count), cancellationToken);
#endif

    public override void Flush()
    {
        // Nothing is held anywhere but the chunks themselves.
    }

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
}
