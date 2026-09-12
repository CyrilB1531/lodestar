using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_mannwhitney.json</c>.</summary>
public sealed class MannWhitneyOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_mannwhitney.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (StatsCorpus.HasNanPolicy(c.GetProperty("args")))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");

            TestResult result = MannWhitney.Test(
                StatsCorpus.Doubles(c.GetProperty("a")),
                StatsCorpus.Doubles(c.GetProperty("b")),
                StatsCorpus.Alternative(args),
                args.GetProperty("use_continuity").GetBoolean()
                    ? Continuity.Applied
                    : Continuity.None,
                StatsCorpus.Method(args));

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

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_mannwhitney.json");
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
            double[] x = StatsCorpus.Doubles(c.GetProperty("a"));
            double[] y = StatsCorpus.Doubles(c.GetProperty("b"));

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => MannWhitney.Test(x, y, nanPolicy: policy));
                replayed++;
                continue;
            }

            TestResult actual = MannWhitney.Test(x, y, nanPolicy: policy);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            if (double.IsNaN(statistic))
            {
                Assert.True(double.IsNaN(actual.Statistic), $"{name}: expected NaN statistic");
            }
            else
            {
                Assert.True(Math.Abs(statistic - actual.Statistic) <= 1e-9 * Math.Max(1.0, Math.Abs(statistic)),
                    $"{name}: statistic expected {statistic}, got {actual.Statistic}");
            }
            replayed++;
        }

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
