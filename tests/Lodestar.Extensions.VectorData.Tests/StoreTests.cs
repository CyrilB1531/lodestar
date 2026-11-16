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
    public async Task A_deleted_collection_stays_retrievable_by_name_without_being_listed()
    {
        using var store = new LodestarVectorStore();
        VectorStoreCollection<string, Document> first = store.GetCollection<string, Document>("documents");
        await first.EnsureCollectionExistsAsync();

        await store.EnsureCollectionDeletedAsync("documents");
        VectorStoreCollection<string, Document> again = store.GetCollection<string, Document>("documents");

        Assert.Same(first, again);
        Assert.Empty(await store.ListCollectionNamesAsync().ToListAsync());

        await again.UpsertAsync(new Document { Id = "a", Text = "a note", Embedding = new ReadOnlyMemory<float>([1f, 0f, 0f]) });
        Assert.Equal(["documents"], await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public async Task A_deleted_name_can_be_taken_by_another_record_type()
    {
        using var store = new LodestarVectorStore();
        await store.GetCollection<string, Document>("documents").EnsureCollectionExistsAsync();

        await store.EnsureCollectionDeletedAsync("documents");
        VectorStoreCollection<int, VectorOnly> other = store.GetCollection<int, VectorOnly>("documents");
        await other.UpsertAsync(new VectorOnly { Id = 1, Embedding = new ReadOnlyMemory<float>([1f, 0f]) });

        Assert.Same(other, store.GetCollection<int, VectorOnly>("documents"));
        Assert.Equal(["documents"], await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public async Task A_name_written_again_after_its_deletion_is_held_again()
    {
        using var store = new LodestarVectorStore();
        VectorStoreCollection<string, Document> documents = store.GetCollection<string, Document>("documents");
        await store.EnsureCollectionDeletedAsync("documents");
        await documents.EnsureCollectionExistsAsync();

        ArgumentException error = Assert.Throws<ArgumentException>(() => store.GetCollection<int, VectorOnly>("documents"));
        Assert.StartsWith(
            "The collection documents is already held over String keys and Document records, and this asks for "
            + "Int32 keys and VectorOnly records;", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Requesting_a_collection_while_the_names_are_enumerated_does_not_throw()
    {
        using var store = new LodestarVectorStore();
        await store.GetCollection<string, Document>("first").EnsureCollectionExistsAsync();
        await store.GetCollection<string, Document>("second").EnsureCollectionExistsAsync();
        var seen = new List<string>();

        await foreach (string name in store.ListCollectionNamesAsync())
        {
            seen.Add(name);
            await store.GetCollection<string, Document>("during-" + name).EnsureCollectionExistsAsync();
        }

        Assert.Equal(["first", "second"], seen.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void Asking_the_store_for_a_null_service_type_throws()
    {
        using var store = new LodestarVectorStore();

        Assert.Throws<ArgumentNullException>(() => store.GetService(null!));
    }

    [Fact]
    public void Asking_a_collection_for_a_null_service_type_throws()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");

        Assert.Throws<ArgumentNullException>(() => collection.GetService(null!));
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

    [Fact]
    public async Task A_cancelled_token_is_refused_before_any_name_is_read_even_by_an_empty_or_disposed_store()
    {
        using var cancelled = new CancellationTokenSource();
        await cancelled.CancelAsync();
        using var empty = new LodestarVectorStore();
        var disposed = new LodestarVectorStore();
        disposed.Dispose();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            async () => await empty.ListCollectionNamesAsync(cancelled.Token).ToListAsync());
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            async () => await disposed.ListCollectionNamesAsync(cancelled.Token).ToListAsync());
    }

    [Fact]
    public async Task A_disposed_store_refuses_every_name_lookup()
    {
        var store = new LodestarVectorStore();
        store.Dispose();

        Assert.Throws<ObjectDisposedException>(() => store.GetCollection<string, Document>("documents"));
        await Assert.ThrowsAsync<ObjectDisposedException>(() => store.CollectionExistsAsync("documents"));
        await Assert.ThrowsAsync<ObjectDisposedException>(() => store.EnsureCollectionDeletedAsync("documents"));
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.ListCollectionNamesAsync().ToListAsync());
    }

    // #1266: GetCollection inserts, and a store shared as a singleton is asked for new names at once.
    [Fact]
    public async Task Concurrent_requests_for_new_names_each_get_one_collection_which_the_store_then_lists()
    {
        using var store = new LodestarVectorStore();
        const int Names = 500;
        var first = new VectorStoreCollection<string, Document>[Names];
        var second = new VectorStoreCollection<string, Document>[Names];

        await Task.WhenAll(Enumerable.Range(0, Names * 2).Select(i => Task.Run(async () =>
        {
            string name = $"c{i % Names}";
            (i < Names ? first : second)[i % Names] = store.GetCollection<string, Document>(name);
            await store.CollectionExistsAsync(name);
            if (i % 50 == 0)
            {
                await store.ListCollectionNamesAsync().ToListAsync();
            }
        })));

        for (int n = 0; n < Names; n++)
        {
            Assert.Same(first[n], second[n]);
            await first[n].EnsureCollectionExistsAsync();
        }

        Assert.Equal(Names, (await store.ListCollectionNamesAsync().ToListAsync()).Count);
    }
}
