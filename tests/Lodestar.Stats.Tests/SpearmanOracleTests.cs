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

    [Fact]
    public void A_variable_count_whose_square_passes_one_array_is_refused()
    {
        // 46,341 variables make two 46,341-square matrices, past Array.MaxLength: the product wrapped or failed (#1614).
        ArgumentException error = Assert.Throws<ArgumentException>(() => Spearman.Matrix([], 46_341));
        Assert.Equal("variableCount", error.ParamName);
    }

    [Fact]
    public void The_square_bound_is_checked_before_the_columns_are_split_out()
    {
        // int.MaxValue variables and no rows: the columns' int.MaxValue arrays ran out of memory before the bound (#1614).
        ArgumentException error = Assert.Throws<ArgumentException>(() => Spearman.Matrix([], int.MaxValue));
        Assert.Equal("variableCount", error.ParamName);
    }

    [Fact]
    public void A_raised_NaN_is_still_reported_before_the_square_bound()
    {
        double[] row = new double[46_341];
        row[7] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Spearman.Matrix(row, 46_341, nanPolicy: NanPolicy.Raise));
        Assert.Equal("data", error.ParamName);
    }

    [Fact]
    public void A_raised_NaN_past_the_square_bound_names_the_lowest_variable_that_holds_one()
    {
        double[] row = new double[46_341];
        row[40_000] = double.NaN;
        row[7] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Spearman.Matrix(row, 46_341, nanPolicy: NanPolicy.Raise));
        Assert.Contains("Variable 7 holds a NaN.", error.Message, StringComparison.Ordinal);
    }
}
