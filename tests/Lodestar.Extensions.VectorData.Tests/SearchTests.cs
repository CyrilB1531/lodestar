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
    public async Task An_empty_collection_refuses_a_query_of_the_wrong_width()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await collection.SearchAsync(new ReadOnlyMemory<float>([1f, 0f]), 1).ToListAsync());

        Assert.Contains("query length 2 != dimension 3", error.Message, StringComparison.Ordinal);
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

    // Three non-matches outrank the two matches and top is 2, so a post-filter returns nothing. m2 is
    // inserted before m1 and scores below it, so insertion order and score order disagree.
    [Fact]
    public async Task Records_ranked_ahead_of_the_matches_do_not_starve_the_filtered_search()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("n1", "a quiet morning by the lake", 1f, 0f, 0f),
            Doc("n2", "a train crossing the bridge", 0.95f, 0.05f, 0f),
            Doc("n3", "an old clock on the wall", 0.9f, 0.1f, 0f),
            Doc("m2", "a bench near the park entrance", 0f, 0.1f, 0.9f),
            Doc("m1", "children playing in the park", 0.1f, 0.9f, 0f),
        ]);

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            2,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Contains("park") })
            .ToListAsync();

        Assert.Equal(["m1", "m2"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_score_threshold_drops_the_records_below_it()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0.6f, 0.8f, 0f),
            Doc("c", "a bird flew over the park", 0f, 1f, 0f),
        ]);

        // Cosine similarities to [1,0,0]: a = 1, b = 0.6, c = 0. A threshold of 0.5 keeps two of three.
        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            3,
            new VectorSearchOptions<Document> { ScoreThreshold = 0.5 })
            .ToListAsync();

        Assert.Equal(["a", "b"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_skip_near_the_largest_int_returns_nothing_rather_than_overflowing()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Skip = int.MaxValue })
            .ToListAsync();

        Assert.Empty(hits);
    }

    [Fact]
    public async Task A_collection_with_no_full_text_property_serves_vector_search()
    {
        using var collection = new LodestarVectorStoreCollection<int, VectorOnly>("vectors");
        await collection.UpsertAsync([
            new VectorOnly { Id = 1, Embedding = new ReadOnlyMemory<float>([1f, 0f]) },
            new VectorOnly { Id = 2, Embedding = new ReadOnlyMemory<float>([0f, 1f]) },
        ]);

        List<VectorSearchResult<VectorOnly>> hits = await collection
            .SearchAsync(new ReadOnlyMemory<float>([0.1f, 1f]), 2).ToListAsync();

        Assert.Equal([2, 1], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_delete_while_a_search_is_enumerated_changes_nothing_already_answered()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();
        var seen = new List<string>();

        await foreach (VectorSearchResult<Document> hit in collection.SearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), 3))
        {
            seen.Add(hit.Record.Id);
            await collection.DeleteAsync(["b", "c"]);
        }

        Assert.Equal("a", seen[0]);
        Assert.Equal(["a", "b", "c"], seen.Order(StringComparer.Ordinal));
    }

    [Fact]
    public async Task A_replace_while_a_search_is_enumerated_keeps_each_score_with_its_record()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();
        var seen = new List<VectorSearchResult<Document>>();

        await foreach (VectorSearchResult<Document> hit in collection.SearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), 3))
        {
            seen.Add(hit);
            await collection.UpsertAsync(Doc("b", "the dog left", 1f, 0f, 0f));
        }

        VectorSearchResult<Document> b = Assert.Single(seen, hit => hit.Record.Id == "b");
        Assert.Equal("the dog ran in the park", b.Record.Text);
    }

    [Fact]
    public async Task An_upsert_while_a_filtered_read_is_enumerated_does_not_throw()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();
        var seen = new List<string>();

        await foreach (Document found in collection.GetAsync(d => d.Text.Contains("park"), 5))
        {
            seen.Add(found.Id);
            await collection.UpsertAsync(Doc("new-" + found.Id, "another park", 0f, 1f, 0f));
        }

        Assert.Equal(["b", "c"], seen.Order(StringComparer.Ordinal));
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

    // The filtered path scores only admitted records and keeps a bounded heap; the unfiltered path
    // ranks everything through EmbeddingIndex.Search. Filtering the latter must give the same hits and bits.
    [Fact]
    public async Task A_filtered_search_ranks_as_filtering_the_whole_ranking_does()
    {
        // S2245 / CA5394: seeded, so a failure reproduces; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(682);
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        string[] words = ["park", "lake", "clock"];
        await collection.UpsertAsync([.. Enumerable.Range(0, 400).Select(i => Doc(
            $"d{i}",
            words[random.Next(words.Length)],
            random.Next(-1, 2), random.Next(-1, 2), (float)random.NextDouble()))]);

        for (int trial = 0; trial < 40; trial++)
        {
            float[] query = [(float)random.NextDouble() - 0.5f, random.Next(-1, 2), 0.25f];
            int top = random.Next(1, 30);
            int skip = random.Next(0, 20);
            float? threshold = random.Next(3) == 0 ? (float)random.NextDouble() - 0.5f : null;
#pragma warning restore S2245, CA5394
            List<VectorSearchResult<Document>> whole = await collection
                .SearchAsync(new ReadOnlyMemory<float>(query), 400).ToListAsync();
            string[] expected = [.. whole
                .Where(hit => hit.Record.Text == "park" && (threshold is null || hit.Score >= threshold))
                .Skip(skip).Take(top)
                .Select(hit => $"{hit.Record.Id}:{BitConverter.DoubleToInt64Bits(hit.Score!.Value)}")];

            List<VectorSearchResult<Document>> filtered = await collection.SearchAsync(
                new ReadOnlyMemory<float>(query),
                top,
                new VectorSearchOptions<Document> { Filter = d => d.Text == "park", Skip = skip, ScoreThreshold = threshold })
                .ToListAsync();

            Assert.Equal(expected, filtered.Select(hit => $"{hit.Record.Id}:{BitConverter.DoubleToInt64Bits(hit.Score!.Value)}"));
        }
    }

    // The filter runs once on every record, in index order, before any record is scored: the count
    // is what a caller with a side-effecting or expensive filter observes.
    [Fact]
    public async Task A_filtered_search_runs_the_filter_once_on_every_record()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();
        var seen = new List<string>();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([0f, 0f, 1f]),
            1,
            new VectorSearchOptions<Document> { Filter = d => Seen(seen, d) })
            .ToListAsync();

        Assert.Equal("c", Assert.Single(hits).Record.Id);
        Assert.Equal(["a", "b", "c"], seen);
    }

    /// <summary>Records the record a filter was called on, and admits it.</summary>
    private static bool Seen(List<string> seen, Document record)
    {
        seen.Add(record.Id);
        return true;
    }

    [Fact]
    public async Task A_filtered_search_over_an_empty_collection_returns_nothing()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Length > 0 })
            .ToListAsync();

        Assert.Empty(hits);
    }

    [Fact]
    public async Task A_filtered_search_refuses_a_query_of_another_width()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(async () => await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Length > 0 })
            .ToListAsync());

        Assert.Contains("dimension", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_filtered_search_with_a_zero_query_scores_every_admitted_record_zero()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([0f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Contains("park") })
            .ToListAsync();

        Assert.Equal(["b", "c"], hits.Select(hit => hit.Record.Id));
        Assert.All(hits, hit => Assert.Equal(0d, hit.Score));
    }
}
