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

    // The chunk being filled, and those before it, listed only once there is one: an artifact under a mebibyte is one
    // chunk, and a list for it cost a small save 88 bytes on 0.7.0's MemoryStream (#1649).
    private byte[]? _last;
    private List<byte[]>? _full;
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
            if (_last is null || _inLast == _last.Length)
            {
                Grow(buffer.Length);
            }

            byte[] last = _last!;
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
    public ValueTask CopyOutAsync(Stream destination, CancellationToken cancellationToken)
    {
        if (_full is null)
        {
            // One chunk, one write, and no state machine of its own, as 0.7.0's one WriteAsync (#1649).
            return _last is null ? default : WriteAsync(destination, _last, _inLast, cancellationToken);
        }

        return CopyAllOutAsync(destination, cancellationToken);
    }

    private async ValueTask CopyAllOutAsync(Stream destination, CancellationToken cancellationToken)
    {
        foreach (byte[] chunk in _full!)
        {
            await WriteAsync(destination, chunk, chunk.Length, cancellationToken).ConfigureAwait(false);
        }
        await WriteAsync(destination, _last!, _inLast, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Makes room for <paramref name="pending"/> more bytes once the last chunk is full. While it holds less than a
    /// mebibyte it grows as <see cref="MemoryStream"/>'s buffer does, to the larger of what the write needs, twice its
    /// length and 256 bytes, up to one array; past that, a new chunk starts, sized the same way from nothing.
    /// </summary>
    private void Grow(int pending)
    {
        if (_last is not null && _last.Length < LargestChunkBytes)
        {
            Array.Resize(ref _last, Sized(_inLast + (long)pending, _last.Length));
            return;
        }

        if (_last is not null)
        {
            (_full ??= []).Add(_last);
        }
        _last = new byte[Sized(pending, 0)];
        _inLast = 0;
    }

    /// <summary><see cref="MemoryStream"/>'s capacity rule: what is needed, at least twice the length and 256 bytes, up to one array.</summary>
    private static int Sized(long needed, int length) =>
        (int)Math.Min(Math.Max(Math.Max(needed, 2L * length), FirstChunkBytes), TableLength.MaxByteLength);

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
