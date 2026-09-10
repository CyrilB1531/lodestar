using System.Diagnostics.CodeAnalysis;
using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>The product of a resident CSR matrix with a dense block, tiled through shared memory.</summary>
/// <remarks>
/// One group per row and column tile. The group loads a tile of the row's stored values and
/// their column indices into shared memory, so a row's non-zeros are read once per group
/// rather than once per thread, and accumulation walks the row in stored order — the order
/// the CPU path walks it, so the two agree far beyond the asserted tolerance. Double
/// precision, because the CPU operand is: FP64 runs at a fraction of FP32 on a consumer
/// card, which is a real reason this kernel may miss decision 0102's gate.
/// </remarks>
public sealed class TiledSparseDenseProduct
{
    /// <summary>The widest group the kernel is compiled for, and the shared tile's size.</summary>
    /// <remarks>
    /// As in <see cref="TiledCosineTopK"/>: ILGPU sizes static shared memory at compile time,
    /// and the group actually launched clamps to what the accelerator allows — ILGPU's CPU
    /// accelerator caps a group dimension at 16.
    /// </remarks>
    private const int MaxGroupSize = 256;

    private readonly GpuContext _context;
    private readonly int _groupSize;
    private readonly Action<KernelConfig, ArrayView<int>, ArrayView<int>, ArrayView<double>,
        ArrayView<double>, ArrayView<double>, int> _product;

    /// <summary>Loads the kernel onto the accelerator.</summary>
    /// <param name="context">The accelerator to compile for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <remarks>Loading compiles, so build this once and reuse it (decision 0102).</remarks>
    public TiledSparseDenseProduct(GpuContext context)
    {
        Guard.NotNull(context);
        _context = context;
        _groupSize = Math.Min(MaxGroupSize, context.Accelerator.MaxGroupSize.X);
        _product = context.Accelerator.LoadStreamKernel<ArrayView<int>, ArrayView<int>,
            ArrayView<double>, ArrayView<double>, ArrayView<double>, int>(ProductKernel);
    }

    /// <summary>Computes <c>matrix · block</c>, row-major.</summary>
    /// <param name="matrix">The resident sparse left operand.</param>
    /// <param name="block">
    /// <c>matrix.ColumnCount</c> rows of <paramref name="width"/>, row-major — the same shape
    /// <c>CsrMatrix.Multiply</c> takes.
    /// </param>
    /// <param name="width">How many columns <paramref name="block"/> holds.</param>
    /// <returns><c>matrix.RowCount</c> rows of <paramref name="width"/>, row-major.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is below 1.</exception>
    /// <exception cref="ArgumentException"><paramref name="block"/> is not that shape.</exception>
    public double[] Multiply(DeviceSparseMatrix matrix, ReadOnlySpan<double> block, int width)
    {
        Guard.NotNull(matrix);
        Guard.NotLessThan(width, 1);
        if (block.Length != (long)matrix.ColumnCount * width)
        {
            throw new ArgumentException(
                $"block holds {block.Length} values, not {matrix.ColumnCount} x {width}.", nameof(block));
        }

        using DeviceDenseBlock dense =
            DeviceDenseBlock.Upload(_context, block, matrix.ColumnCount, width);
        using DeviceDenseBlock product = Multiply(matrix, dense);
        return product.Download();
    }

    /// <summary>Computes <c>matrix · block</c> and leaves the result on the accelerator.</summary>
    /// <param name="matrix">The resident sparse left operand.</param>
    /// <param name="block">The resident dense right operand, as many rows as the matrix has columns.</param>
    /// <returns>A resident block of <c>matrix.RowCount</c> rows and <c>block.ColumnCount</c> columns.</returns>
    /// <exception cref="ArgumentNullException">An argument is null.</exception>
    /// <exception cref="ArgumentException">The two operands do not compose.</exception>
    /// <remarks>
    /// The chaining entry point: its result is the type its own operand is, so a second product
    /// consumes it without crossing the bus. The caller ends the chain with
    /// <see cref="DeviceDenseBlock.Download"/> and pays one copy for however many steps it held.
    /// </remarks>
    public DeviceDenseBlock Multiply(DeviceSparseMatrix matrix, DeviceDenseBlock block)
    {
        Guard.NotNull(matrix);
        Guard.NotNull(block);
        if (block.RowCount != matrix.ColumnCount)
        {
            throw new ArgumentException(
                $"a matrix of {matrix.RowCount} x {matrix.ColumnCount} does not multiply a block "
                + $"of {block.RowCount} rows.", nameof(block));
        }

        Accelerator accelerator = _context.Accelerator;
        MemoryBuffer1D<double, Stride1D.Dense> result =
            accelerator.Allocate1D<double>((long)matrix.RowCount * block.ColumnCount);

        int tiles = (block.ColumnCount + _groupSize - 1) / _groupSize;
        _product(
            new KernelConfig(new Index2D(tiles, matrix.RowCount), new Index2D(_groupSize, 1)),
            matrix.RowPointers.View, matrix.ColumnIndices.View, matrix.Values.View,
            block.Buffer.View, result.View, block.ColumnCount);
        accelerator.Synchronize();

        return new DeviceDenseBlock(result, matrix.RowCount, block.ColumnCount);
    }

    /// <summary>One group per row and column tile, the row's non-zeros tiled through shared memory.</summary>
    // long-comment: why a kernel is excluded from coverage instrumentation. Coverlet
    // rewrites an instrumented method to record each hit through a mutable static array,
    // and ILGPU refuses device code that reads a static field which is not read only. So
    // every kernel in this package failed to compile under coverage while passing without
    // it: measured, thirty-six of forty-six tests failed in that job and none locally.
    // The exclusion covers the device method alone; the host code around it is
    // instrumented as usual.
    [ExcludeFromCodeCoverage]
    private static void ProductKernel(
        ArrayView<int> rowPointers, ArrayView<int> columnIndices, ArrayView<double> values,
        ArrayView<double> block, ArrayView<double> result, int width)
    {
        ArrayView<double> tileValue = SharedMemory.Allocate1D<double>(MaxGroupSize);
        ArrayView<int> tileColumn = SharedMemory.Allocate1D<int>(MaxGroupSize);
        int lanes = Group.DimX;
        int row = Grid.IdxY;
        int column = (Grid.IdxX * lanes) + Group.IdxX;
        int start = rowPointers[row];
        int end = rowPointers[row + 1];
        double accumulator = 0.0;

        for (int at = start; at < end; at += lanes)
        {
            int span = Math.Min(lanes, end - at);
            if (Group.IdxX < span)
            {
                tileValue[Group.IdxX] = values[at + Group.IdxX];
                tileColumn[Group.IdxX] = columnIndices[at + Group.IdxX];
            }

            Group.Barrier();
            if (column < width)
            {
                for (int i = 0; i < span; i++)
                {
                    accumulator += tileValue[i] * block[((long)tileColumn[i] * width) + column];
                }
            }

            Group.Barrier();
        }

        if (column < width)
        {
            result[((long)row * width) + column] = accumulator;
        }
    }
}
