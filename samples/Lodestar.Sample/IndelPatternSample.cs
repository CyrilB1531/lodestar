using Lodestar.Text.Distances;

namespace Lodestar.Sample;

/// <summary>One query against a list, with the pattern's bit-parallel table built once.</summary>
internal static class IndelPatternSample
{
    public static void Run()
    {
        string[] candidates = ["sitting", "mitten", "bitten", "kitten", "knitting"];

        // Disposing returns the table to the pool; a handle is meant to live for the batch.
        using IndelPattern query = IndelPattern.For("kitten");

        foreach (string candidate in candidates)
        {
            Console.WriteLine($"  kitten vs {candidate,-9}= {query.Distance(candidate.AsSpan())} edits, "
                + $"{Inv.F4(query.NormalizedSimilarity(candidate.AsSpan()))} similar, "
                + $"{Inv.F4(query.NormalizedDistance(candidate.AsSpan()))} apart");
        }

        // The answers are Indel's, which is the whole point: only the rebuilding changes.
        Console.WriteLine($"  agrees with the pairwise call       = "
            + $"{query.Distance("sitting".AsSpan()) == Indel.Distance("kitten", "sitting")}");
        Console.WriteLine();
    }
}
