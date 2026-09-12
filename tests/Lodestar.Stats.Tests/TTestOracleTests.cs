using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>
/// Replays <c>tests/oracles/stats_ttest.json</c>. Each case names the scipy call
/// and the arguments it was generated with, and the replay reads them rather
/// than assuming a default.
/// </summary>
public sealed class TTestOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_ttest.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("args").TryGetProperty("nan_policy", out _))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            string call = c.GetProperty("call").GetString()!;
            JsonElement args = c.GetProperty("args");
            double[] a = StatsCorpus.Doubles(c.GetProperty("a"));
            double[] b = StatsCorpus.Doubles(c.GetProperty("b"));
            Alternative alternative = StatsCorpus.Alternative(args);

            TTestResult result = call switch
            {
                "ttest_ind" => TTest.Independent(
                    a, b, alternative,
                    args.GetProperty("equal_var").GetBoolean() ? Variance.Equal : Variance.Welch),
                "ttest_rel" => TTest.Paired(a, b, alternative),
                "ttest_1samp" => TTest.OneSample(
                    a, args.GetProperty("popmean").GetDouble(), alternative),
                _ => throw new InvalidDataException($"Unknown call '{call}'."),
            };

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
            StatsOracleAsserts.Statistic(c.GetProperty("df").GetDouble(), result.Df, $"{name} df");

            (double low, double high) = result.ConfidenceInterval(0.95);
            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("ci_low")), low, $"{name} ci low");
            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("ci_high")), high, $"{name} ci high");

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
        using JsonDocument document = StatsCorpus.Load("stats_ttest.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (!args.TryGetProperty("nan_policy", out JsonElement policyElement))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            string call = c.GetProperty("call").GetString()!;
            NanPolicy policy = policyElement.GetString() switch
            {
                "propagate" => NanPolicy.Propagate,
                "raise" => NanPolicy.Raise,
                "omit" => NanPolicy.Omit,
                _ => throw new InvalidOperationException($"unknown nan_policy in {name}"),
            };
            double[] a = StatsCorpus.Doubles(c.GetProperty("a"));
            double[] b = StatsCorpus.Doubles(c.GetProperty("b"));
            bool raises = c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean();

            if (raises)
            {
                Assert.Throws<ArgumentException>(() => Run(call, a, b, policy));
                replayed++;
                continue;
            }

            TTestResult actual = Run(call, a, b, policy);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            double pValue = StatsCorpus.Number(c.GetProperty("pvalue"));
            AssertClose(statistic, actual.Statistic, name);
            AssertClose(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 18, $"only {replayed} nan_policy cases replayed");
    }

    private static TTestResult Run(string call, double[] a, double[] b, NanPolicy policy) => call switch
    {
        "ttest_ind" => TTest.Independent(a, b, nanPolicy: policy),
        "ttest_rel" => TTest.Paired(a, b, nanPolicy: policy),
        "ttest_1samp" => TTest.OneSample(a, 0.0, nanPolicy: policy),
        _ => throw new InvalidOperationException($"unknown call {call}"),
    };

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
