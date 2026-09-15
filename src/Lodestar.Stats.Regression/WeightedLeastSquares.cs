using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>
/// Weighted least squares with the inference table on top of it, at
/// <c>statsmodels.api.WLS</c> parity.
/// </summary>
/// <remarks>
/// The same table as <see cref="OrdinaryLeastSquares"/>, for rows that are not equally reliable: a
/// weight is proportional to the inverse of its row's variance, as the reference reads it.
/// </remarks>
public static class WeightedLeastSquares
{
    /// <summary>Fits a linear model with one weight per row and reports what a summary table holds.</summary>
    /// <param name="design">The regressors, row-major: <paramref name="featureCount"/> values per row, with no constant column of your own.</param>
    /// <param name="response">One observed value per row of <paramref name="design"/>.</param>
    /// <param name="weights">One non-negative, finite weight per row, proportional to the inverse of that row's variance.</param>
    /// <param name="featureCount">How many regressors each row carries.</param>
    /// <param name="options">Whether to fit an intercept, which covariance, and at what confidence; <see langword="null"/> fits one at 0.95.</param>
    /// <returns>The fitted model, with its standard errors, t statistics, p-values, intervals and VIFs.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or a weight is negative, <c>NaN</c> or infinite.</exception>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows, <paramref name="response"/> or <paramref name="weights"/> has a different length, no residual degrees of freedom are left, or fewer rows carry a positive weight than there are parameters.</exception>
    /// <remarks>
    /// Every row and its response is scaled by the square root of its weight and fitted as
    /// <see cref="OrdinaryLeastSquares"/> would, robust covariances included. R² is the reference's
    /// weighted one rather than the scaled rows' own, a zero weight keeps its row in the degrees of
    /// freedom, and the VIFs read the design as given.
    /// </remarks>
    public static OlsSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> weights,
        int featureCount,
        OlsOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        OlsOptions settings = options ?? new OlsOptions();
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        OrdinaryLeastSquares.RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
        CheckWeights(weights, rowCount, parameterCount);

        return OrdinaryLeastSquares.Summarise(
            design,
            response,
            TotalSumOfSquares(response, weights, settings.WithIntercept),
            rowCount,
            featureCount,
            settings,
            weights);
    }

    /// <summary>Refuses the weights the reference would crash on, propagate as NaN, or answer with a pseudo-inverse.</summary>
    private static void CheckWeights(ReadOnlySpan<double> weights, int rowCount, int parameterCount)
    {
        if (weights.Length != rowCount)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows and weights holds {weights.Length} values.",
                nameof(weights));
        }

        int positive = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            double weight = weights[i];
            if (double.IsNaN(weight) || double.IsInfinity(weight) || weight < 0.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weights), weight, $"weights[{i}] is not a finite, non-negative number.");
            }

            if (weight > 0.0)
            {
                positive++;
            }
        }

        if (positive < parameterCount)
        {
            // The reference answers through a pseudo-inverse here, picking one of the infinitely
            // many estimates that fit the weighted rows equally well; the QR would divide by zero.
            throw new ArgumentException(
                $"{positive} rows carry a positive weight and the model has {parameterCount} parameters.",
                nameof(weights));
        }
    }

    /// <summary>R²'s denominator, weighted: centred on the weighted mean, or against zero with no intercept.</summary>
    private static double TotalSumOfSquares(
        ReadOnlySpan<double> response, ReadOnlySpan<double> weights, bool withIntercept)
    {
        double total = 0.0;
        if (withIntercept)
        {
            double weightedSum = 0.0;
            double weightTotal = 0.0;
            for (int i = 0; i < response.Length; i++)
            {
                weightedSum += weights[i] * response[i];
                weightTotal += weights[i];
            }

            double mean = weightedSum / weightTotal;
            for (int i = 0; i < response.Length; i++)
            {
                double deviation = response[i] - mean;
                total += weights[i] * deviation * deviation;
            }
        }
        else
        {
            for (int i = 0; i < response.Length; i++)
            {
                total += weights[i] * response[i] * response[i];
            }
        }

        return total;
    }
}
