namespace Lodestar.Gpu.Compute;

/// <summary>Which context a device object was allocated on, and whether it has been released.</summary>
/// <remarks>
/// A buffer belongs to one accelerator. Handed to another context's kernel, or read after its
/// release, it is a pointer the launch trusts, so every entry point asks this first: two
/// field reads, against a launch that costs microseconds. Not synchronized with a concurrent
/// <c>Dispose</c>, which is a caller's race as it is for any disposable.
/// </remarks>
internal sealed class DeviceResidency
{
    private bool _released;

    internal DeviceResidency(GpuContext owner) => Owner = owner;

    /// <summary>The context whose accelerator holds the buffers.</summary>
    internal GpuContext Owner { get; }

    /// <summary>Marks the buffers released, and says whether this call is the one that did.</summary>
    /// <returns><see langword="true"/> the first time, so a second <c>Dispose</c> frees nothing twice.</returns>
    internal bool Release()
    {
        if (_released)
        {
            return false;
        }

        _released = true;
        return true;
    }

    /// <summary>Throws when the buffers, or the accelerator under them, are gone.</summary>
    /// <param name="name">The parameter, or the type, the exception names.</param>
    /// <exception cref="ObjectDisposedException">The object or its context was disposed.</exception>
    internal void EnsureLive(string name)
    {
        if (_released)
        {
            throw new ObjectDisposedException(name, $"{name} was disposed, and its device memory with it.");
        }

        if (Owner.IsDisposed)
        {
            throw new ObjectDisposedException(
                name, $"the GpuContext {name} was uploaded to was disposed, and its device memory with it.");
        }
    }

    /// <summary>Throws unless a kernel loaded on <paramref name="context"/> may read the buffers.</summary>
    /// <param name="context">The context the kernel was loaded onto.</param>
    /// <param name="parameter">The parameter the buffers arrived through.</param>
    /// <exception cref="ObjectDisposedException">The object or its context was disposed.</exception>
    /// <exception cref="ArgumentException">The object was uploaded to another context.</exception>
    internal void EnsureUsableBy(GpuContext context, string parameter)
    {
        EnsureLive(parameter);
        if (!ReferenceEquals(Owner, context))
        {
            throw new ArgumentException(
                $"{parameter} was uploaded to another GpuContext than this kernel's; upload it to this one.",
                parameter);
        }
    }
}
