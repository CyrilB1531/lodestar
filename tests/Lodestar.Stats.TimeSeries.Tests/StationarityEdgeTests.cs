using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Every refusal the spec's table lists, by parameter name and a fragment of the message.</summary>
public sealed class StationarityEdgeTests
{
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0];

    [Fact]
    public void Adf_refuses_a_non_finite_value()
    {
        double[] series = (double[])Walk.Clone();
        series[3] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(() => Stationarity.AugmentedDickeyFuller(series));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("row 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_constant_series()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller([2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0]));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("constant", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_maximum_lag_above_what_the_series_supports()
    {
        // 20 points under a constant: 20/2 − 1 − 1 = 8.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(Walk, new DickeyFullerOptions { MaxLag = 9 }));
        Assert.Equal("options", error.ParamName);
        Assert.Contains("8", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_series_too_short_for_its_trend_terms()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(
                [1.0, 3.0, 2.0, 5.0, 4.0, 7.0],
                new DickeyFullerOptions { Regression = TrendTerms.ConstantAndQuadraticTrend }));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("too short", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Kpss_refuses_a_straight_line_its_trend_fits_exactly()
    {
        // The statistic was 0/0 and the window a NaN cast to int; a level null still tests the line (#874).
        double[] line = [.. Enumerable.Range(1, 30).Select(i => (double)i)];

        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => Stationarity.Kpss(line, new KpssOptions { Regression = TrendTerms.ConstantAndTrend }));
        KpssResult level = Stationarity.Kpss(line, new KpssOptions { Regression = TrendTerms.Constant });

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("straight line", refusal.Message, StringComparison.Ordinal);
        Assert.Equal(0.8577681058127098, level.Statistic, 1e-12);
    }

    [Fact]
    public void Kpss_refuses_a_constant_series()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => Stationarity.Kpss([4.0, 4.0, 4.0, 4.0, 4.0]));
        Assert.Equal("series", error.ParamName);
    }

    [Fact]
    public void Kpss_refuses_a_fixed_window_at_the_series_length()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.Kpss(Walk, new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = Walk.Length }));
        Assert.Equal("options", error.ParamName);
        Assert.Contains("20", error.Message, StringComparison.Ordinal);
    }

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible walk; no security use.
#pragma warning disable S2245, CA5394
    [Fact]
    public void Kpss_on_a_walk_rejects_what_adf_cannot()
    {
        double[] walk = new double[200];
        var random = new Random(671);
        for (int i = 1; i < walk.Length; i++)
        {
            walk[i] = walk[i - 1] + (random.NextDouble() - 0.5);
        }

        Assert.True(Stationarity.AugmentedDickeyFuller(walk).PValue > 0.05);
        Assert.True(Stationarity.Kpss(walk).PValue < 0.05);
    }
#pragma warning restore S2245, CA5394
}
