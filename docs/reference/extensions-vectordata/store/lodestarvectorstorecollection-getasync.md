# LodestarVectorStoreCollection.GetAsync

Reads records by key, by keys, or by filter.

<!-- docs-declaration -->

```csharp
public Task<TRecord> GetAsync(TKey key, RecordRetrievalOptions options = null, CancellationToken cancellationToken = default)
public IAsyncEnumerable<TRecord> GetAsync(IEnumerable<TKey> keys, RecordRetrievalOptions options = null, CancellationToken cancellationToken = default)
public IAsyncEnumerable<TRecord> GetAsync(Expression<Func<TRecord, bool>> filter, int top, FilteredRecordRetrievalOptions<TRecord> options = null, CancellationToken cancellationToken = default)
```

**Parameters** — `key` is one record's key. `keys` is several, read in the order given. `filter` is a
predicate over the record type, and `top` is the most records it returns. `options` carries the
abstraction's retrieval settings: on the filtered overload `Skip` is honoured and `OrderBy` refused,
and `IncludeVectors` is not read on any overload. `cancellationToken` is checked between records on
the two enumerating overloads and not observed on the first.

**Returns** — the first overload, a completed task holding the record, or `null` when the key is not
held. The second, the records held under `keys`, in the order of `keys`, skipping any key not held.
The third, at most `top` records the filter admits, after skipping `Skip` of them.

**Exceptions** — `ArgumentNullException` when `keys` or `filter` is null.
`ArgumentOutOfRangeException` when `top` is less than 1. `NotSupportedException` when `options` sets
`OrderBy`. `OperationCanceledException` when `cancellationToken` is cancelled between records. The
enumerating overloads check their arguments when enumeration begins, not when they are called.

**Example** — all three reads over one collection.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> ReadAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "the cat chased the dog", Embedding = new float[] { 0f, 0f, 1f } },
    ]);

    Note one = await notes.GetAsync("b");
    List<Note> several = await notes.GetAsync(["c", "missing", "a"]).ToListAsync();
    List<Note> cats = await notes.GetAsync(note => note.Text.Contains("cat"), top: 5).ToListAsync();

    return $"{one.Id} | {string.Join(",", several.Select(n => n.Id))} | {cats.Count}";
}

string read = ReadAsync().GetAwaiter().GetResult();  // => b | c,a | 2
```

**Remarks** — **none of these rebuild an index.** Reading by key goes straight to the dictionary, and
the filtered overload walks the records rather than a ranking, so a read after a write costs nothing
the write had not already paid.

**`OrderBy` is refused rather than ignored.** A dictionary has no order of its own to sort from, and
ordering by an arbitrary property expression is a feature this package has not decided; an ordering
silently dropped would hand a caller records in an order they did not ask for, with nothing to say
so. `Skip` has an order to count in — the records the filter admits, in the dictionary's enumeration
order — and that order is **not promised**. Page through a filter with `Skip` only where the page
boundaries do not have to be stable.

**The filter is compiled with `Expression.Compile`** once per call, which needs dynamic code: this
overload is not available under trimming or ahead-of-time compilation.

`IncludeVectors` does not apply. The record returned is the one the collection holds, vector
included — the same instance, not a copy, so a change made to it changes the stored record without
marking the indexes stale. The next search picks it up only when the indexes are rebuilt — after any
write, or at once if a write had already made them stale; upsert the changed record to be sure of
it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.SearchAsync`](lodestarvectorstorecollection-searchasync.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
