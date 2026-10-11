namespace Lodestar.Preprocessing.Internal;

/// <summary>The mean and variance of a batch folded into the ones already seen.</summary>
/// <remarks>
/// <c>sklearn.utils.extmath._incremental_mean_and_var</c>, which is Chan, Golub and LeVeque's
/// parallel form, its sums added in numpy's order as the reference adds them (#1230).
/// </remarks>
internal static class IncrementalMoments
{
    /// <summary>Folds one batch into the running moments, per feature.</summary>
    /// <param name="batch">The new samples, row-major.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="rows">How many rows <paramref name="batch"/> holds.</param>
    /// <param name="lastMean">The mean so far; read, never written, and empty where <paramref name="lastCount"/> is 0.</param>
    /// <param name="lastVariance">The population variance so far, empty with it.</param>
    /// <param name="lastCount">How many rows those two summarise.</param>
    public static (double[] Mean, double[] Variance) Update(
        ReadOnlySpan<double> batch,
        int featureCount,
        int rows,
        ReadOnlySpan<double> lastMean,
        ReadOnlySpan<double> lastVariance,
        int lastCount)
    {
        var mean = new double[featureCount];
        var variance = new double[featureCount];
        int updatedCount = lastCount + rows;

        // The reference's sums, in numpy's order, as StandardScaler.Fit's: compensated, a nearly constant column's
        // variance had drifted 1.8% from scikit-learn's over a partial fit (#1230).
        var newSums = new double[featureCount];
        var corrections = new double[featureCount];
        var squareSums = new double[featureCount];
        NumpyReduction.ColumnBatchSums(batch, rows, featureCount, newSums, corrections, squareSums);
        for (int feature = 0; feature < featureCount; feature++)
        {
            double newSum = newSums[feature];
            double correction = corrections[feature];
            double lastSum = lastCount == 0 ? 0.0 : lastMean[feature] * lastCount;
            mean[feature] = (lastSum + newSum) / updatedCount;

            double newUnnormalised = squareSums[feature] - (correction * correction / rows);
            if (lastCount == 0)
            {
                variance[feature] = newUnnormalised / updatedCount;
                continue;
            }

            double lastUnnormalised = lastVariance[feature] * lastCount;
            // The square before the product, the reference's `a / b * d ** 2`: a square that overflows makes it ∞.
            double lastOverNew = (double)lastCount / rows;
            double difference = (lastSum / lastOverNew) - newSum;
            double updated = lastUnnormalised
                + newUnnormalised
                + (lastOverNew / updatedCount * (difference * difference));

            variance[feature] = updated / updatedCount;
        }

        return (mean, variance);
    }
}
