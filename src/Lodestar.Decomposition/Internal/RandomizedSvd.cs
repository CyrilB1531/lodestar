using Lodestar.Abstractions;

namespace Lodestar.Decomposition.Internal;

/// <summary>Halko's randomized SVD — the kernel every fit in this package factors through.</summary>
/// <remarks>
/// The factors come back <em>unflipped</em> and <em>untruncated</em> because the two callers agree
/// on neither: the estimator flips on the right vectors and keeps <c>k</c> of them, while NMF's
/// initialisation flips on the left ones. scikit-learn is arranged the same way — <c>flip_sign</c>
/// is a parameter of <c>randomized_svd</c> and <c>TruncatedSVD</c> declines it.
/// </remarks>
internal static class RandomizedSvd
{
    /// <summary>Factors <paramref name="matrix"/> through a thin random block.</summary>
    /// <remarks>
    /// <c>U</c> is row-major <c>matrix.RowCount × Rank</c>, <c>Vt</c> row-major
    /// <c>Rank × matrix.ColumnCount</c>, and <c>Rank</c> is <c>S.Length</c> — which falls below
    /// <c>componentCount + oversampling</c> whenever a normalizer's economic factorization
    /// narrowed the block on the way, so a caller reads it back rather than assuming it. <c>U</c>
    /// is empty unless <paramref name="leftVectors"/> asks for it: the estimator never reads it (#1232).
    /// </remarks>
    internal static (double[] U, double[] S, double[] Vt, int Rank) Compute(
        CsrMatrix matrix,
        int componentCount,
        int oversampling,
        int powerIterations,
        PowerIterationNormalizer normalizer,
        ReadOnlySpan<double> omega,
        bool leftVectors)
    {
        int rows = matrix.RowCount;
        int features = matrix.ColumnCount;
        int size = componentCount + oversampling;

        // randomized_svd's transpose="auto": a matrix with fewer rows than columns is factored
        // as its transpose, from an Ω drawn for that shape, and the factors swapped back (#1256).
        bool transposed = IsTransposed(matrix);
        int aRows = transposed ? features : rows;
        int aColumns = transposed ? rows : features;

        double[] basis = RandomizedRangeFinder.Find(
            matrix, omega, size, powerIterations, normalizer, transposed);
        int basisSize = basis.Length / aRows;

        // B = Qᵀ A, reached as (Aᵀ Q)ᵀ so the sparse matrix is never transposed.
        double[] b = DenseBlock.Transpose(
            RandomizedRangeFinder.Apply(matrix, basis, basisSize, !transposed), aColumns, basisSize);
        (double[] uhat, double[] s, double[] vt) = JacobiSvd.DecomposeOverwriting(b, basisSize, aColumns);
        int rank = s.Length;

        if (!transposed)
        {
            double[] u = leftVectors ? Product(basis, uhat, rows, basisSize, rank) : [];
            return (u, s, vt, rank);
        }

        // A = Xᵀ: X's right vectors are A's left ones, Q·Û, and X's left vectors are A's right ones.
        double[] aLeft = Product(basis, uhat, features, basisSize, rank);
        double[] xRight = DenseBlock.Transpose(aLeft, features, rank);
        double[] xLeft = leftVectors ? DenseBlock.Transpose(vt, rank, rows) : [];
        return (xLeft, s, xRight, rank);
    }

    /// <summary>Whether <c>randomized_svd(transpose="auto")</c> factors the transpose: fewer rows than columns.</summary>
    internal static bool IsTransposed(CsrMatrix matrix) => matrix.RowCount < matrix.ColumnCount;

    /// <summary>The number of rows Ω must have: the columns of the matrix factored, which is <c>Xᵀ</c> for a wide one.</summary>
    internal static int OmegaRows(CsrMatrix matrix) =>
        IsTransposed(matrix) ? matrix.RowCount : matrix.ColumnCount;

    private static double[] Product(
        double[] left, double[] right, int rows, int inner, int columns)
    {
        double[] result = new double[checked(rows * columns)];
        for (int row = 0; row < rows; row++)
        {
            int target = row * columns;
            for (int middle = 0; middle < inner; middle++)
            {
                double scale = left[(row * inner) + middle];
                int source = middle * columns;
                for (int column = 0; column < columns; column++)
                {
                    result[target + column] += scale * right[source + column];
                }
            }
        }
        return result;
    }
}
