using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Grouped, time-ordered, repeated and stratified splits, with scikit-learn's own seed.</summary>
internal static class SeededSplittersSample
{
    public static void Run()
    {
        Console.WriteLine("Seeded and grouped splitters (Lodestar.Preprocessing)");

        int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];
        int[] groups = [1, 1, 1, 2, 2, 3, 4, 4, 4, 4, 5, 5];

        // The same random_state as scikit-learn gives the same folds: numpy's generator is replayed.
        Console.WriteLine($"  k-fold, seed 42  : [{string.Join(", ", Splitters.KFold(12, 3, randomState: 42)[0].TestIndices)}]");
        Console.WriteLine(
            $"  stratified, 42   : [{string.Join(", ", Splitters.StratifiedKFold(labels, 3, randomState: 42)[0].TestIndices)}]");
        Console.WriteLine(
            $"  train/test, 42   : [{string.Join(", ", Splitters.TrainTest(12, 0.25, randomState: 42).TestIndices)}]");
        Console.WriteLine(
            $"  stratified split : [{string.Join(", ", Splitters.StratifiedTrainTest(labels, 0.25, randomState: 42).TestIndices)}]");

        // A group never straddles two folds.
        Console.WriteLine($"  group k-fold     : [{string.Join(", ", Splitters.GroupKFold(groups, 2)[0].TestIndices)}]");
        Console.WriteLine($"  shuffled groups  : [{string.Join(", ", Splitters.GroupKFold(groups, 2, 0)[0].TestIndices)}]");
        Console.WriteLine(
            $"  stratified groups: [{string.Join(", ", Splitters.StratifiedGroupKFold(labels, groups, 2)[0].TestIndices)}]");
        Console.WriteLine(
            $"  … shuffled       : [{string.Join(", ", Splitters.StratifiedGroupKFold(labels, groups, 2, 0)[0].TestIndices)}]");

        // Forward chaining: every split trains only on rows before its test block.
        IReadOnlyList<FoldSplit> time = Splitters.TimeSeries(12, 3, gap: 1);
        Console.WriteLine($"  time series, last: train {time[2].TrainIndices.Count}, test [{string.Join(", ", time[2].TestIndices)}]");

        Console.WriteLine($"  repeated k-fold  : {Splitters.RepeatedKFold(12, 3, 2, 0).Count} folds");
        Console.WriteLine($"  repeated strat.  : {Splitters.RepeatedStratifiedKFold(labels, 3, 2, 0).Count} folds");
        Console.WriteLine();
    }
}
