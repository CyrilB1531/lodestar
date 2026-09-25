using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>
/// Replays <c>stats_fligner.json</c>, <c>stats_anderson_ksamp.json</c>, <c>stats_spearman_matrix.json</c> and
/// <c>stats_pointbiserial.json</c> (#1162).
/// </summary>
public sealed class Issue1162OracleTests
{
    [Fact]
    public void Every_fligner_case_matches_scipy()
    {
        int replayed = GroupReplay.PlainCases(
            "stats_fligner.json",
            (args, groups) => Fligner.Test(
                GroupReplay.Center(args), args.GetProperty("proportiontocut").GetDouble(), NanPolicy.Propagate, groups));

        Assert.True(replayed >= 20, $"only {replayed} cases replayed");
    }

    [Fact]
    public void Every_fligner_nan_policy_case_matches_scipy()
    {
        int replayed = GroupReplay.NanPolicyCases(
            "stats_fligner.json", (groups, policy) => Fligner.Test(Center.Median, 0.05, policy, groups));

        Assert.Equal(3, replayed);
    }

    [Fact]
    public void Every_anderson_ksamp_case_matches_scipy()
    {
        using JsonDocument corpus = StatsCorpus.Load("stats_anderson_ksamp.json");
        int replayed = 0;
        foreach (JsonElement c in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            double[][] groups = StatsCorpus.Table(c.GetProperty("groups"));
            AndersonKSampleVariant variant = c.GetProperty("args").GetProperty("variant").GetString() switch
            {
                "midrank" => AndersonKSampleVariant.Midrank,
                "right" => AndersonKSampleVariant.Right,
                _ => AndersonKSampleVariant.Continuous,
            };
            AndersonResult result = AndersonDarling.KSample(variant, groups);

            StatsOracleAsserts.Statistic(StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            StatsOracleAsserts.Vector(StatsCorpus.Doubles(c.GetProperty("criticalValues")), result.CriticalValues, name);
            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    [Fact]
    public void Every_spearman_matrix_case_matches_scipy()
    {
        using JsonDocument corpus = StatsCorpus.Load("stats_spearman_matrix.json");
        int replayed = 0;
        foreach (JsonElement c in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            CorrelationMatrix result = Spearman.Matrix(
                StatsCorpus.Doubles(c.GetProperty("data")),
                c.GetProperty("variableCount").GetInt32(),
                StatsCorpus.Alternative(args),
                StatsCorpus.NanPolicy(args));

            double[] statistics = StatsCorpus.Doubles(c.GetProperty("statistic"));
            double[] pValues = StatsCorpus.Doubles(c.GetProperty("pvalue"));
            for (int i = 0; i < statistics.Length; i++)
            {
                StatsOracleAsserts.Statistic(statistics[i], result.Statistics[i], $"{name} [{i}]");
                StatsOracleAsserts.PValue(pValues[i], result.PValues[i], $"{name} [{i}]");
            }

            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    [Fact]
    public void Every_pointbiserial_case_matches_scipy()
    {
        using JsonDocument corpus = StatsCorpus.Load("stats_pointbiserial.json");
        int replayed = 0;
        foreach (JsonElement c in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            bool[] x = [.. c.GetProperty("x").EnumerateArray().Select(v => v.GetBoolean())];
            JsonElement args = c.GetProperty("args");
            NanPolicy policy = StatsCorpus.HasNanPolicy(args) ? StatsCorpus.NanPolicy(args) : NanPolicy.Propagate;
            TestResult result = PointBiserial.Test(x, StatsCorpus.Doubles(c.GetProperty("y")), policy);

            StatsOracleAsserts.Statistic(StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }
}
