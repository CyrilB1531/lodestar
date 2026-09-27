namespace Lodestar.Extensions.VectorData;

/// <summary>The slots a query's keywords matched, in no particular order, with their BM25 scores.</summary>
internal readonly struct KeywordMatches(int[] slots, double[] scores)
{
    public int[] Slots { get; } = slots;

    public double[] Scores { get; } = scores;
}
