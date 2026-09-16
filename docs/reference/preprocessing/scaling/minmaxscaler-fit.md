# MinMaxScaler.Fit

Fits a scaler on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static MinMaxScaler Fit(ReadOnlySpan<double> samples, int featureCount, MinMaxScalerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `options` chooses the range and whether
[`Transform`](minmaxscaler-transform.md) clips to it; `null` is `[0, 1]` without clipping.

**Returns** — a fitted `MinMaxScaler`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, or the range is
not two finite bounds with the low one below the high one. `ArgumentException` when `samples` holds
no row, a partial one, or a non-finite value.

**Example** — a range of `1.11e-15`, which is not zero and is still treated as constant.

```csharp
using Lodestar.Preprocessing;

// Three values a quadrillionth apart: the range is 1.11e-15, below 10 * eps.
double[] nearlyConstant = [1.0, 1.0 + 1e-15, 1.0];

MinMaxScaler scaler = MinMaxScaler.Fit(nearlyConstant, featureCount: 1);

double range = scaler.DataRange[0];  // => 1.1102230246251565E-15
double scale = scaler.Scale[0];      // => 1
```

**Remarks** — **the near-constant test is `range < 10·eps`, not `range == 0`.** That is
`sklearn.preprocessing._data._handle_zeros_in_scale` with no constant mask, which is how the
reference calls it for this scaler, for [`MaxAbsScaler`](maxabsscaler.md) and for
[`RobustScaler`](robustscaler.md) — [`StandardScaler`](standardscaler.md) is the one that passes a
mask, and its rule is the two-pass variance bound instead. Move the example one decade out, to
`1.0 + 4e-15`, and the same shape is above the threshold and scales by about `2.5e14`.

`DataRange` reports what was seen and `Scale` what is divided by, so the two disagree exactly on a
feature the floor caught.

**Non-finite input is refused**, where the reference skips a `NaN`. Refusing rather than propagating
because [`RobustScaler`](robustscaler.md) sorts, and a `NaN` in a sorted column comes back as a
percentile nobody asked for; the three scalers answer alike.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScaler`](minmaxscaler.md), [`MinMaxScaler.Transform`](minmaxscaler-transform.md).
