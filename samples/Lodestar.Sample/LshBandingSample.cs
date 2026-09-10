using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>The band cut a threshold asks for, and the curve it produces.</summary>
internal static class LshBandingSample
{
    public static void Run()
    {
        Console.WriteLine("LshBanding (Lodestar.Text.Similarity)");

        LshBanding banding = LshBanding.Solve(threshold: 0.8, permutations: 128);
        Console.WriteLine($"  solved for 0.8   : {banding.Bands} bands of {banding.RowsPerBand}");
        Console.WriteLine($"  slots used       : {banding.Permutations} of 128");
        Console.WriteLine($"  P(collide | 0.9) : {Inv.F3(banding.CollisionProbability(0.9))}");
        Console.WriteLine($"  P(collide | 0.7) : {Inv.F3(banding.CollisionProbability(0.7))}");

        // Caring more about a missed pair than a wasted check asks for more bands.
        LshBanding recall = LshBanding.Solve(0.8, 128, falsePositiveWeight: 0.1, falseNegativeWeight: 0.9);
        Console.WriteLine($"  recall-weighted  : {recall.Bands} bands of {recall.RowsPerBand}");
        Console.WriteLine();
    }
}
