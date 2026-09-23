using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_quantile.json</c>, frozen at <c>subsample=None</c>.</summary>
public sealed class QuantileTransformerOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_quantile.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            var options = new QuantileTransformerOptions
            {
                QuantileCount = args.GetProperty("quantileCount").GetInt32(),
                Output = args.GetProperty("output").GetString() == "normal"
                    ? QuantileOutput.Normal
                    : QuantileOutput.Uniform,
            };

            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            double[] probe = PreprocessingOracleAsserts.Doubles(c.GetProperty("probe"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            QuantileTransformer fitted = QuantileTransformer.Fit(samples, featureCount, options);

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("references")),
                [.. fitted.References],
                $"{name} references");
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("quantiles")),
                [.. fitted.Quantiles[0]],
                $"{name} quantiles");

            double[] transformed = fitted.Transform(probe);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")), transformed, name);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("inverse")),
                fitted.InverseTransform(transformed),
                $"{name} inverse");
            replayed++;
        }

        Assert.True(replayed >= 6, $"only {replayed} cases replayed");
    }
}
