using System.Collections;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>The Review B findings of <c>Lodestar.Gpu</c> after #1400, one fact each, on ILGPU's CPU accelerator.</summary>
public sealed class ReviewBAfter1400Tests
{
    [Fact]
    public void The_accelerator_is_refused_after_dispose()
    {
        GpuContext context = GpuContext.Create(preferCpu: true);
        context.Dispose();

        // It handed out the freed accelerator (#1512).
        Assert.Throws<ObjectDisposedException>(() => context.Accelerator);
    }

    [Fact]
    public void A_batch_whose_hash_count_passes_one_array_is_refused_before_allocating()
    {
        using var context = GpuContext.Create(preferCpu: true);
        IReadOnlyList<uint>[] documents = [new Counted(int.MaxValue), new Counted(int.MaxValue), new Counted(3)];

        // The int sum wrapped to 1 and the copy failed on an index (#1511).
        ArgumentException error = Assert.Throws<ArgumentException>(() => DeviceTokenHashes.Upload(context, documents));

        Assert.Equal("documents", error.ParamName);
    }

    /// <summary>A document that reports a count and holds nothing: the refusal must come before any read.</summary>
    private sealed class Counted(int count) : IReadOnlyList<uint>
    {
        public int Count { get; } = count;

        public uint this[int index] => throw new InvalidOperationException("The upload read before refusing the batch.");

        public IEnumerator<uint> GetEnumerator() => throw new InvalidOperationException("The upload read before refusing the batch.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
