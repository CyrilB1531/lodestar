using Lodestar.Abstractions;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>What residency between two operations promises, and what it must not change.</summary>
/// <remarks>
/// The claim decision 0102 deferred until three kernels existed: a chain that never leaves the
/// accelerator answers what the same steps answer with a round trip between them. These pin the
/// answer; the benchmark prices the difference.
/// </remarks>
public sealed class DeviceDenseBlockTests
{
    private static DeviceSparseMatrix Upload(GpuContext context, CsrMatrix matrix) =>
        DeviceSparseMatrix.Upload(
            context, matrix.RowPointers, matrix.ColumnIndices, matrix.Values,
            matrix.RowCount, matrix.ColumnCount);

    [Fact]
    public void A_chain_answers_what_the_same_steps_answer_with_a_round_trip()
    {
        // long-comment: two products against the same two with a download between them.
        // Identical rather than close, and that is the assertion worth making: residency
        // moves where the numbers live, not how they are computed, so any difference at
        // all would be a defect rather than a rounding.
        CsrMatrix first = SparseCorpus.Matrix(rows: 40, columns: 60, density: 0.1, seed: 6601);
        CsrMatrix second = SparseCorpus.Matrix(rows: 60, columns: 50, density: 0.1, seed: 6602);
        double[] block = SparseCorpus.Block(50, 8, seed: 6603);

        using var context = GpuContext.Create(preferCpu: true);
        using var deviceFirst = Upload(context, first);
        using var deviceSecond = Upload(context, second);
        var kernel = new TiledSparseDenseProduct(context);

        using var resident = DeviceDenseBlock.Upload(context, block, 50, 8);
        using DeviceDenseBlock inner = kernel.Multiply(deviceSecond, resident);
        using DeviceDenseBlock outer = kernel.Multiply(deviceFirst, inner);
        double[] chained = outer.Download();

        double[] roundTripped = kernel.Multiply(
            deviceFirst, kernel.Multiply(deviceSecond, block, 8), 8);

        Assert.Equal(roundTripped, chained);
    }

    [Fact]
    public void A_chained_product_matches_the_cpu_path_composed_twice()
    {
        CsrMatrix first = SparseCorpus.Matrix(rows: 20, columns: 30, density: 0.2, seed: 6604);
        CsrMatrix second = SparseCorpus.Matrix(rows: 30, columns: 25, density: 0.2, seed: 6605);
        double[] block = SparseCorpus.Block(25, 4, seed: 6606);

        double[] expected = first.Multiply(second.Multiply(block, 4), 4);

        using var context = GpuContext.Create(preferCpu: true);
        using var deviceFirst = Upload(context, first);
        using var deviceSecond = Upload(context, second);
        var kernel = new TiledSparseDenseProduct(context);
        using var resident = DeviceDenseBlock.Upload(context, block, 25, 4);
        using DeviceDenseBlock inner = kernel.Multiply(deviceSecond, resident);
        using DeviceDenseBlock outer = kernel.Multiply(deviceFirst, inner);

        double[] actual = outer.Download();
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], 1e-6);
        }
    }

    [Fact]
    public void A_block_carries_the_shape_the_product_gave_it()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 12, columns: 9, density: 0.3, seed: 6607);
        double[] block = SparseCorpus.Block(9, 5, seed: 6608);

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = Upload(context, matrix);
        using var operand = DeviceDenseBlock.Upload(context, block, 9, 5);
        using DeviceDenseBlock product = new TiledSparseDenseProduct(context).Multiply(resident, operand);

        Assert.Equal(12, product.RowCount);
        Assert.Equal(5, product.ColumnCount);
        Assert.Equal(60, product.Download().Length);
    }

    [Fact]
    public void Downloading_twice_gives_the_same_values()
    {
        // A download is a copy, not a move: a caller inspecting a block mid-chain must not
        // consume it, or the next step would read whatever the buffer became.
        double[] block = SparseCorpus.Block(6, 3, seed: 6609);
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceDenseBlock.Upload(context, block, 6, 3);

        Assert.Equal(resident.Download(), resident.Download());
        Assert.Equal(block, resident.Download());
    }

    [Fact]
    public void A_block_whose_rows_do_not_match_the_matrix_is_refused()
    {
        CsrMatrix matrix = SparseCorpus.Matrix(rows: 8, columns: 10, density: 0.4, seed: 6610);
        double[] block = SparseCorpus.Block(7, 2, seed: 6611);

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = Upload(context, matrix);
        using var operand = DeviceDenseBlock.Upload(context, block, 7, 2);

        Assert.Throws<ArgumentException>(
            () => new TiledSparseDenseProduct(context).Multiply(resident, operand));
    }

    [Fact]
    public void A_block_of_the_wrong_length_is_refused_on_upload()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.Throws<ArgumentException>(
            () => DeviceDenseBlock.Upload(context, new double[7], 3, 3));
    }

    [Fact]
    public void A_non_positive_dimension_is_refused_on_upload()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => DeviceDenseBlock.Upload(context, new double[4], 0, 4));
    }
}
