using System.Runtime.CompilerServices;

namespace Lodestar.Preprocessing.Internal;

/// <summary>numpy's float sums and standard deviations, in its own order of additions.</summary>
/// <remarks>
/// <c>StratifiedGroupKFold</c> compares standard deviations and breaks ties on them, so the last bit decides a fold.
/// A reduction along a contiguous axis is numpy's <c>pairwise_sum</c> of the whole run (eight accumulators up to 128
/// elements, halves past that); one across the rows of a matrix adds them in order, unless the matrix has one column,
/// which makes the column contiguous again. Each rule measured against numpy 2 on thousands of random draws.
/// </remarks>
internal static class NumpyReduction
{
    private const int BlockSize = 128;

    /// <summary><c>np.add.reduce</c> along a contiguous axis.</summary>
    public static double Sum(ReadOnlySpan<double> values) => Pairwise(values);

    /// <summary><c>np.var(values)</c> along a contiguous axis, <c>ddof = 0</c>, its squares in <paramref name="scratch"/>.</summary>
    public static double Variance(ReadOnlySpan<double> values, Span<double> scratch) =>
        Variance(values, scratch, Sum(values) / values.Length);

    /// <summary><see cref="Variance(ReadOnlySpan{double}, Span{double})"/> about a mean already summed, numpy's <c>arrmean</c>.</summary>
    public static double Variance(ReadOnlySpan<double> values, Span<double> scratch, double mean)
    {
        Span<double> squares = scratch.Slice(0, values.Length);
        for (int i = 0; i < values.Length; i++)
        {
            double deviation = values[i] - mean;
            squares[i] = deviation * deviation;
        }

        return Sum(squares) / values.Length;
    }

    /// <summary><c>np.std(values)</c> along a contiguous axis, <c>ddof = 0</c>.</summary>
    public static double StandardDeviation(ReadOnlySpan<double> values)
    {
        Span<double> squares = values.Length <= 64 ? stackalloc double[values.Length] : new double[values.Length];
        return Math.Sqrt(Variance(values, squares));
    }

    /// <summary><c>np.std(matrix, axis=0)</c> of a row-major matrix: each column's deviation, rows added in order.</summary>
    /// <param name="matrix">The matrix, row-major.</param>
    /// <param name="rows">How many rows.</param>
    /// <param name="columns">How many columns.</param>
    /// <param name="result">One deviation per column.</param>
    public static void ColumnStandardDeviations(ReadOnlySpan<double> matrix, int rows, int columns, Span<double> result)
    {
        // No means asked for: none allocated, as main's loop allocated none.
        ColumnMeansAndVariances(matrix, rows, columns, Span<double>.Empty, result);
        for (int column = 0; column < columns; column++)
        {
            result[column] = Math.Sqrt(result[column]);
        }
    }

    /// <summary><c>np.mean(matrix, axis=0)</c> and <c>np.var(matrix, axis=0)</c> of a row-major matrix, as <see cref="ColumnStandardDeviations"/> sums them.</summary>
    /// <param name="matrix">The matrix, row-major.</param>
    /// <param name="rows">How many rows.</param>
    /// <param name="columns">How many columns.</param>
    /// <param name="means">One mean per column, or empty where only the variances are wanted.</param>
    /// <param name="variances">One population variance per column.</param>
    public static void ColumnMeansAndVariances(
        ReadOnlySpan<double> matrix, int rows, int columns, Span<double> means, Span<double> variances)
    {
        // Not ColumnBatchSums: StratifiedGroupKFold calls this per group and fold, and its correction sum and scratch
        // made the splitter two to four times main's; the order of additions is the same.
        if (columns == 1)
        {
            ReadOnlySpan<double> column = matrix.Slice(0, rows);
            double mean = Sum(column) / rows;
            if (!means.IsEmpty)
            {
                means[0] = mean;
            }

            Span<double> squares = rows <= 64 ? stackalloc double[rows] : new double[rows];
            variances[0] = Variance(column, squares, mean);
            return;
        }

        for (int column = 0; column < columns; column++)
        {
            double sum = 0.0 + matrix[column];
            for (int row = 1; row < rows; row++)
            {
                sum += matrix[(row * columns) + column];
            }

            double mean = sum / rows;
            double first = matrix[column] - mean;
            double squares = first * first;
            for (int row = 1; row < rows; row++)
            {
                double deviation = matrix[(row * columns) + column] - mean;
                squares += deviation * deviation;
            }

            if (!means.IsEmpty)
            {
                means[column] = mean;
            }

            variances[column] = squares / rows;
        }
    }

    /// <summary>
    /// One column's three sums, read in place, as <c>_incremental_mean_and_var</c> takes them from a batch: <c>Σx</c>,
    /// then about <c>T = Σx / n</c>, <c>Σ(x − T)</c> and <c>Σ(x − T)²</c>, each pairwise as numpy sums a contiguous axis.
    /// </summary>
    private static (double Sum, double Correction, double Squares) OneColumnSums(ReadOnlySpan<double> values)
    {
        double sum = Pairwise(values);
        double mean = sum / values.Length;
        Span<double> scratch = values.Length <= 64 ? stackalloc double[values.Length] : new double[values.Length];
        for (int row = 0; row < values.Length; row++)
        {
            scratch[row] = values[row] - mean;
        }

        double correction = Pairwise(scratch);
        for (int row = 0; row < values.Length; row++)
        {
            scratch[row] *= scratch[row];
        }

        return (sum, correction, Pairwise(scratch));
    }

    /// <summary>
    /// Every column's three sums as <c>_incremental_mean_and_var</c> takes them from a batch: <c>Σx</c>, then about
    /// <c>T = Σx / n</c>, <c>Σ(x − T)</c> and <c>Σ(x − T)²</c>, each numpy's along axis 0 — pairwise over one column,
    /// row by row over several.
    /// </summary>
    public static void ColumnBatchSums(
        ReadOnlySpan<double> matrix, int rows, int columns, Span<double> sum, Span<double> correction, Span<double> squares)
    {
        if (columns == 1)
        {
            (sum[0], correction[0], squares[0]) = OneColumnSums(matrix.Slice(0, rows));
            return;
        }

        ColumnSums(matrix, rows, columns, sum);

        Span<double> centre = columns <= 64 ? stackalloc double[columns] : new double[columns];
        for (int column = 0; column < columns; column++)
        {
            centre[column] = sum[column] / rows;
        }

        for (int row = 0; row < rows; row++)
        {
            ReadOnlySpan<double> values = matrix.Slice(row * columns, columns);
            for (int column = 0; column < columns; column++)
            {
                double deviation = values[column] - centre[column];
                correction[column] = row == 0 ? 0.0 + deviation : correction[column] + deviation;
                squares[column] = row == 0 ? deviation * deviation : squares[column] + (deviation * deviation);
            }
        }
    }

    /// <summary>Every column's <c>np.sum(axis=0)</c>, in numpy's order, the matrix read once in place.</summary>
    /// <remarks>One column is the contiguous run numpy sums pairwise; several it adds row by row, from the first.</remarks>
    public static void ColumnSums(ReadOnlySpan<double> matrix, int rows, int columns, Span<double> sums)
    {
        if (columns == 1)
        {
            sums[0] = Pairwise(matrix.Slice(0, rows));
            return;
        }

        for (int column = 0; column < columns; column++)
        {
            sums[column] = 0.0 + matrix[column];
        }
        for (int row = 1; row < rows; row++)
        {
            ReadOnlySpan<double> values = matrix.Slice(row * columns, columns);
            for (int column = 0; column < columns; column++)
            {
                sums[column] += values[column];
            }
        }
    }

    /// <summary>
    /// <c>np.ma.mean(axis=0)</c>, a <c>NaN</c> masked: each column filled with 0 and summed as numpy sums it, pairwise
    /// when <paramref name="contiguous"/> and row by row in one pass over the matrix otherwise, then
    /// <c>dsum * 1. / count</c>; <c>NaN</c> where a column is all masked, its count 0.
    /// </summary>
    public static void MaskedColumnMeans(
        ReadOnlySpan<double> matrix, int rows, int columns, bool contiguous, Span<double> means, Span<int> counts)
    {
        counts.Clear();
        if (contiguous)
        {
            FilledPairwiseSums(matrix, rows, columns, means, counts);
        }
        else
        {
            FilledRowSums(matrix, rows, columns, means, counts);
        }

        for (int column = 0; column < columns; column++)
        {
            means[column] = counts[column] == 0 ? double.NaN : means[column] * 1.0 / counts[column];
        }
    }

    /// <summary>Each column filled with 0 into one buffer and summed pairwise, its present values counted.</summary>
    private static void FilledPairwiseSums(ReadOnlySpan<double> matrix, int rows, int columns, Span<double> sums, Span<int> counts)
    {
        var filled = new double[rows];
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                double value = matrix[(row * columns) + column];
                bool present = !double.IsNaN(value);
                filled[row] = present ? value : 0.0;
                counts[column] += present ? 1 : 0;
            }

            sums[column] = Pairwise(filled);
        }
    }

    /// <summary>Every column filled with 0 and added row by row from the first, in one pass, its present values counted.</summary>
    private static void FilledRowSums(ReadOnlySpan<double> matrix, int rows, int columns, Span<double> sums, Span<int> counts)
    {
        for (int row = 0; row < rows; row++)
        {
            ReadOnlySpan<double> values = matrix.Slice(row * columns, columns);
            for (int column = 0; column < columns; column++)
            {
                bool present = !double.IsNaN(values[column]);
                double filled = present ? values[column] : 0.0;
                sums[column] = row == 0 ? 0.0 + filled : sums[column] + filled;
                counts[column] += present ? 1 : 0;
            }
        }
    }

    /// <summary>
    /// <c>np.einsum("ij,ij->i", x, x)</c> over one contiguous row, as numpy's baseline <c>sum_of_products</c> kernel adds
    /// it: two lanes, four vectors a step folded last to first, the tail one vector at a time, the lanes added last.
    /// </summary>
    /// <remarks>
    /// The kernel is compiled for the baseline alone, SSE without FMA, so the order holds on every x86-64 tier; checked
    /// against numpy 2.5.3 on 3,000 rows with and without <c>NPY_DISABLE_CPU_FEATURES</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double EinsumSquares(ReadOnlySpan<double> row)
    {
        double even = 0.0;
        double odd = 0.0;
        int i = 0;
        for (; row.Length - i >= 8; i += 8)
        {
            even = (row[i] * row[i]) + ((row[i + 2] * row[i + 2]) + ((row[i + 4] * row[i + 4]) + ((row[i + 6] * row[i + 6]) + even)));
            odd = (row[i + 1] * row[i + 1]) + ((row[i + 3] * row[i + 3]) + ((row[i + 5] * row[i + 5]) + ((row[i + 7] * row[i + 7]) + odd)));
        }

        for (; row.Length - i >= 2; i += 2)
        {
            even = (row[i] * row[i]) + even;
            odd = (row[i + 1] * row[i + 1]) + odd;
        }

        // numpy pads the last vector with a zero, whose square leaves the odd lane as it was.
        if (i < row.Length)
        {
            even = (row[i] * row[i]) + even;
        }

        return 0.0 + (even + odd);
    }

    /// <summary><c>np.isclose(a, b)</c> at its defaults, <c>rtol = 1e-5</c> and <c>atol = 1e-8</c>.</summary>
    public static bool IsClose(double a, double b)
    {
        if (double.IsInfinity(a) || double.IsInfinity(b))
        {
            // numpy compares infinities for identity; the tolerance below would call any finite value close to one.
            return (double.IsPositiveInfinity(a) && double.IsPositiveInfinity(b))
                || (double.IsNegativeInfinity(a) && double.IsNegativeInfinity(b));
        }

        return Math.Abs(a - b) <= 1e-8 + (1e-5 * Math.Abs(b));
    }

    /// <summary><c>np.abs(row).sum()</c> along a contiguous axis: numpy's pairwise sum over the magnitudes, read in place.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AbsSum(ReadOnlySpan<double> values) =>
        0.0 + (values.Length <= BlockSize ? Block(values, default(Magnitude)) : Pairwise(values, default(Magnitude)));

    /// <summary>
    /// numpy's reduction along a contiguous axis: <c>pairwise_sum</c>, which starts from negative zero, added to the
    /// reduction's +0.0 identity, so a sum of nothing or of -0.0 alone is +0.0 as numpy's is.
    /// </summary>
    private static double Pairwise(ReadOnlySpan<double> values) => 0.0 + Pairwise(values, default(Identity));

    /// <summary>
    /// <see cref="Pairwise(ReadOnlySpan{double})"/> over each value as <typeparamref name="TRead"/> reads it, a struct so
    /// the read inlines: eight lanes up to 128 values, halves past that, a plain loop from -0.0 below eight.
    /// </summary>
    private static double Pairwise<TRead>(ReadOnlySpan<double> values, TRead read)
        where TRead : struct, IRead
    {
        if (values.Length <= BlockSize)
        {
            return Block(values, read);
        }

        int half = values.Length / 2;
        half -= half % 8;
        return Pairwise(values.Slice(0, half), read) + Pairwise(values.Slice(half), read);
    }

    /// <summary>One block of <see cref="Pairwise{TRead}"/>, at most 128 values: kept apart so a short row inlines it.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Block<TRead>(ReadOnlySpan<double> values, TRead read)
        where TRead : struct, IRead
    {
        int n = values.Length;
        if (n < 8)
        {
            double total = -0.0;
            foreach (double value in values)
            {
                total += read.Of(value);
            }

            return total;
        }

        double p0 = read.Of(values[0]), p1 = read.Of(values[1]), p2 = read.Of(values[2]), p3 = read.Of(values[3]);
        double p4 = read.Of(values[4]), p5 = read.Of(values[5]), p6 = read.Of(values[6]), p7 = read.Of(values[7]);
        int i = 8;
        for (; i < n - (n % 8); i += 8)
        {
            p0 += read.Of(values[i]);
            p1 += read.Of(values[i + 1]);
            p2 += read.Of(values[i + 2]);
            p3 += read.Of(values[i + 3]);
            p4 += read.Of(values[i + 4]);
            p5 += read.Of(values[i + 5]);
            p6 += read.Of(values[i + 6]);
            p7 += read.Of(values[i + 7]);
        }

        double result = ((p0 + p1) + (p2 + p3)) + ((p4 + p5) + (p6 + p7));
        for (; i < n; i++)
        {
            result += read.Of(values[i]);
        }

        return result;
    }

    /// <summary>How <see cref="Pairwise{TRead}"/> reads a value before adding it.</summary>
    private interface IRead
    {
        double Of(double value);
    }

    private readonly struct Identity : IRead
    {
        public double Of(double value) => value;
    }

    private readonly struct Magnitude : IRead
    {
        public double Of(double value) => Math.Abs(value);
    }
}
