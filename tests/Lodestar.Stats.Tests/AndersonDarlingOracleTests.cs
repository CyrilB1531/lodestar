using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible fixture; no security use.
#pragma warning disable S2245, CA5394

/// <summary>Replays <c>tests/oracles/stats_anderson.json</c>: both shapes scipy is between.</summary>
public sealed class AndersonDarlingOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_anderson.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            AndersonResult result = AndersonDarling.Test(StatsCorpus.Doubles(c.GetProperty("x")));

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);

            // Stated rather than replayed, the reporting shape being the one scipy removes in
            // 1.19 -- but the generator checks the statement against it while it lasts.
            double[] critical = StatsCorpus.Doubles(c.GetProperty("critical_values"));
            double[] levels = StatsCorpus.Doubles(c.GetProperty("significance_level"));
            Assert.Equal(critical, result.CriticalValues);
            Assert.Equal(levels, result.SignificanceLevels);
            replayed++;
        }

        Assert.True(replayed >= 6, $"only {replayed} cases replayed");
    }

    /// <summary>
    /// The interpolation clamps at the ends of Stephens' table, so no sample can be reported
    /// outside <c>[0.01, 0.15]</c> however far from normal it is. The corpus carries a sample at
    /// each end; this states the rule those two cases are instances of.
    /// </summary>
    [Fact]
    public void The_p_value_never_leaves_the_table()
    {
        var random = new System.Random(1121);
        double[] wild = new double[50];
        for (int i = 0; i < wild.Length; i++)
        {
            wild[i] = Math.Pow(10.0, random.NextDouble() * 6.0);
        }

        AndersonResult far = AndersonDarling.Test(wild);
        AndersonResult close = AndersonDarling.Test([1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0]);

        Assert.InRange(far.PValue, 0.01, 0.15);
        Assert.InRange(close.PValue, 0.01, 0.15);
        Assert.Equal(0.01, far.PValue, 12);
    }
}

#pragma warning restore S2245, CA5394
