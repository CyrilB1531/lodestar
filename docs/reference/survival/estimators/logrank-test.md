# LogRank.Test

Compares the survival of two right-censored samples.

<!-- docs-declaration -->

```csharp
public static LogRankResult Test(ReadOnlySpan<double> durationsA, ReadOnlySpan<bool> eventObservedA, ReadOnlySpan<double> durationsB, ReadOnlySpan<bool> eventObservedB)
```

**Parameters** — `durationsA` holds the first group's non-negative durations and `eventObservedA`
the flag for each of them, `true` where the duration ends in the event. `durationsB` and
`eventObservedB` are the same pair for the second group. Each group's two spans must be the same
length; the two groups need not be the same size as each other.

**Returns** — a `LogRankResult` carrying the statistic, its upper-tail chi-squared p-value, and the
degrees of freedom, which are one for a two-sample comparison.

**Exceptions** — `ArgumentException` when a group's spans differ in length, a group is empty, or a
duration is negative or `NaN`.

**Example** — two arms that separate, and two that cannot.

```csharp
using Lodestar.Survival;

LogRankResult differ = LogRank.Test(
    [6, 7, 10], [true, true, true],
    [1, 2, 3], [true, true, true]);

LogRankResult same = LogRank.Test(
    [2, 4, 6, 8], [true, true, true, true],
    [2, 4, 6, 8], [true, true, true, true]);

double statistic = same.Statistic;  // => 0
double p = same.PValue;  // => 1
int df = differ.DegreesOfFreedom;  // => 1
```

**Remarks** — the second call is the test's own sanity check: identical arms give a statistic of
exactly zero, because at every time the observed count equals the expected one.

**The order of the two groups does not change the statistic.** It is a square, and swapping the arms
flips the sign of the difference being squared. What it does change is nothing a caller can observe,
which is why there is no "reference group" argument.

**A group entirely censored still contributes.** Its subjects sit in the pooled risk sets and raise
the other arm's expected counts, which is the opposite of dropping them — and the result is a larger
statistic, not a smaller one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank`](logrank.md),
[`Distributions.ChiSquaredSf`](../../stats/tails/distributions-chisquaredsf.md).
