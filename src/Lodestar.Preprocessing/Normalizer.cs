using Lodestar.Abstractions;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Scales each row to unit norm, at <c>sklearn.preprocessing.Normalizer</c> parity.
/// </summary>
/// <remarks>
/// The one member of this package that fits nothing: every row is scaled by its own norm, so
/// there is no statistic to learn and no state to carry — the reference is a transformer all the
/// same, with a <c>fit</c> that only validates, so this is static.
/// Rows, not features. Every other member here scales a column; this one scales a row, which is
/// what a distance or a dot product between two rows reads afterwards.
/// </remarks>
public static class Normalizer
{
    /// <summary>Scales each row of a row-major matrix to unit norm.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="norm">Which norm each row is scaled by.</param>
    /// <returns>A new matrix of the same shape; the input is never written to.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or <paramref name="norm"/> is not a defined value.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public static double[] Transform(
        ReadOnlySpan<double> samples, int featureCount, RowNorm norm = RowNorm.L2)
    {
        Guard.NotLessThan(featureCount, 1);
        RequireDefined(norm);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var scaled = new double[samples.Length];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * featureCount;
            double length = Length(samples.Slice(start, featureCount), norm);

            // A row of zeros has no direction to preserve, so the reference leaves it alone
            // rather than dividing by zero. S1244: only an exact zero is that row.
#pragma warning disable S1244
            double divisor = length == 0.0 ? 1.0 : length;
#pragma warning restore S1244
            for (int feature = 0; feature < featureCount; feature++)
            {
                scaled[start + feature] = samples[start + feature] / divisor;
            }
        }

        return scaled;
    }

    /// <summary>Scales each row of a sparse matrix to unit norm.</summary>
    /// <param name="matrix">The matrix to read; never written to.</param>
    /// <param name="norm">Which norm each row is scaled by.</param>
    /// <returns>A new matrix with the same stored positions and scaled values.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="norm"/> is not a defined value.</exception>
    /// <remarks>
    /// The stored positions are kept rather than recomputed: scaling a row by a positive number
    /// turns no stored value into a zero that was not one already, and the reference keeps its
    /// sparsity the same way.
    /// </remarks>
    public static CsrMatrix Transform(CsrMatrix matrix, RowNorm norm = RowNorm.L2)
    {
        Guard.NotNull(matrix);
        RequireDefined(norm);

        double[] values = new double[matrix.Values.Length];
        for (int row = 0; row < matrix.RowCount; row++)
        {
            int start = matrix.RowPointers[row];
            int end = matrix.RowPointers[row + 1];
            double length = Length(matrix.Values.AsSpan(start, end - start), norm);

            // S1244: as above, only a row whose every stored value is an exact zero.
#pragma warning disable S1244
            double divisor = length == 0.0 ? 1.0 : length;
#pragma warning restore S1244
            for (int i = start; i < end; i++)
            {
                values[i] = matrix.Values[i] / divisor;
            }
        }

        return new CsrMatrix(
            matrix.RowCount,
            matrix.ColumnCount,
            values,
            (int[])matrix.ColumnIndices.Clone(),
            (int[])matrix.RowPointers.Clone());
    }

    private static double Length(ReadOnlySpan<double> row, RowNorm norm)
    {
        double total = 0.0;
        switch (norm)
        {
            case RowNorm.L1:
                for (int i = 0; i < row.Length; i++)
                {
                    total += Math.Abs(row[i]);
                }

                return total;

            case RowNorm.Max:
                for (int i = 0; i < row.Length; i++)
                {
                    double magnitude = Math.Abs(row[i]);
                    if (magnitude > total)
                    {
                        total = magnitude;
                    }
                }

                return total;

            default:
                for (int i = 0; i < row.Length; i++)
                {
                    total += row[i] * row[i];
                }

                return Math.Sqrt(total);
        }
    }

    private static void RequireDefined(RowNorm norm)
    {
        if (norm is not (RowNorm.L1 or RowNorm.L2 or RowNorm.Max))
        {
            throw new ArgumentOutOfRangeException(nameof(norm), norm, "Not a defined row norm.");
        }
    }
}
