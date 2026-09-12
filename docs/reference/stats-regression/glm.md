# Generalized linear models — `Lodestar.Stats.Regression`

One entry point, [`GeneralizedLinearModel.Fit`](glm/generalizedlinearmodel-fit.md): it fits a
response through a link function instead of an identity one, by IRLS, and reports what a
`statsmodels` `GLM(...).fit()` summary holds — the same inference table
[`OrdinaryLeastSquares.Fit`](ols/ordinaryleastsquares-fit.md) reports, fitted through a link
instead of directly.

**Two families, both closed.** [`GlmFamily`](glm/glmfamily.md) is `Binomial` (a `{0, 1}` response,
through the logit link) or `Poisson` (a non-negative count, through the log link) — no interface,
no caller-supplied family. IRLS cannot check that a caller-supplied family is internally
consistent, and an incoherent one produces a plausible inference table rather than an error, so a
family is chosen from a fixed set instead. Adding a member later is not a breaking change, which
is what a future Gamma or negative-binomial family relies on.

## Why this is not a second package

[Decision 0111](../../decisions/0111-the-generalized-linear-model-does-not-earn-its-own-package.md)
measured this against the same three criteria
[decision 0096](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) gave
`Lodestar.Stats.Regression` its own package on — dependency profile, audience, release cadence —
and found none of them distinct from the OLS half already here. The IRLS loop reuses the same
Householder-QR least-squares core `OrdinaryLeastSquares.Fit` does, through
`Internal/LeastSquares.cs`, so the two share code neither one exposes publicly.

## Types

| Type | What it is |
| --- | --- |
| [`GeneralizedLinearModel`](glm/generalizedlinearmodel.md) | Fits the model by IRLS and builds the table. |
| [`GlmSummary`](glm/glmsummary.md) | The fitted model, its errors, its p-values and its diagnostics. |
| [`GlmOptions`](glm/glmoptions.md) | The intercept, the confidence level, and the IRLS budget. |
| [`GlmFamily`](glm/glmfamily.md) | The response distribution and its canonical link: `Binomial` or `Poisson`. |

## See also

- [Regression inference](../../guides/regression-inference.md) — reading the table (written for
  OLS; the same reading applies once a link function stands in for the identity one).
- [statsmodels → .NET](../../migration/statsmodels.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
