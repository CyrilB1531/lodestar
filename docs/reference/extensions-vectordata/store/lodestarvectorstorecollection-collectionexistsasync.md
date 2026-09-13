# LodestarVectorStoreCollection.CollectionExistsAsync

Whether the collection has been ensured or written to.

<!-- docs-declaration -->

```csharp
public Task<bool> CollectionExistsAsync(CancellationToken cancellationToken = default)
```

**Parameters** — `cancellationToken` is accepted for the abstraction's sake and not observed.

**Returns** — a completed `Task<bool>`: `true` once
[`LodestarVectorStoreCollection.EnsureCollectionExistsAsync`](lodestarvectorstorecollection-ensurecollectionexistsasync.md)
or an upsert has run, and `false` again after
[`LodestarVectorStoreCollection.EnsureCollectionDeletedAsync`](lodestarvectorstorecollection-ensurecollectiondeletedasync.md).

**Example** — an upsert is enough to make a collection exist.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> ExistAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    bool constructed = await notes.CollectionExistsAsync();

    await notes.UpsertAsync(new Note { Id = "a", Text = "a note", Embedding = new float[] { 1f, 0f, 0f } });
    bool written = await notes.CollectionExistsAsync();

    return $"{constructed} {written}";
}

string states = ExistAsync().GetAwaiter().GetResult();  // => False True
```

**Remarks** — a server-backed provider answers this by asking the server whether a table or an index
is there. An in-process collection has nothing to ask, so existence is a flag the collection keeps,
set and cleared exactly where the abstraction says a server's would be. That is what lets code
written against the abstraction run unchanged against this store.

Deleting every record does **not** clear it; only deleting the collection does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore.CollectionExistsAsync`](lodestarvectorstore-collectionexistsasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
