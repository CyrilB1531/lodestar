# OlsEstimate

An ordinary least-squares estimate: the coefficients and how precisely each is known, without the
inference table.

<!-- docs-declaration -->

```csharp
public sealed class OlsEstimate
```

**Properties** — `Coefficients`, `StandardErrors` and `TStatistics` are parallel lists in the design's
own order, intercept first when one was fitted; the standard errors are the non-robust ones.
`ResidualSumOfSquares` is the sum of the squared residuals, which a likelihood or an information
criterion is read from. `ResidualDegreesOfFreedom` is the rows less the parameters estimated.
`HasIntercept` says whether a constant was fitted, and so whether `Coefficients` starts with it.

**Example** — without an intercept the slope takes the whole line, and one more degree of freedom is
left.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsEstimate throughOrigin = OrdinaryLeastSquares.Estimate(design, response, featureCount: 1, withIntercept: false);

double slope = Math.Round(throughOrigin.Coefficients[0], 4);  // => 2.0039
int degrees = throughOrigin.ResidualDegreesOfFreedom;         // => 7
bool intercept = throughOrigin.HasIntercept;                  // => False
```

**Remarks — there is no public constructor.** An estimate is what
[`OrdinaryLeastSquares.Estimate`](ordinaryleastsquares-estimate.md) returns. A class rather than a
record, for [`OlsSummary`](olssummary.md)'s reason: a record's equality would compare the lists by
reference.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinaryLeastSquares.Estimate`](ordinaryleastsquares-estimate.md),
[`OlsSummary`](olssummary.md).
