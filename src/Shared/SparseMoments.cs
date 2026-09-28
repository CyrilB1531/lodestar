using Lodestar.Abstractions;

namespace Lodestar.Internal;

/// <summary>Each column's mean and variance over a sparse matrix, as <c>mean_variance_axis(X, axis=0)</c> takes them.</summary>
/// <remarks>
/// scikit-learn's <c>_csr_mean_variance_axis0</c>, unweighted: the two-pass form with Chan, Golub and
/// LeVeque's correction, where <c>E[x²] − E[x]²</c> cancelled on a column with a large offset — a
/// column at <c>1e8 + 1 … 1e8 + 4</c> read <c>2</c> where the variance is <c>1.25</c> (#1228). A row
/// storing a column twice is read as the sum, as <c>ToDense</c> reads it (#1044): the reference reads
/// each copy apart there and answers a negative variance. Both callers refuse a <c>NaN</c> first, so
/// the reference's NaN-skipping path has nothing to skip here.
/// </remarks>
internal static class SparseMoments
{
    /// <summary>The per-column means and variances, over every row, the absent zeros included.</summary>
    public static (double[] Means, double[] Variances) MeanVariance(CsrMatrix matrix)
    {
        (double[] values, int[] indices, int stored) = Canonical(matrix);
        return MeanVariance(values.AsSpan(0, stored), indices.AsSpan(0, stored), matrix.RowCount, matrix.ColumnCount);
    }

    /// <summary>As <see cref="MeanVariance(CsrMatrix)"/>, for a matrix already storing each cell once.</summary>
    public static (double[] Means, double[] Variances) MeanVarianceOfConsolidated(CsrMatrix matrix) =>
        MeanVariance(matrix.Values, matrix.ColumnIndices, matrix.RowCount, matrix.ColumnCount);

    private static (double[] Means, double[] Variances) MeanVariance(
        ReadOnlySpan<double> values, ReadOnlySpan<int> indices, int rows, int columns)
    {
        var means = new double[columns];
        var counts = new int[columns];
        for (int k = 0; k < values.Length; k++)
        {
            means[indices[k]] += values[k];
            counts[indices[k]]++;
        }

        for (int column = 0; column < columns; column++)
        {
            means[column] /= rows;
        }

        var variances = new double[columns];
        var correction = new double[columns];
        for (int k = 0; k < values.Length; k++)
        {
            int column = indices[k];
            double difference = values[k] - means[column];
            correction[column] += difference;
            variances[column] += difference * difference;
        }

        for (int column = 0; column < columns; column++)
        {
            double absent = rows - counts[column];
            if (counts[column] != rows)
            {
                // Only when some zero is absent, which is what keeps it from cancelling (the reference's own test).
                correction[column] -= absent * means[column];
                variances[column] += absent * (means[column] * means[column]);
            }

            variances[column] = (variances[column] - (correction[column] * correction[column] / rows)) / rows;
        }

        return (means, variances);
    }

    /// <summary>The stored entries with each row's copies of a column summed into one, in first-seen order.</summary>
    /// <remarks>The matrix's own arrays when no row stores a column twice, which is every one this repository builds.</remarks>
    private static (double[] Values, int[] Indices, int Stored) Canonical(CsrMatrix matrix)
    {
        int[] pointers = matrix.RowPointers;
        int[] indices = matrix.ColumnIndices;
        double[] values = matrix.Values;
        var lastRow = new int[matrix.ColumnCount];
        var slot = new int[matrix.ColumnCount];
        for (int column = 0; column < lastRow.Length; column++)
        {
            lastRow[column] = -1;
        }

        double[]? summed = null;
        int[]? columns = null;
        int written = 0;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            for (int k = pointers[row]; k < pointers[row + 1]; k++)
            {
                int column = indices[k];
                if (lastRow[column] == row)
                {
                    if (summed is null)
                    {
                        // The first repeat: copy what was read so far, then keep summing into it.
                        summed = new double[pointers[matrix.RowCount]];
                        columns = new int[summed.Length];
                        Array.Copy(values, summed, written);
                        Array.Copy(indices, columns, written);
                    }

                    summed[slot[column]] += values[k];
                    continue;
                }

                lastRow[column] = row;
                slot[column] = written;
                if (summed is not null)
                {
                    summed[written] = values[k];
                    columns![written] = column;
                }

                written++;
            }
        }

        return summed is null ? (values, indices, written) : (summed, columns!, written);
    }
}
