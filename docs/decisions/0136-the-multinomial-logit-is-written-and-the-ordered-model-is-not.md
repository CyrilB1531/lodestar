---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0095", "0104", "0110", "0111", "0130", "0134"]
---
# 0136 — The multinomial logit is written; the ordered model is not

**Status:** accepted · **Date:** 2026-09-16 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0104`](0104-generalized-linear-models-are-written-natively.md), [`0110`](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md), [`0111`](0111-the-generalized-linear-model-does-not-earn-its-own-package.md), [`0130`](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md), [`0134`](0134-arima-and-state-space-are-not-written-and-var-is-the-one-that-could-be.md)

## Context

[0104](0104-generalized-linear-models-are-written-natively.md) wrote the GLM natively and left multinomial and ordinal
responses out of its first lot. [#788](https://github.com/CyrilB1531/lodestar/issues/788) took both. Neither
`statsmodels.discrete.discrete_model.MNLogit` nor `statsmodels.miscmodels.ordinal_model.OrderedModel` iterates IRLS:
each maximises a likelihood through a general optimiser.

0130 and 0134 each found a reference whose optimiser does not reproduce its own answer at `1e-9`, and the issue made
that the first question here.

## What reproduces

`statsmodels` 0.15.0. `MNLogit` was fitted on 400 rows, two regressors and three categories. `OrderedModel` was
fitted on 400 rows, two regressors and four ordered levels.

| fit | against | parameters | standard errors |
| --- | --- | ---: | ---: |
| `MNLogit.fit()` (Newton, the default) | Newton at `tol=1e-14` | 2.1e-15 | 1.9e-16 |
| `MNLogit.fit(method="bfgs")` | the same | 1.5e-4 | 1.3e-5 |
| `MNLogit.fit(method="lbfgs")` | the same | 4.2e-4 | 2.3e-5 |
| `MNLogit.fit(method="nm")` | the same | 0.35 | 1.7e-2 |
| `OrderedModel.fit()` (Nelder–Mead, the default), logit / probit | Newton | 2.4e-4 / 1.7e-4 | 4.4e-5 / 2.2e-5 |
| `OrderedModel.fit(method="newton")`, logit | its own refit from its optimum | 1.6e-8 | 5.8e-7 |
| `OrderedModel.fit(method="bfgs", gtol=1e-12)`, logit / probit | Newton | 5.6e-10 / 3.0e-8 | 4.6e-8 / 1.0e-7 |

**Why `MNLogit` reproduces.** Its score and Hessian are analytic, and its default Newton iteration converges
quadratically from zeros in five to seven steps. It stops at the maximum likelihood to the last digits.

**Why `OrderedModel` does not.**

- It defines neither a score nor a Hessian, so `GenericLikelihoodModel` differentiates both numerically.
- Its standard errors are `approx_hess3` at the optimum (measured: the reported ones equal it to `2.5e-16`).
- Even Newton, refitted from its own optimum, moves the parameters by `1.6e-8` and the errors by `5.8e-7`.
- The default, Nelder–Mead, stops `2e-4` short.

A corpus frozen from it would record a finite-difference step and a simplex's stopping point, not the model: 0134's
finding again.

## The .NET side

`tools/survey.cs` ([0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)), pattern
`(Multinomial|Softmax|MaximumEntropy|Ordinal|Ordered|ProportionalOdds|CumulativeLogit|Polytomous|Polychotomous)`:

| package | what matches |
| --- | --- |
| `Accord.Statistics` 3.8.0 | `MultinomialLogisticRegression`, with standard errors, a Wald test and the log-likelihood; `MultinomialTest` |
| `Microsoft.ML` 5.0.0 (`StandardTrainers`) | `LbfgsMaximumEntropy` and `SdcaMaximumEntropy`: regularised multiclass trainers, coefficients only |
| `MathNet.Numerics` 5.0.0 | the `Multinomial` distribution and coefficient |
| `Numerics.NET` 10.7.0 (commercial) | `SoftMax`, `LogSoftMax`, `MultinomialCoefficient`, `IsOrdered` — no model |
| `Meta.Numerics` 4.2.0, `NumFlat` 1.3.4, `Dew.Stats.Core` 6.3.10, `ILNumerics.Toolboxes.Statistics` 7.4.62 | nothing |

**Accord's multinomial logit is the one incumbent, and 0104 already said why it cannot be delegated to:**
LGPL-2.1, last released 2017-10-19, repository archived. **Nothing in .NET fits an ordinal model.**

## Decision

1. **`MNLogit` is written**, in `Lodestar.Stats.Regression` under [0111](0111-the-generalized-linear-model-does-not-earn-its-own-package.md)'s
   reasoning, at `statsmodels` parity.
   - **The table:** the coefficients per non-reference category with their errors, z statistics, p-values and
     intervals, the log-likelihood, McFadden's pseudo-R², the likelihood-ratio test, AIC and BIC.
   - **Two numbers follow the model rather than the reference's arithmetic.**
     - **`llnull` is the closed form** `Σ nⱼ·log(nⱼ/n)`. `statsmodels` reaches it by fitting the constant-only model
       with Nelder–Mead and then BFGS, and lands 3e-11 to 3e-10 away from that closed form, measured on six
       fits. The likelihood-ratio p-value inherits that gap, amplified by the χ² tail: `2.3e-9` to `1.3e-8` relative
       on the five frozen cases. scipy's `chi2.sf` on the closed-form statistic reproduces the C# value, and the
       corpus freezes both.
     - **A perfectly separated response is refused.** `statsmodels` returns NaN coefficients from it and reports
       `converged=True`.
2. **`OrderedModel` is not written.** Its reference does not pin its own answer at the corpus tolerance, and nothing
   in .NET fits one to delegate to. It waits for a caller under [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
   rule. The caller who reopens it inherits the question of what to compare against: an analytic-Hessian fit
   against a numerical-Hessian reference agrees at `1e-7`, not `1e-9`.

### Rejected

- **`OrderedModel` against `method="newton"` at a looser tolerance.** Its standard errors are finite differences, so
  the tolerance would be chosen by the step size, not by the model (0134).
- **Delegating the multinomial logit to Accord.** 0104's licence and maintenance findings still hold.
- **Refitting the constant-only model for `llnull`, as the reference does.** That is an optimiser's approximation of a
  number with a closed form, 3e-10 off it at worst. The pseudo-R² and likelihood-ratio statistic read it; the closed form is what they mean.

## Consequences

- #788 closes with the multinomial logit and this record.
- `docs/migration/statsmodels.md` names `OrderedModel` as not written, pointing here.
