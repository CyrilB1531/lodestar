# VarSummary

What a vector autoregression reports, at `statsmodels` parity.

<!-- docs-declaration -->

```csharp
public sealed class VarSummary
```

**Properties**:

- **The coefficient table.** `Coefficients`, `StandardErrors`, `TStatistics` and `PValues` are indexed by equation
  first, then by parameter. Equation `j` explains variable `j`; its parameters are the constant when one was fitted,
  then lag 1's coefficient for every variable, then lag 2's.
- **The residuals' covariance.** `ResidualCovariance` is `S/(T − k)`, the reference's `sigma_u`, and
  `ResidualCovarianceMaximumLikelihood` is `S/T`, its `sigma_u_mle`. Both are row-major and symmetric.
- **The whole model.** `LogLikelihood`, then `Akaike`, `Bayesian`, `HannanQuinn` and `FinalPredictionError`, each read
  off the maximum-likelihood covariance as the reference reads them.
- **The shape of the fit.** `LagOrder`, `VariableCount`, `ObservationsUsed` (`n − LagOrder`), `ModelDegreesOfFreedom`
  (parameters per equation), `ResidualDegreesOfFreedom` and `HasIntercept`.

**Example** — the residual covariance and the criteria.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
                   0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
                   -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067];

VarSummary fit = VectorAutoregression.Fit(series, 2, 1);

double firstVariance = Math.Round(fit.ResidualCovariance[0], 6);   // => 0.233687
double crossCovariance = Math.Round(fit.ResidualCovariance[1], 6); // => 0.006799
double bayesian = Math.Round(fit.Bayesian, 6);                     // => -2.987651
int residualDegrees = fit.ResidualDegreesOfFreedom;                // => 11
```

**Remarks** — **no intervals.** The reference publishes none for this model, so none are reported here rather than
inventing a convention the caller would have to check against it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`VectorAutoregression.Fit`](vectorautoregression-fit.md), [`VarOptions`](varoptions.md).
