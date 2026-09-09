# StandardScaler.Fit

Fits a scaler on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static StandardScaler Fit(ReadOnlySpan<double> samples, int featureCount, StandardScalerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `options` chooses which of the two steps to
apply; `null` applies both.

**Returns** — a fitted `StandardScaler`, carrying whichever statistics the options call for.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `samples` holds no row, or a partial one — a length that is not a positive
whole number of rows is not a matrix, and guessing which values were meant would be worse than
refusing.

**Example** — a feature whose values are indistinguishable from constant is scaled by 1.

```csharp
using Lodestar.Preprocessing;

// Three samples at 1e8, a hundred-millionth apart. The variance is 1.48e-16 — not zero.
double[] nearlyConstant = [1e8, 1e8 + 1e-8, 1e8 - 1e-8];

StandardScaler scaler = StandardScaler.Fit(nearlyConstant, featureCount: 1);

double variance = scaler.Variance![0];  // => 1.4802973661668753E-16
double scale = scaler.Scale![0];        // => 1
```

**Remarks** — variance is the **population** variance (`ddof=0`), computed in two passes as numpy's
is, so the two agree rather than differing by a factor of `n / (n - 1)`.

**A near-constant feature scales by 1, and the test is not `variance == 0`.** scikit-learn compares
the variance against the error bound of the two-pass algorithm,

```text
var <= n·eps·var + (n·mean·eps)²
```

from Chan, Golub and LeVeque — so a feature with a large mean and a tiny variance counts as constant
to within what the computation could have resolved, and dividing by its standard deviation would
amplify noise rather than reveal signal. The example above sits under that bound; move it one decade
out, to `1e8 ± 1e-7`, and the same shape sits above it and is scaled by `8.5e-08` instead. That pair
is frozen in `tests/oracles/preprocessing_standard_scaler.json`, because an implementation testing
`variance == 0` passes every other case and fails these two by eight orders of magnitude.

The bound is read from `sklearn.preprocessing._data._is_constant_feature` (BSD-3, allowed as a
behaviour reference by [`decisions/0003`](../../../decisions/0003-provenance-and-licensing.md)): the
papers give the error analysis, not the threshold.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScaler`](standardscaler.md),
[`StandardScaler.Transform`](standardscaler-transform.md).
