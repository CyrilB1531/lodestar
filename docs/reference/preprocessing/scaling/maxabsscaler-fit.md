# MaxAbsScaler.Fit

Fits a scaler on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static MaxAbsScaler Fit(ReadOnlySpan<double> samples, int featureCount, MaxAbsScalerOptions options = null)
public static MaxAbsScaler Fit(CsrMatrix samples, MaxAbsScalerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `options` chooses whether
[`Transform`](maxabsscaler-transform.md) clips to `[−1, 1]`; `null` does not.

**Returns** — a fitted `MaxAbsScaler`.

**Exceptions** — `ArgumentNullException` when the sparse overload is given no matrix. `ArgumentOutOfRangeException` when `featureCount` is not positive.
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

**The sparse overload takes a `CsrMatrix`, and this is the scaler that suits one.** It never
subtracts, so a zero stays a zero, and [`Transform`](maxabsscaler-transform.md)'s sparse overload returns the
shape that went in. A column
with no stored value at all has a maximum absolute of 0 and a scale of 1, the floor reached a
different way.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScaler`](maxabsscaler.md), [`MaxAbsScaler.Transform`](maxabsscaler-transform.md).
