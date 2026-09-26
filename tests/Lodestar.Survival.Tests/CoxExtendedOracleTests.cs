using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>Stratified, weighted, penalised, robust and clustered Cox fits, their baselines and predictions, the
/// proportional hazards test and time-varying Cox, against lifelines 0.30.3.</summary>
/// <remarks>Values at 1e-9, absolutely below one and relatively above; p-values relatively, as the Cox suite compares them.</remarks>
public sealed class CoxExtendedOracleTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_cox_extended.json");

    public static TheoryData<string, int> Cases()
    {
        var data = new TheoryData<string, int>();
        foreach (string section in (string[])["fits", "ph_tests", "time_varying"])
        {
            for (int i = 0; i < Corpus.RootElement.GetProperty(section).GetArrayLength(); i++)
            {
                data.Add(section, i);
            }
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void The_extended_cox_matches_lifelines(string section, int index)
    {
        JsonElement c = Corpus.RootElement.GetProperty(section)[index];
        string name = c.GetProperty("name").GetString()!;
        if (section == "time_varying")
        {
            TimeVarying(c, name);
            return;
        }

        CoxSummary fit = Fit(c);
        Near(c, "coefficients", fit.Coefficients, name);
        Near(c, "standardErrors", fit.StandardErrors, name);
        Relative(c, "pValues", fit.PValues, name);
        Near(c.GetProperty("logLikelihood").GetDouble(), fit.LogLikelihood, $"{name}: log-likelihood");
        Near(c.GetProperty("nullLogLikelihood").GetDouble(), fit.NullLogLikelihood, $"{name}: null log-likelihood");
        Near(c.GetProperty("concordanceIndex").GetDouble(), fit.ConcordanceIndex, $"{name}: concordance");
        Baselines(c, fit, name);
        Predictions(c, fit, name);
        if (section == "ph_tests")
        {
            ProportionalHazards(c, fit, name);
        }
    }

    private static CoxSummary Fit(JsonElement c)
    {
        var options = new CoxOptions
        {
            Penalizer = c.GetProperty("penalizer").GetDouble(),
            L1Ratio = c.GetProperty("l1Ratio").GetDouble(),
            Robust = c.GetProperty("robust").GetBoolean(),
        };
        return CoxProportionalHazards.Fit(
            Doubles(c, "design"), Doubles(c, "durations"), Flags(c, "eventObserved"),
            Optional(c, "weights"), OptionalInts(c, "strata"), OptionalInts(c, "clusters"),
            c.GetProperty("featureCount").GetInt32(), options);
    }

    private static void Baselines(JsonElement c, CoxSummary fit, string name)
    {
        JsonElement expected = c.GetProperty("baselines");
        Assert.Equal(expected.GetArrayLength(), fit.Baselines.Count);
        for (int s = 0; s < fit.Baselines.Count; s++)
        {
            CoxBaseline baseline = fit.Baselines[s];
            Assert.Equal(expected[s].GetProperty("stratum").GetInt32(), baseline.Stratum);
            Assert.Equal(Doubles(expected[s], "times"), baseline.Times);
            Near(expected[s], "cumulativeHazard", baseline.CumulativeHazard, $"{name}, stratum {baseline.Stratum}");
        }
    }

    private static void Predictions(JsonElement c, CoxSummary fit, string name)
    {
        double[] rows = Doubles(c, "predictRows");
        int[] strata = OptionalInts(c, "predictStrata");
        double[] times = Doubles(c, "predictTimes");
        Near(c, "logPartialHazards", fit.PredictLogPartialHazard(rows), $"{name}: log partial hazard");
        Near(c, "survival", fit.PredictSurvivalFunction(rows, strata, times), $"{name}: survival");
        Near(c, "cumulativeHazard", fit.PredictCumulativeHazard(rows, strata, times), $"{name}: cumulative hazard");
        Near(c, "expectations", fit.PredictExpectation(rows, strata), $"{name}: expectation");
        double[] medians = fit.PredictMedian(rows, strata);
        JsonElement expected = c.GetProperty("medians");
        for (int i = 0; i < medians.Length; i++)
        {
            double value = expected[i].ValueKind == JsonValueKind.Null ? double.PositiveInfinity : expected[i].GetDouble();
            Assert.Equal(value, medians[i]);
        }
    }

    private static void ProportionalHazards(JsonElement c, CoxSummary fit, string name)
    {
        foreach ((string key, CoxTimeTransform transform) in (ValueTuple<string, CoxTimeTransform>[])[
            ("rank", CoxTimeTransform.Rank), ("kaplanMeier", CoxTimeTransform.KaplanMeier),
            ("identity", CoxTimeTransform.Identity), ("log", CoxTimeTransform.Log)])
        {
            IReadOnlyList<Stats.TestResult> tests = CoxProportionalHazards.TestProportionalHazards(
                Doubles(c, "design"), Doubles(c, "durations"), Flags(c, "eventObserved"),
                Optional(c, "weights"), OptionalInts(c, "strata"), fit, transform);
            JsonElement expected = c.GetProperty("statistics").GetProperty(key);
            Near(expected, "statistics", [.. tests.Select(t => t.Statistic)], $"{name}: {key} statistic");
            Relative(expected, "pValues", [.. tests.Select(t => t.PValue)], $"{name}: {key}");
        }
    }

    private static void TimeVarying(JsonElement c, string name)
    {
        var options = new CoxOptions { Penalizer = c.GetProperty("penalizer").GetDouble(), L1Ratio = c.GetProperty("l1Ratio").GetDouble() };
        CoxSummary fit = CoxTimeVarying.Fit(
            Doubles(c, "design"), Doubles(c, "starts"), Doubles(c, "stops"), Flags(c, "eventObserved"),
            Optional(c, "weights"), OptionalInts(c, "strata"), c.GetProperty("featureCount").GetInt32(), options);
        Near(c, "coefficients", fit.Coefficients, name);
        Near(c, "standardErrors", fit.StandardErrors, name);
        Near(c.GetProperty("logLikelihood").GetDouble(), fit.LogLikelihood, $"{name}: log-likelihood");
        Near(c.GetProperty("nullLogLikelihood").GetDouble(), fit.NullLogLikelihood, $"{name}: null log-likelihood");
        Assert.Equal(Doubles(c, "times"), fit.Baselines[0].Times);
        Near(c, "cumulativeHazard", fit.Baselines[0].CumulativeHazard, $"{name}: baseline");
        Near(c, "partialHazards", fit.PredictPartialHazard(Doubles(c, "predictRows")), $"{name}: partial hazard");
    }

    private static void Near(JsonElement c, string key, IReadOnlyList<double> actual, string name)
    {
        double[] expected = Doubles(c, key);
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Near(expected[i], actual[i], $"{name}: {key}[{i}]");
        }
    }

    private static void Near(double expected, double actual, string name) =>
        Assert.True(
            Math.Abs(expected - actual) <= Tolerance * Math.Max(1.0, Math.Abs(expected)),
            $"{name}: {actual} against {expected}");

    private static void Relative(JsonElement c, string key, IReadOnlyList<double> actual, string name)
    {
        double[] expected = Doubles(c, key);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= Math.Max(Tolerance * expected[i], 1e-14),
                $"{name}: {key}[{i}] {actual[i]} against {expected[i]}");
        }
    }

    private static double[] Optional(JsonElement c, string name) =>
        c.GetProperty(name).ValueKind == JsonValueKind.Null ? [] : Doubles(c, name);

    private static int[] OptionalInts(JsonElement c, string name) =>
        c.GetProperty(name).ValueKind == JsonValueKind.Null ? [] : [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32())];

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetDouble())];

    private static bool[] Flags(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32() == 1)];
}
