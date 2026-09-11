using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>Replays <c>statsmodels.GLM(...).fit()</c> over the frozen cases of <c>stats_glm.json</c>.</summary>
public sealed class GlmOracleTests
{
    private const double Absolute = 1e-9;

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

        GlmSummary actual = GeneralizedLinearModel.Fit(
            Doubles(expected, "design"),
            Doubles(expected, "response"),
            expected.GetProperty("featureCount").GetInt32(),
            family,
            new GlmOptions
            {
                WithIntercept = expected.GetProperty("withIntercept").GetBoolean(),
                ConfidenceLevel = expected.GetProperty("confidenceLevel").GetDouble(),
            });

        AssertVector(Doubles(expected, "coefficients"), actual.Coefficients);
        AssertVector(Doubles(expected, "standardErrors"), actual.StandardErrors);
        AssertVector(Doubles(expected, "zStatistics"), actual.ZStatistics);
        AssertVector(Doubles(expected, "confidenceLower"), actual.ConfidenceLower);
        AssertVector(Doubles(expected, "confidenceUpper"), actual.ConfidenceUpper);
        StatsOracleAsserts.Vector(Doubles(expected, "pValues"), [.. actual.PValues], caseName);

        Assert.Equal(expected.GetProperty("deviance").GetDouble(), actual.Deviance, Absolute);
        Assert.Equal(expected.GetProperty("nullDeviance").GetDouble(), actual.NullDeviance, Absolute);
        Assert.Equal(expected.GetProperty("dispersion").GetDouble(), actual.Dispersion, Absolute);
        Assert.Equal(expected.GetProperty("logLikelihood").GetDouble(), actual.LogLikelihood, Absolute);
        Assert.Equal(expected.GetProperty("akaike").GetDouble(), actual.Akaike, Absolute);
        Assert.Equal(
            expected.GetProperty("residualDegreesOfFreedom").GetInt32(),
            actual.ResidualDegreesOfFreedom);
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

    private static void AssertVector(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Absolute);
        }
    }
}
