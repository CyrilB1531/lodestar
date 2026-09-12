using Lodestar.Decomposition;
using Lodestar.Stats.Regression.Internal;

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
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);

        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows fit {parameterCount} parameters with {residualDegreesOfFreedom} degrees "
                + "of freedom left, and every standard error here divides by that.",
                nameof(design));
        }

        double[] matrix = LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept);
        double[] coefficients = LeastSquares.Solve(matrix, rowCount, parameterCount, response, out double[] inverseUpper);

        double[] residuals = Residuals(matrix, rowCount, parameterCount, response, coefficients);
        double residualSumOfSquares = Dot(residuals, residuals);
        double residualVariance = residualSumOfSquares / residualDegreesOfFreedom;
        double residualStandardError = Math.Sqrt(residualVariance);

        double[] standardErrors = LeastSquares.StandardErrors(inverseUpper, parameterCount, residualVariance);
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

    /// <summary>One VIF per regressor, read off a single decomposition of the standardised block.</summary>
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
        int regressorCount = parameterCount - first;
        var factors = new double[regressorCount];
        if (regressorCount == 1 && !withIntercept)
        {
            // statsmodels raises a ValueError here: nothing is left to explain the only
            // regressor. NaN, because one undefined diagnostic does not sink a valid fit.
            factors[0] = double.NaN;
            return factors;
        }

        // long-comment: why one decomposition replaces one regression per regressor (#591).
        // For standardised Z the correlation matrix is ZᵀZ/n, and a VIF is a diagonal entry of
        // its inverse — the same number the auxiliary regression of a column on the others
        // reaches the long way round. Through the QR that is Zᵀ Z = RᵀR, so (ZᵀZ)⁻¹ = R⁻¹R⁻ᵀ and
        // the j-th diagonal is the squared norm of R⁻¹'s j-th row. The intercept's column drops
        // out rather than being held out: standardised regressors are centred, so a constant
        // explains none of them. Measured against tests/oracles/stats_ols.json, the identity
        // agrees with the regressions to 1.3e-12 relative on the near-collinear case whose VIF
        // is 6e4, and to 3.4e-15 or better on the other five — inside the corpus's own 1e-9.
        double[] standardised = StandardiseRegressors(matrix, rowCount, parameterCount, first);
        QrDecomposition qr = QrDecomposition.Householder(standardised, rowCount, regressorCount);
        double[] inverseUpper = LeastSquares.InvertUpper(qr.R, regressorCount);

        for (int column = 0; column < regressorCount; column++)
        {
            double total = 0.0;
            for (int k = column; k < regressorCount; k++)
            {
                double entry = inverseUpper[(column * regressorCount) + k];
                total += entry * entry;
            }

            factors[column] = Clip(rowCount * total);
        }

        return factors;
    }

    /// <summary>The ceiling a perfectly collinear pair reaches, rather than an infinity.</summary>
    /// <remarks>
    /// The reference clips the explained fraction at <c>1 - 1e-15</c> before dividing, so this
    /// is that quotient rather than a round number: subtracting 1e-15 from one lands on the
    /// neighbouring double, not on 1e-15 exactly, and the cap inherits the difference.
    /// </remarks>
    private const double MaximumFactor = 1.0 / (1.0 - (1.0 - 1e-15));

    /// <summary>A factor held inside the range an explained fraction in [0, 1) can produce.</summary>
    private static double Clip(double factor) =>
        double.IsNaN(factor) ? MaximumFactor : Math.Min(Math.Max(factor, 1.0), MaximumFactor);

    /// <summary>The regressor columns alone, each centred and scaled to unit spread.</summary>
    /// <remarks>
    /// The spread is the population standard deviation, and the 1e-10 floor is the reference's:
    /// a column that does not vary is left as it stands rather than divided by zero.
    /// </remarks>
    private static double[] StandardiseRegressors(
        double[] matrix, int rowCount, int parameterCount, int first)
    {
        int regressorCount = parameterCount - first;
        var working = new double[rowCount * regressorCount];

        for (int column = 0; column < regressorCount; column++)
        {
            int source = column + first;
            double mean = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                mean += matrix[(row * parameterCount) + source];
            }

            mean /= rowCount;

            double variance = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                double deviation = matrix[(row * parameterCount) + source] - mean;
                variance += deviation * deviation;
            }

            double spread = Math.Sqrt(variance / rowCount);
            bool varies = spread > 1e-10;
            for (int row = 0; row < rowCount; row++)
            {
                double value = matrix[(row * parameterCount) + source];
                working[(row * regressorCount) + column] = varies ? (value - mean) / spread : value;
            }
        }

        return working;
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
