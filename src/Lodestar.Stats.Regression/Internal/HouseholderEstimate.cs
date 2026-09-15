namespace Lodestar.Stats.Regression.Internal;

/// <summary>The Householder least squares behind <c>OrdinaryLeastSquares.Estimate</c>, with Q never formed.</summary>
/// <remarks>
/// <c>OrdinaryLeastSquares.Fit</c> builds Q explicitly for the robust covariances' leverages, and a second QR
/// for the VIFs. An estimate needs neither: the reflections are applied to the response as they are
/// formed, and R's inverse gives the coefficients and their standard errors (#671).
/// </remarks>
internal static class HouseholderEstimate
{
    /// <summary>Fits <paramref name="response"/> on <paramref name="design"/> without the inference table.</summary>
    /// <returns>The coefficients, their standard errors and t statistics, intercept first, and the residual sum of squares.</returns>
    internal static (double[] Coefficients, double[] StandardErrors, double[] TStatistics, double ResidualSumOfSquares, int ResidualDegreesOfFreedom) Fit(
        ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, bool withIntercept)
    {
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows fit {parameterCount} parameters with {residualDegreesOfFreedom} degrees "
                + "of freedom left, and every standard error here divides by that.",
                nameof(design));
        }

        // Column-major, so each reflection walks one contiguous column.
        var a = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int source = row * featureCount;
            int column = 0;
            if (withIntercept)
            {
                a[row] = 1.0;
                column = 1;
            }

            for (int feature = 0; feature < featureCount; feature++)
            {
                a[((column + feature) * rowCount) + row] = design[source + feature];
            }
        }

        double[] projected = response.ToArray();
        Triangularize(a, rowCount, parameterCount, projected);

        double residualSumOfSquares = 0.0;
        for (int row = parameterCount; row < rowCount; row++)
        {
            residualSumOfSquares += projected[row] * projected[row];
        }

        (double[] coefficients, double[] standardErrors, double[] tStatistics) =
            Statistics(a, rowCount, parameterCount, projected, residualSumOfSquares / residualDegreesOfFreedom);
        return (coefficients, standardErrors, tStatistics, residualSumOfSquares, residualDegreesOfFreedom);
    }

    /// <summary>Householder reflections reducing <paramref name="a"/> to R in its upper triangle, applied to <paramref name="projected"/> too.</summary>
    private static void Triangularize(double[] a, int rowCount, int parameterCount, double[] projected)
    {
        for (int k = 0; k < parameterCount; k++)
        {
            int diagonal = (k * rowCount) + k;
            double norm = 0.0;
            for (int row = k; row < rowCount; row++)
            {
                double value = a[(k * rowCount) + row];
                norm += value * value;
            }

            norm = Math.Sqrt(norm);
            double alpha = a[diagonal] > 0.0 ? -norm : norm;

            // v = x - alpha·e1 in place, then H = I - 2vvᵀ/(vᵀv); vᵀv = 2·norm·(norm + |x₀|).
            a[diagonal] -= alpha;
            double scale = norm * (norm + Math.Abs(a[diagonal] + alpha));
            if (scale > 0.0)
            {
                for (int column = k + 1; column < parameterCount; column++)
                {
                    Reflect(a, k, rowCount, scale, a, column * rowCount);
                }

                Reflect(a, k, rowCount, scale, projected, 0);
            }

            a[diagonal] = alpha;
        }
    }

    /// <summary>Applies the reflection stored below column <paramref name="k"/>'s diagonal to a vector.</summary>
    private static void Reflect(double[] a, int k, int rowCount, double scale, double[] target, int offset)
    {
        int vector = k * rowCount;
        double dot = 0.0;
        for (int row = k; row < rowCount; row++)
        {
            dot += a[vector + row] * target[offset + row];
        }

        double factor = dot / scale;
        for (int row = k; row < rowCount; row++)
        {
            target[offset + row] -= factor * a[vector + row];
        }
    }

    /// <summary>The coefficients by back substitution, their standard errors from R's inverse, and each quotient.</summary>
    private static (double[] Coefficients, double[] StandardErrors, double[] TStatistics) Statistics(
        double[] a, int rowCount, int parameterCount, double[] projected, double residualVariance)
    {
        var upper = new double[parameterCount * parameterCount];
        for (int row = 0; row < parameterCount; row++)
        {
            for (int column = row; column < parameterCount; column++)
            {
                upper[(row * parameterCount) + column] = a[(column * rowCount) + row];
            }
        }

        double[] inverse = LeastSquares.InvertUpper(upper, parameterCount);
        var coefficients = new double[parameterCount];
        var standardErrors = new double[parameterCount];
        var statistics = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double coefficient = 0.0;
            double squaredNorm = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                double entry = inverse[(i * parameterCount) + k];
                coefficient += entry * projected[k];
                squaredNorm += entry * entry;
            }

            coefficients[i] = coefficient;
            standardErrors[i] = Math.Sqrt(squaredNorm * residualVariance);
            statistics[i] = coefficient / standardErrors[i];
        }

        return (coefficients, standardErrors, statistics);
    }
}
