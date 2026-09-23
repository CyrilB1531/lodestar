# The pair that never needed scoring

**Issue:** [#1134](https://github.com/CyrilB1531/lodestar/issues/1134).
**Status:** written before the work, 2026-09-23.
**Date:** 2026-09-23.

## The problem

`Process.Cdist` takes a `scoreCutoff` and applies it after scoring every pair in full. The cell
below the cutoff is written as `0` — but the scan that produced the score it discarded has
already run.

## What was measured

### The bound is exact, not merely an upper bound

The Indel distance is at least the difference in lengths, so
`ratio ≤ 100 × (1 − |la − lb| / (la + lb))`. Against rapidfuzz 3.14.6 the ceiling *is* the answer
wherever the lengths dominate: `"abc"` / `"xxxabcxxx"` ceiling 50.00 and `fuzz.ratio` 50.00;
`"cat"` / `"the cat sat on the mat"` ceiling 24.00 and 24.00.

### What it saves, and what it costs when it does not fire

`CdistCutoffBenchmarks`, 64 × 64, BenchmarkDotNet 0.14.0 on .NET 10.0.12, AMD Ryzen 7 8700G,
Ubuntu 26.04.1, 2026-09-23. `Cutoff=0` closes the gate, so each row is one method measured
against itself before and after:

| corpus | cutoff | before | after | gain |
| --- | ---: | ---: | ---: | ---: |
| 1–6 words | 60 | 169.00 μs | 131.67 μs | **1.28×** |
| 1–6 words | 80 | 169.00 μs | **82.07 μs** | **2.06×** |
| 1–6 words | 90 | 169.00 μs | **47.63 μs** | **3.55×** |
| 3 words | 60 | 152.84 μs | 154.98 μs | 0.99× |
| 3 words | 80 | 152.84 μs | 150.47 μs | 1.02× |
| 3 words | 90 | 152.84 μs | 113.13 μs | **1.35×** |

Where it cannot fire it costs nothing measurable — 0.99× and 1.02×, inside the ±1.5% the machine
moves anyway — which is why the gate is `scoreCutoff > 0` and nothing finer. The unbounded
control, the same scores through a lambda the gate cannot recognise, is flat at 176–188 μs across
all four cutoffs on the wide corpus: the cutoff alone buys nothing, and the bound behind it is
what does.

The narrow corpus was expected to be pure cost and is not. Three words from a three- to
eight-character vocabulary still spread a phrase over some twelve characters, which at a cutoff of
90 is enough for 1.35×. A corpus has to be genuinely uniform before this is only a branch.

### The constraint that sets the scope

**The bound holds for `Fuzz.Ratio` alone.** Measured:

| pair | ceiling | `ratio` | `partial_ratio` | `token_set_ratio` | `WRatio` |
| --- | ---: | ---: | ---: | ---: | ---: |
| `"abc"` / `"xxxabcxxx"` | 50.00 | 50.00 | **100.00** | 50.00 | **90.00** |
| `"cat"` / `"the cat sat on the mat"` | 24.00 | 24.00 | **100.00** | **100.00** | **90.00** |

A partial or token scorer ignores length by construction. Applying the bound to an arbitrary
delegate would answer `0` where the truth is `100`, so it applies only when the effective scorer
is `Fuzz.Ratio` — which is `Cdist`'s own default.

That is detectable: a method-group conversion of `Fuzz.Ratio` compares equal to another,
`Fuzz.WRatio` does not, and a lambda that merely calls `Ratio` does not either. The check errs
towards skipping the optimisation, never towards a wrong answer.

### Where this sits among the alternatives

Profiling the per-pair floor (#1130) decomposed a 16-character pair into 9.06 ns of scan, 11.0 ns
of table fill and restore, and 9.8 ns of call apparatus. Three levers came out of it: this one, a
hoisted pattern table (3.3×, but new `Lodestar.Text` API and a release order), and a
`char`-specialised affix trim (2× on near-duplicates, a loss elsewhere). This is the best ratio of
gain to complexity: no new public API, no release order, no algorithm change, and it is the only
one that removes work rather than making work faster.

## What ships

No public API changes. `Process.Cdist` gains an internal rejection:

- when `scoreCutoff > 0` **and** the effective scorer is `Fuzz.Ratio`, a pair whose length ceiling
  is below the cutoff is written as `0` without being scored;
- every other combination takes the path it takes today.

### What is not written

- **The bound on `Extract` and `ExtractOne`.** Their default is `WRatio`, which it does not hold
  for, so it would almost never fire and would cost the test on every candidate.
- **A cutoff-aware scorer contract**, the shape rapidfuzz uses, where each scorer carries its own
  bound. It is the general answer and a larger one: it changes `Func<string, string, double>` for
  something that takes a cutoff, on seven scorers. Worth its own issue if the pattern repeats.

## Proof

The answers do not change, and that is what the tests say first: `tests/oracles/process_cdist.json`
already carries three non-zero cutoff cases and is replayed unchanged — but only once its `ratio`
rows are passed as the *default* scorer rather than as a lambda, which is what puts them on the
bounded path at all. One of them rejects ten of twelve cells at a cutoff of 95.

Beyond the frozen corpus, 9,600 cells of a random 40 × 40 corpus — half unrelated, half
near-duplicates of one stem — were compared against `rapidfuzz` 3.14.6 at six cutoffs, with 21
cells still above 95. Every cell agreed **bit for bit**, not to `1e-9`.

A second test asserts the bound is *not* applied to the scorers it does not hold for, over the
pairs in the table above, and a third compares the bounded and unbounded paths over a random
corpus at eight cutoffs.
