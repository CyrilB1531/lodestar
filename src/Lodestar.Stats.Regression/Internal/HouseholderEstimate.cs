namespace Lodestar.Stats.Regression.Internal;

/// <summary>The Householder least squares behind <c>OrdinaryLeastSquares.Estimate</c>, with Q never formed.</summary>
/// <remarks>
/// <c>OrdinaryLeastSquares.Fit</c> also builds the VIFs and, when asked, a robust covariance from the design
/// row by row. An estimate needs neither: the reflections are applied to the response as they are
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
        LeastSquares.Triangularize(a, rowCount, parameterCount, projected);

        double residualSumOfSquares = 0.0;
        for (int row = parameterCount; row < rowCount; row++)
        {
            residualSumOfSquares += projected[row] * projected[row];
        }

        (double[] coefficients, double[] standardErrors, double[] tStatistics) =
            Statistics(a, rowCount, parameterCount, projected, residualSumOfSquares / residualDegreesOfFreedom);
        return (coefficients, standardErrors, tStatistics, residualSumOfSquares, residualDegreesOfFreedom);
    }

    /// <summary>The coefficients by back substitution, their standard errors from R's inverse, and each quotient.</summary>
    private static (double[] Coefficients, double[] StandardErrors, double[] TStatistics) Statistics(
        double[] a, int rowCount, int parameterCount, double[] projected, double residualVariance)
    {
        // The same refusal Fit takes in LeastSquares.FromTriangle, which this path does not call: without it
        // x2 = 3*x1 answered coefficients near 1e14 while Fit refused the same design (#979).
        LeastSquares.RequireFullRank(a, rowCount, parameterCount, LeastSquares.DesignParameter);
        double[] inverse = LeastSquares.InvertUpper(LeastSquares.Upper(a, rowCount, parameterCount), parameterCount);
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
