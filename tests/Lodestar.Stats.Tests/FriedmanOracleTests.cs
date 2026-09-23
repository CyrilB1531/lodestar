using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_friedman.json</c>.</summary>
public sealed class FriedmanOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        int replayed = GroupReplay.PlainCases(
            "stats_friedman.json", (_, groups) => Friedman.Test(groups));

        Assert.True(replayed >= 5, $"only {replayed} cases replayed");
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        int replayed = GroupReplay.NanPolicyCases(
            "stats_friedman.json", (groups, policy) => Friedman.Test(policy, groups));

        Assert.Equal(3, replayed);
    }
}
