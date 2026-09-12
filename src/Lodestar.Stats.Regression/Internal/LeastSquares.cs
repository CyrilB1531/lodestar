using Lodestar.Decomposition;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The least-squares arithmetic the OLS and the GLM both need.</summary>
/// <remarks>
/// Extracted rather than written twice: the GLM's IRLS solves a weighted least squares each
/// iteration, and a weighted solve is this one over rows scaled by the square root of the
/// weight (#616).
/// </remarks>
internal static class LeastSquares
{
    /// <summary>The row count, with the shapes that are not a design refused.</summary>
    /// <remarks>
    /// Shared rather than written per model: an empty design is <c>0 != 0 * featureCount</c>,
    /// which a length comparison alone admits, and it then fails further in as a missing
    /// residual degree of freedom — a diagnosis of the wrong input (#616).
    /// </remarks>
    public static int Rows(
        ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount)
    {
        if (design.Length == 0 || design.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"design holds {design.Length} values, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(design));
        }

        int rowCount = design.Length / featureCount;
        if (response.Length != rowCount)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows and response holds {response.Length} values.",
                nameof(response));
        }

        return rowCount;
    }

    /// <summary>The design matrix, with an intercept column prepended when asked.</summary>
    public static double[] Design(
        ReadOnlySpan<double> design, int rowCount, int featureCount, bool withIntercept)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        var matrix = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * parameterCount;
            if (withIntercept)
            {
                matrix[at++] = 1.0;
            }

            for (int column = 0; column < featureCount; column++)
            {
                matrix[at + column] = design[(row * featureCount) + column];
            }
        }

        return matrix;
    }

    /// <summary>Least squares through a thin QR, reporting the inverse of R the covariance needs.</summary>
    /// <returns>The coefficients, the inverse of R the standard errors are read from, and the factorization itself.</returns>
    /// <remarks>
    /// The QR is returned rather than discarded because a robust covariance needs the leverages,
    /// which are a row of Q against itself. Computing them here instead would charge every IRLS
    /// iteration for something only one caller in one mode wants (#686).
    /// </remarks>
    public static (double[] Coefficients, double[] InverseUpper, QrDecomposition Factorization) Solve(
        double[] matrix,
        int rowCount,
        int parameterCount,
        ReadOnlySpan<double> response)
    {
        QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount, parameterCount);
        IReadOnlyList<double> q = qr.Q;

        var projected = new double[parameterCount];
        for (int column = 0; column < parameterCount; column++)
        {
            double total = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                total += q[(row * parameterCount) + column] * response[row];
            }

            projected[column] = total;
        }

        double[] inverseUpper = InvertUpper(qr.R, parameterCount);
        var coefficients = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                total += inverseUpper[(i * parameterCount) + k] * projected[k];
            }

            coefficients[i] = total;
        }

        return (coefficients, inverseUpper, qr);
    }

    /// <summary>The inverse of an upper-triangular matrix, by back substitution.</summary>
    public static double[] InvertUpper(IReadOnlyList<double> upper, int order)
    {
        var inverse = new double[order * order];
        for (int column = order - 1; column >= 0; column--)
        {
            inverse[(column * order) + column] = 1.0 / upper[(column * order) + column];
            for (int row = column - 1; row >= 0; row--)
            {
                double total = 0.0;
                for (int k = row + 1; k <= column; k++)
                {
                    total += upper[(row * order) + k] * inverse[(k * order) + column];
                }

                inverse[(row * order) + column] = -total / upper[(row * order) + row];
            }
        }

        return inverse;
    }

    /// <summary>The diagonal of the covariance, scaled, square-rooted.</summary>
    public static double[] StandardErrors(
        double[] inverseUpper, int parameterCount, double dispersion)
    {
        var errors = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                double value = inverseUpper[(i * parameterCount) + k];
                total += value * value;
            }

            errors[i] = Math.Sqrt(total * dispersion);
        }

        return errors;
    }
}
