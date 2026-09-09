using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>
/// The published QR, over the corpus the internal kernel already replays.
/// </summary>
/// <remarks>
/// No new corpus: <c>decomposition_qr.json</c> exists and <c>HouseholderQrTests</c> checks
/// the factorization itself against it. What is owed here is the public shape — that the
/// wrapper hands back the same numbers, refuses what it says it refuses, and reports the
/// dimensions a caller needs to read <c>Q</c> and <c>R</c> row-major.
/// </remarks>
public sealed class QrDecompositionTests
{
    private const double Tolerance = 1e-12;

    /// <summary>A 3 x 2 matrix with independent columns.</summary>
    private static readonly double[] Tall = [1.0, 2.0, 3.0, 4.0, 5.0, 7.0];

    [Fact]
    public void The_factors_multiply_back_to_the_matrix()
    {
        QrDecomposition qr = QrDecomposition.Householder(Tall, rowCount: 3, columnCount: 2);

        Assert.Equal(3, qr.RowCount);
        Assert.Equal(2, qr.ColumnCount);
        Assert.Equal(6, qr.Q.Count);
        Assert.Equal(4, qr.R.Count);

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 2; column++)
            {
                double product = 0.0;
                for (int k = 0; k < 2; k++)
                {
                    product += qr.Q[(row * 2) + k] * qr.R[(k * 2) + column];
                }

                Assert.Equal(Tall[(row * 2) + column], product, Tolerance);
            }
        }
    }

    /// <summary>Orthonormal columns: <c>Qᵀ Q</c> is the identity of order <c>ColumnCount</c>.</summary>
    [Fact]
    public void Q_has_orthonormal_columns()
    {
        QrDecomposition qr = QrDecomposition.Householder(Tall, rowCount: 3, columnCount: 2);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                double dot = 0.0;
                for (int row = 0; row < 3; row++)
                {
                    dot += qr.Q[(row * 2) + i] * qr.Q[(row * 2) + j];
                }

                Assert.Equal(i == j ? 1.0 : 0.0, dot, Tolerance);
            }
        }
    }

    /// <summary>R is upper triangular, which is the whole point of the factor.</summary>
    [Fact]
    public void R_is_upper_triangular()
    {
        QrDecomposition qr = QrDecomposition.Householder(Tall, rowCount: 3, columnCount: 2);

        Assert.Equal(0.0, qr.R[2], Tolerance);
    }

    [Fact]
    public void A_shape_that_is_not_a_thin_factorization_is_refused()
    {
        // More columns than rows: there is no thin QR of a wide matrix.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => QrDecomposition.Householder([1.0, 2.0, 3.0, 4.0], rowCount: 2, columnCount: 4));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => QrDecomposition.Householder(Tall, rowCount: 0, columnCount: 2));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => QrDecomposition.Householder(Tall, rowCount: 3, columnCount: 0));
    }

    [Fact]
    public void A_span_that_does_not_match_the_declared_shape_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => QrDecomposition.Householder(Tall, rowCount: 4, columnCount: 2));

        Assert.Equal("matrix", error.ParamName);
    }
}
