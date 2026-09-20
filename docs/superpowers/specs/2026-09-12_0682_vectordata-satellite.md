# 0682 — The VectorData satellite, built on records as the truth and indexes as caches

**Status:** written before the work, 2026-09-12.

**Issue:** [#682](https://github.com/CyrilB1531/lodestar/issues/682), which asks for what
[decision 0100](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0100-vectordata-is-its-own-satellite-and-its-text-edge-waits-on-a-release.md)
decided, *or* an ADR amending 0100 if the decision no longer holds.

## The two readings the issue asks for, taken

Both of the issue's measurement bullets are discharged here, through
[decision 0074](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s
protocol — a package's **exported surface** read through a `MetadataLoadContext`, which is what
`tools/survey.cs` runs.

**`Microsoft.Extensions.VectorData.Abstractions` 10.10.0**, read 2026-09-12, against 0100's reading
of 10.9.0:

| | 0100, at 10.9.0 | now, at 10.10.0 |
| --- | --- | --- |
| exported public types | 44 | **44** |
| target frameworks | `net10.0`, `net8.0`, `net462`, `netstandard2.0` | **unchanged** |
| package dependencies | `Microsoft.Extensions.AI.Abstractions` 10.9.0, and nothing else | **`Microsoft.Extensions.AI.Abstractions` 10.10.0, and nothing else** |
| `VectorStore` | 6 abstract members | **6** |
| `VectorStoreCollection<TKey, TRecord>` | 11 abstract, `Name` included | **11** |
| `IKeywordHybridSearchable<TRecord>` | 2, one the `GetService` the collection also carries | **2** |
| licence | MIT | **MIT** |

Nothing moved. 0100's placement argument rests on the dependency being the one
`Lodestar.Extensions.AI` already carries, and `src/Directory.Packages.props` pins
`Microsoft.Extensions.AI.Abstractions` at **10.10.0** — exactly what this package's single
dependency requires, so there is still no version range to reconcile. **0100 holds and is not
amended.** The issue's alternative outcome is available and is not taken, on measurement rather
than on preference.

**The `Lodestar.Text` block is lifted.** 0100 recorded that the published `Lodestar.Text` 0.5.0
exported 79 types and *"not one of them"* `Bm25Index`, `Bm25Options`, `Bm25Idf`, `SearchHit` or
`RankFusion`. Read from the published package rather than from the tree, `Lodestar.Text` **0.6.0**
exports 66 types, and all five are among them, in `Lodestar.Text.Search`. The condition 0100 parked
the lot on is met.

## What 0100 did not measure, and this does: the floor is shared

0100 argued about **availability** — whether a published package existed carrying the types. The
cost it did not price is that `src/Directory.Packages.props` is Central Package Management, so a
`PackageVersion` is **one version for the whole repository**, not per consumer.

Read today:

| package | pinned floor | latest tag | exports what this needs? |
| --- | --- | --- | --- |
| `Lodestar.Embeddings` | **0.5.0** | 0.7.0 | **exports yes at 0.5.0** — the full `EmbeddingIndex`: `Add`, `Search`, `Count`, `Dimension`, `FromBlock`, `FromOwnedBlock`, `Save`/`Load` — **but 0.5.0 still depends on `Microsoft.ML.OnnxRuntime` 1.28.0; 0.6.0 is the first that does not** |
| `Lodestar.Text` | **0.4.0** | 0.6.0 | **no at 0.4.0** — the keyword half arrives at 0.6.0 |

So this work raises the `Lodestar.Text` floor from **0.4.0 to 0.6.0**, and that raise is not private
to the new package: `Lodestar.Fuzzy` reaches `Lodestar.Text` through the same pin and will restore
against 0.6.0 too. That is a consequence to state in the ADR and to check, not a detail — and it is
the reason `tools/check_version_floor.py` asserts the relationship rather than trusting it.

**Correction, found in the whole-branch review: the `Lodestar.Embeddings` floor rises from 0.5.0 to
0.6.0 too.** This section first said the floor did not move, and that 0100's "0.6.0 is published and
exports it" would have raised a floor for nothing. That reading checked what 0.5.0 **exports** and
missed what it **depends on**: the 0.5.0 `.nuspec` still declares `Microsoft.ML.OnnxRuntime` 1.28.0,
because the split that moved `OnnxTextEmbedder` into `Lodestar.Onnx` first ships in 0.6.0, so a
restore of this package at 0.5.0 resolved the native runtime this spec's *Not in scope* refuses.
0.6.0 is the floor that keeps `Microsoft.ML.OnnxRuntime` off this package's restore path. Under
Central Package Management it moves `Lodestar.Onnx` and `Lodestar.Extensions.AI` as well, at no cost
to either. No gate caught it because `tools/check_nuspec_dependencies.py` asserts direct edges only,
and the runtime arrived one hop down.

## Decision 1 — the shape: records are the truth, indexes are caches

`VectorStoreCollection<TKey, TRecord>` declares `UpsertAsync` and `DeleteAsync` as abstract. The two
structures underneath cannot honour them as they stand:

- **`EmbeddingIndex` only appends.** Its surface is `Add`, `Search`, `Count`, `Dimension` and the
  persistence pair. There is no remove and no replace.
- **`Bm25Index` cannot take a document at all.** It is constructed whole from a `CsrMatrix` of term
  counts and is immutable thereafter — and it scores **term indices**, not text.

So a `Dictionary<TKey, TRecord>` is the collection's state, and both indexes are **derived caches**
rebuilt from it. A write updates the dictionary and sets a dirty flag; the first read after a write
rebuilds. One rebuild produces three things at once: an `EmbeddingIndex` over the vectors, a
`Bm25Index` over the full-text property, and a `TKey[]` mapping index position back to key.

Upsert, delete, and re-upsert of the same key all fall out of the dictionary for free, including the
case a tombstone scheme gets wrong — an updated record whose old vector would otherwise keep
scoring. A write is O(1); a batch of writes pays for exactly one rebuild, because the flag is set,
not counted.

The rebuild uses `EmbeddingIndex.FromOwnedBlock` rather than `Count` calls to `Add`, since the whole
block is known at that moment. That member is in 0.5.0 already, and so in the 0.6.0 floor.

### The two rejected alternatives, with what each costs

**Append with tombstones.** Vectors keep appending and deleted keys are filtered out of results.
Halves the rebuild, and breaks in three places: the vector index grows without bound, an updated
record's superseded vector still scores and must be masked, and every top-k has to over-fetch by an
unknown amount to survive that masking — which makes `top` approximate for a reason the caller
cannot see. `Bm25Index` would need rebuilding regardless, so the saving is one of the two.

**Give `EmbeddingIndex` a remove and `Bm25Index` an incremental update.** The better long-term
shape, and refused here on ordering rather than on merit: `src/` reaches a sibling through a
`PackageReference` on a **published** floor ([decision 0012](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0012-per-package-versioning.md)),
so both core packages would have to be changed, released and tagged before this package could
build against them. That is precisely the trap 0100 recorded once already, and paying it twice in
one lot would block the satellite on two releases instead of zero.

## Decision 2 — searching, and what `top` means with a filter

`VectorSearchOptions<TRecord>.Filter` is an `Expression<Func<TRecord, bool>>`. In process it is
compiled once per call and evaluated over the records.

**When a filter is present, the search is exact.** `EmbeddingIndex.Search(query, Count)` scores
every record, the filter runs over the results, and the first `top` survivors are returned. The
caller gets exactly `top` matching records whenever that many exist, which is what "the five nearest
documents in French" means. Without a filter it is `Search(query, top + Skip)` as usual.

Post-filtering the top-k is rejected: it silently returns fewer than `top` — sometimes zero —
whenever the filter is selective, and a caller reads that as a bug rather than as a documented
design. **Correction, found in the whole-branch review:** this said the exactness costs an
O(n log n) ordering instead of O(n log k). It does not: `EmbeddingIndex.Search` sorts all n records
whatever count it is asked for, so both paths pay the same ordering, and the filter adds only one
predicate call per record — which leaves post-filtering no cost argument to stand on.

`Expression.Compile` needs dynamic code, so this package is not trim- or AOT-safe on that path. That
is stated in its README and in the reference page rather than discovered, and it is not a new
constraint for a satellite.

**Search values.** `ReadOnlyMemory<float>` and `float[]` are accepted directly. A `string` throws
`NotSupportedException` naming the reason — nothing in this package turns text into a vector, and
the embedding generator is the caller's to supply.

## Decision 3 — the schema, and the one refusal

Schema comes from the attributes (`VectorStoreKeyAttribute`, `VectorStoreVectorAttribute`,
`VectorStoreDataAttribute`) **or** from an explicit `VectorStoreCollectionDefinition`, since several
members of the surface exist only to serve the second. `VectorStoreDataAttribute.IsFullTextIndexed`
is what marks the property the `Bm25Index` is built over; a collection with no such property still
works and refuses only `HybridSearchAsync`, naming that.

**`VectorStore.GetDynamicCollection` throws `NotSupportedException`.** A dynamic record is a
`Dictionary<string, object>`, and an `Expression<Func<TRecord, bool>>` over one cannot be compiled
against properties — it needs a separate interpretation path, and the typed key this design rests on
disappears with it. The member is present because the base class declares it abstract; it refuses
with the reason, which is the honest conformance answer rather than a silent partial one.

## Decision 4 — hybrid search is the claim, and every piece of it is already published

`IKeywordHybridSearchable<TRecord>.HybridSearchAsync(searchValue, keywords, top, options)` is the
strategic argument of [#443](https://github.com/CyrilB1531/lodestar/issues/443) — a hybrid vector
store with no external database, where every other provider in the ecosystem needs a service
running somewhere. The adapter only joins what exists:

1. the vector ranking, from `EmbeddingIndex.Search`;
2. the keyword ranking, from `Bm25Index.Top(terms, count)`, where `terms` are the non-zero columns
   of `CountVectorizer.Transform` applied to the keywords joined into one query document — so a
   keyword outside the fitted vocabulary drops, which is what BM25 means by an unseen term;
3. the fusion, from `RankFusion.Rrf(rankings, k)` at its `DefaultK` of 60.

The vectorizer is fitted during the same rebuild that builds the `Bm25Index`, from the same
documents, so the vocabulary a query is transformed against is by construction the one the index was
built over.

`RankFusion.Rrf` scores `Σ 1 / (k + rank)` with **rank counted from 1**, and returns every document
that appeared in any ranking, best first. The collection then maps document positions back to keys
through the positional array and yields `top` of them.

## Decision 5 — `IAsyncEnumerable` over a synchronous index

0100's third decision stands: `async Task` is ordinary here, `IAsyncEnumerable<T>` is the new idiom,
and it costs no new declared dependency — on `netstandard2.0` the type comes from
`Microsoft.Bcl.AsyncInterfaces`, already in the restore graph through
`Microsoft.Extensions.AI.Abstractions`.

Every `Get` and `Search` on the surface returns one over an in-memory result that is already
complete. They are written as `async IAsyncEnumerable<T>` iterators carrying
`[EnumeratorCancellation]`, yielding from the computed list and honouring the token between items.
They do not pretend to be asynchronous by inserting a `Task.Yield`, and they do not block: the work
is done before the first `yield`, which is the truth about an in-memory store and is what the
reference page says.

## The package

`Lodestar.Extensions.VectorData`, interop tier, `net10.0;netstandard2.0` — the seventeenth package
and, with its two edges, the repository's **eleventh and twelfth**. Both counts are asserted:
`tools/check_claude_md_packages.py` against CLAUDE.md's table and `tools/check_nuspec_dependencies.py`
against its `EXPECTED` edge map, so CLAUDE.md's architecture table, its "The edges: **ten**" sentence
and both scripts move in this branch.

| file | holds |
| --- | --- |
| `LodestarVectorStore.cs` | the `VectorStore`: named collections in a dictionary, `GetCollection`, `ListCollectionNamesAsync`, and `GetDynamicCollection`'s refusal |
| `LodestarVectorStoreCollection.cs` | the `VectorStoreCollection<TKey, TRecord>` and its `IKeywordHybridSearchable<TRecord>`: the record dictionary, the write path, the read path |
| `LodestarVectorStoreOptions.cs` | the vectorizer options, the BM25 options, the RRF `k` |
| `Internal/RecordSchema.cs` | reading attributes or a definition into key, vector and full-text accessors |
| `Internal/DerivedIndexes.cs` | the rebuild: block, `EmbeddingIndex`, `CountVectorizer`, `Bm25Index`, positional keys |
| `Internal/RecordFilter.cs` | compiling and applying the expression |

Two test projects, `Lodestar.Extensions.VectorData.Tests` and its
`.NetStandard.Tests` mirror linking the same sources with `SetTargetFramework=netstandard2.0` and a
`ProjectReference` per `Lodestar.*` dependency — never a `PackageReference`, which is where the pin
leaks ([#529](https://github.com/CyrilB1531/lodestar/issues/529), enforced by
`tools/check_netstandard_guards.py`).

## Testing

There is **no Python reference for an interface adapter**, so conformance cannot be an oracle
corpus. It is proven by driving the abstractions exactly as a consumer does — constructing a store,
upserting typed records, searching, filtering, fusing — and the `docs/equivalence.md` row says that
in those words, so the suite does not read as skipped.

The facts worth naming, because each is a place this design could be wrong rather than merely
untested:

- upsert of an existing key replaces rather than duplicates, and a search after it never returns
  the superseded vector — the case the tombstone alternative gets wrong;
- delete then search returns neither the record nor its vector, and the positional mapping still
  lines up for the records that remain;
- a batch of writes followed by one search rebuilds once, asserted by counting rebuilds through an
  internal hook rather than by timing;
- a filter that admits two of ten records returns two when `top` is five, and returns them in score
  order — the exactness Decision 2 buys;
- `HybridSearchAsync` returns a record the vector half ranks poorly and the keyword half ranks
  first, which is the only assertion that proves the fusion happened rather than one side winning;
- a keyword outside the fitted vocabulary contributes nothing and does not throw;
- `GetDynamicCollection`, and a `string` search value with no generator, both throw
  `NotSupportedException` with their reason;
- a collection whose record type marks no `IsFullTextIndexed` property serves `SearchAsync` and
  refuses `HybridSearchAsync`.

## Documentation owed, in the same commit as the code

- CLAUDE.md's package table gains a row, and its edge count moves from ten to twelve; both are
  asserted by the two scripts named above.
- `README.md`'s package list and the pack loop (`tools/check_readme_packages.py`,
  `tools/check_readme_pack_loop.py`), and the release workflow
  (`tools/check_release_workflow_packages.py`).
- `docs/wiki-map.json`: the package, its pages, and the namespace the reference gate enforces.
- Reference pages under `docs/reference/` for every public member, with executable fences.
- A `docs/migration/` row — what a caller uses today instead, and why this exists.
- A `docs/equivalence.md` row saying conformance is proven against the abstractions, not against
  Python.
- `samples/Lodestar.Sample` gains a lot exercising every public **member** — the packaging gate
  counts members, not types, which [#695](https://github.com/CyrilB1531/lodestar/pull/695) paid to
  learn — and the package id joins `check_sample_coverage.py`'s `CONVERTED` list.
- One `CHANGELOG.md` entry under a new `### Lodestar.Extensions.VectorData`.
- An ADR recording Decisions 1 to 5 and the shared-floor consequence, and **applying** 0100 rather
  than amending it, since nothing in 0100 changed. **Take its number from `./.next-adr` at the
  repository root, never by listing `docs/decisions/`** — that listing is what produced three
  collisions in three days; `.next-adr` reported `0119` on 2026-09-12 and reads branches, worktrees
  and sibling clones that a listing cannot see.

## Not in scope

- **No persistence.** `EmbeddingIndex` has `Save`/`Load` and the collection does not expose them:
  a durable store is a second decision about what the record dictionary and the vocabulary are
  serialized as, and the abstraction has no member asking for it.
- **No dynamic collections**, for the reason in Decision 3.
- **No embedding generation.** `VectorStoreVectorProperty.EmbeddingGenerator` is read from the
  schema when present and passed nowhere: generating vectors is `Lodestar.Extensions.AI`'s job and
  giving this package that edge would pull `Microsoft.ML.OnnxRuntime`'s 132.7 MB onto the restore
  path of a caller who wanted a store and no model — the measured cost 0100 refused folding on.
- **No version bump anywhere but the new package's own 0.1.0**, and no release: the
  `Lodestar.Text` and `Lodestar.Embeddings` floor raises each consume an already-published 0.6.0 and
  ask for no new tag.
