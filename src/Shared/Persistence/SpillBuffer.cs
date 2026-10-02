namespace Lodestar.Internal.Persistence;

/// <summary>A write-only stream an asynchronous save composes into, in chunks, before copying them out.</summary>
/// <remarks>
/// <see cref="MemoryStream"/> refuses past <c>int.MaxValue</c> bytes, "Stream was too long", where a save writing its
/// lists a mebibyte at a time writes any length: chunks have no such ceiling, and are never copied but once. They
/// double from 256 bytes, where the stream started, to a mebibyte, so a small artifact costs what it did (#1618).
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
                int size = _chunks.Count == 0 ? FirstChunkBytes : Math.Min(_chunks[_chunks.Count - 1].Length * 2, LargestChunkBytes);
                _chunks.Add(new byte[size]);
                _inLast = 0;
            }

            byte[] last = _chunks[_chunks.Count - 1];
            int taken = Math.Min(buffer.Length, last.Length - _inLast);
            buffer.Slice(0, taken).CopyTo(last.AsSpan(_inLast));
            _inLast += taken;
            _length += taken;
            buffer = buffer.Slice(taken);
        }
    }

    /// <summary>Writes every byte held to <paramref name="destination"/>, a chunk at a time.</summary>
    /// <remarks>
    /// The token goes to every write, as main handed it to its one write of the whole payload: a stream that honours it
    /// stops where it is and raises what it raised on main, and one that ignores it completes, as on main (#1618).
    /// </remarks>
    public async Task CopyOutAsync(Stream destination, CancellationToken cancellationToken)
    {
        for (int i = 0; i < _chunks.Count; i++)
        {
            int count = i == _chunks.Count - 1 ? _inLast : _chunks[i].Length;
#if NETSTANDARD2_0
            await destination.WriteAsync(_chunks[i], 0, count, cancellationToken).ConfigureAwait(false);
#else
            await destination.WriteAsync(_chunks[i].AsMemory(0, count), cancellationToken).ConfigureAwait(false);
#endif
        }
    }

    public override void Flush()
    {
        // Nothing is held anywhere but the chunks themselves.
    }

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
}
