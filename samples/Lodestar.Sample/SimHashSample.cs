using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>One 64-bit fingerprint per document, compared by Hamming distance.</summary>
internal static class SimHashSample
{
    public static void Run()
    {
        Console.WriteLine("SimHash (Lodestar.Text.Similarity)");

        ulong fox = SimHash.Fingerprint(SimilarityCorpus.Fox);
        ulong dog = SimHash.Fingerprint(SimilarityCorpus.Dog);
        ulong other = SimHash.Fingerprint(SimilarityCorpus.Other);

        Console.WriteLine($"  bits             : {SimHash.Bits}");
        Console.WriteLine($"  fox vs dog       : {SimHash.HammingDistance(fox, dog)} bits apart");
        Console.WriteLine($"  fox vs other     : {SimHash.HammingDistance(fox, other)} bits apart");

        // Weights are summed, so unlike MinHash this does see a repeated token.
        ulong repeated = SimHash.Fingerprint(["the", .. SimilarityCorpus.Fox]);
        Console.WriteLine($"  repeat changes it: {fox != repeated}");
        Console.WriteLine();
    }
}
