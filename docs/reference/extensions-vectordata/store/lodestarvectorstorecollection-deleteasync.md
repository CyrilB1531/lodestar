# LodestarVectorStoreCollection.DeleteAsync

Removes records by key.

<!-- docs-declaration -->

```csharp
public Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
public Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
```

**Parameters** — `key` is one record's key. `keys` is several. `cancellationToken` is checked before any
record is removed.

**Returns** — a completed `Task`, or a cancelled one, removing nothing, when `cancellationToken` is
already cancelled ([#1354](https://github.com/CyrilB1531/lodestar/issues/1354)).

**Exceptions** — `ArgumentNullException` when `key` or `keys` is null, or when `keys` holds a null
key. The batch overload reads and checks every key before it removes any, so a refused batch removes
nothing.

**Example** — a deleted record is gone from reads and from searches alike.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> RemoveAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "the elephant", Embedding = new float[] { 0f, 0f, 1f } },
    ]);

    await notes.DeleteAsync("a");
    await notes.DeleteAsync(["c", "never-written"]);

    List<VectorSearchResult<Note>> left = await notes
        .SearchAsync(new float[] { 1f, 0f, 0f }, 3)
        .ToListAsync();

    return string.Join(",", left.Select(hit => hit.Record.Id));
}

string remaining = RemoveAsync().GetAwaiter().GetResult();  // => b
```

**Remarks** — **a key that is not held is not an error**, in either overload, and costs nothing. A
delete costs the removed record alone: its slot is freed for the next new key, and its text's
postings leave the keyword half's terms, so no later search can return it or count it in a term's
document frequency. The records that remain keep their slots.

Deleting the last record does not delete the collection:
[`LodestarVectorStoreCollection.CollectionExistsAsync`](lodestarvectorstorecollection-collectionexistsasync.md)
still answers `true`. [`LodestarVectorStoreCollection.EnsureCollectionDeletedAsync`](lodestarvectorstorecollection-ensurecollectiondeletedasync.md)
is the member that does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.UpsertAsync`](lodestarvectorstorecollection-upsertasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
