# OrdinaryLeastSquares

Ordinary least squares with the inference table on top of it, at `statsmodels.api.OLS` parity.

<!-- docs-declaration -->

```csharp
public static class OrdinaryLeastSquares
```

**Example** — fit a line, and read how sure the fit is of its slope.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);

double slope = summary.Coefficients[1];        // => 1.9976190476190474
double error = summary.StandardErrors[1];      // => 0.027800444266884116
double significance = summary.PValues[1];      // => 4.888933612555103E-10
```

**Remarks** — the estimate is the cheap half. `1.9976` on its own says nothing about whether the
slope is real; the standard error beside it, and the p-value read from it, are what the word
*inference* means and what no maintained .NET library publishes — see the
[namespace page](../ols.md) for the reading that establishes that.

Solved through a Householder QR of the design rather than through the normal equations. Forming
`XᵀX` squares its condition number, and the near-collinear designs a VIF exists to report are
exactly the ones that costs.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](olssummary.md), [`OlsOptions`](olsoptions.md), the
[ordinary least squares index](../ols.md).

## Members

| Member | What it does |
| --- | --- |
| [`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md) | Fits a linear model and reports what a summary table holds. |
