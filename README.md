# Lodestar

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=CyrilB1531_data.net&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=CyrilB1531_data.net)
[![Lodestar.Text on NuGet](https://img.shields.io/nuget/v/Lodestar.Text?label=Lodestar.Text&color=004880)](https://www.nuget.org/packages/Lodestar.Text)
[![Lodestar.Embeddings on NuGet](https://img.shields.io/nuget/v/Lodestar.Embeddings?label=Lodestar.Embeddings&color=004880)](https://www.nuget.org/packages/Lodestar.Embeddings)
[![Lodestar.Stats on NuGet](https://img.shields.io/nuget/v/Lodestar.Stats?label=Lodestar.Stats&color=004880)](https://www.nuget.org/packages/Lodestar.Stats)
[![Targets net10.0 and netstandard2.0](https://img.shields.io/badge/targets-net10.0%20%7C%20netstandard2.0-512bd4)](docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
[![Apache-2.0](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)

**The data-science pieces .NET has no maintained library for, at the parity of the Python
library you already trust** — with no Python at runtime, on .NET 10 and .NET Standard 2.0 from one
package. Where .NET already has the answer, this project does not rewrite it: it tells you which
library to use.

## What it replaces

You arrived with an alternative in mind. Find it below: the row gives the number that settles the
choice and where to go next. A ratio means this library is that many times faster; each number's
machine, window and incumbent version are in [the performance guide](docs/guides/performance.md).

| you would reach for | to do | what settles it | go to |
| --- | --- | --- | --- |
| Fastenshtein, Quickenshtein | edit distance | Levenshtein **2.8× to 4.1×** Quickenshtein, the closest, **2.9× to 33.8×** Fastenshtein; allocates nothing | [from rapidfuzz](docs/guides/migrating-from-rapidfuzz.md) |
| FuzzySharp (the Raffinert fork) | `fuzz.*`, `process.extract` | all four ratios **1.78× to 12.75×**, allocating less on each | [from rapidfuzz](docs/guides/migrating-from-rapidfuzz.md) |
| ML.NET `FeaturizeText` | TF-IDF, count, hashing vectors | a **sparse** matrix at scikit-learn semantics instead of a dense vector inside an `IDataView`; **5.7× to 11×**, not like-for-like, since ML.NET adds character n-grams | [vectorization](docs/guides/vectorization.md) |
| ML.NET's binary evaluator | classification metrics | one call, one `double`: accuracy alone **87× to 322×**, ML.NET's whole bundle **1.57× to 4.84×** | [metrics](docs/guides/metrics.md) |
| `Microsoft.ML.Tokenizers` | a Hugging Face tokenizer | reads the `tokenizer.json` it has no loader for; on identical ids, **1.13× to 2.57×** | [embeddings](docs/guides/embeddings.md) |
| Accord.Statistics (archived; last release 2017), Meta.Numerics | `scipy.stats` tests | **1.18× to 5.94×** Meta.Numerics on all eight shared families at n = 10,000, exact p-values where it is asymptotic | [hypothesis testing](docs/guides/hypothesis-testing.md) |
| Accord.Statistics, Math.NET | regression inference | the whole statsmodels table, VIF included, which Accord does not export and Math.NET stops short of | [regression inference](docs/guides/regression-inference.md) |
| Cortex.TimeSeries | ADF, KPSS, decomposition | ADF **1.79× to 2.26×**, decomposition **1.85×**, KPSS level; a MacKinnon p-value where Cortex clamps at 0.01 | [time-series diagnostics](docs/guides/time-series-diagnostics.md) |
| NumFlat, Meta.Numerics | k-means | Lloyd's iterations **1.52× to 3.66×** NumFlat from the same centres, and runs on `netstandard2.0`, where NumFlat does not install | [scikit-learn](docs/migration/sklearn.md) |
| MAPIE or lifelines, through CSnakes or Python.NET | conformal intervals, survival | **no C# implementation of either exists**; this is one, with no Python runtime to ship | [conformal](docs/guides/conformal.md), [survival](docs/guides/survival-analysis.md) |

**It is not for you** if what you need is dense linear algebra (Math.NET Numerics), training a
model (ML.NET, TorchSharp), or a model that exists only as a Python package (CSnakes).
[`docs/migration/`](docs/migration/README.md) names the .NET library for every need this project
does not write, marks the ones no longer maintained, and says
[when calling Python is still the right answer](docs/migration/README.md#when-calling-python-is-still-the-right-answer).

## The packages

Eighteen packages, each installed, versioned and released on its own. Each one's README says what
it does, shows one call, and links its reference pages, its changelog and its measured
comparisons. **Core** packages carry no external dependency; a **satellite** carries the one
dependency that is its reason to exist; an **interop** package converts to another library's
types ([decision 0003](docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)).

| package | tier | what it does |
| --- | --- | --- |
| [`Lodestar.Abstractions`](src/Lodestar.Abstractions/README.md) | core | `CsrMatrix`, its products, and the public data types every other package declares |
| [`Lodestar.Text`](src/Lodestar.Text/README.md) | core | string distances, phonetics, stemmers, tokenizers, sparse vectorizers, BM25, keyword extraction |
| [`Lodestar.Embeddings`](src/Lodestar.Embeddings/README.md) | core | Hugging Face tokenizers without Python, batch encoding, pooling, SIMD nearest-neighbour search |
| [`Lodestar.Fuzzy`](src/Lodestar.Fuzzy/README.md) | core | rapidfuzz's `fuzz.*` and `process.extract`, and deduplication |
| [`Lodestar.Metrics`](src/Lodestar.Metrics/README.md) | core | classification, regression, clustering and ranking metrics at scikit-learn parity |
| [`Lodestar.Conformal`](src/Lodestar.Conformal/README.md) | core | split and cross-conformal intervals (CV+, Jackknife+) and prediction sets, at MAPIE parity |
| [`Lodestar.Decomposition`](src/Lodestar.Decomposition/README.md) | core | truncated SVD and NMF over a sparse matrix, the Householder QR, explained variance |
| [`Lodestar.Cluster`](src/Lodestar.Cluster/README.md) | core | k-means, DBSCAN and agglomerative clustering at scikit-learn parity |
| [`Lodestar.Preprocessing`](src/Lodestar.Preprocessing/README.md) | core | scalers, encoders, imputers and the cross-validation splitters at scikit-learn parity |
| [`Lodestar.Stats`](src/Lodestar.Stats/README.md) | core | classical hypothesis tests at scipy.stats parity |
| [`Lodestar.Stats.Regression`](src/Lodestar.Stats.Regression/README.md) | core | least squares, GLM and multinomial logit with statsmodels' whole inference table; instrumental variables and panel regression at linearmodels parity |
| [`Lodestar.Stats.TimeSeries`](src/Lodestar.Stats.TimeSeries/README.md) | core | ACF, PACF, Ljung-Box, ADF, KPSS, seasonal decomposition and VAR, at statsmodels parity |
| [`Lodestar.Survival`](src/Lodestar.Survival/README.md) | core | Kaplan-Meier, Nelson-Aalen, the log-rank family, the concordance index, Cox regression, Aalen's additive model and the parametric and accelerated failure time models, at lifelines parity |
| [`Lodestar.Onnx`](src/Lodestar.Onnx/README.md) | satellite | an ONNX encoder run in-process, pooled into a sentence embedding |
| [`Lodestar.Gpu`](src/Lodestar.Gpu/README.md) | satellite | ILGPU kernels over device-resident embeddings, matrices and text |
| [`Lodestar.Extensions.AI`](src/Lodestar.Extensions.AI/README.md) | interop | the ONNX embedding path behind `IEmbeddingGenerator` |
| [`Lodestar.Extensions.MathNet`](src/Lodestar.Extensions.MathNet/README.md) | interop | `CsrMatrix` to and from Math.NET's sparse matrix |
| [`Lodestar.Extensions.VectorData`](src/Lodestar.Extensions.VectorData/README.md) | interop | an in-process `Microsoft.Extensions.VectorData` store with hybrid search |

## Getting started

```bash
dotnet add package Lodestar.Text
```

```csharp
using Lodestar.Text.Distances;

Levenshtein.Distance("kitten", "sitting");             // 3
Levenshtein.NormalizedSimilarity("kitten", "sitting"); // 0.5714…
```

Full guide: [`docs/guides/quickstart.md`](docs/guides/quickstart.md). The guides linked in the
table above each start from the Python call you know. Function by function, the reference pages
under [`docs/reference/`](docs/reference/text/distances.md) say what each member is for, when to
prefer it to its neighbour and what the trap is; the same pages are published to
[the wiki](https://github.com/CyrilB1531/data.net/wiki), where each package's channel follows
`main` and every release is archived under its own version.

## Why not just call Python?

[CSnakes](https://github.com/tonybaloney/CSnakes) and
[Python.NET](https://github.com/pythonnet/pythonnet) both work, both are maintained, and for a
model that only exists as a Python package they are the right answer. What they cost is a Python
runtime to deploy and version alongside the application, no ahead-of-time compilation to a single
artifact, and the GIL between your threads and theirs. Where a .NET library will do, that is a poor
trade, and this project exists to make it an avoidable one.

## Why the gap is where it is

Measured package by package, what .NET lacks is almost never the computation and almost always the
**apparatus around it**: the tokenizer loader and not its encoder, the regression's inference
table and not its coefficients, the time-series diagnostics and not the forecast, sparse
decomposition and not dense. [Decision 0004](docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
decides each case, and its reading is what the table above rests on:

- **Ordinary least squares is everywhere; its inference is not.** Meta.Numerics (MS-PL) reports
  the standard error, the interval and the F test and stops there; the commercial Numerics.NET
  exports the whole table.
- **Split conformal prediction** — an interval instead of a point, a set instead of a class, with
  a finite-sample coverage guarantee — had no C# implementation at all in the survey behind
  [#441](https://github.com/CyrilB1531/lodestar/issues/441). The guarantee assumes exchangeable
  calibration and test data, which [the guide](docs/guides/conformal.md#exchangeability) leads
  with.
- **Right-censored survival** was the largest void
  [#442](https://github.com/CyrilB1531/lodestar/issues/442) surveyed. `scikit-survival` is the
  nearest reference in any language and is refused on its **licence**, not its capability
  ([decision 0002](docs/decisions/0002-provenance-and-the-allowed-references.md)).
- **Distances, embeddings and fuzzy matching** are here for pipeline coherence rather than because
  .NET is empty — it is not, and the first table says by how much.

The `netstandard2.0` build reaches .NET Framework 4.6.1+, Mono, Xamarin and Unity with the same
public API ([decision 0001](docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)).

## Measured against the .NET incumbents

Every package with an in-process incumbent is benchmarked against the .NET library a reader would reach
for, and both sides are checked to return **the same answers** before either is timed —
[`bench/README.md`](bench/README.md) has the harness and the agreement checks. The rows the first
table does not carry:

| package | incumbent | how it reads |
| --- | --- | --- |
| `Lodestar.Text`, `Bm25Index` | LuceneSharp.Core | Same ranking. The query is 1.6× faster at 1,000 documents and 1.4× slower at 20,000; from raw text Lucene is ahead, 2.1× to 2.7× ([performance](src/Lodestar.Text/performance.md#bm25-against-lucenesharp-issue-677)) |
| `Lodestar.Decomposition` | ML.NET `ProjectToPrincipalComponents`; NumFlat; Meta.Numerics | **Not like-for-like** against ML.NET, whose PCA is dense, centred and reports no eigenvalue. The explained variance is 0.83× to 1.36× NumFlat (`net8.0` only) and 36.7× to 809× Meta.Numerics, which refuses a matrix wider than it is tall ([performance](src/Lodestar.Decomposition/performance.md)) |
| `Lodestar.Cluster` | NumFlat, `Dbscan`, `Aglomera` | DBSCAN 2.47× to 10.92×, agglomerative clustering 24× to 590× ([performance](src/Lodestar.Cluster/performance.md)) |
| `Lodestar.Preprocessing` | ML.NET's splitters, normalizers and encoders | Ahead on every row where ML.NET's lazy result is read back, 1.46× to 191×; the lazy call alone is cheaper on the larger splits and the 20,000-row one-hot fit ([performance](src/Lodestar.Preprocessing/performance.md)) |
| `Lodestar.Stats.Regression` | Accord.Statistics; Math.NET Numerics | The GLM is level with Accord at 200 rows and 1.4× behind at 2,000; weighted and generalized least squares are level or ahead of Math.NET while computing the whole table ([performance](src/Lodestar.Stats.Regression/performance.md)) |
| `Lodestar.Onnx`, `Lodestar.Extensions.AI`, `Lodestar.Extensions.MathNet` | — | **Nothing to beat.** Each calls or adapts another library, so what it could be slower than is its own conversion |
| `Lodestar.Extensions.VectorData` | the `Microsoft.Extensions.VectorData` connectors | **Not measured.** Of the connectors surveyed, the ones implementing hybrid search are clients of a server, which an in-process store does not race |
| `Lodestar.Gpu` | — | Measured against this repository's own CPU paths, and each kernel ships only where it passed that gate ([performance](src/Lodestar.Gpu/performance.md)) |

## Parity with the Python reference

Conformance is **proven, not assumed**. Every algorithm replays reference values frozen from the
canonical Python library — rapidfuzz, jellyfish, textdistance, difflib, scikit-learn, scipy,
statsmodels, lifelines, MAPIE, nltk, HuggingFace `tokenizers`, sentencepiece, numpy, ONNX Runtime —
into `tests/oracles/*.json`, compared at `1e-9` for floats and exactly for strings. Python is a
development dependency only. [`docs/equivalence.md`](docs/equivalence.md) maps each Python call to
its C# counterpart, and every deliberate divergence is a record in
[`docs/decisions/`](docs/decisions/README.md).

## Developing

```bash
dotnet build Lodestar.slnx -c Release   # both target frameworks; warnings are errors
dotnet test Lodestar.slnx -c Release    # replays the oracles, on both
```

The project follows **GitHub flow**: `main` is always releasable, and every change arrives through
a short-lived branch and a pull request. Branch conventions, the definition of done, the
oracle-validation procedure and the analyzer policy are in [`CONTRIBUTING.md`](CONTRIBUTING.md);
release history is in [`CHANGELOG.md`](CHANGELOG.md).

A runnable sample, consuming the packages exactly as you would:

```bash
for p in src/Lodestar.Abstractions src/Lodestar.Text src/Lodestar.Embeddings \
        src/Lodestar.Fuzzy src/Lodestar.Metrics src/Lodestar.Conformal \
        src/Lodestar.Decomposition src/Lodestar.Onnx src/Lodestar.Extensions.AI \
        src/Lodestar.Extensions.MathNet src/Lodestar.Extensions.VectorData \
        src/Lodestar.Cluster src/Lodestar.Preprocessing \
        src/Lodestar.Stats src/Lodestar.Stats.Regression src/Lodestar.Stats.TimeSeries \
        src/Lodestar.Survival \
        src/Lodestar.Gpu; do
  dotnet pack "$p" -c Release -o ./artifacts
done
NUGET_PACKAGES=$(mktemp -d) dotnet run -c Release --project samples/Lodestar.Sample
```

The isolated `NUGET_PACKAGES` is not decoration: the global packages folder is consulted ahead of
any source, so a machine that has ever restored a published `Lodestar.*` at one of these versions
runs the sample against **that** rather than against what `pack` just produced — see
`CONTRIBUTING.md`'s Definition of done. On PowerShell the
same isolation is two lines, `$env:NUGET_PACKAGES = (New-Item -ItemType Directory -Path (Join-Path $env:TEMP (New-Guid))).FullName`
before the `dotnet run`, and `Remove-Item Env:NUGET_PACKAGES` after it.

## Structure

What each package holds, and which it depends on, is `CLAUDE.md`'s
[architecture table](CLAUDE.md#architecture); which document carries which fact is its
[*Where a fact belongs*](CLAUDE.md#where-a-fact-belongs).

```text
Lodestar.slnx
├── src/Lodestar.Abstractions/              CsrMatrix, SparseNorm and the packages' public data types (no dependencies)
├── src/Lodestar.Text/                      distances, similarity, tokenizers, vectorizers, stemmers
├── src/Lodestar.Embeddings/                sub-word tokenizers, pooling, SIMD kNN
├── src/Lodestar.Fuzzy/                     fuzz.*, process.extract, deduplication
├── src/Lodestar.Metrics/                   confusion matrix, precision/recall/F1, report, ROC-AUC
├── src/Lodestar.Conformal/                 split and cross-conformal intervals and prediction sets
├── src/Lodestar.Decomposition/             truncated SVD, NMF, the Householder QR, and PCA explained variance
├── src/Lodestar.Cluster/                   k-means by Lloyd's algorithm over a row-major span
├── src/Lodestar.Preprocessing/             feature scaling fitted on arrays and applied to spans
├── src/Lodestar.Stats/                     classical hypothesis tests, at scipy.stats parity
├── src/Lodestar.Stats.Regression/          ordinary, weighted and generalized least squares with the inference table
├── src/Lodestar.Stats.TimeSeries/          autocorrelation, Ljung-Box, ADF, KPSS and seasonal decomposition
├── src/Lodestar.Survival/                  Kaplan-Meier, Nelson-Aalen, log-rank, Cox, Aalen and parametric models
├── src/Lodestar.Onnx/                      ONNX inference — satellite, carries Microsoft.ML.OnnxRuntime (decision 0003)
├── src/Lodestar.Gpu/                       ILGPU kernels — satellite, the one package on net10.0;netstandard2.1
├── src/Lodestar.Extensions.AI/             interop: the ONNX embedding path behind IEmbeddingGenerator
├── src/Lodestar.Extensions.MathNet/        interop: CsrMatrix to and from Math.NET's sparse matrix
├── src/Lodestar.Extensions.VectorData/     interop: an in-process Microsoft.Extensions.VectorData store with hybrid search
├── tests/                                  xUnit: two projects per package — net10.0, and a mirror linking the same sources against netstandard2.0
├── tests/oracles/                          frozen JSON corpora (generated from Python) + a synthetic ONNX model
├── bench/Lodestar.Text.Benchmarks/         BenchmarkDotNet: every non-netstandard benchmark, whatever package it measures
├── bench/Lodestar.NetStandard.Benchmarks/  the netstandard2.0 assemblies, measured on the same host
├── tools/generate_oracles.py               reference generation
├── Directory.Build.props                   (root); src|tests/Directory.Packages.props (central package management)
├── src/*/Version.props                     one version per publishable package (decision 0001)
├── src/*/README.md, CHANGELOG.md, performance.md   each package's own page, history and measured comparisons
├── docs/                                   guides, equivalence table, decision log
├── docs/reference/<package>/               one reference entry per exported type and public method
└── docs/wiki-map.json                      which page ships with which package, and which namespaces the reference gate enforces
```

## Publishing

Eighteen NuGet packages are produced: `Lodestar.Abstractions`, `Lodestar.Text`,
`Lodestar.Embeddings`, `Lodestar.Fuzzy`, `Lodestar.Metrics`, `Lodestar.Conformal`,
`Lodestar.Decomposition`, `Lodestar.Cluster`, `Lodestar.Preprocessing`, `Lodestar.Stats`,
`Lodestar.Stats.Regression`, `Lodestar.Stats.TimeSeries`, `Lodestar.Survival`, `Lodestar.Onnx`, `Lodestar.Gpu`,
`Lodestar.Extensions.AI`, `Lodestar.Extensions.MathNet` and `Lodestar.Extensions.VectorData`.
Thirteen are **core tier** and carry no
external dependency — [`decisions/0003`](docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md).
`Lodestar.Onnx` and `Lodestar.Gpu` are the two **satellites**, each carrying the one dependency
that is its whole reason to be a package; the three `Lodestar.Extensions.*` are the **interop** tier,
which [`decisions/0003`](docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
allows a dependency a core package refused, because converting to a foreign type is not computing
with it.
**Each versions and releases on its own**: shared metadata
(license, icon, repository) lives in `Directory.Build.props`, while the version
is declared per project in `src/<Package>/Version.props` and each package ships its own
`src/<Package>/README.md`, with a release-notes link to its own `CHANGELOG.md`. `Lodestar.Fuzzy` depends
on `Lodestar.Text` as a published package, not as a project reference — see
[`docs/decisions/0001`](docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md).

**`main` carries the next revision rather than the published one.** A package released at
`0.2.0` reads `0.2.1` in its `Version.props`, so every branch packs and every sample restores a
number nuget.org does not hold — a version on the feed is immutable, and a collision would make two
different assemblies answer to one identity. A feature pull request therefore never touches
`Version.props`: it lands on a number already ahead of the feed.

To cut a release, set that file to the version being cut — the number `main` already carries when
the release is a revision, a larger one when the change earns a minor or a major — and land it on
`main`. Turn `## [Unreleased]` into `## [<version>] — <date>` in the package's own
`src/<Package>/CHANGELOG.md` (the root [`CHANGELOG.md`](CHANGELOG.md) lists them), in the shape
[`CONTRIBUTING.md`](CONTRIBUTING.md#definition-of-done)'s item 7 sets. Then tag. Afterwards, close
the release issue by bumping the revision again, which puts `main` back ahead of the feed.

**GitHub Packages** (no nuget.org account needed — uses GitHub's automatic token).
Bump the version, then tag it with the package name. The
[`release`](.github/workflows/release.yml) workflow packs and publishes that
package alone:

```bash
# 1. src/Lodestar.Fuzzy/Version.props declares the version being cut — already true
#    for a revision; edit, commit and merge to main for a minor or a major
# 2. tag the released version — <PackageId>/v<Version>
git tag Lodestar.Fuzzy/v0.3.0
git push origin Lodestar.Fuzzy/v0.3.0
```

The tag does not set the version; it names which declared version to release. The
workflow refuses the job if the tag and `Version.props` disagree. Repository-wide
`v*` tags are retired — there is no single version left for one to designate.

**Step 1 is what decides the number.** A revision needs no edit — `main` already carries the next
one — but a minor or a major does, and tagging before that edit gives a tag the workflow refuses,
because it disagrees with the version `Version.props` declares. Re-tagging a version the feed
already holds is rejected rather than absorbed: the workflows do not pass `--skip-duplicate`, which
used to report that case as a successful release that shipped nothing. That a declared version is
still off the feed is checked directly in CI by `tools/check_version_floor.py`.

To consume them, add a source pointing at the owner's feed (with a GitHub token
that has `read:packages`):

```bash
dotnet nuget add source "https://nuget.pkg.github.com/CyrilB1531/index.json" \
  --name github --username CyrilB1531 --password <GITHUB_TOKEN>
dotnet add package Lodestar.Text
```

**nuget.org** uses Trusted Publishing (OIDC, no stored key): run the
[`Publish to nuget.org`](.github/workflows/release-nuget-org.yml) workflow from
the Actions tab, choosing the package and confirming its version. By hand, with
an API key, one package at a time:

```bash
dotnet pack src/Lodestar.Text -c Release -o artifacts
dotnet nuget push "artifacts/Lodestar.Text.*.nupkg" \
  --source https://api.nuget.org/v3/index.json --api-key <KEY>
```

## License

[Apache-2.0](LICENSE). See [`NOTICE`](NOTICE) and
[`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md) for attributions. The license
choice and the code-provenance rule are documented in
[`docs/decisions/0002-provenance-and-the-allowed-references.md`](docs/decisions/0002-provenance-and-the-allowed-references.md).

*This repository is not legal advice.*
