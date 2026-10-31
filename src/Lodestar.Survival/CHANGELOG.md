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

- `ParametricFit` is renamed `ParametricSummary`, `Aic` is `Akaike` on it and on `AftSummary`, as on the regression summaries, and the `ParametricSurvival` and `AcceleratedFailureTime` fits take the model after the data, as `GeneralizedLinearModel.Fit` takes its family. ([#1224](https://github.com/CyrilB1531/lodestar/issues/1224))
- `AalenAdditive.Fit` accumulates each event time's risk set from the last time back and `CoxTimeVarying.Fit` reads each interval's risk once per evaluation, which moves their last digits but not their agreement with lifelines. ([#1224](https://github.com/CyrilB1531/lodestar/issues/1224))
- `ParametricSurvival`'s generalized gamma fit takes the trigamma it now shares with `Lodestar.Stats`, which moves its standard errors by about 1e-9 relative. ([#1746](https://github.com/CyrilB1531/lodestar/issues/1746))
- `log1p` and `expm1` come from the one implementation `Lodestar.Preprocessing`, `Lodestar.Stats`, `Lodestar.Survival` and `Lodestar.Metrics` share, which can move a value by an ulp. ([#1230](https://github.com/CyrilB1531/lodestar/issues/1230))
- `Lodestar.Survival` needs the next `Lodestar.Stats`, for the normal and chi-squared tails the parametric fits read. ([#1172](https://github.com/CyrilB1531/lodestar/issues/1172))
- The `Lodestar.Stats` dependency floor rises from 0.4.0 to 0.5.0, the release that forwards its data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

### Fixed

- `CoxProportionalHazards.Fit` and `CoxTimeVarying.Fit` halve a Newton step that lowers the likelihood when the penalty has no L1 part, where a fit lifelines solves was refused as separated, refuse as stalled a fit no halving moves on, weigh each risk set from its own largest linear predictor, as the time-varying fit, the robust variance and the proportional hazards test now do, where a late one's sums underflowed and a design with a maximum was refused as separated, refuse as separated a fit whose budget runs out on a collapsed information, and converge where Newton's decrement has vanished, where a maximum far out never converged. ([#1242](https://github.com/CyrilB1531/lodestar/issues/1242))
- The generalized gamma fit no longer throws on a negative df at a large `|λ|`, keeps a finite Hessian at a tiny duration, and reaches a maximum at or past a `λ` near zero, a log-normal sample's among them. ([#1222](https://github.com/CyrilB1531/lodestar/issues/1222))
- The generalized gamma fit shortens a Newton step that would move a parameter by more than three times its size, which can change its path and, where its likelihood has several maxima, the one it reaches. ([#1222](https://github.com/CyrilB1531/lodestar/issues/1222))
- The parametric and AFT fits count as converged where Newton's decrement has vanished, whatever each parameter's scale, and never on a spent budget, where a fit out of budget on a covariate of tiny spread returned a point far from its maximum and one on times of tiny spread refused its maximum; the AFT fits search over centred and scaled covariates, where one of large spread or far from zero did not converge; a parameter past ten or under a hundredth is shifted in its own scale, where a log-logistic scale of 2e5 crept far below its maximum and an exponential one of 1e-12 threw; and a fit whose every observation is a probability refuses a likelihood that reached one, the limit of a point mass, within a bound relative to the total weight; and an interval's probability is read from its two cumulative hazards' gap, where two survivals near one, differenced, kept their rounding and a fit stalled. ([#1222](https://github.com/CyrilB1531/lodestar/issues/1222))
- The left-censored parametric and AFT fits keep a finite Hessian with a subject far past their scale, where they refused a likelihood with a maximum. ([#1222](https://github.com/CyrilB1531/lodestar/issues/1222))
- `KaplanMeier.CompareAt` refuses a curve from `EstimateLeftCensored`, whose statistic meant nothing. ([#1224](https://github.com/CyrilB1531/lodestar/issues/1224))
- `ParametricSummary.Percentile` keeps the generalized gamma's digits at a small shape and answers at a shape of zero. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The log-normal and log-logistic percentiles of `ParametricSummary` and the AFT models keep their digits at a probability near zero or one. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The confidence bands and intervals of `KaplanMeier`, `BreslowFlemingHarrington`, `AalenAdditive`, the Cox and AFT summaries and `ParametricSummary` keep their critical value's digits at a level near one, which moves an ordinary level's by a few ulps toward the exact quantile. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The `AalenSummary` survival-function, percentile, median and expectation predictions document the refusal of rows times event times past one array they inherit from the cumulative hazard, and every Aalen prediction documents its design bounded with any intercept column. ([#1616](https://github.com/CyrilB1531/lodestar/issues/1616))
- `AcceleratedFailureTime.Fit`, `FitLeftCensored`, `FitIntervalCensored` and `AalenAdditive.Fit` compare the design's length in `long`, where a covariate count wrapping the product let a short design through; they, the Cox fits, `TestProportionalHazards` and the AFT and Aalen predictions bound the design's copy; and the AFT fit refuses a parameter count past one array where it builds its column indices. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- The robust Cox covariance no longer sizes its risk sums one row past the design, which failed to allocate for a stratum as long as the design that one array holds. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- Before .NET 6 — on .NET Framework, .NET Core and .NET 5, and on legacy Mono, held there unmeasured — the refusals of an array past the largest one read those runtimes' bound for elements wider than a byte, `0x7FEFFFFF`, where a count between it and `Array.MaxLength` passed them and failed to allocate. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- `CoxSummary`, `AftSummary`, `AalenSummary` and `ParametricSummary` hand out read-only views of the arrays their predictions read, and `CoxSummary` predicts from its own copy of each baseline, so writing to what a fit returns no longer changes its next prediction. ([#1304](https://github.com/CyrilB1531/lodestar/issues/1304))
- `ParametricOptions` compares its breakpoints by value and hands out a copy, the Cox, AFT and Aalen predictions refuse a table past the largest array instead of wrapping, and `PiecewiseExponential` takes the mean of both pieces' hazards at an event on a breakpoint, as lifelines does. ([#1307](https://github.com/CyrilB1531/lodestar/issues/1307), [#1308](https://github.com/CyrilB1531/lodestar/issues/1308), [#1309](https://github.com/CyrilB1531/lodestar/issues/1309))
- The Cox, AFT, Aalen and log-rank fits refuse a square table of covariates, parameters or groups past the largest array instead of wrapping. ([#1311](https://github.com/CyrilB1531/lodestar/issues/1311))
- `LogRank.Pairwise` documents the refusals it can reach rather than pointing at `MultiGroup`'s, whose groups-by-groups table a pair never builds. ([#1305](https://github.com/CyrilB1531/lodestar/issues/1305))

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
