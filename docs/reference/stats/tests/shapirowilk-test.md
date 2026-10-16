# ShapiroWilk.Test

Tests whether a sample could have come from a normal distribution.

<!-- docs-declaration -->

```csharp
public static TestResult Test(ReadOnlySpan<double> sample, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `sample` is the data, at least 3 values; the span is
read, never modified. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`, defaulting
to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: Royston's W statistic, and its p-value.

**Exceptions** — `ArgumentException` when `sample` holds fewer than 3 values or its range is below
`1e-19`, a constant sample included, the two cases Royston's AS R94 refuses (its first and sixth
faults). AS R94 divides by the range throughout; here the sums run on values divided by the range
past a range of `1e100`, where a square would overflow, W being unchanged by the scale. A NaN or infinite range answers
`NaN` for both, a case the algorithm leaves undefined
([decision 0010](../../../decisions/0010-each-algorithm-follows-its-defining-document.md)). `ArgumentOutOfRangeException` when an option is outside its enumeration.

**Example** — a symmetric sample against a heavily right-skewed one of the same size.

```csharp
using Lodestar.Stats;

double[] symmetric = [-1.62, -1.10, -0.74, -0.47, -0.23, 0.0, 0.23, 0.47, 0.74, 1.10, 1.62];
double[] skewed = [0.1, 0.2, 0.3, 0.4, 0.6, 0.9, 1.4, 2.2, 3.6, 6.1, 12.0];

TestResult normalLooking = ShapiroWilk.Test(symmetric);
TestResult notNormal = ShapiroWilk.Test(skewed);

double symmetricW = Math.Round(normalLooking.Statistic, 4);   // => 0.9958
double skewedW = Math.Round(notNormal.Statistic, 4);          // => 0.7088
double skewedP = Math.Round(notNormal.PValue, 6);             // => 0.000608
```

**Remarks — `W` is close to `1` for a shape close to normal, and the p-value follows from how far
below `1` it falls.** `symmetric`'s `W` of `0.9958` carries a p-value over `0.999` — nothing here
argues against normality; `skewed`'s `0.7088` is a p-value of `0.0006`, decisively against it.

**The 3-value floor is Royston's, and so is the 5000-value warning.** AS R94 refuses fewer than three
values, and past 5000 computes with a warning that its fitted transform is extrapolated; this does
the same, without the warning:

```csharp
using Lodestar.Stats;

string message = "nothing was thrown";
try
{
    ShapiroWilk.Test([1.0, 2.0]);
}
catch (ArgumentException error)
{
    message = error.Message;
}

string what = message;   // => Royston's algorithm needs at least 3 values…
```

scipy answers `NaN` for two values where the algorithm refuses them, and computes past 5000 as the
algorithm does.

Omission runs before this test's own requirements, which are unchanged by it — a sample left too
short, or left with a range below `1e-19`, is refused exactly as it would be if passed directly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KolmogorovSmirnov.TwoSample`](kolmogorovsmirnov-twosample.md), the
[Python equivalence table](../../../equivalence.md).
