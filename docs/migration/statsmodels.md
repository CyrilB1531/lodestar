# statsmodels → .NET

**Verdict: mixed, and read rather than assumed.** The foundation (linear
regression, distributions, basic tests) exists, and **the OLS summary table is
now native**. Past it the answer differs by subject, which is what the readings
in [`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md)
and [`decisions/0105`](../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)
settled: **forecasting is already first-party** and is delegated; **the GLM table is
now native too**, and the **time-series diagnostics** are still being written; **mixed
models** remain nobody's, and are still a gap.

| statsmodels need | .NET |
| --- | --- |
| Linear regression, least squares | **Math.NET Numerics** (`Fit`, `MultipleRegression`) — the estimate only |
| `OLS(...).fit()` with its summary table | **native**: [`Lodestar.Stats.Regression`](../reference/stats-regression/ols.md) |
| `OLS(...).fit(cov_type="HC0".."HC3")` | **native**: [`CovarianceType`](../reference/stats-regression/ols/covariancetype.md). The distribution moves with it as it does in statsmodels — *z* for the coefficients, F for the overall test; [`decisions/0115`](../decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md), [#686](https://github.com/CyrilB1531/lodestar/issues/686) |
| `WLS`, `GLS`, cluster-robust covariances | ⚠️ **gap** — decision 0115 puts WLS next and GLS after the GLM families; cluster-robust is unscoped |
| Distributions, basic hypothesis tests | **Math.NET** (`Distributions`). ⛔ *not* Accord.NET — last package October 2017, last commit November 2020; [README](README.md#unmaintained-and-why-that-is-stated-with-dates) |
| GLMs with the inference table (logit, Poisson, …) | **native**: [`Lodestar.Stats.Regression`](../reference/stats-regression/glm.md), binomial and Poisson. ML.NET fits them too, but reports coefficient statistics for *binary logistic only*; [`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md), [#616](https://github.com/CyrilB1531/lodestar/issues/616) |
| Forecasting, seasonality and anomaly detection | **Microsoft.ML.TimeSeries** 5.0.0 (`ForecastBySsa`, `DetectSeasonality`) — first-party and MIT; [`decisions/0105`](../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) |
| Time-series diagnostics: ACF/PACF, Ljung-Box, stationarity, decomposition | **being written** — no maintained .NET package carries them; [#617](https://github.com/CyrilB1531/lodestar/issues/617) |
| ARIMA / SARIMAX estimation, VAR, state-space | ⚠️ **gap** — a later lot, not the first ([#617](https://github.com/CyrilB1531/lodestar/issues/617)) |
| Mixed and hierarchical models | ⚠️ **gap**, and the only one still unread — no .NET package has been loaded for it ([#621](https://github.com/CyrilB1531/lodestar/issues/621)) |

```csharp
using MathNet.Numerics;

// OLS y = a + b·x
(double a, double b) = Fit.Line(xs, ys);
double r2 = GoodnessOfFit.RSquared(xs.Select(x => a + b * x), ys);
```

## Pitfalls

- **Math.NET returns the estimate, not the inference.** Read through a
  `MetadataLoadContext`, every regression entry point in `MathNet.Numerics` 5.0.0
  returns coefficients, and `GoodnessOfFit` adds five whole-model scalars. Nothing
  in its 5 333 exported members is a coefficient p-value, a confidence interval, an
  adjusted R-squared, an overall F or a VIF. That gap is what
  [`Lodestar.Stats.Regression`](../reference/stats-regression/ols.md) fills, at
  `statsmodels` parity; [`decisions/0096`](../decisions/0096-ordinary-least-squares-earns-its-own-package.md)
  has the reading.
- **Time series: the forecast is the part .NET has.** `Microsoft.ML.TimeSeries`
  5.0.0 reads 34 exported types, and they carry `ForecastBySsa`, `DetectSeasonality`
  and four change-point and spike detectors — first-party, MIT, maintained. Across
  those same 34 types a search for ARIMA, autocorrelation, stationarity, Ljung-Box
  or decomposition returns **nothing**, and `Deedle` 8.1.0 returns nothing for them
  across 2 132 members: it is the time *index*, not a model. So the gap is not the
  forecaster, it is the apparatus for deciding whether a series may be modelled at
  all — [`decisions/0105`](../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)
  has the reading, and replaces what this bullet used to claim.
- **A GLM exists in .NET three times, and none of them is usable here.**
  `Accord.Statistics` 3.8.0 ships the whole stack — eleven link functions,
  `IterativeReweightedLeastSquares`, Wald tests, odds ratios, deviance — under
  **LGPL-2.1**, archived since 2017. `Microsoft.ML` 5.0.0 reports `StandardError`,
  `ZScore` and `PValue`, but for **binary logistic only**: a Poisson fit comes back
  with no statistics at all, there is no interval, no odds ratio and no AIC, and the
  standard errors live in `Microsoft.ML.Mkl.Components` behind a native MKL.
  `cs-glm` 1.0.1 has nine families and three IRLS solvers and **installs no assembly**
  — its `lib/net461/Release/` layout is not a lib asset path.
  [`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md)
  has the member listings.

> Before any native development beyond the tables that ship, weigh the **real
> need**: a regression plus a few tests is often enough, and that much now ships,
> with the GLM beside it since [#616](https://github.com/CyrilB1531/lodestar/issues/616).
> One lot is scoped past them — [#617](https://github.com/CyrilB1531/lodestar/issues/617)
> — and it starts from a reading rather than from a claim.

*Guide to be expanded as real needs arise.*
