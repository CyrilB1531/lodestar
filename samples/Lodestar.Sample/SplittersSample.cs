using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Cutting cross-validation folds, with and without keeping each class's share.</summary>
internal static class SplittersSample
{
    public static void Run()
    {
        Console.WriteLine("Splitters (Lodestar.Preprocessing)");

        // Six rows of class 0, three of class 1, three of class 2.
        int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

        IReadOnlyList<FoldSplit> plain = Splitters.KFold(labels.Length, foldCount: 3);
        Console.WriteLine($"  k-fold, fold 0   : [{string.Join(", ", plain[0].TestIndices)}]");

        // Stratifying is what puts a row of every class in every fold: the plain
        // fold above is four rows of class 0 and nothing else.
        IReadOnlyList<FoldSplit> stratified = Splitters.StratifiedKFold(labels, foldCount: 3);
        Console.WriteLine($"  stratified, 0    : [{string.Join(", ", stratified[0].TestIndices)}]");
        Console.WriteLine($"  stratified, 1    : [{string.Join(", ", stratified[1].TestIndices)}]");

        // The permutation is an argument rather than a seed, so the same one gives
        // the same folds here and in scikit-learn.
        int[] order = [9, 4, 1, 7, 0, 3, 6, 8, 2, 5, 11, 10];
        Console.WriteLine($"  read in order    : [{string.Join(", ", Splitters.KFold(12, 3, order)[0].TestIndices)}]");
        Console.WriteLine(
            $"  stratified       : [{string.Join(", ", Splitters.StratifiedKFold(labels, 3, order)[0].TestIndices)}]");

        TrainTestSplit split = Splitters.TrainTest(labels.Length, testFraction: 0.25);
        Console.WriteLine($"  train / test     : {split.TrainIndices.Count} / {split.TestIndices.Count} rows");
        Console.WriteLine($"  permuted test    : [{string.Join(", ", Splitters.TrainTest(12, 0.25, order).TestIndices)}]");
        Console.WriteLine();
    }
}
