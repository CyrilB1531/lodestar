using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>
/// Every statistic here is invariant to the series' scale, and a power of two scales each value exactly, so each reads
/// the same doubles at every scale: KPSS, the autocorrelations and Ljung-Box read NaN at 2^600 and 2^-1000 (#1779).
/// </summary>
public sealed class ScaleInvarianceTests
{
    public static TheoryData<int> Exponents => new() { -1000, -600, -129, -128, -1, 1, 127, 128, 600, 1015 };

    // A 120-point AR(1) at 0.5, its largest magnitude below one.
    private static readonly double[] Series = Autoregressive(120);

    private static double[] Scaled(int exponent) => [.. Series.Select(value => value * Math.Pow(2.0, exponent))];

    [Theory]
    [MemberData(nameof(Exponents))]
    public void Kpss_reads_the_same_statistic_at_every_scale(int exponent)
    {
        foreach (TrendTerms trend in new[] { TrendTerms.Constant, TrendTerms.ConstantAndTrend })
        {
            var options = new KpssOptions { Regression = trend };
            KpssResult expected = Stationarity.Kpss(Series, options);
            KpssResult actual = Stationarity.Kpss(Scaled(exponent), options);

            Assert.Equal(expected.Statistic, actual.Statistic);
            Assert.Equal(expected.LagCount, actual.LagCount);
        }
    }

    [Theory]
    [MemberData(nameof(Exponents))]
    public void The_autocorrelations_read_the_same_values_at_every_scale(int exponent)
    {
        double[] series = Scaled(exponent);

        Assert.Equal(SerialCorrelation.Autocorrelation(Series, 5).Values, SerialCorrelation.Autocorrelation(series, 5).Values);
        Assert.Equal(
            SerialCorrelation.PartialAutocorrelation(Series, 5).Values, SerialCorrelation.PartialAutocorrelation(series, 5).Values);
        Assert.Equal(SerialCorrelation.LjungBox(Series, 5).Statistics, SerialCorrelation.LjungBox(series, 5).Statistics);
    }

    /// <summary>The regression reads each value scaled as it builds its design, the copy saved, so no bit moves (#1779).</summary>
    [Theory]
    [MemberData(nameof(Exponents))]
    public void Adf_reads_the_same_statistic_at_every_scale(int exponent)
    {
        foreach (LagSelection selection in new[] { LagSelection.Akaike, LagSelection.TStatistic })
        {
            var options = new DickeyFullerOptions { Regression = TrendTerms.ConstantAndTrend, LagSelection = selection };
            DickeyFullerResult expected = Stationarity.AugmentedDickeyFuller(Series, options);
            DickeyFullerResult actual = Stationarity.AugmentedDickeyFuller(Scaled(exponent), options);

            Assert.Equal(expected.Statistic, actual.Statistic);
            Assert.Equal(expected.UsedLag, actual.UsedLag);
        }
    }

    private static double[] Autoregressive(int n)
    {
        // CA5394: a seeded Random draws reproducible test data; no security use.
#pragma warning disable CA5394
        var random = new Random(4);
        var series = new double[n];
        double level = 0.0;
        for (int i = 0; i < n; i++)
        {
            level = (0.5 * level) + random.NextDouble() - 0.5;
            series[i] = level;
        }
#pragma warning restore CA5394

        return series;
    }
}
