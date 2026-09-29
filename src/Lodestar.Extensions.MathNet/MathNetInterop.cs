using Lodestar.Abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace Lodestar.Extensions.MathNet;

// This namespace ends in MathNet, so inside it that identifier binds here and not to the
// dependency's root: every reference below is unqualified, reached through the usings above.

/// <summary>
/// Converts between <see cref="CsrMatrix"/> and Math.NET Numerics' sparse matrix.
/// </summary>
/// <remarks>
/// A compressed-row matrix crosses in either direction as a copy of its three arrays,
/// rather than being rebuilt cell by cell: Math.NET's sparse storage is CSR too, and
/// exposes it. The only conversion neither side can already do for itself; the dense pair
/// is not offered because each side builds it unaided.
/// </remarks>
public static class MathNetInterop
{
    /// <summary>Converts a <see cref="CsrMatrix"/> into a Math.NET <see cref="SparseMatrix"/>.</summary>
    /// <param name="matrix">The matrix to convert.</param>
    /// <returns>A sparse matrix of the same shape holding the same values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> is not a valid CSR matrix: one built through <see cref="CsrMatrix.CreateUnchecked"/>, or whose arrays were changed after construction.</exception>
    /// <remarks>
    /// <see cref="CsrMatrix"/> promises no order among the column indices of a row, and Math.NET reaches a cell
    /// by searching the row (#1402). A matrix whose rows are already strictly increasing is copied as it is; any
    /// other is sorted here, a repeated column added in stored order as <see cref="CsrMatrix.ToDense"/> adds it,
    /// because Math.NET's compressed-row factory sorts with an unstable sort past 16 entries (#1548).
    /// </remarks>
    public static SparseMatrix ToSparseMatrix(CsrMatrix matrix)
    {
        Guard.NotNull(matrix);

        if (CheckStructure(matrix))
        {
            return FromCompressedRows(
                matrix.RowCount,
                matrix.ColumnCount,
                Copy(matrix.Values, matrix.NonZeroCount),
                Copy(matrix.ColumnIndices, matrix.NonZeroCount),
                matrix.RowPointers);
        }

        (double[] values, int[] columns, int[] pointers) = SortRows(matrix);
        return FromCompressedRows(matrix.RowCount, matrix.ColumnCount, values, columns, pointers);
    }

    /// <summary>
    /// A Math.NET sparse matrix over arrays whose rows already increase strictly, taken as they are: the
    /// compressed-row factory would copy and sort them again (#1548).
    /// </summary>
    /// <remarks>
    /// <c>ColumnIndices</c> and <c>Values</c> are public fields Math.NET documents, and <c>RowPointers</c> is the
    /// array of <c>rows + 1</c> it allocates, filled here; the two arrays passed must belong to no one else.
    /// </remarks>
    private static SparseMatrix FromCompressedRows(int rows, int columnCount, double[] values, int[] columns, int[] pointers)
    {
        var result = new SparseMatrix(rows, columnCount);
        var storage = (SparseCompressedRowMatrixStorage<double>)result.Storage;
        Array.Copy(pointers, storage.RowPointers, rows + 1);
        storage.ColumnIndices = columns;
        storage.Values = values;
        return result;
    }

    /// <summary>
    /// Refuses a matrix the <see cref="CsrMatrix"/> constructor would have refused, and says whether every row is
    /// already strictly increasing.
    /// </summary>
    /// <remarks>
    /// Math.NET answers an inconsistent matrix with a plain <see cref="Exception"/> and accepts a column outside
    /// the matrix silently (#1549), so the constructor's checks are repeated here, in the same order.
    /// </remarks>
    private static bool CheckStructure(CsrMatrix matrix)
    {
        int[] pointers = matrix.RowPointers;
        int[] columns = matrix.ColumnIndices;
        if (pointers[0] != 0 || pointers[matrix.RowCount] != matrix.NonZeroCount)
        {
            throw new ArgumentException(
                $"The row pointers must run from 0 to the {matrix.NonZeroCount} stored values, but run from {pointers[0]} to {pointers[matrix.RowCount]}.",
                nameof(matrix));
        }

        // Every pointer is checked before any column is read, so a middle one past the end cannot index out.
        for (int row = 0; row < matrix.RowCount; row++)
        {
            if (pointers[row + 1] < pointers[row])
            {
                throw new ArgumentException($"The row pointers decrease at row {row}.", nameof(matrix));
            }
        }

        bool ordered = true;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            for (int k = pointers[row]; k < pointers[row + 1]; k++)
            {
                if ((uint)columns[k] >= (uint)matrix.ColumnCount)
                {
                    throw new ArgumentException(
                        $"Stored value {k} sits in column {columns[k]}, outside [0, {matrix.ColumnCount}).", nameof(matrix));
                }

                ordered &= k == pointers[row] || columns[k - 1] < columns[k];
            }
        }

        return ordered;
    }

    /// <summary>Each row sorted by column, a repeated column summed in stored order, as <c>ToDense</c> sums it.</summary>
    private static (double[] Values, int[] Columns, int[] Pointers) SortRows(CsrMatrix matrix)
    {
        int[] sourcePointers = matrix.RowPointers;
        var values = new double[matrix.NonZeroCount];
        var columns = new int[matrix.NonZeroCount];
        var pointers = new int[matrix.RowCount + 1];
        long[]? keys = null;
        int written = 0;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            int start = sourcePointers[row];
            int length = sourcePointers[row + 1] - start;

            // An unstable sort suffices while no column repeats, the common case; a row where one does is redone stably.
            Array.Copy(matrix.ColumnIndices, start, columns, written, length);
            Array.Copy(matrix.Values, start, values, written, length);
            Array.Sort(columns, values, written, length);
            if (HasRepeat(columns, written, length))
            {
                keys ??= new long[matrix.NonZeroCount];
                written = SortRowStably(matrix, start, length, keys, values, columns, written);
            }
            else
            {
                written += length;
            }

            pointers[row + 1] = written;
        }

        // Trimmed only when repeats were merged: two copies of every entry otherwise bought nothing.
        return written == values.Length
            ? (values, columns, pointers)
            : (Copy(values, written), Copy(columns, written), pointers);
    }

    /// <summary>Whether a sorted run of columns holds one twice.</summary>
    private static bool HasRepeat(int[] columns, int start, int length)
    {
        for (int k = start + 1; k < start + length; k++)
        {
            if (columns[k] == columns[k - 1])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>One row sorted stably by column and written from <paramref name="written"/>, repeats summed in stored order.</summary>
    private static int SortRowStably(
        CsrMatrix matrix, int start, int length, long[] keys, double[] values, int[] columns, int written)
    {
        // Column in the high half, stored position in the low: sorting the keys is a stable sort by column.
        for (int i = 0; i < length; i++)
        {
            keys[i] = ((long)matrix.ColumnIndices[start + i] << 32) | (uint)i;
        }

        Array.Sort(keys, 0, length);
        return MergeRow(matrix.Values, start, keys, length, values, columns, written);
    }

    /// <summary>Writes one sorted row, a column repeated in <paramref name="keys"/> as one stored sum.</summary>
    private static int MergeRow(double[] source, int start, long[] keys, int length, double[] values, int[] columns, int written)
    {
        int i = 0;
        while (i < length)
        {
            int column = (int)(keys[i] >> 32);

            // From the first value, not 0.0: a lone -0.0 stays -0.0, as it does on the ordered path.
            double sum = source[start + (int)(uint)keys[i++]];
            for (; i < length && (int)(keys[i] >> 32) == column; i++)
            {
                sum += source[start + (int)(uint)keys[i]];
            }

            values[written] = sum;
            columns[written] = column;
            written++;
        }

        return written;
    }

    /// <summary>Converts a Math.NET matrix into a <see cref="CsrMatrix"/>.</summary>
    /// <param name="matrix">The matrix to convert; sparse or dense.</param>
    /// <returns>A CSR matrix of the same shape holding the same values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> has more rows than one array of row pointers holds, or its storage — a caller's own — reports an entry outside the matrix, more non-zero values than one array holds, or different entries on two walks.</exception>
    /// <remarks>
    /// A matrix already stored in compressed-row form hands over its three arrays, copied
    /// so neither side can mutate the other's. A dense or diagonal matrix is read from its
    /// own array, so a diagonal matrix costs its diagonal and a dense one its cells; any
    /// other storage is walked over what it stores.
    /// </remarks>
    public static CsrMatrix ToCsrMatrix(Matrix<double> matrix)
    {
        Guard.NotNull(matrix);

        // A diagonal matrix of int.MaxValue rows is cheap to build and wrapped rows + 1 to int.MinValue (#1529).
        if (matrix.RowCount >= TableLength.MaxLength)
        {
            throw new ArgumentException(
                $"A matrix of {matrix.RowCount} rows needs more row pointers than one array holds.", nameof(matrix));
        }

        if (matrix.Storage is SparseCompressedRowMatrixStorage<double> csr)
        {
            return new CsrMatrix(
                matrix.RowCount,
                matrix.ColumnCount,
                Copy(csr.Values, csr.ValueCount),
                Copy(csr.ColumnIndices, csr.ValueCount),
                Copy(csr.RowPointers, matrix.RowCount + 1));
        }

        return matrix.Storage switch
        {
            DenseColumnMajorMatrixStorage<double> dense => FromDense(dense),
            DiagonalMatrixStorage<double> diagonal => FromDiagonal(diagonal),
            _ => FromAnyStorage(matrix),
        };
    }

    /// <summary>Collects a dense matrix's non-zero entries straight from its column-major array.</summary>
    /// <remarks>
    /// Counted per row in one pass and placed in a second, columns in order, so each row comes out sorted: the
    /// enumerator the other storages take cost a dense 1,000-square matrix twice the reads of its own array.
    /// </remarks>
    private static CsrMatrix FromDense(DenseColumnMajorMatrixStorage<double> dense)
    {
        int rows = dense.RowCount;
        int columnCount = dense.ColumnCount;
        double[] data = dense.Data;
        var pointers = new int[rows + 1];
        for (int column = 0; column < columnCount; column++)
        {
            int offset = column * rows;
            for (int row = 0; row < rows; row++)
            {
                if (IsStored(data[offset + row]))
                {
                    pointers[row + 1]++;
                }
            }
        }

        for (int row = 0; row < rows; row++)
        {
            pointers[row + 1] += pointers[row];
        }

        var values = new double[pointers[rows]];
        var columns = new int[pointers[rows]];
        var next = (int[])pointers.Clone();
        for (int column = 0; column < columnCount; column++)
        {
            int offset = column * rows;
            for (int row = 0; row < rows; row++)
            {
                double value = data[offset + row];
                if (IsStored(value))
                {
                    int at = next[row]++;
                    values[at] = value;
                    columns[at] = column;
                }
            }
        }

        return new CsrMatrix(rows, columnCount, values, columns, pointers);
    }

    /// <summary>Collects a diagonal matrix's non-zero entries straight from its array of diagonal values.</summary>
    /// <remarks>
    /// Math.NET's own storage, so its entries need none of the checks a caller's storage gets: a 100k-square
    /// diagonal matrix costs its 1e5 entries, not 1e10 reads (#1220), and no enumerator either (#1547).
    /// </remarks>
    private static CsrMatrix FromDiagonal(DiagonalMatrixStorage<double> diagonal)
    {
        int rows = diagonal.RowCount;
        double[] data = diagonal.Data;
        var pointers = new int[rows + 1];
        for (int row = 0; row < rows; row++)
        {
            pointers[row + 1] = pointers[row] + (row < data.Length && IsStored(data[row]) ? 1 : 0);
        }

        var values = new double[pointers[rows]];
        var columns = new int[pointers[rows]];
        for (int row = 0; row < rows; row++)
        {
            if (pointers[row + 1] > pointers[row])
            {
                values[pointers[row]] = data[row];
                columns[pointers[row]] = row;
            }
        }

        return new CsrMatrix(rows, diagonal.ColumnCount, values, columns, pointers);
    }

    /// <summary>Collects the non-zero entries of a matrix this package cannot read directly.</summary>
    /// <remarks>
    /// Walks what the storage stores, <c>EnumerateIndexed(Zeros.AllowSkip)</c>, twice — once to count each row, once
    /// to place it — rather than reading every cell. Any storage but Math.NET's three lands here, a caller's own
    /// subclass included (#1530): each entry is placed by its row, so the order a storage enumerates in does not
    /// matter. A caller's storage is not trusted: an entry outside the matrix, more entries than one array holds,
    /// or a second walk disagreeing with the first is refused (#1547).
    /// </remarks>
    private static CsrMatrix FromAnyStorage(Matrix<double> matrix)
    {
        int rows = matrix.RowCount;
        var pointers = new int[rows + 1];
        long total = 0;
        foreach ((int row, int column, double value) in matrix.EnumerateIndexed(Zeros.AllowSkip))
        {
            if (IsStored(value))
            {
                RequireInside(matrix, row, column);
                if (++total >= TableLength.MaxLength)
                {
                    throw new ArgumentException(
                        $"The matrix stores more non-zero values than one array holds ({TableLength.MaxLength - 1}).", nameof(matrix));
                }

                pointers[row + 1]++;
            }
        }

        for (int row = 0; row < rows; row++)
        {
            pointers[row + 1] += pointers[row];
        }

        var values = new double[pointers[rows]];
        var columns = new int[pointers[rows]];
        var next = (int[])pointers.Clone();
        foreach ((int row, int column, double value) in matrix.EnumerateIndexed(Zeros.AllowSkip))
        {
            if (IsStored(value))
            {
                RequireInside(matrix, row, column);
                if (next[row] == pointers[row + 1])
                {
                    throw InconsistentWalk(nameof(matrix));
                }

                int at = next[row]++;
                values[at] = value;
                columns[at] = column;
            }
        }

        for (int row = 0; row < rows; row++)
        {
            if (next[row] != pointers[row + 1])
            {
                throw InconsistentWalk(nameof(matrix));
            }
        }

        return new CsrMatrix(rows, matrix.ColumnCount, values, columns, pointers);
    }

    /// <summary>Refuses an entry a caller's storage reports outside its own shape.</summary>
    private static void RequireInside(Matrix<double> matrix, int row, int column)
    {
        if ((uint)row >= (uint)matrix.RowCount || (uint)column >= (uint)matrix.ColumnCount)
        {
            throw new ArgumentException(
                $"The storage reports an entry at ({row}, {column}), outside its {matrix.RowCount} × {matrix.ColumnCount} shape.",
                nameof(matrix));
        }
    }

    private static ArgumentException InconsistentWalk(string paramName) =>
        new("The storage enumerated different non-zero entries on two walks.", paramName);

    /// <summary>Whether CSR keeps the value: a stored zero carries no information, a NaN does.</summary>
    // S1244: "differs from zero" is the structural question here rather than a comparison of measurements.
#pragma warning disable S1244
    private static bool IsStored(double value) => value != 0.0;
#pragma warning restore S1244

    /// <summary>The first <paramref name="length"/> entries, so neither side shares an array.</summary>
    private static T[] Copy<T>(T[] source, int length)
    {
        var copy = new T[length];
        Array.Copy(source, copy, length);
        return copy;
    }
}
