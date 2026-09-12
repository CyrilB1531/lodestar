# GlmSummary

What a generalized linear fit reports, at `statsmodels` parity.

<!-- docs-declaration -->

```csharp
public sealed class GlmSummary
```

**Properties** — the per-parameter lists are parallel and in the design's own order, intercept
first when one was fitted: `Coefficients`, `StandardErrors`, `ZStatistics`, `PValues`,
`ConfidenceLower` and `ConfidenceUpper`. `Deviance` and `NullDeviance` are twice the log-likelihood
gap to a saturated fit, for the fitted model and for the intercept-only one. `Dispersion` is fixed
at `1` for both families here — the corpus reaches `2.9e-11`; it is estimated once a Gamma family
lands. `LogLikelihood` is the fitted log-likelihood and `Akaike` is `2k - 2 logL`.
`ResidualDegreesOfFreedom` is rows less parameters, and `HasIntercept` says whether a column of
ones was fitted. `Converged` says whether IRLS reached the tolerance — **read this first** —
`Iterations` is how many it took, and `DevianceChange` is the absolute deviance change at the last
one, which is what stays large on a fit `Converged` reports `false` for.

**Example** — the whole-model half of the table, on the separable design that does not converge
within its budget.

```csharp
using Lodestar.Stats.Regression;

double[] design = [-2.0, -1.0, 1.0, 2.0];
double[] response = [0.0, 0.0, 1.0, 1.0];

GlmSummary fit = GeneralizedLinearModel.Fit(
    design, response, 1, GlmFamily.Binomial,
    new GlmOptions { MaximumIterations = 15, ThrowOnNonConvergence = false });

bool converged = fit.Converged;               // => False
int iterations = fit.Iterations;               // => 15
double stillMoving = fit.DevianceChange;       // => 1.8272381600430005E-06
```

**Remarks** — **there is no public constructor.** A summary is what
[`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md) returns, and the arithmetic between
the lists is the invariant that makes it one: `ZStatistics[j]` is `Coefficients[j] /
StandardErrors[j]`, and `PValues[j]` is the two-sided normal tail read off it through the
chi-square(1) distribution `z²` follows.

**A `Converged` of `false` does not mean the numbers are missing — it means they should not be
trusted.** The design above separates perfectly: every `y = 1` sits above every `y = 0`, so the
logit slope keeps growing without bound and IRLS exhausts its 15-iteration budget rather than
reaching `Tolerance`. The standard errors on that fit are in the hundreds, which is the table
telling you it is not usable, not a bug.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GeneralizedLinearModel`](generalizedlinearmodel.md),
[`GlmOptions`](glmoptions.md), [`GlmFamily`](glmfamily.md).
