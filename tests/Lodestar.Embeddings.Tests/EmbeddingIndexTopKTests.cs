using Lodestar.Embeddings.Search;
using Xunit;

namespace Lodestar.Embeddings.Tests;

// CA5394: a seeded Random builds a reproducible grid of vectors; no security use.
#pragma warning disable CA5394

/// <summary>The bounded-heap top-k returns exactly the leading hits of a full sort.</summary>
/// <remarks>
/// A small k takes the heap and a k past half the index takes the sort, so asking for every item
/// is the reference the smaller requests are held to. The vectors are quantised onto a coarse grid
/// so that many scores tie, which is where an ordering mistake would show.
/// </remarks>
public sealed class EmbeddingIndexTopKTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(99)]
    public void Every_k_returns_the_leading_hits_of_the_full_ranking(int seed)
    {
        const int dimension = 4;
        const int count = 200;
        var random = new Random(seed);
        var index = new EmbeddingIndex(dimension, normalize: false);
        var row = new float[dimension];
        for (int item = 0; item < count; item++)
        {
            for (int d = 0; d < dimension; d++)
            {
                row[d] = random.Next(-2, 3);
            }

            index.Add(row);
        }

        float[] query = [1, 1, 0, -1];
        IReadOnlyList<SearchResult> full = index.Search(query, count);

        for (int k = 1; k <= count; k++)
        {
            Assert.Equal(full.Take(k), index.Search(query, k));
        }
    }
}
