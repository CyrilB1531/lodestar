using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_spearman.json</c>.</summary>
public sealed class SpearmanOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_spearman.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;

            TestResult result = Spearman.Test(
                StatsCorpus.Doubles(c.GetProperty("x")),
                StatsCorpus.Doubles(c.GetProperty("y")),
                StatsCorpus.Alternative(args));

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            replayed++;
        }

        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
        Assert.Equal(expected, replayed);
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        int replayed = CorrelationReplay.NanPolicyCases(
            "stats_spearman.json",
            (x, y, policy) =>
            {
                TestResult result = Spearman.Test(x, y, nanPolicy: policy);
                return (result.Statistic, result.PValue);
            });

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
