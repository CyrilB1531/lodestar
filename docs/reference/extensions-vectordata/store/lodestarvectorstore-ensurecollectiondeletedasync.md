# LodestarVectorStore.EnsureCollectionDeletedAsync

Empties a collection and marks it as not existing.

<!-- docs-declaration -->

```csharp
public Task EnsureCollectionDeletedAsync(string name, CancellationToken cancellationToken = default)
```

**Parameters** — `name` is the collection to delete. `cancellationToken` is accepted for the
abstraction's sake and not observed.

**Returns** — a completed `Task`.

**Example** — the records go, the collection object stays usable.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> DeleteAsync()
{
    using var store = new LodestarVectorStore();
    VectorStoreCollection<string, Note> notes = store.GetCollection<string, Note>("notes");
    await notes.UpsertAsync(new Note { Id = "a", Text = "a note", Embedding = new float[] { 1f, 0f, 0f } });

    await store.EnsureCollectionDeletedAsync("notes");
    bool gone = await notes.GetAsync("a") is null;
    bool exists = await store.CollectionExistsAsync("notes");

    return $"{gone} {exists}";
}

string after = DeleteAsync().GetAwaiter().GetResult();  // => True False
```

**Remarks** — **"ensure" means it cannot fail for being already done.** A name the store has never
handed out, or a collection deleted twice, completes without error — which is the abstraction's
contract, and what lets a caller delete at the start of a run without checking first.

The store **keeps the collection object**. [`LodestarVectorStore.GetCollection`](lodestarvectorstore-getcollection.md)
with the same name returns the same, now empty, instance, and a write to it makes it exist again.
Forgetting it would let a second request build a second collection under a name a caller may still
hold the first one for.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.EnsureCollectionDeletedAsync`](lodestarvectorstorecollection-ensurecollectiondeletedasync.md),
[`LodestarVectorStore`](lodestarvectorstore.md).
