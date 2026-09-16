using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>The two sides of a fold, and the loop they are for.</summary>
internal static class FoldSplitSample
{
    public static void Run()
    {
        Console.WriteLine("FoldSplit (Lodestar.Preprocessing)");

        // Ten rows, five folds: each fold holds out two of them.
        IReadOnlyList<FoldSplit> folds = Splitters.KFold(10, foldCount: 5);

        foreach (FoldSplit fold in folds)
        {
            Console.WriteLine(
                $"  train / test     : [{string.Join(", ", fold.TrainIndices)}] / [{string.Join(", ", fold.TestIndices)}]");
        }

        Console.WriteLine();
    }
}
