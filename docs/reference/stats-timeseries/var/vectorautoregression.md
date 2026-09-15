# VectorAutoregression

A vector autoregression with the inference table on top of it.

<!-- docs-declaration -->

```csharp
public static class VectorAutoregression
```

**Example** — two series, one lag.

```csharp
using Lodestar.Stats.TimeSeries;

// Row-major in time: two values per observation, oldest first.
double[] series = [0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
                   0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
                   -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067];

VarSummary fit = VectorAutoregression.Fit(series, variableCount: 2, lagOrder: 1);

double ownLag = Math.Round(fit.Coefficients[1][2], 6);   // => -0.32412
int used = fit.ObservationsUsed;                         // => 14
double akaike = Math.Round(fit.Akaike, 6);               // => -3.261533
```

**Remarks** — reference behavior is `statsmodels` 0.15.0's `VAR(y).fit(p)`. The estimate is least
squares equation by equation, through the same Householder QR
[`OrdinaryLeastSquares.Estimate`](../../stats-regression/ols/ordinaryleastsquares-estimate.md) runs.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`VarSummary`](varsummary.md), [`VarOptions`](varoptions.md),
the [vector autoregression index](../var.md).

## Members

| Member | What it does |
| --- | --- |
| [`VectorAutoregression.Fit`](vectorautoregression-fit.md) | Fits a VAR of the given lag order and reports its table. |
