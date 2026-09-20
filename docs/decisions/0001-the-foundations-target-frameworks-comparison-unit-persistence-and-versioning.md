---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0001 — The foundations: target frameworks, comparison unit, persistence and versioning

**Status:** accepted · **Date:** 2026-09-20

## Context

Four choices were made before the second package existed, and every package since has been
written under them: what a library compiles against, what a character-based algorithm counts as
one character, what a fitted model looks like on disk, and what a version number is a statement
about. They are merged here from `0001`, `0002`, `0011`, `0012` and `0055`, restated as what is
true today rather than as what each record found on the day it was written.

Each was a choice between live options. `net8.0` (LTS) was the brief's suggestion and
`netstandard2.0` was declined once before being taken. The comparison unit could have been the
UTF-16 unit, the code point or the grapheme cluster. Persistence could have been `BinaryFormatter`,
a third-party serializer or a hand-written reader, and the repository could have kept the single
repository-wide `<Version>` it started with.

## Decision

**Every package multi-targets `net10.0` and `netstandard2.0`, and the two builds expose the same
public API** (`0001`). `netstandard2.0` reaches equivalent behaviour through conditional
compilation, never a reduced surface; it widens reach to .NET Framework 4.6.1+, Mono, Xamarin and
Unity, which matters because a package on nuget.org has no identified consumer by definition.
Gaps are closed in a fixed order: PolySharp compile-time polyfills, then `System.Memory` 4.6.3 and
`System.Numerics.Vectors` 4.6.1 referenced only on that target, then a hand-written fallback in
`src/Shared/` compiled into every library under `Lodestar.Internal`, so no call site carries an
`#if`. Executables — tests, benchmarks — stay on `net10.0`: `netstandard2.0` is a contract, not a
runtime. The one deliberate behavioural split is [`VectorMath.Dot`](../reference/embeddings/search/vectormath-dot.md), `Vector<T>` on net10 and a
scalar loop below it; `R2` and `ExplainedVariance` add a lane-wise Neumaier sum whose two targets
are **not** guaranteed bit-identical, which is narrower than the bit-identity
[`Pooler.MeanPoolBatch`](../reference/embeddings/pooling/pooler-meanpoolbatch.md) asserts.

- **Drift.** `0001` says "the three libraries". There are eighteen packages now, seventeen of them
  on `net10.0;netstandard2.0` and `Lodestar.Gpu` on `net10.0;netstandard2.1`, because ILGPU
  publishes no `netstandard2.0` asset (`0101`, as amended by `0103`). "Three mirror test projects"
  is likewise eighteen: `dotnet test Lodestar.slnx -c Release` must report 36 assemblies.

**The comparison unit is the UTF-16 code unit by default, and the code point on request**
(`0002`). A Python `str` iterates code points and a .NET `string` iterates UTF-16 code units, so a
character outside the Basic Multilingual Plane occupies two positions here and one there — the
single largest source of divergence from the reference libraries. `TextElement.Utf16Unit` is `0`
and therefore the default: allocation-free, and in agreement with Python for all BMP text.
`TextElement.CodePoint` is offered on every affected algorithm and costs one pooled decode pass
per operand; it is the mode the oracle corpora were generated under and are replayed with, because
rapidfuzz and jellyfish work on code points. Grapheme clusters are deferred — they need
segmentation, they allocate, and no targeted Python library offers an oracle for them. Lone
surrogates are preserved at their unit value, like a Python `str`, rather than throwing.

**A persisted artifact is one versioned JSON document, written and read with `System.Text.Json`**
(`0011`). A loaded file is untrusted input, so `BinaryFormatter` and polymorphic deserialization
are out, and so is the `pickle.load` habit these APIs mirror. `System.Text.Json` is the **single
deliberate exception to the no-external-dependency rule**: in-box from `net8.0`, and taken as a
package pinned at 10.0.12 on `netstandard2.0` alone, declared by `Lodestar.Text` and
`Lodestar.Embeddings` — the two packages that ship artifacts — and by no other.
`tools/check_nuspec_dependencies.py` asserts exactly that per target framework, and
`THIRD-PARTY-NOTICES.md` records it. What the exception buys is not a dependency avoided but a
hand-written JSON parser with a security boundary running through it; the minimal protobuf reader
in `SentencePieceModelLoader` is the counter-example that shows the line is real, four wire types
against a frozen format with no in-box alternative at all.

The contract the format carries:

| element | rule |
| --- | --- |
| header | `$schema` names the artifact kind and `version` is numbered per artifact |
| unknown properties | rejected, naming the property — ignoring them loads a model nobody saved |
| a single double | shortest round-trippable form on net10, invariant `"G17"` on `netstandard2.0` |
| non-finite values | refused on write, and refused again on read of the raw-bits block |
| the idf vector | one base64 string of little-endian IEEE-754 bits, decoded by the same library |
| the vocabulary | stays a plain JSON array of strings: it is what a person reads |
| escaping | `JavaScriptEncoder.UnsafeRelaxedJsonEscaping`, since accented tokens are the ordinary case |
| bounds | vocabulary 1 000 000, token 1024 chars, JSON depth 32, 256 MiB total, array 1 000 000 |
| API | `Save(Stream)`, `Save(string)`, static `Load(…, ArtifactLoadOptions?)`, async counterparts |

Every count read from a file sizes a buffer, so every count is checked before use and exceeding a
limit is `InvalidDataException` naming both the limit and the value. A stream passed in is never
disposed; the `path` overloads own the `FileStream` they open.

**The artifact gets a binary sidecar only once a block can be ingested whole, and that condition
is the decision** (`0055`). Measured on the persistence corpus, 10 000 × 384 with ids: the `.npy`
block plus head is 15 469 135 bytes against the artifact's 20 589 007, **1.331× smaller**, and
[`EmbeddingIndex.Load`](../reference/embeddings/search/embeddingindex-load.md) at 11.834 ms against a sidecar floor of 5.847 ms is **2.02×** — so the size
argument `0011`'s own `#324 update` allowed is joined by a speed one, because the cost is the JSON
scan around the block and not the base64. But rebuilding the index vector by vector cost 17.973 ms,
**slower than the artifact it would replace**, so the order is fixed: bulk ingest first, sidecar
second, head stays JSON and stays the artifact's own. The precondition is now met —
[`EmbeddingIndex.FromBlock`](../reference/embeddings/search/embeddingindex-fromblock.md) and `FromOwnedBlock` shipped under `0056` — and the sidecar itself has
not.

**Each publishable package declares its own version in a sibling `Version.props`, and reaches a
sibling through a `PackageReference` on a published floor** (`0012`). A repository-wide `<Version>`
republished packages at a number describing no change in them. The file holds a *named* property,
`LodestarTextVersion` and its siblings, because three places need the number for three different
reasons: the csproj's own identity, the floor in `src/Directory.Packages.props`, and the version
`samples/` has just packed. The floor is **chosen, not tracked** — `Lodestar.Text` is pinned at
0.6.0 there while `src/Lodestar.Text/Version.props` declares 0.6.1 — which is what makes
`git clone && dotnet build` work with no pack step, and what makes raising it a semver decision.
A declared version never equals one already on the feed, and `tools/check_version_floor.py`
enforces that. `LodestarUseProjectRefs=true` is the local editing loop and is never set by CI,
which asserts the shipped path through evaluated MSBuild rather than by grepping for
`<ProjectReference`. Tags are `<PackageId>/v<Version>`; the tag says which declared version to
release and never sets it. A version tool — MinVer, Nerdbank.GitVersioning, GitVersion — was
declined because all three derive a number from git topology, and this number is a deliberate
statement about a public API.

- **Drift.** `0012` records "exactly one inter-project reference under `src/`"; there are sixteen
  edges now. Its rule that no `src/` project carries a `ProjectReference` has one documented
  exception, `Lodestar.Stats.TimeSeries` → `Lodestar.Stats.Regression`, held there until that
  package's 0.2.0 is published (`0133`, issue #671).

## Consequences

- **Both builds are executed, not merely compiled.** The `*.NetStandard.Tests` mirrors link the
  same test sources and pin `SetTargetFramework=netstandard2.0`, so a new test file is picked up by
  both automatically. `SetTargetFramework` does not cross a `PackageReference` — NuGet resolves
  package assets against the mirror's own framework — so a mirror pins every `Lodestar.*`
  dependency, transitive ones included, with its own `ProjectReference`;
  `NetStandardAssemblyGuardTests` asserts one fact per assembly it must prove it loaded.
- **The `net10.0` packages remain dependency-free**, so the README's claim holds where most
  consumers read it, with the `netstandard2.0` qualifier it now carries.
- **A cross-package change is two pull requests and a release**, not one branch: a change to
  `Lodestar.Text` is not seen by `Lodestar.Fuzzy` until it is published, which is exactly what a
  consumer gets. `CHANGELOG.md` stays one file with per-package headings.
- **The sample judges the packed packages only under an isolated `NUGET_PACKAGES`**, because the
  global packages folder is consulted ahead of every source and would otherwise serve the released
  assembly while appearing to validate the working tree.
- **The pre-rename names survive in one place that matters.** `0011` and `0012` were written when
  the project was `DataNet`; `DataNetUseProjectRefs` is `LodestarUseProjectRefs` today, but
  `ArtifactHeader.SchemaFor` still emits `datanet/<artifact>`. That string is part of the artifact
  contract — changing it stops every file already written from loading — so it is a name to
  recognise, not one to fix in passing.
- **`ArtifactLoadOptions` is still declared once per package**, in `Lodestar.Text.Persistence` and
  `Lodestar.Embeddings.Persistence`, rather than shared: sharing would need either an edge onto a
  published package that does not yet contain the type, or one public type in two assemblies,
  which is an ambiguous reference for anyone consuming both. The reading helpers behind it are
  shared, as `internal` types in `Lodestar.Internal.Persistence` under `src/Shared/Persistence/`.
