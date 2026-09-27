# LodestarVectorStoreCollection

One collection: its records, and a vector index and a BM25 index kept current as they are written.

<!-- docs-declaration -->

```csharp
public sealed class LodestarVectorStoreCollection<TKey, TRecord> : VectorStoreCollection<TKey, TRecord>, IKeywordHybridSearchable<TRecord> where TKey : notnull where TRecord : class
```

**Constructor** — `LodestarVectorStoreCollection(string name, LodestarVectorStoreOptions options = null, VectorStoreCollectionDefinition definition = null)`.
`name` is what `Name` reports. `options` configures the keyword half and the fusion; `null` takes the
defaults. `definition` is an explicit schema; `null` reads `TRecord`'s `VectorStoreKey`,
`VectorStoreVector` and `VectorStoreData` attributes. It throws `ArgumentNullException` when `name`
is null; `ArgumentException` when the schema has no key property, a key property whose type is not
`TKey` (or its nullable), no vector property, a vector property that does not hold
`ReadOnlyMemory<float>`, a full-text indexed property that is not a `string`, or a definition naming
a property `TRecord` lacks; and `NotSupportedException` when the vector property declares a `DistanceFunction` other than
`CosineSimilarity`. A collection constructed directly belongs to no
store; one from [`LodestarVectorStore.GetCollection`](lodestarvectorstore-getcollection.md) is the
same type, held by name.

**Properties** — `Name` is the name the collection was constructed with.

**Example** — a collection on its own, written, searched, and read back.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> RoundTripAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
    ]);

    List<VectorSearchResult<Note>> nearest = await notes
        .SearchAsync(new float[] { 0.2f, 0.9f, 0f }, 1)
        .ToListAsync();
    Note byKey = await notes.GetAsync("a");

    return $"{notes.Name}: {nearest[0].Record.Id}, {byKey.Text}";
}

string seen = RoundTripAsync().GetAwaiter().GetResult();  // => notes: b, the cat sat on the mat
```

**Remarks** — **a write costs its own record, never the collection's.** An upsert normalizes the
record's vector into the record's slot and stages its full-text value; the first hybrid search after
it tokenizes what was staged, once, and adds its postings and counts to the keyword half. A delete
takes them out again. An upserted record's previous vector and text are gone rather than filtered
out of results. Every search ranks exactly as an `EmbeddingIndex`, a `CountVectorizer` vocabulary
with a `Bm25Index` over it, and `RankFusion.Rrf` built from scratch over the records held would —
results, scores and ties.

A search still reads every record, as an exact search must: `n` dot products, and a hybrid search
a pass over each ranking per depth it reads, to find the fused top without sorting either. What
BM25 defines over the whole corpus — a term's IDF, the average length, the floor on a negative IDF,
a share of the mean IDF over the vocabulary — is kept as counts, and only the floor, when a keyword
needs it, costs a pass over the vocabulary after a write. Reads by key touch neither index.

**Scores are cosine similarities.** The vector index normalizes each vector and the query, so a
vector's length never moves its rank — see [`EmbeddingIndex`](../../embeddings/search/embeddingindex.md).
A vector property that declares any other `DistanceFunction` — Euclidean, dot product, a distance of
any kind — is **refused at construction** rather than answered with cosine: a caller who declared a
distance would read a similarity as the wrong measure, and would also set `ScoreThreshold` in the
wrong direction, since a distance keeps what lies below it. Declaring `CosineSimilarity`, or nothing,
is accepted. `IndexKind` is not read: every search here is exact, which any index kind approximates.

**The collection hands back the records it holds, not copies.** Changing a record returned by
[`LodestarVectorStoreCollection.GetAsync`](lodestarvectorstorecollection-getasync.md) or a search
changes the stored one, and what a filter sees, but its vector and text were read at its write and
keep scoring as they were. Upsert the changed record to have it indexed.

**Every asynchronous member completes synchronously.** The work is done in memory before the task or
the first element comes back, and nothing pretends otherwise with a `Task.Yield`. Because an
enumerating member's results are complete before the first is yielded, a write made while a caller
enumerates them changes nothing already answered, and never throws out of the enumeration. A `Task` returned
here is already completed; an `IAsyncEnumerable` does its work when enumeration begins, which is
also when its arguments are checked.

**Thread safety** — searches may run concurrently with each other, the first hybrid search after a
write included: tokenizing what the write staged is done once, under a lock. Not safe for concurrent
writes, or for a search concurrent with a write. An in-memory collection built for one process is
not a database.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore`](lodestarvectorstore.md),
[`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`LodestarVectorStoreCollection.CollectionExistsAsync`](lodestarvectorstorecollection-collectionexistsasync.md) | Whether the collection has been ensured or written to. |
| [`LodestarVectorStoreCollection.DeleteAsync`](lodestarvectorstorecollection-deleteasync.md) | Removes records by key. |
| [`LodestarVectorStoreCollection.EnsureCollectionDeletedAsync`](lodestarvectorstorecollection-ensurecollectiondeletedasync.md) | Drops every record and marks the collection as not existing. |
| [`LodestarVectorStoreCollection.EnsureCollectionExistsAsync`](lodestarvectorstorecollection-ensurecollectionexistsasync.md) | Marks the collection as existing. |
| [`LodestarVectorStoreCollection.GetAsync`](lodestarvectorstorecollection-getasync.md) | Reads records by key, by keys, or by filter. |
| [`LodestarVectorStoreCollection.GetService`](lodestarvectorstorecollection-getservice.md) | Answers for the collection's metadata. |
| [`LodestarVectorStoreCollection.HybridSearchAsync`](lodestarvectorstorecollection-hybridsearchasync.md) | Fuses the vector ranking with a BM25 ranking over keywords. |
| [`LodestarVectorStoreCollection.SearchAsync`](lodestarvectorstorecollection-searchasync.md) | The records nearest a query vector, exactly, with or without a filter. |
| [`LodestarVectorStoreCollection.UpsertAsync`](lodestarvectorstorecollection-upsertasync.md) | Inserts records, or replaces them by key. |
