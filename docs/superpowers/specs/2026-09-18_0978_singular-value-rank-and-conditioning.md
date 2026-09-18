# The rank refusal and the conditioning gate are measured on singular values

**Issues:** [#978](https://github.com/CyrilB1531/lodestar/issues/978) (important),
[#985](https://github.com/CyrilB1531/lodestar/issues/985) (minor).
**Status:** written before the work, 2026-09-18.

## Problem

Two numbers decide how `Lodestar.Stats.Regression` fits, and neither measures what its
own remark says it measures.

### 1. The collinear refusal misses a small dependent column (#978)

`LeastSquares.RequireFullRank` compares `|Rₖₖ|` against `max(n, p)·ε·‖Xₖ‖` — the column's
**own** norm. The remark, and the OLS, WLS and GLS rows of `docs/equivalence.md`, call this
"`numpy.linalg.matrix_rank`'s tolerance". It is not: that tolerance is `σmax·max(M, N)·ε`
on the **singular values**, which is global.

Rounding in the reflections scales with the largest columns, not with the column being
tested, so a dependent column much smaller than the columns it depends on passes.

**Reproduced on fresh `main` (`6346a2ae`).** Intercept, `x₁ = 1..8`,
`x₂ = x₁ + δ·r` with `r = [0.3, −1.1, 0.7, 2.2, −0.4, 1.5, −2.0, 0.9]`, `x₃ = x₁ − x₂`
(exact in double, so the design has rank 3), `y = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1]`:

| δ | Lodestar `OrdinaryLeastSquares.Fit` | statsmodels 0.15.0 |
| --- | --- | --- |
| 1e-2 | accepted, coefficients ±4.2e13, standard errors 1.7e14, `df_resid` 4 | rank 3, `df_resid` 5, `[0.024, −0.014, 2.012, −2.026]` |
| 1e-4 | accepted, ±1.1e14, 2.4e14, `df_resid` 4 | rank 3, `df_resid` 5 |
| 1e-6 | accepted, ±3.2e13, 2.5e14, `df_resid` 4 | rank 3, `df_resid` 5 |
| 1e-8 | accepted, ±8.4e13, 2.0e14, `df_resid` 4 | rank 3, `df_resid` 5 |
| 1e-10 | accepted, ±1.2e14, 3.1e14, `df_resid` 4 | rank 3, `df_resid` 5 |

That is the 1e14 failure [#867](https://github.com/CyrilB1531/lodestar/issues/867) was meant
to remove, still reachable.

`Lodestar.Stats.TimeSeries`' `SharedReflections.RequireFullRank` carries the same test and
the same remark, so it carries the same miss.

### 2. The normal-equations bound is always at least p (#985)

`WellConditioned` accepts the normal equations when `√p·‖D·U⁻¹‖_F ≤ 200`. With unit columns
`Σσᵢ² = p`, and Cauchy–Schwarz gives `Σ1/σᵢ² ≥ p`, so the bound is at least `p` **whatever
the design**. Past about 195 parameters every fit takes the reflections, and the
`κ²·ε ≤ 9e-12` rationale behind the constant 200 no longer describes what the limit does.

**Reproduced on fresh `main`.** Bound against the true column-scaled `κ₂`:

| design | p | bound | true κ₂ |
| --- | --- | --- | --- |
| orthonormal | 10 | 10.00 | 1.000 |
| orthonormal | 50 | 50.00 | 1.000 |
| orthonormal | 200 | 200.00 | 1.000 |
| Gaussian, n = 4 000, intercept | 20 | 20.04 | 1.121 |
| Gaussian, n = 4 000, intercept | 100 | 101.23 | 1.362 |
| Gaussian, n = 4 000, intercept | 199 | 204.18 | 1.559 |
| Gaussian, n = 4 000, intercept | 250 | 258.24 | 1.645 |

The orthonormal rows are the statement in its sharpest form: a perfectly conditioned design
is scored at exactly `p`. The bound is loose by a factor of `p`, because `‖A‖₂ ≤ ‖A‖_F = √p`
gives away `√p` and `‖A⁻¹‖₂ ≤ ‖A⁻¹‖_F` gives away another `√p`.

The issue also claims "from about 50 a mildly correlated design already crosses 200". That
part does **not** reproduce: equicorrelated designs at `ρ = 0.5` score 27.3, 70.6 and 142.7 at
p = 20, 50 and 100. The reproduced claim is the `≥ p` one, which is enough on its own.

## Decision

Both numbers are measured on the singular values of a `p × p` triangular factor, which is
what each of them was always described as measuring. Cyril chose this for both on 2026-09-18,
with speed named as the constraint.

- **#978:** refuse when `σmin(R) ≤ σmax(R)·max(n, p)·ε`. `R` shares `X`'s singular values
  exactly, `Q` being orthogonal, so this **is** `numpy.linalg.matrix_rank`'s test rather than
  an approximation of it, and the remark and the three equivalence rows become true as
  written instead of needing rewording.
- **#985:** when the cheap bound exceeds 200, estimate the `κ₂` of the column-scaled factor
  `U·D⁻¹` by power iteration and let the design through on that. The bound stays as a fast
  accept, so nothing that takes the normal equations today gets slower.

  The exact spectrum was written first and **measured out**: a Jacobi sweep on a 251 × 251
  factor costs 82 ms where a whole 4 000-row reflections fit costs 41 ms, so measuring the
  condition exactly cost twice the detour it was meant to avoid. Power iteration on `AᵀA` is
  `O(p²)` an iteration: 2.4 ms at that order, within 1% of the spectrum on factors of order 8
  to 251. **Cyril's condition, 2026-09-18: if the estimator is still slower than today's
  algorithm, #985 keeps today's algorithm** and the Frobenius bound stays with its limit
  documented.

### Scale invariance is given up, and that is the reference's behaviour

The old per-column test was scale invariant; a global test is not. That is not a divergence
introduced here — `numpy.linalg.matrix_rank` is not scale invariant either, and refuses the
same designs. Measured, n = 200:

| design | old per-column test | new spectral test | `numpy.linalg.matrix_rank` |
| --- | --- | --- | --- |
| population 1e8 / rate 1e-4 / N(0,1) | accepted | accepted | rank 4 — full |
| population 1e8 / independent column at 1e-14 | accepted | refused | rank 2 |
| column at 1e15 / N(0,1) | accepted | refused | rank 1 |
| the #978 repro, every δ | accepted | refused | rank 3 |

The new test agrees with the reference on all four. Aligning with the reference rather than
documenting the divergence is the rule this follows.

## Options rejected

1. **Global scale on `|Rₖₖ|`** — compare against `max(n, p)·ε·maxⱼ‖Xⱼ‖`. Free, and it agreed
   with the spectral test on all four designs above. Rejected because `|Rₖₖ|` is a pivot, not
   a singular value: on an adversarial design (Kahan's matrix is the standard one) R's
   diagonal misreports the rank, so the remark could only say "approximates
   `matrix_rank`", which is the wording the fix exists to stop needing.
2. **Equilibrate the columns to unit norm, then test globally** — attractive because it keeps
   scale invariance. Rejected because it does not work: measured on the #978 repro, the
   equilibrated pivot is 3.2e-14 at δ = 1e-2 and 2.8e-6 at δ = 1e-10, both far above the
   1.8e-15 tolerance. Normalizing the columns does not remove the cancellation, which comes
   from projecting a column of norm 0.037 out of columns of norm 14.
3. **Document the miss and the limit** — reword the remark and the equivalence rows to say
   what the two tests really do. Rejected under the align-with-the-reference rule;
   [#1093](https://github.com/CyrilB1531/lodestar/issues/1093) went back to draft for exactly
   this.
4. **Let the conditioning limit grow with p** (compare against `200·√p` or similar).
   Rejected: the ceiling would be a heuristic with no `κ²·ε` story behind it, and the `κ²·ε`
   story is the whole justification for the constant.
5. **Expose `JacobiSvd` publicly from `Lodestar.Decomposition`.** Its
   `SingularValues` is exactly the kernel needed and `Lodestar.Stats.Regression` already has
   the edge. Rejected because `src/` reaches its neighbours through a published floor:
   CONTRIBUTING.md's *Working across two packages* says a branch whose downstream side needs
   new upstream API cannot go green, and must be split across a release. Cyril asked for one
   pull request.

## How the kernel is shared

`src/Shared/`, opted into per package, which is how `Reflections.cs` already serves
`Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries`
([#843](https://github.com/CyrilB1531/lodestar/issues/843)) and `ElementWise.cs` serves
`Lodestar.Abstractions` and `Lodestar.Decomposition`
([#845](https://github.com/CyrilB1531/lodestar/issues/845)). Shared source is compiled into
each assembly separately, so it crosses the package boundary without an API and without a
release. The sweep moves out of `Lodestar.Decomposition.Internal.JacobiSvd` into the shared
file and `JacobiSvd.SingularValues` delegates to it, so the tree holds one copy and
SonarCloud sees no duplication.

## Speed: the SVD runs only inside a band

`p` is small and `n` is large in every fit that matters, so an `O(p³)` sweep on a `p × p`
factor is noise next to the `O(np²)` triangularization. It is still bracketed, because
"attention à la rapidité" was the condition attached to the decision. Both gates keep a cheap
bound on each side and reach the sweep only when the two disagree.

**The rank refusal**, in order:

1. `min|Rₖₖ| / max|Rₖₖ| ≤ tol` **refuses** with no sweep. Sound because `σmin ≤ min|Rₖₖ|` and
   `σmax ≥ max|Rₖₖ|`, so the ratio bounds `σmin/σmax` from above. This catches every blatant
   case — `x₂ = 2·x₁` puts an exact zero on the diagonal.
2. Otherwise `R⁻¹` is formed, which `FromTriangle` computes next anyway, and
   `1/(‖R‖_F·‖R⁻¹‖_F) > tol` **accepts** with no sweep. Sound because
   `σmax ≤ ‖R‖_F` and `σmin ≥ 1/‖R⁻¹‖_F`. The bound gives away at most a factor `p`, so this
   accepts every design with `κ₂` below roughly `1/(p·n·ε)` — 5.7e10 at n = 4 000, p = 20.
3. Only a design between the two brackets pays the sweep.

**The conditioning gate**, in order:

1. `maxᵢ(Uᵢᵢ/Dᵢ) / minᵢ(Uᵢᵢ/Dᵢ) > 200` **takes the reflections** with no sweep — the diagonal
   ratio bounds `κ₂` from below.
2. Otherwise today's `√p·‖D·U⁻¹‖_F ≤ 200` **takes the normal equations** with no sweep. This
   is the existing test, so every design that is fast today stays exactly as fast.
3. Only a design between the two pays the sweep, and there the sweep's `O(p³)` replaces a
   decision that sent the fit down the `O(np²)` reflections — so it is a saving, not a cost,
   whenever it lets the design through.

## Scope

- `src/Shared/` — the sweep, and its opt-in in `src/Directory.Build.props`.
- `Lodestar.Decomposition` — `JacobiSvd.SingularValues` delegates to it. No public API moves.
- `Lodestar.Stats.Regression` — `LeastSquares.RequireFullRank` and `WellConditioned`.
- `Lodestar.Stats.TimeSeries` — `SharedReflections.RequireFullRank`, which #978 names.

No public API is added or removed in any package, so no reference page gains an entry and no
`Version.props` moves.

## Out of scope

- The refusal's *message*. It names the offending column today (`coefficient {k}'s column`).
  A spectral test knows the rank, not the column, so the message gains the rank and keeps the
  column when the cheap bracket named one. Nothing else about the text changes.
- `TryNormalEquations`' Cholesky pivot floor (`1e-12`), which is a separate gate and is not
  what either issue reports.
