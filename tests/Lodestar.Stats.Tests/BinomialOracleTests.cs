using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_binomial.json</c>, the intervals included.</summary>
public sealed class BinomialOracleTests
{
    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_binomial.json");
        int replayed = 0;
        int intervals = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            JsonElement args = c.GetProperty("args");
            string name = c.GetProperty("name").GetString()!;
            int successes = args.GetProperty("k").GetInt32();
            int trials = args.GetProperty("n").GetInt32();
            double probability = args.GetProperty("p").GetDouble();

            BinomialResult result = Binomial.Test(
                successes, trials, probability, StatsCorpus.Alternative(args));

            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), result.Statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), result.PValue, name);

            foreach (JsonElement interval in c.GetProperty("intervals").EnumerateArray())
            {
                double level = interval.GetProperty("level").GetDouble();
                ProportionInterval method = Method(interval);
                (double low, double high) = result.ProportionConfidenceInterval(level, method);

                // Bounds are proportions, so they share the statistic's scale and its absolute
                // tolerance rather than the p-value's relative one.
                StatsOracleAsserts.Statistic(
                    StatsCorpus.Number(interval.GetProperty("low")), low, $"{name} {method} low@{level}");
                StatsOracleAsserts.Statistic(
                    StatsCorpus.Number(interval.GetProperty("high")), high, $"{name} {method} high@{level}");
                intervals++;
            }

            replayed++;
        }

        Assert.True(replayed >= 200, $"only {replayed} cases replayed");
        Assert.True(intervals >= 1800, $"only {intervals} intervals replayed");
    }

    private static ProportionInterval Method(JsonElement interval) =>
        interval.GetProperty("method").GetString() switch
        {
            "exact" => ProportionInterval.Exact,
            "wilson" => ProportionInterval.Wilson,
            "wilsoncc" => ProportionInterval.WilsonCorrected,
            var other => throw new InvalidDataException($"Unknown interval method '{other}'."),
        };
}
