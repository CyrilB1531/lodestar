# VectorAutoregression.Fit

Fits a VAR of the given lag order and reports what a summary table holds.

<!-- docs-declaration -->

```csharp
public static VarSummary Fit(ReadOnlySpan<double> series, int variableCount, int lagOrder, VarOptions options = null)
```

**Parameters** — `series` is the observations, row-major in time: `variableCount` values each, oldest first.
`variableCount` is how many variables an observation carries, at least two. `lagOrder` is how many lags enter each
equation, at least one. `options` chooses whether each equation carries a constant; `null` fits one, the reference's
`trend="c"`.

**Returns** — [`VarSummary`](varsummary.md): the coefficients per equation with their standard errors, t statistics and
p-values, the two residual covariances, the log-likelihood and the four criteria.

**Exceptions** — `ArgumentOutOfRangeException` when `variableCount` is below two — one variable is an autoregression
rather than a vector one — or when `lagOrder` is below one. `ArgumentException` when `series` is empty or not a whole
number of observations, when it holds a value that is not finite, or when the lags leave no residual degree of freedom:
`n − lagOrder` usable rows must exceed the `(1 or 0) + variableCount·lagOrder` parameters each equation fits.

**Example** — the second equation of a two-variable system, and what the whole model scores.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
                   0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
                   -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067];

VarSummary fit = VectorAutoregression.Fit(series, variableCount: 2, lagOrder: 1);

double crossLag = Math.Round(fit.Coefficients[1][1], 6);     // => 0.624949
double itsError = Math.Round(fit.StandardErrors[1][1], 6);   // => 0.176406
double itsP = Math.Round(fit.PValues[1][1], 6);              // => 0.000396
double logLikelihood = Math.Round(fit.LogLikelihood, 6);     // => -10.899549
```

The second series depends on the first's previous value with a coefficient of 0.625 and a p-value of `0.0004`,
on the 14 usable observations these 15 rows leave at lag 1.

**Remarks** — **the parameters run in the reference's own order**: the constant when one was fitted, then lag 1's
coefficient for every variable, then lag 2's. Equation `j` explains variable `j`.

**The tests read the normal, not Student's t**, which is what `statsmodels` does for this model, and **no intervals are
reported** because the reference publishes none — its `VARResults` has no `conf_int`.

Lag-order selection, impulse responses, forecast-error variance decomposition and Granger causality are not here; each
waits for a caller, as [decision 0095](../../../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) asks.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`VarSummary`](varsummary.md), [`VarOptions`](varoptions.md).
