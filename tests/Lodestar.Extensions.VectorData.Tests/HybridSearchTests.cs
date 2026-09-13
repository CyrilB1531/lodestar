using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class HybridSearchTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    [Fact]
    public async Task The_fused_order_matches_neither_ranking_taken_alone()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            // Vector ranking alone (query [1,0,0]): a, b, c. Keyword ranking alone ("elephant"): c.
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
            Doc("c", "an elephant crossed the river", 0f, 0f, 1f),
        ]);

        // CA1859 asks for the concrete type here, which would delete the fact: what is under
        // test is that IKeywordHybridSearchable alone is enough to fuse a search with.
#pragma warning disable CA1859
        IKeywordHybridSearchable<Document> hybrid = collection;
#pragma warning restore CA1859
        List<VectorSearchResult<Document>> hits = await hybrid
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 2)
            .ToListAsync();

        // At k=60: fused(c) = 1/63 + 1/61 ≈ 0.03227, fused(a) = 1/61 ≈ 0.01639, fused(b) ≈
        // 0.01613 -- [c, a]. Vector alone gives [a, b]; keyword alone gives [c]; this is neither.
        Assert.Equal(2, hits.Count);
        Assert.Equal("c", hits[0].Record.Id);
        Assert.Equal("a", hits[1].Record.Id);
    }

    [Fact]
    public async Task A_keyword_outside_the_vocabulary_contributes_nothing_and_does_not_throw()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        // "a" is nearest the query vector but inserted LAST: a fusion that kept zero-scoring
        // keyword hits in insertion order would rank "b" first instead (task-6-report.md).
        await collection.UpsertAsync([
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
            Doc("c", "a fish swam in the sea", 0f, 0f, 1f),
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
        ]);

        // CA1859 asks for the concrete type here, which would delete the fact: what is under
        // test is that IKeywordHybridSearchable alone is enough to fuse a search with.
#pragma warning disable CA1859
        IKeywordHybridSearchable<Document> hybrid = collection;
#pragma warning restore CA1859
        List<VectorSearchResult<Document>> hits = await hybrid
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["zebra"], 3)
            .ToListAsync();

        Assert.Equal(3, hits.Count);
        Assert.Equal("a", hits[0].Record.Id);
    }

    [Fact]
    public async Task A_collection_with_no_full_text_property_refuses_hybrid_search_by_name()
    {
        using var collection = new LodestarVectorStoreCollection<int, VectorOnly>("vectors");
        await collection.UpsertAsync(new VectorOnly { Id = 1, Embedding = new ReadOnlyMemory<float>([1f, 0f]) });

        // CA1859 asks for the concrete type here, which would delete the fact: what is under
        // test is that IKeywordHybridSearchable alone is enough to fuse a search with.
#pragma warning disable CA1859
        IKeywordHybridSearchable<VectorOnly> hybrid = collection;
#pragma warning restore CA1859
        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await hybrid
                .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f]), ["anything"], 1)
                .ToListAsync());

        Assert.Contains("IsFullTextIndexed", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_filter_applies_to_the_fused_ranking_too()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "an elephant in the park", 0f, 1f, 0f),
            Doc("c", "an elephant crossed the river", 0f, 0f, 1f),
        ]);

        // CA1859 asks for the concrete type here, which would delete the fact: what is under
        // test is that IKeywordHybridSearchable alone is enough to fuse a search with.
#pragma warning disable CA1859
        IKeywordHybridSearchable<Document> hybrid = collection;
#pragma warning restore CA1859
        List<VectorSearchResult<Document>> hits = await hybrid.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            ["elephant"],
            3,
            new HybridSearchOptions<Document> { Filter = d => d.Id != "c" })
            .ToListAsync();

        Assert.DoesNotContain(hits, hit => hit.Record.Id == "c");
    }
}
