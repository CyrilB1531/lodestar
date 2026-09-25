---
status: accepted
supersedes: []
amends: ["0004"]
applies: []
---
# 0008 — numpy's legacy generator is replayed

**Status:** accepted · **Date:** 2026-09-25

## Context

[`0004`](0004-what-is-written-here-and-what-is-delegated.md) records *a portable seed* as **not
written**: reproducing MT19937 bought parity that an explicit input already gave, and a wrong
Mersenne Twister fails quietly. Every shuffled splitter followed it, taking the permutation
scikit-learn drew rather than the `random_state` that drew it.

`train_test_split(stratify=y)` ended that equivalence
([#1157](https://github.com/CyrilB1531/lodestar/issues/1157)). `StratifiedShuffleSplit` decides
how many rows each class sends to each side with `_approximate_mode`, which breaks ties between
classes with `rng.choice`. No permutation a caller hands in carries that choice, so the input that
replaced the seed cannot replay the reference's split.

Two .NET packages reproduce `numpy.random.RandomState`, read on 2026-09-25:
`RandomState(42).permutation(10)` is `[8 1 5 0 7 2 9 4 3 6]` from numpy 2, from NumSharp 0.70.0
(`net8.0` and `net10.0` only) and from NumpyDotNet 0.9.87.2 (`netstandard2.0`, BSD, last published
2025-05-14). Neither can be a dependency of `Lodestar.Preprocessing`: a core package carries none
([`0003`](0003-the-package-layout-tiers-boundaries-and-edges.md)).

## Decision

**numpy's legacy generator is written here, internal to the package that draws from it, and the
seed crosses the public API as a number.** The part replayed is exactly what `check_random_state`
reaches: MT19937 seeded by `init_genrand`, bounded integers by masked rejection, Fisher-Yates from
the top for `shuffle` and `permutation`, and `choice(replace=False)` as `permutation(n)[:size]`.
A seed is a `long` in `[0, 2³² − 1]`, numpy's range, so no generator type is published and nothing
goes to `Lodestar.Abstractions`. A second package that draws moves it to `src/Shared/`.

This amends `0004`'s row: *a portable seed* is **written here**, for the splitters. The inputs that
replaced a seed stay — a permutation, and the Ω of the sparse decomposition — because a caller who
holds one must not need a seed to use it.

## Consequences

- **The quiet failure `0004` feared is made loud.** A corpus of numpy's raw 32-bit outputs,
  permutations and choices over seeds from 0 to 2³² − 1 is replayed before any splitter is: a
  generator one bit wrong fails there by name, not three calls later as a fold that differs.
- **The two packages above are named where the seed is documented**, so the claim that .NET has
  no copy of numpy's generator is withdrawn where it was published.
- **Parity now extends to the seed only where the reference's draw is replayed call for call.** A
  reference that draws from `np.random.default_rng`, from Python's `random`, or from a generator
  outside numpy is not covered by this record.
