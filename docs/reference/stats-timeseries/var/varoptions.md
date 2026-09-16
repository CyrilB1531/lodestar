# VarOptions

What a vector autoregression should estimate.

<!-- docs-declaration -->

```csharp
public sealed record VarOptions
```

**Properties** — `WithIntercept` says whether each equation carries a constant; `true` by default, the reference's
`trend="c"`. Setting it to `false` is its `trend="n"`.

**Example** — dropping the constant changes the fit and what the criteria count.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
                   0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
                   -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067];

VarSummary withConstant = VectorAutoregression.Fit(series, 2, 1);
VarSummary without = VectorAutoregression.Fit(series, 2, 1, new VarOptions { WithIntercept = false });

int withIt = withConstant.ModelDegreesOfFreedom;   // => 3
int withoutIt = without.ModelDegreesOfFreedom;     // => 2
```

**Remarks** — the reference's `"ct"` and `"ctt"` trends, exogenous regressors and lag-order selection are not fitted
here.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`VectorAutoregression.Fit`](vectorautoregression-fit.md), [`VarSummary`](varsummary.md).
