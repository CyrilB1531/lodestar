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

    /// <summary>The stored values grouped by column, each group in storage order.</summary>
    /// <remarks>
    /// One pass over the matrix for every column, where reading a column by scanning all stored
    /// values cost O(stored values x columns). Group <c>c</c> is <c>Values[Offsets[c]..Offsets[c + 1]]</c>.
    /// </remarks>
    public static (double[] Values, int[] Offsets) ByColumn(CsrMatrix matrix)
    {
        var offsets = new int[matrix.ColumnCount + 1];
        for (int i = 0; i < matrix.ColumnIndices.Length; i++)
        {
            offsets[matrix.ColumnIndices[i] + 1]++;
        }

        for (int column = 0; column < matrix.ColumnCount; column++)
        {
            offsets[column + 1] += offsets[column];
        }

        var values = new double[matrix.Values.Length];
        var next = (int[])offsets.Clone();
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            values[next[matrix.ColumnIndices[i]]++] = matrix.Values[i];
        }

        return (values, offsets);
    }

    /// <summary>One column's values, the absent zeros included, sorted into <paramref name="buffer"/>.</summary>
    /// <remarks>
    /// The buffer receives the column's stored values in storage order and zeros after them — the
    /// same sequence a scan of the whole matrix built — so the sort returns the same array.
    /// </remarks>
    public static double[] SortedColumn(double[] grouped, int[] offsets, int column, double[] buffer)
    {
        int start = offsets[column];
        int count = offsets[column + 1] - start;
        Array.Copy(grouped, start, buffer, 0, count);
        Array.Clear(buffer, count, buffer.Length - count);

        // The rest are the zeros nobody stored, and Array.Sort puts them where they belong.
        Array.Sort(buffer);
        return buffer;
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
