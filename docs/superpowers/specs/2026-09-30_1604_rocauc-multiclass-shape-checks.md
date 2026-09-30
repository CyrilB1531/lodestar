# RocAuc.MultiClass's shape checks where scikit-learn makes them

**Issues:** [#1604](https://github.com/CyrilB1531/lodestar/issues/1604),
[#1605](https://github.com/CyrilB1531/lodestar/issues/1605).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Metrics` after #1602, on `main` at `8799cc5a`, against scikit-learn
1.9.1, found two shape checks out of place:

- A `yScore` of a whole number of rows but not one per sample was refused before the row sums,
  the averaging, the labels and the one-vs-one weight; scikit-learn counts samples last, in
  `check_consistent_length`, "Found input variables with inconsistent numbers of samples: [4, 3]".
  #1601's spec had kept the check early because a two-dimensional array cannot disagree with its
  own shape, which holds for its columns and not its rows.
- Two score columns were scored as a multiclass problem; `roc_auc_score` takes the binary path when
  `y_true` holds two labels or fewer, and `column_or_1d` refuses the matrix: "y should be a 1d array,
  got an array of shape (4, 2) instead."

## Decisions

- **A flat span still states its shape first**: a length that is not a whole number of rows cannot
  be read as a matrix at all, so that refusal stays with the array checks, in this package's words.
- **The row count moves to `check_consistent_length`'s place and sentence**, after the labels and
  the one-vs-one weight, and the weights' count joins it, as scikit-learn lists every length it
  compared: `[4, 3, 2]`, and `[4, 4, 2]` when only the weights disagree.
- **One or two columns take scikit-learn's binary path** over a `y_true` of two labels or one, once
  the arrays have passed their own checks. Over two labels they meet that path's
  `check_consistent_length` first — rows and weights counted, as above — then `column_or_1d`, which
  refuses two columns and accepts one: a single column is scored, the greater label positive, as
  `label_binarize` has it, where this refused any `classCount` below two. None of the row sums, the
  labels or a one-vs-one weight is read on that path. Over one label it answers `nan`, whatever the
  counts or the weights. Review A found the one-label case, the count coming first and the single
  column; on that path the weights' own refusals carry `options`, the parameter `RocAuc.MultiClass`
  carries them in, as its other weight refusals now do too. Over three labels scikit-learn takes the
  multiclass path and refuses the row sums or the class count instead, which this reaches too;
  `classCount` below one keeps a refusal with no counterpart.
- **Two parallel tests move off two classes**: the `ArrayPool` collision one to four classes over
  ten samples, which collides the same way, with a two-present-classes run keeping its single
  one-vs-one pair; the many-workers one to three classes.
