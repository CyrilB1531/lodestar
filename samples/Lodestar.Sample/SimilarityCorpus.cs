namespace Lodestar.Sample;

/// <summary>One corpus the sketch samples share, so the numbers line up across them.</summary>
internal static class SimilarityCorpus
{
    /// <summary>A sentence, and one that differs from it by a single word.</summary>
    public static readonly string[] Fox = ["the", "quick", "brown", "fox"];

    /// <summary>The same sentence with its last word changed.</summary>
    public static readonly string[] Dog = ["the", "quick", "brown", "dog"];

    /// <summary>Nothing in common with either.</summary>
    public static readonly string[] Other = ["entirely", "different", "words", "here"];
}
