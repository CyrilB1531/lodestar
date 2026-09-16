# MinMaxScalerOptions

The range [`MinMaxScaler`](minmaxscaler.md) maps each feature onto, and whether it clips to it.

<!-- docs-declaration -->

```csharp
public sealed record MinMaxScalerOptions
```

**Properties** — `Low` and `High` are the bounds each feature's minimum and maximum map to,
defaulting to 0 and 1 as `sklearn.preprocessing.MinMaxScaler`'s `feature_range` does. `Clip`
bounds [`Transform`](minmaxscaler-transform.md)'s output to them, off by default.

**Example** — a range that is not the unit interval.

```csharp
using Lodestar.Preprocessing;

double[] samples = [0.0, 2.0, 4.0];

MinMaxScaler scaler = MinMaxScaler.Fit(
    samples, featureCount: 1, new MinMaxScalerOptions { Low = -5.0, High = 3.0 });

double bottom = scaler.Transform(samples)[0];  // => -5
double top = scaler.Transform(samples)[2];     // => 3
```

**Remarks** — the two bounds are separate properties rather than a tuple, because a record with
named members is what a caller reads back; `feature_range` is scikit-learn's spelling of the same
pair. A low bound at or above the high one is refused, as is a non-finite one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScaler`](minmaxscaler.md), [`MinMaxScaler.Fit`](minmaxscaler-fit.md).
