using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>
/// Kaplan-Meier and Nelson-Aalen against <c>lifelines</c> 0.30.3, over the same samples.
/// </summary>
/// <remarks>
/// Survival probabilities and hazards are compared absolutely at 1e-12; the confidence
/// bounds relatively, because the log-log transform pushes them into the far tail where
/// an absolute tolerance stops meaning anything. A bound lifelines reports as NaN — the
/// degenerate interval where the curve has reached zero — has to be NaN here too, which
/// is asserted rather than skipped.
/// </remarks>
public sealed class SurvivalCurveOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_curves.json");

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
    public void Kaplan_meier_matches_lifelines(int index)
    {
        JsonElement frozen = Cases[index];
        (double[] durations, bool[] events) = Input(frozen);

        KaplanMeierCurve curve = KaplanMeier.Estimate(durations, events);

        double[] timeline = Doubles(frozen, "timeline");
        Assert.Equal(timeline.Length, curve.Steps.Length);
        for (int i = 0; i < timeline.Length; i++)
        {
            Assert.Equal(timeline[i], curve.Steps[i].Time, 1e-12);
        }

        AssertAll(Doubles(frozen, "survival"), curve.Survival, 1e-12);
        AssertRisk(frozen, curve.Steps);
        AssertRelative(Doubles(frozen, "lower"), curve.Lower, frozen);
        AssertRelative(Doubles(frozen, "upper"), curve.Upper, frozen);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void Nelson_aalen_matches_lifelines(int index)
    {
        JsonElement frozen = Cases[index];
        (double[] durations, bool[] events) = Input(frozen);

        NelsonAalenCurve curve = NelsonAalen.Estimate(durations, events);

        AssertAll(Doubles(frozen, "cumulativeHazard"), curve.CumulativeHazard, 1e-12);
    }

    /// <summary>The two estimators share a timeline, because they share a risk table.</summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void Both_estimators_report_the_same_steps(int index)
    {
        JsonElement frozen = Cases[index];
        (double[] durations, bool[] events) = Input(frozen);

        Assert.Equal(
            KaplanMeier.Estimate(durations, events).Steps,
            NelsonAalen.Estimate(durations, events).Steps);
    }

    /// <summary>The sample, each row repeated as many times as its <c>copies</c> entry says.</summary>
    /// <remarks>
    /// A case without <c>copies</c> holds one row per subject. The large-cohort case stores six
    /// rows for 70,000 subjects, which is what keeps the corpus small.
    /// </remarks>
    private static (double[] Durations, bool[] Events) Input(JsonElement frozen)
    {
        double[] durations = Doubles(frozen, "durations");
        bool[] events = [.. frozen.GetProperty("eventObserved").EnumerateArray().Select(e => e.GetInt32() == 1)];
        if (!frozen.TryGetProperty("copies", out JsonElement copies))
        {
            return (durations, events);
        }

        int[] counts = [.. copies.EnumerateArray().Select(e => e.GetInt32())];
        return (
            [.. durations.SelectMany((d, i) => Enumerable.Repeat(d, counts[i]))],
            [.. events.SelectMany((e, i) => Enumerable.Repeat(e, counts[i]))]);
    }

    private static double[] Doubles(JsonElement frozen, string name) =>
        [.. frozen.GetProperty(name).EnumerateArray().Select(e => e.GetDouble())];

    private static void AssertRisk(JsonElement frozen, SurvivalStep[] steps)
    {
        int[] atRisk = Ints(frozen, "atRisk");
        int[] observed = Ints(frozen, "observed");
        int[] censored = Ints(frozen, "censored");
        for (int i = 0; i < steps.Length; i++)
        {
            Assert.Equal(atRisk[i], steps[i].AtRisk);
            Assert.Equal(observed[i], steps[i].Events);
            Assert.Equal(censored[i], steps[i].Censored);
        }
    }

    private static int[] Ints(JsonElement frozen, string name) =>
        [.. frozen.GetProperty(name).EnumerateArray().Select(e => e.GetInt32())];

    private static void AssertAll(double[] expected, double[] actual, double tolerance)
    {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], tolerance);
        }
    }

    private static void AssertRelative(double[] expected, double[] actual, JsonElement frozen)
    {
        Assert.Equal(expected.Length, actual.Length);
        string name = frozen.GetProperty("name").GetString()!;
        for (int i = 0; i < expected.Length; i++)
        {
            if (double.IsNaN(expected[i]))
            {
                Assert.True(double.IsNaN(actual[i]),
                    $"[{name}] step {i}: lifelines reports NaN, this reports {actual[i]}.");
                continue;
            }

            double tolerance = Math.Max(1e-9 * Math.Abs(expected[i]), 1e-12);
            Assert.Equal(expected[i], actual[i], tolerance);
        }
    }
}
