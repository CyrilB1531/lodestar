# API cohesion before 1.0 in Abstractions, Decomposition and Preprocessing

**Issue:** [#1232](https://github.com/CyrilB1531/lodestar/issues/1232).
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The pre-1.0 review of `main` at `d228321f` found five inconsistencies across the three packages,
each a breaking change that is cheap only before 1.0: two enums for one concept, two option types
that were not records, an NMF default that was not scikit-learn's, fitted statistics a caller could
cast back to their array and edit, and three pieces of wasted work.

## Decisions

Taken with Cyril on 2026-09-27, one per finding.

1. **`SparseNorm` and `RowNorm` stay two enums.** `SparseNorm` is what the tf-idf and hashing
   vectorizers take, and scikit-learn's `TfidfTransformer` and `HashingVectorizer` constrain `norm`
   to `{'l1', 'l2', None}`: a `Max` member there would be a value every one of them refuses.
   `CsrMatrix.NormalizeRows` also mutates in place where `Normalizer` copies. `RowNorm`'s remarks
   now say so, replacing a reason about release order that was no longer true.
2. **`TruncatedSvdOptions` and `NmfOptions` become `sealed record`s**, with the hand-written
   equality `KMeansOptions` has, comparing `RandomMatrix` element by element and hashing its length.
   **`Seed` keeps its name and its `int`:** it drives this package's own generator, where the
   splitters' `long randomState` replays numpy's MT19937 and reproduces scikit-learn. Renaming it
   `RandomState` would promise a parity it does not have.
3. **`NmfOptions.Initialization` defaults to `NndSvda`.** scikit-learn 1.9.1's `init=None` resolves
   to `nndsvda` when `n_components <= min(n_samples, n_features)` and to `random` otherwise; the
   single-matrix `Nmf.Fit` refuses every other rank, so `nndsvda` is its answer at every rank it
   accepts. The old default, `nndsvd` with the multiplicative update, is the pair scikit-learn warns
   against. Measured on the reference page's 4 × 3 matrix: `NMF(2, solver='mu')` gives
   `components_[0, 0] = 0.055` and a transform of `[1.775, 0.599]`, which the fit now returns.
4. **Every fitted array is a read-only view.** The properties of the scalers, transformers,
   imputers, encoders, `FoldSplit`, `TrainTestSplit`, `TruncatedSvd`, `Nmf`,
   `PrincipalComponentVariance` and `QrDecomposition` hold an `Array.AsReadOnly` view built once
   in the constructor, so `(double[])scaler.Scale` throws instead of editing the fit. The property
   types do not change.
5. **The wasted work goes, measured before and after on one machine.**
   - `RandomizedSvd.Compute` forms `U = Q Û` only when the caller asks: NNDSVD does, and
     `TruncatedSvd.Fit` never read it.
   - `KBinsDiscretizer.BinOf` searches the interior edges by a branchless bisection —
     `np.searchsorted(edges[1:-1], value, side='right')` — where it scanned them in order.
   - `SparseColumns.RequireFinite` returns the consolidated matrix it had to build anyway, and the
     column statistics read that instead of scanning for duplicates again. `MaxAbsScaler`'s clipping
     transform consolidates its answer only when the check found a duplicate.

## Mechanics

`Lodestar.Decomposition` reaches `Lodestar.Abstractions` by project until the next publication,
as #1155, #1159 and #1161 did for their packages: the records and the new default are in the
unpublished project, and the net10.0 suite would otherwise run against 0.2.0's class. The
`ci.yml` exception and `check_nuspec_dependencies.py`'s `EXPECTED` floor follow.

## Rejected

- Merging the enums into `SparseNorm` with a `Max` member (decision 1).
- `long RandomState` on the decomposition options (decision 2).
- A bisection that branches on each comparison: measured 18 to 40 % *slower* than the linear
  scan at 5 and 64 bins, the mispredictions costing more than the comparisons saved. The
  branchless form, which adds `half` masked by the comparison, is faster at both, so no hybrid
  with a linear scan under some edge count is needed.
