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
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> has a different length, <paramref name="options"/> sets <see cref="OlsOptions.HacLags"/> or <see cref="OlsOptions.SmallSampleCorrection"/> for a type that does not read it or asks for <see cref="CovarianceType.Hac"/> without lags or <see cref="CovarianceType.Cluster"/> without labels, or there are no residual degrees of freedom left.</exception>
    /// <remarks>
    /// Solved through the normal equations when the diagonal of <c>XᵀX</c>'s Cholesky factor stays
    /// within a ratio of 200, and through Householder reflections of the design otherwise: forming
    /// <c>XᵀX</c> squares its condition number, which the near-collinear designs a VIF exists to
    /// report cannot afford. <c>statsmodels</c> solves through a pseudo-inverse.
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

        RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
        CheckCovariance(settings, default, clustered: false, rowCount, nameof(options));

        return Summarise(
            design,
            response,
            TotalSumOfSquares(response, settings.WithIntercept),
            featureCount,
            settings);
    }

    /// <summary>Fits a linear model whose rows fall in clusters, and reports what a summary table holds.</summary>
    /// <param name="design">The regressors, row-major: <paramref name="featureCount"/> values per row, with no constant column of your own.</param>
    /// <param name="response">One observed value per row of <paramref name="design"/>.</param>
    /// <param name="clusters">One cluster label per row of <paramref name="design"/>; any integers, in any order, at least two distinct.</param>
    /// <param name="featureCount">How many regressors each row carries.</param>
    /// <param name="options">Whether to fit an intercept, the correction and the confidence; its <see cref="OlsOptions.CovarianceType"/> must be <see cref="CovarianceType.Cluster"/>.</param>
    /// <returns>The fitted model, with cluster-robust standard errors, z statistics, p-values, intervals and VIFs.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> or <paramref name="clusters"/> has a different length, <paramref name="clusters"/> names fewer than two clusters, <paramref name="options"/> asks for another covariance, or no residual degrees of freedom are left.</exception>
    /// <remarks>
    /// <c>statsmodels</c>' <c>fit(cov_type="cluster", cov_kwds={"groups": clusters})</c>. The labels only say which rows
    /// share a cluster, so relabelling them changes nothing.
    /// </remarks>
    public static OlsSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        ReadOnlySpan<int> clusters,
        int featureCount,
        OlsOptions options)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotNull(options);
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (options.WithIntercept ? 1 : 0);
        RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
        ClusterLabels? labels = CheckCovariance(options, clusters, clustered: true, rowCount, nameof(options));

        return Summarise(
            design,
            response,
            TotalSumOfSquares(response, options.WithIntercept),
            featureCount,
            options,
            labels);
    }

    /// <summary>Refuses the options a covariance type does not take, and makes the cluster labels dense.</summary>
    /// <param name="settings">The options, defaults resolved.</param>
    /// <param name="clusters">The labels the caller passed; empty through an overload without them.</param>
    /// <param name="clustered">Whether the call came through an overload taking labels.</param>
    /// <param name="rowCount">Rows in the design.</param>
    /// <param name="optionsName">The public parameter the options arrived in, named by the exception.</param>
    /// <returns>The labels renumbered from zero in first-seen order, or <see langword="null"/> for an unclustered call.</returns>
    /// <exception cref="ArgumentException">The options and the overload disagree, or the labels are of the wrong length or name one cluster.</exception>
    internal static ClusterLabels? CheckCovariance(
        OlsOptions settings, ReadOnlySpan<int> clusters, bool clustered, int rowCount, string optionsName)
    {
        CovarianceType type = settings.CovarianceType;
        if ((type == CovarianceType.Hac) != settings.HacLags.HasValue)
        {
            throw new ArgumentException(
                $"HacLags is set with covariance type {type}; it is required with Hac and read by nothing else.",
                optionsName);
        }

        if (settings.SmallSampleCorrection.HasValue && type is not (CovarianceType.Hac or CovarianceType.Cluster))
        {
            throw new ArgumentException(
                $"SmallSampleCorrection is set with covariance type {type}, which has no correction to switch.",
                optionsName);
        }

        if ((type == CovarianceType.Cluster) != clustered)
        {
            throw new ArgumentException(
                clustered
                    ? $"Cluster labels were passed with covariance type {type}, which does not read them."
                    : "The Cluster covariance needs one label per row, through the overload that takes them.",
                optionsName);
        }

        return clustered ? ClusterLabels.Dense(clusters, rowCount) : null;
    }

    /// <summary>Refuses a design with no residual degree of freedom left.</summary>
    /// <exception cref="ArgumentException">No residual degree of freedom is left.</exception>
    internal static void RequireResidualDegreesOfFreedom(int rowCount, int parameterCount, string parameterName)
    {
        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows fit {parameterCount} parameters with {residualDegreesOfFreedom} degrees "
                + "of freedom left, and every standard error here divides by that.",
                parameterName);
        }
    }

    /// <summary>Everything from the solve onward, shared by the ordinary and the weighted fit.</summary>
    /// <param name="design">The regressors as the caller gave them, row-major, with no constant column.</param>
    /// <param name="response">The response as the caller gave it.</param>
    /// <param name="totalSumOfSquares">The denominator of R², which a weighted fit centres on its weighted mean.</param>
    /// <param name="featureCount">Regressors per row.</param>
    /// <param name="settings">The options the caller passed, defaults resolved.</param>
    /// <param name="clusters">The dense cluster labels of a cluster fit; <see langword="null"/> otherwise.</param>
    /// <param name="weights">The weighted fit's weights, applied inside the solve rather than to a copy; empty otherwise.</param>
    internal static OlsSummary Summarise(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        double totalSumOfSquares,
        int featureCount,
        OlsOptions settings,
        ClusterLabels? clusters = null,
        ReadOnlySpan<double> weights = default)
    {
        int rowCount = response.Length;
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        (double[] coefficients, double[] inverseUpper) =
            LeastSquares.Solve(design, rowCount, featureCount, settings.WithIntercept, response, weights);

        double[] residuals = Residuals(design, rowCount, featureCount, response, coefficients, weights);

        // The robust covariances read the design row by row, so it is built — and whitened — only for them.
        double[]? robustMatrix = settings.CovarianceType == CovarianceType.Nonrobust
            ? null
            : Whiten(LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept), rowCount, parameterCount, weights);

        return Tabulate(
            new SolvedFit(coefficients, inverseUpper, residuals, robustMatrix, clusters),
            Vif(design, rowCount, featureCount, settings.WithIntercept),
            totalSumOfSquares,
            rowCount,
            settings);
    }

    /// <summary>What the table is read from: the estimate, R⁻¹, the residuals, and the matrix a robust covariance reads.</summary>
    /// <param name="Coefficients">The estimate, intercept first.</param>
    /// <param name="InverseUpper">R⁻¹, or U⁻¹ from the normal equations, row-major.</param>
    /// <param name="RowResiduals">The residuals of the rows the solve ran on — whitened for a weighted or generalized fit.</param>
    /// <param name="RobustMatrix">Those rows' design, intercept column included, when a robust covariance is asked for.</param>
    /// <param name="Clusters">The dense cluster labels of a cluster fit; <see langword="null"/> otherwise.</param>
    internal readonly record struct SolvedFit(
        double[] Coefficients,
        double[] InverseUpper,
        double[] RowResiduals,
        double[]? RobustMatrix,
        ClusterLabels? Clusters = null);

    /// <summary>The inference table on top of a solved fit, shared by the ordinary, weighted and generalized fits.</summary>
    internal static OlsSummary Tabulate(
        SolvedFit fit, double[] varianceInflationFactors, double totalSumOfSquares, int rowCount, OlsOptions settings)
    {
        double[] coefficients = fit.Coefficients;
        double[] inverseUpper = fit.InverseUpper;
        double[] residuals = fit.RowResiduals;
        int parameterCount = coefficients.Length;
        int residualDegreesOfFreedom = rowCount - parameterCount;
        double residualSumOfSquares = Dot(residuals, residuals);
        double residualVariance = residualSumOfSquares / residualDegreesOfFreedom;
        double residualStandardError = Math.Sqrt(residualVariance);

        bool robust = fit.RobustMatrix is not null;
        double[]? covariance = robust ? RobustSandwich(fit, rowCount, settings) : null;

        double[] standardErrors = covariance is null
            ? LeastSquares.StandardErrors(inverseUpper, parameterCount, residualVariance)
            : Diagonal(covariance, parameterCount);

        var tStatistics = new double[parameterCount];
        var pValues = new double[parameterCount];
        var lower = new double[parameterCount];
        var upper = new double[parameterCount];
        double half = 1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0);
        double multiplier = robust
            ? Distributions.NormalQuantile(half)
            : Distributions.StudentQuantile(half, residualDegreesOfFreedom);

        for (int j = 0; j < parameterCount; j++)
        {
            tStatistics[j] = coefficients[j] / standardErrors[j];
            // The square of a standard normal is chi-squared on one degree of freedom, so the
            // two-sided normal p-value is a tail this package already publishes (decision 0115).
            pValues[j] = robust
                ? Distributions.ChiSquaredSf(tStatistics[j] * tStatistics[j], 1.0)
                : 2.0 * Distributions.StudentSf(
                    Math.Abs(tStatistics[j]), residualDegreesOfFreedom);
            lower[j] = coefficients[j] - (multiplier * standardErrors[j]);
            upper[j] = coefficients[j] + (multiplier * standardErrors[j]);
        }

        double rSquared = 1.0 - (residualSumOfSquares / totalSumOfSquares);
        int modelDegreesOfFreedom = parameterCount - (settings.WithIntercept ? 1 : 0);
        double fStatistic = covariance is null
            ? rSquared / modelDegreesOfFreedom / ((1.0 - rSquared) / residualDegreesOfFreedom)
            : RobustCovariance.Wald(
                covariance, coefficients, parameterCount, settings.WithIntercept);

        return new OlsSummary
        {
            Coefficients = coefficients,
            StandardErrors = standardErrors,
            TStatistics = tStatistics,
            PValues = pValues,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            VarianceInflationFactors = varianceInflationFactors,
            CovarianceType = settings.CovarianceType,
            HasIntercept = settings.WithIntercept,
            ConfidenceLevel = settings.ConfidenceLevel,
            RSquared = rSquared,
            AdjustedRSquared = AdjustedRSquared(rSquared, rowCount, parameterCount, settings.WithIntercept),
            FStatistic = fStatistic,
            // A cluster fit reads its F on G − 1 denominator degrees of freedom, statsmodels' df_resid_inference.
            FPValue = Distributions.FisherSf(
                fStatistic, modelDegreesOfFreedom, fit.Clusters is { } groups ? groups.Count - 1 : residualDegreesOfFreedom),
            ResidualDegreesOfFreedom = residualDegreesOfFreedom,
            ResidualStandardError = residualStandardError,
        };
    }

    /// <summary>The robust covariance a fit asked for: its filling, the shared bread, and the type's small-sample factor.</summary>
    private static double[] RobustSandwich(SolvedFit fit, int rowCount, OlsOptions settings)
    {
        double[] matrix = fit.RobustMatrix!;
        int parameterCount = fit.Coefficients.Length;
        double degreesOfFreedomFactor = (double)rowCount / (rowCount - parameterCount);
        (double[] meat, double? correction) = settings.CovarianceType switch
        {
            CovarianceType.Hac => (
                RobustCovariance.HacMeat(matrix, fit.RowResiduals, rowCount, parameterCount, settings.HacLags!.Value),
                settings.SmallSampleCorrection == true ? degreesOfFreedomFactor : (double?)null),
            CovarianceType.Cluster => (
                RobustCovariance.ClusterMeat(matrix, fit.RowResiduals, fit.Clusters!.Labels, parameterCount, fit.Clusters.Count),
                settings.SmallSampleCorrection ?? true
                    ? (double)fit.Clusters.Count / (fit.Clusters.Count - 1) * ((rowCount - 1.0) / (rowCount - parameterCount))
                    : (double?)null),
            _ => (
                RobustCovariance.HeteroskedasticMeat(matrix, fit.InverseUpper, fit.RowResiduals, rowCount, parameterCount, settings.CovarianceType),
                settings.CovarianceType == CovarianceType.Hc1 ? degreesOfFreedomFactor : (double?)null),
        };

        return RobustCovariance.Sandwich(meat, fit.InverseUpper, parameterCount, correction);
    }

    /// <summary>Fits a linear model and reports the estimates and their standard errors, without the inference table.</summary>
    /// <param name="design">The regressors, row-major: <paramref name="featureCount"/> values per row, with no constant column of your own.</param>
    /// <param name="response">One observed value per row of <paramref name="design"/>.</param>
    /// <param name="featureCount">How many regressors each row carries.</param>
    /// <param name="withIntercept">Whether to fit a constant, prepended to the coefficients. Default true.</param>
    /// <returns>The coefficients, their standard errors and t statistics, and the residual sum of squares.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> has a different length, or there are no residual degrees of freedom left.</exception>
    /// <remarks>
    /// Always the Householder reflections <see cref="Fit(ReadOnlySpan{double}, ReadOnlySpan{double}, int, OlsOptions)"/> falls back to, for a caller fitting many regressions and
    /// reading a coefficient, a t statistic or a likelihood from each. It skips what <see cref="Fit(ReadOnlySpan{double}, ReadOnlySpan{double}, int, OlsOptions)"/>
    /// adds on top — p-values, intervals, R², the F test, the VIFs and the robust covariances — and agrees with
    /// <see cref="Fit(ReadOnlySpan{double}, ReadOnlySpan{double}, int, OlsOptions)"/> on the numbers it keeps to rounding, not to the bit, when that fit took the normal equations.
    /// </remarks>
    public static OlsEstimate Estimate(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        bool withIntercept = true)
    {
        Guard.NotLessThan(featureCount, 1);
        (double[] coefficients, double[] standardErrors, double[] tStatistics, double residualSumOfSquares, int residualDegreesOfFreedom) =
            HouseholderEstimate.Fit(design, response, featureCount, withIntercept);

        return new OlsEstimate
        {
            Coefficients = coefficients,
            StandardErrors = standardErrors,
            TStatistics = tStatistics,
            ResidualSumOfSquares = residualSumOfSquares,
            ResidualDegreesOfFreedom = residualDegreesOfFreedom,
            HasIntercept = withIntercept,
        };
    }

    /// <summary>The square roots of a covariance's diagonal, which are its standard errors.</summary>
    private static double[] Diagonal(double[] covariance, int order)
    {
        var errors = new double[order];
        for (int i = 0; i < order; i++)
        {
            errors[i] = Math.Sqrt(covariance[(i * order) + i]);
        }

        return errors;
    }

    /// <summary>What the model leaves unexplained, row by row.</summary>
    private static double[] Residuals(
        ReadOnlySpan<double> design,
        int rowCount,
        int featureCount,
        ReadOnlySpan<double> response,
        double[] coefficients,
        ReadOnlySpan<double> weights)
    {
        int first = coefficients.Length - featureCount;
        var residuals = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            double fitted = first == 1 ? coefficients[0] : 0.0;
            int at = row * featureCount;
            for (int column = 0; column < featureCount; column++)
            {
                fitted += design[at + column] * coefficients[column + first];
            }

            // A weighted residual is the whitened one, √w·(y − ŷ), which is what the variance and HC read.
            residuals[row] = weights.IsEmpty ? response[row] - fitted : Math.Sqrt(weights[row]) * (response[row] - fitted);
        }

        return residuals;
    }

    /// <summary>The design scaled row by row by the square root of its weight, for the robust covariances that read it; the design itself when unweighted.</summary>
    private static double[] Whiten(double[] matrix, int rowCount, int parameterCount, ReadOnlySpan<double> weights)
    {
        if (weights.IsEmpty)
        {
            return matrix;
        }

        var whitened = new double[matrix.Length];
        for (int row = 0; row < rowCount; row++)
        {
            double root = Math.Sqrt(weights[row]);
            int start = row * parameterCount;
            for (int column = 0; column < parameterCount; column++)
            {
                whitened[start + column] = matrix[start + column] * root;
            }
        }

        return whitened;
    }

    /// <summary>R²'s denominator — centred against the mean, or against zero with no intercept.</summary>
    private static double TotalSumOfSquares(ReadOnlySpan<double> response, bool withIntercept)
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

        return total;
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
    internal static double[] Vif(ReadOnlySpan<double> design, int rowCount, int regressorCount, bool withIntercept)
    {
        var factors = new double[regressorCount];
        if (regressorCount == 1 && !withIntercept)
        {
            // statsmodels raises a ValueError here: nothing is left to explain the only
            // regressor. NaN, because one undefined diagnostic does not sink a valid fit.
            factors[0] = double.NaN;
            return factors;
        }

        // long-comment: why one decomposition replaces one regression per regressor (#591).
        // Reached only when the Gram route above declines: a factor past its ceiling, or a Gram matrix
        // that is not safely positive definite — the near-perfectly collinear designs the clip is for.
        // For standardised Z the correlation matrix is ZᵀZ/n, and a VIF is a diagonal entry of
        // its inverse — the same number the auxiliary regression of a column on the others
        // reaches the long way round. Through the QR that is Zᵀ Z = RᵀR, so (ZᵀZ)⁻¹ = R⁻¹R⁻ᵀ and
        // the j-th diagonal is the squared norm of R⁻¹'s j-th row. The intercept's column drops
        // out rather than being held out: standardised regressors are centred, so a constant
        // explains none of them. Measured against tests/oracles/stats_ols.json, the identity
        // agrees with the regressions to 1.3e-12 relative on the near-collinear case whose VIF
        // is 6e4, and to 3.4e-15 or better on the other five — inside the corpus's own 1e-9.
        if (TryGramFactors(design, rowCount, regressorCount, factors))
        {
            return factors;
        }

        double[] standardised = StandardiseRegressors(design, rowCount, regressorCount);
        double[] columns = LeastSquares.ColumnMajor(standardised, rowCount, regressorCount);
        LeastSquares.Triangularize(columns, rowCount, regressorCount, projected: null);
        double[] inverseUpper = LeastSquares.InvertUpper(
            LeastSquares.Upper(columns, rowCount, regressorCount), regressorCount);

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

    /// <summary>The largest factor the Gram route is trusted to report; past it the QR route answers.</summary>
    /// <remarks>
    /// The Gram matrix squares the standardised block's condition number, so a factor near <c>κ</c> carries a
    /// relative error near <c>κ·ε</c>. Measured on the corpus's near-collinear case, a VIF of 5.9e4 lands 2.8e-11
    /// from statsmodels' (the QR route: 1.3e-12), so a ceiling of 1e5 keeps the Gram route near 5e-11, twenty
    /// times inside the corpus's 1e-9 (#782).
    /// </remarks>
    private const double GramFactorCeiling = 1e5;

    /// <summary>The VIFs from the regressors' Gram matrix, <c>n·diag((ZᵀZ)⁻¹)</c>, when that route is safe.</summary>
    /// <returns><see langword="false"/>, leaving <paramref name="factors"/> to the QR route, when the Gram matrix is not safely positive definite or a factor passes <see cref="GramFactorCeiling"/>.</returns>
    /// <remarks>
    /// Two passes over the rows and a <c>p × p</c> factorization, where the QR route is a standardised copy and
    /// reflections over every row: the VIFs were two fifths of a weighted fit's remaining time at 2 000 rows (#782).
    /// The standardisation is the same as <see cref="StandardiseRegressors"/>, applied inside the sums.
    /// </remarks>
    private static bool TryGramFactors(ReadOnlySpan<double> design, int rowCount, int order, double[] factors)
    {
        double[] gram = StandardisedGram(design, rowCount, order);
        if (!LeastSquares.TryUpperCholesky(gram, order))
        {
            return false;
        }

        // G = UᵀU, so G⁻¹ = U⁻¹U⁻ᵀ and its j-th diagonal is the squared norm of U⁻¹'s j-th row, as in the QR route.
        double[] inverseUpper = LeastSquares.InvertUpper(gram, order);
        for (int column = 0; column < order; column++)
        {
            double total = 0.0;
            for (int k = column; k < order; k++)
            {
                double entry = inverseUpper[(column * order) + k];
                total += entry * entry;
            }

            double factor = rowCount * total;
            if (!(factor <= GramFactorCeiling))
            {
                return false;
            }

            factors[column] = Clip(factor);
        }

        return true;
    }

    /// <summary><c>ZᵀZ</c> for the standardised regressors, row-major and symmetric, without forming <c>Z</c>.</summary>
    /// <remarks>
    /// Two passes over the rows whatever the regressor count: the means, then the centred cross-products, whose
    /// diagonal is each column's variance. Standardising is dividing those by the spreads afterwards, not per row.
    /// A column whose population spread is at or below 1e-10 is left uncentred and unscaled, as
    /// <see cref="StandardiseRegressors"/> leaves it.
    /// </remarks>
    private static double[] StandardisedGram(ReadOnlySpan<double> design, int rowCount, int order)
    {
        var mean = new double[order];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * order;
            for (int column = 0; column < order; column++)
            {
                mean[column] += design[at + column];
            }
        }

        for (int column = 0; column < order; column++)
        {
            mean[column] /= rowCount;
        }

        double[] centred = CentredCrossProducts(design, rowCount, mean);
        var spread = new double[order];
        bool allVary = true;
        for (int column = 0; column < order; column++)
        {
            spread[column] = Math.Sqrt(centred[(column * order) + column] / rowCount);
            allVary &= spread[column] > 1e-10;
        }

        if (!allVary)
        {
            // Rare: redo the sums with the constant columns left as they stand rather than centred.
            for (int column = 0; column < order; column++)
            {
                if (!(spread[column] > 1e-10))
                {
                    mean[column] = 0.0;
                    spread[column] = 1.0;
                }
            }

            centred = CentredCrossProducts(design, rowCount, mean);
        }

        var gram = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int j = i; j < order; j++)
            {
                double value = centred[(i * order) + j] / (spread[i] * spread[j]);
                gram[(i * order) + j] = value;
                gram[(j * order) + i] = value;
            }
        }

        return gram;
    }

    /// <summary><c>Σ (xᵢ − mᵢ)(xⱼ − mⱼ)</c> over the regressor columns, upper triangle, one pass over the rows.</summary>
    private static double[] CentredCrossProducts(ReadOnlySpan<double> design, int rowCount, double[] mean)
    {
        int order = mean.Length;
        var sums = new double[order * order];
        var deviation = new double[order];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * order;
            for (int column = 0; column < order; column++)
            {
                deviation[column] = design[at + column] - mean[column];
            }

            for (int i = 0; i < order; i++)
            {
                double di = deviation[i];
                for (int j = i; j < order; j++)
                {
                    sums[(i * order) + j] += di * deviation[j];
                }
            }
        }

        return sums;
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
    private static double[] StandardiseRegressors(ReadOnlySpan<double> design, int rowCount, int regressorCount)
    {
        var working = new double[rowCount * regressorCount];

        for (int column = 0; column < regressorCount; column++)
        {
            int source = column;
            double mean = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                mean += design[(row * regressorCount) + source];
            }

            mean /= rowCount;

            double variance = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                double deviation = design[(row * regressorCount) + source] - mean;
                variance += deviation * deviation;
            }

            double spread = Math.Sqrt(variance / rowCount);
            bool varies = spread > 1e-10;
            for (int row = 0; row < rowCount; row++)
            {
                double value = design[(row * regressorCount) + source];
                working[(row * regressorCount) + column] = varies ? (value - mean) / spread : value;
            }
        }

        return working;
    }

    /// <summary>A plain inner product; the two arrays here are always the same length.</summary>
    internal static double Dot(double[] left, double[] right)
    {
        double total = 0.0;
        for (int i = 0; i < left.Length; i++)
        {
            total += left[i] * right[i];
        }

        return total;
    }
}
