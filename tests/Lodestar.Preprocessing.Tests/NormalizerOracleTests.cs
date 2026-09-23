using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_normalizer.json</c>.</summary>
public sealed class NormalizerOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_normalizer.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            double[] samples = PreprocessingOracleAsserts.Doubles(c.GetProperty("samples"));
            int featureCount = c.GetProperty("featureCount").GetInt32();
            RowNorm norm = c.GetProperty("args").GetProperty("norm").GetString() switch
            {
                "l1" => RowNorm.L1,
                "max" => RowNorm.Max,
                _ => RowNorm.L2,
            };

            PreprocessingOracleAsserts.Row(
                PreprocessingOracleAsserts.Doubles(c.GetProperty("transformed")),
                Normalizer.Transform(samples, featureCount, norm),
                name);
            replayed++;
        }

        Assert.True(replayed >= 9, $"only {replayed} cases replayed");
    }

    /// <summary>The sparse overload agrees with the dense one, and leaves its input alone.</summary>
    [Fact]
    public void The_sparse_overload_agrees_with_the_dense_one()
    {
        double[] dense = [1.0, 0.0, -2.0, 0.0, 3.0, 4.0];
        var sparse = new Lodestar.Abstractions.CsrMatrix(
            2, 3, [1.0, -2.0, 3.0, 4.0], [0, 2, 1, 2], [0, 2, 4]);

        double[] scaled = Normalizer.Transform(dense, 3, RowNorm.L2);
        Lodestar.Abstractions.CsrMatrix result = Normalizer.Transform(sparse, RowNorm.L2);

        Assert.Equal(scaled[0], result.Values[0], 12);
        Assert.Equal(scaled[2], result.Values[1], 12);
        Assert.Equal(scaled[4], result.Values[2], 12);
        Assert.Equal(scaled[5], result.Values[3], 12);
        Assert.Equal([1.0, -2.0, 3.0, 4.0], sparse.Values);
    }
}
