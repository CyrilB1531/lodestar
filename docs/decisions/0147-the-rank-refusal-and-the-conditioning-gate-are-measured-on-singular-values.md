---
status: accepted
supersedes: []
amends: []
applies: ["0096", "0105"]
---
# 0147 — The rank refusal and the conditioning gate are measured on singular values

**Status:** accepted · **Date:** 2026-09-18 · **Applies:** [`0096`](0096-ordinary-least-squares-earns-its-own-package.md), [`0105`](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)

## Context

Two numbers decide how a least-squares fit is answered, and neither measured what its own remark
said it measured.

The **collinear refusal** ([#978](https://github.com/CyrilB1531/lodestar/issues/978)) compared
`|Rₖₖ|` against `max(n, p)·ε` times **that column's own norm**, and called it
`numpy.linalg.matrix_rank`'s tolerance. It is not: that tolerance is `σmax·max(M, N)·ε` on the
singular values, which is global. Rounding in the reflections scales with the largest columns, so
a dependent column far smaller than the columns it depends on passes. On an intercept, `x₁ = 1..8`,
`x₂ = x₁ + δ·r` and `x₃ = x₁ − x₂` — exact in double, so rank 3 — the fit was answered with
coefficients of ±4e13 to ±1.2e14 at every `δ` from 1e-2 to 1e-10, where statsmodels 0.15.0 finds
rank 3 and `df_resid` 5. That is the failure [#867](https://github.com/CyrilB1531/lodestar/issues/867)
was meant to remove, still reachable.

The **normal-equations gate** ([#985](https://github.com/CyrilB1531/lodestar/issues/985)) accepted
a design when `√p·‖D·U⁻¹‖_F ≤ 200`. That Frobenius bound gives away `√p` on each of its two norms,
so it is **at least `p` for any design at all**: an orthonormal design at `p = 200` scores exactly
200.00 against a true `κ₂` of 1.000. Past about 195 parameters every fit took the reflections, and
the `κ²·ε ≤ 9e-12` rationale behind the constant described nothing that was happening.

## Decision

**Both are measured on the singular values of the `p × p` triangular factor each already claimed to
measure.**

- The refusal is `σmin(R) ≤ σmax(R)·max(n, p)·ε`. `R` carries the design's singular values
  unchanged, the reflections being orthogonal, so this **is** `matrix_rank`'s test rather than an
  approximation of it, and the remark and the OLS, WLS and GLS rows of `docs/equivalence.md` become
  true as written. `Lodestar.Stats.TimeSeries`' `SharedReflections` carried the same test and takes
  the same correction.
- The gate estimates `κ₂` of the column-scaled factor `U·D⁻¹` when its cheap bounds disagree, so
  the limit is again a statement about conditioning rather than about `p`. **Estimated, not
  computed**, and that is a measured decision rather than a shortcut: a one-sided Jacobi spectrum
  of a 251 × 251 factor costs 82 ms where a whole 4 000-row fit by reflections costs 41 ms, so
  computing the condition exactly costs twice the detour it exists to avoid. Power iteration on
  `AᵀA` is `O(p²)` an iteration against the sweep's `O(p³)`: 2.4 ms at that order, and within 1%
  of the spectrum on factors of order 8 to 251. It is LAPACK's own reasoning in `dtrcon`.

**The loser is scale invariance.** The per-column test did not move with a column's scale and a
global test does. That is not a divergence introduced here — `matrix_rank` is not scale invariant
either, and refuses the same designs: measured at n = 200, a column at 1e-14 beside one at 1e8 is
rank 2 to numpy and refused here, a column at 1e15 beside a unit one is rank 1 to numpy and refused
here, and a population/rate/normal design is full rank to both. Aligning with the reference rather
than documenting the divergence is what decided it.

**Neither is paid on every fit.** Each gate keeps a cheap bound on either side of its threshold and
reaches its estimator only when the two disagree. For the refusal,
`minₖ|Rₖₖ|/maxₖ|Rₖₖ|` bounds `σmin/σmax` from above and refuses outright, and
`1/(‖R‖_F·‖R⁻¹‖_F)` bounds it from below and accepts outright — the latter clears every design
conditioned better than about `1/(p·n·ε)`. For the gate, the existing Frobenius bound still accepts
on its own, so no design that is fast today became slower.

## Consequences

- **A rank-deficient design that used to be answered is now refused.** The `1e14` coefficients it
  used to produce were not an answer, but a caller reading them is a caller whose call now throws.
- `src/Shared/JacobiSpectrum.cs` holds the one-sided Jacobi sweep, opted into by
  `Lodestar.Decomposition`, `Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries`, and
  `src/Shared/RankBracket.cs` the three tests above, opted into by the two that refuse on them —
  so the packages cannot come to refuse different designs. Shared source is how they hold one copy: `src/` reaches its neighbours through a published floor,
  and [0012](0012-per-package-versioning.md) means a downstream package cannot consume new upstream
  API in the same lot. This follows `Reflections.cs` and `ElementWise.cs`, which are shared for the
  same reason.
- `Lodestar.Decomposition.Internal.JacobiSvd.SingularValues` delegates to it, so the sweep is one
  text and the three packages cannot drift apart in the order they add terms.
- No public API moved, so no reference page gains an entry and no `Version.props` moves. What the
  reference pages describe — *when* a fit refuses — did change, and they say so.
- **The normal equations may only answer a design their own bracket proves full rank.** The
  refusal is scale-sensitive and both gates on that path are scale invariant, so without a third
  test the two entry points diverged: `Fit` answered a design `Estimate` refused — a column at
  `1e-14·U(0,1)` beside a unit one came back with a coefficient near 1e11. `UᵀU = XᵀX = RᵀR`, so
  `U` carries the same singular values `R` does and the same Frobenius bracket reads there.
  Anything it cannot settle falls through to the reflections, which decide on the spectrum and
  raise the refusal, so the two cannot disagree. It costs `2p²` products on a path that forms the
  Gram in `np²/2`.
- **The estimate errs towards accepting.** A Rayleigh quotient is a lower bound, so the product of
  the two is one too, and a design whose true `κ₂` is a little past 200 can be let through. The
  limit stands for `200²·ε`, 9e-12 against corpora compared at 1e-9, so the measured 1% stays a
  hundredfold inside the budget it guards. The gate never over-refuses, which is the direction that
  would cost a caller an answer.
- A design past 200 parameters can now take the normal equations, which the benchmarks price. Where
  they do not show a gain, the Frobenius bound stays and the gate is left as it was: the exact
  spectrum was tried first and dropped for costing more than it saved, which is the measurement
  this record is here to keep.
