namespace Lodestar.Embeddings.Search;

/// <summary>A single search hit: the item's index and its similarity score.</summary>
public readonly record struct SearchResult(int Index, float Score)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1294).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            return (((17 * 31) + Index) * 31) + ValueEquality.HashOf(Score);
        }
    }
}
