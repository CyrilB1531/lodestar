using Lodestar.Internal;
using Lodestar.Decomposition.Internal;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>
/// The dense kernels' faster walks, held to the loops they replaced bit for bit: each one reorders memory access
/// and never arithmetic, so an equality within a tolerance would hide exactly the regression it is meant to catch.
/// </summary>
public sealed class DenseKernelPathTests
{
    public static TheoryData<string> Blocks() => ["finite", "zero-column", "infinity", "nan"];

    [Theory]
    [MemberData(nameof(Blocks))]
    public void The_row_by_row_reflections_give_the_column_by_column_bits(string kind)
    {
        const int rows = 23;
        const int columns = 7;
        double[] block = Block(rows, columns, seed: 11);
        switch (kind)
        {
            case "zero-column":
                for (int i = 0; i < rows; i++)
                {
                    block[(i * columns) + 2] = 0.0;
                }
                break;
            case "infinity":
                block[(15 * columns) + 4] = double.PositiveInfinity;
                break;
            case "nan":
                block[(3 * columns) + 5] = double.NaN;
                break;
        }

        (double[] q, double[] r) = HouseholderQr.Decompose(block, rows, columns);
        (double[] expectedQ, double[] expectedR) = ColumnByColumnQr(block, rows, columns);

        Assert.Equal(Bits(expectedQ), Bits(q));
        Assert.Equal(Bits(expectedR), Bits(r));
    }

    [Theory]
    [InlineData(9, 31)]
    [InlineData(31, 9)]
    [InlineData(12, 12)]
    public void Overwriting_the_block_factors_it_as_the_copy_does(int rows, int columns)
    {
        double[] block = Block(rows, columns, seed: 5);

        (double[] u, double[] s, double[] vt) = JacobiSvd.Decompose(block, rows, columns);
        (double[] overU, double[] overS, double[] overVt) =
            JacobiSvd.DecomposeOverwriting((double[])block.Clone(), rows, columns);

        Assert.Equal(Bits(u), Bits(overU));
        Assert.Equal(Bits(s), Bits(overS));
        Assert.Equal(Bits(vt), Bits(overVt));
    }

    [Fact]
    public void The_element_wise_updates_give_the_scalar_bits_at_every_length()
    {
        for (int length = 0; length < 19; length++)
        {
            double[] left = Block(1, length, seed: length);
            double[] right = Block(1, length, seed: length + 100);

            double[] rotatedLeft = (double[])left.Clone();
            double[] rotatedRight = (double[])right.Clone();
            ElementWise.Rotate(rotatedLeft, rotatedRight, 0.8, -0.6);
            double[] added = (double[])left.Clone();
            ElementWise.AddScaled(added, right, 1.7);
            double[] subtracted = (double[])left.Clone();
            ElementWise.SubtractScaled(subtracted, right, 1.7);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(Bits((0.8 * left[i]) - (-0.6 * right[i])), Bits(rotatedLeft[i]));
                Assert.Equal(Bits((-0.6 * left[i]) + (0.8 * right[i])), Bits(rotatedRight[i]));
                Assert.Equal(Bits(left[i] + (1.7 * right[i])), Bits(added[i]));
                Assert.Equal(Bits(left[i] - (1.7 * right[i])), Bits(subtracted[i]));
            }
        }
    }

    // CA5394: a seeded Random draws a reproducible block, not anything security-sensitive.
#pragma warning disable CA5394
    private static double[] Block(int rows, int columns, int seed)
    {
        var random = new Random(seed);
        var block = new double[rows * columns];
        for (int i = 0; i < block.Length; i++)
        {
            block[i] = (random.NextDouble() * 2.0) - 1.0;
        }
        return block;
    }
#pragma warning restore CA5394

    private static long Bits(double value) => BitConverter.DoubleToInt64Bits(value);

    private static long[] Bits(double[] values) => [.. values.Select(BitConverter.DoubleToInt64Bits)];

    /// <summary>The QR as it was written before the reflections walked rows: every column, one at a time.</summary>
    private static (double[] Q, double[] R) ColumnByColumnQr(double[] block, int rows, int columns)
    {
        double[] work = (double[])block.Clone();
        var vectors = new double[columns][];
        var norms = new double[columns];
        for (int k = 0; k < columns; k++)
        {
            double[] v = new double[rows - k];
            double norm = 0;
            for (int i = k; i < rows; i++)
            {
                v[i - k] = work[(i * columns) + k];
                norm += v[i - k] * v[i - k];
            }
            norm = Math.Sqrt(norm);
            vectors[k] = v;
            if (norm == 0)
            {
                continue;
            }

            v[0] -= v[0] >= 0 ? -norm : norm;
            foreach (double value in v)
            {
                norms[k] += value * value;
            }
            ColumnByColumn(work, rows, columns, k, v, norms[k]);
        }

        double[] q = new double[rows * columns];
        for (int j = 0; j < columns; j++)
        {
            q[(j * columns) + j] = 1.0;
        }
        for (int k = columns - 1; k >= 0; k--)
        {
            if (norms[k] != 0)
            {
                ColumnByColumn(q, rows, columns, k, vectors[k], norms[k]);
            }
        }

        return (q, UpperTriangle(work, columns));
    }

    private static double[] UpperTriangle(double[] work, int columns)
    {
        double[] r = new double[columns * columns];
        for (int i = 0; i < columns; i++)
        {
            for (int j = i; j < columns; j++)
            {
                r[(i * columns) + j] = work[(i * columns) + j];
            }
        }
        return r;
    }

    private static void ColumnByColumn(double[] block, int rows, int columns, int from, double[] v, double normSquared)
    {
        for (int j = 0; j < columns; j++)
        {
            double dot = 0;
            for (int i = from; i < rows; i++)
            {
                dot += v[i - from] * block[(i * columns) + j];
            }
            double scale = 2.0 * dot / normSquared;
            for (int i = from; i < rows; i++)
            {
                block[(i * columns) + j] -= scale * v[i - from];
            }
        }
    }
}
