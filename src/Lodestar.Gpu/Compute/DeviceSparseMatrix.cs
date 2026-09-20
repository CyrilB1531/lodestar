using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>A CSR matrix held on the accelerator across many products.</summary>
/// <remarks>
/// The three CSR arrays are taken as spans rather than as a <c>CsrMatrix</c>: decision 0003
/// forbids an edge into this package and an edge out would floor it on a published sibling
/// for one type. A caller holding a <c>CsrMatrix</c> passes its
/// <c>RowPointers</c>, <c>ColumnIndices</c> and <c>Values</c> directly.
/// </remarks>
public sealed class DeviceSparseMatrix : IDisposable
{
    internal MemoryBuffer1D<int, Stride1D.Dense> RowPointers { get; }

    internal MemoryBuffer1D<int, Stride1D.Dense> ColumnIndices { get; }

    internal MemoryBuffer1D<double, Stride1D.Dense> Values { get; }

    /// <summary>How many rows the matrix has.</summary>
    public int RowCount { get; }

    /// <summary>How many columns the matrix has.</summary>
    public int ColumnCount { get; }

    /// <summary>How many stored values the matrix holds.</summary>
    public int NonZeroCount => (int)Values.Length;

    private DeviceSparseMatrix(
        MemoryBuffer1D<int, Stride1D.Dense> rowPointers,
        MemoryBuffer1D<int, Stride1D.Dense> columnIndices,
        MemoryBuffer1D<double, Stride1D.Dense> values,
        int rowCount,
        int columnCount)
    {
        RowPointers = rowPointers;
        ColumnIndices = columnIndices;
        Values = values;
        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    /// <summary>Uploads a CSR matrix to the accelerator.</summary>
    /// <param name="context">The accelerator to upload to.</param>
    /// <param name="rowPointers"><paramref name="rowCount"/> + 1 offsets into the other two.</param>
    /// <param name="columnIndices">One column per stored value, ascending inside a row.</param>
    /// <param name="values">The stored values, in the same order.</param>
    /// <param name="rowCount">How many rows the matrix has.</param>
    /// <param name="columnCount">How many columns the matrix has.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowCount"/> or <paramref name="columnCount"/> is below 1.</exception>
    /// <exception cref="ArgumentException">The three arrays do not describe one CSR matrix.</exception>
    public static DeviceSparseMatrix Upload(
        GpuContext context,
        ReadOnlySpan<int> rowPointers,
        ReadOnlySpan<int> columnIndices,
        ReadOnlySpan<double> values,
        int rowCount,
        int columnCount)
    {
        Guard.NotNull(context);
        Guard.NotLessThan(rowCount, 1);
        Guard.NotLessThan(columnCount, 1);
        if (rowPointers.Length != rowCount + 1)
        {
            throw new ArgumentException(
                $"rowPointers holds {rowPointers.Length} offsets, not {rowCount + 1}.", nameof(rowPointers));
        }

        if (columnIndices.Length != values.Length)
        {
            throw new ArgumentException(
                $"columnIndices holds {columnIndices.Length} entries and values {values.Length}.",
                nameof(columnIndices));
        }

        if (rowPointers[rowCount] != values.Length)
        {
            throw new ArgumentException(
                $"rowPointers ends at {rowPointers[rowCount]} and values holds {values.Length}.",
                nameof(rowPointers));
        }

        RefuseMalformedStructure(rowPointers, columnIndices, columnCount);

        Accelerator accelerator = context.Accelerator;
        MemoryBuffer1D<int, Stride1D.Dense> pointers = accelerator.Allocate1D(rowPointers.ToArray());
        MemoryBuffer1D<int, Stride1D.Dense> columns = accelerator.Allocate1D(columnIndices.ToArray());
        MemoryBuffer1D<double, Stride1D.Dense> stored = accelerator.Allocate1D(values.ToArray());
        return new DeviceSparseMatrix(pointers, columns, stored, rowCount, columnCount);
    }

    /// <summary>Throws unless the offsets and the columns stay inside the arrays the kernel reads.</summary>
    /// <remarks>
    /// The kernel indexes device memory with these values unchecked, so a pointer that starts
    /// past zero or steps back, or a column outside the matrix, reads another buffer's memory
    /// rather than failing (#898). One pass over each array, paid once per upload.
    /// </remarks>
    private static void RefuseMalformedStructure(
        ReadOnlySpan<int> rowPointers, ReadOnlySpan<int> columnIndices, int columnCount)
    {
        if (rowPointers[0] != 0)
        {
            throw new ArgumentException(
                $"rowPointers starts at {rowPointers[0]}, not 0.", nameof(rowPointers));
        }

        for (int row = 1; row < rowPointers.Length; row++)
        {
            if (rowPointers[row] < rowPointers[row - 1])
            {
                throw new ArgumentException(
                    $"rowPointers decreases from {rowPointers[row - 1]} to {rowPointers[row]} at row {row - 1}.",
                    nameof(rowPointers));
            }
        }

        for (int at = 0; at < columnIndices.Length; at++)
        {
            if ((uint)columnIndices[at] >= (uint)columnCount)
            {
                throw new ArgumentException(
                    $"columnIndices[{at}] is {columnIndices[at]}, outside a matrix of {columnCount} columns.",
                    nameof(columnIndices));
            }
        }
    }

    /// <summary>Frees the three device buffers.</summary>
    public void Dispose()
    {
        RowPointers.Dispose();
        ColumnIndices.Dispose();
        Values.Dispose();
    }
}
