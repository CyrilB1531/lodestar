# 0788 — The multinomial logit, at `statsmodels` parity

**Status:** accepted, 2026-09-16. Written before the work.

Issue: [#788](https://github.com/CyrilB1531/lodestar/issues/788), a sub-issue of the umbrella
[#777](https://github.com/CyrilB1531/lodestar/issues/777).

Decision: [0136](../../decisions/0136-the-multinomial-logit-is-written-and-the-ordered-model-is-not.md). It records
the reproducibility measurements, the .NET survey and why `OrderedModel` is not written. This spec is the
multinomial logit alone.

## What the reference does, read and measured

`statsmodels.discrete.discrete_model.MNLogit`, 0.15.0:

1. **Categories.** The response's distinct values, sorted ascending. The first is the reference category, and each of
   the other `J − 1` gets one equation of `K` coefficients.
2. **Fit.**
   - Newton-Raphson from zeros (`base.optimizer._fit_newton`) on `f = −loglike/n`.
   - The Hessian of `f` gets `1e-10` added to its diagonal before `np.linalg.solve`.
   - It stops when every parameter moves by at most `tol=1e-8`, or at `maxiter=35`.
   - `converged` is false exactly when the iteration count reached `maxiter`.
3. **Standard errors.** `√diag((−H)⁻¹)` at the final parameters, with the analytic Hessian and no ridge. The z
   statistics, two-sided normal p-values and normal intervals are read off them.
4. **Summary numbers.**
   - `df_model` is `(rank(X) − 1)·(J − 1)`, **with or without a constant**.
   - `df_resid` is `n − df_model − (J − 1)`.
   - `llr` is `−2(llnull − llf)`, and its p-value is the χ² tail on `df_model`.
   - `prsquared` is `1 − llf/llnull`.
   - `aic` is `−2(llf − (df_model + J − 1))`, and `bic` is `−2 llf + log(n)(df_model + J − 1)`.
5. **`llnull`** refits the constant-only model with Nelder–Mead and then BFGS. That reaches the closed form
   `Σ nⱼ log(nⱼ/n)` to 3e-11 to 3e-10 (decision 0136).
6. **A perfectly separated response** returns NaN coefficients with `converged=True`.

## Placement

```csharp
MultinomialLogitSummary MultinomialLogit.Fit(
    ReadOnlySpan<double> design, ReadOnlySpan<int> response, int featureCount, MultinomialLogitOptions? options = null)
```

- **`MultinomialLogitOptions`:** `WithIntercept` (true), `ConfidenceLevel` (0.95), `MaximumIterations` (35),
  `Tolerance` (1e-8) and `ThrowOnNonConvergence` (true). These are the reference's defaults and `GlmOptions`' names.
- **`MultinomialLogitSummary`:**
  - **Categories:** `Categories`, ascending, the first being the reference.
  - **Per equation, then per parameter:** `Coefficients`, `StandardErrors`, `ZStatistics`, `PValues`,
    `ConfidenceLower`, `ConfidenceUpper`.
  - **Whole-model numbers:** `LogLikelihood`, `NullLogLikelihood`, `PseudoRSquared`, `LikelihoodRatio`,
    `LikelihoodRatioPValue`, `Akaike`, `Bayesian`, `ModelDegreesOfFreedom`, `ResidualDegreesOfFreedom`.
  - **The fit itself:** `HasIntercept`, `ConfidenceLevel`, `Converged`, `Iterations`.
- **Response type:** `int` labels rather than `double`. A category is a name, and `GlmFamily.Binomial`'s `0`/`1`
  check exists because a `double` response invites a value that is not one.
- **Rejected:**
  - **A `GlmFamily.Multinomial` member.** It is not IRLS, and its table is a matrix where `GlmSummary`'s is a vector.
  - **Coefficients as one flat list.** The reference's is a `K × (J − 1)` frame, and a flat list reorders it silently.

## Behaviour

- **Parity** on points 1 to 4 and the iteration count. `NullLogLikelihood` is the closed form of point 5, so the
  pseudo-R² and the likelihood-ratio statistic built on it agree within the reference's own 3e-10.
- **`LikelihoodRatioPValue`** is asserted two ways:
  - at `1e-9` against scipy's `chi2.sf` on the closed-form statistic;
  - at `5e-8` against the reference's own value, which the χ² tail moves by up to `1.3e-8` on the corpus.
- **Refused:**
  - fewer than two categories;
  - lengths that disagree;
  - no residual degree of freedom;
  - a Hessian that is not positive definite, or a non-finite parameter during Newton. That is where separation and a
    rank-deficient design both land: the reference returns NaN for the first and a singular-matrix error for the
    second.
- **Non-convergence** throws unless `ThrowOnNonConvergence` is false, as for the GLM.

## Evidence

- **`stats_mnlogit.json`**, frozen from `statsmodels` 0.15.0:
  - three categories on one regressor;
  - four categories on two regressors at 99%;
  - labels that are negative and not contiguous;
  - no intercept;
  - two categories.
- **Edge tests:**
  - each refusal;
  - two categories equal `GeneralizedLinearModel.Fit` with `GlmFamily.Binomial` on the same rows, coefficients and
    errors to `1e-6`, IRLS stopping on a deviance change that leaves its coefficients about `5e-8` from Newton's;
  - relabelling while keeping the order changes nothing.
- **Benchmark:**
  - `MultinomialLogitBenchmarks` against Accord's `MultinomialLogisticRegression` with `LowerBoundNewtonRaphson`,
    agreement checked before timing, allocations reported;
  - `compare-glm` gains `mnlogit_*` rows against `statsmodels`.
