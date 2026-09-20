# 0786 — Vector autoregression with the inference table, at `statsmodels` parity

**Status:** written before the work, 2026-09-16.

Issue: [#786](https://github.com/CyrilB1531/lodestar/issues/786).

Reading: [decision 0134](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md), which read
ARIMA, SARIMAX, VAR and state space, wrote none of them, and named VAR "the one model of the four that could be written
at parity": `statsmodels.tsa.api.VAR(...).fit(p)` matched `numpy.linalg.lstsq` on the stacked lags to a relative gap of
`0.0`. Not amended or applied since. [Decision 0105](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)
delegates *forecasting* to `Microsoft.ML.TimeSeries`; this is estimation with its table, which that package does not do.

## Problem

A caller with several series that move together — an inflation and an unemployment rate, two demand streams — has no
way to estimate how each depends on the others' history, and no table for the coefficients. `Lodestar.Stats.TimeSeries`
publishes the diagnostics (autocorrelation, Ljung-Box, stationarity, decomposition); the model those diagnostics lead to
is missing, and nothing in .NET estimates one (0134's survey, re-read below).

## What the reference does, read and measured

`statsmodels` 0.15.0, on 200 rows of a two-variable series with `p = 2`:

1. **The fit is least squares, equation by equation**, on the stacked lags with a constant when `trend="c"`. The
   estimate matched `numpy.linalg.lstsq` on that design to `0.0`.
2. **Shapes.** `params` is `(1 + K·p) × K`: one column per equation, the constant first, then lag 1's `K`
   coefficients, then lag 2's. `trend="n"` drops the constant row. `nobs` is `T = n − p`.
3. **Standard errors** are the per-equation OLS errors: `σ_jj·(ZᵀZ)⁻¹` with `σ_jj = SSEⱼ/(T − 1 − K·p)`, which is the
   diagonal of `sigma_u`.
4. **The tests read the normal**, not Student's t: `pvalues = 2·Φ̄(|t|)`, measured to the last bit. **The reference
   publishes no intervals at all** — `VARResults` has no `conf_int`.
5. **The residual covariances.** `sigma_u = S/(T − df_model)` and `sigma_u_mle = S/T`, with `S = residualᵀresidual` and
   `df_model = 1 + K·p` (or `K·p` without the constant).
6. **The whole-model numbers**, each reproduced here from `Σ̂ = sigma_u_mle` and `m = K²p + K`:
   - `llf = −TK/2·log(2π) − T/2·log|Σ̂| − TK/2`;
   - `aic = log|Σ̂| + 2m/T`;
   - `bic = log|Σ̂| + m·log(T)/T`;
   - `hqic = log|Σ̂| + 2m·log(log(T))/T`;
   - `fpe = |Σ̂|·((T + df_model)/(T − df_model))^K`.

   Each matched the reported value to the last digits.

## Placement

- **Package:** `Lodestar.Stats.TimeSeries`, which already depends on `Lodestar.Stats` and `Lodestar.Stats.Regression`.
  The fit is `OrdinaryLeastSquares.Estimate` per equation — the published surface, not an internal — so the lag design
  is built here and the solve is the one the regression package already ships.
- **Surface:**

  ```csharp
  VarSummary VectorAutoregression.Fit(
      ReadOnlySpan<double> series, int variableCount, int lagOrder, VarOptions? options = null)
  ```

  `series` is row-major in time: `variableCount` values per observation, oldest first.
- **`VarOptions`:** `WithIntercept` (true, the reference's `trend="c"`).
- **`VarSummary`:** `Coefficients`, `StandardErrors`, `TStatistics` and `PValues`, each one list per equation in the
  reference's row order; `ResidualCovariance` and `ResidualCovarianceMaximumLikelihood` row-major `K × K`;
  `LogLikelihood`, `Akaike`, `Bayesian`, `HannanQuinn`, `FinalPredictionError`; `LagOrder`, `VariableCount`,
  `ObservationsUsed`, `ModelDegreesOfFreedom`, `ResidualDegreesOfFreedom`, `HasIntercept`.
- **Rejected:**
  - **Confidence intervals.** The reference publishes none for this model, and [decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
    says a member waits for a caller who needs it.
  - **`Lodestar.Stats.Regression` as the home.** The estimator is a time-series model, and its lag design and
    information criteria belong beside the diagnostics that lead to it.
  - **A `coefs`-shaped `K × K × p` member.** The reference has both; one layout, the one its table prints, is enough
    until a caller asks.

## Behaviour

- **Parity** on points 1 to 6, `nobs`, `df_model` and `df_resid` included.
- **Out of scope**, each its own issue if a caller asks: lag-order selection (`select_order`), impulse responses,
  forecast-error variance decomposition, Granger causality (`test_causality`), the `"ct"` and `"ctt"` trends, and
  exogenous regressors.
- **Refused:**
  - fewer than two variables, which is an autoregression rather than a vector one;
  - a lag order below one;
  - a series that is not a whole number of observations;
  - fewer observations than the fit has parameters, where no residual degree of freedom is left;
  - a non-finite value.

## Evidence

- **`stats_var.json`**, frozen from `statsmodels` 0.15.0: two variables at lag 1 and lag 2, three variables at lag 2,
  a fit without an intercept, and a series long enough that the criteria separate. Every field replayed at `1e-9`.
- **Edge tests:** each refusal; one variable's equation against `OrdinaryLeastSquares.Fit` on the same lag design, to
  `1e-12`; and `sigma_u` against the residuals computed by hand.
- **Benchmark:** no .NET package estimates a VAR (0134, re-read for this lot), so the incumbent is `statsmodels`
  through a `compare-var` harness over the benchmark corpus, plus a `VectorAutoregressionBenchmarks` class for the
  allocations the harness does not see.
