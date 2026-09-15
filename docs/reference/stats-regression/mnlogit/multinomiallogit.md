# MultinomialLogit

The multinomial logit, fitted by Newton-Raphson, with the whole inference table.

<!-- docs-declaration -->

```csharp
public static class MultinomialLogit
```

**Example** — three categories on one regressor.

```csharp
using Lodestar.Stats.Regression;

double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23,
                   -0.96, 1.6, 0.2, -1.73, -0.08, -1.16, -0.63, -0.49, -0.71, 0.55, -0.06, -0.59,
                   0.41, 0.83, -1.64, -0.26, -0.98, -0.17, -1.29, 0.02, -0.04, -0.3, -1.05, -0.4];
int[] response = [2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2,
                  0, 0, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2];

MultinomialLogitSummary fit = MultinomialLogit.Fit(design, response, 1);

double explained = Math.Round(fit.PseudoRSquared, 6);  // => 0.602479
int steps = fit.Iterations;                            // => 8
```

**Remarks** — reference behavior is `statsmodels` 0.15.0's `MNLogit(...).fit()`, Newton from zeros with its 35-step
budget. For two categories it is the binomial logit, which
[`GeneralizedLinearModel`](../glm/generalizedlinearmodel.md) also fits.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MultinomialLogitSummary`](multinomiallogitsummary.md),
[`MultinomialLogitOptions`](multinomiallogitoptions.md), the [multinomial logit index](../mnlogit.md).

## Members

| Member | What it does |
| --- | --- |
| [`MultinomialLogit.Fit`](multinomiallogit-fit.md) | Fits one model and reports its inference table. |
