# LogRankResult

The outcome of a log-rank test, two groups or more.

<!-- docs-declaration -->

```csharp
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom)
```

**Parameters** — `Statistic` is the log-rank statistic, `PValue` its upper-tail chi-squared
probability, and `DegreesOfFreedom` is one for two groups and one fewer than the groups for more.

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

**Remarks** — `Statistic` is a quadratic form and is never negative. `DegreesOfFreedom` is a field
rather than a constant because [`LogRank.MultiGroup`](logrank-multigroup.md) reports `k − 1`, and a
caller reading it should not have to know which test produced the record.

`PValue` comes from
[`Distributions.ChiSquaredSf`](../../stats/tails/distributions-chisquaredsf.md), the same tail the
chi-squared tests in `Lodestar.Stats` report.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank.Test`](logrank-test.md).
