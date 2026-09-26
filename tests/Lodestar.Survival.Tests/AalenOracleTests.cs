using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>lifelines' <c>AalenAdditiveFitter</c> 0.30.3: its tables, bounds, slopes, concordance and predictions.</summary>
/// <remarks>Values at 1e-9, absolutely below one and relatively above; a percentile lifelines finds infinite is frozen as null.</remarks>
public sealed class AalenOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_aalen.json");

    public static TheoryData<int> Cases()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Corpus.RootElement.GetProperty("fits").GetArrayLength(); i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void The_aalen_fit_matches_lifelines(int index)
    {
        JsonElement c = Corpus.RootElement.GetProperty("fits")[index];
        string name = c.GetProperty("name").GetString()!;
        JsonElement o = c.GetProperty("options");
        var options = new AalenOptions
        {
            FitIntercept = o.GetProperty("fitIntercept").GetBoolean(),
            ConfidenceLevel = 1.0 - o.GetProperty("alpha").GetDouble(),
            CoefficientPenalizer = o.GetProperty("coefPenalizer").GetDouble(),
            SmoothingPenalizer = o.GetProperty("smoothingPenalizer").GetDouble(),
        };
        double[] weights = o.GetProperty("weighted").GetBoolean() ? Doubles(c, "weights") : [];
        int width = c.GetProperty("design")[0].GetArrayLength();
        AalenSummary fit = AalenAdditive.Fit(Rows(c, "design"), Doubles(c, "durations"), Flags(c, "eventObserved"), weights, width, options);

        string[] names = [.. fit.CovariateIndices.Select(j => j < 0 ? "Intercept" : $"x{j}")];
        Assert.Equal(c.GetProperty("names").EnumerateArray().Select(e => e.GetString()), names);
        Near(c, "eventTimes", fit.EventTimes, name);
        Near(c, "hazards", fit.Hazards, name);
        Near(c, "cumulativeHazard", fit.CumulativeHazards, name);
        Near(c, "cumulativeVariance", fit.CumulativeVariance, name);
        Near(c, "confidenceLower", fit.ConfidenceLower, name);
        Near(c, "confidenceUpper", fit.ConfidenceUpper, name);
        Near(c, "slopes", fit.Slopes, name);
        Near(c, "slopeErrors", fit.SlopeStandardErrors, name);
        JsonElement concordance = c.GetProperty("concordance");
        if (concordance.ValueKind != JsonValueKind.Null)
        {
            Near(concordance.GetDouble(), fit.ConcordanceIndex, $"{name}: concordance");
        }

        double[] predict = Rows(c, "predictDesign");
        Near(c, "predictedCumulativeHazard", fit.PredictCumulativeHazard(predict), name);
        Near(c, "median", fit.PredictMedian(predict), name);
        Near(c, "quartile", fit.PredictPercentile(predict, 0.75), name);
        Near(c, "expectation", fit.PredictExpectation(predict), name);
        Near(c, "smoothed", fit.SmoothedHazards(1.5), name);
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

    private static void Near(double expected, double actual, string name)
    {
        if (double.IsInfinity(expected) || double.IsNaN(expected))
        {
            Assert.Equal(expected, actual);
            return;
        }

        Assert.True(Math.Abs(expected - actual) <= 1e-9 * Math.Max(1.0, Math.Abs(expected)), $"{name}: {actual} against {expected}");
    }

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.ValueKind == JsonValueKind.Null ? double.PositiveInfinity : e.GetDouble())];

    private static double[] Rows(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().SelectMany(row => row.EnumerateArray().Select(e => e.GetDouble()))];

    private static bool[] Flags(JsonElement c, string name) => [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32() == 1)];
}
