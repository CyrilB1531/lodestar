using Lodestar.Text.Search;

namespace Lodestar.Sample;

/// <summary>Fusing two rankings without reconciling their scales.</summary>
internal static class RankFusionSample
{
    public static void Run()
    {
        Console.WriteLine("Reciprocal rank fusion (Lodestar.Text.Search)");

        // A keyword ranking and a vector one, disagreeing about the top document.
        int[] keyword = [2, 0, 1];
        int[] vector = [0, 2, 1];

        foreach (SearchHit hit in RankFusion.Rrf([keyword, vector]))
        {
            Console.WriteLine($"  doc {hit.Document} fused to {Inv.F5(hit.Score)}");
        }

        Console.WriteLine();
    }
}
