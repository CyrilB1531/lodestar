namespace Lodestar.Text.Benchmarks;

/// <summary>A read-only stream over bytes that declares no length and cannot seek, as a pipe does.</summary>
internal sealed class UndeclaredStream(byte[] bytes) : Stream
{
    private int _position;

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int taken = Math.Min(count, bytes.Length - _position);
        Array.Copy(bytes, _position, buffer, offset, taken);
        _position += taken;
        return taken;
    }

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override void Flush()
    {
        // Nothing is written.
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
}
