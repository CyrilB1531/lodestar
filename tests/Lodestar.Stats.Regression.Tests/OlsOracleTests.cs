using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Replays <c>statsmodels.api.OLS(...).fit()</c> over the six frozen cases of
/// <c>tests/oracles/stats_ols.json</c>.
/// </summary>
/// <remarks>
/// Each case is chosen for something an inference table can get wrong rather than for
/// something a solve can: a fitted intercept and none, a 99% level, a near-collinear pair
/// whose VIF reaches 6e4, and one residual degree of freedom.
/// </remarks>
public sealed class OlsOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_ols.json");

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
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static OlsSummary Fit(JsonElement frozen) => OrdinaryLeastSquares.Fit(
        Doubles(frozen, "design"),
        Doubles(frozen, "response"),
        frozen.GetProperty("featureCount").GetInt32(),
        new OlsOptions
        {
            WithIntercept = frozen.GetProperty("withIntercept").GetBoolean(),
            ConfidenceLevel = frozen.GetProperty("confidenceLevel").GetDouble(),
        });

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_estimates_and_their_errors_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        OlsSummary summary = Fit(frozen);

        AssertList(frozen, "coefficients", summary.Coefficients);
        AssertList(frozen, "standardErrors", summary.StandardErrors);
        AssertList(frozen, "tStatistics", summary.TStatistics);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_p_values_and_intervals_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        OlsSummary summary = Fit(frozen);

        AssertList(frozen, "pValues", summary.PValues);
        AssertList(frozen, "confidenceLower", summary.ConfidenceLower);
        AssertList(frozen, "confidenceUpper", summary.ConfidenceUpper);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_whole_model_statistics_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        OlsSummary summary = Fit(frozen);

        AssertRelative(frozen.GetProperty("rSquared").GetDouble(), summary.RSquared, "rSquared");
        AssertRelative(
            frozen.GetProperty("adjustedRSquared").GetDouble(), summary.AdjustedRSquared, "adjustedRSquared");
        AssertRelative(frozen.GetProperty("fStatistic").GetDouble(), summary.FStatistic, "fStatistic");
        AssertRelative(frozen.GetProperty("fPValue").GetDouble(), summary.FPValue, "fPValue");
        AssertRelative(
            frozen.GetProperty("residualStandardError").GetDouble(),
            summary.ResidualStandardError,
            "residualStandardError");
        Assert.Equal(
            frozen.GetProperty("residualDegreesOfFreedom").GetInt32(), summary.ResidualDegreesOfFreedom);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_variance_inflation_factors_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        AssertList(frozen, "varianceInflationFactors", Fit(frozen).VarianceInflationFactors);
    }

    private static void AssertList(JsonElement frozen, string name, IReadOnlyList<double> actual)
    {
        double[] expected = Doubles(frozen, name);

        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            AssertRelative(expected[i], actual[i], $"{name}[{i}]");
        }
    }

    /// <summary>
    /// Relative, because the corpus reaches 2.9e-11 and a VIF reaches 6e4: one absolute
    /// tolerance cannot hold both ends, and at the small end it would assert only that a
    /// number came back. Decision 0081 settled this for Lodestar.Stats already.
    /// </summary>
    private static void AssertRelative(double expected, double actual, string what)
    {
        double gap = Math.Abs(actual - expected) / Math.Abs(expected);

        Assert.True(
            gap <= Relative,
            $"{what}: expected {expected:R}, got {actual:R} — relative gap {gap:R}");
    }
}
