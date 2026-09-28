using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>The Review B findings of <c>Lodestar.Extensions.VectorData</c> after #1325.</summary>
public sealed class ReviewBFindingsTests
{
    private static Document Doc(string id) => new() { Id = id, Text = id, Embedding = new[] { 1f, 0f, 0f } };

    [Fact]
    public void A_width_whose_one_row_is_past_the_largest_array_is_refused_before_it_allocates()
    {
        var held = new HeldRecords<string, Document>(int.MaxValue);

        Assert.Throws<InvalidOperationException>(() => held.Put("a", Doc("a"), new float[3]));
        Assert.Throws<InvalidOperationException>(() => held.EnsureRoomFor(["a", "b"]));
        Assert.Equal(0, held.Count);
    }

    [Fact]
    public async Task A_null_collection_name_is_refused_under_name()
    {
        using var store = new LodestarVectorStore();

        Assert.Equal("name", (await Assert.ThrowsAsync<ArgumentNullException>(() => store.CollectionExistsAsync(null!))).ParamName);
        Assert.Equal("name", (await Assert.ThrowsAsync<ArgumentNullException>(() => store.EnsureCollectionDeletedAsync(null!))).ParamName);
    }

    [Fact]
    public async Task A_null_key_among_keys_is_refused_under_keys()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync(Doc("a"));

        ArgumentNullException error = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await collection.GetAsync(["a", null!]).ToListAsync());

        Assert.Equal("keys", error.ParamName);
    }

    [Fact]
    public async Task A_cancelled_token_writes_nothing_and_cancels_the_task()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        using var cancelled = new CancellationTokenSource();
        await cancelled.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.UpsertAsync(Doc("a"), cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.UpsertAsync([Doc("b")], cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.GetAsync("a", cancellationToken: cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.CollectionExistsAsync(cancelled.Token));
        Assert.Null(await collection.GetAsync("a"));
        Assert.Null(await collection.GetAsync("b"));

        await collection.UpsertAsync(Doc("c"));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.DeleteAsync("c", cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.DeleteAsync(["c"], cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.EnsureCollectionDeletedAsync(cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => collection.EnsureCollectionExistsAsync(cancelled.Token));
        Assert.NotNull(await collection.GetAsync("c"));
    }
}
