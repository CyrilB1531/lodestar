# Vector store — `Lodestar.Extensions.VectorData`

An in-process [`Microsoft.Extensions.VectorData`](https://www.nuget.org/packages/Microsoft.Extensions.VectorData.Abstractions)
provider. A consumer written against that abstraction — a Semantic Kernel pipeline, a
retrieval-augmented chain, anything that takes a `VectorStore` — gets **hybrid keyword and vector
search with no database and no service running anywhere**. Every other provider that offers hybrid
search talks to a server, and the providers that run in process offer none; this one holds the
records in the calling process and fuses both rankings there.

It adds **no retrieval arithmetic of its own**. The vector half is
[`EmbeddingIndex`](../embeddings/search/embeddingindex.md), the keyword half is
[`Bm25Index`](../text/search/bm25index.md) over a
[`CountVectorizer`](../text/vectorizers/countvectorizer.md) vocabulary, and the fusion is
[`RankFusion.Rrf`](../text/search/rankfusion-rrf.md). What this package owes is the interface
contract: keys, upserts, deletes, filters, and the one place the two rankings meet.

## The shape: records are the truth, indexes are caches

Neither index can honour an upsert as it stands — `EmbeddingIndex` only appends, and `Bm25Index` is
built whole from a matrix and never changes. So each collection keeps its records in a dictionary
keyed by the record's key, and both indexes are **rebuilt from it on the first search after a write**.
A batch of writes costs one rebuild, and an updated record's old vector is gone rather than masked.
`decisions/0123`
records the choice and the two alternatives it beat.

That is also the cost to know about: the search that follows a write pays for a rebuild over
**every** record. A workload that alternates one write with one search rebuilds on every search;
reading a record by key never rebuilds anything.

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
| [`LodestarVectorStoreCollection`](store/lodestarvectorstorecollection.md) | One collection: records in a dictionary, a vector index and a BM25 index derived from them. |
| [`LodestarVectorStoreOptions`](store/lodestarvectorstoreoptions.md) | How a collection builds its keyword half, and the offset reciprocal-rank fusion uses. |

## See also

- [Semantic search with embeddings](../../guides/embeddings.md) — where the vectors come from.
- [Keyword search](../../guides/keyword-search.md) — the BM25 half on its own.
- [Python → C# equivalence](../../equivalence.md).
