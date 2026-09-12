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
            if (StatsCorpus.HasNanPolicy(c.GetProperty("args")))
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
            .Count(c => !StatsCorpus.HasNanPolicy(c.GetProperty("args")));
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
            if (!StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            NanPolicy policy = StatsCorpus.NanPolicy(args);
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
            StatsOracleAsserts.Statistic(statistic, actual.Statistic, name);
            StatsOracleAsserts.PValue(pValue, actual.PValue, name);
            replayed++;
        }

        Assert.True(replayed >= 6, $"only {replayed} nan_policy cases replayed");
    }
}
