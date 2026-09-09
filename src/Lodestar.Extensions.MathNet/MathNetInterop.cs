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
/// Both directions move the three compressed-row arrays in one pass over the stored
/// values, rather than rebuilding a matrix cell by cell: Math.NET's storage is CSR too,
/// and exposes it. The whole package, and the only conversion neither side can already
/// do for itself — decision 0086 records why the dense pair is not offered.
/// </remarks>
public static class MathNetInterop
{
    /// <summary>Converts a <see cref="CsrMatrix"/> into a Math.NET <see cref="SparseMatrix"/>.</summary>
    /// <param name="matrix">The matrix to convert.</param>
    /// <returns>A sparse matrix of the same shape holding the same values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <remarks>
    /// <see cref="CsrMatrix"/> promises no order among the column indices of a row, and
    /// Math.NET reaches a cell by searching the row, so the rows are sorted here and
    /// duplicate columns are added together. A matrix already in that shape — everything
    /// this repository's vectorizers produce — takes a single comparison per stored value
    /// and no allocation beyond the copy Math.NET makes anyway.
    /// </remarks>
    public static SparseMatrix ToSparseMatrix(CsrMatrix matrix)
    {
        Guard.NotNull(matrix);

        double[] values = matrix.Values;
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;

        if (!IsCanonical(columns, pointers))
        {
            Canonicalize(matrix, out values, out columns, out pointers);
        }

        SparseCompressedRowMatrixStorage<double> storage =
            SparseCompressedRowMatrixStorage<double>.OfCompressedSparseRowFormat(
                matrix.RowCount, matrix.ColumnCount, values.Length, pointers, columns, values);

        return new SparseMatrix(storage);
    }

    /// <summary>Converts a Math.NET matrix into a <see cref="CsrMatrix"/>.</summary>
    /// <param name="matrix">The matrix to convert; sparse or dense.</param>
    /// <returns>A CSR matrix of the same shape holding the same values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <remarks>
    /// A matrix already stored in compressed-row form hands over its three arrays, copied
    /// so neither side can mutate the other's. Any other storage — dense, or
    /// compressed-column — is walked row by row and its non-zero entries collected, which
    /// is the honest cost of changing layout rather than a defect of this path.
    /// </remarks>
    public static CsrMatrix ToCsrMatrix(Matrix<double> matrix)
    {
        Guard.NotNull(matrix);

        if (matrix.Storage is SparseCompressedRowMatrixStorage<double> csr)
        {
            return new CsrMatrix(
                matrix.RowCount,
                matrix.ColumnCount,
                Copy(csr.Values, csr.ValueCount),
                Copy(csr.ColumnIndices, csr.ValueCount),
                Copy(csr.RowPointers, matrix.RowCount + 1));
        }

        return FromAnyStorage(matrix);
    }

    /// <summary>Whether the rows are already sorted by column with no duplicate.</summary>
    private static bool IsCanonical(int[] columns, int[] pointers)
    {
        for (int row = 0; row + 1 < pointers.Length; row++)
        {
            for (int k = pointers[row] + 1; k < pointers[row + 1]; k++)
            {
                if (columns[k] <= columns[k - 1])
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>Sorts each row by column index and adds duplicate columns together.</summary>
    private static void Canonicalize(
        CsrMatrix matrix, out double[] values, out int[] columns, out int[] pointers)
    {
        int rows = matrix.RowCount;
        var sortedValues = new List<double>(matrix.NonZeroCount);
        var sortedColumns = new List<int>(matrix.NonZeroCount);
        pointers = new int[rows + 1];

        for (int row = 0; row < rows; row++)
        {
            int start = matrix.RowPointers[row];
            int end = matrix.RowPointers[row + 1];

            var entries = new (int Column, double Value)[end - start];
            for (int k = start; k < end; k++)
            {
                entries[k - start] = (matrix.ColumnIndices[k], matrix.Values[k]);
            }

            Array.Sort(entries, static (left, right) => left.Column.CompareTo(right.Column));

            foreach ((int column, double value) in entries)
            {
                if (sortedColumns.Count > pointers[row] && sortedColumns[sortedColumns.Count - 1] == column)
                {
                    sortedValues[sortedValues.Count - 1] += value;
                    continue;
                }

                sortedColumns.Add(column);
                sortedValues.Add(value);
            }

            pointers[row + 1] = sortedColumns.Count;
        }

        values = sortedValues.ToArray();
        columns = sortedColumns.ToArray();
    }

    /// <summary>Collects the non-zero entries of a matrix this package cannot read directly.</summary>
    private static CsrMatrix FromAnyStorage(Matrix<double> matrix)
    {
        int rows = matrix.RowCount;
        var values = new List<double>();
        var columns = new List<int>();
        var pointers = new int[rows + 1];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < matrix.ColumnCount; column++)
            {
                double value = matrix.At(row, column);

                // S1244: a stored zero carries no information in CSR, and "differs from zero"
                // is the structural question here rather than a comparison of measurements.
#pragma warning disable S1244
                if (value != 0.0)
#pragma warning restore S1244
                {
                    values.Add(value);
                    columns.Add(column);
                }
            }

            pointers[row + 1] = values.Count;
        }

        return new CsrMatrix(rows, matrix.ColumnCount, values.ToArray(), columns.ToArray(), pointers);
    }

    /// <summary>The first <paramref name="length"/> entries, so neither side shares an array.</summary>
    private static T[] Copy<T>(T[] source, int length)
    {
        var copy = new T[length];
        Array.Copy(source, copy, length);
        return copy;
    }
}
