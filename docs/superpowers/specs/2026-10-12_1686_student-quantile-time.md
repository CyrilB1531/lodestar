# Student's quantile takes log B(df/2, 1/2) once per inversion

**Issues:** [#1686](https://github.com/CyrilB1531/lodestar/issues/1686).
**Status:** written with the work, 2026-10-12.
**Date:** 2026-10-12.

## The problem

The Review B after #1685 timed `Distributions.StudentQuantile(p, 10)` slower than Lodestar.Stats 0.5.0,
a slowdown already on `main` before #1685. Bisecting Stats between the release and `b9131663` put it
at `888bfde2` (#1158): 0.343 ms for 1,000 quantiles at its parent, 0.381 at it, two thirds of it the
new small-shape `Beta.Front`, which since #1158's accuracy rewrite takes `log B(a, b)` through
`LogBeta` at every call, the rest code layout. The 500.5 ns the Stats performance page recorded on
2026-09-25 was `888bfde2`'s own run, already slowed; no BenchmarkDotNet figure for the release exists.

## Decisions

- **Keep #1158's accuracy, take `log B(df/2, 1/2)` once.** A Student inversion holds both shapes
  fixed, yet every Newton step's tail evaluation took the beta function's logarithm afresh — three
  log-gammas below shapes of 10, a Stirling increment from there. `StudentTail` takes it once and
  hands it through private overloads of `StudentSf`, `RegularizedIncomplete` and `Front`; the value
  is the same, so every quantile keeps its bits — 95,833 Student and F calls, refusals included,
  compared with `main` bit for bit. From `df / 2 = 1000` it is left untaken: the large-shape
  expansion answers every step with `t² ≤ df` without it, and taking it there cost those quantiles a
  fifth; a far-tail step past `t² > df` takes it afresh, as before. Ten of `main`'s quantiles either
  side of `df = 20` and `2000` are pinned to their bits in the Stats tests, and an eleventh,
  `p = 5e-324` at `df = 2000`, ends past `t² > df` on that uncached far-tail step.
- **Not extended to the beta quantile.** Holding both orders' `log B` in `BetaQuantile`'s solver
  was tried for the F quantile and measured level (207–212 ns against `main`'s 207), so it was left
  out.
- **Measured with BenchmarkDotNet**, AMD Ryzen 7 8700G, .NET 10.0.12, default job, one window
  under the lock: `StudentQuantile(0.975, df)` against `main` reads 470.7 against 509.2 ns at
  `df = 10`, 181.1 against 180.0 at 4,000 and 163.5 against 160.8 at 100,000; the review's own run
  read 1.03 to 1.04 times `main` from `df = 500` up, under the 1.05 the project counts, the gain being
  where `log B` costs three log-gammas, below `df = 20`. A hand-written loop
  over 1,000 quantiles reads two modes under the default tiered JIT, the slower twice the faster,
  and none fully optimised (`DOTNET_TieredCompilation=0`: 0.373 ms against the release's 0.406);
  BenchmarkDotNet's steady state is the one the performance page records.
- **Carried from the Review B after #1685:** `ShapiroWilk`'s `n = 3` comment, which read the middle
  Blom score as `-0.0` where `ndtri` gives `+0.0`.
