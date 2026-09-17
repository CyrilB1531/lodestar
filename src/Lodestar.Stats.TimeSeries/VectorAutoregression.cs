using Lodestar.Stats.Regression;
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>
/// A vector autoregression with the inference table on top of it, at <c>statsmodels.tsa.api.VAR</c> parity.
/// </summary>
/// <remarks>
/// Several series that move together, each explained by every series' own past. The fit is least squares equation by
/// equation on the stacked lags — <see cref="OrdinaryLeastSquares.Estimate"/>'s arithmetic per equation, over one shared QR — which is why
/// <see href="https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md">decision 0134</see>
/// could write this model and not the likelihood-fitted ones beside it.
/// </remarks>
public static class VectorAutoregression
{
    /// <summary>Fits a VAR of the given lag order and reports what a summary table holds.</summary>
    /// <param name="series">The observations, row-major in time: <paramref name="variableCount"/> values each, oldest first.</param>
    /// <param name="variableCount">How many variables each observation carries; at least two.</param>
    /// <param name="lagOrder">How many lags enter each equation; at least one.</param>
    /// <param name="options">Whether to fit a constant, or null for the reference's default of one.</param>
    /// <returns>The coefficients per equation with their errors, t statistics and p-values, the residual covariances, and the criteria.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="variableCount"/> is below two, or <paramref name="lagOrder"/> is below one.</exception>
    /// <exception cref="ArgumentException"><paramref name="series"/> is not a whole number of observations, holds a non-finite value, or leaves no residual degree of freedom after the lags are taken.</exception>
    /// <remarks>
    /// The p-values read the normal, as the reference's do, and no intervals are reported because the reference
    /// publishes none for this model.
    /// </remarks>
    public static VarSummary Fit(
        ReadOnlySpan<double> series,
        int variableCount,
        int lagOrder,
        VarOptions? options = null)
    {
        Guard.NotLessThan(variableCount, 2);
        Guard.NotLessThan(lagOrder, 1);
        VarOptions settings = options ?? new VarOptions();
        int observations = Observations(series, variableCount);
        int usable = observations - lagOrder;
        int parameters = (settings.WithIntercept ? 1 : 0) + (variableCount * lagOrder);
        if (usable - parameters < 1)
        {
            throw new ArgumentException(
                $"{observations} observations of {variableCount} variables at lag {lagOrder} leave {usable} usable rows "
                + $"for {parameters} parameters per equation, and every standard error here divides by their difference.",
                nameof(series));
        }

        double[] design = LagDesign.Stack(series, variableCount, lagOrder, settings.WithIntercept, out double[][] responses);
        var coefficients = new double[variableCount][];
        var errors = new double[variableCount][];
        var residuals = new double[variableCount][];

        // Every equation shares the design, so one QR and one inverse of R serve them all.
        var reflections = new SharedReflections(design, parameters, withIntercept: false, responses);
        reflections.ReflectThrough(parameters);
        double[] inverse = reflections.InverseUpper(parameters);
        double[] squaredNorms = SharedReflections.SquaredNorms(inverse, parameters);
        for (int equation = 0; equation < variableCount; equation++)
        {
            (coefficients[equation], errors[equation]) = reflections.Estimates(
                equation, parameters, inverse, squaredNorms, reflections.ResidualSumOfSquares(equation, parameters));
            residuals[equation] = Residuals(design, responses[equation], coefficients[equation], parameters);
        }

        return Tabulate(new VarFit(coefficients, errors, residuals, usable, parameters), variableCount, lagOrder, settings);
    }

    /// <summary>What the table is read from: the per-equation estimates, errors and residual rows, and the two counts they share.</summary>
    private sealed record VarFit(
        double[][] Estimates, double[][] Errors, double[][] Rows, int Usable, int Parameters);

    private static int Observations(ReadOnlySpan<double> series, int variableCount)
    {
        if (series.Length == 0 || series.Length % variableCount != 0)
        {
            throw new ArgumentException(
                $"series holds {series.Length} values, which is not a whole number of observations of {variableCount} variables.",
                nameof(series));
        }

        for (int i = 0; i < series.Length; i++)
        {
            if (double.IsNaN(series[i]) || double.IsInfinity(series[i]))
            {
                throw new ArgumentException($"series[{i}] is {series[i]}, and a fit reads every observation.", nameof(series));
            }
        }

        return series.Length / variableCount;
    }

    private static double[] Residuals(double[] design, double[] response, double[] coefficients, int parameters)
    {
        var residuals = new double[response.Length];
        for (int row = 0; row < response.Length; row++)
        {
            double fitted = 0.0;
            int at = row * parameters;
            for (int column = 0; column < parameters; column++)
            {
                fitted += design[at + column] * coefficients[column];
            }

            residuals[row] = response[row] - fitted;
        }

        return residuals;
    }

    private static VarSummary Tabulate(VarFit fit, int variableCount, int lagOrder, VarOptions settings)
    {
        int usable = fit.Usable;
        var z = new double[variableCount][];
        var p = new double[variableCount][];
        for (int equation = 0; equation < variableCount; equation++)
        {
            z[equation] = new double[fit.Parameters];
            p[equation] = new double[fit.Parameters];
            for (int column = 0; column < fit.Parameters; column++)
            {
                double statistic = fit.Estimates[equation][column] / fit.Errors[equation][column];
                z[equation][column] = statistic;
                // The reference reads these against the normal, not Student's t, and the square of a standard
                // normal is chi-squared on one degree of freedom.
                p[equation][column] = Distributions.ChiSquaredSf(statistic * statistic, 1.0);
            }
        }

        double[] crossProducts = CrossProducts(fit.Rows, usable);
        var covariance = new double[crossProducts.Length];
        var maximumLikelihood = new double[crossProducts.Length];
        for (int i = 0; i < crossProducts.Length; i++)
        {
            covariance[i] = crossProducts[i] / (usable - fit.Parameters);
            maximumLikelihood[i] = crossProducts[i] / usable;
        }

        double logDeterminant = Math.Log(Determinant(maximumLikelihood, variableCount));
        // The reference counts free_params = df_model * neqs, so a fit without a constant drops K of them:
        // (K·p + 1)·K with one, K²p without.
        double free = (double)fit.Parameters * variableCount;
        return new VarSummary
        {
            Coefficients = fit.Estimates,
            StandardErrors = fit.Errors,
            TStatistics = z,
            PValues = p,
            ResidualCovariance = covariance,
            ResidualCovarianceMaximumLikelihood = maximumLikelihood,
            LogLikelihood = (-0.5 * usable * variableCount * Math.Log(2.0 * Math.PI))
                - (0.5 * usable * logDeterminant) - (0.5 * usable * variableCount),
            Akaike = logDeterminant + (2.0 * free / usable),
            Bayesian = logDeterminant + (free * Math.Log(usable) / usable),
            HannanQuinn = logDeterminant + (2.0 * free * Math.Log(Math.Log(usable)) / usable),
            FinalPredictionError = Determinant(maximumLikelihood, variableCount)
                * Math.Pow((usable + fit.Parameters) / (double)(usable - fit.Parameters), variableCount),
            LagOrder = lagOrder,
            VariableCount = variableCount,
            ObservationsUsed = usable,
            ModelDegreesOfFreedom = fit.Parameters,
            ResidualDegreesOfFreedom = usable - fit.Parameters,
            HasIntercept = settings.WithIntercept,
        };
    }

    /// <summary><c>residualᵀresidual</c>, row-major and symmetric.</summary>
    private static double[] CrossProducts(double[][] residuals, int usable)
    {
        int variableCount = residuals.Length;
        var products = new double[variableCount * variableCount];
        for (int i = 0; i < variableCount; i++)
        {
            for (int j = 0; j < variableCount; j++)
            {
                double total = 0.0;
                for (int row = 0; row < usable; row++)
                {
                    total += residuals[i][row] * residuals[j][row];
                }

                products[(i * variableCount) + j] = total;
            }
        }

        return products;
    }

    /// <summary>The determinant of a small symmetric matrix, by Gaussian elimination with partial pivoting.</summary>
    /// <remarks>
    /// The residual covariance is positive definite for any design the fit accepted, so a Cholesky would serve; the
    /// elimination is here because it also reports a singular matrix as a zero determinant rather than a failure, and
    /// the criteria above then read as the infinities they are.
    /// </remarks>
    private static double Determinant(double[] matrix, int order)
    {
        double[] working = [.. matrix];
        double determinant = 1.0;
        for (int pivot = 0; pivot < order; pivot++)
        {
            determinant *= Pivot(working, order, pivot) ? 1.0 : -1.0;
            double head = working[(pivot * order) + pivot];
            determinant *= head;
            if (head == 0.0)
            {
                return 0.0;
            }

            Eliminate(working, order, pivot, head);
        }

        return determinant;
    }

    /// <summary>Moves the largest remaining entry of the pivot's column onto the diagonal; false when a swap was needed.</summary>
    private static bool Pivot(double[] working, int order, int pivot)
    {
        int largest = pivot;
        for (int row = pivot + 1; row < order; row++)
        {
            if (Math.Abs(working[(row * order) + pivot]) > Math.Abs(working[(largest * order) + pivot]))
            {
                largest = row;
            }
        }

        if (largest == pivot)
        {
            return true;
        }

        for (int column = 0; column < order; column++)
        {
            (working[(pivot * order) + column], working[(largest * order) + column]) =
                (working[(largest * order) + column], working[(pivot * order) + column]);
        }

        return false;
    }

    /// <summary>Clears the pivot's column below the diagonal.</summary>
    private static void Eliminate(double[] working, int order, int pivot, double head)
    {
        for (int row = pivot + 1; row < order; row++)
        {
            double factor = working[(row * order) + pivot] / head;
            for (int column = pivot; column < order; column++)
            {
                working[(row * order) + column] -= factor * working[(pivot * order) + column];
            }
        }
    }
}
