using Lodestar.Text.Similarity;

namespace Lodestar.Sample;

/// <summary>Candidates from a banded index, then scored to become matches.</summary>
internal static class LshIndexSample
{
    public static void Run()
    {
        Console.WriteLine("LshIndex (Lodestar.Text.Similarity)");

        var hasher = new MinHash(MinHashPermutationsSample.Four);
        var index = new LshIndex(new LshBanding(2, 2));
        index.Add("fox", hasher.Signature(SimilarityCorpus.Fox));
        index.Add("other", hasher.Signature(SimilarityCorpus.Other));

        uint[] query = hasher.Signature(SimilarityCorpus.Dog);
        IReadOnlyList<string> candidates = index.Query(query);
        Console.WriteLine($"  banding          : {index.Banding.Bands} x {index.Banding.RowsPerBand}");
        Console.WriteLine($"  held             : {index.Count}");
        Console.WriteLine($"  candidates       : {candidates.Count}");

        // A candidate is not a match: scoring it is the caller's step.
        foreach (string key in candidates)
        {
            string[] tokens = key == "fox" ? SimilarityCorpus.Fox : SimilarityCorpus.Other;
            double score = MinHash.Jaccard(query, hasher.Signature(tokens));
            Console.WriteLine($"    {key} scored {Inv.F3(score)}");
        }

        Console.WriteLine();
    }
}
