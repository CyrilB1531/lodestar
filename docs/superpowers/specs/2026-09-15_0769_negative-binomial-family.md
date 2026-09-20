# 0769 — The negative binomial family, at `statsmodels.GLM` parity

**Status:** written before the work, 2026-09-15.

Issue: [#769](https://github.com/CyrilB1531/lodestar/issues/769), split out of the econometrics umbrella
[#338](https://github.com/CyrilB1531/lodestar/issues/338).
Reading: [decision 0104](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0104-generalized-linear-models-are-written-natively.md), which wrote
the GLM natively and left this family out by name, and
[decision 0115](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md),
which puts it second after `WLS`.

## Problem

A Poisson model assumes the variance of a count equals its mean. Real counts — visits, claims,
defects — are nearly always over-dispersed, and a Poisson table on them reports standard errors that
are too small and p-values that are too confident. The negative binomial family is what an analyst
reaches for next, and `GeneralizedLinearModel` ships only `Binomial` and `Poisson`.

## Scope

- `sm.GLM(y, X, family=sm.families.NegativeBinomial(alpha=a)).fit()` with a **given** `alpha`, the
  default log link, at `statsmodels` 0.15.0 parity over every number `GlmSummary` carries.
- **Out:** `sm.NegativeBinomial` (the discrete model that *estimates* `alpha`). It maximises a likelihood
  with a quasi-Newton optimiser rather than iterating IRLS, so its reproducibility at the corpus tolerance
  is the question [decision 0130](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md)
  found open for `MixedLM`; it waits for a caller and its own reading. Also out: links other than log,
  `var_weights`/`freq_weights`, offsets and exposure (#777).

## What the reference does, measured

Read from `statsmodels/genmod/families` 0.15.0 and run from `/var/tmp`:

1. **Link and variance.** The default link is `Log`; the variance is `μ + αμ²`, with `μ` clipped below at
   machine epsilon. The log link is not the family's canonical one, and IRLS does not need it to be:
   the working weight `1 / (V(μ) g'(μ)²)` is the same expression the shipped loop already evaluates.
2. **Deviance.** `2 Σ [ y log(y/μ) − (y + 1/α) log((y + 1/α)/(μ + 1/α)) ]`, where `y/μ` is clipped to
   machine epsilon, so a zero count contributes `−(1/α) log((1/α)/(μ + 1/α))` and no `NaN`.
3. **Log-likelihood.** `Σ [ y log(αμ) − (y + 1/α) log(1 + αμ) + lnΓ(y + 1/α) − lnΓ(1/α) − lnΓ(y + 1) ]`,
   and the AIC is `2k − 2ℓ` as for the other families.
4. **Scale.** Fixed at one; the Wald table reads the normal, as for Poisson.
5. **Start and stop.** `starting_mu` is `(y + ȳ)/2`, Poisson's; the criterion is the absolute deviance
   change, shared by every family.
6. **Null deviance** at the constant fit `μ = ȳ` — the intercept-only score equation with a log link
   solves to the mean whatever `α` is, so the shipped `NullDeviance` holds unchanged.
7. **`alpha`.** Not set, it defaults to `1.0` with a `ValueWarning`. `alpha=0` constructs and then fails
   the fit with `ZeroDivisionError` (`1/α`); a negative `alpha` fails with *"The first guess on the
   deviance function returned a nan"*. A small `alpha` converges to the Poisson fit: at `1e-3` the slope
   is `0.27106` against Poisson's `0.27097`.
8. **A fractional response** fits: `gammaln` takes any positive argument, and nothing checks for a count.

## Placement — a family member and a nullable option

- **`GlmFamily.NegativeBinomial`**, a new member. The enum's own remark says adding one is not breaking,
  and is what the follow-up families rely on.
- **`GlmOptions.NegativeBinomialAlpha`, a `double?`.** `alpha` is a parameter of one family, and an enum
  member cannot carry it. `null` fits the reference's default `1.0` — a library has no warning channel a
  caller reads, so the default is documented rather than warned about. A value set for another family is
  refused: a setting that silently does nothing is a bug in the call, not a preference.
- **Rejected:** an overload `Fit(..., double alpha, ...)` (a second entry point for one family, and #770's
  Gamma would want a third), and a `GlmFamily` class hierarchy (the enum is closed on purpose, #616).

## Behaviour

- **Parity** on points 1 to 6, and on 7's default and small-`alpha` behaviour.
- **Refused:** an `alpha` that is not finite and above zero (`ArgumentOutOfRangeException` from the option,
  where the reference divides by zero or fails its first deviance); an `alpha` given for `Binomial` or
  `Poisson` (`ArgumentException` naming `options`).
- **The response** is refused as Poisson's is (point 8 diverges) — a finite non-negative integer count, not all zero. The
  reference's `gammaln` would accept a fractional count; the shipped Poisson already refuses one, and the
  two count families keep one rule. Recorded in `docs/equivalence.md`.
- **`lnΓ` at non-integer arguments** is new: the shipped `LogFactorial` covers integers only. An internal
  `LogLikelihood.LogGamma(x)` for `x > 0` shifts the argument above 40 by the recurrence
  `lnΓ(x) = lnΓ(x + n) − Σ log(x + k)` and takes the Stirling series `LogFactorial` already uses. Measured
  against `gammaln`, a shift to 20 leaves `4.6e-13` — the omitted `1/(1680x⁷)` term — and a shift to 40
  leaves under `4e-15`. A corpus from `scipy.special.gammaln` over `(0, 1e6]` holds it.

## Evidence

- `tests/oracles/stats_glm.json` gains negative binomial cases from the same generator: `alpha` 0.5, 1
  and 2; with and without an intercept; zeros in the response; a 99% level; and a near-Poisson `alpha` of `1e-3`. Counts stay small:
  `generate_regression_log_factorial`'s docstring measured a large-count fit's log-likelihood
  regenerating `3.7e-9` apart between hosts, past the reproducibility gate's absolute `1e-9`.
- `regression_log_gamma.json` from `scipy.special.gammaln`, compared relatively at `1e-9`.
- Edge tests for each refusal, and that a small `alpha` approaches the Poisson fit.
