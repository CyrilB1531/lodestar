using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_chisquare.json</c>, both calls.</summary>
public sealed class ChiSquareOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_chisquare.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("args").TryGetProperty("nan_policy", out _))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            double expectedStatistic = c.GetProperty("statistic").GetDouble();
            double expectedP = c.GetProperty("pvalue").GetDouble();

            if (c.GetProperty("call").GetString() == "chisquare")
            {
                AssertGoodnessOfFit(c, name, expectedStatistic, expectedP);
            }
            else
            {
                AssertContingency(c, name, expectedStatistic, expectedP);
            }

            replayed++;
        }

        // Every case without a nan_policy was replayed; those are covered by
        // Every_nan_policy_case_matches_scipy instead.
        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !c.GetProperty("args").TryGetProperty("nan_policy", out _));
        Assert.Equal(expected, replayed);
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_chisquare.json");
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
            double[] observed = StatsCorpus.Doubles(c.GetProperty("observed"));
            double[] expectedInput = StatsCorpus.Doubles(c.GetProperty("expected_input"));

            if (c.TryGetProperty("raises", out JsonElement r) && r.GetBoolean())
            {
                Assert.Throws<ArgumentException>(
                    () => ChiSquare.GoodnessOfFit(observed, expectedInput, policy));
                replayed++;
                continue;
            }

            TestResult actual = ChiSquare.GoodnessOfFit(observed, expectedInput, policy);
            double statistic = StatsCorpus.Number(c.GetProperty("statistic"));
            double pValue = StatsCorpus.Number(c.GetProperty("pvalue"));
            StatsOracleAsserts.Statistic(statistic, actual.Statistic, name);
            StatsOracleAsserts.PValue(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 3, $"only {replayed} nan_policy cases replayed");
    }

    private static void AssertGoodnessOfFit(
        JsonElement c, string name, double expectedStatistic, double expectedP)
    {
        double[] observed = StatsCorpus.Doubles(c.GetProperty("observed"));
        double[] expected = StatsCorpus.Doubles(c.GetProperty("expected_input"));

        TestResult result = expected.Length == 0
            ? ChiSquare.GoodnessOfFit(observed)
            : ChiSquare.GoodnessOfFit(observed, expected);

        StatsOracleAsserts.Statistic(expectedStatistic, result.Statistic, name);
        StatsOracleAsserts.PValue(expectedP, result.PValue, name);
    }

    private static void AssertContingency(
        JsonElement c, string name, double expectedStatistic, double expectedP)
    {
        Chi2ContingencyResult result = ChiSquare.Contingency(
            StatsCorpus.Table(c.GetProperty("table")),
            c.GetProperty("args").GetProperty("correction").GetBoolean()
                ? Continuity.Applied
                : Continuity.None);

        StatsOracleAsserts.Statistic(expectedStatistic, result.Statistic, name);
        StatsOracleAsserts.PValue(expectedP, result.PValue, name);
        Assert.Equal(c.GetProperty("dof").GetInt32(), result.Dof);

        double[][] expectedFreq = StatsCorpus.Table(c.GetProperty("expected_freq"));
        for (int i = 0; i < expectedFreq.Length; i++)
        {
            for (int j = 0; j < expectedFreq[i].Length; j++)
            {
                StatsOracleAsserts.Statistic(
                    expectedFreq[i][j], result.ExpectedFrequencies[i][j],
                    $"{name} expected[{i}][{j}]");
            }
        }
    }
}
