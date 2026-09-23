# PearsonResult.ConfidenceInterval

The confidence interval for the correlation, through the Fisher z transform.

<!-- docs-declaration -->

```csharp
public (double Low, double High) ConfidenceInterval(double level = 0.95)
```

**Parameters** — `level` is the confidence level, strictly between 0 and 1.

**Returns** — `(double Low, double High)`: the interval around the correlation the test measured.
A one-sided test's interval is half-open, and the far bound is the correlation's own limit, `-1`
or `1`, rather than an infinity — a correlation cannot leave that range, so there is no larger
number for the bound to be.

**Exceptions** — `ArgumentOutOfRangeException` when `level` is `NaN` or outside `(0, 1)`.

**Example** — the same eight panels, at two levels.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

PearsonResult result = Pearson.Test(sugar, sweetness);
(double low95, double high95) = result.ConfidenceInterval();
(double low99, double high99) = result.ConfidenceInterval(0.99);

double narrow = Math.Round(low95, 4);   // => 0.8783
double wide = Math.Round(low99, 4);     // => 0.7979
```

**Remarks — the interval is not symmetric about the coefficient**, and that is the transform
doing its job rather than an artefact. `r` is bounded and its sampling distribution is skewed
near the ends; `atanh` carries it onto a scale where the distribution is close to normal, the
interval is built there, and `tanh` carries the two bounds back. A correlation of `0.9778` with
an upper bound of `0.9961` has far less room above it than below, because there is far less room
above it.

**Below four pairs the interval is the whole range.** The standard error is `1 / sqrt(n - 3)`,
which has nothing to say at three pairs or fewer, so `(-1, 1)` is returned rather than a `NaN` —
the same answer scipy gives there, and an honest one: the data constrains nothing.

```csharp
using Lodestar.Stats;

(double low, double high) = Pearson.Test([1.0, 2.0, 3.0], [1.0, 3.0, 2.0])
    .ConfidenceInterval();

double lower = low;    // => -1
double upper = high;   // => 1
```

**A one-sided interval is half-open, and its fixed bound survives a `NaN` coefficient.** A
constant sample has no correlation, so the computed bound is `NaN` — but the other bound was
never computed from the data, and `-1` or `1` is still true of it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Pearson.Test`](pearson-test.md), [`PearsonResult`](pearsonresult.md),
[`TTestResult.ConfidenceInterval`](ttestresult-confidenceinterval.md), the
[Python equivalence table](../../../equivalence.md).
