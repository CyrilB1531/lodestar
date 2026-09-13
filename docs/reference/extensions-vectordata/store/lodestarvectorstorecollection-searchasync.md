# LodestarVectorStoreCollection.SearchAsync

The records nearest a query vector, exactly, with or without a filter.

<!-- docs-declaration -->

```csharp
public IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(TInput searchValue, int top, VectorSearchOptions<TRecord> options = null, CancellationToken cancellationToken = default) where TInput : notnull
```

**Parameters** — `searchValue` is the query vector, as a `ReadOnlyMemory<float>` or a `float[]`, of
the collection's vector width. `top` is the most results returned. `options` carries `Filter`, `Skip`
and `ScoreThreshold`, all honoured; `VectorProperty` and `IncludeVectors` are not read, since a
record has one vector and is returned whole. `cancellationToken` is checked between results.

**Returns** — `IAsyncEnumerable<VectorSearchResult<TRecord>>`, best first: each result is a held
record and its cosine similarity to the query. At most `top` results, fewer only when fewer records
pass the filter and the threshold after `Skip`.

**Exceptions** — `ArgumentOutOfRangeException` when `top` is less than 1. `ArgumentException` when
the query, or a record written since the last search, is not the collection's vector width.
`NotSupportedException` when `searchValue` is not a vector — a `string` included.
`OperationCanceledException` when `cancellationToken` is cancelled between results. All of them are
raised when enumeration begins, not when the method is called.

**Example** — a selective filter still returns `top` records.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> FilterAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "n1", Text = "news", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "n2", Text = "news", Embedding = new float[] { 0.9f, 0.1f, 0f } },
        new Note { Id = "n3", Text = "news", Embedding = new float[] { 0.8f, 0.2f, 0f } },
        new Note { Id = "m1", Text = "memo", Embedding = new float[] { 0.5f, 0.5f, 0f } },
        new Note { Id = "m2", Text = "memo", Embedding = new float[] { 0f, 1f, 0f } },
    ]);

    var memosOnly = new VectorSearchOptions<Note> { Filter = note => note.Text == "memo" };
    List<VectorSearchResult<Note>> hits = await notes
        .SearchAsync(new float[] { 1f, 0f, 0f }, 2, memosOnly)
        .ToListAsync();

    return string.Join(",", hits.Select(hit => hit.Record.Id));
}

string memos = FilterAsync().GetAwaiter().GetResult();  // => m1,m2
```

The three `news` records are nearer the query than either memo. A search that kept its two best
and filtered afterwards would return nothing at all.

**Remarks** — **with a filter, every record is scored and the filter runs before the cut**, so `top`
means `top`: asking for the five nearest records in French returns five whenever five are in French.
Filtering the top `k` after the fact is the cheaper design and is refused, because it returns fewer
results than asked — sometimes none — for a reason the caller cannot see. The exactness costs an
ordering over the whole collection, `O(n log n)`, where a search without a filter orders only
`top + Skip`.

`Skip` and `ScoreThreshold` both count over the records the filter admitted, and `Skip` counts after
the threshold. A threshold is a cosine similarity, so it lies in `[-1, 1]`.

**Only vectors are accepted.** Nothing in this package turns text into a vector; a `string` search
value is refused with that reason rather than embedded behind the caller's back. Embed it first —
[`Lodestar.Extensions.AI`](../../extensions-ai/generation.md) produces vectors of the width the
record declares.

A filter is compiled with `Expression.Compile`, once per call, which is not available under trimming
or ahead-of-time compilation. The first search after a write rebuilds the indexes; see
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md). An empty collection returns no
results.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.HybridSearchAsync`](lodestarvectorstorecollection-hybridsearchasync.md),
[`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
