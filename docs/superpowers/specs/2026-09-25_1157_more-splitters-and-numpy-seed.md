# The splitters scikit-learn users reach for first, with its own seed

**Issue:** [#1157](https://github.com/CyrilB1531/lodestar/issues/1157).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`Splitters` offers `KFold`, `StratifiedKFold` and `TrainTest`. `docs/equivalence.md` carries two
*not written* rows: `train_test_split(..., stratify=y)`, and `GroupKFold`, `TimeSeriesSplit`,
`RepeatedKFold`. A caller whose rows are not independent — grouped data, a time series, a
variance estimate over repeats — reaches for exactly those.

The shuffled splitters take the permutation as an input, not a seed, because `random_state`
reaches numpy's generator and .NET has no copy of it. That held until the stratified train/test
split: `StratifiedShuffleSplit` breaks ties between classes with `rng.choice`, so no permutation a
caller could pass replays it.

## Decisions

1. **numpy's legacy generator is replayed, and every shuffled splitter takes scikit-learn's
   `random_state`.** `check_random_state(int)` builds a `numpy.random.RandomState`: MT19937 seeded
   by `init_genrand`, drawing bounded integers by masked rejection (`random_interval`), shuffling
   by Fisher-Yates from the top, and drawing `choice(replace=False)` as `permutation(n)[:size]`.
   A Python replica of those four pieces matched numpy on raw outputs and on `permutation` for
   seeds 0, 1, 42, 12345, 2³¹−1 and 2³²−1 and lengths 1 to 1,000, with zero mismatches. Rejected:
   the stratified split as one fold of `StratifiedKFold` at `round(1 / f)` folds, whose test size
   is not `ceil(n·f)`; and tie-breaking by class order, which matches the reference only where no
   remainder tie is partly served. This reverses a row of
   [decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md), so
   [decision 0008](../../decisions/0008-numpy-s-legacy-generator-is-replayed.md) amends it.
2. **The generator stays internal**, `Lodestar.Preprocessing.Internal.NumpyRandomState`. The seed
   crosses the API as a `long` in `[0, 2³² − 1]`, numpy's own range, so no new public type exists
   and nothing goes to `Lodestar.Abstractions`. A second package that needs it moves it to
   `src/Shared/`.
3. **Seed overloads on the three existing splitters**: `KFold(sampleCount, foldCount,
   randomState)`, `StratifiedKFold(labels, foldCount, randomState)` and `TrainTest(sampleCount,
   testFraction, randomState)`, which are `KFold(shuffle=True)`, `StratifiedKFold(shuffle=True)` and
   `train_test_split(shuffle=True)` with that `random_state`. The `order` overloads stay.
4. **New members**, each deterministic without a seed and replayed with one where the reference
   shuffles:
   - `GroupKFold(groups, foldCount)` and `(groups, foldCount, randomState)`: the greedy
     assignment of the largest group to the lightest fold, groups ranked by `np.unique` and sizes
     sorted by a stable argsort reversed; the shuffled form permutes the distinct groups and
     cuts them with `np.array_split`.
   - `StratifiedGroupKFold(labels, groups, foldCount)` and `(…, randomState)`: groups by
     descending standard deviation of their class counts (stable), each to the fold that
     minimises the mean over classes of the standard deviation of the per-fold class shares,
     ties by `np.isclose` then fewer samples. `np.std` and `np.isclose` are reproduced to the
     bit, in numpy's own order of additions (below).
   - `TimeSeries(sampleCount, splitCount, testSize, gap, maxTrainSize)`, with `testSize` and
     `maxTrainSize` nullable as `None` is in the reference.
   - `RepeatedKFold(sampleCount, foldCount, repeatCount, randomState)` and
     `RepeatedStratifiedKFold(labels, foldCount, repeatCount, randomState)`: one generator shared
     across the repeats, the folds of every repeat in one list, as the reference yields them.
   - `StratifiedTrainTest(labels, testFraction, randomState)`: `train_test_split(stratify=y)`,
     through `StratifiedShuffleSplit`'s `_approximate_mode` twice and a permutation per class.
     The reference refuses stratification without shuffling, so there is no unseeded form.
5. **Refusals follow the reference's**: more folds than groups, a time-series split with no
   training row left, a class of one member or fewer test rows than classes in the stratified
   split, all as `ArgumentException` or `ArgumentOutOfRangeException` with the reference's reason.
   Two time-series inputs are the exception, below.
6. **A claim corrected.** `Splitters`' remarks and `docs/equivalence.md` say no permutation
   reproduces `StratifiedKFold(shuffle=True)`. One does: each class's rows read in the order of the
   permutation scikit-learn draws for that class's fold list, classes in order of first
   appearance. Measured on 300 random cases, single and repeated, with zero mismatches. The text
   is corrected, and the seed overload makes the question moot for a caller.

## What the work found

- **Two .NET packages already replay the generator**: NumSharp 0.70.0 and NumpyDotNet 0.9.87.2 both
  give numpy's `RandomState(42).permutation(10)`. Neither can be a dependency of a core package, so
  decision 0008 names them and still writes the generator.
- **numpy 2 sums a contiguous run pairwise from its first element**, and a column of a matrix row by
  row unless the matrix has one column. The first reading, first element plus the pairwise sum of
  the rest, passed the corpus and failed the random differential run on a five-class
  `StratifiedGroupKFold` case, now frozen.
- **`TimeSeriesSplit` reads two odd inputs its own way**: `max_train_size=0` is no cap, and a
  negative `gap` overlaps training and test. Both are followed. Two are refused, where the reference
  returns an empty block without a word: a negative `max_train_size`, and test blocks needing more
  rows than there are, which only a negative gap lets past its check.
- **Adding `long randomState` beside `ReadOnlySpan<int> order`** makes `default` as the third
  argument ambiguous; the changelog says so.
- **The benchmark moved three things**: the seeded train/test split read its halves out of a mask
  rather than sorting them, the time series describes each block rather than copying it, and
  classes are hashed before their distinct values are sorted. The seeded train/test split stays
  behind scikit-learn past 10,000 rows, on the cost of the generator's draws.

## Proof

- A corpus of raw generator outputs, `permutation` and `choice` from numpy, over the seeds above:
  a wrong Mersenne Twister fails there first, and loudly.
- A splitter corpus from `sklearn.model_selection` 1.9.1, compared index for index: every new
  splitter, both forms, the seed overloads of the existing three, and the edge cases (fewer groups
  than folds, `gap` larger than a fold, `max_train_size` binding, remainder ties in the stratified
  split, a class of two members).
- A random differential run against scikit-learn before the push.

## Knock-on changes

Reference pages for each new member and overload, `docs/equivalence.md` rows replacing the two
*not written* ones and correcting the shuffled row, uses in the sample, the package changelog,
decision 0008 with the index and README counts it moves, and a benchmark against scikit-learn in
`src/Lodestar.Preprocessing/performance.md`.
