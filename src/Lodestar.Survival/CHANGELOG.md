# Changelog — Lodestar.Survival

What changed in `Lodestar.Survival`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `AalenAdditive.Fit` fits Aalen's additive hazards model, lifelines' `AalenAdditiveFitter`, with its penalties, weights, slopes table, concordance and predictions. ([#1173](https://github.com/CyrilB1531/lodestar/issues/1173))

- `ParametricSurvival` fits lifelines' six parametric univariate models right-, left- and interval-censored with weights and delayed entry, `AcceleratedFailureTime` its Weibull, log-normal and log-logistic regressions, and `BreslowFlemingHarrington.Estimate` and `KaplanMeier.EstimateLeftCensored` complete its non-parametric curves. ([#1172](https://github.com/CyrilB1531/lodestar/issues/1172))

- `CoxProportionalHazards.Fit` takes subject weights, strata, clusters, lifelines' elastic-net penalty and the robust variance, `CoxSummary` carries the baselines and lifelines' predictions, `TestProportionalHazards` runs the proportional hazards test, and `CoxTimeVarying` fits start-stop intervals. ([#1171](https://github.com/CyrilB1531/lodestar/issues/1171))

- `LogRank` runs the Wilcoxon, Tarone-Ware, Peto and Fleming-Harrington weightings, subject weights and a truncation, over two groups, several or every pair, and `KaplanMeier.RestrictedMean`, `KaplanMeier.CompareAt` and `Concordance.Index` complete lifelines' closed forms. ([#1170](https://github.com/CyrilB1531/lodestar/issues/1170))

### Changed

- `Lodestar.Survival` needs the next `Lodestar.Stats`, for the normal and chi-squared tails the parametric fits read. ([#1172](https://github.com/CyrilB1531/lodestar/issues/1172))
- The `Lodestar.Stats` dependency floor rises from 0.4.0 to 0.5.0, the release that forwards its data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

## [0.2.0] — 2026-09-24

### Added

- `CoxProportionalHazards.Fit` fits the Cox proportional hazards model, at lifelines parity. ([#684](https://github.com/CyrilB1531/lodestar/issues/684), [`94531850`](https://github.com/CyrilB1531/lodestar/commit/94531850))

### Changed

- `SurvivalStep`, `KaplanMeierCurve`, `NelsonAalenCurve` and `LogRankResult` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `CoxProportionalHazards.Fit` reuses its likelihood buffers across Newton iterations and searches each subject's concordance level once. ([#843](https://github.com/CyrilB1531/lodestar/issues/843))
- `LogRank.Test` sorts each arm once and walks it with the event times, where it rescanned both arms at every time. ([#811](https://github.com/CyrilB1531/lodestar/issues/811))
- `KaplanMeierCurve` and `NelsonAalenCurve` compare their arrays by value. ([#668](https://github.com/CyrilB1531/lodestar/issues/668), [`a2b11493`](https://github.com/CyrilB1531/lodestar/commit/a2b11493))

### Fixed

- `KaplanMeier.Estimate` computes Greenwood's denominator in `double`, where past 46,340 subjects at risk it overflowed `int` and gave wrong confidence bounds. ([#865](https://github.com/CyrilB1531/lodestar/issues/865))

## [0.1.0] — 2026-09-10

### Added

- **A fifteenth package, and the first survival analysis on NuGet.** `KaplanMeier.Estimate` returns the survival function with its step table — a time, the risk set before it, and what happened at it — plus Greenwood's variance and confidence bounds on the **log-log transform**, which is lifelines' default and not the plain Greenwood interval: that one reaches `1.0067` at `S = 0.857` on 21 subjects, outside what a probability can be. `NelsonAalen.Estimate` returns the cumulative hazard over the same step table, and **its tie increment is not `d/n`** — with `d` events at one time it is `Σ 1/(n - i)`, so three events among 21 at risk give `0.150251` rather than `0.142857`, a 5% difference at the first step of the trial every survival text opens with. `LogRank.Test` compares two arms with the hypergeometric variance under ties, and takes its p-value from `Distributions.ChiSquaredSf` rather than re-deriving a tail. Right censoring only; left truncation, interval censoring, Cox regression and the accelerated-failure-time models are each their own lot.

  **Core tier holds**: one Lodestar edge, to `Lodestar.Stats` 0.4.0 for the two members [decision 0097](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0097-the-chi-squared-tail-joins-the-published-four.md) and [decision 0098](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md) published for it, and nothing external.

  **There is no incumbent to compare against, and that is recorded rather than assumed.** A NuGet search on 2026-09-09 returned 0 packages for `survival analysis` and 0 for `kaplan meier`, so [decision 0099](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md) discharges [ADR 0074](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s protocol by recording the searches — there was no assembly to read through a `MetadataLoadContext` — and `bench/README.md` section 20 is that absence rather than a blank. **`scikit-survival` is refused** on GPL-3.0-or-later per [decision 0003](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0003-provenance-and-licensing.md); `lifelines` 0.30.3 is MIT, read from the wheel because `autograd` and `autograd-gamma` both report `License: UNKNOWN` in legacy metadata. Adding it drops `pandas` to 2.3.3 with **all 127 corpora unmoved**, and pins `autograd-gamma` to 0.4.2 because 0.5.0 ships no wheel. Replays `tests/oracles/survival_curves.json` (8 samples) and `survival_logrank.json` (5 comparisons), with ties on purpose in half of them. ([#569](https://github.com/CyrilB1531/lodestar/issues/569))
