namespace Lodestar.Fuzzy;

/// <summary>A score for every pair of two collections — what <see cref="Process.Cdist"/> answers with.</summary>
/// <remarks>
/// A type rather than a bare array because the shape is not an input here: every bulk member of
/// <c>Lodestar.Preprocessing</c> takes a <c>featureCount</c> beside its span, and a caller given
/// only an array would have to carry the column count alongside it. The storage is row-major all
/// the same, and <see cref="ToArray"/> hands it over in that layout.
/// </remarks>
public readonly struct ScoreMatrix : IEquatable<ScoreMatrix>
{
    private readonly double[] _scores;

    internal ScoreMatrix(int rows, int columns, double[] scores)
    {
        Rows = rows;
        Columns = columns;
        _scores = scores;
    }

    /// <summary>How many queries were scored — the number of rows.</summary>
    public int Rows { get; }

    /// <summary>How many choices each query was scored against — the number of columns.</summary>
    public int Columns { get; }

    /// <summary>The score of query <paramref name="row"/> against choice <paramref name="column"/>.</summary>
    /// <param name="row">The query's index.</param>
    /// <param name="column">The choice's index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Either index is outside the matrix.</exception>
    public double this[int row, int column]
    {
        get
        {
            if ((uint)row >= (uint)Rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row), row, $"The matrix has {Rows} rows.");
            }
            if ((uint)column >= (uint)Columns)
            {
                throw new ArgumentOutOfRangeException(nameof(column), column, $"The matrix has {Columns} columns.");
            }

            return _scores[(row * Columns) + column];
        }
    }

    /// <summary>One query's scores against every choice, without copying.</summary>
    /// <param name="row">The query's index.</param>
    /// <returns>A window onto the row; empty when there were no choices.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/> is outside the matrix.</exception>
    public ReadOnlySpan<double> Row(int row)
    {
        if ((uint)row >= (uint)Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(row), row, $"The matrix has {Rows} rows.");
        }

        return _scores.AsSpan(row * Columns, Columns);
    }

    /// <summary>The whole matrix, row-major — the layout every bulk member in this repository takes.</summary>
    /// <returns>A copy: the matrix is immutable and hands out no reference to its own storage.</returns>
    public double[] ToArray() => (double[])(_scores ?? []).Clone();

    /// <summary>Whether two matrices are the same one.</summary>
    /// <remarks>
    /// Reference equality on the storage, not a cell-by-cell comparison: this is a result a caller
    /// reads rather than a value they match on, and comparing two score matrices element-wise is
    /// what <see cref="Row"/> is for.
    /// </remarks>
    /// <param name="other">The matrix to compare with.</param>
    public bool Equals(ScoreMatrix other) =>
        Rows == other.Rows && Columns == other.Columns && ReferenceEquals(_scores, other._scores);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ScoreMatrix other && Equals(other);

    /// <inheritdoc/>
    /// <remarks>
    /// Hand-rolled rather than <c>HashCode.Combine</c>, which netstandard2.0 does not carry and
    /// no polyfill in this repository supplies. The shape alone: two matrices of the same shape
    /// collide, which costs a hash bucket and never a wrong answer, since
    /// <see cref="Equals(ScoreMatrix)"/> settles it on the storage.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            return (((17 * 31) + Rows) * 31) + Columns;
        }
    }

    /// <summary>Whether two matrices are the same one.</summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    public static bool operator ==(ScoreMatrix left, ScoreMatrix right) => left.Equals(right);

    /// <summary>Whether two matrices are not the same one.</summary>
    /// <param name="left">The first matrix.</param>
    /// <param name="right">The second matrix.</param>
    public static bool operator !=(ScoreMatrix left, ScoreMatrix right) => !left.Equals(right);
}
