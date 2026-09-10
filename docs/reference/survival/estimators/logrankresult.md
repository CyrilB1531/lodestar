# LogRankResult

The outcome of a two-sample log-rank test.

<!-- docs-declaration -->

```csharp
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom)
```

**Parameters** — `Statistic` is the log-rank statistic, `PValue` its upper-tail chi-squared
probability, and `DegreesOfFreedom` is one for a two-sample comparison.

**Example** — two arms that cannot separate.

```csharp
using Lodestar.Survival;

LogRankResult result = LogRank.Test(
    [2, 4, 6], [true, true, true],
    [2, 4, 6], [true, true, true]);

double statistic = result.Statistic;  // => 0
double p = result.PValue;  // => 1
int df = result.DegreesOfFreedom;  // => 1
```

**Remarks** — `Statistic` is a square and is never negative. `DegreesOfFreedom` is a field rather
than a constant because a k-sample log-rank would report `k - 1`, and a caller reading it should not
have to know which test produced the record.

`PValue` comes from
[`Distributions.ChiSquaredSf`](../../stats/tails/distributions-chisquaredsf.md), the same tail the
chi-squared tests in `Lodestar.Stats` report.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank.Test`](logrank-test.md).
