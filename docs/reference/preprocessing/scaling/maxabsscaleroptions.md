# MaxAbsScalerOptions

Whether [`MaxAbsScaler`](maxabsscaler.md) clips a scaled value into `[−1, 1]`.

<!-- docs-declaration -->

```csharp
public sealed record MaxAbsScalerOptions
```

**Properties** — `Clip` bounds [`Transform`](maxabsscaler-transform.md)'s output to `[−1, 1]`, off
by default as `sklearn.preprocessing.MaxAbsScaler`'s `clip` is.

**Example** — the option exists for values the fit never saw.

```csharp
using Lodestar.Preprocessing;

MaxAbsScaler scaler = MaxAbsScaler.Fit([1.0, 2.0], 1, new MaxAbsScalerOptions { Clip = true });

double bounded = scaler.Transform([8.0])[0];  // => 1
```

**Remarks** — one property rather than a range, because the range is not a choice here: dividing by
the largest absolute value puts every fitted row inside `[−1, 1]` by construction, and clipping only
decides what happens to a row that arrives later and is larger.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScaler`](maxabsscaler.md), [`MaxAbsScaler.Fit`](maxabsscaler-fit.md).
