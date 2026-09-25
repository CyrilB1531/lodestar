using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>sklearn.model_selection</c> over <c>tests/oracles/preprocessing_splitters_seeded.json</c> (#1157).</summary>
/// <remarks>Compared exactly, index for index, with scikit-learn's own <c>random_state</c> passed in (decision 0008).</remarks>
public sealed class SeededSplittersOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument corpus = OracleLoader.Load("preprocessing_splitters_seeded.json");
        int replayed = 0;

        foreach (JsonElement frozen in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = frozen.GetProperty("name").GetString()!;
            string call = frozen.GetProperty("call").GetString()!;
            if (call is "trainTest" or "stratifiedTrainTest")
            {
                AssertSplit(frozen, TrainTest(frozen, call));
            }
            else
            {
                AssertFolds(frozen, Folds(frozen, call), name);
            }

            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static TrainTestSplit TrainTest(JsonElement frozen, string call)
    {
        double fraction = frozen.GetProperty("testFraction").GetDouble();
        long seed = frozen.GetProperty("randomState").GetInt64();
        return call == "trainTest"
            ? Splitters.TrainTest(frozen.GetProperty("sampleCount").GetInt32(), fraction, seed)
            : Splitters.StratifiedTrainTest(Integers(frozen, "labels"), fraction, seed);
    }

    private static IReadOnlyList<FoldSplit> Folds(JsonElement frozen, string call)
    {
        long? seed = frozen.TryGetProperty("randomState", out JsonElement given) ? given.GetInt64() : null;
        int folds = frozen.TryGetProperty("foldCount", out JsonElement count) ? count.GetInt32() : 0;
        return call switch
        {
            "kfold" => Splitters.KFold(frozen.GetProperty("sampleCount").GetInt32(), folds, seed!.Value),
            "stratified" => Splitters.StratifiedKFold(Integers(frozen, "labels"), folds, seed!.Value),
            "repeatedKfold" => Splitters.RepeatedKFold(
                frozen.GetProperty("sampleCount").GetInt32(), folds, frozen.GetProperty("repeatCount").GetInt32(), seed!.Value),
            "repeatedStratified" => Splitters.RepeatedStratifiedKFold(
                Integers(frozen, "labels"), folds, frozen.GetProperty("repeatCount").GetInt32(), seed!.Value),
            "groupKfold" => seed is { } groupSeed
                ? Splitters.GroupKFold(Integers(frozen, "groups"), folds, groupSeed)
                : Splitters.GroupKFold(Integers(frozen, "groups"), folds),
            "stratifiedGroupKfold" => seed is { } stratifiedSeed
                ? Splitters.StratifiedGroupKFold(Integers(frozen, "labels"), Integers(frozen, "groups"), folds, stratifiedSeed)
                : Splitters.StratifiedGroupKFold(Integers(frozen, "labels"), Integers(frozen, "groups"), folds),
            _ => Splitters.TimeSeries(
                frozen.GetProperty("sampleCount").GetInt32(),
                frozen.GetProperty("splitCount").GetInt32(),
                OptionalInteger(frozen, "testSize"),
                frozen.GetProperty("gap").GetInt32(),
                OptionalInteger(frozen, "maxTrainSize")),
        };
    }

    private static void AssertSplit(JsonElement frozen, TrainTestSplit split)
    {
        Assert.Equal(Integers(frozen, "trainIndices"), split.TrainIndices);
        Assert.Equal(Integers(frozen, "testIndices"), split.TestIndices);
    }

    private static void AssertFolds(JsonElement frozen, IReadOnlyList<FoldSplit> actual, string name)
    {
        JsonElement[] expected = [.. frozen.GetProperty("folds").EnumerateArray()];

        Assert.True(expected.Length == actual.Count, $"{name}: {actual.Count} folds, expected {expected.Length}");
        for (int fold = 0; fold < expected.Length; fold++)
        {
            Assert.True(
                Integers(expected[fold], "testIndices").SequenceEqual(actual[fold].TestIndices),
                $"{name}: fold {fold} holds out other rows");
            Assert.True(
                Integers(expected[fold], "trainIndices").SequenceEqual(actual[fold].TrainIndices),
                $"{name}: fold {fold} trains on other rows");
        }
    }

    private static int? OptionalInteger(JsonElement frozen, string key) =>
        frozen.GetProperty(key).ValueKind == JsonValueKind.Null ? null : frozen.GetProperty(key).GetInt32();

    private static int[] Integers(JsonElement element, string key) =>
        [.. element.GetProperty(key).EnumerateArray().Select(v => v.GetInt32())];
}
