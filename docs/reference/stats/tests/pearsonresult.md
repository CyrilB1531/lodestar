# PearsonResult

A Pearson correlation and the p-value that goes with it.

<!-- docs-declaration -->

```csharp
public sealed record PearsonResult(double Statistic, double PValue)
```

**Properties** — `Statistic` is the correlation coefficient, in `[-1, 1]`. `PValue` is the
p-value on the requested tail.

**Example** — the coefficient and its interval come from one call, not two.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

PearsonResult result = Pearson.Test(sugar, sweetness);
(double low, double high) = result.ConfidenceInterval();

double lower = Math.Round(low, 4);    // => 0.8783
double upper = Math.Round(high, 4);   // => 0.9961
```

**Remarks** — alongside the two public properties, a `PearsonResult` privately carries the number
of pairs it was computed over and the tail that was asked for. They are internal because a caller
only reaches them through
[`ConfidenceInterval`](pearsonresult-confidenceinterval.md), which needs both: the sample size
sets the standard error, and the tail decides whether the interval is half-open. A second call
that re-derived them would be a second chance to disagree with the first, which is the reason
scipy carries the same two on its own `PearsonRResult`.

Being a `record`, two results with the same statistic and p-value are equal — the internal
fields take part in that equality too, so two results from samples of different sizes are not
equal even when their two numbers coincide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Pearson.Test`](pearson-test.md), [`TestResult`](testresult.md),
[`TTestResult`](ttestresult.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`PearsonResult.ConfidenceInterval`](pearsonresult-confidenceinterval.md) | The confidence interval for the correlation, through the Fisher z transform. |
