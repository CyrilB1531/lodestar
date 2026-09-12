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
            if (c.GetProperty("args").TryGetProperty("nan_policy", out _))
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
            .Count(c => !c.GetProperty("args").TryGetProperty("nan_policy", out _));
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
            if (!args.TryGetProperty("nan_policy", out JsonElement policyElement))
            {
                continue;
            }

            string name = $"{fileName}: {c.GetProperty("name").GetString()}";
            NanPolicy policy = policyElement.GetString() switch
            {
                "propagate" => NanPolicy.Propagate,
                "raise" => NanPolicy.Raise,
                "omit" => NanPolicy.Omit,
                _ => throw new InvalidOperationException($"unknown nan_policy in {name}"),
            };
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
            AssertClose(statistic, actual.Statistic, name);
            AssertClose(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 3, $"only {replayed} nan_policy cases replayed in {fileName}");
    }

    private static void AssertClose(double expected, double actual, string name)
    {
        if (double.IsNaN(expected))
        {
            Assert.True(double.IsNaN(actual), $"{name}: expected NaN, got {actual}");
            return;
        }
        Assert.True(Math.Abs(expected - actual) <= 1e-9 * Math.Max(1.0, Math.Abs(expected)),
            $"{name}: expected {expected}, got {actual}");
    }
}
