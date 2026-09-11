---
status: accepted
supersedes: []
amends: []
applies: ["0074"]
---
# 0100 — `VectorData` is its own satellite, and its `Lodestar.Text` edge waits on a release

**Status:** accepted · **Date:** 2026-09-10

## Context

[#572](https://github.com/CyrilB1531/lodestar/issues/572) is lot 2 of
[#443](https://github.com/CyrilB1531/lodestar/issues/443)'s Phase 5, and #443 calls it the
strategic argument of the whole roadmap: **a hybrid vector store with no external database**, where
every other provider in the Semantic Kernel ecosystem needs a service running somewhere. Its
acceptance criteria ask for this record *before* code, on two questions — where the conforming
types live, and what the new inter-package edges cost.

Both are settled here. Nothing about the adapter's own shape is: `UpsertAsync` against an
append-only `EmbeddingIndex`, and what a compiled `Expression<Func<TRecord, bool>>` filter means
in process, are implementation decisions and belong to the code that makes them.

## The reading

[ADR 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) requires a package's
**exported surface** through a `MetadataLoadContext`, not its README.
`Microsoft.Extensions.VectorData.Abstractions` **10.9.0**, MIT, read on 2026-09-10:

| | |
| --- | --- |
| exported public types | **44** |
| target frameworks | `net10.0`, `net8.0`, `net462`, **`netstandard2.0`** — both of this repository's |
| package dependencies | exactly one: `Microsoft.Extensions.AI.Abstractions` **10.9.0** |
| `VectorStore` | **6** abstract members |
| `VectorStoreCollection<TKey, TRecord>` | **11** abstract members, `Name` included |
| `IKeywordHybridSearchable<TRecord>` | **2**, one of which is the `GetService` the collection also carries |

Every type #572 names is there, `IKeywordHybridSearchable<TRecord>` and `HybridSearchOptions<T>`
among them. The single dependency is **the version `Lodestar.Extensions.AI` already pins**, so no
version range has to be reconciled.

## Decision 1 — a satellite of its own, named `Lodestar.Extensions.VectorData`

A conforming package carries an external dependency, so it is satellite tier rather than core
([decision 0076](0076-a-core-package-carries-no-external-dependency.md)). The name follows
`Lodestar.Extensions.AI` and `Lodestar.Extensions.MathNet`: the interop tier is named for the
thing it interoperates with, and it is the tier
[decision 0089](0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md) opened for
exactly this — a dependency a core package refused.

**The option that had to be refused is folding it into `Lodestar.Extensions.AI`**, which is
tempting for a real reason — that package already carries `Microsoft.Extensions.AI.Abstractions`
10.9.0, the one dependency this needs, so folding would add *no* new external dependency at all.

It is refused on a measured cost. `Lodestar.Extensions.AI` carries an edge to `Lodestar.Onnx`,
whose whole reason to exist is `Microsoft.ML.OnnxRuntime` — **1.28.0 is a 132.7 MB package**
(139 145 017 bytes), almost all of it native assets for platforms a given consumer does not run
on. Folding would put that on the restore path of every caller who wanted a vector store and no
model, which is exactly the caller this lot is for. A satellite that shares a dependency with
another satellite is cheap; one that inherits the other's dependencies is not.

## Decision 2 — two edges, and one of them is not available yet

`Lodestar.Extensions.VectorData` needs **two** new inter-package edges, each with a published
floor in `src/Directory.Packages.props`, an `EXPECTED` entry in
`tools/check_nuspec_dependencies.py` and an edge in `tools/check_version_floor.py`:

- **`→ Lodestar.Embeddings`**, for `EmbeddingIndex` — the vector half. Available: `0.6.0` is
  published and exports it.
- **`→ Lodestar.Text`**, for `Bm25Index` and `RankFusion` — the keyword half. **Not available.**

`src/` reaches a sibling through a `PackageReference` on a **published** floor
([decision 0012](0012-per-package-versioning.md)), and the same `MetadataLoadContext` reading
applied to the published `Lodestar.Text` **0.5.0** finds **79 exported types and not one of
them** `Bm25Index`, `Bm25Options`, `Bm25Idf`, `SearchHit` or `RankFusion`. The tag
`Lodestar.Text/v0.5.0` was cut on #558, two days before
[#573](https://github.com/CyrilB1531/lodestar/issues/573) added them.

**So this lot lands after a `Lodestar.Text` release that carries the keyword half**, and not
before. That is the same ordering [#569](https://github.com/CyrilB1531/lodestar/issues/569) met
and is not a new rule; it is recorded here because #572's own acceptance criteria list the edge
without listing the release it waits on, and because the alternative routes are both worse:

- **Reimplementing BM25 inside the satellite** duplicates a published algorithm across two
  packages, which the repository resolves by extraction rather than by copying.
- **Taking the keyword half as a caller-supplied delegate** removes the edge and, with it, the
  claim: a store that only ranks by keyword if the caller wires the ranking is not the
  batteries-included hybrid #443 refuses to ship without. That is option B under another name,
  and #443 refuses option B outright.

## Decision 3 — `IAsyncEnumerable` is the new thing, `async` is not

Issue #572 records that "the API is asynchronous and `IAsyncEnumerable`-centred, which nothing in
this repository uses today." Half of that is stale and the half that stands is the expensive half.
`async Task` is already here — [`EmbeddingIndex.SaveAsync`](../reference/embeddings/search/embeddingindex-saveasync.md),
[`EmbeddingIndex.LoadAsync`](../reference/embeddings/search/embeddingindex-loadasync.md) and four
persistence loaders — so the asynchronous entry points are ordinary. **`IAsyncEnumerable<T>`
genuinely is absent**, and every `Get`/`Search` on this surface returns one.

It costs no new declared dependency: on `netstandard2.0` the type comes from
`Microsoft.Bcl.AsyncInterfaces`, which is already in the restore graph transitively, and
`Microsoft.Extensions.AI.Abstractions` brings it on that target in any case. What it costs is a
first idiom — how a synchronous in-memory index is yielded asynchronously without pretending to be
asynchronous — and the code that introduces it owns that.

## Consequences

- A fifteenth package, third of the interop tier, and the repository's sixth and seventh
  inter-package edges.
- `Lodestar.Text` must be released with `Lodestar.Text.Search` before this branch can be green.
  Releasing it is a separate act with its own tag, and this record is what says why it comes first.
- The three gates apply unchanged: a sample per public type, a reference page per public member,
  an equivalence row. The equivalence row says there is no Python reference for an interface
  adapter and that conformance is proven by driving the abstractions as a consumer would, so the
  suite does not read as skipped.
