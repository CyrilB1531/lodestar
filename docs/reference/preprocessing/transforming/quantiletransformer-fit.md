# QuantileTransformer.Fit

Fits the quantiles of every feature.

<!-- docs-declaration -->

```csharp
public static QuantileTransformer Fit(ReadOnlySpan<double> samples, int featureCount, QuantileTransformerOptions options = null)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row; the span is
read, never modified. `options` chooses how many quantiles to fit and what they map onto; `null`
takes the reference's defaults, a thousand quantiles onto the unit interval.

**Returns** — a fitted [`QuantileTransformer`](quantiletransformer.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the
quantile count is below one. `ArgumentException` when `samples` holds no row, a partial one, or a
non-finite value.

**Example** — five quantiles over ten rows, which is what `References` and `Quantiles` hold.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer coarse = QuantileTransformer.Fit(
    skew, 1, new QuantileTransformerOptions { QuantileCount = 5 });

int levels = coarse.References.Count;          // => 5
double quarterLevel = coarse.References[1];    // => 0.25
double quarterValue = coarse.Quantiles[0][1];  // => 2.25
double medianValue = coarse.Quantiles[0][2];   // => 6.5
```

**Remarks — the quantile count is clamped to the number of rows.** Asking for a thousand over ten
rows fits ten, because there are only ten order statistics to read; the reference clamps the same
way and warns. Asking for fewer than there are rows is the useful direction: it is a
piecewise-linear approximation of the distribution, cheaper to hold and smoother to invert.

**The fit sorts each column once** and reads the levels off it, so it costs `features × n log n`
and holds `features × QuantileCount` doubles afterwards.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformer.Transform`](quantiletransformer-transform.md),
[`QuantileTransformerOptions`](quantiletransformeroptions.md).
