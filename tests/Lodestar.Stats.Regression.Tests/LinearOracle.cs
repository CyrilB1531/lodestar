using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>What the OLS and the WLS corpora share: their field names, their options, their tolerance.</summary>
/// <remarks>
/// One place because statsmodels returns the same results object from both and the C# the same
/// <see cref="OlsSummary"/>, so a replay that drifted between the two would be testing the
/// replay rather than the fit (#768).
/// </remarks>
internal static class LinearOracle
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Relative = 1e-9;

    public static IReadOnlyList<JsonElement> Cases(JsonDocument corpus) =>
        [.. corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices(JsonDocument corpus)
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases(corpus).Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    public static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    /// <summary>The estimator a case names, spelled as statsmodels' own `cov_type` string.</summary>
    public static CovarianceType Covariance(JsonElement frozen) =>
        frozen.GetProperty("covarianceType").GetString() switch
        {
            "HC0" => CovarianceType.Hc0,
            "HC1" => CovarianceType.Hc1,
            "HC2" => CovarianceType.Hc2,
            "HC3" => CovarianceType.Hc3,
            "HAC" => CovarianceType.Hac,
            "cluster" => CovarianceType.Cluster,
            _ => CovarianceType.Nonrobust,
        };

    public static OlsOptions Options(JsonElement frozen) => new()
    {
        WithIntercept = frozen.GetProperty("withIntercept").GetBoolean(),
        ConfidenceLevel = frozen.GetProperty("confidenceLevel").GetDouble(),
        CovarianceType = Covariance(frozen),
        HacLags = frozen.TryGetProperty("hacLags", out JsonElement lags) ? lags.GetInt32() : null,
        SmallSampleCorrection = frozen.TryGetProperty("useCorrection", out JsonElement correction) ? correction.GetBoolean() : null,
    };

    /// <summary>The cluster labels a case carries, or <see langword="null"/> for a case fitted without them (#775).</summary>
    public static int[]? Groups(JsonElement frozen) =>
        frozen.TryGetProperty("groups", out JsonElement groups) ? [.. groups.EnumerateArray().Select(v => v.GetInt32())] : null;

    public static void AssertEstimates(JsonElement frozen, OlsSummary summary)
    {
        AssertList(frozen, "coefficients", summary.Coefficients);
        AssertList(frozen, "standardErrors", summary.StandardErrors);
        AssertList(frozen, "tStatistics", summary.TStatistics);
    }

    public static void AssertTests(JsonElement frozen, OlsSummary summary)
    {
        AssertList(frozen, "pValues", summary.PValues);
        AssertList(frozen, "confidenceLower", summary.ConfidenceLower);
        AssertList(frozen, "confidenceUpper", summary.ConfidenceUpper);
    }

    public static void AssertWholeModel(JsonElement frozen, OlsSummary summary)
    {
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

    public static void AssertList(JsonElement frozen, string name, IReadOnlyList<double> actual)
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
