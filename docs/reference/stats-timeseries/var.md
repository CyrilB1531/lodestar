# Vector autoregression — `Lodestar.Stats.TimeSeries`

One entry point, [`VectorAutoregression.Fit`](var/vectorautoregression-fit.md). It estimates a VAR(p): several series
that move together, each explained by every series' own past, with the table `statsmodels`' `VAR(y).fit(p)` prints —
the coefficients per equation with their standard errors, t statistics and p-values, the residual covariances, the
log-likelihood, and the four information criteria.

**Why this model and not the others.**
[Decision 0134](../../decisions/0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md) read
ARIMA, SARIMAX, VAR and state space. The likelihood-fitted three do not reproduce their own answer at the tolerance
every corpus here is held to; the VAR is least squares on the stacked lags, which `numpy.linalg.lstsq` matches at a
relative gap of `0.0`. Forecasting stays delegated to `Microsoft.ML.TimeSeries`
([decision 0105](../../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)).

## Types

| Type | What it is |
| --- | --- |
| [`VectorAutoregression`](var/vectorautoregression.md) | Fits the model and builds the table. |
| [`VarSummary`](var/varsummary.md) | The coefficients per equation, their inference, and the whole-model numbers. |
| [`VarOptions`](var/varoptions.md) | Whether each equation carries a constant. |

## See also

- [Stationarity tests](stationarity-tests.md) — what to run before fitting one.
- [Serial correlation](correlation.md) — the diagnostics that lead to a lag order.
- [statsmodels → .NET](../../migration/statsmodels.md), [Python → C# equivalence](../../equivalence.md).
