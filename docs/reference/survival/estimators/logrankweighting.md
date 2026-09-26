# LogRankWeighting

Which member of the log-rank family [`LogRank`](logrank.md) runs: lifelines' `weightings=`.

<!-- docs-declaration -->

```csharp
public enum LogRankWeighting { LogRank, Wilcoxon, TaroneWare, Peto, FlemingHarrington }
```

**Members** — `LogRank`, the default, weighs every event time one: lifelines' `weightings=None`.
`Wilcoxon` weighs it by the pooled number at risk, Gehan and Breslow's generalised Wilcoxon test.
`TaroneWare` weighs it by that number's square root. `Peto` weighs it by Peto and Peto's modified
survival estimate. `FlemingHarrington` weighs it by `S^p (1 − S)^q`, `S` the pooled Kaplan-Meier
curve just before the time, with `p` and `q` from [`LogRankOptions`](logrankoptions.md).

**Example** — the early weightings and the late one on Freireich's two arms.

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] treatmentObserved = [true, true, true, true, true, true, true, true, true,
                            false, false, false, false, false, false, false, false, false, false, false, false];
double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlObserved = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];

double wilcoxon = LogRank.Test(treatment, treatmentObserved, control, controlObserved,
    new LogRankOptions { Weighting = LogRankWeighting.Wilcoxon }).Statistic;
double late = LogRank.Test(treatment, treatmentObserved, control, controlObserved,
    new LogRankOptions { Weighting = LogRankWeighting.FlemingHarrington, P = 0, Q = 1 }).Statistic;

double early = Math.Round(wilcoxon, 6);   // => 13.457852
double lateOnly = Math.Round(late, 6);    // => 13.048449
```

**Remarks** — **the weight says where a difference counts.** Wilcoxon, Tarone-Ware and Peto weigh the
early times, where most subjects are still at risk; Fleming-Harrington with `q > 0` weighs the late
ones, and with `p = q = 0` it is the log-rank test itself. Choose before looking at the data: picking
the weighting that rejects is a multiple comparison the p-value does not know about.

**Peto's weight includes the time it weighs**, `∏ (1 − d/(n + 1))` up to and including it, as
lifelines computes it; the pooled curve under Fleming-Harrington is the one just before.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRankOptions`](logrankoptions.md), [`LogRank.Test`](logrank-test.md).
