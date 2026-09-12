# GeneralizedLinearModel

A generalized linear model, fitted by IRLS, with the whole inference table.

<!-- docs-declaration -->

```csharp
public static class GeneralizedLinearModel
```

**Example** — a logistic fit, and the table that makes it inference.

```csharp
using Lodestar.Stats.Regression;

double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

GlmSummary fit = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Binomial);

double slope = fit.Coefficients[1];   // => 1.2140275858506053
double deviance = fit.Deviance;       // => 4.955973670099226
bool converged = fit.Converged;       // => True
```

**Remarks** — reference behavior is `statsmodels` 0.15.0's `GLM(...).fit()`. The response is a
count or a `{0, 1}` outcome; for a real-valued one,
[`OrdinaryLeastSquares`](../ols/ordinaryleastsquares.md) is the same table without a link.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GlmSummary`](glmsummary.md), [`GlmOptions`](glmoptions.md),
[`GlmFamily`](glmfamily.md), the [generalized linear model index](../glm.md).

## Members

| Member | What it does |
| --- | --- |
| [`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md) | Fits one model and reports its inference table. |
