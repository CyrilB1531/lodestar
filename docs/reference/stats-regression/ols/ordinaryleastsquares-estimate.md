# OrdinaryLeastSquares.Estimate

Fits a linear model and reports the estimates and their standard errors, without the inference table.

<!-- docs-declaration -->

```csharp
public static OlsEstimate Estimate(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, bool withIntercept = true)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one observed value per row of `design`. `featureCount`
is how many regressors each row carries. `withIntercept` says whether to fit a constant, prepended to
the coefficients; `true` by default.

**Returns** — [`OlsEstimate`](olsestimate.md): the coefficients, their standard errors and t
statistics, and the residual sum of squares.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `design` is not a whole number of rows, when `response` has a different
length, or when no residual degrees of freedom are left.

**Example** — the same line [`Fit`](ordinaryleastsquares-fit.md) reads, with only what a caller fitting
many of them needs.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, response, featureCount: 1);

double slope = Math.Round(estimate.Coefficients[1], 4);         // => 1.9976
double error = Math.Round(estimate.StandardErrors[1], 4);       // => 0.0278
double t = Math.Round(estimate.TStatistics[1], 4);              // => 71.8557
double residuals = Math.Round(estimate.ResidualSumOfSquares, 4);  // => 0.1948
```

**Remarks** — always the Householder reflections [`Fit`](ordinaryleastsquares-fit.md) falls back
to, for a caller fitting many regressions and reading a coefficient, a t statistic or a likelihood
from each — the augmented Dickey-Fuller lag search in `Lodestar.Stats.TimeSeries` fits one per
candidate lag. It skips what `Fit` adds on top: p-values, intervals, R², the F test, the variance
inflation factors and the robust covariances. The standard errors are the non-robust ones; a robust
covariance is `Fit`'s. Where `Fit` took the normal equations the two agree to rounding rather than to
the bit: `1.9976190476190478` here against `Fit`'s `1.9976190476190496` on the example above.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsEstimate`](olsestimate.md), [`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md),
the [Python equivalence table](../../../equivalence.md).
