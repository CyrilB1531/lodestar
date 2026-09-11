using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>The coefficients a signature is built from, supplied rather than seeded.</summary>
internal static class MinHashPermutationsSample
{
    /// <summary>Four permutations, small enough to print.</summary>
    public static MinHashPermutations Four { get; } =
        new([3UL, 5UL, 7UL, 11UL], [13UL, 17UL, 19UL, 23UL]);

    /// <summary>The same four under the family the reference now defaults to.</summary>
    /// <remarks>
    /// Every multiplier here is already odd and inside 32 bits, which is what
    /// <see cref="MinHashScheme.Affine32"/> requires of one — the constructor refuses a pair it
    /// cannot read rather than computing a signature from it.
    /// </remarks>
    public static MinHashPermutations FourAffine32 { get; } =
        new([3UL, 5UL, 7UL, 11UL], [13UL, 17UL, 19UL, 23UL], MinHashScheme.Affine32);

    public static void Run()
    {
        Console.WriteLine("MinHashPermutations (Lodestar.Text.Similarity)");
        Console.WriteLine($"  count            : {Four.Count}");
        Console.WriteLine($"  first a, b       : {Four.Multiplier(0)}, {Four.Addend(0)}");
        Console.WriteLine($"  scheme           : {Four.Scheme}");
        Console.WriteLine($"  the other family : {FourAffine32.Scheme}");

        string[] tokens = ["the", "quick", "brown", "fox"];
        uint[] legacy = new MinHash(Four).Signature(tokens);
        uint[] affine = new MinHash(FourAffine32).Signature(tokens);
        Console.WriteLine($"  agree on slot 0  : {legacy[0] == affine[0]}");
        Console.WriteLine();
    }
}
