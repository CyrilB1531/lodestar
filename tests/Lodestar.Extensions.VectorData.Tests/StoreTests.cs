using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class StoreTests
{
    [Fact]
    public void The_same_name_returns_the_same_collection()
    {
        using var store = new LodestarVectorStore();

        VectorStoreCollection<string, Document> first = store.GetCollection<string, Document>("documents");
        VectorStoreCollection<string, Document> second = store.GetCollection<string, Document>("documents");

        Assert.Same(first, second);
    }

    [Fact]
    public async Task A_collection_appears_in_the_listing_once_it_exists()
    {
        using var store = new LodestarVectorStore();
        VectorStoreCollection<string, Document> collection = store.GetCollection<string, Document>("documents");

        Assert.Empty(await store.ListCollectionNamesAsync().ToListAsync());

        await collection.EnsureCollectionExistsAsync();

        Assert.Equal(["documents"], await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public async Task The_store_reports_whether_a_collection_exists()
    {
        using var store = new LodestarVectorStore();

        // Requested but never ensured: CollectionExistsAsync must read the collection's own
        // Exists flag, not merely whether the store has ever handed one out under this name.
        store.GetCollection<string, Document>("requested-only");
        Assert.False(await store.CollectionExistsAsync("requested-only"));

        await store.GetCollection<string, Document>("documents").EnsureCollectionExistsAsync();

        Assert.True(await store.CollectionExistsAsync("documents"));
        Assert.False(await store.CollectionExistsAsync("absent"));
    }

    [Fact]
    public async Task Ensuring_a_collection_deleted_removes_it_from_the_listing()
    {
        using var store = new LodestarVectorStore();
        await store.GetCollection<string, Document>("documents").EnsureCollectionExistsAsync();

        await store.EnsureCollectionDeletedAsync("documents");

        Assert.Empty(await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public void A_dynamic_collection_is_refused_with_its_reason()
    {
        using var store = new LodestarVectorStore();
        var definition = new VectorStoreCollectionDefinition
        {
            Properties = [new VectorStoreKeyProperty("Id", typeof(string))],
        };

        NotSupportedException error = Assert.Throws<NotSupportedException>(
            () => store.GetDynamicCollection("documents", definition));

        Assert.Contains("typed", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Asking_for_one_name_under_two_record_types_is_refused()
    {
        using var store = new LodestarVectorStore();
        store.GetCollection<string, Document>("documents");

        Assert.Throws<ArgumentException>(() => store.GetCollection<int, VectorOnly>("documents"));
    }
}
