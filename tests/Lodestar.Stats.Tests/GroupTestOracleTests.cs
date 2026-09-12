using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays the two k-sample corpora, which share a fixture shape.</summary>
public sealed class GroupTestOracleTests
{
    [Theory]
    [InlineData("stats_anova.json")]
    [InlineData("stats_kruskal.json")]
    public void Every_case_matches_scipy(string fileName)
    {
        using JsonDocument document = StatsCorpus.Load(fileName);
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (StatsCorpus.HasNanPolicy(c.GetProperty("args")))
            {
                continue;
            }

            string name = $"{fileName}: {c.GetProperty("name").GetString()}";
            double[][] groups = StatsCorpus.Table(c.GetProperty("groups"));

            TestResult result = c.GetProperty("call").GetString() == "f_oneway"
                ? OneWayAnova.Test(groups)
                : KruskalWallis.Test(groups);

            StatsOracleAsserts.Statistic(
                c.GetProperty("statistic").GetDouble(), result.Statistic, name);
            StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
            replayed++;
        }

        // Every case without a nan_policy was replayed; those cases are covered
        // by Every_nan_policy_case_matches_scipy instead.
        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
        Assert.Equal(expected, replayed);
    }

    [Theory]
    [InlineData("stats_anova.json")]
    [InlineData("stats_kruskal.json")]
    public void Every_nan_policy_case_matches_scipy(string fileName)
    {
        using JsonDocument document = StatsCorpus.Load(fileName);
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (!StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = $"{fileName}: {c.GetProperty("name").GetString()}";
            NanPolicy policy = StatsCorpus.NanPolicy(args);
            double[][] groups = StatsCorpus.Table(c.GetProperty("groups"));
            bool isAnova = c.GetProperty("call").GetString() == "f_oneway";

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => isAnova
                    ? OneWayAnova.Test(policy, groups)
                    : KruskalWallis.Test(policy, groups));
                replayed++;
                continue;
            }

            TestResult actual = isAnova
                ? OneWayAnova.Test(policy, groups)
                : KruskalWallis.Test(policy, groups);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            double pValue = StatsCorpus.Number(c.GetProperty("pvalue"));
            StatsOracleAsserts.Statistic(statistic, actual.Statistic, name);
            StatsOracleAsserts.PValue(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 3, $"only {replayed} nan_policy cases replayed in {fileName}");
    }
}
