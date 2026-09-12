namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The Levinson-Durbin recursion, whose reflection coefficients are the partial
/// autocorrelations.</summary>
/// <remarks>
/// The reference solves a separate Yule-Walker system per order and keeps each solution's last
/// coefficient, quartic in the order; this recursion is the same numbers in <c>O(order^2)</c>,
/// solving order <c>k</c> from order <c>k - 1</c>. Near a series' own ceiling the Toeplitz system
/// can be singular and <c>error</c> can pass through zero with no guard -- seasonal fixture at
/// <c>len(x) / 2</c>: error -0.068, one coefficient +3.71. The reference returns numbers there
/// too rather than refusing, so this does not guard either.
/// </remarks>
internal static class LevinsonDurbin
{
    /// <summary>The reflection coefficients of an autocovariance sequence.</summary>
    /// <param name="covariance">Autocovariance at lags 0 through <paramref name="order"/>.</param>
    /// <param name="order">The highest lag to report.</param>
    /// <returns>Length <paramref name="order"/> + 1, with index 0 fixed at 1.</returns>
    internal static double[] ReflectionCoefficients(double[] covariance, int order)
    {
        var reflection = new double[order + 1];
        reflection[0] = 1.0;

        var coefficients = new double[order + 1];
        var previous = new double[order + 1];
        double error = covariance[0];

        for (int k = 1; k <= order; k++)
        {
            double numerator = covariance[k];
            for (int j = 1; j < k; j++)
            {
                numerator -= previous[j] * covariance[k - j];
            }

            double current = numerator / error;
            reflection[k] = current;
            coefficients[k] = current;
            for (int j = 1; j < k; j++)
            {
                coefficients[j] = previous[j] - (current * previous[k - j]);
            }

            error *= 1.0 - (current * current);
            Array.Copy(coefficients, previous, k + 1);
        }

        return reflection;
    }
}
