# Lodestar.Metrics review findings before 1.0

**Issues:** [#1200](https://github.com/CyrilB1531/lodestar/issues/1200),
[#1201](https://github.com/CyrilB1531/lodestar/issues/1201),
[#1202](https://github.com/CyrilB1531/lodestar/issues/1202),
[#1203](https://github.com/CyrilB1531/lodestar/issues/1203),
[#1204](https://github.com/CyrilB1531/lodestar/issues/1204),
[#1205](https://github.com/CyrilB1531/lodestar/issues/1205),
[#1206](https://github.com/CyrilB1531/lodestar/issues/1206),
[#1249](https://github.com/CyrilB1531/lodestar/issues/1249),
[#1250](https://github.com/CyrilB1531/lodestar/issues/1250),
[#1251](https://github.com/CyrilB1531/lodestar/issues/1251),
[#1252](https://github.com/CyrilB1531/lodestar/issues/1252).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

Two reviews of `main` (at `d228321f` and `9b142ab2`) found eleven places where `Lodestar.Metrics`
answered differently from scikit-learn 1.9.1, or not at all:

1. The precision family built a dense `m × m` matrix per call: 65,536 labels wrapped `m·m` to 0 and
   threw, 46,341 overflowed, 20,000 allocated 3.2 GB (#1200).
2. `Averaging.Binary` refused a batch without the positive class (#1201), and refused explicit
   `labels` over a binary target (#1249). scikit-learn scores the first through `zero_division` and
   replaces `labels` with `[pos_label]` in the second.
3. A requested label absent from `yTrue` was refused by the whole precision family, where only
   `confusion_matrix` refuses it (#1202).
4. The three classifier curves kept zero-weight samples as thresholds of their own (#1203).
5. `CalibrationCurve`'s edges were `i / n` and the textbook interpolation, an ulp off numpy's
   `linspace` and `_lerp`, so a probability on an edge changed bin (#1204).
6. `DaviesBouldin.Score` lacked the reference's `np.allclose` short-circuit (#1205).
7. Several inputs the reference refuses were scored: non-finite features, scores and relevances, a
   precomputed silhouette diagonal, a multiclass `yTrue` label outside `Labels`; and
   `MeanSquaredLogError` overflowed near `1e306` (#1206).
8. `LikelihoodRatios` skipped the `LR-` replacement without a positive sample (#1250), divided
   through `1 − specificity`, which cancels (#1252), and `DetCurve` drew a `NaN` curve on a
   single-class truth (#1251).

## Decisions

1. **The precision family counts per label from the samples.** A new internal `PrfCounts` holds
   tp, predicted and true weight per requested label. Weights are summed in one pass in sample
   order, `np.bincount`'s, so the sums land on its bits; unweighted counts are integers, exact in
   any order, and up to 64 labels still come from the old matrix counting, one increment a sample,
   which the benchmark below needed to stay at par; past 64, per-label sums in `O(n + k)`. The
   span overloads of `Precision`, `Recall`, `F1`, `FBeta`, `JaccardScore` and `ClassificationReport`
   use it; their `ConfusionMatrix` overloads read the same counts off the matrix, whose column sums
   are now vectorised — the same additions in the same order, several columns at a time.
2. **`Averaging.Binary` is `_check_set_wise_labels`.** Its counts are the positive label's alone,
   `labels` unread; the observed labels decide the refusal — more than two, or two without
   `posLabel`. The matrix overload reads those observed labels off the matrix, which now keeps them.
3. **The "label must occur in yTrue" refusal stays in `ConfusionMatrix.Compute`**, which
   `CohenKappa` goes through, as `cohen_kappa_score` goes through `confusion_matrix`; the precision
   family no longer reaches it.
4. **numpy's grid is shared source**, `src/Shared/NumpyGrid.cs` behind
   `LodestarIncludesNumpyGrid`: `linspace` as `i · step + start` with the last point exact, and the
   linear `percentile` with its two-sided `_lerp`. `Lodestar.Preprocessing` is to take it for
   `KBinsDiscretizer` with #1226 and #1237, replacing its own `Percentile.Linear`.
5. **The ratios are scikit-learn's arithmetic**: `tp·(tn+fp) / (fp·(tp+fn))` and
   `fn·(tn+fp) / (tn·(tp+fn))`, each replaced only when its own count vanishes. With no positive
   sample the one not replaced is `0/0`, `NaN`, with no special case.
6. **The refusals carry the reference's sentences** — `check_array`'s, `check_X_y`'s,
   `silhouette_samples`' diagonal test at `100·ε`, `det_curve`'s — through the package's existing
   `Inputs.RequireFinite`, except the classifier scores, which keep their indexed message and now
   name an infinity as well as a `NaN`.

The differential below found two further divergences in the same code, fixed here, and one residue that is not:

- **`RocCurve`** answered `0` at the origin of an axis whose class had no weight; `roc_curve` makes
  the whole axis `NaN`, the origin included, on `fps[-1] <= 0`.
- **`LikelihoodRatios`** scored a target holding one class; the reference raises unpacking a
  `1 × 1` matrix, so it is refused.
- **`DaviesBouldin`** keeps 14 of 800 random cases apart, all clusters of one sample: the
  reference's `euclidean_distances` expands `xx − 2·x·y + yy`, and an FMA BLAS kernel leaves a
  residue near `1e-8·|x|` where the exact spread is `0`. `OPENBLAS_CORETYPE=Prescott` removes it and
  all 800 match, with the expanded form and with the difference squared alike; the expanded form
  cost 20% on Davies-Bouldin and 60% on `Silhouette` for no case gained, so neither takes it.

## Verification

- **Oracles**: new fixtures in five generators — four binary-averaging fixtures and two
  label-absent ones in `classification_metrics.json`, two in `likelihood_ratios.json`, two in
  `calibration_curve.json`, one zero-weight case in `curves.json`, one small-scale clustering in
  `internal_validity.json`. The 18 tests they add fail on `main`; the existing cases are unchanged.
- **Unit tests** (`ReviewFindingTests`) for what a corpus cannot carry: 65,536 labels, each refusal
  with scikit-learn's sentence, the overflow. Two older tests pinned behaviour scikit-learn refuses,
  an infinite ROC score and a multiclass label outside `Labels`, and now assert the refusal.
- **A random differential** against scikit-learn 1.9.1 over 10,076 cases, 15,733 comparisons:
  4,000 precision-family calls across the five averages, labels, weights with zeros and the three
  zero-division values, 4,500 weighted curves, 1,476 calibration curves, 1,500 likelihood-ratio
  pairs over weights from `1e-3` to `1e9`, 800 Davies-Bouldin scores down to `1e-11`, 800 silhouette
  scores from `1e-8` to `1e3`. `main`: 3,554 mismatches. Here: 14, all of them the BLAS residue
  above.

## Review before merge

The `code-review` tool over the diff raised ten points, all handled in this change:
`DetCurve` refused one class but not three; `LikelihoodRatios` scored a two-class target without
`posLabel`; the new finite check reached `ReciprocalRank`, which has no reference and ranks `−∞`
last, so it takes none, while the label-ranking scores, which scikit-learn does check, now do;
all-zero curve weights took a sentence of their own rather than `_check_sample_weight`'s; weighted
counts on few labels summed down matrix columns, not in sample order; `Silhouette` kept the
difference-squared distance — measured, it needs no change, see above; the `NumpyGrid` comment claimed a Preprocessing use not yet made;
the precision family's XML still described the old refusals; Davies-Bouldin computed each centroid
distance three times; and the span path skipped `fbeta_2.0`. A test that fed `±∞` and `NaN` to
`CoverageError` now uses finite ties, since both are refused before either counting path — which
leaves `LabelRanking.MaxRank`'s `NaN` branch unreachable, a point for the package review.

## Rejected

- **Keeping the dense matrix and checking `m·m` for overflow**, the smallest fix for #1200. It would
  still allocate `8·m²` bytes for a result of `3·m` numbers.
- **Reproducing the FMA residue in Davies-Bouldin.** It depends on which OpenBLAS kernel the
  reference runs; a CPU without FMA gives the answer this code gives.
