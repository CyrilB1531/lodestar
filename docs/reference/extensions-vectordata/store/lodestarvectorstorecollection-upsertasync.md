# LodestarVectorStoreCollection.UpsertAsync

Inserts records, or replaces them by key.

<!-- docs-declaration -->

```csharp
public Task UpsertAsync(TRecord record, CancellationToken cancellationToken = default)
public Task UpsertAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken = default)
```

**Parameters** — `record` is one record to write. `records` is several, written in order, so a key
repeated inside the batch keeps its last record. `cancellationToken` is accepted for the
abstraction's sake and not observed.

**Returns** — a completed `Task`.

**Exceptions** — `ArgumentNullException` when `record` or `records` is null, or when `records` holds a
null record. `ArgumentException` when a record's key is null, or when its vector is not the width the
schema declares; the message names the key and both widths.

**Example** — writing the same key twice replaces the record, and its old vector with it.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> ReplaceAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "first draft", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "another note", Embedding = new float[] { 0f, 1f, 0f } },
    ]);
    await notes.UpsertAsync(new Note { Id = "a", Text = "second draft", Embedding = new float[] { 0f, 0f, 1f } });

    List<VectorSearchResult<Note>> hits = await notes
        .SearchAsync(new float[] { 1f, 0f, 0f }, 2)
        .ToListAsync();

    return string.Join(",", hits.Select(hit => $"{hit.Record.Id}={hit.Score}"));
}

string scored = ReplaceAsync().GetAwaiter().GetResult();  // => a=0,b=0
```

Nothing scores `1` any more: the vector `a` once had is not in the index, rather than hidden from
the results.

**Remarks** — **a write never rebuilds anything.** It stores the record under its key, marks the
collection as existing, and marks the indexes stale; the next
[`LodestarVectorStoreCollection.SearchAsync`](lodestarvectorstorecollection-searchasync.md) or
[`LodestarVectorStoreCollection.HybridSearchAsync`](lodestarvectorstorecollection-hybridsearchasync.md)
rebuilds them once, however many writes came first. Prefer the batch overload for a bulk load only
for readability — a hundred single upserts followed by one search cost the same single rebuild.

**A vector of the wrong width is refused here, at the write.** A record whose vector is not the
schema's `Dimensions` long throws `ArgumentException` naming its key and both widths, and nothing is
stored. The batch overload checks **every** record before it writes any, so a batch holding one bad
record — a wrong width, a null key, a null record — leaves the collection exactly as it was.

The record is **held, not copied**: mutating it after the upsert changes what the collection holds
without marking the indexes stale. Upsert it again after changing it — a vector changed in place to
another width is the one case the write cannot see, and the next rebuild refuses it instead.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.DeleteAsync`](lodestarvectorstorecollection-deleteasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
