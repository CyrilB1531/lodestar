# Vector store — `Lodestar.Extensions.VectorData`

An in-process [`Microsoft.Extensions.VectorData`](https://www.nuget.org/packages/Microsoft.Extensions.VectorData.Abstractions)
provider. A consumer written against that abstraction — a Semantic Kernel pipeline, a
retrieval-augmented chain, anything that takes a `VectorStore` — gets **hybrid keyword and vector
search with no database and no service running anywhere**. Every other provider that offers hybrid
search talks to a server, and the providers that run in process offer none; this one holds the
records in the calling process and fuses both rankings there.

It ranks **exactly as the published pieces rank**. The vector half scores as
[`EmbeddingIndex`](../embeddings/search/embeddingindex.md) does, the keyword half as
[`Bm25Index`](../text/search/bm25index.md) does over the vocabulary a
[`CountVectorizer`](../text/vectorizers/countvectorizer.md) would fit, and the fusion is
[`RankFusion.Rrf`](../text/search/rankfusion-rrf.md)'s. What this package owes is the interface
contract — keys, upserts, deletes, filters, and the one place the two rankings meet — and keeping
both halves current as records are written.

## The shape: a write costs its own record

Neither published index can honour an upsert as it stands — `EmbeddingIndex` only appends, and
`Bm25Index` is built whole from a matrix and never changes — and rebuilding them after every write
made a workload alternating writes and searches pay for **every** record on every search. So each
collection keeps what they are built from, per record: a vector normalized once into the record's
slot, and its text tokenized once by the vectorizer, with the postings and the counts BM25 reads
added and removed in place. An updated record's old vector and text are gone rather than masked.

What stays whole-corpus is what BM25 defines that way: a term's IDF depends on how many records
hold it, and the floor on a negative IDF is a share of the mean IDF over the whole vocabulary. Both
are kept as counts; the floor, when a query needs it, costs one pass over the vocabulary after each
write, and never one over the records. Results, scores and ties are those a from-scratch build over
the same records gives, which the suite replays random writes against. Reading a record by key
touches neither half.

## What it refuses, and why each refusal names its reason

- [`LodestarVectorStore.GetDynamicCollection`](store/lodestarvectorstore-getdynamiccollection.md)
  throws. A dynamic record is a dictionary, and a filter here is an expression compiled against a
  record's properties.
- A `string` search value throws. Nothing in this package turns text into a vector; embed it first
  — [`Lodestar.Extensions.AI`](../extensions-ai/generation.md) is one way.
- A filtered [`LodestarVectorStoreCollection.GetAsync`](store/lodestarvectorstorecollection-getasync.md)
  with `OrderBy` throws, because a dictionary has no order of its own to sort from.
- [`LodestarVectorStoreCollection.HybridSearchAsync`](store/lodestarvectorstorecollection-hybridsearchasync.md)
  throws on a record type that marks no `IsFullTextIndexed` property: there is no keyword half to
  fuse. It also throws when `ScoreThreshold` is set, because a fused score is not a similarity.
- A vector property declaring a `DistanceFunction` other than `CosineSimilarity` is refused when the
  [`LodestarVectorStoreCollection`](store/lodestarvectorstorecollection.md) is constructed: the index
  scores cosine only, and a declared distance would also turn `ScoreThreshold` around.

## Not trimming- or AOT-safe on the filter path

A filter is an `Expression<Func<TRecord, bool>>`, run in process by `Expression.Compile`, which needs
dynamic code. A trimmed or ahead-of-time compiled application can use every member here **without**
a filter; with one, it cannot.

## Types

| Type | What it is |
| --- | --- |
| [`LodestarVectorStore`](store/lodestarvectorstore.md) | An in-process vector store: named collections, held for the store's lifetime. |
| [`LodestarVectorStoreCollection`](store/lodestarvectorstorecollection.md) | One collection: its records, and a vector index and a BM25 index kept current as they are written. |
| [`LodestarVectorStoreOptions`](store/lodestarvectorstoreoptions.md) | How a collection builds its keyword half, and the offset reciprocal-rank fusion uses. |

## See also

- [Semantic search with embeddings](../../guides/embeddings.md) — where the vectors come from.
- [Keyword search](../../guides/keyword-search.md) — the BM25 half on its own.
- [Python → C# equivalence](../../equivalence.md).
