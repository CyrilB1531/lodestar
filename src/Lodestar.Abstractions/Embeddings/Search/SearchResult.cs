namespace Lodestar.Embeddings.Search;

/// <summary>A single search hit: the item's index and its similarity score.</summary>
public readonly record struct SearchResult(int Index, float Score);
