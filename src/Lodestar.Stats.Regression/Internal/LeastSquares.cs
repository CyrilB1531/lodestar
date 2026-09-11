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
    public static double[] Solve(
        double[] matrix,
        int rowCount,
        int parameterCount,
        ReadOnlySpan<double> response,
        out double[] inverseUpper)
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

        inverseUpper = InvertUpper(qr.R, parameterCount);
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

        return coefficients;
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
