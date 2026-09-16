# MaxAbsScaler.Fit

Fits a scaler on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static MaxAbsScaler Fit(ReadOnlySpan<double> samples, int featureCount, MaxAbsScalerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `options` chooses whether
[`Transform`](maxabsscaler-transform.md) clips to `[−1, 1]`; `null` does not.

**Returns** — a fitted `MaxAbsScaler`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — a feature that is all zeros divides by 1 rather than by 0.

```csharp
using Lodestar.Preprocessing;

double[] zeros = [0.0, 0.0, 0.0];

MaxAbsScaler scaler = MaxAbsScaler.Fit(zeros, featureCount: 1);

double maximum = scaler.MaximumAbsolute[0];  // => 0
double divisor = scaler.Scale[0];            // => 1
```

**Remarks** — the floor is `max < 10·eps` rather than `max == 0`, the rule
[`MinMaxScaler.Fit`](minmaxscaler-fit.md) states with its measurement: a feature whose largest
absolute value is a few quadrillionths is constant to within what a division could resolve, and
dividing by it would amplify noise rather than reveal signal.

`MaximumAbsolute` reports what was seen and `Scale` what is divided by, so the two disagree exactly
on the features the floor caught — the pair above is the smallest case of that.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScaler`](maxabsscaler.md), [`MaxAbsScaler.Transform`](maxabsscaler-transform.md).
