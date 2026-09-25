# Instrumental variables: 2SLS, LIML and two-step GMM, at linearmodels parity

**Issue:** [#1155](https://github.com/CyrilB1531/lodestar/issues/1155).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`docs/equivalence.md` carries `IV2SLS(y, exog, endog, instruments).fit()`, `IVLIML` and `IVGMM`
(two-step) from `linearmodels` as *no counterpart yet*. No .NET library publishes an
instrumental-variables estimator with its inference table
([decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md) reads them as
writable closed forms).

## Decisions

1. **One static class, three estimators.** `Lodestar.Stats.Regression.InstrumentalVariables` with
   `TwoStageLeastSquares`, `Liml` and `Gmm`, each taking the response, then the exogenous regressors,
   the endogenous regressors and the excluded instruments as row-major blocks with their column
   counts, as `OrdinaryLeastSquares.Fit` takes its design; a second overload of each takes one
   cluster label per row. Coefficients come exogenous first, then endogenous, the intercept first
   when `WithIntercept` adds it — `linearmodels`' order for `x = [exog, endog]`.
2. **The data types live in `Lodestar.Abstractions`, in their own namespace**,
   `Lodestar.Stats.Regression.Instrumental` — not `…InstrumentalVariables`, which would share its full name
   with the class — as Cyril asked: `IvCovarianceType`
   (`Unadjusted`, `Robust`, `Kernel`, `Clustered` — the reference's four, rather than
   `CovarianceType`, whose HC1–HC3 and statsmodels semantics do not exist there), `IvKernel`
   (`Bartlett`, `Parzen`, `QuadraticSpectral`), `IvOptions`, `IvSummary`, `IvFirstStage` and
   `IvTest`. `Lodestar.Stats.Regression` takes an unconditional `ProjectReference` to
   `Lodestar.Abstractions` until the next publication, with its line in `ci.yml`'s exception and
   `EXPECTED`'s floor, as #1142 did.
3. **Defaults are the reference's**: `CovarianceType = Robust` (`fit()`'s default), `Debiased =
   false`, `Kernel = Bartlett`, `Bandwidth = null` (Newey and West's automatic choice),
   `Fuller = 0`, and for GMM `GmmWeightType = Robust`, with a kernel weight's bandwidth defaulting
   to `n − 2` as `KernelWeightMatrix` does.
4. **Behaviour replayed**, each point checked by an independent numpy reading against
   `linearmodels` 7.0 on 600 random cases before any C# (0 differences past `1e-8`):
   - `κ` for LIML is the smallest eigenvalue of `(E₁ᵀE₁)` against `(E_zᵀE_z)`, with `E = [y, X_endog]`
     annihilated by the exogenous block and by the instruments; Fuller subtracts `α / (n − L)`.
   - Covariances are `V⁻¹ S V⁻¹ / n` over `x̂ = P_z x`; `debiased` scales `S` by `n / (n − k)`, and
     the clustered one takes `G/(G−1)·(n−1)/n` **only when debiased**.
   - The automatic bandwidth runs Newey and West's rule on the scores summed over the columns
     that are not the constant, the constant found by numpy's rank test on `x̂` — not by equality,
     since a projected constant is not exactly one.
   - Two-step GMM starts from `(ZᵀZ/n)⁻¹`, re-weights once with the chosen weight type (the
     homoskedastic weight centres the residuals), and reads its covariance through the same score
     estimators.
   - The first-stage diagnostics reuse the fit's covariance configuration, **the bandwidth the fit
     chose included**; the first-stage F is an F for the unadjusted covariance and a χ² otherwise.
5. **Two readings differ from the reference, both about precision, not value.** p-values are the
   precise tail rather than `2 − 2·cdf`, which returns 0 below `1e-16`; and the overidentification
   test is `null` when the model is just identified, where `linearmodels` returns a J of
   rounding noise on zero degrees of freedom.

## What the work found

- **Eight parameters were too many for one signature** (S107): the response and three blocks are
  one `IvDesign`, a `readonly ref struct` in `Lodestar.Abstractions`, so the spans are read in
  place and each estimator takes `(design, options)` or `(design, clusters, options)`.
- **A type in `Lodestar.Abstractions` is checked by the package that forwards it.** The IV data
  types never lived in `Lodestar.Stats.Regression`, and it forwards them anyway, so its reference
  gate and its wiki page own them as they own the types #1142 moved.
- **Seventeen of 1,498 random cases differ past `1e-9`, every one ill-conditioned**: GMM weights from a
  quadratic-spectral kernel at `n − 2` lags (condition numbers `1e4` to `3e10`), covariances past
  `1e7`, or statistics that cancel — an adjusted R² of `0.004`, a Sargan statistic of `2e-5`. The
  corpus leaves out the one case whose covariance exceeds `1e6` in condition number, where no
  digit survives a `1e-9` comparison, and says so in the generator.
- **The kernel covariance is summed lag by lag, as the reference sums it.** Smoothing each row over
  its neighbours instead costs `n·m·k` rather than `n·m·k²`, and moved an ill-conditioned model test
  by `2e-9`.

- **The code review found three defects, all fixed here**: the inverse compared each pivot to the
  largest entry, so a badly scaled design read as collinear (every inverted matrix is now equilibrated
  by its diagonal first, which also brought the random differences from twenty cases to seventeen); a
  bandwidth of `int.MaxValue` overflowed the kernel's weights (they stop at the sample's last lag);
  and options an estimator does not read were ignored (they are refused, as `OlsOptions.HacLags` is).

## Proof

- `tests/oracles/stats_iv.json` from `linearmodels` 7.0: just- and over-identified models, one
  and two endogenous regressors, with and without an intercept, every covariance and kernel, fixed
  and automatic bandwidths, debiased, Fuller, and each GMM weight type, compared at `1e-9`.
- A random differential run against `linearmodels` before the push.
- `linearmodels` (NCSA), `pyhdfe` and `mypy_extensions` (MIT) are pinned in
  `tools/requirements.txt` and the lock; decision 0002's rule admits permissive references.

## Knock-on changes

Reference pages, `docs/equivalence.md` rows replacing the *no counterpart yet* one, a sample,
the changelog, and a benchmark against `linearmodels` in the package's `performance.md`, which
also records that no .NET incumbent exists.
