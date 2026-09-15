# 0768 — Weighted least squares, at `statsmodels.WLS` parity

**Status:** accepted, 2026-09-15. Written before the work.

Issue: [#768](https://github.com/CyrilB1531/lodestar/issues/768), split out of the econometrics umbrella
[#338](https://github.com/CyrilB1531/lodestar/issues/338).
Reading: [decision 0115](../../decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md),
which shipped the robust covariances and put `WLS` first in the order after them.

## Problem

`docs/migration/statsmodels.md` lists `WLS` as a gap and `docs/equivalence.md` as *no counterpart yet*.
A caller whose rows are not equally reliable — a mean over groups of different sizes, a measurement
whose variance is known to grow with its level — has the estimate from Math.NET's
`WeightedRegression.Weighted` and nothing past it: no standard error, no test, no interval. That is the
same gap [decision 0096](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) closed for
the unweighted fit, one parameter wider.

## Scope

- `sm.WLS(y, X, weights=w).fit()` and `.fit(cov_type="HC0".."HC3")` at `statsmodels` 0.15.0 parity, over
  every number `OlsSummary` carries.
- **Out:** `GLS` (#771, a caller-supplied covariance needs a whitening factorization, not a vector),
  cluster-robust and HAC covariances (#773), `WLS.fit_regularized`, the weighted `.predict` and the
  `wresid`/`wendog` side outputs.

## What the reference does, measured

Run against `statsmodels` 0.15.0 from `/var/tmp` before this spec was written:

1. **The fit is OLS on whitened rows.** Each row of the design — the constant column included — and
   each response is multiplied by `√w`. `params` and `bse` agree with `sm.OLS(y·√w, X·√w).fit()` to
   `np.allclose`, and so do HC0 through HC3: the robust covariances are the sandwich on the whitened
   matrix and the whitened residuals, leverages included.
2. **R² is weighted, not whitened.** With a constant, `rsquared` is `1 − SSR / Σ wᵢ(yᵢ − ȳ_w)²` with
   `ȳ_w = Σ wᵢyᵢ / Σ wᵢ` on the original response. Without one it is `1 − SSR / Σ wᵢyᵢ²`, which is the
   whitened uncentred total. `SSR` is `Σ wᵢrᵢ²` in both. The two agreed to the last printed digit.
3. **The constant is the model's, not the whitened matrix's.** `sm.OLS` on the whitened design sees no
   constant column and puts the intercept into its robust F; `WLS` knows it has one and leaves it out.
   The robust F test is therefore `WLS`'s, not the whitened OLS's — on the probe, 22 684.8 against
   79 937.0 under HC0.
4. **A zero weight keeps its row.** With one of eight weights at zero, `nobs` is 8 and `df_resid` 6.
   The row contributes nothing to the estimate and still counts in the degrees of freedom and in HC1's
   `n / (n − k)`.
5. **Bad weights are not refused.** A negative weight takes `√` of it and fails inside the SVD; `NaN`
   and infinity propagate; all-zero weights answer through the pseudo-inverse with no error.
6. **`WLS` has no VIF.** The only VIF statsmodels offers is `variance_inflation_factor(exog, i)`, which
   takes the design and no weights.

## Placement — a sibling entry point

Three candidates, the question the issue left open:

- **`OlsOptions.Weights`.** Rejected: an options record would hold a per-row array, which is data rather
  than a setting, and a record whose collection member compares by reference advertises value equality
  it does not have — [#668](https://github.com/CyrilB1531/lodestar/issues/668)'s whole subject.
- **An overload `OrdinaryLeastSquares.Fit(design, response, weights, featureCount, options)`.**
  Rejected: the class name says *ordinary*, which a weighted fit is not, and every unqualified
  `<see cref="Fit"/>` and every page that links `OrdinaryLeastSquares.Fit` becomes ambiguous between
  two overloads.
- **`WeightedLeastSquares.Fit(design, response, weights, featureCount, options)`.** Chosen. It is
  `sm.WLS` by name, it leaves `OrdinaryLeastSquares` untouched, and #771's `GeneralizedLeastSquares`
  has an obvious place beside it. It takes `OlsOptions` and returns `OlsSummary` because the table and
  the settings are the same ones; statsmodels returns the same `RegressionResults` from both.

## Behaviour

- **Parity** on points 1 to 4 above: whitening, weighted R² and adjusted R², the model's own constant
  in the F test, zero weights kept in the degrees of freedom.
- **Refused**, each a divergence recorded in `docs/equivalence.md`:
  - `weights` of a different length than `response` — `ArgumentException` naming `weights`;
  - a negative, `NaN` or infinite weight — `ArgumentOutOfRangeException` naming `weights`, where the
    reference fails in its SVD or returns `NaN`;
  - **fewer positively weighted rows than parameters** — `ArgumentException`, where the reference
    answers with the minimum-norm solution. All-zero weights are the extreme of this rule. The same
    rank deficiency on the design itself is not caught here, as it is not in `OrdinaryLeastSquares.Fit`.
- **VIF on the unweighted design**, which is the number `variance_inflation_factor` returns for the same
  `exog`; the corpus freezes it that way.
- **Equal weights reproduce `OrdinaryLeastSquares.Fit` bit for bit.** `√1` is exactly one, the weighted
  mean of unit weights is the plain one, and the pipeline is shared — a test pins it with `Assert.Equal`.

## Implementation shape

`OrdinaryLeastSquares.Fit` keeps its validation and hands everything from the solve onward to an
internal `Summarise(matrix, response, unweighted, totalSumOfSquares, rowCount, parameterCount, settings)`.
`WeightedLeastSquares.Fit` validates the weights, whitens a copy of `LeastSquares.Design` and the
response, computes the weighted total sum of squares, and calls the same method with the unweighted
matrix for the VIFs. No arithmetic in the OLS path moves, so `stats_ols.json` replays unchanged.

## Evidence

- `tests/oracles/stats_wls.json` from a new `generate_stats_wls`, nonrobust and HC0–HC3, with and
  without a constant, a zero weight, unit weights, constant weights, a 99% level and a near-collinear
  pair — every number `generate_stats_ols` freezes, compared at the same relative `1e-9`.
- Edge tests for each refusal, the unit-weight identity and the constant-weight scaling.

## Rejected along the way

- **Dropping zero-weight rows.** The issue's first wording said so; the reference does not, and every
  degree-of-freedom-dependent number would move.
- **VIF on the whitened design.** It answers a different question — collinearity of the rows as
  weighted — and no statsmodels call returns it, so nothing could hold it to parity.
