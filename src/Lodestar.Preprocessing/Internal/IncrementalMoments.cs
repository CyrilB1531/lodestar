namespace Lodestar.Preprocessing.Internal;

/// <summary>The mean and variance of a batch folded into the ones already seen.</summary>
/// <remarks>
/// <c>sklearn.utils.extmath._incremental_mean_and_var</c>, which is Chan, Golub and LeVeque's
/// parallel form. The correction term is what holds the result within a couple of units in the last
/// place of a fit over the whole matrix — measured at 3.3e-16 relative, not zero; the corpus freezes
/// both paths and compares them.
/// </remarks>
internal static class IncrementalMoments
{
    /// <summary>Folds one batch into the running moments, per feature.</summary>
    /// <param name="batch">The new samples, row-major.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="rows">How many rows <paramref name="batch"/> holds.</param>
    /// <param name="lastMean">The mean so far; read, never written.</param>
    /// <param name="lastVariance">The population variance so far.</param>
    /// <param name="lastCount">How many rows those two summarise.</param>
    public static (double[] Mean, double[] Variance) Update(
        ReadOnlySpan<double> batch,
        int featureCount,
        int rows,
        double[] lastMean,
        double[] lastVariance,
        int lastCount)
    {
        var mean = new double[featureCount];
        var variance = new double[featureCount];
        int updatedCount = lastCount + rows;

        for (int feature = 0; feature < featureCount; feature++)
        {
            // Compensated, as StandardScaler.Fit's own passes are, where the reference accumulates
            // plainly: two paths of one class should be equally careful (#765, measured).
            double newSum = 0.0;
            double newLost = 0.0;
            for (int row = 0; row < rows; row++)
            {
                Add(ref newSum, ref newLost, batch[(row * featureCount) + feature]);
            }

            newSum += newLost;

            double lastSum = lastMean[feature] * lastCount;
            mean[feature] = (lastSum + newSum) / updatedCount;

            // Taken about the batch's own mean and corrected afterwards: the two-pass form the
            // reference uses, which is what holds the gap to a couple of units in the last place.
            double batchMean = newSum / rows;
            double correction = 0.0;
            double correctionLost = 0.0;
            double squares = 0.0;
            double squaresLost = 0.0;
            for (int row = 0; row < rows; row++)
            {
                double deviation = batch[(row * featureCount) + feature] - batchMean;
                Add(ref correction, ref correctionLost, deviation);
                Add(ref squares, ref squaresLost, deviation * deviation);
            }

            correction += correctionLost;
            squares += squaresLost;

            double newUnnormalised = squares - (correction * correction / rows);
            if (lastCount == 0)
            {
                variance[feature] = newUnnormalised / updatedCount;
                continue;
            }

            double lastUnnormalised = lastVariance[feature] * lastCount;
            double lastOverNew = (double)lastCount / rows;
            double difference = (lastSum / lastOverNew) - newSum;
            double updated = lastUnnormalised
                + newUnnormalised
                + (lastOverNew / updatedCount * difference * difference);

            variance[feature] = updated / updatedCount;
        }

        return (mean, variance);
    }

    /// <summary>Neumaier's compensated addition, the one <see cref="StandardScaler"/>'s own passes use.</summary>
    private static void Add(ref double total, ref double lost, double value)
    {
        double sum = total + value;
        lost += Math.Abs(total) >= Math.Abs(value)
            ? (total - sum) + value
            : (value - sum) + total;
        total = sum;
    }
}
