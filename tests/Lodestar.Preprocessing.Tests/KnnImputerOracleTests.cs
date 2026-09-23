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

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")),
                KnnImputer.Fit(samples, featureCount, options).Transform(samples),
                name);
            replayed++;
        }

        Assert.True(replayed >= 18, $"only {replayed} cases replayed");
    }

    /// <summary>Past the measured ceiling the call is refused rather than run for however long it takes.</summary>
    [Fact]
    public void A_matrix_past_the_distance_ceiling_is_refused()
    {
        // Rows * rows * features must exceed the ceiling: 11,000 rows of one feature is
        // 121 million terms, just past it.
        var big = new double[11_000];
        KnnImputer imputer = KnnImputer.Fit(big, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => imputer.Transform(big));
    }

    /// <summary>
    /// A feature missing from every fitted row is dropped rather than filled, as the reference
    /// drops it: there is nothing to impute it from and nothing to impute it with.
    /// </summary>
    [Fact]
    public void A_feature_missing_everywhere_is_dropped()
    {
        double[] samples = [1.0, double.NaN, 4.0, double.NaN, 7.0, double.NaN];

        KnnImputer imputer = KnnImputer.Fit(samples, 2);

        Assert.Equal(1, imputer.OutputFeatureCount);
        Assert.Equal([0], imputer.KeptFeatures);
        Assert.Equal([1.0, 4.0, 7.0], imputer.Transform(samples));
    }
}
