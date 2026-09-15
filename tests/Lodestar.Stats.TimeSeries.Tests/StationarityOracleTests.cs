using System.Text.Json;
using Lodestar.Stats.TimeSeries.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Replays <c>tests/oracles/stats_stationarity.json</c>.</summary>
public sealed class StationarityOracleTests
{
    [Fact]
    public void Every_adfuller_case_matches_statsmodels() => Replay("adfuller", ReplayDickeyFuller);

    [Fact]
    public void Every_kpss_case_matches_statsmodels() => Replay("kpss", ReplayKpss);

    private static void Replay(string call, Action<JsonElement, string, double[]> replay)
    {
        using JsonDocument document = StatsCorpus.Load("stats_stationarity.json");
        int replayed = 0;
        int expected = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("call").GetString() != call)
            {
                continue;
            }

            expected++;
            replay(c, c.GetProperty("name").GetString()!, StatsCorpus.Doubles(c.GetProperty("series")));
            replayed++;
        }

        Assert.True(expected > 0, $"the corpus holds no {call} case.");
        Assert.Equal(expected, replayed);
    }

    private static void ReplayDickeyFuller(JsonElement c, string name, double[] series)
    {
        JsonElement autolag = c.GetProperty("autolag");
        JsonElement maxlag = c.GetProperty("maxlag");
        DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(series, new DickeyFullerOptions
        {
            Regression = Terms(c.GetProperty("regression").GetString()!),
            LagSelection = autolag.ValueKind == JsonValueKind.Null ? LagSelection.Fixed : autolag.GetString() switch
            {
                "AIC" => LagSelection.Akaike,
                "BIC" => LagSelection.Schwarz,
                "t-stat" => LagSelection.TStatistic,
                var other => throw new InvalidDataException($"Unknown autolag '{other}'."),
            },
            MaxLag = maxlag.ValueKind == JsonValueKind.Null ? null : maxlag.GetInt32(),
        });

        StatsOracleAsserts.Statistic(c.GetProperty("statistic").GetDouble(), result.Statistic, name);
        StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
        Assert.Equal(c.GetProperty("usedlag").GetInt32(), result.UsedLag);
        Assert.Equal(c.GetProperty("nobs").GetInt32(), result.ObservationCount);

        double[] critical = StatsCorpus.Doubles(c.GetProperty("critical"));
        for (int level = 0; level < critical.Length; level++)
        {
            StatsOracleAsserts.Statistic(critical[level], result.CriticalValues[level], $"{name} critical[{level}]");
        }

        JsonElement icbest = c.GetProperty("icbest");
        if (icbest.ValueKind == JsonValueKind.Null)
        {
            Assert.True(double.IsNaN(result.InformationCriterion), $"{name}: a fixed lag reports no criterion.");
        }
        else
        {
            StatsOracleAsserts.PValue(icbest.GetDouble(), result.InformationCriterion, $"{name} icbest");
        }
    }

    private static void ReplayKpss(JsonElement c, string name, double[] series)
    {
        JsonElement nlags = c.GetProperty("nlags");
        bool fixedWindow = nlags.ValueKind == JsonValueKind.Number;
        KpssLagRule rule = nlags.ToString() == "legacy" ? KpssLagRule.Legacy : KpssLagRule.Automatic;
        KpssResult result = Stationarity.Kpss(series, new KpssOptions
        {
            Regression = Terms(c.GetProperty("regression").GetString()!),
            LagRule = fixedWindow ? KpssLagRule.Fixed : rule,
            LagCount = fixedWindow ? nlags.GetInt32() : 0,
        });

        StatsOracleAsserts.Statistic(c.GetProperty("statistic").GetDouble(), result.Statistic, name);
        StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
        Assert.Equal(c.GetProperty("lags").GetInt32(), result.LagCount);

        double[] critical = StatsCorpus.Doubles(c.GetProperty("critical"));
        Assert.Equal(critical, result.CriticalValues);

        PValueBound bound = c.GetProperty("bound").GetString() switch
        {
            "smaller" => PValueBound.ActualIsSmaller,
            "greater" => PValueBound.ActualIsGreater,
            _ => PValueBound.None,
        };
        Assert.Equal(bound, result.PValueBound);
    }

    private static TrendTerms Terms(string regression) => regression switch
    {
        "n" => TrendTerms.None,
        "c" => TrendTerms.Constant,
        "ct" => TrendTerms.ConstantAndTrend,
        "ctt" => TrendTerms.ConstantAndQuadraticTrend,
        _ => throw new InvalidDataException($"Unknown regression '{regression}'."),
    };
}
