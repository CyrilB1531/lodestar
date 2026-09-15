# 0771 — Generalized least squares with a caller-supplied error covariance, at `statsmodels.GLS` parity

**Status:** accepted, 2026-09-15. Written before the work.

Issue: [#771](https://github.com/CyrilB1531/lodestar/issues/771), split out of the econometrics umbrella
[#338](https://github.com/CyrilB1531/lodestar/issues/338).
Reading: [decision 0115](../../decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md),
which puts `GLS` fourth and after `WLS` (#768, shipped in #774); not amended. The weighted fit's spec,
[`2026-09-15_0768_weighted-least-squares.md`](2026-09-15_0768_weighted-least-squares.md), set the placement
this one follows.

## Problem

Weighted least squares handles errors that are independent with unequal variances. Errors that are
**correlated** — neighbouring measurements in time or space, repeated readings of one unit — need the whole
covariance, and an OLS or WLS table on them reports standard errors that are too small. `GLS` takes that
covariance from the caller, whitens the rows by its Cholesky factor, and fits.

## Scope

- `sm.GLS(y, X, sigma=Σ).fit()` and `.fit(cov_type="HC0".."HC3")` with `Σ` a full `n × n` matrix, at
  `statsmodels` 0.15.0 parity over every number `OlsSummary` carries.
- **`sigma` as a vector** is `WLS` with weights `1/σ`, measured below; it is served by
  `WeightedLeastSquares.Fit` and pinned by a test, not by a second entry point.
- **Out:** `GLSAR` and any feasible GLS that estimates `Σ` (a later lot, as the issue names it), a scalar
  `sigma`, `GLS.loglike` (no `OlsSummary` field carries a likelihood), `fit_regularized`.

## What the reference does, measured

Read from `statsmodels/regression/linear_model.py` 0.15.0 and run from `/var/tmp`:

1. **Whitening.** `_get_sigma` takes `L = cholesky(Σ, lower=True)` and its inverse `L⁻¹` through LAPACK's
   `dtrtri`; `whiten(x)` is `L⁻¹ x` for the design (constant column included) and the response. The fit is
   OLS on the whitened rows, robust covariances included.
2. **A non-positive-definite `Σ`** raises `LinAlgError` from `potrf`. **An asymmetric `Σ` is accepted
   silently**: `cholesky` reads the lower triangle, so setting `Σ[0,1] = 0.9` on an AR(1) matrix leaves the
   estimates unchanged.
3. **R² with a constant** is centred on the mean *estimated in whitened space*:
   `m = (L⁻¹y)·(L⁻¹1) / (L⁻¹1)·(L⁻¹1)`, then `Σ (L⁻¹(y − m))²`. Without a constant it is `Σ (L⁻¹y)²`. With a
   diagonal `Σ` both reduce to `WLS`'s weighted forms exactly.
4. **The constant is the model's**, detected on the unwhitened design, so the robust Wald test leaves it
   out, as for `WLS`.
5. **Vector `sigma`.** `sm.GLS(y, X, sigma=s)` against `sm.WLS(y, X, weights=1/s)`: parameters within
   `2.7e-15`, standard errors within `4.4e-16`, R² identical, F within `2.8e-13`. A diagonal *matrix* `Σ`
   agrees with both to the same digits.
6. **An AR(1) covariance** `Σᵢⱼ = 0.6^|i−j|` on ten rows: slope `1.97118644`, standard error `0.29237649`,
   R² `0.85033814`, F `45.4538`, and HC1 standard error `0.2473294` — the probe the corpus starts from.

## Placement — a sibling entry point

`GeneralizedLeastSquares.Fit(design, response, covariance, featureCount, options)`, beside
`WeightedLeastSquares`, returning `OlsSummary` and taking `OlsOptions`. `covariance` is row-major,
`n × n`. The #768 spec's reasons carry over: a per-row or `n²` array does not belong on an options record
(#668), and an `OrdinaryLeastSquares.Fit` overload would make every `Fit` link ambiguous.

**Rejected:** accepting a length-`n` vector in the same span and dispatching on length. A span whose meaning
depends on its length is a call that type-checks and means something else; `WeightedLeastSquares` already
is the diagonal case.

## Behaviour

- **Parity** on points 1, 3, 4 and 5; point 6 is in the corpus.
- **Whitening without an explicit inverse.** `L⁻¹ X` is computed by forward substitution against `L`, one
  column at a time, rather than by forming `L⁻¹` as the reference does; the two agree to rounding, inside the
  corpus's `1e-9` for the well-conditioned covariances frozen there.
- **The pipeline is `OrdinaryLeastSquares.Summarise`**, fed the whitened design and response, the GLS total
  sum of squares, and the unwhitened design for the VIFs — as `WeightedLeastSquares` feeds it.
- **The Cholesky** is written in `Lodestar.Stats.Regression.Internal`. `Lodestar.Survival` has one, internal
  to that package, and this package does not depend on it.
- **Refused**, each recorded in `docs/equivalence.md`:
  - `covariance` whose length is not `n²` — `ArgumentException` on `covariance`;
  - a non-finite entry — `ArgumentOutOfRangeException` on `covariance`;
  - an **asymmetric** `covariance`, beyond a relative `1e-12` of the larger entry of the pair —
    `ArgumentException`, where the reference silently reads the lower triangle (point 2);
  - a covariance that is **not positive definite**, a pivot not above `1e-12` of its diagonal —
    `ArgumentException`, as the reference raises `LinAlgError`.
- **VIFs** on the unwhitened design, as for `WLS`: `variance_inflation_factor` takes no covariance.

## Evidence

- `tests/oracles/stats_gls.json` from `generate_stats_gls`, sharing `_linear_case` with the OLS and WLS
  generators: AR(1) at `ρ` 0.6 and 0.3, with and without an intercept, a two-regressor design, an
  equicorrelated block covariance, a 99% level, and HC0 to HC3 on the AR(1) case — compared at the
  repository's relative `1e-9`. Wald statistics kept to the hundreds, as #768's corpus does.
- Edge tests: each refusal; a diagonal `covariance` reproduces `WeightedLeastSquares.Fit` with weights `1/σ`
  to `1e-12`; the identity covariance reproduces `OrdinaryLeastSquares.Fit`; the Cholesky's forward
  substitution against a hand 3 × 3.
