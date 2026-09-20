# 0787 — Offsets and exposure on the generalized linear model, at `statsmodels` parity

**Status:** written before the work, 2026-09-15.

Issue: [#787](https://github.com/CyrilB1531/lodestar/issues/787), a sub-issue of the umbrella
[#777](https://github.com/CyrilB1531/lodestar/issues/777).

Reading: [decision 0104](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0104-generalized-linear-models-are-written-natively.md) and
[decision 0111](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0111-the-generalized-linear-model-does-not-earn-its-own-package.md). Neither is
amended. 0111 is applied by 0114, 0128 and 0133, none of which touches offsets.

## Problem

A rate model fits counts per unit of exposure — claims per policy-year, events per person-time — as
`log μ = Xβ + log(exposure)`. The fixed term carries no coefficient.

`GeneralizedLinearModel.Fit` has no way to add one. A caller today either divides the counts by the exposure,
which changes the likelihood, or adds `log(exposure)` as a regressor, which estimates a coefficient the model
fixes at one.

## Scope

- `sm.GLM(y, X, family=f, offset=o, exposure=e).fit()` for the four families that ship, one or both of the
  two arguments given:
  - binomial with logit;
  - Poisson;
  - negative binomial;
  - Gamma with either link.
- **Out of scope:**
  - `freq_weights` and `var_weights`;
  - offsets on `predict`, since there is no prediction API here;
  - offsets on the least-squares fits, whose reference takes none.

## What the reference does, read and measured

From `statsmodels/genmod/generalized_linear_model.py`, 0.15.0:

1. **`exposure` is logged on the way in**, and the model carries `offset + log(exposure)` as one term. Either
   argument alone is that term.
2. **IRLS:**
   - The starting mean is `family.starting_mu(y)` and ignores the offset.
   - The working response is `η + (y − μ)·g′(μ) − offset`.
   - After each solve the linear predictor is `Xβ + offset`.
   - Everything else in the loop is unchanged: the criterion, the weights, and the covariance from the last
     solve.
3. **The null deviance.** With no offset and no exposure it is the deviance at the response mean, as today.
   With either given — **a vector of zeros included** — it refits the intercept-only model with the same
   offset by IRLS:
   - started from `link(mean(y))`;
   - at the default `maxiter=100` and `tol=1e-8`, whatever the caller passed.
4. **Refusals:**
   - `exposure` with any link but log raises `ValueError`.
   - A length that differs from the response raises `ValueError`.
   - A zero, negative or non-finite exposure, and a non-finite offset, reach `log` or the predictor as
     `inf`/`NaN`. They raise `ValueError: NaN, inf or invalid value detected`.

Measured with statsmodels 0.15.0 on 60 rows, each family with offset and exposure together (binomial with an
offset alone):

| family | iterations | params, default against `tol=1e-14` | null deviance, against a `tol=1e-14` refit |
| --- | ---: | ---: | ---: |
| Poisson | 5 | 3.3e-15 | 0.0 |
| negative binomial, α = 0.5 | 7 | 4.1e-6 | 3.3e-15 |
| Gamma, log link | 16 | 3.4e-10 | 0.0 |
| binomial | 5 | 5.2e-13 | 0.0 |

**The main fit stops where its criterion does.** The corpus therefore pins the reference's path: the same
start, criterion and iteration count, which is what `stats_glm.json` already does for every case in it.

**The null refit converges to its fixed point within `3.3e-15`**, so its own path cannot move a `1e-9` corpus.

**A zero offset matches no offset** to `2.9e-16` at worst, on the null deviance.

## Placement

- **One new overload:**

  ```csharp
  GeneralizedLinearModel.Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response,
      ReadOnlySpan<double> offset, ReadOnlySpan<double> exposure, int featureCount, GlmFamily family,
      GlmOptions? options = null)
  ```

  Either span may be empty, meaning "not given". Empty is the one length no fit can mean, since a fit needs
  more rows than parameters.
- **Rejected alternatives:**
  - **The arrays on `GlmOptions`.** A per-row array on an options record is #668's equality problem.
  - **Two overloads, one per argument.** They share a signature and cannot both exist.
  - **An offset-only overload with callers logging the exposure themselves.** It would drop the reference's
    log-link refusal and its `exposure=` spelling.

## Behaviour

- **Parity** on points 1 to 3, the null refit included. A non-empty offset or exposure takes the refit path,
  so a vector of zeros reports the refit's null deviance, as the reference does.
- **Refused, as the reference refuses:**
  - `exposure` unless the resolved link is log, which excludes binomial and Gamma with the inverse link;
  - a non-empty offset or exposure of the wrong length;
  - a non-finite offset;
  - an exposure that is not finite and above zero.
- **The existing overload's arithmetic does not move.** With no offset the loop is the one on `main`, and the
  benchmark checks that.

## Evidence

- **`stats_glm.json`** gains offset and exposure cases:
  - Poisson with exposure alone, with offset alone, and with both;
  - negative binomial with exposure;
  - Gamma, log link, with offset and exposure;
  - binomial with an offset;
  - Poisson with a zero offset, which pins the refit path.

  The existing cases regenerate unchanged.
- **Edge tests:**
  - each refusal;
  - a zero offset equals no offset on every coefficient and error to `1e-12`;
  - exposure equals an offset of its logarithm, exactly.
- **Benchmark:**
  - `tools/survey.cs` on `Accord.Statistics` 3.8.0 with `(Offset|Exposure)` matches only
    `ProportionalHazards.Offsets`, the Cox model's, and nothing on its GLM;
  - 0104 read the other .NET GLMs and found none usable here;
  - the incumbent is therefore `statsmodels` through `compare-glm`, with a `glm_poisson_exposure` row;
  - plus an A/B/A of `GlmBenchmarks` and `GlmPoissonBenchmarks` against `main`, for the loop every fit shares.
