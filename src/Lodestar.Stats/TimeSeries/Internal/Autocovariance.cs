namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The sample autocovariance the three diagnostics all start from.</summary>
internal static class Autocovariance
{
    /// <summary>Autocovariance at lags 0 through <paramref name="lagCount"/>, inclusive.</summary>
    /// <remarks>
    /// <paramref name="adjusted"/> false divides every lag by <c>n</c>, which is the reference's
    /// default and the estimator that keeps the sequence positive semi-definite; true divides lag
    /// <c>k</c> by <c>n - k</c> instead. Lag zero divides by <c>n</c> either way, because
    /// <c>n - 0</c> is <c>n</c>.
    /// </remarks>
    internal static double[] Of(ReadOnlySpan<double> series, int lagCount, bool adjusted)
    {
        int n = series.Length;
        double mean = 0.0;
        for (int i = 0; i < n; i++)
        {
            mean += series[i];
        }

        mean /= n;

        var result = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            double total = 0.0;
            for (int t = 0; t + lag < n; t++)
            {
                total += (series[t] - mean) * (series[t + lag] - mean);
            }

            result[lag] = total / (adjusted ? n - lag : n);
        }

        return result;
    }
}
