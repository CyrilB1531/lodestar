using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_shapiro.json</c>.</summary>
public sealed class ShapiroWilkOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_shapiro.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (StatsCorpus.HasNanPolicy(c.GetProperty("args")))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;

            TestResult result = ShapiroWilk.Test(StatsCorpus.Doubles(c.GetProperty("x")));

            // Statistic uses the absolute tolerance (Royston's approximation both sides) --
            // p-value is relative, a normal tail of a fitted transform.
            StatsOracleAsserts.Statistic(
                c.GetProperty("statistic").GetDouble(), result.Statistic, name);
            StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
            replayed++;
        }

        // Every case without a nan_policy was replayed; those are covered by
        // Every_nan_policy_case_matches_scipy instead.
        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
        Assert.Equal(expected, replayed);
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_shapiro.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (!StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            NanPolicy policy = StatsCorpus.NanPolicy(args);
            double[] x = StatsCorpus.Doubles(c.GetProperty("x"));

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => ShapiroWilk.Test(x, policy));
                replayed++;
                continue;
            }

            TestResult actual = ShapiroWilk.Test(x, policy);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            double pValue = StatsCorpus.Number(c.GetProperty("pvalue"));
            StatsOracleAsserts.Statistic(statistic, actual.Statistic, name);
            StatsOracleAsserts.PValue(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 3, $"only {replayed} nan_policy cases replayed");
    }
}
