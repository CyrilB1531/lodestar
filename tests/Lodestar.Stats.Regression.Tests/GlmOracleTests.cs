using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>Replays <c>statsmodels.GLM(...).fit()</c> over the frozen cases of <c>stats_glm.json</c>.</summary>
public sealed class GlmOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_glm.json");

    private static IReadOnlyList<JsonElement> Cases(string block) =>
        [.. Corpus.RootElement.GetProperty(block).GetProperty("cases").EnumerateArray()];

    public static TheoryData<string, int> Indices()
    {
        var data = new TheoryData<string, int>();
        foreach (string block in new[] { "binomial", "poisson" })
        {
            for (int i = 0; i < Cases(block).Count; i++)
            {
                data.Add(block, i);
            }
        }

        return data;
    }

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    [Theory]
    [MemberData(nameof(Indices))]
    public void Every_case_matches_statsmodels(string block, int index)
    {
        JsonElement expected = Cases(block)[index];
        string caseName = expected.GetProperty("name").GetString() ?? $"{block}[{index}]";
        GlmFamily family = block == "binomial" ? GlmFamily.Binomial : GlmFamily.Poisson;
        bool withIntercept = expected.GetProperty("withIntercept").GetBoolean();

        GlmSummary actual = GeneralizedLinearModel.Fit(
            Doubles(expected, "design"),
            Doubles(expected, "response"),
            expected.GetProperty("featureCount").GetInt32(),
            family,
            new GlmOptions
            {
                WithIntercept = withIntercept,
                ConfidenceLevel = expected.GetProperty("confidenceLevel").GetDouble(),
            });

        AssertVector(expected, "coefficients", actual.Coefficients, caseName);
        AssertVector(expected, "standardErrors", actual.StandardErrors, caseName);
        AssertVector(expected, "zStatistics", actual.ZStatistics, caseName);
        AssertVector(expected, "confidenceLower", actual.ConfidenceLower, caseName);
        AssertVector(expected, "confidenceUpper", actual.ConfidenceUpper, caseName);
        StatsOracleAsserts.Vector(Doubles(expected, "pValues"), [.. actual.PValues], caseName);

        AssertScalar(expected, "deviance", actual.Deviance, caseName);
        AssertScalar(expected, "nullDeviance", actual.NullDeviance, caseName);
        AssertScalar(expected, "dispersion", actual.Dispersion, caseName);
        AssertScalar(expected, "logLikelihood", actual.LogLikelihood, caseName);
        AssertScalar(expected, "akaike", actual.Akaike, caseName);
        Assert.Equal(
            expected.GetProperty("residualDegreesOfFreedom").GetInt32(),
            actual.ResidualDegreesOfFreedom);
        Assert.Equal(expected.GetProperty("iterations").GetInt32(), actual.Iterations);
        Assert.Equal(withIntercept, actual.HasIntercept);
        Assert.True(actual.Converged);
    }

    [Fact]
    public void The_separable_case_does_not_converge_and_says_how_far_it_got()
    {
        JsonElement block = Corpus.RootElement.GetProperty("separable");

        GlmSummary actual = GeneralizedLinearModel.Fit(
            Doubles(block, "design"),
            Doubles(block, "response"),
            block.GetProperty("featureCount").GetInt32(),
            GlmFamily.Binomial,
            new GlmOptions
            {
                MaximumIterations = block.GetProperty("maximumIterations").GetInt32(),
                ThrowOnNonConvergence = false,
            });

        Assert.Equal(block.GetProperty("converged").GetBoolean(), actual.Converged);
        Assert.Equal(block.GetProperty("iterations").GetInt32(), actual.Iterations);
    }

    private static void AssertVector(
        JsonElement frozen, string name, IReadOnlyList<double> actual, string caseName)
    {
        double[] expected = Doubles(frozen, name);

        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            AssertRelative(expected[i], actual[i], $"{caseName}: {name}[{i}]");
        }
    }

    private static void AssertScalar(
        JsonElement frozen, string name, double actual, string caseName) =>
        AssertRelative(frozen.GetProperty(name).GetDouble(), actual, $"{caseName}: {name}");

    /// <summary>
    /// Relative, as <c>OlsOracleTests</c> compares the table beside this one: measured over this
    /// corpus the worst field reaches 3.4e-14, and an absolute tolerance would assert only that a
    /// number came back wherever a z statistic or a deviance is small. No frozen value here is an
    /// exact zero, so every field has a relative neighbourhood to be compared in. Decision 0081
    /// settled this for Lodestar.Stats already.
    /// </summary>
    private static void AssertRelative(double expected, double actual, string what)
    {
        double gap = Math.Abs(actual - expected) / Math.Abs(expected);

        Assert.True(
            gap <= Relative,
            $"{what}: expected {expected:R}, got {actual:R} — relative gap {gap:R}");
    }
}
