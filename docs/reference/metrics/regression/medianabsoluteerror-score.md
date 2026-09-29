# MedianAbsoluteError.Score

The median of the absolute residuals.

<!-- docs-declaration -->

```csharp
public static double Score(ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPred, int outputCount = 1, ReadOnlySpan<double> sampleWeight = default, ReadOnlySpan<double> outputWeights = default)
```

**Parameters** — `yTrue` and `yPred` are the true and predicted values, row-major when there is
more
than one output. `outputCount` is how many outputs each row holds, `sampleWeight` weights the
rows,
and `outputWeights` weights the outputs in the reduction.

**Returns** — `double`, never negative, in the target's own units.

**Exceptions** — `ArgumentException` when a length disagrees with the shape, the input is empty, or
it holds a non-finite value; `ArgumentOutOfRangeException` when `outputCount` is below one.
`outputWeights` are refused with an `ArgumentException` too, in scikit-learn's words, in three cases:

- holding `NaN` or an infinity — "Input contains NaN." or its infinity counterpart
  ([#1461](https://github.com/CyrilB1531/lodestar/issues/1461));
- given for a single output — "Custom weights are useful only in multi-output cases."
  ([#1533](https://github.com/CyrilB1531/lodestar/issues/1533));
- summing to zero, such as `[0, 0]` or `[-1, 1]` — "Weights sum to zero, can't be normalized.",
  where scikit-learn raises `ZeroDivisionError`
  ([#1588](https://github.com/CyrilB1531/lodestar/issues/1588)).

**Example** — three exact predictions and one catastrophic one. `MeanAbsoluteError.Score` on this
data is `24`.

```csharp
using Lodestar.Metrics;

double[] yTrue = [1.0, 2.0, 3.0, 100.0];
double[] yPred = [1.0, 2.0, 3.0, 4.0];

double typical = MedianAbsoluteError.Score(yTrue, yPred);   // => 0
```

**Remarks** — reach for this when your data has outliers you do not believe in — mistyped labels,
a
sensor that dropped out, a fraud in the training set. Its breakdown point is 50%: half your
samples
can be arbitrarily wrong and this number does not move. That is a genuinely different question
from
the one `MeanAbsoluteError.Score` answers, and reporting the two side by side is the fastest way
to
see whether a dataset has a tail.

Which is the trap, stated as bluntly as the example above puts it: **`0` here does not mean the
model
is good.** It means at least half the predictions are exact, and says nothing about the other
half.
Never report this alone; pair it with `MeanAbsoluteError.Score` or `MaxError.Score`.

Under `sampleWeight` this stops being the value at the halfway point. scikit-learn takes an
*averaged* weighted percentile — the mean of the first value whose cumulative weight reaches half
the total and the one just past the last that comes within one machine epsilon of it — and that
tolerance is load-bearing rather than decoration: a uniform weight is *usually* the ordinary median
and not always. Measured, `[0.7] * 10` gives `5.0` on the weighted path against `4.5` unweighted,
while `[0.1] * 10` gives `4.5` on both. Both agree with scikit-learn, whose `_weighted_percentile`
compares against `np.finfo(np.float64).eps` rather than zero.

**Negative weights are read as scikit-learn reads them.** A negative weight makes the cumulative
weight fall as well as rise, and the halfway point is then found by numpy 2.5's own `searchsorted`,
whose branchless halving lands where a textbook binary search does not: `sampleWeight: [1, -1, 0]`
on `[1, 2, 3]` against `[1, 2, 4]` gives `1.0`, as scikit-learn does
([#1546](https://github.com/CyrilB1531/lodestar/issues/1546)). One case stays out of reach: equal
errors carrying different weights, where the result follows the order numpy's `argsort` leaves them
in, which is not stable and depends on the CPU's vector units. `sampleWeight: [-2, 0, 2, -1]` on
`[2, 2, 0, 1]` against zeros gives `0.0` there and `2.0` here. Measured, 1 random case in 2,000 with
continuous errors and about 5 % with tied integer errors, both under negative weights; with
non-negative weights that order changes nothing.

**Applies to** — net10.0, netstandard2.0.

**See also** — `MedianAbsoluteError.PerOutput`, `MeanAbsoluteError.Score`, `MaxError.Score`,
the [Python equivalence table](../../../equivalence.md).
