# GeneralizedLeastSquares

Generalized least squares with a caller-supplied error covariance and the inference table on top of it, at
`statsmodels.api.GLS` parity.

<!-- docs-declaration -->

```csharp
public static class GeneralizedLeastSquares
```

**Example** — ten readings whose errors are correlated with their neighbours, `0.6^|i−j|`.

```csharp
using Lodestar.Stats.Regression;

double[] time = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0];
double[] reading = [2.4, 3.6, 6.9, 7.1, 10.8, 11.2, 15.1, 14.6, 19.3, 19.9];
double[] covariance = new double[100];
for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        covariance[(i * 10) + j] = Math.Pow(0.6, Math.Abs(i - j));
    }
}

OlsSummary fit = GeneralizedLeastSquares.Fit(time, reading, covariance, featureCount: 1);

double slope = Math.Round(fit.Coefficients[1], 4);      // => 1.9712
double error = Math.Round(fit.StandardErrors[1], 4);    // => 0.2924
```

**Remarks** — the same rows fitted by [`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md) report
a slope standard error of `0.1102`, less than half: ten correlated readings carry less information than ten
independent ones, and the ordinary table counts them as independent.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](../ols/olssummary.md), [`WeightedLeastSquares`](../wls/weightedleastsquares.md),
the [generalized least squares index](../gls.md).

## Members

| Member | What it does |
| --- | --- |
| [`GeneralizedLeastSquares.Fit`](generalizedleastsquares-fit.md) | Fits a linear model under a given error covariance and reports what a summary table holds. |
