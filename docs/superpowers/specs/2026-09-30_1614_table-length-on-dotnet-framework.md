# TableLength bounds an array as the runtime does, .NET Framework included

**Issues:** [#1614](https://github.com/CyrilB1531/lodestar/issues/1614).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

`src/Shared/TableLength.cs`, compiled into every package, bounded every array at `0x7FFFFFC7`, .NET
6's `Array.MaxLength`. The netstandard2.0 build also runs on .NET Framework — and `Lodestar.Gpu`'s
netstandard2.1 one on .NET Core 3.x and .NET 5 — which cap an array of elements wider than one byte
at `0x7FEFFFFF` (with `gcAllowVeryLargeObjects`; at 2 GB without it). A count between the two passed
every refusal built on the bound — `TableLength.Of`, `MathNetInterop.ToCsrMatrix`'s caller-storage
count, the growth of `EmbeddingIndex`, `CharTrie` and `HeldRecords`, and `Process.Cdist`, which
bounded its matrix at `int.MaxValue` — and then failed to allocate with `OutOfMemoryException` where
an `ArgumentException` is documented. Found by the Review A of
[#1607](https://github.com/CyrilB1531/lodestar/pull/1607); the impact is nearly nil, about 16 GB of
values.

## Decisions

- **A host that cannot describe itself takes the lower bound.** The description is read through a
  non-inlined call inside a `try`, so a facade that fails to load leaves the bound at `0x7FEFFFFF`
  rather than every package's `TableLength` a `TypeInitializationException`.
- **The bound is read from the runtime, once.** Review A found .NET Core 3.1 and .NET 5 capping
  arrays as .NET Framework does — the single bound came with .NET 6 — so only a
  `RuntimeInformation.FrameworkDescription` of ".NET" and a major version of 6 or more gives
  `0x7FFFFFC7`; every other runtime, legacy Mono ("Mono 6.x", Unity, classic Xamarin) included as
  unmeasured, gets `0x7FEFFFFF`; the Mono behind .NET 6 and later reports ".NET" and takes
  `Array.MaxLength`. The net10.0 build takes `Array.MaxLength` itself; only the netstandard2.0 build
  reads the description. The choice is a pure function, `TableLength.Bound`, so a test on .NET 10
  can pin the branches it cannot run.
- **Bytes keep the higher bound.** .NET Framework lets an array of one-byte elements reach
  `0x7FFFFFC7`, so `TableLength.MaxByteLength` bounds the byte buffers — the precompiled
  normaliser's UTF-8 output, the base64 block an `EmbeddingIndex` is loaded through, and the symbol
  block `Lodestar.Gpu` uploads a batch's characters as — where `MaxLength` would refuse a length the
  runtime allows.
- **One bound, not four.** `Lodestar.Text`'s `WideAlphabet.MaxTableLength`, `Lodestar.Survival`'s
  `ResultTable` and `CsrMatrix`'s dense and product bounds each spelled `0x7FFFFFC7` out again — the
  last with a remark conceding the Framework gap; all read `TableLength` now, which
  `Lodestar.Abstractions` takes on its own as it takes `Guard`. The persistence layer's
  `DefaultMaxSingleBuffer` bounds a byte buffer, already right on every runtime, and stays.
- **AgglomerativeClustering's pairwise distances are bounded too.** Review A found its condensed
  matrix, `n (n − 1) / 2` doubles, allocated unchecked: 65,537 rows passed `Array.MaxLength` and
  failed in the allocation. Under any linkage but single the rows are refused past the runtime's
  largest array; single linkage needs no such matrix, and is bounded by its merge tree of twice the
  rows less one, from about 1.07 billion rows — too many to pin in a test.
- **The products Review A swept for are bounded too.** `Spearman.Matrix`'s two variable-count
  squares, `MultinomialLogit`'s probabilities and Hessian, `VectorAutoregression`'s and the
  augmented Dickey-Fuller's lagged designs, and two-way `PanelRegression.FixedEffects`' dummies were
  `int` products allocated unchecked, which wrapped to `OverflowException` or passed
  `Array.MaxLength` and failed to allocate; each goes through `TableLength.Of`, blamed on the public
  parameter that sized it. So are the designs every regression forms — `LeastSquares.Design`, the
  panel's sorted regressors, the column of ones the panel's and the instrumental rank tests append,
  the response the within fits join beside the regressors, LIML's `[y, X_endog]` — where an appended
  column can push a design whose span fits past one array. `MultinomialLogit` checks its
  probabilities and its Hessian before copying the design, so the Hessian bound is reached by a
  46,343-row design and pinned by a test.
- **A refusal sits where its allocation failed, after every refusal `main` made.** Review A moved
  several bounds ahead of costly work and then found each move reordering a refusal `main` made
  cleanly — a non-finite covariate, an option, a collinear column, a period count — so the rule is
  that an input `main` refused keeps its message and its order, and only an input `main` crashed on
  gets a new refusal, at the allocation that crashed. Least squares bounds the design where its
  VIFs' QR fallback copies it, after the solve's collinearity refusal, and builds the robust
  covariance's design where it always did; generalized least squares bounds its covariance's copy
  where the Cholesky factor makes it; the AFT fit refuses a parameter count past one array where it
  builds its column indices and keeps the Hessian's square where #1311 put it; the survival fits and
  predictions bound their design's copy after the finiteness scan, the Cox fits where `CoxData`
  makes it; the panel bounds its regressors where it sorts them, the rank test's column of ones
  where the constant search appends it, and the joined block and the two-way dummies after its
  degree-of-freedom refusals.
- **A bound checked ahead of other refusals is checked twice.** Lowering the bound before .NET 6
  moves an input between the two bounds onto a check that `Nmf`, `TruncatedSvd`, `CalibrationCurve`,
  `Pooling.MeanPoolBatch` and `MathNetInterop`'s walk of a caller's storage run before their other
  refusals, so each keeps `TableLength.ArrayMaxLength`, `main`'s bound, where `main` checked it —
  with `main`'s message, printed bound included — and checks `MaxLength` once its other refusals
  have run.
- **What the bound counts is what gets allocated.** `OrdinaryLeastSquares.Estimate` forms its design
  on its own and takes the same bound; the augmented Dickey-Fuller bound counts the constant column
  the fit appends; the instrumental and panel column counts are summed in `long`, since with no rows
  every block length passes, as is `VectorAutoregression`'s parameter count per equation, which a
  lag order past `int` wrapped negative. The robust Cox covariance's risk sums lost the trailing
  zero row that made them one row longer than the design, rather than gaining a refusal. A panel
  with no rows is refused before its constant search reads past the end, and every panel estimator
  but a first difference, which refuses a constant first, is refused for want of rows before that
  search reads the regressors.
- **Shape checks compare in `long`.** `AcceleratedFailureTime`'s and `AalenAdditive`'s shared design
  check, which now also bounds the copy it makes, and `SplitConformal.LeastAmbiguousScores`'
  probabilities checked their length against an `int` product a large count wrapped;
  `Spearman.Matrix` checks its square before splitting out columns a count that large would allocate
  first, after a raised NaN as scipy orders them.
- **An input `main` passed through a wrapped `int` counts as one it crashed on.** A shape check
  comparing against an `int` product let a short input through when the product wrapped — 65,536
  variables squared, 65,536 labels by 65,536 classes, a covariate count wrapping the design's
  length, column counts summing negative — and `main` then refused it for whatever came next or
  failed; the corrected check refuses it where the wrapped one passed it, and a count `main` printed
  wrapped — the instrumental and the VAR column counts, `MultinomialLogit`'s parameter count,
  `SplitConformal`'s expected length — now prints the count itself. `Process.Cdist` keeps `main`'s
  "past int.MaxValue" past `int.MaxValue` and says "more than one array holds" only between the two
  bounds.
- **The 2 GB default stays the runtime's.** Without `gcAllowVeryLargeObjects`, .NET Framework
  refuses any array past 2 GB itself; no API reads that setting, so the README states it and the
  bound assumes the setting on.
- Rejected: **the lower bound everywhere**, which would refuse on .NET 10 arrays the runtime holds.
