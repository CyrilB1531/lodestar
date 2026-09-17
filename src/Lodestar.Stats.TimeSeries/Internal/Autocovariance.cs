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

        // Centred once rather than twice per product: the same subtractions, so the same doubles.
        var centred = new double[n];
        for (int i = 0; i < n; i++)
        {
            centred[i] = series[i] - mean;
        }

        var result = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            ReadOnlySpan<double> early = centred.AsSpan(0, n - lag);
            ReadOnlySpan<double> late = centred.AsSpan(lag, early.Length);
            double total = 0.0;
            for (int t = 0; t < early.Length; t++)
            {
                total += early[t] * late[t];
            }

            result[lag] = total / (adjusted ? n - lag : n);
        }

        return result;
    }
}
