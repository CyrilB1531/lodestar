using Lodestar.Abstractions;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>The SpMM kernel against <c>CsrMatrix.Multiply</c>, the path it replaces.</summary>
/// <remarks>
/// All on ILGPU's CPU accelerator: correctness has to be answerable where there is no GPU.
/// The kernel walks a row in stored order, as the CPU does, so the two agree far inside the
/// tolerance asserted here — a looser agreement would mean the orders had diverged.
/// </remarks>
public sealed class TiledSparseDenseProductTests
{
    private const double Tolerance = 1e-6;

    [Fact]
    public void A_sparse_block_product_matches_csrmatrix()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 120, columns: 80, density: 0.08, seed: 5551);
        double[] block = SparseCorpus.Block(80, 16, seed: 5552);

        AssertSameProduct(matrix, block, 16);
    }

    [Fact]
    public void A_width_past_one_tile_still_matches()
    {
        // 300 columns spans more than one tile with the last partial, which is the case
        // an off-by-one in the tile loop gets wrong and a narrow block never reaches.
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 40, columns: 30, density: 0.2, seed: 5553);
        double[] block = SparseCorpus.Block(30, 300, seed: 5554);

        AssertSameProduct(matrix, block, 300);
    }

    [Fact]
    public void A_row_with_more_non_zeros_than_one_tile_still_matches()
    {
        // A dense row: its stored values outrun the shared tile, so the kernel has to
        // reload the tile mid-row. A matrix whose rows all fit never tests that loop.
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 8, columns: 400, density: 1.0, seed: 5555);
        double[] block = SparseCorpus.Block(400, 4, seed: 5556);

        AssertSameProduct(matrix, block, 4);
    }

    [Fact]
    public void An_empty_row_produces_zeros_rather_than_stale_memory()
    {
        // A row with no stored value never enters the accumulation loop, so its output is
        // whatever the result buffer held. Allocation zeroes it; this fails if it stops.
        double[] values = [2.0, 3.0];
        int[] columns = [0, 2];
        int[] pointers = [0, 1, 1, 2];
        var matrix = new CsrMatrix(3, 3, values, columns, pointers);
        double[] block = SparseCorpus.Block(3, 5, seed: 5557);

        double[] actual = Run(matrix, block, 5);
        for (int column = 0; column < 5; column++)
        {
            Assert.Equal(0.0, actual[(1 * 5) + column]);
        }

        AssertSameProduct(matrix, block, 5);
    }

    [Fact]
    public void A_single_column_block_is_the_matrix_vector_product()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 64, columns: 50, density: 0.1, seed: 5558);
        double[] block = SparseCorpus.Block(50, 1, seed: 5559);

        double[] expected = matrix.Multiply(block);
        double[] actual = Run(matrix, block, 1);

        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Tolerance);
        }
    }

    [Fact]
    public void A_block_of_the_wrong_height_is_refused()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 4, columns: 4, density: 0.5, seed: 5560);
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = Upload(context, matrix);
        var kernel = new TiledSparseDenseProduct(context);

        Assert.Throws<ArgumentException>(() => kernel.Multiply(resident, new double[7], 2));
    }

    [Fact]
    public void A_row_pointer_array_of_the_wrong_length_is_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.Throws<ArgumentException>(() => DeviceSparseMatrix.Upload(
            context, [0, 1], [0], [1.0], rowCount: 3, columnCount: 2));
    }

    [Fact]
    public void A_second_product_on_one_instance_agrees_with_the_first()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 32, columns: 24, density: 0.15, seed: 5561);
        double[] block = SparseCorpus.Block(24, 6, seed: 5562);

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = Upload(context, matrix);
        var kernel = new TiledSparseDenseProduct(context);

        Assert.Equal(kernel.Multiply(resident, block, 6), kernel.Multiply(resident, block, 6));
    }

    private static DeviceSparseMatrix Upload(GpuContext context, CsrMatrix matrix) =>
        DeviceSparseMatrix.Upload(
            context, matrix.RowPointers, matrix.ColumnIndices, matrix.Values,
            matrix.RowCount, matrix.ColumnCount);

    private static double[] Run(CsrMatrix matrix, double[] block, int width)
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = Upload(context, matrix);
        return new TiledSparseDenseProduct(context).Multiply(resident, block, width);
    }

    private static void AssertSameProduct(CsrMatrix matrix, double[] block, int width)
    {
        double[] expected = matrix.Multiply(block, width);
        double[] actual = Run(matrix, block, width);

        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Tolerance);
        }
    }
}
