using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>A single held-out set, and the rounding that decides its size.</summary>
internal static class TrainTestSplitSample
{
    public static void Run()
    {
        Console.WriteLine("TrainTestSplit (Lodestar.Preprocessing)");

        // ceil(10 x 0.25) = 3, which is the reference's rounding rather than 2.
        TrainTestSplit split = Splitters.TrainTest(10, testFraction: 0.25);

        Console.WriteLine($"  train            : [{string.Join(", ", split.TrainIndices)}]");
        Console.WriteLine($"  test             : [{string.Join(", ", split.TestIndices)}]");
        Console.WriteLine();
    }
}
