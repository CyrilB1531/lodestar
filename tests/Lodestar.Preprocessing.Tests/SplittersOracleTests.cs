using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>sklearn.model_selection</c> over <c>tests/oracles/preprocessing_splitters.json</c> (#762).</summary>
/// <remarks>
/// Compared exactly: these are indices. The two permuted cases pass the reference's own permutation in, which is what
/// decision 0132 makes the input rather than a seed.
/// </remarks>
public sealed class SplittersOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument corpus = OracleLoader.Load("preprocessing_splitters.json");
        int replayed = 0;

        foreach (JsonElement frozen in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = frozen.GetProperty("name").GetString()!;
            int[] order = frozen.TryGetProperty("order", out JsonElement given) ? Integers(given) : [];

            switch (frozen.GetProperty("call").GetString())
            {
                case "kfold":
                    AssertFolds(
                        frozen,
                        Splitters.KFold(
                            frozen.GetProperty("sampleCount").GetInt32(), frozen.GetProperty("foldCount").GetInt32(), order),
                        name);
                    break;
                case "stratified":
                    AssertFolds(
                        frozen,
                        Splitters.StratifiedKFold(
                            Integers(frozen.GetProperty("labels")), frozen.GetProperty("foldCount").GetInt32(), order),
                        name);
                    break;
                default:
                    TrainTestSplit split = Splitters.TrainTest(
                        frozen.GetProperty("sampleCount").GetInt32(), frozen.GetProperty("testFraction").GetDouble(), order);
                    Assert.Equal(Integers(frozen.GetProperty("trainIndices")), split.TrainIndices);
                    Assert.Equal(Integers(frozen.GetProperty("testIndices")), split.TestIndices);
                    break;
            }

            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static void AssertFolds(JsonElement frozen, IReadOnlyList<FoldSplit> actual, string name)
    {
        JsonElement[] expected = [.. frozen.GetProperty("folds").EnumerateArray()];

        Assert.Equal(expected.Length, actual.Count);
        for (int fold = 0; fold < expected.Length; fold++)
        {
            Assert.Equal(Integers(expected[fold].GetProperty("testIndices")), actual[fold].TestIndices);
            Assert.Equal(Integers(expected[fold].GetProperty("trainIndices")), actual[fold].TrainIndices);
            Assert.True(actual[fold].TestIndices.Count > 0, $"{name}: fold {fold} holds out nothing");
        }
    }

    private static int[] Integers(JsonElement element) => [.. element.EnumerateArray().Select(v => v.GetInt32())];
}
