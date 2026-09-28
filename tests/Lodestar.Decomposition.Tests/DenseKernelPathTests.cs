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

        Assert.Equal(CanonicalBits(expectedQ), CanonicalBits(q));
        Assert.Equal(CanonicalBits(expectedR), CanonicalBits(r));
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

    /// <summary>The bits, with every NaN as one: the JIT may swap an addition's operands, and x86 keeps the first NaN's sign.</summary>
    private static long[] CanonicalBits(double[] values) =>
        [.. values.Select(value => BitConverter.DoubleToInt64Bits(double.IsNaN(value) ? double.NaN : value))];

    /// <summary>The same reflectors applied column by column: dgeqr2 and dorg2r as LAPACK walks them.</summary>
    private static (double[] Q, double[] R) ColumnByColumnQr(double[] block, int rows, int columns)
    {
        double[] work = (double[])block.Clone();
        var vectors = new double[columns][];
        var taus = new double[columns];
        for (int k = 0; k < columns; k++)
        {
            double[] v = new double[rows - k];
            double sum = 0;
            for (int i = k + 1; i < rows; i++)
            {
                v[i - k] = work[(i * columns) + k];
                sum += v[i - k] * v[i - k];
            }
            v[0] = 1.0;
            vectors[k] = v;
            double norm = Math.Sqrt(sum);
            if (norm == 0)
            {
                continue;
            }

            double alpha = work[(k * columns) + k];
            double beta = -Hypotenuse(alpha, norm);
            beta = BitConverter.DoubleToInt64Bits(alpha) < 0 ? -beta : beta;
            taus[k] = (beta - alpha) / beta;
            double scale = 1.0 / (alpha - beta);
            for (int i = 1; i < v.Length; i++)
            {
                v[i] *= scale;
            }
            work[(k * columns) + k] = beta;
            ColumnByColumn(work, rows, columns, k, k + 1, v, taus[k]);
        }

        double[] q = new double[rows * columns];
        for (int k = columns - 1; k >= 0; k--)
        {
            ColumnByColumn(q, rows, columns, k, k + 1, vectors[k], taus[k]);
            q[(k * columns) + k] = 1.0 - taus[k];
            for (int i = k + 1; i < rows; i++)
            {
                q[(i * columns) + k] = -taus[k] * vectors[k][i - k];
            }
        }

        return (q, UpperTriangle(work, columns));
    }

    private static double Hypotenuse(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
        {
            return double.IsNaN(x) ? x : y;
        }

        double larger = Math.Max(Math.Abs(x), Math.Abs(y));
        double smaller = Math.Min(Math.Abs(x), Math.Abs(y));
        return smaller == 0 || double.IsInfinity(larger)
            ? larger
            : larger * Math.Sqrt(1 + ((smaller / larger) * (smaller / larger)));
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

    /// <summary>dlarf, one column at a time: v's trailing zeros and the trailing all-zero columns are skipped.</summary>
    private static void ColumnByColumn(
        double[] block, int rows, int columns, int from, int firstColumn, double[] v, double tau)
    {
        if (tau == 0)
        {
            return;
        }

        int length = rows - from;
        while (length > 0 && v[length - 1] == 0)
        {
            length--;
        }

        int last = columns;
        while (last > firstColumn && Enumerable.Range(from, length).All(i => block[(i * columns) + last - 1] == 0))
        {
            last--;
        }

        for (int j = firstColumn; j < last; j++)
        {
            double dot = 0;
            for (int i = 0; i < length; i++)
            {
                dot += v[i] * block[((from + i) * columns) + j];
            }
            double scale = tau * dot;
            for (int i = 0; i < length; i++)
            {
                block[((from + i) * columns) + j] -= v[i] * scale;
            }
        }
    }
}
