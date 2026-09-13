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

    [Fact]
    public async Task Upserting_the_same_key_replaces_rather_than_duplicates()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        await collection.UpsertAsync(Doc("a", "the dog ran", 0f, 1f, 0f));

        Document? found = await collection.GetAsync("a");

        Assert.Equal("the dog ran", found!.Text);
        Assert.Equal(1, collection.Current().Vectors.Count);
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
