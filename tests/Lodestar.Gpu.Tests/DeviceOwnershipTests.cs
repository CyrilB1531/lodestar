using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>What every multi-buffer upload leans on: a buffer is released when what is built from it fails (#1265).</summary>
public sealed class DeviceOwnershipTests
{
    [Fact]
    public void A_failed_build_releases_what_it_was_handed_and_rethrows()
    {
        using var owned = new Tracked();

        InvalidOperationException thrown = Assert.Throws<InvalidOperationException>(
            () => DeviceOwnership.ReleaseOnFailure<int>(owned, () => throw new InvalidOperationException("launch failed")));

        Assert.Equal("launch failed", thrown.Message);
        Assert.True(owned.Disposed);
    }

    [Fact]
    public void A_successful_build_keeps_what_it_was_handed()
    {
        using var owned = new Tracked();

        Assert.Equal(7, DeviceOwnership.ReleaseOnFailure(owned, () => 7));
        Assert.False(owned.Disposed);
    }

    private sealed class Tracked : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose() => Disposed = true;
    }
}
