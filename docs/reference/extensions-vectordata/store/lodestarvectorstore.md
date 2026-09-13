# LodestarVectorStore

An in-process vector store: named collections, held for the store's lifetime.

<!-- docs-declaration -->

```csharp
public sealed class LodestarVectorStore : VectorStore
```

**Constructor** — `LodestarVectorStore(LodestarVectorStoreOptions options = null)`. Every collection
the store hands out takes the same [`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md);
`null` takes the defaults. It throws nothing and opens nothing — there is no connection to make.

**Example** — a store, one collection, and the hybrid search the package exists for. The record type
is declared once, and every example on these pages uses it:

<!-- docs-compile: skip - a class carrying attributes cannot be declared inside the method a fence becomes; the snippet project declares this same type -->
```csharp
using Microsoft.Extensions.VectorData;

sealed class Note
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
```

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> FuseAsync()
{
    using var store = new LodestarVectorStore();
    VectorStoreCollection<string, Note> notes = store.GetCollection<string, Note>("notes");

    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "an elephant crossed the river", Embedding = new float[] { 0f, 0f, 1f } },
    ]);

    var hybrid = (IKeywordHybridSearchable<Note>)notes;
    List<VectorSearchResult<Note>> hits = await hybrid
        .HybridSearchAsync(new float[] { 1f, 0f, 0f }, ["elephant"], 2)
        .ToListAsync();

    return string.Join(",", hits.Select(hit => hit.Record.Id));
}

string order = FuseAsync().GetAwaiter().GetResult();  // => c,a
```

The query vector points at `a`, the keyword matches only `c`, and the fused order is `c` then `a`
— which neither ranking gives alone. `ToListAsync` is .NET 10's `System.Linq.AsyncEnumerable`; the
`GetAwaiter().GetResult()` only lets a synchronous example drive an asynchronous one.

**Remarks** — **a collection exists once something has ensured it or written to it**, and that is
what [`LodestarVectorStore.ListCollectionNamesAsync`](lodestarvectorstore-listcollectionnamesasync.md)
and [`LodestarVectorStore.CollectionExistsAsync`](lodestarvectorstore-collectionexistsasync.md)
report. [`LodestarVectorStore.GetCollection`](lodestarvectorstore-getcollection.md) alone creates
nothing, the way asking a database for a table object does not create the table.

**One name is one collection for the store's lifetime.** Asking for the same name again returns the
same instance, and deleting a collection empties it without forgetting it — a later write to that
instance makes it exist again.

**Nothing is persisted.** The records live in this process and are gone when it ends. `EmbeddingIndex`
can save itself, and this store does not expose that: a durable store would have to decide how the
records and the fitted vocabulary are serialized, and the abstraction asks for neither.

**Disposing the store disposes every collection it handed out** and forgets them. `Dispose` is the
base class's, so it has no entry of its own here.

**Thread safety** — not safe for concurrent writes, which is true of every collection it holds.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md),
[`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`LodestarVectorStore.CollectionExistsAsync`](lodestarvectorstore-collectionexistsasync.md) | Whether a collection of that name has been created or written to. |
| [`LodestarVectorStore.EnsureCollectionDeletedAsync`](lodestarvectorstore-ensurecollectiondeletedasync.md) | Empties a collection and marks it as not existing. |
| [`LodestarVectorStore.GetCollection`](lodestarvectorstore-getcollection.md) | The collection held under a name, created on first request. |
| [`LodestarVectorStore.GetDynamicCollection`](lodestarvectorstore-getdynamiccollection.md) | Refuses: this store serves typed records only. |
| [`LodestarVectorStore.GetService`](lodestarvectorstore-getservice.md) | Answers for the store's metadata. |
| [`LodestarVectorStore.ListCollectionNamesAsync`](lodestarvectorstore-listcollectionnamesasync.md) | The names of the collections that exist. |
