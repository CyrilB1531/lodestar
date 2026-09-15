---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0095", "0110", "0129", "0130", "0134"]
---
# 0135 — Instrumental-variable and panel estimators have no .NET incumbent, and both could be written at parity

**Status:** accepted · **Date:** 2026-09-15 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0110`](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md), [`0129`](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md), [`0130`](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md), [`0134`](0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md)

## Context

Issue #338 named instrumental variables and panel data as the estimator families an econometrician reaches for
after least squares, with no reading behind either. [#776](https://github.com/CyrilB1531/lodestar/issues/776)
exists to read them before anything is claimed, and it could conclude *written*, *delegated* or *left for a
caller*, per family.

Neither family is a covariance option on the existing fits. Each is a new estimator with its own table.
Their reference is also not `statsmodels`:

- **IV:** `statsmodels` carries `IV2SLS` only under `statsmodels.sandbox`.
- **Panel:** `statsmodels` has no panel estimator at all.

So the reference had to be read first. [0130](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md)
and [0134](0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md) each found a
reference that does not reproduce itself at the corpus tolerance, so reproducibility was the first thing to
measure.

## The reference

`linearmodels` 7.0, released 2025-10-21, installed into `.venv-oracles` for this reading and not pinned in
`tools/requirements*.txt`, which waits for a lot that writes a corpus.

**Licence, read from the wheel** as [0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md) requires:

- `License-Expression: NCSA` in `METADATA`;
- `licenses/LICENSE.md`, "Copyright (c) 2017 Kevin Sheppard", granting use, copying, modification and
  redistribution with the notice kept.

It is permissive, and reading it to build a corpus is what every other oracle here does.

## The .NET side

nuget.org, 2026-09-15:

| query | hits | anything that estimates either family |
| --- | ---: | --- |
| `instrumental variables`, `two stage least squares`, `econometrics` | 0 | — |
| `2sls` | 6 | none: SLS log sinks for Aliyun and an Epicor client |
| `panel data` | 53 | none: UI panels, dashboards, data-feed clients |
| `fixed effects` | 4 | none: UI effects and a rendering sample |

Surfaces read with `tools/survey.cs` ([0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)),
commercial libraries included as [0129](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md) did.
The pattern was
`(Instrument|TwoStage|2Sls|2SLS|Tsls|TSLS|IV2|Liml|LIML|Gmm|GMM|MethodOfMoments|Panel|FixedEffect|RandomEffect|WithinEstimat|BetweenEstimat|FirstDifferen|Endogen|Hausman|Sargan|Durbin.?Wu)`:

| package | exported types | members read | matches |
| --- | ---: | ---: | --- |
| `Accord.Statistics` 3.8.0 | 561 | 8,105 | none |
| `MathNet.Numerics` 5.0.0 | 336 | 6,938 | none |
| `Meta.Numerics` 4.2.0 | 177 | 2,374 | none |
| `NumFlat` 1.3.4 | 123 | 1,181 | `Clustering.ToGmm`, `ToDiagonalGmm` — Gaussian mixtures, not the generalized method of moments |
| `Numerics.NET` 10.7.0 (commercial) | 905 | 17,727 | none |
| `Dew.Stats.Core` 6.3.10 (commercial) | 51 | 967 | none |
| `ILNumerics.Toolboxes.Statistics` 7.4.62 (commercial) | 513 | 3,844 | none |

The commercial libraries are read and not run: nothing is timed or fitted under a trial licence.

## What reproduces

Every estimator below was fitted by `linearmodels` 7.0 and computed a second time directly in numpy on the same
seeded data. For IV the data was 500 rows, one exogenous regressor, one endogenous regressor and two instruments.
For panel it was 60 entities over 8 periods, balanced and with a quarter of the rows dropped.

| estimator | compared against | largest relative gap |
| --- | --- | ---: |
| `IV2SLS`, unadjusted | `(XᵀP_Z X)⁻¹ XᵀP_Z y`, `s² = eᵀe/n` | 2.4e-16 (estimates), 1.5e-16 (errors) |
| `IV2SLS`, `debiased=True` | `s² = eᵀe/(n − k)`; and `statsmodels.sandbox` `IV2SLS` | 3.1e-16; 3.7e-16 |
| `IV2SLS`, `cov_type="robust"` | the HC0 sandwich on the projected regressors | 3.9e-16 |
| `IVLIML` | κ as the smallest eigenvalue of `(YᵀM_Z Y)⁻¹ YᵀM_W Y`, then k-class | 8.8e-16 (κ), 8.5e-16 (estimates) |
| `IVGMM`, two-step | the two-step estimator with the heteroskedastic weight | 6.6e-16 |
| `IVGMM`, iterated | itself at `tol=1e-6` against `tol=1e-12` (3 and 6 iterations) | **6.6e-6** |
| `PanelOLS`, entity effects | least squares on the within-transformed rows | 3.5e-16 |
| `PanelOLS`, entity and time effects | least squares with a dummy per entity and per period, balanced / unbalanced | 2.2e-15 / 3.9e-15 |
| `BetweenOLS` | least squares on the entity means | 1.3e-15 |
| `FirstDifferenceOLS` | least squares on the within-entity differences | 1.2e-15 |
| `RandomEffects` | itself, fitted twice | 0.0 |

**Every estimator but iterated GMM is a closed form, and the reference reproduces it to the last digits.**
Iterated GMM stops on a tolerance and moves with it, which is [0134](0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md)'s
finding again at a smaller scale.

**The errors follow `linearmodels`' conventions, not `statsmodels`'.** On the unbalanced panel:

- **Unadjusted fixed-effects errors** are least squares on the within rows scaled by `√((n − k)/(n − k − N))`,
  1.0996 here, because the absorbed effects count against the degrees of freedom.
- **Entity-clustered errors** are 0.99306 of what `statsmodels`' cluster correction gives on those same rows.

The sandwich #775 added computes the filling either way; the small-sample factor is the part that differs, and a corpus
has to pin it rather than assume it.

## Decision

1. **Both families could be written at `linearmodels` parity, and neither has an incumbent to delegate to.**
   Across seven .NET libraries, three of them commercial, no exported member estimates an instrumental-variable
   or panel model. The reference reproduces each closed-form estimator at `1e-15`. The `1e-9` corpora every
   package here is held to are therefore available, as they were not for 0130's mixed models or 0134's ARIMA.
2. **Instrumental variables: `IV2SLS`, `IVLIML` and two-step `IVGMM`**, with the table the reference prints
   beside the estimates.
   - **What the reference returns:** `first_stage` reports R², partial R², Shea's R² and the first-stage F
     per endogenous regressor. The results also carry the Wu–Hausman, Durbin, Sargan, Basmann,
     Anderson–Rubin and three Wooldridge tests.
   - **Which of those ship** is the spec's to decide; none was measured here.
   - **Iterated GMM is left out:** its answer moves with its tolerance at `1e-6`.
3. **Panel data: `PanelOLS` with entity and time effects, `BetweenOLS`, `FirstDifferenceOLS` and
   `RandomEffects`**, with the reference's unadjusted, robust and clustered covariances and its own
   small-sample factors, two of which are measured above.
   - `PanelOLS(...).fit(use_lsmr=True)`, an iterative solver, is out, for the same reason as iterated GMM.
   - The default path matched the dummy-variable solve at `3.9e-15` on the unbalanced panel.
4. **Neither is written by this record.** Whether each earns a lot is a question for its issue, proposed rather
   than opened under [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s rule. The
   spec of each lot:
   - names the package (`Lodestar.Stats.Regression` holds the least-squares table both would reuse);
   - pins `linearmodels` 7.0 in `tools/requirements.txt`;
   - benchmarks against `linearmodels` through the cross-language harness, since there is no .NET row to
     time against.
5. **`docs/migration/statsmodels.md`** gains the two rows, and `docs/equivalence.md` a section recording what
   was measured.

### Rejected

- **`statsmodels.sandbox.regression.gmm.IV2SLS` as the reference.** It agrees with `linearmodels`' debiased
  2SLS to `3.1e-16`, but it lives in the sandbox, carries none of the diagnostics, and there is no panel
  counterpart for it to share conventions with.
- **Iterated GMM at a looser tolerance.** A tolerance chosen to make a corpus pass is chosen by the
  implementation (0134).
- **Opening both lots now.** The reading answers whether they could be written; issue #776's rule is that each
  family that earns a lot leaves as its own issue, and issues are proposed first.

## Consequences

- #776 closes on this record.
- The migration page names both families as unwritten and writable, pointing here.
- An IV lot or a panel lot, if Cyril wants either, starts from the reproducibility table above and the
  covariance conventions it measured.
