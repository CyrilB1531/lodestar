using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>
/// Generalized least squares with a caller-supplied error covariance and the inference table on top of it, at
/// <c>statsmodels.api.GLS</c> parity.
/// </summary>
/// <remarks>
/// For errors that are correlated rather than only unequal: rows and response are whitened by the inverse of the
/// covariance's Cholesky factor and fitted as <see cref="OrdinaryLeastSquares"/> would. A diagonal covariance is
/// <see cref="WeightedLeastSquares"/> with weights <c>1/σ</c>.
/// </remarks>
public static class GeneralizedLeastSquares
{
    /// <summary>The relative gap past which a pair of mirrored covariance entries is not the same number.</summary>
    private const double SymmetryTolerance = 1e-12;

    /// <summary>Fits a linear model under a given error covariance and reports what a summary table holds.</summary>
    /// <param name="design">The regressors, row-major: <paramref name="featureCount"/> values per row, with no constant column of your own.</param>
    /// <param name="response">One observed value per row of <paramref name="design"/>.</param>
    /// <param name="covariance">The error covariance, row-major, one row and one column per row of <paramref name="design"/>: symmetric and positive definite.</param>
    /// <param name="featureCount">How many regressors each row carries.</param>
    /// <param name="options">Whether to fit an intercept, which covariance of the estimates, and at what confidence; <see langword="null"/> fits one at 0.95.</param>
    /// <returns>The fitted model, with its standard errors, t statistics, p-values, intervals and VIFs.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or <paramref name="covariance"/> holds a value that is not finite.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> has a different length, <paramref name="covariance"/> is not the square of that length, is not symmetric or is not positive definite, <paramref name="options"/> asks for <see cref="CovarianceType.Hac"/> or <see cref="CovarianceType.Cluster"/>, no residual degrees of freedom are left, or a column of the whitened design, intercept included, is collinear with the columns before it.</exception>
    /// <remarks>
    /// R² follows the reference: centred, with an intercept, on the mean estimated in whitened space. The VIFs
    /// read the design as given, as <c>variance_inflation_factor</c> does.
    /// </remarks>
    public static OlsSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> covariance,
        int featureCount,
        OlsOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        OlsOptions settings = options ?? new OlsOptions();
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        OrdinaryLeastSquares.RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
        if (settings.CovarianceType is CovarianceType.Hac or CovarianceType.Cluster)
        {
            // Out of #775's scope: no corpus pins either covariance on whitened GLS rows.
            throw new ArgumentException(
                $"Covariance type {settings.CovarianceType} is not offered for generalized least squares.", nameof(options));
        }

        OrdinaryLeastSquares.CheckCovariance(settings, default, clustered: false, rowCount, nameof(options));
        double[] lower = Factor(covariance, rowCount);

        double[] whitened = LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept);
        for (int column = 0; column < parameterCount; column++)
        {
            Cholesky.ForwardSubstitute(lower, rowCount, whitened, parameterCount, column);
        }

        double[] whitenedResponse = response.ToArray();
        Cholesky.ForwardSubstitute(lower, rowCount, whitenedResponse);

        // The whitened design carries its constant as a column of its own, L⁻¹·1, so the solve reads every column.
        (double[] coefficients, double[] inverseUpper) =
            LeastSquares.Solve(whitened, rowCount, parameterCount, withIntercept: false, whitenedResponse);
        double[] residuals = WhitenedResiduals(whitened, whitenedResponse, coefficients, rowCount);

        return OrdinaryLeastSquares.Tabulate(
            new OrdinaryLeastSquares.SolvedFit(
                coefficients,
                inverseUpper,
                residuals,
                settings.CovarianceType == CovarianceType.Nonrobust ? null : whitened),
            OrdinaryLeastSquares.Vif(design, rowCount, featureCount, settings.WithIntercept),
            TotalSumOfSquares(lower, response, whitenedResponse, settings.WithIntercept ? whitened : null),
            rowCount,
            settings);
    }

    /// <summary><c>L⁻¹y − L⁻¹X β</c>, the residuals the variance and the robust covariances read.</summary>
    private static double[] WhitenedResiduals(double[] whitened, double[] whitenedResponse, double[] coefficients, int rowCount)
    {
        int parameterCount = coefficients.Length;
        var residuals = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            double fitted = 0.0;
            for (int column = 0; column < parameterCount; column++)
            {
                fitted += whitened[(row * parameterCount) + column] * coefficients[column];
            }

            residuals[row] = whitenedResponse[row] - fitted;
        }

        return residuals;
    }

    /// <summary>The covariance's Cholesky factor, after refusing what the reference would reject or silently misread.</summary>
    private static double[] Factor(ReadOnlySpan<double> covariance, int rowCount)
    {
        // In long: 65,536 rows square to 2³², which wraps an int to zero and let an empty covariance through (#905).
        long expected = (long)rowCount * rowCount;
        if (covariance.Length != expected)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows, so covariance holds {expected} values, not {covariance.Length}.",
                nameof(covariance));
        }

        for (int i = 0; i < covariance.Length; i++)
        {
            if (double.IsNaN(covariance[i]) || double.IsInfinity(covariance[i]))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(covariance), covariance[i], $"covariance[{i}] is not a finite number.");
            }
        }

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < row; column++)
            {
                double below = covariance[(row * rowCount) + column];
                double above = covariance[(column * rowCount) + row];
                if (Math.Abs(below - above) > SymmetryTolerance * Math.Max(Math.Abs(below), Math.Abs(above)))
                {
                    // The reference's Cholesky reads the lower triangle and ignores the upper one.
                    throw new ArgumentException(
                        $"covariance[{row}, {column}] is {below} and covariance[{column}, {row}] is {above}: an error "
                        + "covariance is symmetric.", nameof(covariance));
                }
            }
        }

        if (!Cholesky.TryFactor(covariance, rowCount, out double[] lower))
        {
            throw new ArgumentException(
                "covariance is not positive definite, so no Cholesky factor whitens the rows.", nameof(covariance));
        }

        return lower;
    }

    /// <summary>R²'s denominator: centred on the mean estimated in whitened space, or the whitened response's own square.</summary>
    /// <param name="lower">The covariance's Cholesky factor.</param>
    /// <param name="response">The response as the caller gave it.</param>
    /// <param name="whitenedResponse"><c>L⁻¹y</c>.</param>
    /// <param name="whitenedDesign">The whitened design of a fit with an intercept, whose column 0 is already <c>L⁻¹·1</c>; null without one.</param>
    private static double TotalSumOfSquares(
        double[] lower, ReadOnlySpan<double> response, double[] whitenedResponse, double[]? whitenedDesign)
    {
        int rowCount = response.Length;
        if (whitenedDesign is null)
        {
            return OrdinaryLeastSquares.Dot(whitenedResponse, whitenedResponse);
        }

        // The same substitution on the same column of ones, so the same bits, without solving it a second time.
        int parameterCount = whitenedDesign.Length / rowCount;
        var ones = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            ones[row] = whitenedDesign[row * parameterCount];
        }
        double mean = OrdinaryLeastSquares.Dot(whitenedResponse, ones) / OrdinaryLeastSquares.Dot(ones, ones);

        var centred = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            centred[row] = response[row] - mean;
        }

        Cholesky.ForwardSubstitute(lower, rowCount, centred);
        return OrdinaryLeastSquares.Dot(centred, centred);
    }
}
