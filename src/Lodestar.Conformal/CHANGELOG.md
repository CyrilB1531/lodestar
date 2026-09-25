# Changelog — Lodestar.Conformal

What changed in `Lodestar.Conformal`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `CrossConformal` computes CV+, Jackknife+ and jackknife-after-bootstrap intervals at MAPIE parity, and `SplitConformal.GammaScores` and `GammaInterval` the gamma conformity score. ([#1159](https://github.com/CyrilB1531/lodestar/issues/1159))

## [0.2.0] — 2026-09-24

### Added

- `SplitConformal.NormalisedResiduals` and `NormalisedInterval` make the interval width vary with the input. ([#683](https://github.com/CyrilB1531/lodestar/issues/683), [`42cc0384`](https://github.com/CyrilB1531/lodestar/commit/42cc0384))
- `SplitConformal.Quantile` takes a `ConformalQuantileRule`, whose `MapieClassification` reads the quantile MAPIE's prediction sets read, one rank above the default ceiling rule at 19 scores and 10 %. ([#866](https://github.com/CyrilB1531/lodestar/issues/866))

### Changed

- `ConformalQuantileRule` is compiled into `Lodestar.Abstractions` under the same name and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

### Fixed

- `SplitConformal.PredictionSet` includes a class within MAPIE's 1e-8 of the threshold, and `SplitConformal.Quantile` refuses a NaN score, which moved every rank down one. ([#889](https://github.com/CyrilB1531/lodestar/issues/889))

## [0.1.0] — 2026-09-01

### Added

- **Split conformal prediction, at MAPIE 1.5.0 parity.** `SplitConformal` turns a point prediction into an interval or a class into a prediction set, with a finite-sample coverage guarantee: `AbsoluteResiduals` and `LeastAmbiguousScores` score a calibration set, `Quantile` reduces the scores to the one number that carries the guarantee, and `Interval` and `PredictionSet` apply it. Static and dependency-free, the fifth package under [decision 0069](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md)'s first rule. The empty LAC prediction set is reproduced rather than repaired, and `k > n` returns an infinite interval instead of MAPIE's clamp to the widest score — [decision 0070](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0070-k-greater-than-n-returns-an-infinite-interval.md). The guarantee assumes exchangeability, which the guide leads with. ([#441](https://github.com/CyrilB1531/lodestar/issues/441))
