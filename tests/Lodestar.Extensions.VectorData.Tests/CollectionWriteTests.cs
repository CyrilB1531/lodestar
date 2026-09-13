using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class CollectionWriteTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    private static LodestarVectorStoreCollection<string, Document> Collection() => new("documents");

    [Fact]
    public async Task An_upserted_record_comes_back_by_key()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));

        Document? found = await collection.GetAsync("a");

        Assert.NotNull(found);
        Assert.Equal("the cat sat", found.Text);
    }

    private static readonly ReadOnlyMemory<float> TowardA = new([1f, 0f, 0f]);

    private static Task<List<VectorSearchResult<Document>>> Nearest(
        LodestarVectorStoreCollection<string, Document> collection, ReadOnlyMemory<float> query, int top) =>
        collection.SearchAsync(query, top).ToListAsync().AsTask();

    // Each write below follows a search, so the caches are already built when the write
    // lands: an implementation that forgets to mark them stale answers from the old state.
    [Fact]
    public async Task Upserting_the_same_key_replaces_rather_than_duplicates()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        await collection.UpsertAsync(Doc("b", "the dog ran", 0f, 1f, 0f));
        await Nearest(collection, TowardA, 2);

        await collection.UpsertAsync(Doc("a", "the bird flew", 0f, 0f, 1f));

        List<VectorSearchResult<Document>> towardOld = await Nearest(collection, TowardA, 2);
        Assert.Equal(2, towardOld.Count);
        Assert.All(towardOld, hit => Assert.True(hit.Score < 0.5, $"{hit.Record.Id} still scores {hit.Score}"));

        VectorSearchResult<Document> towardNew = Assert.Single(
            await Nearest(collection, new ReadOnlyMemory<float>([0f, 0f, 1f]), 1));
        Assert.Equal("a", towardNew.Record.Id);
        Assert.Equal("the bird flew", towardNew.Record.Text);
        Assert.Equal(1d, towardNew.Score!.Value, 6);
    }

    [Fact]
    public async Task A_batch_upsert_after_a_search_is_what_the_next_search_sees()
    {
        using var collection = Collection();
        await collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("b", "the dog ran", 0f, 1f, 0f)]);
        await Nearest(collection, TowardA, 2);

        await collection.UpsertAsync([Doc("a", "the cat left", 0f, 1f, 0f), Doc("c", "the bird flew", 0f, 0f, 1f)]);

        List<VectorSearchResult<Document>> hits = await Nearest(collection, TowardA, 3);
        Assert.Equal(3, hits.Count);
        Assert.All(hits, hit => Assert.True(hit.Score < 0.5, $"{hit.Record.Id} still scores {hit.Score}"));
    }

    [Fact]
    public async Task A_delete_after_a_search_is_gone_from_the_next_search()
    {
        using var collection = Collection();
        await collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("b", "the dog ran", 0f, 1f, 0f)]);
        await Nearest(collection, TowardA, 2);

        await collection.DeleteAsync("a");

        VectorSearchResult<Document> left = Assert.Single(await Nearest(collection, TowardA, 2));
        Assert.Equal("b", left.Record.Id);
    }

    [Fact]
    public async Task A_batch_delete_after_a_search_is_gone_from_the_next_search()
    {
        using var collection = Collection();
        await collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("b", "the dog ran", 0f, 1f, 0f)]);
        await Nearest(collection, TowardA, 2);

        await collection.DeleteAsync(["a", "never-written"]);

        VectorSearchResult<Document> left = Assert.Single(await Nearest(collection, TowardA, 2));
        Assert.Equal("b", left.Record.Id);
    }

    [Fact]
    public async Task Deleting_the_collection_after_a_search_empties_the_next_search()
    {
        using var collection = Collection();
        await collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("b", "the dog ran", 0f, 1f, 0f)]);
        await Nearest(collection, TowardA, 2);

        await collection.EnsureCollectionDeletedAsync();

        Assert.Empty(await Nearest(collection, TowardA, 2));
    }

    [Fact]
    public async Task A_batch_delete_holding_a_null_key_removes_nothing()
    {
        using var collection = Collection();
        await collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("b", "the dog ran", 0f, 1f, 0f)]);
        await Nearest(collection, TowardA, 2);

        await Assert.ThrowsAsync<ArgumentNullException>(() => collection.DeleteAsync(["a", null!]));

        Assert.NotNull(await collection.GetAsync("a"));
        List<VectorSearchResult<Document>> hits = await Nearest(collection, TowardA, 2);
        Assert.Equal(["a", "b"], hits.Select(hit => hit.Record.Id));
    }

    [Fact]
    public async Task A_batch_upsert_holding_a_null_record_writes_nothing()
    {
        using var collection = Collection();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), null!]));

        Assert.Null(await collection.GetAsync("a"));
        Assert.False(await collection.CollectionExistsAsync());
    }

    [Fact]
    public async Task A_vector_of_the_wrong_width_is_refused_at_write_by_key_and_width()
    {
        using var collection = Collection();

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
            () => collection.UpsertAsync(Doc("short", "the cat sat", 1f, 0f)));

        Assert.Contains("short", error.Message, StringComparison.Ordinal);
        Assert.Contains("of 2 where this collection is 3 wide", error.Message, StringComparison.Ordinal);
        Assert.Null(await collection.GetAsync("short"));
    }

    [Fact]
    public async Task A_batch_holding_one_wrong_width_vector_writes_none_of_it()
    {
        using var collection = Collection();

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
            () => collection.UpsertAsync([Doc("a", "the cat sat", 1f, 0f, 0f), Doc("short", "the dog ran", 0f, 1f)]));

        Assert.Contains("short", error.Message, StringComparison.Ordinal);
        Assert.Null(await collection.GetAsync("a"));
        Assert.Empty(await Nearest(collection, TowardA, 2));
    }

    [Fact]
    public async Task A_deleted_record_leaves_neither_a_record_nor_a_vector()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        await collection.UpsertAsync(Doc("b", "the dog ran", 0f, 1f, 0f));

        await collection.DeleteAsync("a");

        Assert.Null(await collection.GetAsync("a"));
        Assert.Equal(1, collection.Current().Vectors.Count);
        Assert.Equal(["b"], collection.Current().Keys);
    }

    [Fact]
    public async Task Deleting_a_key_that_is_not_there_is_not_an_error()
    {
        using var collection = Collection();

        await collection.DeleteAsync("missing");

        Assert.Equal(0, collection.Current().Vectors.Count);
    }

    [Fact]
    public async Task A_batch_of_writes_rebuilds_once()
    {
        using var collection = Collection();
        await collection.UpsertAsync([
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
            Doc("c", "the bird flew", 0f, 0f, 1f),
        ]);

        collection.Current();
        collection.Current();

        Assert.Equal(1, collection.RebuildCount);
    }

    [Fact]
    public async Task A_write_after_a_read_rebuilds_again()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        collection.Current();

        await collection.UpsertAsync(Doc("b", "the dog ran", 0f, 1f, 0f));
        collection.Current();

        Assert.Equal(2, collection.RebuildCount);
    }

    [Fact]
    public async Task The_collection_exists_once_it_has_been_ensured()
    {
        using var collection = Collection();

        Assert.False(await collection.CollectionExistsAsync());
        await collection.EnsureCollectionExistsAsync();
        Assert.True(await collection.CollectionExistsAsync());

        await collection.EnsureCollectionDeletedAsync();
        Assert.False(await collection.CollectionExistsAsync());
    }

    [Fact]
    public async Task Deleting_the_collection_drops_its_records()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));

        await collection.EnsureCollectionDeletedAsync();

        Assert.Null(await collection.GetAsync("a"));
    }

    [Fact]
    public void The_collection_reports_the_name_it_was_given()
    {
        using var collection = Collection();

        Assert.Equal("documents", collection.Name);
    }
}
