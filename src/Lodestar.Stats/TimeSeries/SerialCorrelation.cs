using System.Globalization;
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a series carries serial dependence, and at which lags.</summary>
public static class SerialCorrelation
{
    /// <summary>The autocorrelation function, with its confidence band.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lagCount">
    /// How many lags past zero to report. Required rather than defaulted: the reference defaults
    /// it to <c>min(10*log10(n), n - 1)</c> here and to <c>min(10*log10(n), n/2 - 1)</c> for the
    /// partial function, and a default that differs between two functions read side by side is a
    /// trap. Pass either rule deliberately to reproduce a reference plot.
    /// </param>
    /// <param name="options">The estimator, the band and its level, or null for the defaults.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> holds fewer than two points, is constant, or carries a non-finite
    /// value; or <paramref name="lagCount"/> is below one or reaches the series length.
    /// </exception>
    public static AutocorrelationResult Autocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null)
    {
        AutocorrelationOptions settings = options ?? new AutocorrelationOptions();
        RefuseUnusableSeries(series, lagCount, series.Length - 1);

        // Lag zero divides by n - 0, which is n, whichever flag was passed -- so the adjusted
        // estimator is a bigger numerator over the same denominator, as the reference's is.
        double[] covariance = Autocovariance.Of(series, lagCount, settings.Adjusted);

        var values = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            values[lag] = covariance[lag] / covariance[0];
        }

        double multiplier = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));
        double[] variance = BandVariance(values, series.Length, settings.BartlettConfidenceInterval);

        var lower = new double[lagCount + 1];
        var upper = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            double half = multiplier * Math.Sqrt(variance[lag]);
            lower[lag] = values[lag] - half;
            upper[lag] = values[lag] + half;
        }

        return new AutocorrelationResult
        {
            Values = values,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
        };
    }

    /// <summary>The partial autocorrelation function, with its confidence band.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lagCount">
    /// How many lags past zero to report, at most half the series length. The reference defaults
    /// it to <c>min(10*log10(n), n/2 - 1)</c>; this asks rather than defaulting, for the reason
    /// <see cref="Autocorrelation"/> gives.
    /// </param>
    /// <param name="options">
    /// Only <see cref="AutocorrelationOptions.ConfidenceLevel"/> is read. <c>Adjusted</c> is fixed
    /// here — the reference's <c>ywadjusted</c> method is the adjusted estimator by definition —
    /// and Bartlett's formula does not apply to a partial autocorrelation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> holds fewer than two points, is constant, or carries a non-finite
    /// value; or <paramref name="lagCount"/> is below one or above half the series length.
    /// </exception>
    public static AutocorrelationResult PartialAutocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null)
    {
        AutocorrelationOptions settings = options ?? new AutocorrelationOptions();
        RefuseUnusableSeries(series, lagCount, series.Length / 2);

        double[] covariance = Autocovariance.Of(series, lagCount, adjusted: true);
        double[] values = LevinsonDurbin.ReflectionCoefficients(covariance, lagCount);

        // Quenouille: a partial autocorrelation past the true order is asymptotically N(0, 1/n),
        // so the band is flat rather than Bartlett's widening one.
        double half = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0)) / Math.Sqrt(series.Length);

        var lower = new double[lagCount + 1];
        var upper = new double[lagCount + 1];
        lower[0] = values[0];
        upper[0] = values[0];
        for (int lag = 1; lag <= lagCount; lag++)
        {
            lower[lag] = values[lag] - half;
            upper[lag] = values[lag] + half;
        }

        return new AutocorrelationResult
        {
            Values = values,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
        };
    }

    /// <summary>Bartlett's widening variance, or the flat one.</summary>
    /// <remarks>
    /// Bartlett's is <c>(1 + 2*sum_{j&lt;k} r_j^2) / n</c> past lag one, which is the reference's
    /// default. Lag zero is exactly zero either way, so its interval is the point 1.
    /// </remarks>
    private static double[] BandVariance(double[] values, int n, bool bartlett)
    {
        var variance = new double[values.Length];
        if (!bartlett)
        {
            for (int lag = 1; lag < values.Length; lag++)
            {
                variance[lag] = 1.0 / n;
            }

            return variance;
        }

        double running = 0.0;
        for (int lag = 1; lag < values.Length; lag++)
        {
            variance[lag] = (1.0 + (2.0 * running)) / n;
            running += values[lag] * values[lag];
        }

        return variance;
    }

    /// <summary>The refusals every member of this class shares.</summary>
    /// <param name="series">The observations.</param>
    /// <param name="lagCount">The requested lag count.</param>
    /// <param name="lagCeiling">The largest lag this member can answer for.</param>
    internal static void RefuseUnusableSeries(
        ReadOnlySpan<double> series, int lagCount, int lagCeiling)
    {
        if (series.Length < 2)
        {
            throw new ArgumentException(
                $"a series of {series.Length} carries no correlation: two points are the "
                + "minimum.", nameof(series));
        }

        for (int row = 0; row < series.Length; row++)
        {
            // double.IsFinite is not on netstandard2.0, so the two halves are asked separately --
            // Lodestar.Stats.Regression's Irls.AllFinite records the same constraint.
            if (double.IsNaN(series[row]) || double.IsInfinity(series[row]))
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row].ToString(CultureInfo.InvariantCulture)}, which would propagate through every lag. "
                    + "A gapped series needs the interpolation this does not do.", nameof(series));
            }
        }

        // S1244: a constant series is exactly constant or it is not -- a tolerance band here
        // would refuse a series that merely varies little, which is a different thing.
#pragma warning disable S1244
        bool constant = true;
        for (int row = 1; row < series.Length && constant; row++)
        {
            constant = series[row] == series[0];
        }
#pragma warning restore S1244

        if (constant)
        {
            throw new ArgumentException(
                $"every value is {series[0].ToString(CultureInfo.InvariantCulture)}, so the lag-zero autocovariance is zero and every "
                + "correlation would be 0/0. The reference answers NaN; this refuses, as "
                + "KruskalWallis.Test refuses a fully tied sample.", nameof(series));
        }

        if (lagCount < 1)
        {
            throw new ArgumentException(
                $"a lag count of {lagCount} asks for nothing: one or more.", nameof(lagCount));
        }

        if (lagCount > lagCeiling)
        {
            throw new ArgumentException(
                $"a lag count of {lagCount} is above the {lagCeiling} a series of "
                + $"{series.Length} supports.", nameof(lagCount));
        }
    }
}
