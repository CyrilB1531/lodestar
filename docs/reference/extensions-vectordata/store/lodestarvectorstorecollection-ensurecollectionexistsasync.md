# LodestarVectorStoreCollection.EnsureCollectionExistsAsync

Marks the collection as existing.

<!-- docs-declaration -->

```csharp
public Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default)
```

**Parameters** — `cancellationToken` is checked before the collection is marked as existing.

**Returns** — a completed `Task`, or a cancelled one, changing nothing, when `cancellationToken` is
already cancelled ([#1354](https://github.com/CyrilB1531/lodestar/issues/1354)).

**Example** — an empty collection that exists, and is listed.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> EnsureAsync()
{
    using var store = new LodestarVectorStore();
    VectorStoreCollection<string, Note> notes = store.GetCollection<string, Note>("notes");

    await notes.EnsureCollectionExistsAsync();
    await notes.EnsureCollectionExistsAsync();

    List<string> names = await store.ListCollectionNamesAsync().ToListAsync();
    return string.Join(",", names);
}

string listed = EnsureAsync().GetAwaiter().GetResult();  // => notes
```

**Remarks** — calling it on a collection that already exists does nothing, and it never touches the
records: ensuring a collection that holds records keeps every one of them. A consumer written
against the abstraction calls this before its first write, because a server-backed store needs the
table created; here it only sets the flag
[`LodestarVectorStoreCollection.CollectionExistsAsync`](lodestarvectorstorecollection-collectionexistsasync.md)
reads, and an upsert would have set it anyway.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.EnsureCollectionDeletedAsync`](lodestarvectorstorecollection-ensurecollectiondeletedasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
