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
        // keyword hits in insertion order would rank "b" first instead.
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

    // Vector ranking for [1,0,0]: a, b, c (b and c tie at zero, index order). Keyword ranking for
    // "elephant": b, c. At k=60 that fuses to b (1/62 + 1/61), c (1/63 + 1/62), a (1/61).
    private static async Task<LodestarVectorStoreCollection<string, Document>> TwoElephants()
    {
        var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "an elephant in the park", 0f, 1f, 0f),
            Doc("c", "an elephant crossed the river", 0f, 0f, 1f),
        ]);
        return collection;
    }

    [Fact]
    public async Task A_filter_applies_to_the_fused_ranking_too()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await TwoElephants();

        List<VectorSearchResult<Document>> hits = await collection.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            ["elephant"],
            3,
            new HybridSearchOptions<Document> { Filter = d => d.Id != "c" })
            .ToListAsync();

        Assert.Equal(["b", "a"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task Skip_drops_the_leading_fused_results()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await TwoElephants();

        List<VectorSearchResult<Document>> hits = await collection.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            ["elephant"],
            2,
            new HybridSearchOptions<Document> { Skip = 1 })
            .ToListAsync();

        Assert.Equal(["c", "a"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_score_threshold_is_refused_with_the_reason()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await TwoElephants();

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(async () =>
            await collection.HybridSearchAsync(
                new ReadOnlyMemory<float>([1f, 0f, 0f]),
                ["elephant"],
                3,
                new HybridSearchOptions<Document> { ScoreThreshold = 0.01 })
                .ToListAsync());

        Assert.Contains("ScoreThreshold", error.Message, StringComparison.Ordinal);
        Assert.Contains("not a similarity", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_matched_record_scoring_zero_stays_in_the_keyword_ranking()
    {
        // Two records, one holding "apple": Robertson's IDF is log(1.5 / 1.5) = 0, so the match
        // scores zero. Vector order is b, a; keeping a as the keyword hit fuses a (1/62 + 1/61) first.
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "red apple", 0f, 1f, 0f),
            Doc("b", "green pear", 1f, 0f, 0f),
        ]);

        List<VectorSearchResult<Document>> hits = await collection
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["apple"], 2)
            .ToListAsync();

        Assert.Equal(["a", "b"], hits.Select(hit => hit.Record.Id));
        Assert.Equal((1.0 / 62) + (1.0 / 61), hits[0].Score!.Value, 12);
    }

    [Fact]
    public async Task A_matched_record_scoring_below_zero_stays_in_the_keyword_ranking()
    {
        // One record: every IDF is log(0.5 / 1.5) < 0, so the floor, a share of their mean, is too.
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync(Doc("a", "red apple", 0f, 1f, 0f));

        List<VectorSearchResult<Document>> hits = await collection
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["apple"], 1)
            .ToListAsync();

        Assert.Equal(2.0 / 61, Assert.Single(hits).Score!.Value, 12);
    }

    [Fact]
    public async Task Texts_that_yield_no_tokens_degrade_to_the_vector_ranking()
    {
        // Single letters are dropped, so the vocabulary is empty. Vector order is c, a, b; keeping the
        // keyword half's zero-scoring ties would reward insertion order a, b, c and put a first.
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "a", 1f, 0f, 0f),
            Doc("b", "I", 0f, 1f, 0f),
            Doc("c", "x y", 0f, 0f, 1f),
        ]);

        List<VectorSearchResult<Document>> hits = await collection
            .HybridSearchAsync(new ReadOnlyMemory<float>([0.2f, 0.1f, 1f]), ["x"], 3)
            .ToListAsync();

        Assert.Equal(["c", "a", "b"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_delete_while_a_hybrid_search_is_enumerated_changes_nothing_already_answered()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await TwoElephants();
        var seen = new List<string>();

        await foreach (VectorSearchResult<Document> hit in collection.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 3))
        {
            seen.Add(hit.Record.Id);
            await collection.DeleteAsync(["a", "c"]);
        }

        Assert.Equal(["b", "c", "a"], seen);
    }

    [Fact]
    public async Task An_empty_collection_refuses_a_hybrid_query_of_the_wrong_width()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await collection.HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f]), ["elephant"], 1).ToListAsync());

        Assert.Contains("query length 2 != dimension 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task An_empty_collection_of_a_full_text_record_returns_nothing_rather_than_refusing()
    {
        // Document marks Text as full-text indexed, but no record has been written, so the
        // rebuild builds no keyword index. That is an empty collection, not a missing property.
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");

        List<VectorSearchResult<Document>> hits = await collection
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 3)
            .ToListAsync();

        Assert.Empty(hits);
    }
}
