---
status: accepted
supersedes: []
amends: []
applies: ["0074"]
---
# 0105 — The time-series forecast is delegated to `Microsoft.ML.TimeSeries`, and the diagnostics are the gap

**Status:** accepted · **Date:** 2026-09-11

## Context

[#338](https://github.com/CyrilB1531/lodestar/issues/338)'s second subject is the time index.
[Decision 0096](0096-ordinary-least-squares-earns-its-own-package.md) put it on that issue's side
of the line; [decision 0104](0104-generalized-linear-models-are-written-natively.md) took the
first subject and this one takes the second. The method is 0104's, and is not restated.

This reading had a claim of our own to check, which is the unusual part.
`docs/migration/statsmodels.md` has said, since before anything was read:

> **Time series.** Nothing equivalent to `SARIMAX`/`statespace`: either restrict the scope, or
> make it a native lot of its own.

[Decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) exists because a
sentence like that is exactly what turns out to be false, and 0096 had already replaced one:
[#442](https://github.com/CyrilB1531/lodestar/issues/442)'s "nobody in .NET does *inference*".
This one is false too, and in the direction that matters: the forecast is the part .NET already
has.

## The reading

Read on **2026-09-11**, by 0104's method.

### `Microsoft.ML.TimeSeries` 5.0.0 — forecasting ships, first-party

MIT, published 2025-11-11, **2 851 378 downloads**, `dotnet/machinelearning` pushed 2026-09-10
with a 6.0.0 preview out this month. 34 exported types, 124 members. Its whole catalogue:

```text
Microsoft.ML.TimeSeriesCatalog
  ForecastBySsa
  DetectSeasonality
  DetectIidSpike, DetectIidChangePoint
  DetectSpikeBySsa, DetectChangePointBySsa
  DetectAnomalyBySrCnn, DetectEntireAnomalyBySrCnn
  LocalizeRootCause, LocalizeRootCauses
Microsoft.ML.Transforms.TimeSeries
  SsaForecastingEstimator, SsaForecastingTransformer
  TimeSeriesPredictionEngine<TSrc,TDst>
```

Singular spectrum analysis forecasting, seasonality detection, four change-point and spike
detectors and an anomaly detector, all first-party and maintained.

And across those same 34 types and 124 members, a search for
`Arima|Autocorrel|Acf|Pacf|Stationar|LjungBox|Dickey|Decompos` returns **zero matches**. There is
no ARIMA, no autocorrelation function, no stationarity test, no Ljung-Box, and no decomposition
that hands back trend, seasonal and residual.

So the migration page's sentence is true only of `SARIMAX` by name. Read as written — nothing
equivalent to state-space forecasting — it is false, and this record replaces it.

### `Deedle` 8.1.0 — the index, and no model

BSD-2-Clause per `fslaborg/Deedle` (the package publishes **no licence field**, the trap 0104
records), 1 005 stars, pushed 2026-09-10. 210 exported types, 2 132 members: `Series`, `Frame`,
the `Aggregation` windowing family, resampling and rolling statistics.

A search across all 2 132 members for `Arima|Autocorrel|Acf|Stationar|LjungBox|Forecast|Seasonal`
returns **zero matches**. Deedle is the time *index* and the moving window over it. It is not a
model and does not claim to be — the migration hub already points at it for exactly that, and that
row is correct.

### `Cortex.TimeSeries` 1.1.0 — nearly the whole surface, and nobody is using it

MIT, published 2026-03-30, **240 downloads**, and **no source repository listed on the package**.
27 exported types, 130 members:

```text
Cortex.TimeSeries.Models
  ARIMA, SARIMA, AutoARIMA, ExponentialSmoothing,
  SimpleMovingAverageForecast, Backtesting, InformationCriterion
Cortex.TimeSeries.Decomposition
  SeasonalDecompose, Periodogram, FFTPeriodogram
Cortex.TimeSeries.Diagnostics
  AutocorrelationTests, LjungBoxResult, StationarityTests, StationarityTestResult
Cortex.TimeSeries.Features
  LagFeatures, RollingFeatures, FourierFeatures, DateTimeFeatures
```

That is very nearly what `statsmodels.tsa` offers, in a package six months old with 240 downloads
and no repository to read. It also depends on `Cortex.ML` and `Cortex` — an ecosystem edge, which
bars it from a core package under
[decision 0076](0076-a-core-package-carries-no-external-dependency.md) whatever it exports and
whatever its licence says.

It is recorded at length anyway, because the honest statement of this gap is not "nothing exists".

### The two narrow ones

`stlnet` (assembly `Visus.Stl`) 1.3.0, MIT, 16 264 downloads: 15 exported types, 97 members, STL
by loess and nothing else. `UniStuttgart-VISUS/Visus.Stl` has 2 stars and was last pushed
2021-01-25.

`NW.UnivariateForecasting` 4.2.1, MIT, 5 679 downloads: 20 exported types, 113 members, a
single-series heuristic forecaster. 3 stars, maintained, and not a statistical model.

Recorded and not read: the three `Dew.Stats` packages, which do carry ARIMA and are sold under a
commercial per-seat licence — decision 0003 excludes them before capability is relevant — and the
`Tsfm.Forecasting*` family, which wraps TimesFM and Kronos foundation models under ONNX. The
latter is a different answer to a different question and is named here so it is not mistaken for
an absence.

### The searches

| query, nuget.org, 2026-09-11 | packages |
| --- | --- |
| `vector autoregression` | 0 |
| `arima` | 4 — three are the commercial `Dew.Stats`, the fourth is `Cortex.TimeSeries` |
| `stl decomposition` | 1 — `stlnet` |
| `seasonal decomposition` | 2 — `stlnet`, `Cortex.TimeSeries` |

### Our own repository

Nothing. The grep 0104 records returns no time-series hit in `src/`; `Lodestar.Decomposition` is
matrix decomposition — truncated SVD and NMF — and has no seasonal sense at all.

## Decision

**Forecasting and anomaly detection are delegated to `Microsoft.ML.TimeSeries`.** It is
first-party, MIT, maintained, and it does the job. Writing an SSA forecaster next to it would be
the "rewrite Python's ecosystem" this project exists not to do, one ecosystem over. The migration
page names it, and `docs/migration/pandas.md` keeps pointing at Deedle for the index.

**The diagnostics are written natively**, as
[#617](https://github.com/CyrilB1531/lodestar/issues/617): autocorrelation and partial
autocorrelation with their bands, the Ljung-Box test, a stationarity test with ADF and KPSS as
complements, and seasonal decomposition into trend, seasonal and residual.

That is the inversion this reading produced, and it is worth stating plainly: the naive scope for
"time series in .NET" is a forecaster, and a forecaster is the one thing already shipped by
Microsoft. What nobody maintained ships is the apparatus for deciding whether a series may be
modelled at all and whether a fit left anything behind — which is the same shape as 0096's
finding, where the estimate was everywhere and the inference nowhere.

Explicitly **not** in the first lot: ARIMA and SARIMAX estimation, VAR, Kalman filtering and
state-space models, panel data, and HAC/Newey-West covariances. A native ARIMA is a defensible
later lot — `Cortex.TimeSeries` is the only permissive one and it is unadopted — but it is not
this one, and #617 names it as a follow-up rather than leaving it implied.

**The package is not named here**, for 0104's reason. Two-level naming makes
`Lodestar.Stats.TimeSeries` available.

## Consequences

- [#617](https://github.com/CyrilB1531/lodestar/issues/617) is opened with this reading as its
  evidence, on milestone 0.7.0, with a spec owed before any code.
- `docs/migration/statsmodels.md`'s time-series pitfall is rewritten as a read with a version and
  a member count, in the register of the Math.NET bullet above it, and its single ⚠️ row becomes
  one row per subject.
- **A delegation is a promise to keep reading.** `Microsoft.ML.TimeSeries` has a 6.0.0 preview
  out; if it grows an ARIMA or an ACF, #617's scope shrinks and this decision is superseded rather
  than edited. That is the cost of delegating to something alive, and it is cheaper than the cost
  of duplicating it.
- The oracle for #617 is `statsmodels` 0.15.0 — `acf`, `pacf`, `acorr_ljungbox`, `adfuller`,
  `kpss`, `seasonal_decompose` — already in the lock since #566.
- `bench/README.md` gains a section when the lot ships, measuring against `Cortex.TimeSeries`
  where the capabilities overlap. Its 240 downloads are a reason to prefer our own, not a reason
  to skip the measurement — 0096's rule.
- With 0104, this discharges #338, which closes. The third subject neither decision read —
  mixed and hierarchical models — leaves as
  [#621](https://github.com/CyrilB1531/lodestar/issues/621) rather than staying implied in a
  closed issue's body.
