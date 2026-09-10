# OrdinaryLeastSquares.Fit

Fits a linear model and reports what a summary table holds.

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, OlsOptions options = null)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one observed value per row of `design`. `featureCount`
is how many regressors each row carries. `options` chooses whether to fit an intercept and at what
confidence; `null` fits one at 0.95.

**Returns** — the fitted model, with its standard errors, t statistics, p-values, confidence
intervals and variance inflation factors.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `design` is not a whole number of rows, when `response` has a different
length, or when no residual degrees of freedom are left — every standard error here divides by
that count, so a design that fits its rows exactly is refused rather than answered with zeros.

**Example** — a coefficient can be large, precise-looking and statistically indistinguishable from
zero, all at once.

```csharp
using Lodestar.Stats.Regression;

// x2 is x1 plus a hundredth. Two regressors that say almost the same thing.
double[] design = [1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                   6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01];
double[] response = [2.2, 4.1, 6.3, 7.9, 10.2, 12.1, 14.3, 15.9, 18.2, 20.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 2);

double explained = summary.RSquared;               // => 0.9995494585696385
double firstP = summary.PValues[1];                // => 0.13402447032528716
double firstVif = summary.VarianceInflationFactors[0];  // => 59483.30118110545
```

The model explains 99.95% of the variance and neither slope reaches significance, because the two
regressors are interchangeable and the fit cannot tell which one earns the credit. The VIF is what
names that: 59 483 rather than the 1 an independent regressor scores.

**Remarks** — **the intercept is not a column you supply.** `WithIntercept` prepends it, so
`Coefficients[0]` is the intercept and the regressors follow in the design's own order.
`VarianceInflationFactors` is therefore one shorter than `Coefficients` whenever an intercept was
fitted.

Turning the intercept off changes more than the coefficient count. `RSquared` becomes the
*uncentred* one — measured against zero rather than against the response's mean — and the overall F
test gains a degree of freedom. Both follow statsmodels, which reports the uncentred R-squared
without saying so in the table.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](olssummary.md), [`OlsOptions`](olsoptions.md).
