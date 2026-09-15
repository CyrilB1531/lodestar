# GeneralizedLeastSquares.Fit

Fits a linear model under a given error covariance and reports what a summary table holds.

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> covariance, int featureCount, OlsOptions options = null)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no constant
column of your own. `response` is one observed value per row of `design`. `covariance` is the error
covariance, row-major, one row and one column per row of `design`: symmetric and positive definite.
`featureCount` is how many regressors each row carries. `options` chooses whether to fit an intercept, which
covariance of the estimates to report, and at what confidence; `null` fits an intercept at 0.95.

**Returns** — [`OlsSummary`](../ols/olssummary.md): the fitted model, with its standard errors, t statistics,
p-values, confidence intervals and variance inflation factors.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, or when `covariance` holds
a value that is not finite. `ArgumentException` when `design` is not a whole number of rows, when `response`
has a different length, when `covariance` is not the square of that length, is not symmetric or is not
positive definite, or when no residual degrees of freedom are left.

**Example** — a diagonal covariance is the weighted fit with weights `1/σ`, to rounding.

```csharp
using Lodestar.Stats.Regression;

double[] x = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
double[] y = [2.2, 3.9, 6.4, 7.7, 10.3, 11.8];
double[] variances = [1.0, 0.5, 2.0, 1.0, 0.25, 4.0];
double[] covariance = new double[36];
double[] weights = new double[6];
for (int i = 0; i < 6; i++)
{
    covariance[(i * 6) + i] = variances[i];
    weights[i] = 1.0 / variances[i];
}

OlsSummary generalized = GeneralizedLeastSquares.Fit(x, y, covariance, featureCount: 1);
OlsSummary weighted = WeightedLeastSquares.Fit(x, y, weights, featureCount: 1);

double gap = Math.Abs(generalized.Coefficients[1] - weighted.Coefficients[1]);
bool agree = gap < 1e-12;  // => True
```

**Remarks** — **the rows are whitened by `L⁻¹`**, where `covariance = L Lᵀ` is its Cholesky factorization, and
fitted as [`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md) would, the robust covariances on the
whitened rows included. Three numbers follow the reference rather than the whitened rows:

- `RSquared` with an intercept is centred on the mean estimated in whitened space, `(L⁻¹y)·(L⁻¹1) / (L⁻¹1)·(L⁻¹1)`,
  which reduces to the weighted mean when `covariance` is diagonal.
- The model's own constant stays out of the robust Wald test.
- `VarianceInflationFactors` are those of the design as given, which `variance_inflation_factor` returns.

**An asymmetric covariance is refused**, where the reference's Cholesky reads the lower triangle and ignores
the upper one; a pair of mirrored entries may differ by `1e-12` of the larger. `L⁻¹` is applied by forward
substitution rather than formed, which agrees with the reference's explicit inverse to rounding.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](../ols/olssummary.md), [`OlsOptions`](../ols/olsoptions.md),
[`WeightedLeastSquares.Fit`](../wls/weightedleastsquares-fit.md), the [generalized least squares index](../gls.md).
