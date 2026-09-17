using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>
/// The multinomial logit with its inference table, at <c>statsmodels.discrete.discrete_model.MNLogit</c> parity.
/// </summary>
/// <remarks>
/// A response with more than two unordered categories, each non-reference category getting its own equation against the
/// smallest label. Fitted by Newton-Raphson on the analytic score and Hessian, which reproduces the reference at
/// <c>1e-15</c>; the ordered model beside it in <c>statsmodels</c> does not reproduce, and is not written (decision 0136).
/// </remarks>
public static class MultinomialLogit
{
    /// <summary>Fits one model and reports its inference table.</summary>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row, with no constant column of your own.</param>
    /// <param name="response">One category label per row: any integers, at least two distinct.</param>
    /// <param name="featureCount">How many regressors a row carries.</param>
    /// <param name="options">The fit's settings, or null for the reference's defaults.</param>
    /// <returns>The coefficients per non-reference category with their errors, z statistics, p-values and intervals, and the whole-model table.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> has another length or fewer than two categories, no residual degree of freedom is left, or the Hessian is not positive definite — a separated category or a rank-deficient design.</exception>
    /// <exception cref="InvalidOperationException">Newton spent its budget and <see cref="MultinomialLogitOptions.ThrowOnNonConvergence"/> says throw.</exception>
    /// <remarks>
    /// <see cref="MultinomialLogitSummary.NullLogLikelihood"/> is the closed form <c>Σ nⱼ·log(nⱼ/n)</c>, which the reference
    /// approximates by an optimiser to within <c>3e-10</c>. A separated response is refused where the reference returns NaN.
    /// </remarks>
    public static MultinomialLogitSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<int> response,
        int featureCount,
        MultinomialLogitOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        MultinomialLogitOptions settings = options ?? new MultinomialLogitOptions();
        int rowCount = response.Length;
        if (design.Length == 0 || design.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"design holds {design.Length} values, which is not a positive whole number of rows of {featureCount}.",
                nameof(design));
        }

        if (design.Length / featureCount != rowCount)
        {
            // The design's shape is sound, so the label count is what disagrees, as OLS and the GLM name it (#905).
            throw new ArgumentException(
                $"design has {design.Length / featureCount} rows and response holds {rowCount} labels.",
                nameof(response));
        }

        (int[] categories, int[] labels, int[] counts) = Categorise(response);
        int columnCount = featureCount + (settings.WithIntercept ? 1 : 0);
        int equations = categories.Length - 1;
        int modelDegreesOfFreedom = (columnCount - 1) * equations;
        int residualDegreesOfFreedom = rowCount - modelDegreesOfFreedom - equations;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows fit {columnCount * equations} parameters with {residualDegreesOfFreedom} residual degrees "
                + "of freedom left.", nameof(design));
        }

        double[] matrix = LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept);
        MultinomialFit fit = MultinomialNewton.Fit(matrix, labels, columnCount, categories.Length, settings, nameof(design));
        if (!fit.Converged && settings.ThrowOnNonConvergence)
        {
            throw new InvalidOperationException(
                $"Newton spent all {fit.Iterations} iterations without every step falling to {settings.Tolerance:G3}. The fit "
                + $"is not usable; set {nameof(MultinomialLogitOptions.ThrowOnNonConvergence)} to false to inspect it.");
        }

        return Tabulate(fit, categories, counts, columnCount, modelDegreesOfFreedom, residualDegreesOfFreedom, settings);
    }

    /// <summary>The distinct labels in ascending order, each row's index among them, and how many rows each holds.</summary>
    private static (int[] Categories, int[] Labels, int[] Counts) Categorise(ReadOnlySpan<int> response)
    {
        int[] sorted = [.. response];
        Array.Sort(sorted);
        var distinct = new List<int>();
        for (int i = 0; i < sorted.Length; i++)
        {
            if (i == 0 || sorted[i] != sorted[i - 1])
            {
                distinct.Add(sorted[i]);
            }
        }

        if (distinct.Count < 2)
        {
            throw new ArgumentException(
                $"The response holds {distinct.Count} distinct label(s); a multinomial logit compares at least two.",
                nameof(response));
        }

        int[] categories = [.. distinct];
        var labels = new int[response.Length];
        var counts = new int[categories.Length];
        for (int row = 0; row < response.Length; row++)
        {
            int index = Array.BinarySearch(categories, response[row]);
            labels[row] = index;
            counts[index]++;
        }

        return (categories, labels, counts);
    }

    private static MultinomialLogitSummary Tabulate(
        MultinomialFit fit,
        int[] categories,
        int[] counts,
        int columnCount,
        int modelDegreesOfFreedom,
        int residualDegreesOfFreedom,
        MultinomialLogitOptions settings)
    {
        int equations = categories.Length - 1;
        int rowCount = residualDegreesOfFreedom + modelDegreesOfFreedom + equations;
        double multiplier = Distributions.NormalQuantile(1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));
        var coefficients = new double[equations][];
        var errors = new double[equations][];
        var z = new double[equations][];
        var p = new double[equations][];
        var lower = new double[equations][];
        var upper = new double[equations][];
        for (int j = 0; j < equations; j++)
        {
            coefficients[j] = new double[columnCount];
            errors[j] = new double[columnCount];
            z[j] = new double[columnCount];
            p[j] = new double[columnCount];
            lower[j] = new double[columnCount];
            upper[j] = new double[columnCount];
            for (int k = 0; k < columnCount; k++)
            {
                double estimate = fit.Coefficients[(j * columnCount) + k];
                double error = Math.Sqrt(fit.Variances[(j * columnCount) + k]);
                coefficients[j][k] = estimate;
                errors[j][k] = error;
                z[j][k] = estimate / error;
                // The two-sided normal tail through chi-square(1), as GeneralizedLinearModel reads it.
                p[j][k] = Distributions.ChiSquaredSf(z[j][k] * z[j][k], 1.0);
                lower[j][k] = estimate - (multiplier * error);
                upper[j][k] = estimate + (multiplier * error);
            }
        }

        double nullLogLikelihood = 0.0;
        for (int j = 0; j < counts.Length; j++)
        {
            nullLogLikelihood += counts[j] * Math.Log((double)counts[j] / rowCount);
        }

        double ratio = -2.0 * (nullLogLikelihood - fit.LogLikelihood);
        int parameters = modelDegreesOfFreedom + equations;
        return new MultinomialLogitSummary
        {
            Categories = categories,
            Coefficients = coefficients,
            StandardErrors = errors,
            ZStatistics = z,
            PValues = p,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            LogLikelihood = fit.LogLikelihood,
            NullLogLikelihood = nullLogLikelihood,
            PseudoRSquared = 1.0 - (fit.LogLikelihood / nullLogLikelihood),
            LikelihoodRatio = ratio,
            LikelihoodRatioPValue = Distributions.ChiSquaredSf(ratio, modelDegreesOfFreedom),
            Akaike = -2.0 * (fit.LogLikelihood - parameters),
            Bayesian = (-2.0 * fit.LogLikelihood) + (Math.Log(rowCount) * parameters),
            ModelDegreesOfFreedom = modelDegreesOfFreedom,
            ResidualDegreesOfFreedom = residualDegreesOfFreedom,
            HasIntercept = settings.WithIntercept,
            ConfidenceLevel = settings.ConfidenceLevel,
            Converged = fit.Converged,
            Iterations = fit.Iterations,
        };
    }
}
