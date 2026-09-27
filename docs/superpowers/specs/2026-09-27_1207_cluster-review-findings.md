# Lodestar.Cluster review findings before 1.0

**Issues:** [#1207](https://github.com/CyrilB1531/lodestar/issues/1207),
[#1208](https://github.com/CyrilB1531/lodestar/issues/1208).
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The pre-1.0 review of `main` at `d228321f` found three defects in `Lodestar.Cluster`:

1. `Dbscan.FitPrecomputed` took a negative distance as within every radius, where scikit-learn's
   `check_non_negative` raises `Negative values in data passed to X`. A malformed matrix clustered
   silently.
2. The weighted overloads named their parameter `sampleWeights`, the only four such parameters
   against 125 `sampleWeight` in `src/`, and `KMeans.Fit` placed it second where `Dbscan` and
   `Lodestar.Metrics` place it after the required arguments.
3. `KMeans.Iterations` differed from `n_iter_` by one on 8 of 300 fuzz cases.

## Decisions

1. **A negative distance is refused** with an `ArgumentException` naming `distances`, after the
   finiteness check both precomputed overloads share. A negative zero passes, as it passes
   `check_non_negative`.
2. **`sampleWeight`, after the required arguments and before `options`**:
   `KMeans.Fit(samples, featureCount, clusterCount, sampleWeight, options)`. The weighted overloads
   are unreleased (#1163), so nothing published breaks.
3. **Lloyd runs on the samples minus their column means**, starting centres included, and the
   means are added back to the centres — `KMeans.fit`'s own order, the tolerance still scaled on the
   uncentred samples as `_check_params_vs_input` scales it. Replaying scikit-learn 1.9.1's
   `lloyd_iter_chunked_dense` centred and uncentred over the reviewer's 300 cases changes `n_iter_`
   on seven, and those are seven of the eight mismatches: on case 579 the second shift is `3.6e-31`
   centred and `0` uncentred, so under `tol=0` the reference runs a third iteration and this did
   not. After the change one mismatch is left, case 710, whose first assignment holds an exact tie
   — decision 0007's divergence. k-means++ still draws on the uncentred samples, so a seeded fit
   picks the same rows it always did.

## Verification

- A frozen case, `a zero shift only the centred samples reach`, found by search among five-row
  fits with no exact tie, no distance within `1e-6` of a tie and no empty cluster: the reference
  stops after two iterations, uncentred the shift is `7.9e-31` and a third ran.
- The reviewer's fuzz harness, replayed against the build: iteration mismatches 8 → 1, label
  mismatches unchanged at 8, all on ties.

## Rejected

- **Computing distances in the reference's expanded form** `‖c‖² − 2x·c`. It would move the tie
  cases, not settle them — decision 0007 has two configurations sending scikit-learn's choice both
  ways — and it would cost the exact per-lane distances the SIMD assignment is built on.
