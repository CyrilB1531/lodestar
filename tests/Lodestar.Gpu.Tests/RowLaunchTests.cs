using Lodestar.Abstractions;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random builds reproducible token hashes so a
// failing case replays; there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>A launch split into slices answers bit for bit what one launch does.</summary>
/// <remarks>
/// CUDA refuses more than 65,535 rows on a grid's second axis (#897), and ILGPU's CPU accelerator
/// refuses nothing, so the slicing is forced here through the internal limit. Seven divides none
/// of the counts, which leaves a short last slice: the case a wrong offset gets wrong.
/// </remarks>
public sealed class RowLaunchTests
{
    private const int Slice = 7;

    [Fact]
    public void A_sparse_product_launched_in_slices_matches_one_launch()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 101, columns: 40, density: 0.1, seed: 8971);
        double[] block = SparseCorpus.Block(40, 20, seed: 8972);
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceSparseMatrix.Upload(
            context, matrix.RowPointers, matrix.ColumnIndices, matrix.Values,
            matrix.RowCount, matrix.ColumnCount);

        double[] whole = new TiledSparseDenseProduct(context).Multiply(resident, block, 20);
        double[] sliced = new TiledSparseDenseProduct(context, Slice).Multiply(resident, block, 20);

        Assert.Equal(whole, sliced);
    }

    [Fact]
    public void MinHash_signatures_launched_in_slices_match_one_launch()
    {
        var random = new Random(8973);
        uint[][] documents = [.. Enumerable.Range(0, 101).Select(row =>
            Enumerable.Range(0, row % 13).Select(_ => (uint)random.Next()).ToArray())];
        ulong[] multipliers = [.. Enumerable.Range(0, 20).Select(_ => (ulong)random.NextInt64(1, long.MaxValue))];
        ulong[] addends = [.. Enumerable.Range(0, 20).Select(_ => (ulong)random.NextInt64(0, long.MaxValue))];
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, documents);

        IReadOnlyList<uint[]> whole = new TiledMinHashSignatures(context).Signatures(resident, multipliers, addends);
        IReadOnlyList<uint[]> sliced =
            new TiledMinHashSignatures(context, Slice).Signatures(resident, multipliers, addends);

        Assert.Equal(whole, sliced);
    }

    [Fact]
    public void A_query_batch_launched_in_slices_matches_one_launch()
    {
        const int count = 90;
        const int dimension = 24;
        const int queries = 101;
        float[] rows = CosineCorpus.Rows(count, dimension, seed: 8974);
        float[] batch = CosineCorpus.Rows(queries, dimension, seed: 8975);
        using var context = GpuContext.Create(preferCpu: true);
        using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count, dimension);

        IReadOnlyList<IReadOnlyList<GpuSearchResult>> whole =
            new TiledCosineTopK(context).Search(matrix, batch, queries, 5);
        IReadOnlyList<IReadOnlyList<GpuSearchResult>> sliced =
            new TiledCosineTopK(context, Slice).Search(matrix, batch, queries, 5);

        Assert.Equal(whole, sliced);
    }
}
