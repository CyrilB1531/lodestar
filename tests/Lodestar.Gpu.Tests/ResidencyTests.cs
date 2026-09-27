using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>Every entry point refuses device data from another context, or data already released.</summary>
/// <remarks>
/// A buffer is a pointer into one accelerator's memory, and a launch trusts it. Handed to a kernel
/// of another context, or read after its release, it is read or written anyway unless the entry
/// point asks first (#1214), so each test here names the parameter the refusal must carry.
/// </remarks>
public sealed class ResidencyTests
{
    private static readonly float[] Rows = [1f, 0f, 0f, 1f];
    private static readonly ulong[] Multipliers = [3UL];
    private static readonly ulong[] Addends = [5UL];

    [Fact]
    public void A_matrix_from_another_context_is_refused_by_the_cosine_kernel()
    {
        using var mine = GpuContext.Create(preferCpu: true);
        using var theirs = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(theirs, Rows, count: 2, dimension: 2);
        var kernel = new TiledCosineTopK(mine);

        ArgumentException refused = Assert.Throws<ArgumentException>(() => kernel.Search(matrix, [1f, 0f], 1, 1));
        Assert.Equal("matrix", refused.ParamName);
    }

    [Fact]
    public void A_disposed_matrix_is_refused_by_the_cosine_kernel()
    {
        using var context = GpuContext.Create(preferCpu: true);
        var matrix = DeviceEmbeddingMatrix.Upload(context, Rows, count: 2, dimension: 2);
        var kernel = new TiledCosineTopK(context);
        matrix.Dispose();

        ObjectDisposedException refused =
            Assert.Throws<ObjectDisposedException>(() => kernel.Search(matrix, [1f, 0f], 1, 1));
        Assert.Equal("matrix", refused.ObjectName);
    }

    [Fact]
    public void A_kernel_whose_context_was_disposed_is_refused()
    {
        var context = GpuContext.Create(preferCpu: true);
        var matrix = DeviceEmbeddingMatrix.Upload(context, Rows, count: 2, dimension: 2);
        var kernel = new TiledCosineTopK(context);
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => kernel.Search(matrix, [1f, 0f], 1, 1));
        matrix.Dispose();
    }

    [Fact]
    public void A_disposed_context_is_refused_by_every_upload_and_every_kernel()
    {
        var context = GpuContext.Create(preferCpu: true);
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => DeviceEmbeddingMatrix.Upload(context, Rows, 2, 2));
        Assert.Throws<ObjectDisposedException>(() => DeviceDenseBlock.Upload(context, [1.0], 1, 1));
        Assert.Throws<ObjectDisposedException>(() => DeviceSparseMatrix.Upload(context, [0, 1], [0], [1.0], 1, 1));
        Assert.Throws<ObjectDisposedException>(() => DeviceTextBlock.Upload(context, "ab", ["ab"]));
        Assert.Throws<ObjectDisposedException>(() => DeviceTokenHashes.Upload(context, [[1u]]));
        Assert.Throws<ObjectDisposedException>(() => new TiledCosineTopK(context));
        Assert.Throws<ObjectDisposedException>(() => new TiledSparseDenseProduct(context));
        Assert.Throws<ObjectDisposedException>(() => new BitParallelEditDistance(context));
        Assert.Throws<ObjectDisposedException>(() => new TiledMinHashSignatures(context));
    }

    [Fact]
    public void Operands_from_another_context_are_refused_by_the_product()
    {
        using var mine = GpuContext.Create(preferCpu: true);
        using var theirs = GpuContext.Create(preferCpu: true);
        using var ownMatrix = DeviceSparseMatrix.Upload(mine, [0, 1], [0], [2.0], 1, 1);
        using var foreignMatrix = DeviceSparseMatrix.Upload(theirs, [0, 1], [0], [2.0], 1, 1);
        using var ownBlock = DeviceDenseBlock.Upload(mine, [3.0], 1, 1);
        using var foreignBlock = DeviceDenseBlock.Upload(theirs, [3.0], 1, 1);
        var kernel = new TiledSparseDenseProduct(mine);

        Assert.Equal("matrix", Assert.Throws<ArgumentException>(() => kernel.Multiply(foreignMatrix, [3.0], 1)).ParamName);
        Assert.Equal("matrix", Assert.Throws<ArgumentException>(() => kernel.Multiply(foreignMatrix, ownBlock)).ParamName);
        Assert.Equal("block", Assert.Throws<ArgumentException>(() => kernel.Multiply(ownMatrix, foreignBlock)).ParamName);
    }

    [Fact]
    public void Disposed_operands_are_refused_by_the_product()
    {
        using var context = GpuContext.Create(preferCpu: true);
        var matrix = DeviceSparseMatrix.Upload(context, [0, 1], [0], [2.0], 1, 1);
        using var liveMatrix = DeviceSparseMatrix.Upload(context, [0, 1], [0], [2.0], 1, 1);
        var block = DeviceDenseBlock.Upload(context, [3.0], 1, 1);
        var kernel = new TiledSparseDenseProduct(context);
        matrix.Dispose();
        block.Dispose();

        Assert.Equal("matrix", Assert.Throws<ObjectDisposedException>(() => kernel.Multiply(matrix, [3.0], 1)).ObjectName);
        Assert.Equal("block", Assert.Throws<ObjectDisposedException>(() => kernel.Multiply(liveMatrix, block)).ObjectName);
    }

    [Fact]
    public void A_product_lives_on_the_kernels_context_and_chains_there()
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceSparseMatrix.Upload(context, [0, 1], [0], [2.0], 1, 1);
        using var block = DeviceDenseBlock.Upload(context, [3.0], 1, 1);
        var kernel = new TiledSparseDenseProduct(context);

        using DeviceDenseBlock once = kernel.Multiply(matrix, block);
        using DeviceDenseBlock twice = kernel.Multiply(matrix, once);

        Assert.Equal([12.0], twice.Download());
    }

    [Fact]
    public void A_disposed_block_refuses_to_download()
    {
        using var context = GpuContext.Create(preferCpu: true);
        var block = DeviceDenseBlock.Upload(context, [3.0], 1, 1);
        block.Dispose();

        Assert.Throws<ObjectDisposedException>(block.Download);
    }

    [Fact]
    public void A_text_block_from_another_context_or_disposed_is_refused()
    {
        using var mine = GpuContext.Create(preferCpu: true);
        using var theirs = GpuContext.Create(preferCpu: true);
        using var foreign = DeviceTextBlock.Upload(theirs, "ab", ["ab"]);
        var released = DeviceTextBlock.Upload(mine, "ab", ["ab"]);
        released.Dispose();
        var kernel = new BitParallelEditDistance(mine);

        Assert.Equal("texts", Assert.Throws<ArgumentException>(() => kernel.Distance("ab", foreign)).ParamName);
        Assert.Equal("texts", Assert.Throws<ObjectDisposedException>(() => kernel.Distance("ab", released)).ObjectName);
    }

    [Fact]
    public void Token_hashes_from_another_context_or_disposed_are_refused()
    {
        using var mine = GpuContext.Create(preferCpu: true);
        using var theirs = GpuContext.Create(preferCpu: true);
        using var foreign = DeviceTokenHashes.Upload(theirs, [[1u, 2u]]);
        var released = DeviceTokenHashes.Upload(mine, [[1u, 2u]]);
        released.Dispose();
        var kernel = new TiledMinHashSignatures(mine);

        Assert.Equal("documents", Assert.Throws<ArgumentException>(
            () => kernel.Signatures(foreign, Multipliers, Addends)).ParamName);
        Assert.Equal("documents", Assert.Throws<ObjectDisposedException>(
            () => kernel.Signatures(released, Multipliers, Addends)).ObjectName);
    }

    [Fact]
    public void A_second_dispose_frees_nothing_twice()
    {
        // Each is disposed twice here and a third time by its using, context last.
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, Rows, count: 2, dimension: 2);
        using var sparse = DeviceSparseMatrix.Upload(context, [0, 1], [0], [2.0], 1, 1);
        using var block = DeviceDenseBlock.Upload(context, [3.0], 1, 1);
        using var texts = DeviceTextBlock.Upload(context, "ab", ["ab"]);
        using var hashes = DeviceTokenHashes.Upload(context, [[1u]]);
        IDisposable[] owned = [matrix, sparse, block, texts, hashes, context];

        Exception? repeated = Record.Exception(() =>
        {
            foreach (IDisposable resource in owned.Concat(owned))
            {
                resource.Dispose();
            }
        });

        Assert.Null(repeated);
    }
}
