# OlsOptions

What an ordinary least-squares fit should estimate, and at what confidence.

<!-- docs-declaration -->

```csharp
public sealed class OlsOptions
```

**Properties** — `WithIntercept` fits a constant term, as `statsmodels.api.add_constant` would;
`true` by default. `ConfidenceLevel` is the level of the reported intervals; 0.95 by default, and
it must lie strictly inside (0, 1).

**Example** — a wider level widens both ends without moving the estimate.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

OlsSummary ninetyFive = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);
OlsSummary ninetyNine = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1, new OlsOptions { ConfidenceLevel = 0.99 });

double narrow = ninetyFive.ConfidenceLower[1];  // => 1.9295938110753135
double wide = ninetyNine.ConfidenceLower[1];    // => 1.8945509015387234
```

**Remarks** — **turning the intercept off does more than drop a coefficient.**
[`OlsSummary.RSquared`](olssummary.md) becomes the uncentred one, measured against zero rather than
against the response's mean, and the overall F test gains a degree of freedom. Both follow
statsmodels. On the data above, the centred R-squared is 0.9988 and the uncentred one 0.9998: the
second is larger not because the model is better but because it is being scored against a lower
bar.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md),
[`OlsSummary`](olssummary.md).
