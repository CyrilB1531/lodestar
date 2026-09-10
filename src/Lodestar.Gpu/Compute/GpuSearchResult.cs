namespace Lodestar.Gpu.Compute;

/// <summary>One hit from a device sweep: the row's index and its cosine similarity.</summary>
/// <param name="Index">The row of the device matrix that matched.</param>
/// <param name="Score">Cosine similarity, higher is better.</param>
/// <remarks>
/// Deliberately the same shape as <c>Lodestar.Embeddings.Search.SearchResult</c> without
/// being it: decision 0101 forbids an edge from a core package to this one, and an edge
/// the other way would floor this package on a sibling for one struct.
/// </remarks>
public readonly record struct GpuSearchResult(int Index, float Score);
