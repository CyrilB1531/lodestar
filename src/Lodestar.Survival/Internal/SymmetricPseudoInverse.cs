namespace Lodestar.Survival.Internal;

/// <summary>The quadratic form <c>zᵀ A⁺ z</c> of a small symmetric matrix, through its eigenvectors.</summary>
/// <remarks>
/// lifelines reads the multi-group log-rank statistic through <c>numpy.linalg.pinv</c>, which drops the singular
/// values below <c>1e-15</c> times the largest. For a symmetric matrix those are the absolute eigenvalues, so the
/// cyclic Jacobi rotations below reach the same pseudo-inverse without a general SVD. The matrix is one row per
/// group but one, a handful in practice, and a rank-deficient one is the case the pseudo-inverse exists for: a
/// group nobody is at risk in by the first event.
/// </remarks>
internal static class SymmetricPseudoInverse
{
    /// <summary>numpy's default relative cutoff for <c>pinv</c>.</summary>
    private const double RelativeCutoff = 1e-15;

    /// <summary>Sweeps past which an off-diagonal that has not vanished is left as it is.</summary>
    private const int MaxSweeps = 100;

    /// <summary>Evaluates <c>zᵀ A⁺ z</c>.</summary>
    /// <param name="matrix">The symmetric matrix, row-major, <paramref name="size"/> squared values; overwritten.</param>
    /// <param name="vector">The vector <c>z</c>, <paramref name="size"/> values.</param>
    /// <param name="size">The side of the matrix.</param>
    internal static double QuadraticForm(double[] matrix, double[] vector, int size)
    {
        double[] eigenvectors = Diagonalise(matrix, size);
        double largest = 0.0;
        for (int i = 0; i < size; i++)
        {
            largest = Math.Max(largest, Math.Abs(matrix[(i * size) + i]));
        }

        double cutoff = RelativeCutoff * largest;
        double total = 0.0;
        for (int k = 0; k < size; k++)
        {
            double eigenvalue = matrix[(k * size) + k];
            if (Math.Abs(eigenvalue) <= cutoff)
            {
                continue;
            }

            double projection = 0.0;
            for (int i = 0; i < size; i++)
            {
                projection += eigenvectors[(i * size) + k] * vector[i];
            }

            total += projection * projection / eigenvalue;
        }

        return total;
    }

    /// <summary>Rotates <paramref name="matrix"/> to its eigenvalues in place and returns the eigenvectors, column by column.</summary>
    private static double[] Diagonalise(double[] matrix, int size)
    {
        var vectors = new double[size * size];
        for (int i = 0; i < size; i++)
        {
            vectors[(i * size) + i] = 1.0;
        }

        for (int sweep = 0; sweep < MaxSweeps && OffDiagonal(matrix, size) > 0.0; sweep++)
        {
            for (int p = 0; p < size - 1; p++)
            {
                for (int q = p + 1; q < size; q++)
                {
                    Rotate(matrix, vectors, size, p, q);
                }
            }
        }

        return vectors;
    }

    /// <summary>The squared off-diagonal mass, zero once every rotation has done its work.</summary>
    private static double OffDiagonal(double[] matrix, int size)
    {
        double total = 0.0;
        for (int p = 0; p < size; p++)
        {
            for (int q = p + 1; q < size; q++)
            {
                double value = matrix[(p * size) + q];
                total += value * value;
            }
        }

        return total;
    }

    /// <summary>One Jacobi rotation, zeroing the pair <c>(p, q)</c>; Golub and Van Loan's symmetric Schur step.</summary>
    private static void Rotate(double[] matrix, double[] vectors, int size, int p, int q)
    {
        double apq = matrix[(p * size) + q];
        // S1244: an exact zero needs no rotation, and any other value does, however small.
#pragma warning disable S1244
        if (apq == 0.0)
#pragma warning restore S1244
        {
            return;
        }

        double app = matrix[(p * size) + p];
        double aqq = matrix[(q * size) + q];
        double theta = (aqq - app) / (2.0 * apq);
        // The smaller root of t² + 2θt − 1 = 0, the rotation by at most 45°; θ = 0 is exactly 45°, and past
        // θ² overflowing the root is 1 / (2θ) to working precision.
        double t = double.IsInfinity(theta * theta)
            ? 1.0 / (2.0 * theta)
            : Math.Sign(theta) / (Math.Abs(theta) + Math.Sqrt((theta * theta) + 1.0));
#pragma warning disable S1244
        if (theta == 0.0)
#pragma warning restore S1244
        {
            t = 1.0;
        }

        double c = 1.0 / Math.Sqrt((t * t) + 1.0);
        double s = t * c;
        for (int k = 0; k < size; k++)
        {
            double akp = matrix[(k * size) + p];
            double akq = matrix[(k * size) + q];
            matrix[(k * size) + p] = (c * akp) - (s * akq);
            matrix[(k * size) + q] = (s * akp) + (c * akq);
        }

        for (int k = 0; k < size; k++)
        {
            double apk = matrix[(p * size) + k];
            double aqk = matrix[(q * size) + k];
            matrix[(p * size) + k] = (c * apk) - (s * aqk);
            matrix[(q * size) + k] = (s * apk) + (c * aqk);
        }

        matrix[(p * size) + q] = 0.0;
        matrix[(q * size) + p] = 0.0;
        for (int k = 0; k < size; k++)
        {
            double vkp = vectors[(k * size) + p];
            double vkq = vectors[(k * size) + q];
            vectors[(k * size) + p] = (c * vkp) - (s * vkq);
            vectors[(k * size) + q] = (s * vkp) + (c * vkq);
        }
    }
}
