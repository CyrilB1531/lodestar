using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>A row-major embedding matrix held on the accelerator across many queries.</summary>
/// <remarks>
/// Residency is the point. A GPU package that uploads its corpus per call is slower than
/// the SIMD path it replaces, so the matrix is uploaded once and swept. This is residency
/// only — decision 0102 defers the chainable device-resident types until three kernels
/// exist, because chainability is a claim about two operations sharing a residency.
/// Rows are L2-normalized on upload by default, which is what turns cosine similarity
/// into a dot product.
/// </remarks>
public sealed class DeviceEmbeddingMatrix : IDisposable
{
    internal MemoryBuffer1D<float, Stride1D.Dense> Buffer { get; }

    /// <summary>How many rows the matrix holds.</summary>
    public int Count { get; }

    /// <summary>The embedding dimension.</summary>
    public int Dimension { get; }

    private DeviceEmbeddingMatrix(MemoryBuffer1D<float, Stride1D.Dense> buffer, int count, int dimension)
    {
        Buffer = buffer;
        Count = count;
        Dimension = dimension;
    }

    /// <summary>Uploads a row-major block of vectors to the accelerator.</summary>
    /// <param name="context">The accelerator to upload to.</param>
    /// <param name="rows">
    /// <paramref name="count"/> × <paramref name="dimension"/> values, row-major and contiguous.
    /// </param>
    /// <param name="count">How many rows the block holds.</param>
    /// <param name="dimension">How many values each row holds.</param>
    /// <param name="normalize">L2-normalize each row on upload (default true).</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> or <paramref name="dimension"/> is below 1.</exception>
    /// <exception cref="ArgumentException"><paramref name="rows"/> is not exactly the block.</exception>
    public static DeviceEmbeddingMatrix Upload(
        GpuContext context, ReadOnlySpan<float> rows, int count, int dimension, bool normalize = true)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(dimension, 1);
        if (rows.Length != (long)count * dimension)
        {
            throw new ArgumentException(
                $"rows holds {rows.Length} values, not {count} x {dimension}.", nameof(rows));
        }

        float[] staged = rows.ToArray();
        if (normalize)
        {
            NormalizeRows(staged, count, dimension);
        }

        MemoryBuffer1D<float, Stride1D.Dense> buffer = context.Accelerator.Allocate1D(staged);
        return new DeviceEmbeddingMatrix(buffer, count, dimension);
    }

    /// <summary>Scales each row to unit length, leaving a zero row alone.</summary>
    internal static void NormalizeRows(float[] values, int count, int dimension)
    {
        for (int row = 0; row < count; row++)
        {
            int start = row * dimension;
            double sum = 0.0;
            for (int i = 0; i < dimension; i++)
            {
                double v = values[start + i];
                sum += v * v;
            }

            double norm = Math.Sqrt(sum);
            // S1244: exact zero is the only norm that makes the division undefined; a
            // tolerance would leave a short-but-real vector unnormalized instead.
#pragma warning disable S1244
            if (norm == 0.0)
#pragma warning restore S1244
            {
                continue;
            }

            for (int i = 0; i < dimension; i++)
            {
                values[start + i] = (float)(values[start + i] / norm);
            }
        }
    }

    /// <summary>Frees the device memory the matrix holds.</summary>
    public void Dispose() => Buffer.Dispose();
}
