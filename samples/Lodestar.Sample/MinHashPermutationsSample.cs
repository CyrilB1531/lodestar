using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>The coefficients a signature is built from, supplied rather than seeded.</summary>
internal static class MinHashPermutationsSample
{
    /// <summary>Four permutations, small enough to print.</summary>
    public static MinHashPermutations Four { get; } =
        new([3UL, 5UL, 7UL, 11UL], [13UL, 17UL, 19UL, 23UL]);

    public static void Run()
    {
        Console.WriteLine("MinHashPermutations (Lodestar.Text.Similarity)");
        Console.WriteLine($"  count            : {Four.Count}");
        Console.WriteLine($"  first a, b       : {Four.Multiplier(0)}, {Four.Addend(0)}");
        Console.WriteLine();
    }
}
