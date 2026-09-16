# MaxAbsScaler.PartialFit

Folds another batch into the largest absolute value and the scale, as `partial_fit` does.

<!-- docs-declaration -->

```csharp
public MaxAbsScaler PartialFit(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the next batch, row-major, with `FeatureCount` values per row.

**Returns** — a new scaler summarising every batch seen so far; the one it was called on is unchanged.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — the running maximum absolute value.

```csharp
using Lodestar.Preprocessing;

MaxAbsScaler scaler = MaxAbsScaler.Fit([2.0, 1.0], featureCount: 1).PartialFit([-8.0]);

double largest = scaler.MaximumAbsolute[0];  // => 8
int seen = scaler.SampleCount;               // => 3
```

**Remarks** — the largest **absolute** value, so a negative batch can move it where a maximum would
not. The near-constant floor is applied to the result, as `Fit` applies it.

**It returns a new scaler and leaves this one alone** — see
[`StandardScaler.PartialFit`](standardscaler-partialfit.md) for why.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScaler`](maxabsscaler.md), [`MaxAbsScaler.Fit`](maxabsscaler-fit.md).
