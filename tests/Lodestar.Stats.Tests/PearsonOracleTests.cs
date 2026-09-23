using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_pearson.json</c>.</summary>
public sealed class PearsonOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_pearson.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            Alternative alternative = StatsCorpus.Alternative(args);

            PearsonResult result = Pearson.Test(
                StatsCorpus.Doubles(c.GetProperty("x")),
                StatsCorpus.Doubles(c.GetProperty("y")),
                alternative);

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);

            foreach (JsonElement interval in c.GetProperty("intervals").EnumerateArray())
            {
                double level = interval.GetProperty("level").GetDouble();
                (double low, double high) = result.ConfidenceInterval(level);

                // The bounds are correlations, so they share the statistic's scale and its
                // absolute tolerance rather than the p-value's relative one.
                StatsOracleAsserts.Statistic(
                    StatsCorpus.Number(interval.GetProperty("low")), low, $"{name} low@{level}");
                StatsOracleAsserts.Statistic(
                    StatsCorpus.Number(interval.GetProperty("high")), high, $"{name} high@{level}");
            }

            replayed++;
        }

        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
        Assert.Equal(expected, replayed);
    }

    /// <summary>
    /// The <c>nan_policy</c> arm, whose values the corpus states rather than replays --
    /// <c>scipy.stats.pearsonr</c> takes no such parameter (scipy/scipy#22155), so
    /// <c>omit</c> is frozen as scipy applied to the pairwise-filtered input.
    /// </summary>
    /// <summary>
    /// The <c>nan_policy</c> arm, whose values the corpus states rather than replays --
    /// <c>scipy.stats.pearsonr</c> takes no such parameter (scipy/scipy#22155), so
    /// <c>omit</c> is frozen as scipy applied to the pairwise-filtered input.
    /// </summary>
    [Fact]
    public void Every_nan_policy_case_matches_the_stated_value()
    {
        int replayed = CorrelationReplay.NanPolicyCases(
            "stats_pearson.json",
            (x, y, policy) =>
            {
                PearsonResult result = Pearson.Test(x, y, nanPolicy: policy);
                return (result.Statistic, result.PValue);
            });

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
