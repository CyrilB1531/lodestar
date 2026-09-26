# LogRank

The log-rank family: two groups or more, weighted, pairwise.

<!-- docs-declaration -->

```csharp
public static class LogRank
```

**Example** — Freireich's two arms, which is what the test was published to compare.

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] treatmentObserved = [true, true, true, true, true, true, true, true, true,
                            false, false, false, false, false, false, false, false, false, false, false, false];
double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlObserved = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];

LogRankResult result = LogRank.Test(treatment, treatmentObserved, control, controlObserved);

double statistic = result.Statistic;  // => 16.79…
double p = result.PValue;  // => 4.168…
```

**Remarks** — at each time carrying an event in either arm, the first group's observed events are
compared against what the pooled risk sets would give it. The statistic is the squared total
difference over the summed variance, so it is never negative and is zero when the two curves agree
step for step.

**Ties are handled by the hypergeometric variance**, not by an approximation of it. With one event
at a time it reduces to the familiar `n₁n₂/n²`; with several it carries the `(n - d) / (n - 1)`
factor that a naive implementation drops, and that factor is what makes a heavily tied comparison
come out at the reference's number.

**Its p-value is the published chi-squared tail**, not a second one:
[`Distributions.ChiSquaredSf`](../../stats/tails/distributions-chisquaredsf.md) on one degree of
freedom. [Decision 0003](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
published that member for this call rather than let a second far-tail approximation into the tree,
and a test asserts the two routes agree.

A time that only censors, or a time where one subject remains, contributes nothing: the
hypergeometric variance is zero there and the term carries no information. Where no time compares
both groups at all, the result is a statistic of zero and a p-value of one — there is nothing to
reject rather than an error to raise.

**The weightings, the truncation and the subject weights** are [`LogRankOptions`](logrankoptions.md)
and the weighted overloads: Wilcoxon, Tarone-Ware, Peto and Fleming-Harrington weigh each time before
it is summed, and a truncation censors every event past it.

Reference behaviour is `lifelines.statistics` 0.30.3 — `logrank_test`, `multivariate_logrank_test`
and `pairwise_logrank_test` — matched under every weighting, with and without subject weights.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`KaplanMeier`](kaplanmeier.md).

## Members

| Member | What it does |
| --- | --- |
| [`LogRank.MultiGroup`](logrank-multigroup.md) | Compares several groups at once. |
| [`LogRank.Pairwise`](logrank-pairwise.md) | Runs the two-sample test on every pair of groups. |
| [`LogRank.Test`](logrank-test.md) | Compares the survival of two right-censored samples, under any weighting. |
