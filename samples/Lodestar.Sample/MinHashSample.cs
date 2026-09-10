using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>Jaccard estimated from signatures rather than from the sets.</summary>
internal static class MinHashSample
{
    public static void Run()
    {
        Console.WriteLine("MinHash (Lodestar.Text.Similarity)");

        var hasher = new MinHash(MinHashPermutationsSample.Four);
        uint[] fox = hasher.Signature(SimilarityCorpus.Fox);
        uint[] dog = hasher.Signature(SimilarityCorpus.Dog);
        uint[] other = hasher.Signature(SimilarityCorpus.Other);

        Console.WriteLine($"  signature length : {hasher.Length}");
        Console.WriteLine($"  fox vs dog       : {Inv.F3(MinHash.Jaccard(fox, dog))}");
        Console.WriteLine($"  fox vs other     : {Inv.F3(MinHash.Jaccard(fox, other))}");

        // A minimum is idempotent, so a repeated token cannot move a MinHash signature.
        uint[] repeated = hasher.Signature(["the", .. SimilarityCorpus.Fox]);
        Console.WriteLine($"  repeat changes it: {!fox.SequenceEqual(repeated)}");
        Console.WriteLine();
    }
}
