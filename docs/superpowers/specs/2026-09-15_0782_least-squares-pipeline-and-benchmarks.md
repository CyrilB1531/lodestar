# 0782 — The least-squares pipeline, measured against its incumbents and rebuilt where it lost

**Status:** accepted, 2026-09-15. Written during the work, from its measurements: the benchmarks this records were owed
by #774 and #778, and what they found changed the scope from a benchmark to a refactor.

Issues: [#782](https://github.com/CyrilB1531/lodestar/issues/782) (weighted least squares against its incumbent) and
[#781](https://github.com/CyrilB1531/lodestar/issues/781) (the negative binomial GLM against its incumbent), one pull
request. Rule: every lot of a `Lodestar.Stats.*` package carries a benchmark against its natural incumbent.

## Problem

`WeightedLeastSquares.Fit` (#768) and `GlmFamily.NegativeBinomial` (#769) merged with `statsmodels` fixtures and no
measurement. Measured, `main` lost:

| comparison | `main` |
| --- | --- |
| WLS against Math.NET `WeightedRegression.Weighted`, 200 to 200,000 rows | **3.0× to 5.2× slower** |
| negative binomial GLM against `statsmodels`, 10,000 and 100,000 rows | 1.54× and 1.27× slower, wall clock |

Math.NET returns the coefficients alone and Lodestar the whole table, which explains part of the gap and not 5×.

## Incumbents

- **WLS and OLS: Math.NET Numerics 5.0.0**, MIT, maintained, the numerics library `src/` already reaches through
  `Lodestar.Extensions.MathNet`. `WeightedRegression.Weighted` and `MultipleRegression.QR`.
- **Negative binomial GLM: `statsmodels` 0.15.0**, through the cross-language harness. Accord's generalized linear
  regression weights IRLS by the link's derivative alone, right only for a canonical link; the commercial libraries are
  not timed under a trial licence.

## What the profile found

A Stopwatch replay of `Summarise`'s phases on `main`, 20,000 rows of four regressors: forming Q explicitly in the solve,
35 % of a weighted fit; a second QR with Q formed for the VIFs, 25 %; everything else — copies, residuals, tails — under
10 % each. `OrdinaryLeastSquares.Estimate`, which never forms Q, ran the same QR in a third of the time.

## Decisions

1. **Q is never formed.** `LeastSquares.Solve` applies the Householder reflections to the response, as
   `HouseholderEstimate` already did, and the two share the code. The leverages the robust covariances need are rows of
   `X R⁻¹` against themselves — for a full-rank design `Q = X R⁻¹`. The explicit-Q `QrDecomposition` stays only in the
   Wald test's `p × p` block.
2. **The reflections' loops are unrolled four terms at a time**, scalar, so both target frameworks add in one order.
3. **The VIFs come from the standardised regressors' Gram matrix** — two passes over the rows and a `p × p` Cholesky —
   when every factor stays at or below 1e5; the QR route answers past that, or when the Gram matrix is not safely positive
   definite. Measured: 2.8e-11 from `statsmodels` at VIF 5.9e4, against the QR route's 1.3e-12; both inside 1e-9.
4. **The solve takes the normal equations when the design allows**: `XᵀWX = UᵀU`, used only when the ratio of `U`'s
   diagonal entries is at most 200 (`200²·ε ≈ 9e-12`); the reflections answer otherwise. `U` stands in for R everywhere R is
   read. A test holds the two routes to 1e-12 on a well-conditioned design and pins a near-collinear one to the reflections.
5. **A weighted fit applies its weights inside the solve** and computes √w-scaled residuals from the design as given; the
   whitened design is built only for a robust covariance. **The design is read in place**, its constant column implied,
   rather than copied with it.
6. **IRLS stays on the reflections.** The first A/B/A had it on the normal equations: at a Poisson mean of 5e6 their
   rounding kept the absolute deviance change above `1e-8` for 19 iterations where `main` stopped at 9, and the fit
   regressed 34 %. On the reflections it stops at 4. The reference stops at 13 on those counts, where the change sits at
   the tolerance and rounding decides; no fixture holds counts that large, on purpose.
7. **The negative binomial log-likelihood reads `lnΓ(1/α)` once**, not per row: about a tenth of each fit.

**Rejected:** SIMD through `Vector<T>` in the reflections (a second deliberate difference between target frameworks,
where `VectorMath.Dot` is the only one); a Gram-matrix VIF with no ceiling (its error grows with the factor); normal
equations inside IRLS (decision 6); publishing the WLS loss and optimising in a later lot (Cyril, same day: no merge of the
WLS half below parity).

## Evidence

- A/B/A under the lock, `main` at `94e13f2d` against this branch, default job, in `docs/guides/performance.md`: WLS 4.4× to
  7.9× faster than `main` and 1.00× to 1.94× Math.NET's speed; OLS 3.3× to 6.1× Math.NET's; HC0–HC3 2.8× faster than
  `main`; the logistic GLM ahead of Accord in three cells of four and 4 % behind in the fourth; the Poisson GLM 1.4× to 2.8×
  faster than `main`; the negative binomial GLM 1.35× to 3.56× `statsmodels`' wall clock.
- Every OLS, WLS, GLM, robust-covariance and log-likelihood fixture at 1e-9; `LeastSquaresTests` for the two routes;
  `RobustCovarianceTests` holding the leverages to the thin Q to 1e-14.
