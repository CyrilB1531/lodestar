# LogRankOptions

Which member of the log-rank family [`LogRank`](logrank.md) runs, and up to when.

<!-- docs-declaration -->

```csharp
public sealed record LogRankOptions
```

**Properties** — `Weighting` is the [`LogRankWeighting`](logrankweighting.md), `LogRank` by default.
`P` and `Q` are the Fleming-Harrington exponents, lifelines' `p` and `q`, zero by default and read
under that weighting alone. `Truncation` is lifelines' `t_0`: every event after it counts as a
censoring, `null` for none.

**Example** — the log-rank test stopped at time 10.

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] treatmentObserved = [true, true, true, true, true, true, true, true, true,
                            false, false, false, false, false, false, false, false, false, false, false, false];
double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlObserved = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];

LogRankResult untilTen = LogRank.Test(treatment, treatmentObserved, control, controlObserved,
    new LogRankOptions { Truncation = 10 });

double statistic = Math.Round(untilTen.Statistic, 6);   // => 6.942586
```

**Remarks** — a `Weighting` outside the enumeration, `P` or `Q` negative, infinite or `NaN`, or a `Truncation` negative or `NaN`, is refused
with `ArgumentOutOfRangeException` by every `LogRank` entry point, as lifelines refuses a negative `p`
or `q`. The truncation censors events; the subjects stay in the risk sets up to their durations.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRankWeighting`](logrankweighting.md), [`LogRank`](logrank.md).
