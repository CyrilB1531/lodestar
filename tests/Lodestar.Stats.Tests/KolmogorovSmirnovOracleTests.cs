using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_ks.json</c>.</summary>
public sealed class KolmogorovSmirnovOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_ks.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("args").TryGetProperty("nan_policy", out _))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");

            KsResult result = KolmogorovSmirnov.TwoSample(
                StatsCorpus.Doubles(c.GetProperty("a")),
                StatsCorpus.Doubles(c.GetProperty("b")),
                StatsCorpus.Alternative(args),
                StatsCorpus.Method(args));

            StatsOracleAsserts.Statistic(
                c.GetProperty("statistic").GetDouble(), result.Statistic, name);
            StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
            StatsOracleAsserts.Statistic(
                c.GetProperty("statistic_location").GetDouble(),
                result.StatisticLocation, $"{name} location");
            Assert.Equal(c.GetProperty("statistic_sign").GetInt32(), result.StatisticSign);
            replayed++;
        }

        // Every case without a nan_policy was replayed; those cases are covered
        // by Every_nan_policy_case_matches_scipy instead.
        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !c.GetProperty("args").TryGetProperty("nan_policy", out _));
        Assert.Equal(expected, replayed);
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_ks.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (!args.TryGetProperty("nan_policy", out JsonElement policyElement))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            NanPolicy policy = policyElement.GetString() switch
            {
                "propagate" => NanPolicy.Propagate,
                "raise" => NanPolicy.Raise,
                "omit" => NanPolicy.Omit,
                _ => throw new InvalidOperationException($"unknown nan_policy in {name}"),
            };
            double[] a = StatsCorpus.Doubles(c.GetProperty("a"));
            double[] b = StatsCorpus.Doubles(c.GetProperty("b"));

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(
                    () => KolmogorovSmirnov.TwoSample(a, b, nanPolicy: policy));
                replayed++;
                continue;
            }

            KsResult actual = KolmogorovSmirnov.TwoSample(a, b, nanPolicy: policy);
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
