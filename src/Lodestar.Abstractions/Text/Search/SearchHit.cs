namespace Lodestar.Text.Search;

/// <summary>One document and the score that ranked it.</summary>
/// <param name="Document">The document's row index in the matrix that was scored.</param>
/// <param name="Score">Higher is better. The scale is BM25's and is not comparable across corpora.</param>
public sealed record SearchHit(int Document, double Score)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Document;
            return (hash * 31) + ValueEquality.HashOf(Score);
        }
    }
}
