# CalibrationCurve.Compute

Draws the reliability curve — `sklearn.calibration.calibration_curve`.

<!-- docs-declaration -->

```csharp
public static CalibrationCurve Compute(ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yProb, int posLabel = 1, int nBins = 5, BinStrategy strategy = BinStrategy.Uniform)
```

**Parameters** — `yTrue` is the true labels, one per sample, naming at most two classes. `yProb` is a
predicted probability per sample, each within `[0, 1]`. `posLabel` is the label counted as positive,
`1` by default where the reference infers it. `nBins` is how many bins to cut `[0, 1]` into, `5` by
default. `strategy` is where the edges come from — see [the type page](calibrationcurve.md) for what
the two divide.

**Returns** — a `CalibrationCurve` whose `ProbTrue` and `ProbPred` share a length that is **at most**
`nBins`: a bin no sample fell into is dropped.

**Exceptions** — `ArgumentException` when the inputs disagree in length, are empty, carry a
probability outside `[0, 1]` while none is NaN, or name more than two classes. A NaN probability is
not refused, as scikit-learn does not refuse it: numpy's minimum and maximum turn NaN, so its range
test passes — for every other probability too — the NaN falls in the last bin, or every bin's edge
is NaN under the quantile strategy, and the bin's mean comes out NaN
([#1464](https://github.com/CyrilB1531/lodestar/issues/1464)). `posLabel` is read as an explicit
`pos_label`: a `yTrue` of two labels, neither of them `posLabel`, scores every sample negative, as
`calibration_curve(..., pos_label=1)` does on `{0, 2}`; the reference refuses `{0, 2}` only when
`pos_label` is left out, which a C# default cannot be
([#1542](https://github.com/CyrilB1531/lodestar/issues/1542)).
`ArgumentOutOfRangeException` when `nBins` is below `1`, or past the length of the largest array ([#1538](https://github.com/CyrilB1531/lodestar/issues/1538)).

**Example** — four probabilities over five bins, one of which nothing falls into.

```csharp
using Lodestar.Metrics;

int[] truth = [0, 1, 1, 0];
double[] probabilities = [0.1, 0.9, 0.8, 0.3];

CalibrationCurve curve = CalibrationCurve.Compute(truth, probabilities);
int points = curve.ProbTrue.Count;   // => 4
```

**Remarks** — there is no `sampleWeight`: the reference has none for this curve, where
[`BrierScore.Score`](brierscore-score.md) and [`LogLoss.Score`](logloss-score.md) both take one.
`BinStrategy.Quantile` reads its edges from a linear-interpolation percentile rather than from the
weighted one the medians are pinned to, because the reference does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BrierScore.Score`](brierscore-score.md), [`LogLoss.Score`](logloss-score.md),
[`RocCurve.Compute`](roccurve-compute.md), the
[Python equivalence table](../../../equivalence.md).
