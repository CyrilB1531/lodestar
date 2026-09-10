using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>A row-major dense block held on the accelerator between two operations.</summary>
/// <remarks>
/// The type that makes a chain a chain. A kernel returning <c>double[]</c> has already paid a
/// device-to-host copy, so the next one pays a host-to-device copy undoing it; a block produced
/// on the accelerator and consumed there crosses the bus once at each end of the chain instead,
/// and <see cref="Download"/> is where a caller says the chain is over. Decision 0102 deferred
/// this until three kernels existed, because chainability is a claim about two operations
/// sharing a residency and cannot be measured with one.
/// </remarks>
public sealed class DeviceDenseBlock : IDisposable
{
    internal MemoryBuffer1D<double, Stride1D.Dense> Buffer { get; }

    /// <summary>How many rows the block holds.</summary>
    public int RowCount { get; }

    /// <summary>How many columns each row holds.</summary>
    public int ColumnCount { get; }

    internal DeviceDenseBlock(MemoryBuffer1D<double, Stride1D.Dense> buffer, int rowCount, int columnCount)
    {
        Buffer = buffer;
        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    /// <summary>Uploads a host block to the accelerator.</summary>
    /// <param name="context">The accelerator to upload to.</param>
    /// <param name="values"><paramref name="rowCount"/> × <paramref name="columnCount"/>, row-major.</param>
    /// <param name="rowCount">How many rows the block holds.</param>
    /// <param name="columnCount">How many columns each row holds.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A dimension is below 1.</exception>
    /// <exception cref="ArgumentException"><paramref name="values"/> is not exactly the block.</exception>
    public static DeviceDenseBlock Upload(
        GpuContext context, ReadOnlySpan<double> values, int rowCount, int columnCount)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentOutOfRangeException.ThrowIfLessThan(rowCount, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(columnCount, 1);
        if (values.Length != (long)rowCount * columnCount)
        {
            throw new ArgumentException(
                $"values holds {values.Length} numbers, not {rowCount} x {columnCount}.", nameof(values));
        }

        return new DeviceDenseBlock(
            context.Accelerator.Allocate1D(values.ToArray()), rowCount, columnCount);
    }

    /// <summary>Copies the block back to the host, ending the chain.</summary>
    /// <returns><see cref="RowCount"/> × <see cref="ColumnCount"/> values, row-major.</returns>
    /// <remarks>
    /// The one device-to-host copy a chain pays, however many operations it held. Calling this
    /// between two kernels is what residency exists to avoid, and the benchmark prices it.
    /// </remarks>
    public double[] Download() => Buffer.GetAsArray1D();

    /// <summary>Frees the device memory the block holds.</summary>
    public void Dispose() => Buffer.Dispose();
}
