using Lodestar.Extensions.VectorData;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>What a collection's keyword half and its fusion are configured with.</summary>
internal static class LodestarVectorStoreOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Vector store options (Lodestar.Extensions.VectorData)");

        var defaults = new LodestarVectorStoreOptions();
        Console.WriteLine($"  default fusion k  : {defaults.RankFusionK}");
        Console.WriteLine($"  default vectorizer: {defaults.Vectorizer is null}");
        Console.WriteLine($"  default bm25      : {defaults.Bm25 is null}");

        var custom = new LodestarVectorStoreOptions
        {
            RankFusionK = 30,
            Vectorizer = new CountVectorizerOptions { Lowercase = false },
            Bm25 = new Bm25Options { K1 = 1.5, B = 0.8 },
        };
        Console.WriteLine($"  custom fusion k   : {custom.RankFusionK}");
        Console.WriteLine($"  custom vectorizer : {custom.Vectorizer?.Lowercase}");
        Console.WriteLine($"  custom bm25       : k1={Inv.F1(custom.Bm25?.K1 ?? 0.0)}");

        // Zero would divide by zero at rank zero, so the setter refuses it rather than
        // letting a fused search fail lazily on the first result.
        try
        {
            _ = new LodestarVectorStoreOptions { RankFusionK = 0 };
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"  k <= 0 refused    : {ex.ParamName}");
        }

        Console.WriteLine();
    }
}
