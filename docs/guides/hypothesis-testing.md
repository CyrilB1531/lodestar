# Hypothesis testing

`Lodestar.Stats` answers one question in ten forms: **is this difference more
than noise?**

## What happens to a missing value

By default a `NaN` anywhere in a sample reaches the statistic and the p-value, which is scipy's
`nan_policy='propagate'` and what every test here did before the parameter existed. Pass
[`NanPolicy.Omit`](../reference/stats/nanpolicy.md) to drop the missing observations instead, or
`NanPolicy.Raise` to refuse the input. For a paired test, `Omit` drops the **pair** — filtering the
two samples separately would change what is being tested, which is the mistake the parameter
exists to prevent.

## Which test

| you have | and you assume | use |
| --- | --- | --- |
| two independent samples | roughly normal | [`TTest.Independent`](../reference/stats/tests/ttest-independent.md) |
| two independent samples | nothing about the shape | [`MannWhitney.Test`](../reference/stats/tests/mannwhitney-test.md) |
| the same subjects measured twice | roughly normal differences | [`TTest.Paired`](../reference/stats/tests/ttest-paired.md) |
| the same subjects measured twice | nothing about the shape | [`Wilcoxon.Paired`](../reference/stats/tests/wilcoxon-paired.md) |
| counts in categories | a stated expected distribution | [`ChiSquare.GoodnessOfFit`](../reference/stats/tests/chisquare-goodnessoffit.md) |
| a contingency table | cells large enough for the approximation | [`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) |
| a 2×2 table with small cells | nothing | [`FisherExact.Test`](../reference/stats/tests/fisherexact-test.md) |
| two samples, whole distributions | nothing | [`KolmogorovSmirnov.TwoSample`](../reference/stats/tests/kolmogorovsmirnov-twosample.md) |
| three or more groups | roughly normal, similar spread | [`OneWayAnova.Test`](../reference/stats/tests/onewayanova-test.md) |
| three or more groups | nothing about the shape | [`KruskalWallis.Test`](../reference/stats/tests/kruskalwallis-test.md) |
| one sample, and a normality assumption to check | nothing | [`ShapiroWilk.Test`](../reference/stats/tests/shapirowilk-test.md) |
| many p-values at once | nothing | [`MultipleComparisons`](../reference/stats/tests/multiplecomparisons.md) |

## What a p-value is, and is not

A p-value is the probability of seeing a difference at least this large **if the
null hypothesis is true**. It is not the probability that the null hypothesis is
true, and it is not the probability that your result is a fluke. A p-value of
0.03 does not mean there is a 3 % chance you are wrong.

Two consequences worth acting on:

- **`0.049` and `0.051` are the same evidence.** The threshold is a convention,
  not a discovery. Report the number.
- **Twenty tests at 5 % produce one significant result by chance.** That is what
  [`MultipleComparisons`](../reference/stats/tests/multiplecomparisons.md) is
  for, and it is not optional once you are testing more than a couple of things.

The step-up procedure below is
[`MultipleComparisons.BenjaminiHochberg`](../reference/stats/tests/multiplecomparisons-benjaminihochberg.md),
the one most people reach for first:

```csharp
using Lodestar.Stats;

double[] pValues = [0.001, 0.008, 0.039, 0.041, 0.042];

double[] adjusted = MultipleComparisons.BenjaminiHochberg(pValues);

bool stillSignificant = adjusted[0] < 0.05;   // => True
```

## One default that is not scipy's

[`TTest.Independent`](../reference/stats/tests/ttest-independent.md) defaults to
Welch's test; `scipy.stats.ttest_ind` defaults to Student's. Pooling the two
variances is only correct when the populations really share one, which is an
assumption most callers have not checked. Pass `Variance.Equal` for scipy's
default. Everything else in this package matches `scipy.stats` 1.18.0 exactly,
and the [equivalence table](../equivalence.md) is the row-by-row map, including
the handful of places one call refuses rather than answering — a NaN, a
warning, or a number a caller did not ask for.

## Exact and asymptotic

Three tests carry both an exact null distribution and a normal approximation to
it, selected by `ExactMethod`:

- `Auto` — exact for a small, untied sample; asymptotic otherwise. What `scipy`'s
  `method='auto'` does, and the same thresholds.
- `Exact` — always the exact distribution. On tied data the number is only
  approximate, because ties break the equal-probability argument the enumeration
  rests on; `scipy` computes there too rather than refusing, and so does this.
- `Asymptotic` — always the normal approximation, whatever the sample size.

The branch changes the number, not just the running time, which is why it is a
parameter and not a hidden optimisation. Each of the three tests also refuses
`ExactMethod.Exact` past its own size bound — the exact table is
`O(n·m)` or worse to build, and `Auto` never crosses that bound on its own,
falling back to the asymptotic answer instead. The three reference pages
([`MannWhitney.Test`](../reference/stats/tests/mannwhitney-test.md),
[`Wilcoxon.Paired`](../reference/stats/tests/wilcoxon-paired.md),
[`KolmogorovSmirnov.TwoSample`](../reference/stats/tests/kolmogorovsmirnov-twosample.md))
each state their own bound, because it is not the same number twice.

## The incumbents, and the one measured

`MathNet.Numerics` 5.0.0 (2022-04-03, 74.7M downloads) is the dominant
third-party numerical library for .NET, and it ships probability distributions
and descriptive statistics — no hypothesis tests. ML.NET does prediction: a t-test
exists only in Azure ML Studio (classic), a retired hosted product, and
Mann-Whitney only in Kusto/KQL — neither is a .NET library a project can
reference.

Three .NET libraries do carry the tests, and this page said for a while that only the first did
([decision 0129](../decisions/0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)
has the reading that corrected it):

- **`Accord.Statistics` 3.8.0**, LGPL-2.1, last published on **2017-10-19**; its framework,
  `accord-net/framework` (4.5k stars), was **archived by its owner on 2020-11-19**.
- **`Meta.Numerics` 4.2.0**, MS-PL, `netstandard2.0`, published 2025-07-14 after five years
  without a release. It carries eight of this package's ten families — the t-tests,
  Mann-Whitney, Wilcoxon, Kruskal-Wallis, Kolmogorov-Smirnov, one-way ANOVA, Fisher's exact test
  and the chi-square table — and Shapiro-Francia rather than Shapiro-Wilk. It is **not measured
  yet**: [#756](https://github.com/CyrilB1531/lodestar/issues/756).
- **`Numerics.NET` 10.7.0**, formerly Extreme Optimization, maintained and **commercial**. Named
  so its absence from the benchmarks is not mistaken for an absence from .NET.

`Accord.Statistics` is the one measured so far, because #442's own constraint asks for a named
.NET incumbent where one exists at all. [`TTest.Independent`](../reference/stats/tests/ttest-independent.md),
[`MannWhitney.Test`](../reference/stats/tests/mannwhitney-test.md) and
[`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) are benchmarked and
cross-checked against it in
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#18-lodestarstats-against-accordstatistics-issue-442)
and [`docs/guides/performance.md`](performance.md#lodestarstats-against-accordstatistics-issue-442) —
archived is not the same as absent, and this package's own oracle discipline
means the comparison is also a second opinion on `scipy`, not only a timing.
