using System.Linq;
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_kendall.json</c>.</summary>
public sealed class KendallTauOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_kendall.json");
        int replayed = 0;
        int refused = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            double[] x = StatsCorpus.Doubles(c.GetProperty("x"));
            double[] y = StatsCorpus.Doubles(c.GetProperty("y"));
            Alternative alternative = StatsCorpus.Alternative(args);
            KendallVariant variant = StatsCorpus.Variant(args);
            ExactMethod method = StatsCorpus.Method(args);

            if (c.TryGetProperty("raises", out JsonElement raises) && raises.GetBoolean())
            {
                // scipy refuses the exact method on a tied sample, and so does this.
                Assert.Throws<ArgumentException>(
                    () => KendallTau.Test(x, y, alternative, variant, method));
                refused++;
                replayed++;
                continue;
            }

            TestResult result = KendallTau.Test(x, y, alternative, variant, method);
            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            replayed++;
        }

        int expected = document.RootElement.GetProperty("cases").EnumerateArray()
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
        Assert.Equal(expected, replayed);
        Assert.True(refused > 0, "no tied-exact refusal was replayed");
    }

    /// <summary>
    /// The claim the tied fixtures exist for: the two variants normalise the same count, so
    /// they answer the same p-value and differ only in the statistic.
    /// </summary>
    [Fact]
    public void The_two_variants_share_a_p_value_and_differ_in_the_statistic()
    {
        double[] x = [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0];
        double[] y = [1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0, 5.0];

        TestResult tauB = KendallTau.Test(x, y, variant: KendallVariant.TauB);
        TestResult tauC = KendallTau.Test(x, y, variant: KendallVariant.TauC);

        StatsOracleAsserts.PValue(tauB.PValue, tauC.PValue, "shared p-value");
        Assert.True(
            Math.Abs(tauB.Statistic - tauC.Statistic) > 1e-6,
            $"the variants agreed on {tauB.Statistic}, so this fixture no longer has ties");
    }

    [Fact]
    public void Every_nan_policy_case_matches_scipy()
    {
        int replayed = CorrelationReplay.NanPolicyCases(
            "stats_kendall.json",
            (x, y, policy) =>
            {
                TestResult result = KendallTau.Test(x, y, nanPolicy: policy);
                return (result.Statistic, result.PValue);
            });

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
