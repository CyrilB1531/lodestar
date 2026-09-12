using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_wilcoxon.json</c>.</summary>
public sealed class WilcoxonOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_wilcoxon.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("args").TryGetProperty("nan_policy", out _))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");

            ZeroMethod zeroMethod = args.GetProperty("zero_method").GetString() switch
            {
                "wilcox" => ZeroMethod.Wilcox,
                "pratt" => ZeroMethod.Pratt,
                "zsplit" => ZeroMethod.ZSplit,
                var other => throw new InvalidDataException($"Unknown zero_method '{other}'."),
            };

            TestResult result = Wilcoxon.Paired(
                StatsCorpus.Doubles(c.GetProperty("x")),
                StatsCorpus.Doubles(c.GetProperty("y")),
                zeroMethod,
                StatsCorpus.Alternative(args),
                args.GetProperty("correction").GetBoolean() ? Continuity.Applied : Continuity.None,
                StatsCorpus.Method(args));

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

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_wilcoxon.json");
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
            double[] x = StatsCorpus.Doubles(c.GetProperty("x"));
            double[] y = StatsCorpus.Doubles(c.GetProperty("y"));

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => Wilcoxon.Paired(x, y, nanPolicy: policy));
                replayed++;
                continue;
            }

            TestResult actual = Wilcoxon.Paired(x, y, nanPolicy: policy);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            double pValue = StatsCorpus.Number(c.GetProperty("pvalue"));
            if (double.IsNaN(statistic))
            {
                Assert.True(double.IsNaN(actual.Statistic), $"{name}: expected NaN statistic");
            }
            else
            {
                Assert.True(Math.Abs(statistic - actual.Statistic) <= 1e-9 * Math.Max(1.0, Math.Abs(statistic)),
                    $"{name}: statistic expected {statistic}, got {actual.Statistic}");
            }
            if (double.IsNaN(pValue))
            {
                Assert.True(double.IsNaN(actual.PValue), $"{name}: expected NaN p-value");
            }
            else
            {
                Assert.True(Math.Abs(pValue - actual.PValue) <= 1e-9 * Math.Max(1.0, Math.Abs(pValue)),
                    $"{name}: p-value expected {pValue}, got {actual.PValue}");
            }
            replayed++;
        }

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
