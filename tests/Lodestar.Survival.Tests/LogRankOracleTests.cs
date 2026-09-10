using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>The two-sample log-rank test against <c>lifelines.statistics</c> 0.30.3.</summary>
/// <remarks>
/// The statistic absolutely at 1e-10; the p-value <strong>relatively</strong>, as decision
/// 0081 established for every p-value here — the Freireich pair reaches 3.4e-3, but a
/// wider separation would reach far enough that an absolute tolerance accepts a zero.
/// </remarks>
public sealed class LogRankOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_logrank.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Fact]
    public void Metadata_is_lifelines()
    {
        Assert.Equal("lifelines", Corpus.RootElement.GetProperty("metadata")
            .GetProperty("library").GetString());
        Assert.NotEmpty(Cases);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void Log_rank_matches_lifelines(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;

        LogRankResult result = LogRank.Test(
            Doubles(frozen, "durationsA"), Flags(frozen, "eventsA"),
            Doubles(frozen, "durationsB"), Flags(frozen, "eventsB"));

        double expectedStatistic = frozen.GetProperty("statistic").GetDouble();
        double expectedP = frozen.GetProperty("pValue").GetDouble();

        Assert.Equal(expectedStatistic, result.Statistic, 1e-10);
        Assert.Equal(expectedP, result.PValue, Math.Max(1e-9 * expectedP, 1e-12));
        Assert.Equal(frozen.GetProperty("degreesOfFreedom").GetInt32(), result.DegreesOfFreedom);
        Assert.True(result.Statistic >= 0.0, $"[{name}] the statistic is a square.");
    }

    private static double[] Doubles(JsonElement frozen, string name) =>
        [.. frozen.GetProperty(name).EnumerateArray().Select(e => e.GetDouble())];

    private static bool[] Flags(JsonElement frozen, string name) =>
        [.. frozen.GetProperty(name).EnumerateArray().Select(e => e.GetInt32() == 1)];
}
