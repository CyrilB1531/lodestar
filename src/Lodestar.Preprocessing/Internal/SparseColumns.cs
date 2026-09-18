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
        CsrMatrix read = Consolidated(matrix);
        var sums = new double[read.ColumnCount];
        var squares = new double[read.ColumnCount];
        for (int i = 0; i < read.Values.Length; i++)
        {
            int column = read.ColumnIndices[i];
            double value = read.Values[i];
            sums[column] += value;
            squares[column] += value * value;
        }

        return (sums, squares);
    }

    /// <summary>The matrix with each row storing every column once, a row's duplicates summed.</summary>
    /// <remarks>
    /// Returns <paramref name="matrix"/> itself when no row stores a column twice, which is every
    /// matrix this repository builds. The statistics here are scikit-learn's, read through scipy
    /// reductions that call <c>sum_duplicates()</c> first, so they must read what
    /// <see cref="CsrMatrix.ToDense"/> reads rather than each stored entry on its own (#1044).
    /// </remarks>
    public static CsrMatrix Consolidated(CsrMatrix matrix)
    {
        var mark = new int[matrix.ColumnCount];
        ResetMarks(mark);
        if (!HasDuplicate(matrix, mark))
        {
            return matrix;
        }

        ResetMarks(mark);
        var totals = new double[matrix.ColumnCount];
        var values = new List<double>(matrix.Values.Length);
        var columns = new List<int>(matrix.Values.Length);
        var pointers = new int[matrix.RowCount + 1];
        var present = new List<int>();

        for (int row = 0; row < matrix.RowCount; row++)
        {
            present.Clear();
            for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
            {
                int column = matrix.ColumnIndices[i];
                if (mark[column] != row)
                {
                    mark[column] = row;
                    totals[column] = 0.0;
                    present.Add(column);
                }

                totals[column] += matrix.Values[i];
            }

            present.Sort();
            foreach (int column in present)
            {
                columns.Add(column);
                values.Add(totals[column]);
            }

            pointers[row + 1] = values.Count;
        }

        return new CsrMatrix(matrix.RowCount, matrix.ColumnCount, [.. values], [.. columns], pointers);
    }

    /// <summary>Marks every column as seen in no row, which row 0 would otherwise look like.</summary>
    private static void ResetMarks(int[] mark)
    {
        for (int column = 0; column < mark.Length; column++)
        {
            mark[column] = -1;
        }
    }

    /// <summary>Whether any row stores one column twice, in one pass and with no allocation of its own.</summary>
    private static bool HasDuplicate(CsrMatrix matrix, int[] mark)
    {
        for (int row = 0; row < matrix.RowCount; row++)
        {
            for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
            {
                int column = matrix.ColumnIndices[i];
                if (mark[column] == row)
                {
                    return true;
                }

                mark[column] = row;
            }
        }

        return false;
    }

    /// <summary>Each column's largest absolute value, which is zero for a column with no stored entry.</summary>
    public static double[] MaximumAbsolute(CsrMatrix matrix)
    {
        CsrMatrix read = Consolidated(matrix);
        var maxima = new double[read.ColumnCount];
        for (int i = 0; i < read.Values.Length; i++)
        {
            int column = read.ColumnIndices[i];
            double value = Math.Abs(read.Values[i]);
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
        CsrMatrix read = Consolidated(matrix);
        var offsets = new int[read.ColumnCount + 1];
        for (int i = 0; i < read.ColumnIndices.Length; i++)
        {
            offsets[read.ColumnIndices[i] + 1]++;
        }

        for (int column = 0; column < read.ColumnCount; column++)
        {
            offsets[column + 1] += offsets[column];
        }

        var values = new double[read.Values.Length];
        var next = (int[])offsets.Clone();
        for (int i = 0; i < read.Values.Length; i++)
        {
            values[next[read.ColumnIndices[i]]++] = read.Values[i];
        }

        return (values, offsets);
    }

    /// <summary>One column's values, the absent zeros included, sorted into <paramref name="buffer"/>.</summary>
    /// <remarks>
    /// The buffer receives the column's stored values in storage order and zeros after them — the
    /// same sequence a scan of the whole matrix built — so the sort returns the same array. It is
    /// one slot per row, which fits because <see cref="ByColumn"/> consolidated the matrix first:
    /// a column is then stored at most once per row, so it cannot hold more values than there are
    /// rows, which is what used to reach the caller as an <c>Array.Copy</c> failure (#1045).
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

    /// <summary>Refuses a matrix carrying a value the caller cannot answer for.</summary>
    /// <param name="matrix">The matrix to read.</param>
    /// <param name="parameterName">The public parameter it arrived as.</param>
    /// <param name="because">Why that value is refused, which a fit and a transform say differently (#989).</param>
    public static void RequireFinite(CsrMatrix matrix, string parameterName, string because)
    {
        for (int i = 0; i < matrix.Values.Length; i++)
        {
            if (double.IsNaN(matrix.Values[i]) || double.IsInfinity(matrix.Values[i]))
            {
                throw new ArgumentException(
                    $"{parameterName} stores {matrix.Values[i]} at position {i}. {because}",
                    parameterName);
            }
        }

        // Two finite entries of one cell can sum past double.MaxValue, and every statistic reads the sum (#1101).
        CsrMatrix read = Consolidated(matrix);
        if (ReferenceEquals(read, matrix))
        {
            return;
        }

        for (int row = 0; row < read.RowCount; row++)
        {
            for (int i = read.RowPointers[row]; i < read.RowPointers[row + 1]; i++)
            {
                if (double.IsInfinity(read.Values[i]))
                {
                    throw new ArgumentException(
                        $"{parameterName} stores column {read.ColumnIndices[i]} more than once in row {row}, "
                        + $"and those entries sum to {read.Values[i]}. {because}",
                        parameterName);
                }
            }
        }
    }

    /// <summary>Why a fit refuses a non-finite stored value.</summary>
    public const string FitReason =
        "The dense overloads refuse a non-finite value for the same reason: a percentile over a sorted column "
        + "cannot answer for it.";

    /// <summary>Why a transform refuses one, where the reference passes a NaN through and refuses an infinity.</summary>
    public const string TransformReason =
        "The dense overloads refuse one too, and so do the three sparse ones. The reference passes a NaN "
        + "through here and refuses an infinity; docs/equivalence.md's transform row records the divergence.";

    /// <summary>A copy of <paramref name="samples"/> with every stored value divided by its column's scale.</summary>
    /// <remarks>
    /// Multiplied by <c>1 / scale</c> rather than divided, as <c>sklearn.utils.sparsefuncs.inplace_column_scale</c>
    /// is given it: <c>x · (1/s)</c> is not bit for bit <c>x / s</c>. A <see langword="null"/> scale copies (#895).
    /// </remarks>
    public static CsrMatrix Divided(CsrMatrix samples, int featureCount, IReadOnlyList<double>? scale, bool requireFinite)
    {
        double[] factors = Factors(samples, featureCount, scale, requireFinite);
        for (int i = 0; i < factors.Length; i++)
        {
            factors[i] = 1.0 / factors[i];
        }

        return MultiplyColumns(samples, factors);
    }

    /// <summary>A copy of <paramref name="samples"/> with every stored value multiplied by its column's scale.</summary>
    public static CsrMatrix Multiplied(CsrMatrix samples, int featureCount, IReadOnlyList<double>? scale, bool requireFinite) =>
        MultiplyColumns(samples, Factors(samples, featureCount, scale, requireFinite));

    /// <summary>Refuses a null matrix, and any matrix at all when the scaler subtracts a centre.</summary>
    /// <exception cref="InvalidOperationException"><paramref name="centres"/> is set.</exception>
    public static void RefuseCentring(CsrMatrix samples, bool centres, string option)
    {
        Guard.NotNull(samples);
        if (centres)
        {
            throw new InvalidOperationException(
                "This scaler centres, and a sparse matrix cannot be centred: subtracting a centre makes every "
                + $"absent zero a stored value. Fit with {option} = false.");
        }
    }

    /// <summary>The checks both directions share, then a fresh copy of the scale to multiply by, ones when there is none.</summary>
    private static double[] Factors(CsrMatrix samples, int featureCount, IReadOnlyList<double>? scale, bool requireFinite)
    {
        Guard.NotNull(samples);
        if (samples.RowCount == 0)
        {
            throw new ArgumentException(
                "samples holds no row. The dense overloads and the reference refuse an empty matrix too.",
                nameof(samples));
        }

        if (samples.ColumnCount != featureCount)
        {
            throw new ArgumentException(
                $"samples has {samples.ColumnCount} columns, but the scaler was fitted on {featureCount} features.",
                nameof(samples));
        }

        if (requireFinite)
        {
            RequireFinite(samples, nameof(samples), TransformReason);
        }

        var factors = new double[featureCount];
        for (int i = 0; i < factors.Length; i++)
        {
            factors[i] = scale is null ? 1.0 : scale[i];
        }

        return factors;
    }

    private static CsrMatrix MultiplyColumns(CsrMatrix matrix, double[] factors)
    {
        var values = new double[matrix.Values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = matrix.Values[i] * factors[matrix.ColumnIndices[i]];
        }

        return new CsrMatrix(
            matrix.RowCount, matrix.ColumnCount, values, [.. matrix.ColumnIndices], [.. matrix.RowPointers]);
    }
}
