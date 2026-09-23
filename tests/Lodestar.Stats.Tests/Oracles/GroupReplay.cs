using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Tests.Oracles;

/// <summary>The arm the three group corpora replay identically.</summary>
/// <remarks>
/// Levene, Bartlett and Friedman read the same <c>groups</c> shape and the same
/// <c>nan_policy</c> cases, so the replay is written once —
/// <see cref="CorrelationReplay"/> does the same for the paired families.
/// </remarks>
internal static class GroupReplay
{
    /// <summary>One group test, reduced to the two numbers a corpus case carries.</summary>
    internal delegate TestResult Compare(double[][] groups, NanPolicy policy);

    /// <summary>Replays every case of <paramref name="fileName"/> that carries a policy.</summary>
    internal static int NanPolicyCases(string fileName, Compare compare)
    {
        using JsonDocument document = StatsCorpus.Load(fileName);
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
            double[][] groups = StatsCorpus.Table(c.GetProperty("groups"));

            if (c.TryGetProperty("raises", out JsonElement raises) && raises.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => compare(groups, policy));
                replayed++;
                continue;
            }

            TestResult result = compare(groups, policy);
            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            replayed++;
        }

        return replayed;
    }

    /// <summary>Replays every case without a policy, the statistic and the p-value only.</summary>
    internal static int PlainCases(string fileName, Func<JsonElement, double[][], TestResult> run)
    {
        using JsonDocument document = StatsCorpus.Load(fileName);
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            if (StatsCorpus.HasNanPolicy(args))
            {
                continue;
            }

            string name = c.GetProperty("name").GetString()!;
            TestResult result = run(args, StatsCorpus.Table(c.GetProperty("groups")));

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);
            replayed++;
        }

        return replayed;
    }

    /// <summary>The Levene <c>center</c> a case was generated with.</summary>
    internal static Center Center(JsonElement args) =>
        args.GetProperty("center").GetString() switch
        {
            "median" => Lodestar.Stats.Center.Median,
            "mean" => Lodestar.Stats.Center.Mean,
            "trimmed" => Lodestar.Stats.Center.Trimmed,
            var other => throw new InvalidDataException($"Unknown center '{other}'."),
        };
}
