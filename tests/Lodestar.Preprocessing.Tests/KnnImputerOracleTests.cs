using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_knn_imputer.json</c>.</summary>
public sealed class KnnImputerOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_knn_imputer.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            var options = new KnnImputerOptions
            {
                NeighbourCount = args.GetProperty("neighbourCount").GetInt32(),
                Weights = args.GetProperty("weights").GetString() == "distance"
                    ? NeighbourWeights.Distance
                    : NeighbourWeights.Uniform,
            };

            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            KnnImputer imputer = KnnImputer.Fit(samples, featureCount, options);

            // A feature missing from every fitted row is dropped, so the output can be narrower
            // than the input; get_feature_names_out is how the reference says which survived.
            Assert.Equal(c.GetProperty("outputFeatureCount").GetInt32(), imputer.OutputFeatureCount);
            Assert.Equal(
                [.. c.GetProperty("keptFeatures").EnumerateArray().Select(v => v.GetInt32())],
                imputer.KeptFeatures);

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")),
                imputer.Transform(samples),
                name);
            replayed++;
        }

        Assert.True(replayed >= 18, $"only {replayed} cases replayed");
    }

    /// <summary>Past the measured ceiling the call is refused rather than run for however long it takes.</summary>
    /// <remarks>
    /// Hand-written rather than frozen, and it has to be: the ceiling is this package's own
    /// refusal, so the reference has no answer to capture (#1128).
    /// </remarks>
    [Fact]
    public void A_matrix_past_the_distance_ceiling_is_refused()
    {
        // Rows * rows * features must exceed the ceiling: 11,000 rows of one feature is
        // 121 million terms, just past it.
        var big = new double[11_000];
        KnnImputer imputer = KnnImputer.Fit(big, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => imputer.Transform(big));
    }

}
