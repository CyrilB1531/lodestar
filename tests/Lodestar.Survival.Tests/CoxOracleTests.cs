using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>The whole Cox table against <c>lifelines.CoxPHFitter</c> 0.30.3, frozen at its maximum.</summary>
/// <remarks>
/// Floats at 1e-9 absolute and p-values at 1e-9 relative, as decision 0081 set for every p-value
/// here. The corpus was fitted at <c>precision=1e-20</c>, which puts it within 3.1e-13 of an
/// independent Newton-Raphson, so the repository's usual tolerances apply unchanged.
/// </remarks>
public sealed class CoxOracleTests
{
    private const double Absolute = 1e-9;
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_cox.json");

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

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(value => value.GetDouble())];

    private static CoxSummary Fit(JsonElement fixture)
    {
        bool[] events =
            [.. fixture.GetProperty("eventObserved").EnumerateArray().Select(value => value.GetInt32() == 1)];
        return CoxProportionalHazards.Fit(
            Doubles(fixture, "design"),
            Doubles(fixture, "durations"),
            events,
            fixture.GetProperty("featureCount").GetInt32());
    }

    private static void AssertList(JsonElement fixture, string name, IReadOnlyList<double> actual, bool relative = false)
    {
        double[] expected = Doubles(fixture, name);
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            AssertValue(expected[i], actual[i], $"{name}[{i}]", relative);
        }
    }

    private static void AssertValue(double expected, double actual, string name, bool relative = false)
    {
        double bound = relative ? Relative * Math.Abs(expected) : Absolute;
        Assert.True(
            Math.Abs(expected - actual) <= bound,
            $"{name}: expected {expected:R}, got {actual:R}, off by {Math.Abs(expected - actual):E2}");
    }

    [Fact]
    public void Metadata_is_lifelines_with_efron_ties()
    {
        JsonElement metadata = Corpus.RootElement.GetProperty("metadata");
        Assert.Equal("lifelines", metadata.GetProperty("library").GetString());
        Assert.Equal("survival-cox", metadata.GetProperty("family").GetString());
        Assert.Equal(5, Cases.Count);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_coefficient_table_matches_lifelines(int index)
    {
        JsonElement fixture = Cases[index];

        CoxSummary summary = Fit(fixture);

        AssertList(fixture, "coefficients", summary.Coefficients);
        AssertList(fixture, "standardErrors", summary.StandardErrors);
        AssertList(fixture, "zStatistics", summary.ZStatistics);
        AssertList(fixture, "pValues", summary.PValues, relative: true);
        AssertList(fixture, "confidenceLower", summary.ConfidenceLower);
        AssertList(fixture, "confidenceUpper", summary.ConfidenceUpper);
        AssertList(fixture, "hazardRatios", summary.HazardRatios);
        AssertList(fixture, "hazardRatioLower", summary.HazardRatioLower);
        AssertList(fixture, "hazardRatioUpper", summary.HazardRatioUpper);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_likelihood_ratio_test_and_concordance_match_lifelines(int index)
    {
        JsonElement fixture = Cases[index];

        CoxSummary summary = Fit(fixture);

        AssertValue(fixture.GetProperty("logLikelihood").GetDouble(), summary.LogLikelihood, "logLikelihood");
        AssertValue(fixture.GetProperty("nullLogLikelihood").GetDouble(), summary.NullLogLikelihood, "nullLogLikelihood");
        AssertValue(
            fixture.GetProperty("likelihoodRatioStatistic").GetDouble(),
            summary.LikelihoodRatioStatistic, "likelihoodRatioStatistic");
        AssertValue(
            fixture.GetProperty("likelihoodRatioPValue").GetDouble(),
            summary.LikelihoodRatioPValue, "likelihoodRatioPValue", relative: true);
        Assert.Equal(
            fixture.GetProperty("likelihoodRatioDegreesOfFreedom").GetInt32(),
            summary.LikelihoodRatioDegreesOfFreedom);
        AssertValue(fixture.GetProperty("concordanceIndex").GetDouble(), summary.ConcordanceIndex, "concordanceIndex");
        Assert.Equal(fixture.GetProperty("confidenceLevel").GetDouble(), summary.ConfidenceLevel);
    }
}
