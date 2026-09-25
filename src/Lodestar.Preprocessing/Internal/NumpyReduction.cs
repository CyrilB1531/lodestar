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

    /// <summary><c>np.std(values)</c> along a contiguous axis, <c>ddof = 0</c>.</summary>
    public static double StandardDeviation(ReadOnlySpan<double> values)
    {
        double mean = Sum(values) / values.Length;
        Span<double> squares = values.Length <= 64 ? stackalloc double[values.Length] : new double[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            double deviation = values[i] - mean;
            squares[i] = deviation * deviation;
        }

        return Math.Sqrt(Sum(squares) / values.Length);
    }

    /// <summary><c>np.std(matrix, axis=0)</c> of a row-major matrix: each column's deviation, rows added in order.</summary>
    /// <param name="matrix">The matrix, row-major.</param>
    /// <param name="rows">How many rows.</param>
    /// <param name="columns">How many columns.</param>
    /// <param name="result">One deviation per column.</param>
    public static void ColumnStandardDeviations(ReadOnlySpan<double> matrix, int rows, int columns, Span<double> result)
    {
        if (columns == 1)
        {
            result[0] = StandardDeviation(matrix.Slice(0, rows));
            return;
        }

        for (int column = 0; column < columns; column++)
        {
            double sum = matrix[column];
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

            result[column] = Math.Sqrt(squares / rows);
        }
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

    /// <summary>numpy's <c>pairwise_sum</c>, which starts from negative zero.</summary>
    private static double Pairwise(ReadOnlySpan<double> values)
    {
        int n = values.Length;
        if (n < 8)
        {
            double total = -0.0;
            foreach (double value in values)
            {
                total += value;
            }

            return total;
        }

        if (n <= BlockSize)
        {
            Span<double> partial = stackalloc double[8];
            values.Slice(0, 8).CopyTo(partial);
            int i = 8;
            for (; i < n - (n % 8); i += 8)
            {
                for (int lane = 0; lane < 8; lane++)
                {
                    partial[lane] += values[i + lane];
                }
            }

            double result = ((partial[0] + partial[1]) + (partial[2] + partial[3]))
                + ((partial[4] + partial[5]) + (partial[6] + partial[7]));
            for (; i < n; i++)
            {
                result += values[i];
            }

            return result;
        }

        int half = n / 2;
        half -= half % 8;
        return Pairwise(values.Slice(0, half)) + Pairwise(values.Slice(half));
    }
}
