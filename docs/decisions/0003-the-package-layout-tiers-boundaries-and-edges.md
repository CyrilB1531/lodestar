---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0003 — The package layout: tiers, boundaries and edges

**Status:** accepted · **Date:** 2026-09-24

## Context

Seventeen records decided, one lot at a time, where a capability lives and what it may carry:
`0016` put the metrics outside the text package, `0071` moved `CsrMatrix` out from under it, `0076`
stated the tier rule, `0081` kept a numerical layer internal, `0089` opened the interop tier, `0095`
published four of that layer's members with `0097` and `0098` publishing a fifth and a sixth under
the same rule, `0096` gave ordinary least squares a package while `0111`, `0119` and `0124` refused
one to the generalized linear model, the explained variance and the Cox model, `0100` and `0101`
(amended by `0103`) added the last two satellites, and `0138` and `0139` took the two most recent
edges. Read singly they answer one lot each; read together they are one axis, and this record is
that axis.

**This is the record's second text, written in numbering epoch 3.** The first, dated 2026-09-20,
drew `Lodestar.Abstractions`' line by *exchange* — it held "the types packages exchange" — so a
public enum or options record no second package happened to name stayed where it was declared, and
an audit on 2026-09-24 over the net10.0 assemblies of `9f9406c5` found 94 such types across thirteen
packages. That line is replaced by (e) below. The same pass drops the table of edges the first text
carried: it stated sixteen while the graph held seventeen, and an immutable record cannot follow a
graph that grows. [#1103](https://github.com/CyrilB1531/lodestar/issues/1103) carries the rewrite;
the first text reads at `9f9406c5`.

`tools/check_nuspec_dependencies.py`'s `EXPECTED` is the authority on the shipped graph, because
`dotnet pack` derives a package's dependencies from what restore resolved rather than from anything
a human wrote. Each edge is asserted there per target framework and per version range, and
`CLAUDE.md`'s count follows it under `tools/check_claude_md_packages.py`.

## Decision

### (a) A capability earns its own package when its audience is distinct, and otherwise it is a namespace

**A split is licensed by a distinct dependency profile, a distinct audience or a distinct release
cadence, never by tidiness, and the audience test is the one that decides most lots: a package is
earned when a caller who wants the new capability wants none of what the neighbouring package
already holds.**

`0016` applied it before the vocabulary existed. A confusion matrix takes `int` labels and `double`
weights and would work identically for a model that never saw a string, so the metrics could not be
a namespace in the text package — and the split is cheapest at the moment a package has no users,
because moving a published type is a break for every consumer that referenced it. `0096` applied it
to ordinary least squares against ten hypothesis-test families: a caller who wants a regression
table wants none of them, and a caller running a Kruskal-Wallis wants no QR.

The same test refuses a package three times, which is what keeps it a test rather than a habit:

- `0111` measured the generalized linear model against all three criteria and found none distinct —
  the same zero external dependencies and the same two edges before and after, a caller who wants
  *more of the same* table rather than none of it, no cadence of its own, and an
  `Internal/LeastSquares.cs` that both halves call. It ships beside the OLS it was built next to.
- `0119` put the explained variance in `Lodestar.Decomposition` rather than `Lodestar.Preprocessing`,
  because the package's subject was already the decompositions this repository writes by hand —
  [`QrDecomposition.Householder`](../reference/decomposition/factorization/qrdecomposition-householder.md) takes a dense span and no `CsrMatrix` — and the adjacency argument
  for the pipeline package would have cost either a second copy of the Jacobi kernel or a whole edge
  for one member.
- `0124` kept the Cox model in `Lodestar.Survival`: its caller holds the same censored durations a
  Kaplan-Meier curve is fitted on, and usually fits the curve first.

### (b) Three tiers, and the tier is decided by what the package carries, not by what it does

**A core package carries no external dependency (`0076`). A satellite carries the one dependency
that is its whole reason to be a package, and is named for it. The interop tier is
`Lodestar.Extensions.<Dependency>`, and it may take a dependency a core package refused (`0089`).**

`0076` turned the tier table into a rule a script can check: `OnnxTextEmbedder` left
`Lodestar.Embeddings` for a `Lodestar.Onnx` named after `Microsoft.ML.OnnxRuntime`, so
`dotnet add package Lodestar.Embeddings` no longer restores a native runtime. The `netstandard2.0`
polyfills and `System.Text.Json` are not external dependencies for this rule: they are shims for
what is in-box on `net10.0`, so a package carrying them offers one API at two implementations rather
than a second thing to install.

`0089` settled the case where the two halves of the rule pull apart. `Lodestar.Decomposition`
refused `MathNet.Numerics` on freshness because it was choosing a dependency to **compute with**,
and a stale library there is a liability its users inherit without asking.
`Lodestar.Extensions.MathNet` takes the same dependency because it is choosing one to **convert
to**: its entire value is that the caller already holds those types, so refusing would protect
nobody. What a satellite may not do is reach for a stale dependency to *implement* something.

**A framework floor is a permission, not an obligation.** `Lodestar.Onnx`,
`Lodestar.Extensions.MathNet` and `Lodestar.Extensions.VectorData` all ship the default pair because
their dependencies do. `Lodestar.Gpu` is the one package that does not: ILGPU publishes **no**
`netstandard2.0` asset, which is an absence upstream rather than a gap a polyfill closes, and does
publish `netstandard2.1`. `0101` read the first fact and jumped to `net10.0` alone; **`0103` amended
it, and the shipped truth is `net10.0;netstandard2.1`** — the floor ILGPU itself declares, reaching
.NET 8, Mono and Unity, with a `tests/Lodestar.Gpu.NetStandard.Tests` mirror pinned to 2.1 so the
second assembly is executed rather than merely compiled. **Nothing under `src/` may depend on
`Lodestar.Gpu`**, so the SIMD path stays the only path a `netstandard2.0` caller has, and stays
complete.

### (c) A package publishes what a named caller outside it needs, and no more

**An internal layer opens one member at a time, each time a second package names it, because
publishing later is always available and unpublishing never is.**

`0081` kept `Lodestar.Stats`' numerical layer internal and refused a public `Lodestar.Stats.Special`
namespace: each member would become a parity promise in its own right, with its own reference page,
its own `wiki-map.json` row, its own use in the packaging sample and its own corpus at a tolerance a
general-purpose caller would need rather than the one ten tests happen to need. It wrote its own
escape hatch, and `0095` spent it — four members, [`Distributions.StudentSf`](../reference/stats/tails/distributions-studentsf.md),
[`Distributions.StudentQuantile`](../reference/stats/tails/distributions-studentquantile.md), [`Distributions.FisherSf`](../reference/stats/tails/distributions-fishersf.md) and [`QrDecomposition.Householder`](../reference/decomposition/factorization/qrdecomposition-householder.md), for the
OLS table. `0097` and `0098` **apply** that rule without amending it: [`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md)
for the log-rank test, and [`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md) for the Kaplan-Meier log-log bound after
the obvious substitute — a large-`df` Student quantile — was measured and floored at about `9e-9`,
which the transform pushes past the `1e-9` the corpora are compared at. `0124` is the same rule read
from the other side: the Cox table needed two tails, both already public, so nothing new was
published.

Publishing is also where a member's name is tested. Both quantiles turned out to be inverse
*survival* functions internally, returning the opposite sign to the printed tables; the published
members negate, which is the distribution's symmetry rather than a correction.

A function published this way stays in the package that owns it, and its neighbour takes an edge:
the numerical layer did not move to `Lodestar.Abstractions` (`0081`, generalised by `0095`), and (e)
says why nothing that computes ever does.

### (d) An edge is taken rather than a member copied, and it is declared in four places

**A member another `Lodestar` package publishes is depended on, not copied; where depending would
make a cycle, the member moves to `Lodestar.Abstractions` instead (`0138`, `0071`).**

`0071` is the move: `CsrMatrix` and `SparseNorm` left `Lodestar.Text` because a package wanting a
sparse matrix and two products would otherwise have taken the distances, the phonetics, the
stemmers, the tokenizers, the vectorizers, the persistence layer and `System.Text.Json` with it.
`0138` is the precedent for the ordinary case — `RobustScaler(unit_variance=True)` divides by a
normal quantile this repository already publishes and tests against `scipy`, and two copies drift.
`0139` applied it the same week for `CsrMatrix`, and recorded the thing worth recording: **a second
edge in the same package inside one lot needed no fresh argument, while a first edge from another
package still earns its own record.**

An edge costs four declarations: the `EXPECTED` entry in `tools/check_nuspec_dependencies.py`,
`CLAUDE.md`'s table, the `.csproj` — a `PackageReference` on a
**published** floor, a `ProjectReference` behind `LodestarUseProjectRefs` for the developer loop, and
its own `ProjectReference` in the `netstandard` mirror, because `SetTargetFramework` does not cross a
`PackageReference` — and the floor itself in `src/Directory.Packages.props`. Because the floor names
a published version, **the depended-on package ships before the consumer's next release, never
after**: a two-package lot is two pull requests with a release between them, which is what
`Lodestar.Survival` waited on twice.

**An edge is not a decision, and this record does not list them.** The first text did, and counted
itself among an edge's declarations, which left it wrong the day the seventeenth edge landed and
unable to follow any edge after. A new edge — a first one from its package or not — earns a record
only when it changes a rule stated here.

### (e) `Lodestar.Abstractions` holds the public data types, and no code

**`Lodestar.Abstractions` holds the public types, enums, interfaces and data-transfer objects the
packages declare, and no code: no logic, no validation, no numerical layer.** A type belongs there
when every member the compiler did not write is absent — no hand-written accessor, setter,
constructor body or method, no non-public member, and nothing it names that stays behind — with
one allowance: a structural `Equals`/`GetHashCode`, comparing the type's own collections by value,
together with the shared `ValueEquality` helper it calls, which the package then compiles. Interface
members may be abstract; none may have a body. The test runs on the IL, not on a reading of the
source, so it is a rule rather than a judgement.

**One exception by name: `CsrMatrix`, `SparseNorm`, the products `0071` moved with them and the
`ElementWise` helper that implements those products** stay the sparse primitive they are, code
included. `CsrMatrix.CreateUnchecked`, the factory `Lodestar.Text`'s vectorizers call to skip a
validation their output does not need, is public for the reason the next paragraph gives. No other
type in the package carries logic.

**`Lodestar.Abstractions` grants no `InternalsVisibleTo`** — to `Lodestar.Text` or to its own tests.
A package every other one depends on cannot also let one of them read its internals: the grant is
what forced the package to refuse the shared helpers (#440), and an internal a published consumer
was compiled against becomes a contract nothing versions.

**A type keeps its namespace and changes its assembly.** [`Lodestar.Stats.NanPolicy`](../reference/stats/nanpolicy.md) is compiled into
`Lodestar.Abstractions.dll` and is still `Lodestar.Stats.NanPolicy`; the package it left declares
`[assembly: TypeForwardedTo(typeof(NanPolicy))]`. An unchanged full name is what lets a forwarder
keep both binary and source compatibility, and it spares the three names two packages declare
apiece — `BinStrategy`, `MinHashScheme`, `ArtifactLoadOptions`. Only `CsrMatrix` and `SparseNorm`
live in the `Lodestar.Abstractions` namespace.

**What stays in its package:** a result with no public constructor, whose construction is the
algorithm's; a data type that validates in its setters or its constructor — [`OlsOptions`](../reference/stats-regression/ols/olsoptions.md) through
`OptionGuards`, until that validation is detached; a type with a computed member, or one that
reaches an internal of its package — [`TTestResult.ConfidenceInterval`](../reference/stats/tests/ttestresult-confidenceinterval.md)
through `Internal.Beta.StudentQuantile`, [`WordPieceVocabulary`](../reference/embeddings/tokenization/wordpiecevocabulary.md)'s
`Count`; and any type that names one of those.

Three alternatives lost:

- **Flattening into the `Lodestar.Abstractions` namespace.** A changed full name defeats a
  forwarder, so every moved type would break its callers at source and at run time, and three
  names would collide.
- **Keeping the exchange rule.** A type's home would depend on whether a second package had named
  it yet, and it would move the day one did.
- **Admitting data types that validate.** The package would carry code, and "no code" would stop
  being something the IL can check.

Merging or renaming the types two packages both declare is left to 1.0.

## The eighteen packages

The default pair is `net10.0;netstandard2.0`; only the exception is listed.

| package | tier | holds | target frameworks |
| --- | --- | --- | --- |
| `Lodestar.Abstractions` | core | the public data types the packages declare, under their own namespaces, and `CsrMatrix`, `SparseNorm` and the dense-block products — the sparse primitive the others share | |
| `Lodestar.Text` | core | distances, phonetics, set similarity, stemmers, tokenizers, sparse vectorizers, persistence, `BkTree`, keyword extraction, BM25 | |
| `Lodestar.Embeddings` | core | sub-word tokenizers, the batch encoding pipeline, pooling, the SIMD kNN `EmbeddingIndex`, `.npy` interop | |
| `Lodestar.Fuzzy` | core | `fuzz.*`, `process.extract`, blocking deduplication | |
| `Lodestar.Metrics` | core | classification, regression, clustering and ranking metrics | |
| `Lodestar.Conformal` | core | split conformal intervals and prediction sets | |
| `Lodestar.Decomposition` | core | truncated SVD and NMF over a `CsrMatrix`, the Householder QR, the variance principal components explain | |
| `Lodestar.Cluster` | core | k-means by Lloyd's algorithm over a row-major span | |
| `Lodestar.Preprocessing` | core | scaling, encoding, imputation and the cross-validation splitters, dense and sparse | |
| `Lodestar.Stats` | core | the hypothesis tests, and the tail members `0095`, `0097` and `0098` published for its neighbours | |
| `Lodestar.Stats.Regression` | core | ordinary, weighted and generalized least squares with the inference table, and the GLM `0111` kept here | |
| `Lodestar.Stats.TimeSeries` | core | the autocorrelation functions, Ljung-Box, augmented Dickey-Fuller, KPSS, seasonal decomposition | |
| `Lodestar.Survival` | core | Kaplan-Meier, Nelson-Aalen, the log-rank test and the Cox model `0124` kept here | |
| `Lodestar.Onnx` | satellite | `OnnxTextEmbedder`, and the reason the tier exists: `Microsoft.ML.OnnxRuntime` | |
| `Lodestar.Extensions.AI` | interop | the ONNX embedding path behind `IEmbeddingGenerator`; carries `Microsoft.Extensions.AI.Abstractions` | |
| `Lodestar.Extensions.MathNet` | interop | `CsrMatrix` to and from Math.NET's sparse matrix; carries `MathNet.Numerics` | |
| `Lodestar.Extensions.VectorData` | interop | an in-process `VectorStore` with hybrid search; carries `Microsoft.Extensions.VectorData.Abstractions` | |
| `Lodestar.Gpu` | satellite | ILGPU kernels over device-resident matrices and text; no `src/` project may depend on it | `net10.0;netstandard2.1` |

## The edges

The graph is `tools/check_nuspec_dependencies.py`'s `EXPECTED`, floors included, since an edge with
the wrong floor is a different edge; `CLAUDE.md` restates it and `tools/check_claude_md_packages.py`
holds the two together. Two edges carry a reason worth keeping here, because they are the ones the
rules above were written from: `Lodestar.Text` → `Lodestar.Abstractions` is `0071`'s move, and
`Lodestar.Preprocessing` → `Lodestar.Stats` is `0138`'s precedent for depending rather than copying.
Nothing takes an edge into `Lodestar.Gpu`, as (b) requires.

## Consequences

- `tools/check_nuspec_dependencies.py`'s `EXPECTED` remains the authority: an unexpected dependency
  fails as loudly as a missing one, and so does a moved version range. `CLAUDE.md`'s package table
  and edge count follow it, and `tools/check_claude_md_packages.py` fails when they drift.
- Counts in the merged records of the first numbering were right on their dates and are not today —
  `0076`'s layout table lists eight packages, `0100` calls `Lodestar.Extensions.VectorData` the
  fifteenth, `0111` concludes the repository stays at sixteen. This record states none, for the
  same reason.
- `0101`'s title and decision sentence — `Lodestar.Gpu` targets `net10.0` alone — is the claim `0103`
  amended, and this record states the amended truth. `netstandard2.0` is still absent upstream and
  still not offered.
- `0016` is written throughout in the repository's former `DataNet.*` naming; every package it names
  ships as `Lodestar.*`. Its reasoning is unaffected.
- A new edge, and a further member published from `Lodestar.Stats`' numerical layer to a named
  caller, are routine under (c) and (d); either needs a record only if it changes a rule here.
- (e) moves 94 types, and every package that declared one takes an edge to `Lodestar.Abstractions`,
  which therefore ships before each of them
  ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142)). From then on, changing a moved
  type is a two-package change, which is the price of the rule.
- A namespace now spans two assemblies, so whatever reads a namespace's types — the reference gate,
  the sample coverage check — reads both.
