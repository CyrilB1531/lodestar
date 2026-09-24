# Changelog — Lodestar.Stats.TimeSeries

What changed in `Lodestar.Stats.TimeSeries`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.1.0] — 2026-09-24

### Added

- The package, with `Stationarity.AugmentedDickeyFuller`, `Stationarity.Kpss` and `SeasonalDecomposition.Decompose`. ([#671](https://github.com/CyrilB1531/lodestar/issues/671), [`2986d69e`](https://github.com/CyrilB1531/lodestar/commit/2986d69e))
- `SerialCorrelation`, with `Autocorrelation`, `PartialAutocorrelation` and `LjungBox`, moved here from `Lodestar.Stats`. ([#617](https://github.com/CyrilB1531/lodestar/issues/617), [`2f5efb26`](https://github.com/CyrilB1531/lodestar/commit/2f5efb26))
- `VectorAutoregression.Fit` estimates a VAR(p) with its inference table. ([#786](https://github.com/CyrilB1531/lodestar/issues/786), [`2b5cd107`](https://github.com/CyrilB1531/lodestar/commit/2b5cd107))

### Changed

- `KpssLagRule`, `LagSelection`, `PValueBound`, `SeasonalModel`, `TrendTerms` and `VarOptions` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- The augmented Dickey-Fuller lag search and `VectorAutoregression.Fit` factor their design once for every lag or equation, 8.5 times faster at 2,000 points, and the autocovariance centres its series once. ([#843](https://github.com/CyrilB1531/lodestar/issues/843))

### Fixed

- `VectorAutoregression.Fit` refuses a collinear lagged design with `ArgumentException`, where a variable proportional to another gave coefficients near 1e13. ([#873](https://github.com/CyrilB1531/lodestar/issues/873))
- The Dickey-Fuller and seasonal decomposition options refuse an undeclared enum value, a too-short augmented Dickey-Fuller series is refused naming `series` or `options`, and a lag count below one is `ArgumentOutOfRangeException` like the other counts. ([#907](https://github.com/CyrilB1531/lodestar/issues/907))
- `SerialCorrelation.LjungBox` computes its scale in `double`, where from 46,340 observations `n * (n + 2)` overflowed and every statistic came out negative with a p-value of 1. ([#864](https://github.com/CyrilB1531/lodestar/issues/864))
- `Stationarity.Kpss` refuses a series lying exactly on a straight line under `ConstantAndTrend`, where it returned `NaN` and a window that depended on the runtime. ([#874](https://github.com/CyrilB1531/lodestar/issues/874))
- `Stationarity.AugmentedDickeyFuller` names `series` when the lagged design it builds has no unique solution, where the estimate's refusal cited a `design` parameter no caller passed. ([#979](https://github.com/CyrilB1531/lodestar/issues/979))
- `Stationarity.Kpss` measures that line against `n·ε` of the largest observation, where only a fit leaving exact zeros was refused and 99 of 100 random lines were answered from their rounding noise. ([#976](https://github.com/CyrilB1531/lodestar/issues/976))
- Both stationarity tests refuse a straight line on one bar that does not grow with the series, `Stationarity.AugmentedDickeyFuller` at lag zero too, where a million points of a 1e-10 wobble were called a line and a line's lag-zero statistic was one rounding error over another. ([#1080](https://github.com/CyrilB1531/lodestar/issues/1080))
- The augmented Dickey-Fuller lag search refuses a candidate design with no unique solution instead of ranking it on rounding, which answered a statistic of exactly 0 on a line bent at its first point. ([#977](https://github.com/CyrilB1531/lodestar/issues/977))
- A too-large `DickeyFullerOptions.MaxLag` is refused with the reason that applies, and `KpssOptions.LagRule` refuses an undeclared value where it is set. ([#984](https://github.com/CyrilB1531/lodestar/issues/984))
- `VectorAutoregression.Fit` refuses a collinear lagged design on the singular values, as `Lodestar.Stats.Regression`'s fits now do, where the per-column pivot missed a column far smaller than the ones it depends on. ([#978](https://github.com/CyrilB1531/lodestar/issues/978))
