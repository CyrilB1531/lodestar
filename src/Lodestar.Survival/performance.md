# Performance — Lodestar.Survival

What `Lodestar.Survival` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## The extended Cox model against lifelines (issue #1171)

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#67-the-extended-cox-model-against-lifelines-issue-1171).
lifelines 0.30.3 on numpy 2.5.3 is the incumbent: no free .NET library fits a Cox model. Machine: AMD
Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores, .NET 10.0.12, under the
repository's machine lock on 2026-09-26: the C# side 10:18 to 10:19 UTC, the Python side 10:19 to 10:21
UTC. Milliseconds of processor time per call, best of five, four covariates, each fit at lifelines'
defaults as a caller runs it.

| operation | 1,000 | 10,000 |
| --- | ---: | ---: |
| [`CoxProportionalHazards.Fit`](../../docs/reference/survival/estimators/coxproportionalhazards-fit.md), three strata, weighted | 0.493 / 56.6 (**115**) | 5.74 / 316 (**55.1**) |
| `Fit`, ridge 0.1, robust variance | 0.551 / 54.7 (**99.3**) | 6.79 / 568 (**83.7**) |
| `Fit`, lasso 0.05 | 4.00 / 480 (**120**) | 36.8 / 3,155 (**85.8**) |
| [`TestProportionalHazards`](../../docs/reference/survival/estimators/coxproportionalhazards-testproportionalhazards.md), rank | 0.081 / 10.3 (**126**) | 1.43 / 98.5 (**69.0**) |
| [`PredictSurvivalFunction`](../../docs/reference/survival/estimators/coxsummary-predictsurvivalfunction.md), 100 subjects × 10 times | 0.020 / 0.790 (**40.0**) | 0.021 / 0.786 (**37.6**) |
| [`CoxTimeVarying.Fit`](../../docs/reference/survival/estimators/coxtimevarying-fit.md), two intervals per subject | 40.0 / 159 (**3.98**) | 1,563 / 2,802 (**1.79**) |

Each cell is Lodestar / lifelines, and the ratio lifelines over Lodestar.

**How it reads.** Ahead on every row. The time-varying fit is the close one: at each event time both
sides rebuild the risk set from every interval, lifelines' algorithm and this one's, so the cost grows
with events times intervals on both sides and the margin is only the per-interval arithmetic. The lasso
is lifelines' own loop, reproduced step for step, so both sides take the same number of steps; the
robust variance is linear here where lifelines' score residuals are quadratic in the subjects.

## The log-rank family, the restricted mean and the concordance index against lifelines (issue #1170)

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#66-the-log-rank-family-the-restricted-mean-and-the-concordance-index-against-lifelines-issue-1170).
No .NET library computes any of these, so lifelines 0.30.3 on numpy 2.5.3 is the incumbent. Machine:
AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores, .NET 10.0.12,
under the repository's machine lock on 2026-09-26: the Python side 08:05 to 08:07 UTC, the C# side
08:07 to 08:10 UTC. Milliseconds of processor time per call, best of five; each operation includes
the fit it reads.

| operation | 1,000 | 10,000 | 100,000 |
| --- | ---: | ---: | ---: |
| [`LogRank.Test`](../../docs/reference/survival/estimators/logrank-test.md), Wilcoxon, weighted | 0.014 / 5.87 (**413**) | 0.157 / 6.92 (**44.1**) | 2.76 / 12.9 (**4.68**) |
| `LogRank.Test`, Fleming-Harrington | 0.020 / 9.05 (**447**) | 0.210 / 11.3 (**54.0**) | 2.68 / 20.8 (**7.77**) |
| [`LogRank.MultiGroup`](../../docs/reference/survival/estimators/logrank-multigroup.md), five groups | 0.067 / 11.9 (**179**) | 1.47 / 20.2 (**13.8**) | 6.86 / 32.0 (**4.67**) |
| [`LogRank.Pairwise`](../../docs/reference/survival/estimators/logrank-pairwise.md), ten pairs | 0.175 / 60.1 (**343**) | 2.94 / 71.8 (**24.4**) | 29.2 / 129 (**4.42**) |
| [`KaplanMeier.RestrictedMean`](../../docs/reference/survival/estimators/kaplanmeier-restrictedmean.md), with the fit | 0.054 / 470 (**8,722**) | 1.13 / 522 (**464**) | 5.95 / 193 (**32.5**) |
| [`KaplanMeier.CompareAt`](../../docs/reference/survival/estimators/kaplanmeier-compareat.md), with two fits | 0.022 / 6.29 (**284**) | 0.215 / 7.87 (**36.7**) | 3.30 / 15.8 (**4.79**) |
| [`Concordance.Index`](../../docs/reference/survival/estimators/concordance-index.md) | 0.031 / 3.99 (**130**) | 1.45 / 47.9 (**33.0**) | 18.5 / 414 (**22.3**) |

Each cell is Lodestar / lifelines, and the ratio lifelines over Lodestar.

**How it reads.** Ahead on every row, on both clocks: lifelines runs single-threaded here, so its
elapsed and processor times agree. The small sizes measure lifelines' pandas overhead more than any
arithmetic. The restricted mean is the outlier because lifelines integrates the second moment by
adaptive quadrature over a step function, which is both the cost and the 1.5 % divergence the
reference page records; this sums the steps.
