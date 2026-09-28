namespace Lodestar.Text.Keywords;

/// <summary>One extracted phrase and the score that ranked it.</summary>
/// <param name="Phrase">The phrase, as the extractor assembled it.</param>
/// <param name="Score">Higher is better. The scale is the extractor's, and is not comparable across extractors.</param>
public readonly record struct KeywordMatch(string Phrase, double Score)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(Phrase);
            return (hash * 31) + ValueEquality.HashOf(Score);
        }
    }
}
