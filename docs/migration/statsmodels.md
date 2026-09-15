# statsmodels → .NET

**Verdict: mixed, and read rather than assumed.** The foundation (linear
regression, distributions, basic tests) exists, and **the OLS summary table is
now native**. Past it the answer differs by subject, which is what the readings
in [`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md)
and [`decisions/0105`](../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)
settled: **forecasting is already first-party** and is delegated; **the GLM table is
now native too**, the **time-series diagnostics** — serial correlation, stationarity and
seasonal decomposition — are native in `Lodestar.Stats.TimeSeries`; **mixed models** have no .NET package at all and wait for a caller; **instrumental-variable and panel estimators**
have none either and reproduce exactly in `linearmodels`, so they could be written
([`decisions/0135`](../decisions/0135-instrumental-variables-and-panel-estimators-have-no-incumbent-and-both-could-be-written.md)).
[`decisions/0129`](../decisions/0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)
read Meta.Numerics and the commercial Numerics.NET on top, and each carries part of what this page
used to call absent.

| statsmodels need | .NET |
| --- | --- |
| Linear regression, least squares | **Math.NET Numerics** (`Fit`, `MultipleRegression`) — the estimate only |
| `OLS(...).fit()` with its summary table | **native**: [`Lodestar.Stats.Regression`](../reference/stats-regression/ols.md) |
| `OLS(...).fit(cov_type="HC0".."HC3")` | **native**: [`CovarianceType`](../reference/stats-regression/ols/covariancetype.md). The distribution moves with it as it does in statsmodels — *z* for the coefficients, F for the overall test; [`decisions/0115`](../decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md), [#686](https://github.com/CyrilB1531/lodestar/issues/686) |
| `WLS(...).fit()`, with `cov_type="HC0".."HC3"` | **native**: [`WeightedLeastSquares`](../reference/stats-regression/wls.md), the same `OlsSummary` table. Math.NET's `WeightedRegression.Weighted` returns the coefficients only; [#768](https://github.com/CyrilB1531/lodestar/issues/768) |
| `GLS(...).fit()` with a full `sigma` | **native**: [`GeneralizedLeastSquares`](../reference/stats-regression/gls.md), the same `OlsSummary` table; a vector `sigma` is `WeightedLeastSquares` with weights `1/σ`. `GLSAR` is not here; [#771](https://github.com/CyrilB1531/lodestar/issues/771) |
| `fit(cov_type="HAC")` and `fit(cov_type="cluster")` | **native**: [`CovarianceType.Hac` and `CovarianceType.Cluster`](../reference/stats-regression/ols/covariancetype.md) on `OrdinaryLeastSquares` and `WeightedLeastSquares`, one-way clusters and the Bartlett kernel. No free .NET library read computes either (Accord.Statistics, Math.NET Numerics, Meta.Numerics, NumFlat); [#775](https://github.com/CyrilB1531/lodestar/issues/775) |
| Distributions, basic hypothesis tests | **Math.NET** (`Distributions`) for the distributions; it ships no test. The tests are **native** in [`Lodestar.Stats`](../reference/stats.md), at scipy parity, and **Meta.Numerics** 4.2.0 (MS-PL, `netstandard2.0`) carries eight of its ten families, not yet measured against it ([#756](https://github.com/CyrilB1531/lodestar/issues/756)). ⛔ *not* Accord.NET — last package October 2017, last commit November 2020; [README](README.md#unmaintained-and-why-that-is-stated-with-dates) |
| GLMs with the inference table (logit, Poisson, …) | **native**: [`Lodestar.Stats.Regression`](../reference/stats-regression/glm.md), binomial, Poisson, negative binomial with a given `alpha`, and Gamma with its estimated scale, each with `offset` and `exposure` ([#787](https://github.com/CyrilB1531/lodestar/issues/787)). ML.NET fits them too, but reports coefficient statistics for *binary logistic only*; [`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md), [#616](https://github.com/CyrilB1531/lodestar/issues/616) |
| Forecasting, seasonality and anomaly detection | **Microsoft.ML.TimeSeries** 5.0.0 (`ForecastBySsa`, `DetectSeasonality`) — first-party and MIT; [`decisions/0105`](../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) |
| `acf`, `pacf`, `acorr_ljungbox` | **native**: [`Lodestar.Stats.TimeSeries`](../reference/stats-timeseries/correlation.md), with the confidence bands; [#617](https://github.com/CyrilB1531/lodestar/issues/617). Meta.Numerics carries the autocovariance and Ljung-Box and no partial autocorrelation; Numerics.NET, commercial, carries all three |
| `adfuller`, `kpss` | **native**: [`Stationarity`](../reference/stats-timeseries/stationarity-tests.md) in `Lodestar.Stats.TimeSeries`, every `regression` and `autolag`, MacKinnon's p-value, and KPSS's clamped p-value as a property rather than a warning; [#671](https://github.com/CyrilB1531/lodestar/issues/671) |
| `seasonal_decompose` | **native**: [`SeasonalDecomposition`](../reference/stats-timeseries/seasonality.md), additive and multiplicative, `period` required. `STL` is not here; `stlnet` ships it under MIT |
| ARIMA / SARIMAX estimation, VAR, state-space | ⛔ **not written** — [`decisions/0134`](../decisions/0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md): `statsmodels`' own optimisers disagree at `1e-4`, so no corpus can pin the likelihood fits, and no free .NET package estimates them — `Cortex.TimeSeries`' `ARIMA` returns a pure autoregression's least squares under that name, with no standard errors. VAR is equation-by-equation least squares and could be written at parity; it waits for an issue. Forecasting stays with **Microsoft.ML.TimeSeries** |
| `MixedLM`: mixed and hierarchical models | ⚠️ **gap, and not written until a caller needs it** — no .NET package fits one, free or commercial, across nine read. Accord's `TwoWayAnovaModel.Mixed` and NMath's `OneWayRanova`/`TwoWayRanova` are classical ANOVA with a random or repeated factor; Infer.NET can express a hierarchical model by hand but prints no REML table. [`decisions/0130`](../decisions/0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md) has the reading, and why `MixedLM`'s own solvers disagree past what a frozen corpus can hold |
| `MNLogit`: unordered categories with the inference table | **native**: [`MultinomialLogit`](../reference/stats-regression/mnlogit.md), at statsmodels parity. Accord's `MultinomialLogisticRegression` carries a table too, and is LGPL-2.1 and archived ([`decisions/0104`](../decisions/0104-generalized-linear-models-are-written-natively.md)); ML.NET's maximum-entropy trainers return coefficients only; [#788](https://github.com/CyrilB1531/lodestar/issues/788) |
| `OrderedModel`: ordinal responses | ⛔ **not written** — [`decisions/0136`](../decisions/0136-the-multinomial-logit-is-written-and-the-ordered-model-is-not.md): its numerical Hessian and default Nelder–Mead do not reproduce at `1e-9`, and no .NET package fits one |
| `IV2SLS`, `IVLIML`, `IVGMM` (`linearmodels`; `statsmodels.sandbox` for `IV2SLS`) | ⚠️ **gap, writable** — [`decisions/0135`](../decisions/0135-instrumental-variables-and-panel-estimators-have-no-incumbent-and-both-could-be-written.md): no .NET package estimates one, free or commercial, across seven read; `linearmodels` 7.0 reproduces 2SLS, LIML and two-step GMM at `1e-15`, so a lot could be written at parity. Iterated GMM moves at `1e-6` with its tolerance and is left out |
| `PanelOLS`, `RandomEffects`, `BetweenOLS`, `FirstDifferenceOLS` (`linearmodels`) | ⚠️ **gap, writable** — the same record: nothing in .NET, and each estimator reproduces at `1e-15`; the errors follow `linearmodels`' own small-sample factors, not `statsmodels`' |

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
  has the reading, and replaces what this bullet used to claim. That gap is in the free,
  maintained packages only: the commercial Numerics.NET 10.7.0 exports the autocorrelation and
  partial autocorrelation functions, Ljung-Box, ADF and KPSS
  ([`decisions/0129`](../decisions/0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)).
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
