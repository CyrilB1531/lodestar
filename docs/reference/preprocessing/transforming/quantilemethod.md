# QuantileMethod

Which percentile convention a quantile-strategy fit reads.

<!-- docs-declaration -->

```csharp
public enum QuantileMethod
```

**Values** — `Linear` interpolates between the two neighbouring order statistics; numpy's
`'linear'`. `AveragedInvertedCdf` reads the inverted empirical CDF, averaged where it jumps;
numpy's `'averaged_inverted_cdf'`, and the default
[`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md) takes.

**Example** — six values cut into three.

```csharp
using Lodestar.Preprocessing;

double[] six = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];

var ordinal = new KBinsDiscretizerOptions { BinCount = 3, Encoding = BinEncoding.Ordinal };

KBinsDiscretizer averaged = KBinsDiscretizer.Fit(six, 1, ordinal);

double firstEdge = averaged.BinEdges[0][1];    // => 2.5
double secondEdge = averaged.BinEdges[0][2];   // => 4.5
```

**Remarks — these are two definitions of the same word, not two approximations of one answer.**
The third of six values is genuinely ambiguous: `AveragedInvertedCdf` answers 2.5, the midpoint of
the two order statistics that straddle the level, and `Linear` answers 2.667, the point two thirds
of the way from the second to the third. Neither is more accurate; they differ in what "the 33rd
percentile" is taken to mean.

**The arithmetic order matters, and is not the obvious one.** The position is `n × (percent / 100)`
and not `n × percent / 100`: for nine values at a third, the first gives `3.0000000000000004` and
reads one order statistic, the second gives exactly `3.0` and averages two — a whole different
answer from a rounding difference. This follows numpy, which the reference calls.

[`RobustScaler`](../scaling/robustscaler.md) reads a third convention, numpy's `'linear'` under a
different name, because its own reference calls `numpy.percentile` with the default.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md),
[`BinStrategy`](binstrategy.md).
