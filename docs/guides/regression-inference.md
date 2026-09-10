# Regression inference

`Lodestar.Stats.Regression` answers a question a solver does not: **is this coefficient real, and
how sure are we?**

Prediction and inference are different jobs for different readers. ML.NET predicts; so does
`sklearn.linear_model.LinearRegression`, and so does `MathNet.Numerics.LinearRegression`. All three
return the estimate and stop. What follows is the other half.

## The whole surface

One call, one object:

```csharp
using Lodestar.Stats.Regression;

// Row-major: featureCount values per row, and no constant column of your own.
double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);
```

`summary.Coefficients[0]` is the intercept, and the regressors follow in the design's own order.
The slope here is `1.9976`, its standard error `0.0278`, and its p-value `4.9e-10`.

## Reading the table

| you want to know | read |
| --- | --- |
| the estimate | `Coefficients[j]` |
| how precisely it is pinned | `StandardErrors[j]` |
| whether it is distinguishable from zero | `PValues[j]`, or `ConfidenceLower[j]`/`ConfidenceUpper[j]` |
| whether the model as a whole explains anything | `FStatistic` and `FPValue` |
| how much of the variance it explains | `RSquared`, and `AdjustedRSquared` once there are several regressors |
| how far the fit typically misses | `ResidualStandardError` |
| whether two regressors are saying the same thing | `VarianceInflationFactors[j]` |

## Four things the table will not tell you

**A high R-squared is not evidence.** The clearest way to see it is a design whose two regressors
are nearly identical — `x2 = x1 + 0.01`:

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                   6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01];
double[] response = [2.2, 4.1, 6.3, 7.9, 10.2, 12.1, 14.3, 15.9, 18.2, 20.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 2);
```

`RSquared` is `0.9995` and **neither slope reaches significance** — the first p-value is `0.134`.
The model explains almost everything and cannot say which regressor deserves the credit. The VIF
names it: `59483` for each, against the `1` an independent regressor scores. A rule of thumb puts
`5` at "look into it" and `10` at "these two are one variable"; nothing here decides for you.

**A p-value is not the probability that you are wrong.** It is the probability of an estimate at
least this far from zero *if the true coefficient were zero*. `0.049` and `0.051` are the same
evidence — the same point [hypothesis testing](hypothesis-testing.md) makes, and it applies to each
row of this table.

**The overall F test is not the coefficients' p-values combined.** It asks whether *any* slope is
non-zero. A model can pass it with every individual coefficient insignificant, which is exactly the
collinear case above.

**Dropping the intercept changes what R-squared means.** With `WithIntercept = false` the fit is
scored against zero rather than against the response's mean, so the number goes up without the
model getting better — `0.9988` becomes `0.9998` on the first dataset above. `statsmodels` reports
the uncentred figure the same way, and the overall F test gains a degree of freedom with it.

## Confidence intervals

`ConfidenceLevel` defaults to `0.95` and takes anything strictly inside `(0, 1)`:

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1, new OlsOptions { ConfidenceLevel = 0.99 });

double lower = summary.ConfidenceLower[1];
double upper = summary.ConfidenceUpper[1];
```

The multiplier is a Student quantile on `ResidualDegreesOfFreedom`, not `1.96`. On eight rows and
two parameters that is six degrees of freedom, where the 95% multiplier is `2.447` — a quarter
wider than the normal approximation would give you.

## What is refused

- A design that is not a whole number of rows, or a response of a different length.
- A design with **no residual degrees of freedom left**. Every standard error divides by that
  count, so a fit that reproduces its rows exactly is refused rather than answered with zeros.
- A confidence level outside `(0, 1)`.

One case returns rather than throws: a single regressor fitted with no intercept has no other
column to be explained by, so its VIF is `NaN`. `statsmodels` raises there; the rest of the table
is sound, so the diagnostic that is not says so. `docs/equivalence.md` carries the row.

## See also

- [`OrdinaryLeastSquares`](../reference/stats-regression/ols.md) — the reference pages.
- [statsmodels → .NET](../migration/statsmodels.md) — what is delegated and what is not.
- [`decisions/0096`](../decisions/0096-ordinary-least-squares-earns-its-own-package.md) — why this
  is a package, and what reading the incumbents' exported surface actually found.
