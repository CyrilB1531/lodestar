using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>The weighted, multi-group and pairwise log-rank tests, the restricted mean and the concordance index against lifelines 0.30.3.</summary>
/// <remarks>Statistics absolutely at 1e-9, p-values relatively, as the two-sample suite compares them.</remarks>
public sealed class LogRankFamilyOracleTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Family = OracleLoader.Load("survival_logrank_family.json");
    private static readonly JsonDocument Restricted = OracleLoader.Load("survival_restricted.json");
    private static readonly JsonDocument ConcordanceCorpus = OracleLoader.Load("survival_concordance.json");

    public static TheoryData<string, int> FamilyCases() => Indices(Family, "two_sample", "multigroup", "pairwise");

    public static TheoryData<string, int> RestrictedCases() => Indices(Restricted, "rmst", "fixed_point");

    public static TheoryData<string, int> ConcordanceCases() => Indices(ConcordanceCorpus, "cases");

    [Theory]
    [MemberData(nameof(FamilyCases))]
    public void The_log_rank_family_matches_lifelines(string section, int index)
    {
        JsonElement c = Family.RootElement.GetProperty(section)[index];
        string name = c.GetProperty("name").GetString()!;
        LogRankOptions options = Options(c);
        if (section == "pairwise")
        {
            IReadOnlyList<PairwiseLogRankResult> pairs = LogRank.Pairwise(
                Doubles(c, "durations"), Ints(c, "groups"), Flags(c, "eventObserved"), options);
            JsonElement expected = c.GetProperty("pairs");
            Assert.Equal(expected.GetArrayLength(), pairs.Count);
            for (int i = 0; i < pairs.Count; i++)
            {
                Assert.Equal(expected[i].GetProperty("groupA").GetInt32(), pairs[i].GroupA);
                Assert.Equal(expected[i].GetProperty("groupB").GetInt32(), pairs[i].GroupB);
                Matches(expected[i], pairs[i].Result, $"{name}, pair {i}");
            }

            return;
        }

        LogRankResult result = section == "two_sample"
            ? LogRank.Test(
                Doubles(c, "durationsA"), Flags(c, "eventsA"), Optional(c, "weightsA"),
                Doubles(c, "durationsB"), Flags(c, "eventsB"), Optional(c, "weightsB"), options)
            : LogRank.MultiGroup(
                Doubles(c, "durations"), Ints(c, "groups"), Flags(c, "eventObserved"), Optional(c, "weights"), options);
        Matches(c, result, name);
        Assert.Equal(c.GetProperty("degreesOfFreedom").GetInt32(), result.DegreesOfFreedom);
    }

    [Theory]
    [MemberData(nameof(RestrictedCases))]
    public void The_restricted_mean_and_the_fixed_point_test_match_lifelines(string section, int index)
    {
        JsonElement c = Restricted.RootElement.GetProperty(section)[index];
        string name = c.GetProperty("name").GetString()!;
        if (section == "rmst")
        {
            KaplanMeierCurve curve = KaplanMeier.Estimate(Doubles(c, "durations"), Flags(c, "eventObserved"));
            JsonElement horizon = c.GetProperty("horizon");
            RestrictedMeanResult result = horizon.ValueKind == JsonValueKind.Null
                ? KaplanMeier.RestrictedMean(curve)
                : KaplanMeier.RestrictedMean(curve, horizon.GetDouble());
            Assert.True(Math.Abs(c.GetProperty("mean").GetDouble() - result.Mean) <= Tolerance, $"{name}: mean {result.Mean}");
            Assert.True(
                Math.Abs(c.GetProperty("variance").GetDouble() - result.Variance) <= Tolerance,
                $"{name}: variance {result.Variance}");

            // lifelines' own second moment is a quadrature of the step function: 1.5 % from the exact one at worst over
            // 300 random curves, so this only checks both describe the same quantity.
            double lifelines = c.GetProperty("lifelinesVariance").GetDouble();
            Assert.True(Math.Abs(lifelines - result.Variance) <= 2e-2 * Math.Max(1.0, Math.Abs(result.Variance)), name);
            return;
        }

        KaplanMeierCurve a = KaplanMeier.Estimate(Doubles(c, "durationsA"), Flags(c, "eventsA"));
        KaplanMeierCurve b = KaplanMeier.Estimate(Doubles(c, "durationsB"), Flags(c, "eventsB"));
        Stats.TestResult test = KaplanMeier.CompareAt(c.GetProperty("time").GetDouble(), a, b);
        Assert.True(Math.Abs(c.GetProperty("statistic").GetDouble() - test.Statistic) <= Tolerance, $"{name}: {test.Statistic}");
        double expectedP = c.GetProperty("pValue").GetDouble();
        Assert.True(Math.Abs(expectedP - test.PValue) <= Math.Max(Tolerance * expectedP, 1e-12), $"{name}: p {test.PValue}");
    }

    [Theory]
    [MemberData(nameof(ConcordanceCases))]
    public void The_concordance_index_matches_lifelines(string section, int index)
    {
        JsonElement c = ConcordanceCorpus.RootElement.GetProperty(section)[index];
        JsonElement observed = c.GetProperty("eventObserved");
        double result = observed.ValueKind == JsonValueKind.Null
            ? Concordance.Index(Doubles(c, "durations"), Doubles(c, "scores"))
            : Concordance.Index(Doubles(c, "durations"), Doubles(c, "scores"), Flags(c, "eventObserved"));
        Assert.Equal(c.GetProperty("index").GetDouble(), result, Tolerance);
    }

    private static void Matches(JsonElement expected, LogRankResult result, string name)
    {
        double statistic = expected.GetProperty("statistic").GetDouble();
        double p = expected.GetProperty("pValue").GetDouble();
        Assert.True(Math.Abs(statistic - result.Statistic) <= Tolerance, $"{name}: statistic {result.Statistic} against {statistic}");
        Assert.True(Math.Abs(p - result.PValue) <= Math.Max(Tolerance * p, 1e-12), $"{name}: p {result.PValue} against {p}");
    }

    private static LogRankOptions Options(JsonElement c)
    {
        JsonElement truncation = c.GetProperty("truncation");
        return new LogRankOptions
        {
            Weighting = c.GetProperty("weighting").GetString() switch
            {
                "wilcoxon" => LogRankWeighting.Wilcoxon,
                "tarone-ware" => LogRankWeighting.TaroneWare,
                "peto" => LogRankWeighting.Peto,
                "fleming-harrington" => LogRankWeighting.FlemingHarrington,
                _ => LogRankWeighting.LogRank,
            },
            P = c.GetProperty("p").GetDouble(),
            Q = c.GetProperty("q").GetDouble(),
            Truncation = truncation.ValueKind == JsonValueKind.Null ? null : truncation.GetDouble(),
        };
    }

    private static TheoryData<string, int> Indices(JsonDocument corpus, params string[] sections)
    {
        var data = new TheoryData<string, int>();
        foreach (string section in sections)
        {
            for (int i = 0; i < corpus.RootElement.GetProperty(section).GetArrayLength(); i++)
            {
                data.Add(section, i);
            }
        }

        return data;
    }

    private static double[] Optional(JsonElement c, string name) =>
        c.GetProperty(name).ValueKind == JsonValueKind.Null ? [] : Doubles(c, name);

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetDouble())];

    private static int[] Ints(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32())];

    private static bool[] Flags(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32() == 1)];
}
