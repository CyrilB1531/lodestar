using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Tests.Oracles;

/// <summary>The arm the three correlation corpora replay identically.</summary>
/// <remarks>
/// The plain arms differ — Pearson checks an interval, Kendall a refusal per method — but the
/// <c>nan_policy</c> arm is the same thirty lines three times over, differing only in which
/// function is called. Written once here so a change to what a policy case proves reaches all
/// three, and so the duplication gate has nothing to report on new code.
/// </remarks>
internal static class CorrelationReplay
{
    /// <summary>One correlation call, reduced to the two numbers a corpus case carries.</summary>
    internal delegate (double Statistic, double PValue) Correlate(
        double[] x, double[] y, NanPolicy policy);

    /// <summary>Replays every case of <paramref name="fileName"/> that carries a policy.</summary>
    /// <returns>How many cases were replayed, which the caller asserts against.</returns>
    internal static int NanPolicyCases(string fileName, Correlate correlate)
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
            double[] x = StatsCorpus.Doubles(c.GetProperty("x"));
            double[] y = StatsCorpus.Doubles(c.GetProperty("y"));

            if (c.TryGetProperty("raises", out JsonElement raises) && raises.GetBoolean())
            {
                Assert.Throws<ArgumentException>(() => correlate(x, y, policy));
                replayed++;
                continue;
            }

            (double statistic, double pValue) = correlate(x, y, policy);
            StatsOracleAsserts.Statistic(
                StatsCorpus.Number(c.GetProperty("statistic")), statistic, name);
            StatsOracleAsserts.PValue(
                StatsCorpus.Number(c.GetProperty("pvalue")), pValue, name);
            replayed++;
        }

        return replayed;
    }
}
