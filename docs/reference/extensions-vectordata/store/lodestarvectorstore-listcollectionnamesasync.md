# LodestarVectorStore.ListCollectionNamesAsync

The names of the collections that exist.

<!-- docs-declaration -->

```csharp
public IAsyncEnumerable<string> ListCollectionNamesAsync(CancellationToken cancellationToken = default)
```

**Parameters** — `cancellationToken` is checked before each name.

**Returns** — `IAsyncEnumerable<string>`: one name per collection this store has handed out that
has been ensured or written to, and not deleted since. No order is promised.

**Exceptions** — `OperationCanceledException` when `cancellationToken` is cancelled while the names
are enumerated.

**Example** — a collection that was only asked for is not listed.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> ListAsync()
{
    using var store = new LodestarVectorStore();
    await store.GetCollection<string, Note>("written").UpsertAsync(
        new Note { Id = "a", Text = "a note", Embedding = new float[] { 1f, 0f, 0f } });
    await store.GetCollection<string, Note>("ensured").EnsureCollectionExistsAsync();
    store.GetCollection<string, Note>("requested");

    List<string> names = await store.ListCollectionNamesAsync().ToListAsync();
    names.Sort(StringComparer.Ordinal);
    return string.Join(",", names);
}

string listed = ListAsync().GetAwaiter().GetResult();  // => ensured,written
```

**Remarks** — **the enumeration is not asynchronous work.** The names are already in memory, so
nothing is awaited between them; the method is an iterator because the abstraction returns one, and
it does not insert a `Task.Yield` to look otherwise. Like every iterator, it runs nothing until it is
enumerated.

A collection deleted through
[`LodestarVectorStore.EnsureCollectionDeletedAsync`](lodestarvectorstore-ensurecollectiondeletedasync.md)
drops out of the listing and stays retrievable by name, empty; writing to it lists it again.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore.CollectionExistsAsync`](lodestarvectorstore-collectionexistsasync.md),
[`LodestarVectorStore`](lodestarvectorstore.md).
