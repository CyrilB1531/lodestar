using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_polynomial.json</c>, the column names included.</summary>
public sealed class PolynomialFeaturesOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_polynomial.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            var options = new PolynomialFeaturesOptions
            {
                Degree = args.GetProperty("degree").GetInt32(),
                InteractionOnly = args.GetProperty("interactionOnly").GetBoolean(),
                IncludeBias = args.GetProperty("includeBias").GetBoolean(),
            };

            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            string[] names = [.. c.GetProperty("names").EnumerateArray().Select(v => v.GetString()!)];

            // The column order is the contract, so the names are compared exactly rather than
            // counted: a caller reading a coefficient back needs to know which term it belongs to.
            Assert.Equal(names, PolynomialFeatures.FeatureNames(featureCount, options));
            Assert.Equal(names.Length, PolynomialFeatures.OutputFeatureCount(featureCount, options));
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")),
                PolynomialFeatures.Transform(samples, featureCount, options),
                name);
            replayed++;
        }

        Assert.True(replayed >= 30, $"only {replayed} cases replayed");
    }

    /// <summary>The one pair the reference refuses, refused here too rather than answered with nothing.</summary>
    /// <remarks>
    /// Measured against scikit-learn 1.9.1: it raises <c>ValueError("Setting degree to zero and
    /// include_bias to False would result in an empty output array.")</c>. Not in the corpus,
    /// because a corpus holds values and this case has none.
    /// </remarks>
    [Fact]
    public void Degree_zero_without_a_bias_is_refused()
    {
        var options = new PolynomialFeaturesOptions { Degree = 0, IncludeBias = false };
        double[] row = [3.0, 4.0];

        Assert.Throws<ArgumentException>(() => PolynomialFeatures.Transform(row, 2, options));
        Assert.Throws<ArgumentException>(() => PolynomialFeatures.FeatureNames(2, options));
        Assert.Throws<ArgumentException>(() => PolynomialFeatures.OutputFeatureCount(2, options));

        // Degree 0 with the bias is the one-column expansion the reference does answer.
        Assert.Equal(
            ["1"],
            PolynomialFeatures.FeatureNames(2, new PolynomialFeaturesOptions { Degree = 0 }));
    }
}
