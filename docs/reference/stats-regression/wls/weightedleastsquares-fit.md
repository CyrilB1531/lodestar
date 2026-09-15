# WeightedLeastSquares.Fit

Fits a linear model with one weight per row and reports what a summary table holds.

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> weights, int featureCount, OlsOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> weights, ReadOnlySpan<int> clusters, int featureCount, OlsOptions options)
```

The second overload is the cluster-robust weighted fit, whose `options` must ask for
[`CovarianceType.Cluster`](../ols/covariancetype.md).

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one observed value per row of `design`. `weights` is one
non-negative, finite weight per row, proportional to the inverse of that row's variance. `clusters`
is one label per row, any integers in any order, naming at least two clusters.
`featureCount` is how many regressors each row carries. `options` chooses whether to fit an
intercept, which covariance to estimate, and at what confidence; `null` fits an intercept at 0.95.

**Returns** — [`OlsSummary`](../ols/olssummary.md): the fitted model, with its standard errors, t
statistics, p-values, confidence intervals and variance inflation factors.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, or when a weight
is negative, `NaN` or infinite. `ArgumentException` when `design` is not a whole number of rows, when
`response`, `weights` or `clusters` has a different length, when no residual degrees of freedom are
left, when fewer rows carry a positive weight than the model has parameters, or when `options` and
the overload disagree, as on [`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md).

**Example** — the robust covariances apply to a weighted fit as they do to an ordinary one.

```csharp
using Lodestar.Stats.Regression;

double[] dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
double[] meanResponse = [2.3, 3.8, 6.4, 7.7, 11.6, 10.9];
double[] groupSize = [40.0, 35.0, 30.0, 12.0, 5.0, 3.0];

OlsSummary robust = WeightedLeastSquares.Fit(
    dose, meanResponse, groupSize, featureCount: 1,
    new OlsOptions { CovarianceType = CovarianceType.Hc1 });

double explained = Math.Round(robust.RSquared, 4);          // => 0.9694
double error = Math.Round(robust.StandardErrors[1], 4);     // => 0.1688
int freedom = robust.ResidualDegreesOfFreedom;              // => 4
```

**Remarks** — **each row and its response is scaled by the square root of its weight** and fitted
as [`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md) would, the robust covariances
on the scaled rows included. Three numbers follow the reference rather than the scaled rows:

- `RSquared` is weighted — centred on the weighted mean of the response with an intercept, against
  zero without one — and `AdjustedRSquared` and the overall F test read it.
- **A zero weight keeps its row.** It adds nothing to the estimate and still counts in
  `ResidualDegreesOfFreedom` and in HC1's correction, as `statsmodels` counts it.
- `VarianceInflationFactors` are those of the design as given, which is what
  `variance_inflation_factor` returns for the same `exog`; the reference's `WLS` reports none.
- `Hac` and `Cluster` read the scores of the scaled rows — each scaled row times its scaled
  residual — which is what the reference sums.

Equal weights reproduce `OrdinaryLeastSquares.Fit` exactly, and a constant weight moves only
`ResidualStandardError`. Weights the reference would crash on or answer through a pseudo-inverse are
refused instead — the [equivalence table](../../../equivalence.md) lists each.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](../ols/olssummary.md), [`OlsOptions`](../ols/olsoptions.md),
[`CovarianceType`](../ols/covariancetype.md), the [weighted least squares index](../wls.md).
