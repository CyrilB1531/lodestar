using System.Text.Json;
using Lodestar.Stats.TimeSeries.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Replays <c>tests/oracles/stats_seasonal.json</c>.</summary>
public sealed class SeasonalDecompositionOracleTests
{
    [Fact]
    public void Every_case_matches_statsmodels()
    {
        using JsonDocument document = StatsCorpus.Load("stats_seasonal.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            SeasonalComponents result = SeasonalDecomposition.Decompose(
                StatsCorpus.Doubles(c.GetProperty("series")),
                c.GetProperty("period").GetInt32(),
                new SeasonalDecompositionOptions
                {
                    Model = c.GetProperty("model").GetString() == "multiplicative"
                        ? SeasonalModel.Multiplicative : SeasonalModel.Additive,
                    TwoSided = c.GetProperty("two_sided").GetBoolean(),
                    ExtrapolateTrend = c.GetProperty("extrapolate_trend").GetInt32(),
                });

            Components(StatsCorpus.Doubles(c.GetProperty("trend")), result.Trend, $"{name} trend");
            Components(StatsCorpus.Doubles(c.GetProperty("seasonal")), result.Seasonal, $"{name} seasonal");
            Components(StatsCorpus.Doubles(c.GetProperty("resid")), result.Residual, $"{name} residual");
            replayed++;
        }

        Assert.Equal(
            document.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static void Components(double[] expected, IReadOnlyList<double> actual, string name)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            StatsOracleAsserts.Statistic(expected[i], actual[i], $"{name}[{i}]");
        }
    }
}
