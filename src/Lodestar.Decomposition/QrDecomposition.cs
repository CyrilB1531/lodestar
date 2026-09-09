using Lodestar.Decomposition.Internal;

namespace Lodestar.Decomposition;

/// <summary>
/// A thin QR factorization of a dense matrix, by Householder reflections.
/// </summary>
/// <remarks>
/// <strong>Thin, not full</strong>: for <c>m ≥ n</c>, <see cref="Q"/> is <c>m × n</c> with
/// orthonormal columns and <see cref="R"/> is <c>n × n</c> upper triangular — what a
/// least-squares solve wants. Published narrowly under decision 0095; the LU and the
/// one-sided Jacobi SVD beside it stay internal.
/// </remarks>
public sealed class QrDecomposition
{
    private readonly double[] _q;
    private readonly double[] _r;

    private QrDecomposition(int rowCount, int columnCount, double[] q, double[] r)
    {
        RowCount = rowCount;
        ColumnCount = columnCount;
        _q = q;
        _r = r;
    }

    /// <summary>Rows of the factorized matrix.</summary>
    public int RowCount { get; }

    /// <summary>Columns of the factorized matrix, and the order of <see cref="R"/>.</summary>
    public int ColumnCount { get; }

    /// <summary>The orthonormal factor, row-major and <see cref="RowCount"/> × <see cref="ColumnCount"/>.</summary>
    public IReadOnlyList<double> Q => _q;

    /// <summary>The upper-triangular factor, row-major and <see cref="ColumnCount"/> square.</summary>
    public IReadOnlyList<double> R => _r;

    /// <summary>Factorizes a row-major matrix by Householder reflections.</summary>
    /// <param name="matrix">The matrix, row-major: <paramref name="columnCount"/> values per row.</param>
    /// <param name="rowCount">How many rows it has.</param>
    /// <param name="columnCount">How many columns it has; must not exceed <paramref name="rowCount"/>.</param>
    /// <returns>The thin factorization.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowCount"/> or <paramref name="columnCount"/> is not positive, or there are more columns than rows.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> does not hold exactly <paramref name="rowCount"/> × <paramref name="columnCount"/> values.</exception>
    /// <remarks>
    /// The signs are the reflections', not a convention: a QR is unique only up to the sign
    /// of each column, and no normalisation is applied here. A caller comparing against
    /// <c>numpy.linalg.qr</c> should compare <c>Q · R</c>, or compare column by column up to
    /// sign, rather than expecting the two to agree entry for entry.
    /// </remarks>
    public static QrDecomposition Householder(
        ReadOnlySpan<double> matrix, int rowCount, int columnCount)
    {
        Guard.NotLessThan(rowCount, 1);
        Guard.NotLessThan(columnCount, 1);
        if (columnCount > rowCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(columnCount), columnCount,
                $"A thin QR needs at least as many rows as columns, and this matrix has {rowCount}.");
        }

        if (matrix.Length != rowCount * columnCount)
        {
            throw new ArgumentException(
                $"matrix holds {matrix.Length} values, not the {rowCount} x {columnCount} its shape declares.",
                nameof(matrix));
        }

        (double[] q, double[] r) = HouseholderQr.Decompose(matrix, rowCount, columnCount);
        return new QrDecomposition(rowCount, columnCount, q, r);
    }
}
