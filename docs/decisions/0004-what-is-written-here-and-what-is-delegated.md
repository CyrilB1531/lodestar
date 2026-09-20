---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0004 — What is written here and what is delegated

**Status:** accepted · **Date:** 2026-09-20

## Context

This project's thesis is a claim about .NET, and a claim about .NET has to be checked rather than
assumed. Nineteen records answered it one capability at a time — the tokenizer, the sparse
decomposition, the explained variance, the GLM, the forecast, the clustering algorithms, the
splitters, the kNN kernel — and a reader who wanted the axis read nineteen records and assembled
it. This record is the axis, with one row per capability decided.

**The method the nineteen converged on**, in the order it is applied:

1. **Check this repository first.** A roadmap once proposed a metrics phase that had already
   shipped.
2. **Read the incumbent's exported surface, never its README.** A `MetadataLoadContext` over the
   package's own assemblies, today `tools/survey.cs`, which names its counting basis. A search of
   NuGet alone closed two lots that a reading then reopened, and reopened two the search had left
   open.
3. **Read the licence from the package**, not from the repository, and record an empty search
   rather than omitting it.
4. **Name the target frameworks.** A library that installs only on `net8.0` answers nothing for the
   `netstandard2.0` half of what ships here.
5. **Date the reading.** Every verdict below was taken between 2026-08-30 and 2026-09-16, and each
   is a statement about what was published on its day.

Three of the nineteen corrected an earlier one, which is why the method has the shape it has.
`0074` replaced `0059`'s NuGet searches with a reading of what the packages export. `0129` withdrew
three absence claims this repository was publishing, amending `0105` and `0116`. `0140` measured
`0060`'s question on a named machine and reversed its direction. What follows states the corrected
truth, not the first one.

## Decision

**Native code is written only where .NET has no maintained equivalent at the reference's parity,
and no Python runs at runtime. Measured package by package, that gap is consistently the apparatus
around a computation rather than the computation.** The tokenizer loader and not its encoder, the
regression's inference table and not its coefficients, the time-series diagnostics and not the
forecast, sparse decomposition and not dense.

A capability is decided by three questions, in this order.

**Is there a free, permissively licensed, maintained .NET incumbent at the reference's parity, on
every framework this repository ships?** All five clauses bind. Accord's GLM is complete and
LGPL-2.1. `Numerics.NET` carries the time-series diagnostics and is commercial. NumFlat's PCA and
Gaussian mixture are MIT and install on `net8.0` alone. `cs-glm` has a real stack and cannot be
consumed as published. `Cortex.TimeSeries`' `ARIMA` returns a pure autoregression's coefficients
under an ARIMA name. Each is a different reason, and none of them is a judgement of quality.

**Is there a parity target that pins its own answer at the tolerance a corpus is compared at?** The
corpora agree at `1e-9`. A reference whose own optimisers disagree with each other by more than
that cannot be frozen: what a corpus would record is one optimiser's stopping point, and a correct
implementation reaching a better optimum would fail it. This is what separates the multinomial
logit from the ordered model, and `PanelOLS` from `MixedLM`, with no appeal to how useful either
is.

**Is there a caller?** A void that is real, provable and unclaimed is recorded and left. Publishing
later is always available; unpublishing is not.

| capability | verdict | why | from |
| --- | --- | --- | --- |
| `tokenizer.json` and `spiece.model` loading | written here | no .NET tokenizer reads the artefact a user has: every `Microsoft.ML.Tokenizers` 2.0.0 factory takes a vocabulary file, a `spiece.model` or constructed objects | `0068` |
| the encode kernels behind that loader | written here, and behind the incumbent | ordinary code losing to an available alternative is a defect to fix, not a reason to delegate the loader with it | `0068` |
| sparse truncated SVD and NMF over a `CsrMatrix` | written here | Math.NET's QR and SVD are dense and its sparse-SVD request has been open since 2013; parity is provable because Ω is an input rather than a seed | `0059`, `0072` |
| a portable seed for that draw | not written | reproducing MT19937 buys parity the explicit Ω already gives, and a wrong Mersenne Twister fails quietly | `0072` |
| dense PCA projection | delegated to `Microsoft.ML` on every target, NumFlat on `net8.0` and above | ML.NET projects from a `netstandard2.0` package and consumes a sparse vector in O(nnz); centring a `CsrMatrix` densifies, so this package refuses PCA over sparse input by name | `0059`, `0116` |
| the variance a component explains | written here | ML.NET's PCA exports fourteen members and no eigenvalue; the free `netstandard2.0` incumbent that does report it, Meta.Numerics under MS-PL, was found after the type had shipped against a frozen scikit-learn corpus it has never been held to | `0116`, as amended by `0129` |
| the ten hypothesis-test families | written here | Meta.Numerics carries eight of them on `netstandard2.0` under MS-PL, so the gap is not existence: it is agreement with scipy at `1e-9` through a frozen corpus, and a span API under Apache-2.0 | `0129` |
| the generalized linear model and its table | written here | Accord ships the whole thing under LGPL-2.1 and archived; ML.NET is binary logistic only, with no link function as a concept and its standard errors behind a native MKL dependency; `cs-glm` installs nothing | `0104` |
| HC0–HC3 robust covariances | written here, before the rest of the tail | the package sells the inference table, and a standard error computed under an assumption nobody checked is wrong and silent, where a missing family stops a fit visibly | `0115` |
| WLS, GLS, negative binomial, Gamma | written here, in that order, each opening when the one before ships | they are the same table under a different variance assumption; inverse Gaussian and Tweedie wait for a caller | `0115` |
| the multinomial logit | written here | Accord's is the one incumbent and is LGPL-2.1; `statsmodels`' analytic Newton reproduces itself to `2.1e-15`, so the corpus holds | `0136` |
| the ordered model | not written, void recorded | nothing in .NET fits one, and the reference defines neither score nor Hessian: its default stops `2e-4` short and even Newton moves `1.6e-8` when refitted from its own optimum | `0136` |
| regularised fits — lasso, ridge, elastic net | delegated to `Microsoft.ML`'s LBFGS trainers, with the penalty scaled by the row count | first-party, MIT, and it reaches the same optimum and the same selected variables; the reference publishes no inference table for a penalised fit, deliberately, so there is no apparatus left to add | `0137` |
| mixed and hierarchical models | not written, void recorded | no .NET package fits one, free or commercial, and the reference's five solvers disagree by `1e-5` on a coefficient and `3e-3` on the likelihood when a variance sits on its boundary | `0130` |
| time-series forecasting, seasonality and anomaly detection | delegated to `Microsoft.ML.TimeSeries` | first-party, MIT, maintained, 2.85 M downloads; writing an SSA forecaster beside it is the rewrite this project exists not to do | `0105` |
| the time-series diagnostics — ACF, PACF, Ljung-Box, ADF, KPSS, seasonal decomposition | written here, in `Lodestar.Stats.TimeSeries` | free and permissive, a partial autocorrelation, a stationarity test and a seasonal decomposition exist only in the unadopted `Cortex.TimeSeries`; they are their own package because ADF's lag search is an OLS, which `Lodestar.Stats` must not drag in | `0105` as amended by `0129`, `0133` |
| ARIMA, SARIMAX, state-space estimation | not written, void recorded | no free incumbent estimates one — `Cortex.TimeSeries`' ARIMA returns the pure autoregression's coefficients — and the reference's own optimisers part by `2.6e-4`, one of them reporting non-convergence | `0134` |
| vector autoregression | not written, and the one of the four that could be | `VAR(...).fit(p)` is equation-by-equation least squares on stacked lags and matches `numpy.linalg.lstsq` at `0.0`; no .NET package estimates one | `0134` |
| instrumental variables — 2SLS, LIML, two-step GMM | not written; no incumbent, writable at parity | seven .NET libraries, three commercial, export nothing that fits one; `linearmodels` 7.0 reproduces each closed form at `1e-15`. Iterated GMM is out: it moves with its tolerance at `6.6e-6` | `0135` |
| panel estimators — `PanelOLS`, `BetweenOLS`, `FirstDifferenceOLS`, `RandomEffects` | not written; no incumbent, writable at parity | the same reading; the small-sample factors are `linearmodels`' own and a corpus has to pin them rather than assume `statsmodels`' | `0135` |
| k-means | written here | ahead of both free incumbents on the machine below, and the only other free `netstandard2.0` k-means takes no seed and no starting centres | `0131` |
| DBSCAN | written here | the one free .NET DBSCAN is planar: its `Point` carries `X` and `Y` and nothing else | `0131` |
| agglomerative clustering | written here, with `Aglomera` as its measured incumbent | `Aglomera` covers every linkage on `netstandard2.0` and has had no release since 2020 — the "no *maintained* equivalent" this project writes against — and has never been compared with scikit-learn | `0131` |
| HDBSCAN | delegated to `HdbscanSharp` | MIT, `netstandard2.0`, released 2025 | `0131` |
| Gaussian mixture and k-medoids | delegated to NumFlat on `net8.0`; the mixture is a scoped gap below it | NumFlat covers both where most callers are, and k-medoids is not in scikit-learn at all | `0131` |
| `MiniBatchKMeans` | not written | its batches are drawn inside the fit and no parameter hands them in, so no run is reproducible from .NET | `0131` |
| spectral clustering | not written until an eigensolver exists | it needs the leading eigenvectors of an affinity Laplacian, which is not the small dense Gram matrix the explained variance solves | `0131` |
| `KFold`, `StratifiedKFold`, `train_test_split` | written here, first of the preprocessing lots | no .NET splitter reproduces scikit-learn's folds, the maintained first-party one cannot stratify at all, and `Lodestar.Conformal`'s guarantee is a property of how the split was made | `0132` |
| scalers, encoders and the imputer | written here | ML.NET has all of them and welds each to an `IDataView` and a column name; what is added is the same answer on an array, at the reference's category order and unknown-value handling | `0132` |
| SMOTE and imbalanced resampling | not written, void recorded | the emptiest square on the board and the least provable: the one NuGet hit wraps a framework abandoned in 2019, and no parameter hands in the draws | `0132` |
| the kNN dot kernel | written here | ahead of `TensorPrimitives` by 3–7× on one named machine and behind it by at most 23% on another, and the dot is 23–49% of a query on the two; delegation is off the table until a machine shows it ahead by more than this one shows it behind | `0060` as amended by `0140` |
| MinHash, LSH and SimHash | not written | the sketch, the banded index and the threshold-to-(b, r) solve all ship, in `MinHashSharp`, `SimhashLib` and LuceneSharp's `MinHashFilter` | `0059`, `0074` |
| BM25 over an in-memory `CsrMatrix` | written here | the Block-Max WAND top-k the lot was proposed for ships in `LuceneSharp.Core`; what does not ship is BM25 with no `Directory`, no `IndexWriter`, no codec and no NLP dependency | `0074` |
| keyword extraction — RAKE, TextRank, MMR | written here | a C# YAKE exists and is not written; a search for `rake textrank` returns zero packages | `0059`, `0074` |
| BK-tree | written here; VP-tree kNN delegated to `vptree` | `vptree` is generic over any metric and takes our distance kernels, and a search for `bktree` returns nothing that contains one | `0074` |

## Where each measurement holds

A ratio belongs to the machine and the window it was taken in, and to nothing else.

- **The kNN kernel, twice, in opposite directions.** On a hosted runner — 4 cores, AVX2, .NET
  10.0.11, three rounds of nine interleaved runs — `TensorPrimitives` was **1.09–1.23× ahead** on
  the dot and the dot was 49% of a query. On an **AMD Ryzen 7 8700G (AVX-512), Ubuntu 26.04.1, .NET
  10.0.12, pinned to one core**, five runs of nine, our kernel is **3–7× ahead** (dot ratio 0.14
  with the 512-bit path on, 0.32 with it off) and the dot is 23–33% of a query. Neither machine is
  declared the true one; both readings are why the kernel stays.
- **Clustering, one machine.** AMD Ryzen 7 8700G, default job: Lloyd's iterations from the same
  centres **1.52–3.66× ahead** of NumFlat's, and a whole default fit **5.5–13× cheaper** than
  NumFlat's and **4.1–20× cheaper** than Meta.Numerics'. Every pair agreed on the centres before it
  was timed.
- **The tokenizers, no machine.** `Microsoft.ML.Tokenizers` encoded the same artefacts in 54.33 ms
  against our 112.36 for WordPiece and 56.94 against 682.64 for SentencePiece, both sides returning
  identical ids. Those are **container timings and carry no machine, so the ratios are not
  published as speeds**; the allocation beside them — 3.55 MB against 118.84, 3.09 MB against
  519.51 — is a property of the code path and does not need one.
- **Agreement, which needs no machine.** Over the same Ω, a step-by-step reimplementation of
  `randomized_svd` reproduces `U`, `s` and `Vᵀ` at **exactly 0.0**. ML.NET's LBFGS trainers reach
  `statsmodels`' regularised optimum with the same zeros, `1.8e-5` apart because their weights are
  `float`. The two-sided normal p-value is `ChiSquaredSf(z², 1)` exactly, agreeing with scipy at
  `1.8e-16` at z = 0.5 and `1.1e-13` at z = 37.

## Consequences

- **A delegation is a promise to keep reading.** `Microsoft.ML.TimeSeries` has a 6.0.0 preview out;
  if it grows an ACF, the diagnostics' scope shrinks and this record is superseded rather than
  edited. That is the cost of delegating to something alive, and it is cheaper than duplicating it.
- **An absence claim is the fragile half of every row above.** Three of them were false when
  `0129` went looking, in documents a reader meets before any decision record —
  [`docs/migration/`](../migration/README.md), `README.md`'s incumbent table, a guide. A claim that
  nothing exists is corrected where it is published, not only here.
- **A commercial incumbent is named, never delegated to.** A migration row recommends what a reader
  can install without buying a licence; `Numerics.NET`, `Dew.Stats`, ILNumerics and NMath are named
  beside the subjects they cover so that an absence claim is not quietly true only of free
  software. Nothing is timed or fitted under a trial licence.
- **Every *not written* row carries its reopening condition**, and it is a caller rather than a
  gap: the ordered model, mixed models, SMOTE, ARIMA, VAR, IV and panel each name what the lot
  would be, and the two families whose reference is unpinnable also name the conformance question
  their caller inherits.
- **A verdict of *delegated* moves a row in [`docs/equivalence.md`](../equivalence.md) and in
  `docs/migration/`, in the same commit as the reading.** That is where a reader meets it.
