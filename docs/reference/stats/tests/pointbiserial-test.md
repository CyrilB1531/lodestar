# PointBiserial.Test

Correlates a binary variable with a continuous one, `scipy.stats.pointbiserialr`.

<!-- docs-declaration -->

```csharp
public static TestResult Test(ReadOnlySpan<bool> x, ReadOnlySpan<double> y, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `x` is the binary variable and `y` the continuous one, the same length.
`nanPolicy` is what to do with a `NaN` in `y`; [`NanPolicy.Omit`](../nanpolicy.md) drops the pair.

**Returns** — `TestResult`: the coefficient and its two-sided p-value, both `NaN` when either
side is constant or fewer than two pairs remain, where scipy answers rather than raises.

**Exceptions** — `ArgumentException` when the samples differ in length, or when a `NaN` meets
[`NanPolicy.Raise`](../nanpolicy.md).

**Example** — whether the students who passed studied longer.

```csharp
using Lodestar.Stats;

bool[] passed = [true, false, true, true, false, true, false, false, true, true];
double[] hours = [12.0, 4.5, 9.0, 14.0, 6.0, 10.5, 3.0, 7.5, 11.0, 8.0];

TestResult result = PointBiserial.Test(passed, hours);

double r = Math.Round(result.Statistic, 6);   // => 0.824775
double p = Math.Round(result.PValue, 6);      // => 0.003319
```

**Remarks** — scipy offers no `alternative` here, and neither does this; for a one-sided
reading, [`Pearson.Test`](pearson-test.md) on the coded variable takes one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Pearson.Test`](pearson-test.md).
