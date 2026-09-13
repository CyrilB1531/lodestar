using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class SearchTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    private static async Task<LodestarVectorStoreCollection<string, Document>> Seeded()
    {
        var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
            Doc("c", "a bird flew over the park", 0f, 0f, 1f),
        ]);
        return collection;
    }

    [Fact]
    public async Task The_nearest_vector_comes_first()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection
            .SearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), 2).ToListAsync();

        Assert.Equal(2, hits.Count);
        Assert.Equal("a", hits[0].Record.Id);
    }

    // Three non-matches outrank the two matches, and top (2) is fewer than that. A post-filter
    // implementation fetches only the non-matching three and returns nothing; the exact one scores everything first.
    [Fact]
    public async Task Records_ranked_ahead_of_the_matches_do_not_starve_the_filtered_search()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("n1", "a quiet morning by the lake", 1f, 0f, 0f),
            Doc("n2", "a train crossing the bridge", 0.95f, 0.05f, 0f),
            Doc("n3", "an old clock on the wall", 0.9f, 0.1f, 0f),
            Doc("m1", "children playing in the park", 0.1f, 0.9f, 0f),
            Doc("m2", "a bench near the park entrance", 0f, 0.1f, 0.9f),
        ]);

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            2,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Contains("park") })
            .ToListAsync();

        Assert.Equal(2, hits.Count);
        Assert.All(hits, hit => Assert.Contains("park", hit.Record.Text, StringComparison.Ordinal));
    }

    [Fact]
    public async Task A_filter_that_admits_nothing_returns_nothing()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Id == "absent" })
            .ToListAsync();

        Assert.Empty(hits);
    }

    [Fact]
    public async Task Skip_drops_the_leading_hits_rather_than_the_trailing_ones()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            1,
            new VectorSearchOptions<Document> { Skip = 1 })
            .ToListAsync();

        VectorSearchResult<Document> hit = Assert.Single(hits);
        Assert.NotEqual("a", hit.Record.Id);
    }

    [Fact]
    public async Task A_string_search_value_is_refused_with_the_reason()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync("the cat", 2).ToListAsync());

        Assert.Contains("vector", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Filtered_retrieval_returns_the_matching_records()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<Document> found = await collection.GetAsync(d => d.Text.Contains("park"), 5).ToListAsync();

        Assert.Equal(2, found.Count);
    }

    [Fact]
    public async Task Skip_drops_the_leading_matching_records_from_filtered_retrieval()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("m1", "a walk in the park", 1f, 0f, 0f),
            Doc("m2", "a bench in the park", 0f, 1f, 0f),
            Doc("m3", "trees in the park", 0f, 0f, 1f),
        ]);

        List<Document> found = await collection.GetAsync(
            d => d.Text.Contains("park"),
            2,
            new FilteredRecordRetrievalOptions<Document> { Skip = 1 })
            .ToListAsync();

        Assert.Equal(2, found.Count);
        Assert.DoesNotContain(found, d => d.Id == "m1");
    }

    [Fact]
    public async Task Filtered_retrieval_refuses_an_order_by()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(async () =>
            await collection.GetAsync(
                d => d.Text.Contains("park"),
                5,
                new FilteredRecordRetrievalOptions<Document> { OrderBy = o => o.Ascending(d => d.Id) })
                .ToListAsync());

        Assert.Contains("order", error.Message, StringComparison.OrdinalIgnoreCase);
    }
}
