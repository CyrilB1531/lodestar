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
`RSquared`, `AdjustedRSquared`, `FStatistic`, `FPValue`, `ResidualDegreesOfFreedom` and
`ResidualStandardError`, and `ConfidenceLevel` says what the two interval lists were computed at.

**Example** — the whole-model half of the table.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);

double explained = summary.RSquared;              // => 0.9988392866011389
double residualError = summary.ResidualStandardError;  // => 0.18016747059421534
int degreesOfFreedom = summary.ResidualDegreesOfFreedom;  // => 6
double overall = summary.FPValue;                 // => 4.88893361255519E-10
```

**Remarks** — **there is no public constructor.** A summary is what
[`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md) returns, and the arithmetic between the
lists is the invariant that makes it one: `TStatistics[j]` is `Coefficients[j] / StandardErrors[j]`
and each interval is centred on its estimate.

**`FStatistic` and `FPValue` test every slope at once**, not the intercept. With a fitted intercept
the null is that all `featureCount` slopes are zero; without one the intercept is not there to
exclude and the test gains a degree of freedom.

**A VIF is not a p-value's replacement.** It says how much of a regressor the *others* already
explain — 1 when nothing does, and unbounded as two regressors converge on saying the same thing.
A single regressor fitted with no intercept has no other column to be explained by, and its entry
is `NaN`: statsmodels raises there, and this returns the whole table with the one undefined
diagnostic marked rather than refusing a fit that is otherwise sound.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinaryLeastSquares`](ordinaryleastsquares.md), [`OlsOptions`](olsoptions.md).
