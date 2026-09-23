using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>stats_levene.json</c> and <c>stats_bartlett.json</c>.</summary>
public sealed class VarianceTestOracleTests
{
    [Fact]
    public void Every_levene_case_matches_scipy()
    {
        int replayed = GroupReplay.PlainCases(
            "stats_levene.json",
            (args, groups) => Levene.Test(
                GroupReplay.Center(args),
                args.GetProperty("proportiontocut").GetDouble(),
                NanPolicy.Propagate,
                groups));

        Assert.True(replayed >= 20, $"only {replayed} cases replayed");
    }

    [Fact]
    public void Every_levene_nan_policy_case_matches_scipy()
    {
        int replayed = GroupReplay.NanPolicyCases(
            "stats_levene.json",
            (groups, policy) => Levene.Test(Center.Median, 0.05, policy, groups));

        Assert.Equal(3, replayed);
    }

    [Fact]
    public void Every_bartlett_case_matches_scipy()
    {
        int replayed = GroupReplay.PlainCases("stats_bartlett.json", (_, groups) => Bartlett.Test(groups));

        Assert.True(replayed >= 5, $"only {replayed} cases replayed");
    }

    [Fact]
    public void Every_bartlett_nan_policy_case_matches_scipy()
    {
        int replayed = GroupReplay.NanPolicyCases(
            "stats_bartlett.json", (groups, policy) => Bartlett.Test(policy, groups));

        Assert.Equal(3, replayed);
    }
}
