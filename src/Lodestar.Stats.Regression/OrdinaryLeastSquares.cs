using Lodestar.Decomposition;

namespace Lodestar.Stats.Regression;

/// <summary>
/// Ordinary least squares with the inference table on top of it, at
/// <c>statsmodels.api.OLS</c> parity.
/// </summary>
/// <remarks>
/// Spans in, one summary out. The estimate is the cheap half: what this returns that a
/// solver does not is the covariance of the estimates, and the tail probabilities read on it.
/// </remarks>
public static class OrdinaryLeastSquares
{
    /// <summary>Fits a linear model and reports what a summary table holds.</summary>
    /// <param name="design">The regressors, row-major: <paramref name="featureCount"/> values per row, with no constant column of your own.</param>
    /// <param name="response">One observed value per row of <paramref name="design"/>.</param>
    /// <param name="featureCount">How many regressors each row carries.</param>
    /// <param name="options">Whether to fit an intercept and at what confidence; <see langword="null"/> fits one at 0.95.</param>
    /// <returns>The fitted model, with its standard errors, t statistics, p-values, intervals and VIFs.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> has a different length, or there are no residual degrees of freedom left.</exception>
    /// <remarks>
    /// Solved through a Householder QR of the design rather than the normal equations:
    /// forming <c>XᵀX</c> squares its condition number, and the near-collinear designs a VIF
    /// exists to report are exactly the ones that costs.
    /// </remarks>
    public static OlsSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        OlsOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        OlsOptions settings = options ?? new OlsOptions();
        int rowCount = Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);

        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows fit {parameterCount} parameters with {residualDegreesOfFreedom} degrees "
                + "of freedom left, and every standard error here divides by that.",
                nameof(design));
        }

        double[] matrix = Design(design, rowCount, featureCount, settings.WithIntercept);
        double[] coefficients = Solve(matrix, rowCount, parameterCount, response, out double[] inverseUpper);

        double[] residuals = Residuals(matrix, rowCount, parameterCount, response, coefficients);
        double residualSumOfSquares = Dot(residuals, residuals);
        double residualVariance = residualSumOfSquares / residualDegreesOfFreedom;
        double residualStandardError = Math.Sqrt(residualVariance);

        double[] standardErrors = StandardErrors(inverseUpper, parameterCount, residualVariance);
        var tStatistics = new double[parameterCount];
        var pValues = new double[parameterCount];
        var lower = new double[parameterCount];
        var upper = new double[parameterCount];
        double multiplier = Distributions.StudentQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0), residualDegreesOfFreedom);

        for (int j = 0; j < parameterCount; j++)
        {
            tStatistics[j] = coefficients[j] / standardErrors[j];
            pValues[j] = 2.0 * Distributions.StudentSf(Math.Abs(tStatistics[j]), residualDegreesOfFreedom);
            lower[j] = coefficients[j] - (multiplier * standardErrors[j]);
            upper[j] = coefficients[j] + (multiplier * standardErrors[j]);
        }

        double rSquared = RSquared(response, residualSumOfSquares, settings.WithIntercept);
        int modelDegreesOfFreedom = parameterCount - (settings.WithIntercept ? 1 : 0);
        double fStatistic = rSquared / modelDegreesOfFreedom
            / ((1.0 - rSquared) / residualDegreesOfFreedom);

        return new OlsSummary
        {
            Coefficients = coefficients,
            StandardErrors = standardErrors,
            TStatistics = tStatistics,
            PValues = pValues,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            VarianceInflationFactors = Vif(matrix, rowCount, parameterCount, settings.WithIntercept),
            HasIntercept = settings.WithIntercept,
            ConfidenceLevel = settings.ConfidenceLevel,
            RSquared = rSquared,
            AdjustedRSquared = AdjustedRSquared(rSquared, rowCount, parameterCount, settings.WithIntercept),
            FStatistic = fStatistic,
            FPValue = Distributions.FisherSf(fStatistic, modelDegreesOfFreedom, residualDegreesOfFreedom),
            ResidualDegreesOfFreedom = residualDegreesOfFreedom,
            ResidualStandardError = residualStandardError,
        };
    }

    /// <summary>The row count, with the shapes that are not a design refused.</summary>
    private static int Rows(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount)
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

    /// <summary>The design as the fit sees it: a leading column of ones when an intercept is wanted.</summary>
    private static double[] Design(
        ReadOnlySpan<double> design, int rowCount, int featureCount, bool withIntercept)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        var matrix = new double[rowCount * parameterCount];

        for (int row = 0; row < rowCount; row++)
        {
            int target = row * parameterCount;
            if (withIntercept)
            {
                matrix[target++] = 1.0;
            }

            for (int column = 0; column < featureCount; column++)
            {
                matrix[target + column] = design[(row * featureCount) + column];
            }
        }

        return matrix;
    }

    /// <summary>Least squares through a thin QR, reporting the inverse of R the covariance needs.</summary>
    private static double[] Solve(
        double[] matrix,
        int rowCount,
        int parameterCount,
        ReadOnlySpan<double> response,
        out double[] inverseUpper)
    {
        QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount, parameterCount);
        IReadOnlyList<double> q = qr.Q;
        IReadOnlyList<double> r = qr.R;

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

        inverseUpper = InvertUpper(r, parameterCount);
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

    /// <summary>The inverse of an upper-triangular matrix, by back substitution column by column.</summary>
    private static double[] InvertUpper(IReadOnlyList<double> upper, int order)
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

    /// <summary>What the model leaves unexplained, row by row.</summary>
    private static double[] Residuals(
        double[] matrix,
        int rowCount,
        int parameterCount,
        ReadOnlySpan<double> response,
        double[] coefficients)
    {
        var residuals = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            double fitted = 0.0;
            for (int column = 0; column < parameterCount; column++)
            {
                fitted += matrix[(row * parameterCount) + column] * coefficients[column];
            }

            residuals[row] = response[row] - fitted;
        }

        return residuals;
    }

    /// <summary>The diagonal of σ²(XᵀX)⁻¹, reached through R⁻¹ rather than through XᵀX.</summary>
    private static double[] StandardErrors(double[] inverseUpper, int order, double residualVariance)
    {
        var errors = new double[order];
        for (int row = 0; row < order; row++)
        {
            double total = 0.0;
            for (int k = row; k < order; k++)
            {
                double entry = inverseUpper[(row * order) + k];
                total += entry * entry;
            }

            errors[row] = Math.Sqrt(residualVariance * total);
        }

        return errors;
    }

    /// <summary>Explained fraction — centred against the mean, or against zero with no intercept.</summary>
    private static double RSquared(
        ReadOnlySpan<double> response, double residualSumOfSquares, bool withIntercept)
    {
        double total = 0.0;
        if (withIntercept)
        {
            double mean = 0.0;
            for (int i = 0; i < response.Length; i++)
            {
                mean += response[i];
            }

            mean /= response.Length;
            for (int i = 0; i < response.Length; i++)
            {
                double deviation = response[i] - mean;
                total += deviation * deviation;
            }
        }
        else
        {
            for (int i = 0; i < response.Length; i++)
            {
                total += response[i] * response[i];
            }
        }

        return 1.0 - (residualSumOfSquares / total);
    }

    /// <summary>R-squared penalised for the parameters spent reaching it.</summary>
    private static double AdjustedRSquared(
        double rSquared, int rowCount, int parameterCount, bool withIntercept)
    {
        // With no intercept the uncentred R-squared is adjusted against n rather than n-1,
        // which is statsmodels' rule and the reason the two forms differ by more than a term.
        int total = withIntercept ? rowCount - 1 : rowCount;
        return 1.0 - ((1.0 - rSquared) * total / (rowCount - parameterCount));
    }

    /// <summary>One VIF per regressor: each column explained by the others the model carries.</summary>
    /// <remarks>
    /// The design is standardised first, which is what
    /// <c>statsmodels.stats.outliers_influence.variance_inflation_factor</c> does since 0.15.0.
    /// With an intercept that is a no-op on the answer — an affine change of the regressors
    /// leaves R² alone once a constant absorbs the shift — and without one it is the whole
    /// difference between this and the textbook formula.
    /// </remarks>
    private static double[] Vif(double[] matrix, int rowCount, int parameterCount, bool withIntercept)
    {
        int first = withIntercept ? 1 : 0;
        var factors = new double[parameterCount - first];
        if (parameterCount - first == 1 && !withIntercept)
        {
            // statsmodels raises a ValueError here: nothing is left to explain the only
            // regressor. NaN, because one undefined diagnostic does not sink a valid fit.
            factors[0] = double.NaN;
            return factors;
        }

        double[] working = Standardise(matrix, rowCount, parameterCount);
        var auxiliary = new double[rowCount * (parameterCount - 1)];
        var target = new double[rowCount];
        for (int column = first; column < parameterCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                target[row] = working[(row * parameterCount) + column];
                int written = 0;
                for (int other = 0; other < parameterCount; other++)
                {
                    if (other != column)
                    {
                        auxiliary[(row * (parameterCount - 1)) + written++] = working[(row * parameterCount) + other];
                    }
                }
            }

            double explained = AuxiliaryRSquared(auxiliary, rowCount, parameterCount - 1, target, withIntercept);

            // The reference clips before dividing, which caps a perfectly collinear pair at
            // 1e15 instead of returning an infinity.
            explained = Math.Min(Math.Max(explained, 0.0), 1.0 - 1e-15);
            factors[column - first] = 1.0 / (1.0 - explained);
        }

        return factors;
    }

    /// <summary>Each column centred and scaled to unit spread, leaving a constant one alone.</summary>
    /// <remarks>
    /// The spread is the population standard deviation, and the 1e-10 floor is the reference's:
    /// it is what exempts the intercept's column of ones from being divided by zero.
    /// </remarks>
    private static double[] Standardise(double[] matrix, int rowCount, int parameterCount)
    {
        var working = new double[matrix.Length];
        Array.Copy(matrix, working, matrix.Length);

        for (int column = 0; column < parameterCount; column++)
        {
            double mean = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                mean += matrix[(row * parameterCount) + column];
            }

            mean /= rowCount;

            double variance = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                double deviation = matrix[(row * parameterCount) + column] - mean;
                variance += deviation * deviation;
            }

            double spread = Math.Sqrt(variance / rowCount);
            if (spread <= 1e-10)
            {
                continue;
            }

            for (int row = 0; row < rowCount; row++)
            {
                working[(row * parameterCount) + column] =
                    (matrix[(row * parameterCount) + column] - mean) / spread;
            }
        }

        return working;
    }

    /// <summary>The R-squared of one column on the others, which is all a VIF reads.</summary>
    private static double AuxiliaryRSquared(
        double[] auxiliary, int rowCount, int parameterCount, double[] target, bool withIntercept)
    {
        double[] coefficients = Solve(auxiliary, rowCount, parameterCount, target, out _);
        double[] residuals = Residuals(auxiliary, rowCount, parameterCount, target, coefficients);
        return RSquared(target, Dot(residuals, residuals), withIntercept);
    }

    /// <summary>A plain inner product; the two arrays here are always the same length.</summary>
    private static double Dot(double[] left, double[] right)
    {
        double total = 0.0;
        for (int i = 0; i < left.Length; i++)
        {
            total += left[i] * right[i];
        }

        return total;
    }
}
