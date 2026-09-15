# WeightedLeastSquares

Weighted least squares with the inference table on top of it, at `statsmodels.api.WLS` parity.

<!-- docs-declaration -->

```csharp
public static class WeightedLeastSquares
```

**Example** — six group means, the first over forty observations and the last over three.

```csharp
using Lodestar.Stats.Regression;

double[] dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
double[] meanResponse = [2.3, 3.8, 6.4, 7.7, 11.6, 10.9];
double[] groupSize = [40.0, 35.0, 30.0, 12.0, 5.0, 3.0];

OlsSummary weighted = WeightedLeastSquares.Fit(dose, meanResponse, groupSize, featureCount: 1);

double slope = Math.Round(weighted.Coefficients[1], 4);      // => 1.9882
double error = Math.Round(weighted.StandardErrors[1], 4);    // => 0.1767
```

**Remarks** — the same rows fitted by [`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md)
give a slope of `1.9343` with a standard error of `0.2388`: the two smallest groups, which pull the
line hardest, are the ones measured least precisely, and weighting hands the say back to the forty.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](../ols/olssummary.md), [`OlsOptions`](../ols/olsoptions.md), the
[weighted least squares index](../wls.md).

## Members

| Member | What it does |
| --- | --- |
| [`WeightedLeastSquares.Fit`](weightedleastsquares-fit.md) | Fits a linear model with one weight per row and reports what a summary table holds. |
