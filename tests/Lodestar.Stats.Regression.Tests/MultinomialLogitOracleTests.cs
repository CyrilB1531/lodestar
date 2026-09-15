using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>Replays <c>statsmodels</c>' <c>MNLogit(...).fit()</c> over the frozen cases of <c>tests/oracles/stats_mnlogit.json</c> (#788).</summary>
/// <remarks>
/// Relative at 1e-9, as the GLM corpus beside it. The null log-likelihood and the two statistics built on it hold
/// only because each fixture's likelihood-ratio statistic is large: the reference reaches that null by an optimiser
/// and the C# by its closed form (decision 0136).
/// </remarks>
public sealed class MultinomialLogitOracleTests
{
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_mnlogit.json");

    private static IReadOnlyList<JsonElement> Cases => [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    private static MultinomialLogitSummary Fit(JsonElement frozen) => MultinomialLogit.Fit(
        [.. frozen.GetProperty("design").EnumerateArray().Select(v => v.GetDouble())],
        [.. frozen.GetProperty("labels").EnumerateArray().Select(v => v.GetInt32())],
        frozen.GetProperty("featureCount").GetInt32(),
        new MultinomialLogitOptions
        {
            WithIntercept = frozen.GetProperty("withIntercept").GetBoolean(),
            ConfidenceLevel = frozen.GetProperty("confidenceLevel").GetDouble(),
        });

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_coefficient_table_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        MultinomialLogitSummary actual = Fit(frozen);
        string name = frozen.GetProperty("name").GetString()!;

        Assert.Equal(
            [.. frozen.GetProperty("categories").EnumerateArray().Select(v => v.GetInt32())],
            actual.Categories);
        AssertTable(frozen, "coefficients", actual.Coefficients, name);
        AssertTable(frozen, "standardErrors", actual.StandardErrors, name);
        AssertTable(frozen, "zStatistics", actual.ZStatistics, name);
        AssertTable(frozen, "confidenceLower", actual.ConfidenceLower, name);
        AssertTable(frozen, "confidenceUpper", actual.ConfidenceUpper, name);

        JsonElement[] pRows = [.. frozen.GetProperty("pValues").EnumerateArray()];
        for (int j = 0; j < pRows.Length; j++)
        {
            StatsOracleAsserts.Vector(
                [.. pRows[j].EnumerateArray().Select(v => v.GetDouble())], [.. actual.PValues[j]], $"{name}: pValues[{j}]");
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_whole_model_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        MultinomialLogitSummary actual = Fit(frozen);
        string name = frozen.GetProperty("name").GetString()!;

        AssertScalar(frozen, "logLikelihood", actual.LogLikelihood, name);
        AssertScalar(frozen, "nullLogLikelihood", actual.NullLogLikelihood, name);
        AssertScalar(frozen, "pseudoRSquared", actual.PseudoRSquared, name);
        AssertScalar(frozen, "likelihoodRatio", actual.LikelihoodRatio, name);
        AssertScalar(frozen, "likelihoodRatioPValueClosedNull", actual.LikelihoodRatioPValue, name);

        // statsmodels' own p-value reads its optimiser's null, which the chi-squared tail moves to 1.3e-8 here.
        double reference = frozen.GetProperty("likelihoodRatioPValue").GetDouble();
        Assert.True(
            Math.Abs(actual.LikelihoodRatioPValue - reference) <= 5e-8 * reference,
            $"{name}: likelihoodRatioPValue {actual.LikelihoodRatioPValue:R} against statsmodels' {reference:R}");
        AssertScalar(frozen, "akaike", actual.Akaike, name);
        AssertScalar(frozen, "bayesian", actual.Bayesian, name);
        Assert.Equal(frozen.GetProperty("modelDegreesOfFreedom").GetInt32(), actual.ModelDegreesOfFreedom);
        Assert.Equal(frozen.GetProperty("residualDegreesOfFreedom").GetInt32(), actual.ResidualDegreesOfFreedom);
        Assert.Equal(frozen.GetProperty("iterations").GetInt32(), actual.Iterations);
        Assert.Equal(frozen.GetProperty("converged").GetBoolean(), actual.Converged);
    }

    private static void AssertTable(
        JsonElement frozen, string field, IReadOnlyList<IReadOnlyList<double>> actual, string name)
    {
        JsonElement[] rows = [.. frozen.GetProperty(field).EnumerateArray()];
        Assert.Equal(rows.Length, actual.Count);
        for (int j = 0; j < rows.Length; j++)
        {
            double[] expected = [.. rows[j].EnumerateArray().Select(v => v.GetDouble())];
            Assert.Equal(expected.Length, actual[j].Count);
            for (int k = 0; k < expected.Length; k++)
            {
                AssertRelative(expected[k], actual[j][k], $"{name}: {field}[{j}][{k}]");
            }
        }
    }

    private static void AssertScalar(JsonElement frozen, string field, double actual, string name) =>
        AssertRelative(frozen.GetProperty(field).GetDouble(), actual, $"{name}: {field}");

    private static void AssertRelative(double expected, double actual, string what)
    {
        double gap = Math.Abs(actual - expected) / Math.Abs(expected);
        Assert.True(gap <= Relative, $"{what}: expected {expected:R}, got {actual:R} — relative gap {gap:R}");
    }
}
