# LodestarVectorStore.CollectionExistsAsync

Whether a collection of that name has been created or written to.

<!-- docs-declaration -->

```csharp
public Task<bool> CollectionExistsAsync(string name, CancellationToken cancellationToken = default)
```

**Parameters** — `name` is the collection asked about. `cancellationToken` is checked on entry:
one already cancelled returns a cancelled task ([#1354](https://github.com/CyrilB1531/lodestar/issues/1354)).

**Returns** — a completed `Task<bool>`: `true` when the store holds a collection under `name` that
has been ensured or written to and not deleted since; `false` otherwise, including for a name the
store has never seen.

**Exceptions** — `ArgumentNullException` when `name` is null ([#1353](https://github.com/CyrilB1531/lodestar/issues/1353)).

**Example** — requested, ensured, deleted.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> TrackAsync()
{
    using var store = new LodestarVectorStore();
    VectorStoreCollection<string, Note> notes = store.GetCollection<string, Note>("notes");
    bool requested = await store.CollectionExistsAsync("notes");

    await notes.EnsureCollectionExistsAsync();
    bool ensured = await store.CollectionExistsAsync("notes");

    await store.EnsureCollectionDeletedAsync("notes");
    bool deleted = await store.CollectionExistsAsync("notes");

    return $"{requested} {ensured} {deleted}";
}

string states = TrackAsync().GetAwaiter().GetResult();  // => False True False
```

**Remarks** — it answers the same question as the collection's own
[`LodestarVectorStoreCollection.CollectionExistsAsync`](lodestarvectorstorecollection-collectionexistsasync.md),
asked by name. A name the store has never handed out is not an error, since asking whether something
exists is how a caller finds out that it does not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore.ListCollectionNamesAsync`](lodestarvectorstore-listcollectionnamesasync.md),
[`LodestarVectorStore`](lodestarvectorstore.md).
