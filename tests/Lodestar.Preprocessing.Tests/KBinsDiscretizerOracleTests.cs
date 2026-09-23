using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_kbins.json</c>.</summary>
public sealed class KBinsDiscretizerOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_kbins.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            var options = new KBinsDiscretizerOptions
            {
                BinCount = args.GetProperty("binCount").GetInt32(),
                Strategy = args.GetProperty("strategy").GetString() switch
                {
                    "uniform" => BinStrategy.Uniform,
                    "kmeans" => BinStrategy.KMeans,
                    _ => BinStrategy.Quantile,
                },
                Encoding = args.GetProperty("encoding").GetString() == "ordinal"
                    ? BinEncoding.Ordinal
                    : BinEncoding.OneHot,
                QuantileMethod = args.GetProperty("quantileMethod").GetString() == "linear"
                    ? QuantileMethod.Linear
                    : QuantileMethod.AveragedInvertedCdf,
            };

            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            KBinsDiscretizer fitted = KBinsDiscretizer.Fit(samples, featureCount, options);

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("binEdges")),
                [.. fitted.BinEdges[0]],
                $"{name} edges");

            double[] transformed = fitted.Transform(samples);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")), transformed, name);
            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("inverse")),
                fitted.InverseTransform(transformed),
                $"{name} inverse");
            replayed++;
        }

        Assert.True(replayed >= 19, $"only {replayed} cases replayed");
    }
}
