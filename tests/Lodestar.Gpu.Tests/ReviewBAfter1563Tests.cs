using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>The Review B findings of <c>Lodestar.Gpu</c> after #1563, one fact each, on ILGPU's CPU accelerator.</summary>
public sealed class ReviewBAfter1563Tests
{
    [Fact]
    public void Hits_past_one_array_are_refused_before_any_launch()
    {
        // 70,000 queries of 32,768 hits is 2.29e9 results, copied back only after both launches (#1576).
        using var context = GpuContext.Create(preferCpu: true);
        const int Rows = 32_768;
        const int Queries = 70_000;
        float[] rows = [.. Enumerable.Repeat(1f, Rows)];
        using DeviceEmbeddingMatrix matrix = DeviceEmbeddingMatrix.Upload(context, rows, Rows, 1);
        var kernel = new TiledCosineTopK(context);
        float[] queries = [.. Enumerable.Repeat(1f, Queries)];

        ArgumentException error = Assert.Throws<ArgumentException>(() => kernel.Search(matrix, queries, Queries, Rows));
        Assert.Equal("k", error.ParamName);
    }

    [Fact]
    public void The_resident_product_still_refuses_past_one_array_under_block()
    {
        // The span overload now refuses first, under width (#1578); the resident one keeps its own check (#1558).
        using var context = GpuContext.Create(preferCpu: true);
        const int Rows = 70_000;
        const int Width = 32_768;
        using DeviceSparseMatrix matrix = DeviceSparseMatrix.Upload(context, new int[Rows + 1], [], [], Rows, 1);
        using DeviceDenseBlock block = DeviceDenseBlock.Upload(context, new double[Width], 1, Width);
        var kernel = new TiledSparseDenseProduct(context);

        ArgumentException error = Assert.Throws<ArgumentException>(() => kernel.Multiply(matrix, block));
        Assert.Equal("block", error.ParamName);
    }
}
