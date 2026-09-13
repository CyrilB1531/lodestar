---
status: accepted
supersedes: []
amends: []
applies: ["0100"]
---
# 0123 — The `VectorData` store holds the records and derives both indexes from them

**Status:** accepted · **Date:** 2026-09-13

## Context

[Decision 0100](0100-vectordata-is-its-own-satellite-and-its-text-edge-waits-on-a-release.md)
placed a `Microsoft.Extensions.VectorData` provider in a satellite of its own,
`Lodestar.Extensions.VectorData`, and parked it until a published `Lodestar.Text` carried the keyword
half. It left the adapter's own shape to the code that would make it: an `UpsertAsync` over an
`EmbeddingIndex` that only appends, and what a compiled filter means in process.
[#682](https://github.com/CyrilB1531/lodestar/issues/682) asked for that code, or for a record
amending 0100 if the decision no longer held.

It holds, and this record **applies** 0100 rather than amending it. The reading 0100 took of
`Microsoft.Extensions.VectorData.Abstractions` 10.9.0 was taken again of 10.10.0 on 2026-09-12,
through [decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s
`MetadataLoadContext` protocol, and matched on every count:

| | 0100, at 10.9.0 | at 10.10.0 |
| --- | --- | --- |
| exported public types | 44 | 44 |
| target frameworks | `net10.0`, `net8.0`, `net462`, `netstandard2.0` | unchanged |
| package dependencies | `Microsoft.Extensions.AI.Abstractions` 10.9.0 only | `Microsoft.Extensions.AI.Abstractions` 10.10.0 only |
| `VectorStore` | 6 abstract members | 6 |
| `VectorStoreCollection<TKey, TRecord>` | 11 abstract members | 11 |
| `IKeywordHybridSearchable<TRecord>` | 2 members | 2 |
| licence | MIT | MIT |

`src/Directory.Packages.props` pins `Microsoft.Extensions.AI.Abstractions` at 10.10.0, which is
exactly what the one dependency asks for. And the block 0100 recorded is lifted: the published
`Lodestar.Text` **0.6.0** exports `Bm25Index`, `Bm25Options`, `Bm25Idf`, `SearchHit` and `RankFusion`
in `Lodestar.Text.Search`, where 0.5.0 exported none of them.

Read on 2026-09-13 at `CommunityToolkit/AI` `215a5bad`, where the `Microsoft.Extensions.VectorData`
connectors now live: of the ten connectors under `MEVD/src`, six implement
`IKeywordHybridSearchable<TRecord>` — Azure AI Search, Cosmos DB, PostgreSQL, Qdrant, SQL Server and
Weaviate — and every one of the six is a client of a server. The two that run in process,
`InMemory` and `SqliteVec`, implement no hybrid search. That is the gap this package fills.

## Decision 1 — records are the truth, and both indexes are caches

`VectorStoreCollection<TKey, TRecord>` declares `UpsertAsync` and `DeleteAsync` abstract, and
neither structure underneath can honour them. `EmbeddingIndex` only appends. `Bm25Index` is built
whole from a `CsrMatrix` of term counts and never changes.

**So a collection's state is a `Dictionary<TKey, TRecord>`, and both indexes are derived from it.**
A write changes the dictionary and marks the derived indexes stale. The first search after it
rebuilds three things in one pass: an `EmbeddingIndex` over every vector, through
[`EmbeddingIndex.FromOwnedBlock`](../reference/embeddings/search/embeddingindex-fromownedblock.md); a `CountVectorizer` vocabulary with a `Bm25Index` over every
full-text value; and the array mapping an index position back to a key. Upsert, delete and re-upsert
of one key fall out of the dictionary, and a batch of writes pays for one rebuild. Reading by key or
by filter walks the dictionary and rebuilds nothing.

The cost is stated rather than hidden: **a search after a write is linear in the records held**
before any scoring, which suits a collection loaded in batches and searched many times and does not
suit one interleaving single writes with searches at scale.

### The two alternatives that lost

**Appending with tombstones.** Vectors keep appending and deleted keys are masked from results. It
would halve the rebuild, and it breaks in three places: the vector index grows without bound, an
updated record's superseded vector still scores and has to be masked, and every top-`k` must
over-fetch by an amount nobody knows — which makes `top` approximate for a reason the caller cannot
see. `Bm25Index` would need rebuilding regardless, so the saving is on one index of two.

**Giving `EmbeddingIndex` a remove and `Bm25Index` an incremental update.** The better long-term
shape, and refused on ordering rather than on merit. `src/` reaches a sibling through a
`PackageReference` on a published floor
([decision 0012](0012-per-package-versioning.md)), so both core packages would have to change, be
released and be tagged before this one could build — the trap 0100 recorded once, paid twice.

## Decision 2 — a filtered search is exact, so `top` means `top`

A filter is an `Expression<Func<TRecord, bool>>`, compiled once per call. **When one is present,
every record is scored and the filter runs before the cut**, so a caller asking for five matching
records gets five whenever five match. Without a filter the index is asked for `top + Skip`, as
usual.

Post-filtering the top `k` lost: it returns fewer than `top` — sometimes none — whenever the filter
is selective, and a caller reads that as a bug. **Exactness costs no extra ordering**, which an
earlier draft of this record got wrong by claiming `O(n log n)` against `O(n log k)`:
`EmbeddingIndex.Search` scores and sorts all `n` records whatever `k` it is asked for, so the
unfiltered search pays the same `O(n log n)` and only copies fewer results out. The filter adds one
predicate call per record and nothing else, which leaves post-filtering no cost argument at all.

`Expression.Compile` needs dynamic code, so the filter path is not available under trimming or
ahead-of-time compilation. The reference pages say so.

A search value is a `ReadOnlyMemory<float>` or a `float[]`. A `string` throws
`NotSupportedException` naming the reason: nothing here generates embeddings, and giving this
package that edge would put `Microsoft.ML.OnnxRuntime` on the restore path of a caller who wanted a
store and no model — the measured cost 0100 refused folding on.

## Decision 3 — the schema, and what is refused rather than ignored

The schema comes from the `VectorStoreKey`, `VectorStoreVector` and `VectorStoreData` attributes, or
from an explicit `VectorStoreCollectionDefinition`. `IsFullTextIndexed` marks the property the
keyword half is built over; a record type with none serves every member except
`HybridSearchAsync`, which refuses by name.

**`GetDynamicCollection` throws `NotSupportedException`.** A dynamic record is a dictionary, and a
filter compiled against properties has none to bind to; interpreting the expression over dictionary
lookups would be a second filter path, and the typed key this design rests on would go with it.

**The filtered `GetAsync` honours `Skip` and refuses `OrderBy`**, which implementation added to the
spec. The first draft accepted `FilteredRecordRetrievalOptions` and read nothing from it. `Skip`
counts over the records the filter admits, as a filtered search counts it. `OrderBy` throws with its
reason: a dictionary has no order of its own, and ordering by an arbitrary property expression is a
feature this package has not decided. Refusing it now leaves implementing it later additive, where
un-ignoring an option once silently dropped would change behaviour under a caller.

**Two more options are refused rather than ignored, on the same reasoning**, both found in the
whole-branch review. `HybridSearchOptions.ScoreThreshold` throws `NotSupportedException`: a fused
score is a sum of `1 / (k + rank)`, not a similarity, so no threshold a caller writes for similarities
means anything against it. And a vector property declaring a `DistanceFunction` other than
`CosineSimilarity` is refused when the collection is constructed: `EmbeddingIndex` computes cosine
over normalised vectors only, and answering a declared distance with a similarity would also invert
which side of `ScoreThreshold` a result must fall on. `IndexKind` stays unread, because an exact
search is what every index kind approximates.

## Decision 4 — hybrid search joins three published pieces, and keeps only what the keywords matched

`HybridSearchAsync` is the strategic claim of
[#443](https://github.com/CyrilB1531/lodestar/issues/443): hybrid retrieval with no service running
anywhere. It joins what `Lodestar.Embeddings` and `Lodestar.Text` already publish:

1. the vector ranking of every record, from [`EmbeddingIndex.Search`](../reference/embeddings/search/embeddingindex-search.md);
2. the keyword ranking, from [`Bm25Index.Top`](../reference/text/search/bm25index-top.md), over the column indices [`CountVectorizer.Transform`](../reference/text/vectorizers/countvectorizer-transform.md)
   gives the keywords joined into one query document — so a keyword outside the fitted vocabulary
   drops, which is what BM25 means by an unseen term;
3. the fusion, [`RankFusion.Rrf`](../reference/text/search/rankfusion-rrf.md), at the collection's `RankFusionK`, 60 by default.

The vectorizer is fitted in the same rebuild that builds the `Bm25Index`, from the same values, so a
query is transformed against exactly the vocabulary the index was built over.

**Two facts established during implementation shape step 2**, and both are recorded here because
the spec did not know them.

**[`Bm25Index.Top`](../reference/text/search/bm25index-top.md) returns every document, zero-scorers included, tie-broken in index order, and
[`RankFusion.Rrf`](../reference/text/search/rankfusion-rrf.md) reads rank positions, not scores.** Passing `Top`'s whole list to the fusion would
hand a document the keywords never matched `1 / (k + rank)` credit for the order it was inserted in.
So the keyword ranking keeps only documents scoring above zero. The test that proves it inserts the
nearest-vector record last, so an insertion-order keyword ranking would put the wrong record first.

**A collection whose full-text values yield no tokens does not throw.** When every word is a stop
word or a single letter, [`CountVectorizer.FitTransform`](../reference/text/vectorizers/countvectorizer-fittransform.md) returns a matrix of no columns, `Bm25Index`
builds over no terms, and `Top` returns a fully tied ranking scored zero — which the filter above
empties. Hybrid search on such a collection degrades to the vector ranking scored through the
fusion. Throwing would make a store unusable over short texts whose property is marked, and the
refusal Decision 3 names correctly does not fire, because the property is marked.

## Decision 5 — `IAsyncEnumerable` over a synchronous index, and a correction to what it cost

0100's third decision stands: `async Task` is ordinary in this repository, and
`IAsyncEnumerable<T>` is the new idiom. Every enumerating member is an `async` iterator carrying
`[EnumeratorCancellation]`, yielding from a result already complete and checking the token between
items. None inserts a `Task.Yield` to look asynchronous, and none blocks: the work is done in memory,
which is the truth about an in-process store.

**The spec said `IAsyncEnumerable` costs no new declared dependency. That is true of the shipped
package and false of its test mirror**, and this record corrects it. The package's `.nuspec` gains
nothing: on `netstandard2.0`, `Microsoft.Bcl.AsyncInterfaces` arrives through
`Microsoft.Extensions.AI.Abstractions`. But `Lodestar.Extensions.VectorData.NetStandard.Tests` runs
on `net10.0` and loads the `netstandard2.0` build, which was compiled against
`Microsoft.Bcl.AsyncInterfaces` **10.0.12**, while the mirror's own restore resolves a lower version
through `xunit.v3`. Every test constructing a collection then failed at class load with
`FileNotFoundException`, since the type's vtable carries an `IAsyncEnumerable`-returning override.
It is [#529](https://github.com/CyrilB1531/lodestar/issues/529)'s trap one hop further down, and the
mirror pins the package at 10.0.12 in `tests/Directory.Packages.props` with a direct
`PackageReference`. A future bump of that assembly moves the pin with it, and nothing yet guards
another package's mirror against meeting the same trap.

## Consequences

**The `Lodestar.Text` floor rises from 0.4.0 to 0.6.0 for the whole repository, not for this package
alone.** 0100 argued about availability — whether a published package carried the types. What it
did not price is that `src/Directory.Packages.props` is Central Package Management, where a
`PackageVersion` is one version for every consumer: `Lodestar.Fuzzy` reaches `Lodestar.Text`
through the same pin and restores against 0.6.0 too. `tools/check_version_floor.py` asserts the
relationship rather than trusting it.

**The `Lodestar.Embeddings` floor rises from 0.5.0 to 0.6.0, for the same shared-pin reason.** The
first reading of this record kept it at 0.5.0, and that reading was wrong: it checked what 0.5.0
**exports** — `FromOwnedBlock`, `Search`, `Count` and `Dimension` are all there — and missed what
0.5.0 **depends on**. Its `.nuspec` still declares `Microsoft.ML.OnnxRuntime` 1.28.0 on both target
frameworks, because the split that moved `OnnxTextEmbedder` into `Lodestar.Onnx` first ships in
`Lodestar.Embeddings` 0.6.0. At 0.5.0 this package's restore graph resolved the native runtime —
the very dependency Decision 2 refuses to take — so 0100's "0.6.0" was the right floor, not a raise
for nothing. 0.6.0 is the floor that keeps `Microsoft.ML.OnnxRuntime` off this package's restore path;
the pin moves `Lodestar.Onnx` and `Lodestar.Extensions.AI` with it, which costs them nothing, since
0.6.0 exports everything 0.5.0 did and `Lodestar.Onnx` carries `Microsoft.ML.OnnxRuntime` 1.30.0
directly. **No gate caught it** because `tools/check_nuspec_dependencies.py` asserts a package's
direct edges only: this package's own `.nuspec` never named the runtime, which arrived one hop down.

- A seventeenth package, `net10.0;netstandard2.0`, in the interop tier, versioned 0.1.0, and the
  repository's eleventh and twelfth inter-package edges — to `Lodestar.Embeddings` and to
  `Lodestar.Text`. No other package's version moves and no tag is cut.
- **Nothing is persisted.** `EmbeddingIndex` can save itself and the collection does not expose it:
  a durable store is a second decision about how the records and the vocabulary are serialized, and
  the abstraction asks for neither.
- **No dynamic collections and no embedding generation**, for the reasons in Decisions 2 and 3.
- There is no Python reference for an interface adapter, so conformance is proven by driving the
  abstractions as a consumer does rather than by a frozen corpus, and `docs/equivalence.md` says so.
