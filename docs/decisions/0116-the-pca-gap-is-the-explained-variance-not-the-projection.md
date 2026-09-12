---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0076"]
---
# 0116 — The PCA gap is the explained variance, not the projection

**Status:** accepted · **Date:** 2026-09-12 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0076`](0076-a-core-package-carries-no-external-dependency.md)

## Context

`docs/equivalence.md:314` has carried the same row since the package shipped:

> `PCA`, `SparsePCA`, `DictionaryLearning` — no counterpart yet. The truncated SVD and NMF are
> what ships in 0.1.0. **PCA in particular is not the SVD with centring bolted on.**

That sentence is correct and is the reason the row exists: centring a `CsrMatrix` subtracts a
column mean from every stored zero, so PCA over sparse input is a different computation rather
than a preprocessing step in front of the one that ships. scikit-learn refuses it for the same
reason — `TruncatedSVD` exists precisely because `PCA` will not take a sparse matrix.

What the row did not say is whether .NET has PCA at all.
[`#685`](https://github.com/CyrilB1531/lodestar/issues/685) asked, and
[decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) says how: read the
exported surface, never the README.

## The reading

Run with `tools/survey.cs`, which [decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)
built so a reading is re-runnable rather than retyped. Both licences read from the artefact, per
[decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md).

```bash
dotnet run tools/survey.cs -- Microsoft.ML 5.0.0 'PrincipalComponent|Pca' --assembly Microsoft.ML.PCA
dotnet run tools/survey.cs -- NumFlat 1.3.4 'Pca|PrincipalComponent'
```

| | Microsoft.ML 5.0.0 | NumFlat 1.3.4 |
| --- | --- | --- |
| assembly | `Microsoft.ML.PCA.dll`, **6 exported types, 14 public members** | `NumFlat.dll`, 123 exported types, 938 public members |
| licence, from the package | MIT | **MIT**, `<license type="expression">` |
| `lib/` | **`netstandard2.0`** | **`net8.0` and nothing else** |
| projection | `PcaCatalog.ProjectToPrincipalComponents`, `RandomizedPca` | `MultivariateAnalyses.Pca`, `PrincipalComponentAnalysis.Transform` |
| the components | `PcaModelParameters.GetEigenVectors`, `.GetMean` | `.EigenVectors`, `.Mean`, `.InverseTransform` |
| **how much variance each explains** | **nothing. Fourteen members and not one of them** | `.EigenValues` |
| shape of the call | an `IDataView` estimator inside a pipeline | `Mat<double>`, dense |

Two things follow, and neither was guessable from the outside.

**NumFlat is maintained, MIT and complete — and cannot be reached below `net8.0`.** Its `lib/`
holds one folder. Every core package here ships `netstandard2.0`
([decision 0001](0001-target-framework.md)), so a .NET Framework, Mono or Unity caller has no
NumFlat at all.

**ML.NET does reach `netstandard2.0`, and cannot say how much variance a component explains.**
That was the surprise. The argument that looked strongest before the reading — *PCA is missing
below net8.0* — is false: `Microsoft.ML.PCA.dll` is in a `netstandard2.0` package and projects
perfectly well. What it will not tell you is `explained_variance_ratio_`, the number a scree plot
is made of and the one every "how many components?" decision is taken on.

## Decision

**`Lodestar.Decomposition` does not write a projection, and the gap it may fill later is the
explained variance.** The row in `docs/equivalence.md` stops saying *no counterpart* and starts
saying which of the two to reach for.

- **Sparse input stays with `TruncatedSvd`, and PCA is refused for it by name.** Not an omission:
  centring densifies, and a package whose subject is `CsrMatrix` offering an operation that
  destroys sparsity would be offering a trap. `docs/reference/decomposition/` says so where a
  reader looks for PCA.
- **Dense projection is delegated.** `Microsoft.ML` on any target this repository supports;
  NumFlat when a caller is on `net8.0` or above and wants a matrix API rather than a pipeline.
  Both get a `docs/migration/` row with the constraint that picks between them.
- **The explained variance is left open, scoped, and not taken here.** It is one lot — eigenvalues
  of the covariance of a centred dense block, the ratios, and the cumulative curve — and it has no
  incumbent on `netstandard2.0`. It waits for a caller, under
  [decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s rule
  rather than being written on the strength of a gap alone.

**`Nmf.Transform(X_new)` is deferred with it**, and for a reason of its own:
`docs/equivalence.md:325` already calls it *"a factorization with `H` held fixed rather than a
projection, so it is a solve and not a product"*. It is not part of the PCA question and it does
not become one by being adjacent in the table.

## Options that lost

- **Write a dense PCA here.** The honest case for it is real: explained variance below `net8.0`
  exists nowhere, and a framework-free call is what `README.md` already sells against ML.NET for
  metrics. Refused for *this* package rather than for ever — `Lodestar.Decomposition`'s stated
  subject is decompositions **over a `CsrMatrix`**, and a dense-only member would be the first
  thing in it that does not take one. Where the explained variance lands is a question for the lot
  that writes it, and putting it here by default would settle that question by accident.
- **Delegate to NumFlat alone.** Cleaner as a sentence, and wrong for a third of the targets this
  repository supports. A migration row naming one library that a `netstandard2.0` caller cannot
  install is a row that fails exactly the readers `docs/migration/` exists for.
- **Say "ML.NET has PCA" and close the row.** It would have been true and useless. The member
  count is what stops that: fourteen members, `GetEigenVectors` and `GetMean` among them, and no
  eigenvalue anywhere — a reader told "ML.NET has PCA" reaches for a scree plot and finds nothing.

## Consequences

- `README.md`'s incumbent table gains the reading for `Lodestar.Decomposition`, which previously
  said only *"not like-for-like"* against `ProjectToPrincipalComponents`. It is still not
  like-for-like, and now the row says what ML.NET does and does not report.
- `tools/survey.cs` gained an `--assembly` argument on the way, because it could not read this
  one otherwise: `Microsoft.ML` installs no `Microsoft.ML.dll`, its surface is spread over eight
  assemblies, and `Microsoft.ML.PCA` is not a package id anyone can install. Deriving the target
  from the package id made that surface unreadable — found on the tool's second use.
- The reopening condition is written down rather than implied: **a caller who needs the explained
  variance**. Two of the three refusals above turn on it, and a package that cannot say how much
  variance it captured is the finding, not the projection.
