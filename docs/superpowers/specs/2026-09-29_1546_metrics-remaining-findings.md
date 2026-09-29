# The Metrics findings left after #1591

**Issues:** [#1546](https://github.com/CyrilB1531/lodestar/issues/1546),
[#1569](https://github.com/CyrilB1531/lodestar/issues/1569),
[#1592](https://github.com/CyrilB1531/lodestar/issues/1592),
[#1593](https://github.com/CyrilB1531/lodestar/issues/1593),
[#1594](https://github.com/CyrilB1531/lodestar/issues/1594),
[#1595](https://github.com/CyrilB1531/lodestar/issues/1595).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

Six Metrics issues remained open after #1591, against scikit-learn 1.9.1 and numpy 2.5.3:

- `MedianAbsoluteError` placed the halfway point differently from `_weighted_percentile` once a
  negative weight made the cumulative weight fall: `[1, -1, 0]` gave 0.5 where scikit-learn gives
  1.0 (#1546).
- `RocAuc.MultiClass` let a NaN score through the row sums and refused it inside a class curve,
  naming an index into a compacted column, where `check_array` says "Input contains NaN." (#1569).
- Three pages or tags said less, or other, than the code refuses (#1592, #1593, #1594).
- `NumpyAverage`'s eight-lane and halving branches were never reached by a test (#1595).

## Decisions

- **The weighted percentile is scikit-learn's, step for step**: the cumulative weight, the search,
  the machine-epsilon test, the next index past a zero weight. On a non-monotone cumulative weight
  the search decides the answer, and numpy 2.5's `searchsorted` is branchless — halve by probing
  `base + half`, compare once more — which matched `np.searchsorted` on 40,000 random cases where
  the textbook binary search matched 97.7 %. A randomized differential on 2,000 medians with
  negative and zero weights agrees on 1,999; the last one holds two equal errors weighted
  differently, whose order is numpy's `argsort`'s — not stable, and dependent on the CPU's vector
  units — reproducible neither by a stable sort nor by .NET's. On tied integer errors the gap
  reaches about 5 % of cases, and it reaches the D² scores the same way. With non-negative weights
  that order changes nothing, so the gap is stated on the pages rather than chased.
- **Every refusal happens before a worker starts.** A non-finite score is refused before the rows
  are summed, as `check_array` refuses it, and a sample weight every class curve would refuse
  alike is refused once, after the zero-total shortcut, in `_average_binary_score`'s order. No
  worker can then fail, so the per-worker catch, the failure slots and the lowest-index rethrow are
  removed rather than kept unreachable. Of the three tests that planted a NaN to make one class or
  pair fail first, one becomes a parity test of the up-front NaN refusal and a second new one
  covers the weight refusal, sequential against parallel; the other two, the early-stop guard and
  the one-vs-one lowest-pair test, are deleted with the machinery they guarded.
- **The port reaches the D² scores too.** `D2AbsoluteError` and `D2Pinball` take the same weighted
  percentile in their denominator, so under negative weights they now give scikit-learn's value
  where the old reading differed — pinned at alpha 0.5 and 0.3.
- **The branch test is scikit-learn's own value**: one-vs-one over 17 classes averages 136 pair
  scores, and scores that are integers over 1024 are exact in both languages; on that input the
  pairwise sum gives `roc_auc_score`'s bits and an in-order sum does not.
