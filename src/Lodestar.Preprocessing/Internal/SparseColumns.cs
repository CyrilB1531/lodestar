using Lodestar.Abstractions;

namespace Lodestar.Preprocessing.Internal;

/// <summary>Per-column statistics read off a <see cref="CsrMatrix"/> without materialising it.</summary>
/// <remarks>
/// The zeros are the point: a column's absent entries are zeros, and every statistic here accounts
/// for them by counting rather than by visiting them. That is what makes the sparse path worth
/// having — and what makes centring impossible on it, since subtracting a mean would turn every one
/// of those absent zeros into a stored value.
/// </remarks>
internal static class SparseColumns
{
    /// <summary>Each column's sum of values and sum of squares, the zeros contributing nothing to either.</summary>
    public static (double[] Sums, double[] Squares) Moments(CsrMatrix matrix)
    {
        var sums = new double[matrix.ColumnCount];
        var squares = new double[matrix.ColumnCount];
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            int column = matrix.ColumnIndices[i];
            double value = matrix.Values[i];
            sums[column] += value;
            squares[column] += value * value;
        }

        return (sums, squares);
    }

    /// <summary>Each column's largest absolute value, which is zero for a column with no stored entry.</summary>
    public static double[] MaximumAbsolute(CsrMatrix matrix)
    {
        var maxima = new double[matrix.ColumnCount];
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            int column = matrix.ColumnIndices[i];
            double value = Math.Abs(matrix.Values[i]);
            if (value > maxima[column])
            {
                maxima[column] = value;
            }
        }

        return maxima;
    }

    /// <summary>One column's values, the absent zeros included, sorted — what a percentile needs.</summary>
    public static double[] SortedColumn(CsrMatrix matrix, int column)
    {
        var values = new double[matrix.RowCount];
        int next = 0;
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            if (matrix.ColumnIndices[i] == column)
            {
                values[next++] = matrix.Values[i];
            }
        }

        // The rest are the zeros nobody stored, and Array.Sort puts them where they belong.
        Array.Sort(values);
        return values;
    }

    /// <summary>Refuses a matrix carrying a value no statistic can answer for.</summary>
    public static void RequireFinite(CsrMatrix matrix, string parameterName)
    {
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            if (double.IsNaN(matrix.Values[i]) || double.IsInfinity(matrix.Values[i]))
            {
                throw new ArgumentException(
                    $"{parameterName} stores {matrix.Values[i]} at position {i}. The dense overloads refuse a "
                    + "non-finite value for the same reason: a percentile over a sorted column cannot answer for it.",
                    parameterName);
            }
        }
    }
}
