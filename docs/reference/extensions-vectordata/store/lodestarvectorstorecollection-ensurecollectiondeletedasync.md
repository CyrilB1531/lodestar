# LodestarVectorStoreCollection.EnsureCollectionDeletedAsync

Drops every record and marks the collection as not existing.

<!-- docs-declaration -->

```csharp
public Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default)
```

**Parameters** — `cancellationToken` is accepted for the abstraction's sake and not observed.

**Returns** — a completed `Task`.

**Example** — deleted, then written to again.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> RecycleAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync(new Note { Id = "a", Text = "old", Embedding = new float[] { 1f, 0f, 0f } });

    await notes.EnsureCollectionDeletedAsync();
    bool emptied = await notes.GetAsync("a") is null;

    await notes.UpsertAsync(new Note { Id = "b", Text = "new", Embedding = new float[] { 0f, 1f, 0f } });
    List<VectorSearchResult<Note>> hits = await notes.SearchAsync(new float[] { 1f, 0f, 0f }, 5).ToListAsync();

    return $"{emptied} {await notes.CollectionExistsAsync()} {hits.Count}";
}

string after = RecycleAsync().GetAwaiter().GetResult();  // => True True 1
```

**Remarks** — deleting a collection that does not exist does nothing, which is the abstraction's
meaning of "ensure". The indexes are marked stale with the records, so a search after a deletion
never finds a record the deletion removed.

The object remains usable, and a store that handed it out keeps holding it under its name — see
[`LodestarVectorStore.EnsureCollectionDeletedAsync`](lodestarvectorstore-ensurecollectiondeletedasync.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.DeleteAsync`](lodestarvectorstorecollection-deleteasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
