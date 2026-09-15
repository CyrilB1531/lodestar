---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0095", "0105", "0130"]
---
# 0134 — ARIMA, SARIMAX and state-space models are not written; VAR is the one that could be

**Status:** accepted · **Date:** 2026-09-15 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0105`](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) (as amended by [`0129`](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)), [`0130`](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md)

## Context

[Decision 0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) delegated forecasting
to `Microsoft.ML.TimeSeries`, wrote the diagnostics, and left ARIMA, SARIMAX, VAR and state-space *estimation* to a
later lot. The lot named for it, #617, closed on the diagnostics, and `docs/migration/statsmodels.md` kept pointing at
it. [#772](https://github.com/CyrilB1531/lodestar/issues/772) exists to read the subject before anything is claimed,
and allowed the verdict to be *not written*. 0130 had just found `statsmodels`' `MixedLM` unpinnable at the corpus
tolerance, and a time-series model is a likelihood optimisation too, so that finding was the first thing to check.

## The searches

nuget.org, 2026-09-15:

| query | hits | anything that estimates the model |
| --- | ---: | --- |
| `arima` | 4 | `Cortex.TimeSeries` 1.1.0; `Dew.Stats`, `Dew.Stats.Core`, `Dew.Stats.Linux` 6.3.10 — read below |
| `sarima`, `vector autoregression` | 0 | — |
| `kalman` | 18 | none — signal filters (`MathNet.Filtering.Kalman`), GPS smoothing, trading strategies |
| `state space` | 22 | none — PDF, planning and UI libraries |
| `time series forecasting` | 9 | `Cortex.TimeSeries`, `Dew.Stats.Core` again; `NW.UnivariateForecasting`, `Tsfm.Forecasting` forecast without an ARIMA estimator |

## The reading

Surfaces with `tools/survey.cs` ([decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)),
licences from the package ([decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)), pattern
`(Arima|ARIMA|Sarima|VectorAutoregress|VarModel|StateSpace|Kalman|UnobservedComponent|ExponentialSmoothing|Garch)`:

| package | licence, from the package | surface | what the pattern returns |
| --- | --- | --- | --- |
| `Cortex.TimeSeries` 1.1.0 | MIT, `<license type="expression">` | 27 types, 130 members | `ARIMA(p, d, q)`, `SARIMA`, `AutoARIMA`: AR and MA coefficients, intercept, AIC, BIC, forecasts; no standard error, no likelihood, no VAR, no state space |
| `Numerics.NET` 10.7.0 | commercial ([0129](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)) | 13,839 members | `ArimaModel`, `ExponentialSmoothingModel`, `GarchModel`; no VAR, no SARIMAX, no state space |
| `Dew.Stats.Core` 6.3.10 | commercial ([0130](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md)) | 727 members | `ARIMASimulate` — a simulator, no estimator |
| `Microsoft.ML.TimeSeries` 5.0.0 | MIT | read for 0105 | singular spectrum analysis; no ARIMA |

The commercial libraries are read and not run: nothing is timed or fitted under a trial licence. Julia's `StateSpaceModels.jl` was not measured: no Julia toolchain is installed here, and `statsmodels` is the reference every corpus in this repository is frozen from, so a second reference would not change what parity means.

### What `Cortex.TimeSeries`' ARIMA estimates

One series of 300 points simulated from ARMA(2,1) with `φ = (0.6, −0.2)`, `θ = 0.3` and a fixed numpy seed, fitted as
ARIMA(2,0,1) with a constant on both sides:

| estimator | φ₁ | φ₂ | θ₁ |
| --- | ---: | ---: | ---: |
| `statsmodels` `ARIMA(...).fit()` — state-space MLE | 0.5312 | −0.1028 | 0.4706 |
| `statsmodels`, `method="innovations_mle"` | 0.5311 | −0.1028 | 0.4706 |
| conditional sum of squares, fitted with `scipy` | 0.5347 | −0.1048 | 0.4701 |
| Hannan–Rissanen, `statsmodels` | 0.6076 | −0.1563 | 0.3862 |
| **AR(2) by ordinary least squares, the MA term ignored** | **0.9365** | **−0.3837** | — |
| **`Cortex.TimeSeries` `ARIMA(2, 0, 1).Fit`** | **0.9337** | **−0.3787** | 0.0466 |

Every ARMA estimator lands near `(0.53, −0.10, 0.47)`; Cortex lands on the pure autoregression's coefficients, and its
AIC reads 47.8 where the likelihood's is 895.8. **It does not estimate an ARIMA model** in the sense a `statsmodels` caller
means, so it cannot be delegated to as one — and, reported with no standard errors, it would not carry the inference
table this project's thesis is about even if it did.

## What parity would mean

The same series, fitted by `statsmodels` 0.15.0 with its own optimisers:

| comparison | largest relative gap in the parameters |
| --- | ---: |
| `ARIMA.fit()` (state space) against `method="innovations_mle"` | 8.8e-5 |
| `SARIMAX.fit()` L-BFGS against Nelder–Mead | 2.6e-4 (log-likelihoods −442.89174442 and −442.89174456) |
| `SARIMAX.fit(method="bfgs")` | 1.27 — reports non-convergence |

**The reference does not pin its own answer to the tolerance every corpus here is held to.** A corpus frozen from one
optimiser is a record of that optimiser's stopping point, and a C# fit that agreed with it at `1e-9` would be agreeing
with a path, not with the model: 0130's finding for `MixedLM`, measured again. State-space models (`UnobservedComponents`,
`DynamicFactor`) are the same likelihood optimised the same way.

**VAR is not.** `statsmodels.tsa.api.VAR(...).fit(p)` is equation-by-equation least squares on the stacked lags, and on a
two-variable series it matches `numpy.linalg.lstsq` on that design to a relative gap of `0.0`. Its standard errors,
`t` statistics and information criteria are the least-squares table `Lodestar.Stats.Regression` already builds.

## Decision

1. **ARIMA, SARIMAX and state-space estimation are not written.** There is no parity target at the corpus tolerance, and
   no free .NET incumbent to delegate to: `Cortex.TimeSeries`' ARIMA is a pure autoregression's least squares under an
   ARIMA name. Forecasting stays delegated to `Microsoft.ML.TimeSeries` (0105).
2. **The lot waits for a caller**, under [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
   rule. The caller who reopens it inherits two questions this record cannot answer: what tolerance a likelihood fit is
   held to (a log-likelihood match, not parameters, is the candidate), and which of the reference's optimisers is the
   reference.
3. **VAR is the one model of the four that could be written at parity**, as a least-squares system with the table beside
   it, and no .NET package read here estimates one. It is not written by this record: whether it earns a lot is a
   question for an issue, proposed rather than opened.
4. **`docs/migration/statsmodels.md`** points at this record instead of the closed #617, and names `Cortex.TimeSeries`'
   ARIMA for what it computes.

### Rejected

- **Writing ARIMA against a looser tolerance now.** A tolerance chosen to make the corpus pass is a tolerance chosen by
  the implementation; with no caller to say what agreement they need, there is nothing to choose it by.
- **Recommending `Cortex.TimeSeries` for ARIMA.** Its coefficients are a different model's.
- **Timing `Numerics.NET`'s `ArimaModel`.** Commercial, trial licence; read only.

## Consequences

- The migration row for ARIMA/SARIMAX/VAR/state space reads *not written*, with VAR named as the writable one.
- #772 closes on this record. A VAR lot, if Cyril wants one, is a new issue that starts from the least-squares identity
  measured above.
