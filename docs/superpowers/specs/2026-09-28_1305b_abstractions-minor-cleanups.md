# The array remarks, `LogRank.Pairwise`'s refusals and a QR that is numpy's

**Issues:** [#1305](https://github.com/CyrilB1531/lodestar/issues/1305).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

Issue #1305 collected what the invariant sweeps of `Lodestar.Abstractions`, `Lodestar.Survival` and
`Lodestar.Decomposition` found below the stop rule they then had, which no longer exists:

1. Twenty public array members of eight records said nothing of who owns them; `KMeansOptions`'
   two made twenty-two in nine.
2. `LogRank.Pairwise`'s `ArgumentException` read "as `MultiGroup`", whose refusals now include a
   groups-by-groups table a pair never builds.
3. `QrDecomposition.Householder` on `[[1, ∞], [1, 1], [2, 3]]` returned `R[1,1] = NaN` where
   `numpy.linalg.qr` returns `+∞`: the pivot was computed by reflecting the column onto itself,
   `∞ − ∞`.

## Decisions

- **One remark per record, on the type or the property, and on its page**: the arrays are taken
  and exposed as they are, not copied, so a write reaches the record, what it equals and every
  holder of the same array, a `with` copy included. The hash reads lengths only, so it does not
  move, which the test pins beside the equality. Review A added `KMeansOptions`' two members, which
  the Abstractions sweep after #1325 found the same way.
- **`Pairwise` states its own refusals**: the pair's durations table, never the square one.
- **The QR is LAPACK's `dgeqr2` and `dorg2r`**, rather than a patch to the one entry reported.
  Setting the pivot to `β` fixes that entry; the rest of numpy's non-finite pattern follows from
  `dlarfg`'s `v₀ = 1` scaling and `τ`, `dlarf`'s trimming of `v`'s trailing zeros and of the
  trailing zero columns — which is what keeps a NaN `τ` from reaching entries the reflector leaves
  alone — and `dorg2r` setting each column of Q rather than reflecting the identity. With those,
  finite inputs of full rank gain numpy's signs too, and `docs/equivalence.md` no longer asks a
  reader to compare up to sign; past a vanished pivot a column of Q is rounding noise on both sides. The norm is a plain sum of squares, rescaled only where it overflowed or may have
  underflowed, and `+∞` on an infinity as OpenBLAS's `dnrm2` gives.

## Rejected

- **Refusing a non-finite matrix.** numpy answers one, and the project aligns with the reference.
- **Setting the pivot alone.** It fixes the reported entry and leaves other NaNs where numpy has
  finite values: `[[∞, 1], [1, 1], [2, 3]]` keeps `R[1,1] = −3.162` only through `dlarf`'s trims.

## Verification

- `decomposition_qr_numpy.json`, new: 62 matrices, a third holding `±∞` or NaN, others an exact zero
  column or scaled by `1e200` or `1e-300`, with numpy's Q and R, asserted entry by entry; the corpus
  regenerates within `1e-9` under OpenBLAS's Haswell, Sandybridge, Prescott, SkylakeX and Zen
  kernels, with the same non-finite entries.
- `DenseKernelPathTests` holds the row-by-row walk to a column-by-column `dgeqr2` bit for bit, NaNs
  compared as one, since the JIT may swap an addition's operands and x86 keeps the first NaN's sign.
- `RecordArrayOwnershipTests` pins each remark.
- Random differential against numpy 2.5.3: 3,000 matrices up to 39 × 12, a sixth each finite,
  holding one to three `±∞` or NaN (two sixths), with a zero column, scaled by `1e±200` or `1e-300`,
  or with a dependent column; on the first five kinds, 2,500 matrices, `main` mismatches 1,274 and
  this branch none. The dependent-column sixth is rounding noise past the pivot on both sides.
- A/B/A against `main` at `eb4d240a` on the developer machine, `HouseholderQrBenchmarks` (30 columns):
  2,000 rows 933 / 934 / 926 µs, 20,000 rows 11.13 / 10.92 / 11.02 ms. A first port wrote Q's own
  column in a pass of its own, striding a whole row per element, and was 7 % slower at 20,000 rows;
  the column is now written in the walk that applies the reflector.
