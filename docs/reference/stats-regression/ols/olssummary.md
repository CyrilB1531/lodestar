# OlsSummary

What a fitted ordinary least-squares model reports: the estimates, and how sure it is of each of
them.

<!-- docs-declaration -->

```csharp
public sealed class OlsSummary
```

**Properties** — the per-coefficient lists are parallel and in the design's own order, intercept
first when one was fitted: `Coefficients`, `StandardErrors`, `TStatistics`, `PValues`,
`ConfidenceLower` and `ConfidenceUpper`. `VarianceInflationFactors` carries one entry per
*regressor*, so it is shorter by one whenever `HasIntercept` is true. The whole-model figures are
`RSquared`, `AdjustedRSquared`, `FStatistic`, `FPValue`, `FNumeratorDegreesOfFreedom`,
`ResidualDegreesOfFreedom` and `ResidualStandardError`, and `ConfidenceLevel` says what the two interval lists were computed at.
Under an intercept, a response that does not vary, every value equal to the first a positive weight keeps, leaves
nothing to explain: `RSquared` and `AdjustedRSquared` are `NaN`, its residuals exactly zero or not, and the F as below
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779)).
[`CovarianceType`](covariancetype.md) echoes the estimator that produced the standard errors, and
is what tells a reader holding only this summary whether `TStatistics` and `PValues` are *t*
against Student's or *z* against the normal.

**Example** — the whole-model half of the table.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);

double explained = summary.RSquared;              // => 0.9988392866011389
double residualError = summary.ResidualStandardError;  // => 0.18016747059421467
int degreesOfFreedom = summary.ResidualDegreesOfFreedom;  // => 6
double overall = summary.FPValue;                 // => 4.888933612554975E-10
int tested = summary.FNumeratorDegreesOfFreedom;  // => 1
```

**Remarks** — **there is no public constructor.** A summary is what
[`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md) returns, and the arithmetic between the
lists is the invariant that makes it one: `TStatistics[j]` is `Coefficients[j] / StandardErrors[j]`
and each interval is centred on its estimate. The one exception is an exact fit under a robust covariance, every
residual within its rounding of zero, read for a generalized fit on the rows as given: the covariance is zero, so each standard error is 0, each interval the point
estimate, and each t statistic and p-value `NaN`, a test on a zero variance, as the F is
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779)).

**`FStatistic` and `FPValue` test every slope at once**, not the intercept. With a fitted intercept
the null is that all `featureCount` slopes are zero; without one the intercept is not there to
exclude and the test gains a degree of freedom. Without a robust covariance the F is `+∞`, with `FPValue` 0, wherever
the residuals are exactly zero, `y = 0` included, as the IV unadjusted model and first-stage tests and the panel's model
tests are there; where its residuals are not all exactly zero, a response that does not vary under an intercept, every
value equal to the first a positive weight keeps, reads `NaN` for both, where statsmodels reads `−df_resid` off the rounding
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779)). Under a robust covariance whose tested block is short
of full rank — as a cluster fit's is when its scores span fewer directions than the slopes — the
statistic is the pseudo-inverse's quadratic form over the block's rank, read from the covariance's score
rows rather than the formed block so that a badly scaled column keeps its direction, and `NaN` when that
rank is zero, as on an exact fit whose every residual lies within its rounding of zero
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779)), or when the rounding of the residuals can move the form by a
millionth of itself.
`FNumeratorDegreesOfFreedom` is the numerator `FPValue` reads, statsmodels' `f_test(...).df_num`: the slopes
tested, or that rank ([#1223](https://github.com/CyrilB1531/lodestar/issues/1223)). `FPValue` reads the F on `ResidualDegreesOfFreedom`
denominator degrees of freedom, except under `CovarianceType.Cluster`, where it reads the cluster count
less one, as statsmodels' `df_resid_inference` does; `ResidualDegreesOfFreedom` itself stays rows less
parameters.

**A VIF is not a p-value's replacement.** It says how much of a regressor the *others* already
explain — 1 when nothing does, and unbounded as two regressors converge on saying the same thing.
A single regressor fitted with no intercept has no other column to be explained by, and its entry
is `NaN`: statsmodels raises there, and this returns the whole table with the one undefined
diagnostic marked rather than refusing a fit that is otherwise sound.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinaryLeastSquares`](ordinaryleastsquares.md), [`OlsOptions`](olsoptions.md).
