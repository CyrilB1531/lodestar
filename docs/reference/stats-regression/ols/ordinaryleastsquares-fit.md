# OrdinaryLeastSquares.Fit

Fits a linear model and reports what a summary table holds.

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, OlsOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static OlsSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<int> clusters, int featureCount, OlsOptions options)
```

The second overload is the cluster-robust fit: its `options` must ask for
[`CovarianceType.Cluster`](covariancetype.md), and no other type reads the labels.

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one observed value per row of `design`. `clusters` is one
label per row, any integers in any order, naming at least two clusters. `featureCount` is how many
regressors each row carries. `options` chooses whether to fit an intercept, which covariance to
estimate and at what confidence; `null` fits one at 0.95.

**Returns** — the fitted model, with its standard errors, t statistics, p-values, confidence
intervals and variance inflation factors.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `design` is not a whole number of rows, when `response` or `clusters` has
a different length, or when no residual degrees of freedom are left — every standard error here
divides by that count, so a design that fits its rows exactly is refused rather than answered with
zeros. `ArgumentException` too when `options` and the overload disagree: `Hac` without
[`HacLags`](olsoptions.md), `HacLags` or `SmallSampleCorrection` on a type that does not read it,
`Cluster` through the overload without labels, labels with any other type, or labels that put every
row in one cluster.

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
double firstP = summary.PValues[1];                // => 0.13402447032529333
double firstVif = summary.VarianceInflationFactors[0];  // => 59483.30118126556
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

**Clusters** — rows that share a label are free to share an error, and the standard errors count
clusters rather than rows.

```csharp
using Lodestar.Stats.Regression;

// Twenty readings in time order; each cluster is five consecutive readings.
double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0,
                   11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0, 20.0];
double[] response = [2.43, 3.16, 4.05, 4.97, 5.77, 4.73, 5.77, 7.8, 8.67, 9.68,
                     9.7, 10.86, 11.12, 10.9, 11.98, 14.32, 15.33, 14.54, 15.6, 19.15];
int[] clusters = [0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3];

OlsSummary ordinary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);
OlsSummary clustered = OrdinaryLeastSquares.Fit(
    design, response, clusters, featureCount: 1,
    new OlsOptions { CovarianceType = CovarianceType.Cluster });

double naive = Math.Round(ordinary.StandardErrors[1], 6);    // => 0.034032
double honest = Math.Round(clustered.StandardErrors[1], 6);  // => 0.046546
double overall = Math.Round(clustered.FPValue, 6);           // => 0.00046
```

Four clusters are four independent observations of the errors, not twenty: the slope's standard
error grows by 37%, and the overall test reads its F on `G - 1 = 3` denominator degrees of freedom
where the ordinary one reads 18 — which is what takes its p-value from `8.7e-15` to `0.00046`.
`ResidualDegreesOfFreedom` still reports 18, as statsmodels' `df_resid` does. Statsmodels hands
`int64` labels to `np.bincount`, which refuses a negative one; any `int` is a label here.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsSummary`](olssummary.md), [`OlsOptions`](olsoptions.md),
[`CovarianceType`](covariancetype.md).
