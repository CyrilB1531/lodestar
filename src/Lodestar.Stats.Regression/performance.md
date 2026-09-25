# Performance — Lodestar.Stats.Regression

What `Lodestar.Stats.Regression` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Lodestar.Stats.Regression against Accord.Statistics (issue #566)

Full method, what the pair does and does not compare, and why `MathNet.Numerics` is not a candidate
here:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#19-lodestarstatsregression-against-accordstatistics-issue-566).
This section carries only the numbers, per the rule for where a fact belongs
(`CLAUDE.md`'s "Where a fact belongs" table).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.112, .NET 10.0.12 runtime — a
developer workstation shared with other checkouts of this repository, so **this row is indicative,
not authoritative**. Window: one `BenchmarkDotNet` run, `ShortRun` job (`IterationCount=3`,
`WarmupCount=3`, `LaunchCount=1`), 2026-09-10; run time 28.65 s across the 4 benchmarks
(2 pairs × 2 sample sizes). Four regressors throughout, on a seeded design.

| Method | SampleSize | Mean | Ratio | Allocated |
| --- | ---: | ---: | ---: | ---: |
| `Lodestar_Ols` | 100 | 49.75 μs | 1.00 | 68.84 KB |
| `Accord_Ols` | 100 | 136.55 μs | 2.74 | 43 KB |
| `Lodestar_Ols` | 10,000 | 3,700.63 μs | 1.00 | 6,490.19 KB |
| `Accord_Ols` | 10,000 | 2,779.11 μs | 0.75 | 3,523.47 KB |

**The two rows are not doing the same work, and that is the finding rather than a caveat.**
`Accord.Statistics` exports no variance inflation factor anywhere in its 4 796 members — decision
0003's reading established that — so `MultipleLinearRegressionAnalysis.Learn` performs one solve.
[`OrdinaryLeastSquares.Fit`](../../docs/reference/stats-regression/ols/ordinaryleastsquares-fit.md)
performs **five** at four regressors: the model, plus one auxiliary regression per regressor for
the VIFs, each with its own Householder QR over the full design.

Read that way the numbers are consistent. At 100 rows the fixed per-call overhead dominates and
this package is 2.7× faster despite doing five times the factorisation. At 10,000 rows the
`O(mn²)` work dominates instead, and doing it five times costs 1.33× Accord's single solve. The
allocation follows the same shape — 1.8× Accord's at both sizes — because each auxiliary
regression materialises a standardised copy of the design and its own `Q`.

**So the honest summary is a shape rather than a winner**: below roughly a thousand rows this
package is faster and returns strictly more; above it, the extra diagnostic is what you are paying
for. A caller who does not want VIFs has no way to say so today, and that is the obvious next
measurement rather than a defect — the table is one call by design.

## Lodestar.Stats.Regression's generalized linear model against Accord.Statistics (issue #678)

Full method, and why the untyped `GeneralizedLinearRegression` is the one driven:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#27-lodestarstatsregressions-generalized-linear-model-against-accordstatistics-issue-616).
**Accord.Statistics 3.8.0 is archived.** Its last package shipped on 2017-10-19 and the repository
was archived on 2020-11-18 ([`docs/migration/README.md`](../../docs/migration/README.md)). A lead over it is
a smaller claim than a lead over a maintained library, and a deficit against it is still a deficit.

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime, AVX-512.
Window: one `BenchmarkDotNet` run, **default job**, on 2026-09-14, 8 benchmarks. A binomial model with
a logit link and an intercept, both sides given 100 iterations and a `1e-8` tolerance. The benchmark
project builds `Lodestar.Stats` 0.5.0 from source. Its normal quantile is inverted by Newton (#709)
on the `erfc` `docs/guides/performance.md`
describes. `Lodestar.Stats.Regression`'s published floor, `Lodestar.Stats` 0.4.0, carries neither
yet.

| SampleSize | Regressors | `Lodestar_Glm` | `Accord_Glm` | Accord / Lodestar | Allocated, Lodestar | Allocated, Accord |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 200 | 1 | 29.48 μs | 29.61 μs | 1.00 | 50.54 KB | 117.67 KB |
| 200 | 3 | 59.40 μs | 61.64 μs | 1.04 | 95.51 KB | 168.44 KB |
| 2,000 | 1 | 292.42 μs | 213.73 μs | **0.73** | 486.48 KB | 881.77 KB |
| 2,000 | 3 | 579.36 μs | 406.43 μs | **0.70** | 925.2 KB | 1,265.51 KB |

**At 200 rows the two are level, and at 2,000 Accord is 1.4× faster.** This package allocates less
in every cell, by 1.4× to 2.3×.

**The 200-row result moved, and one call moved it.** On 2026-09-13, before the quantiles stopped
bisecting ([issue #709](https://github.com/CyrilB1531/lodestar/issues/709)), the same cells read
41.19 μs against Accord's 30.33 μs and 71.31 μs against 60.31 μs. The gap was 10.9 and 11.0 μs,
flat across the regressor count. That is the one
[`Distributions.NormalQuantile`](../../docs/reference/stats/tails/distributions-normalquantile.md) call the
intervals need, which `Accord`'s `GetWaldTest` does not make, and it then cost about 11.5 μs. It now
costs 654 ns, and the two 200-row rows fell by 11.7 and 11.9 μs. The 2,000-row rows fell by 14.7 and
24.6 μs, where `Accord`'s control rows moved by 3% to 4% the same way, so part of that is the window.

**At 2,000 rows the gap is per iteration, not iteration count.** The two stop on different
quantities: this package when the absolute change in deviance falls to the tolerance, statsmodels'
`atol` with `rtol` at zero; `Accord`'s `Run` when the largest relative change in the coefficients
does. On these designs this package stops after 4 iterations and `Accord` after 4 to 6. So at
2,000 × 1, where both take 4, each of this package's iterations costs about 73 μs against `Accord`'s
53 μs. This run does not attribute that difference. Each iteration here is a Householder
factorization of the weighted design, and reading its `Q` through `IReadOnlyList<double>` is one
cost [issue #670](https://github.com/CyrilB1531/lodestar/issues/670) already names.

**Both sides return the same fit, and the check that says so is not the timed call.** Run once
outside `BenchmarkDotNet` on the same seeded designs:

| stopping rule | coefficients, relative | standard errors, relative | first p-values, relative |
| --- | ---: | ---: | ---: |
| the benchmark's `1e-8` on both sides | 1.4e-10 | 5.1e-7 | 8.8e-5 |
| this package at `1e-13`, `Accord` at `1e-15` | 1.4e-10 | 6.1e-11 | 4.3e-10 |

The coefficients agree at the oracle tolerance under either rule. The standard errors do not under the
benchmark's: each side computes its covariance from the IRLS weights of its own last iterate, and the
two last iterates differ because the stopping rules do. Tightened, every quantity agrees within `1e-9`.
What this package computes beyond `Accord`'s coefficients and standard errors — every interval, every
p-value, the deviance, the null deviance, the log-likelihood and the AIC — is in the timed call.

## The least-squares pipeline against Math.NET Numerics (issue #782)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#36-weighted-least-squares-against-mathnet-numerics-and-the-least-squares-pipeline-under-it-issue-782).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime, AVX-512.
Window: one `BenchmarkDotNet` 0.14.0 run, **default job**, on 2026-09-15. `MathNet.Numerics` 5.0.0, `Accord.Statistics` 3.8.0. Every pair returned the same slope before it
was timed.

### Weighted least squares against `WeightedRegression.Weighted`

| n | [`WeightedLeastSquares.Fit`](../../docs/reference/stats-regression/wls/weightedleastsquares-fit.md) | Math.NET | Math.NET / Lodestar | Allocated, Lodestar | Allocated, Math.NET |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 200 | **6.650 μs** | 9.865 μs | **1.48** | 3.35 KB | 59.08 KB |
| 2,000 | **54.601 μs** | 54.718 μs | 1.00 | 17.41 KB | 509.88 KB |
| 20,000 | **0.591 ms** | 0.923 ms | **1.56** | 158.41 KB | 5,033.56 KB |
| 200,000 | **5.718 ms** | 11.072 ms | **1.94** | 1,564.58 KB | 50,035.09 KB |

**Level at 2,000 rows and faster everywhere else, while computing the whole table** — Math.NET
returns the coefficients alone — and allocating 17× to 32× less.

### Ordinary least squares against `MultipleRegression.QR` and Accord

| n | [`OrdinaryLeastSquares.Fit`](../../docs/reference/stats-regression/ols/ordinaryleastsquares-fit.md) | Math.NET | Accord | Math.NET / Lodestar | Allocated, Lodestar |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 100 | **3.938 μs** | 24.096 μs | 135.345 μs | **6.12** | 2.57 KB |
| 10,000 | **262.402 μs** | 860.391 μs | 2,677.370 μs | **3.28** | 79.91 KB |

At 10,000 rows the robust covariances on top of that fit cost 0.628–0.631 ms for HC0 to HC3, and
6.27–6.31 μs at 100 rows (`RobustCovarianceBenchmarks`).

## Generalized least squares against Math.NET Numerics (issue #771)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#38-generalized-least-squares-against-mathnet-numerics-issue-771).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-15, the run taken after GLS was
rebased on the shared least-squares pipeline of
[#782](https://github.com/CyrilB1531/lodestar/issues/782).
`MathNet.Numerics` 5.0.0. Four regressors and an intercept, AR(1) errors at 0.6; each pair returned the same
slope within `1e-9` before it was timed.

| n | [`GeneralizedLeastSquares.Fit`](../../docs/reference/stats-regression/gls/generalizedleastsquares-fit.md) | Math.NET, Cholesky and normal equations | Math.NET / Lodestar | Allocated, Lodestar | Allocated, Math.NET |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 50 | **17.79 μs** | 23.38 μs | **1.31** | 27.02 KB | 34.14 KB |
| 200 | **527.74 μs** | 1,010.36 μs | **1.91** | 336.49 KB | 371.24 KB |
| 500 | **5.763 ms** | 9.121 ms | **1.58** | 2,010.14 KB | 2,227.48 KB |
| 1,000 | **47.752 ms** | 57.188 ms | **1.20** | 7,924.16 KB | 9,049.56 KB |

**Faster than Math.NET at every size, while computing the whole table** — standard errors, p-values,
intervals, R², the F test and the VIFs — against a Math.NET path that stops at the coefficients.
Math.NET exports no GLS, so its row is the one its users write: its Cholesky factor of the
covariance, and the normal equations solved through it.

**From 500 rows on, the covariance's Cholesky factor is the fit**, `n³/6` products, which is why the
ratio falls toward 1.20 as `n` grows rather than holding at the 1.9 it reaches at 200 rows.

**Neither side is allocation-free**, and neither can be: the factor is `n²` doubles, 8 MB at 1,000
rows, and each side holds one. Lodestar allocates 0.79× to 0.91× what Math.NET does.

## The negative binomial GLM against statsmodels (issue #781)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#37-the-negative-binomial-glm-against-statsmodels-issue-781).
**No free .NET library fits this family at parity** — Accord's IRLS is right only for canonical links, and commercial
libraries are not timed under a trial licence — so the incumbent is `statsmodels` 0.15.0, through the cross-language
harness. Machine: the AMD Ryzen 7 8700G named above, on 2026-09-15; the Python side is one run of
`bench_stats.py` on the same machine. Milliseconds per fit, best of five, `α = 1`, four regressors
and an intercept. The coefficients agree to 1.3e-14 relative or better with the same iteration
counts.

| n | [`GeneralizedLinearModel.Fit`](../../docs/reference/stats-regression/glm/generalizedlinearmodel-fit.md) | `statsmodels`, wall | `statsmodels`, cpu | statsmodels / Lodestar, wall |
| ---: | ---: | ---: | ---: | ---: |
| 1,000 | **0.404 ms** | 1.437 ms | 1.437 ms | **3.56** |
| 10,000 | **4.136 ms** | 5.604 ms | 5.603 ms | **1.35** |
| 100,000 | **45.520 ms** | 74.138 ms | 1,174.297 ms | **1.63** |

The log-likelihood reads `lnΓ(1/α)` once rather than once per row — a few dozen logarithms per fit.
At 100,000 rows `statsmodels` reaches LAPACK through numpy's threads and spends sixteen times the
processor time it takes in wall clock; the `wall` column is still the comparison.

## The Gamma GLM against statsmodels, and the IRLS loop it widened (issue #770)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#39-the-gamma-glm-against-statsmodels-issue-770).
**No free .NET library fits this family** — Accord's generalized linear regression returned NaN coefficients on the
inverse link — so the incumbent is `statsmodels` 0.15.0, through the cross-language harness.
Machine: the AMD Ryzen 7 8700G named above. Window: `BenchmarkDotNet` 0.14.0 and the harness,
**default job**, on 2026-09-15; the Python side one run of `bench_stats.py`. Log link, four
regressors and an intercept; milliseconds per fit, best of five.

| n | [`GeneralizedLinearModel.Fit`](../../docs/reference/stats-regression/glm/generalizedlinearmodel-fit.md), Gamma | `statsmodels`, wall | `statsmodels`, cpu | statsmodels / Lodestar, wall |
| ---: | ---: | ---: | ---: | ---: |
| 1,000 | **0.371 ms** | 1.795 ms | 1.794 ms | **4.84** |
| 10,000 | **3.397 ms** | 6.634 ms | 6.633 ms | **1.95** |
| 100,000 | **43.016 ms** | 90.541 ms | 1,429.046 ms | **2.10** |

**The IRLS loop the other families share is unchanged by the Gamma link**, and their classes sit
level with `Accord` or ahead of it: `GlmBenchmarks`' logistic fit reads 21.75 μs at 200 rows and one
regressor against Accord's 28.29 μs, 29.80 against 58.82 at three regressors, 211.34 against 209.32
at 2,000 rows and one regressor — level — and 287.29 against 398.97 at three. `GlmPoissonBenchmarks`
has no Accord counterpart and reads 191.5 to 197.6 μs across mean counts of 5 to 5,000,000, at
16.85 KB.

## HAC and cluster-robust covariances against statsmodels (issue #775)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#40-hac-and-cluster-robust-covariances-against-statsmodels-issue-775).
**No free .NET library computes either covariance**, so the incumbent is `statsmodels` 0.15.0 through the cross-language
harness. Machine: the AMD Ryzen 7 8700G named above, on 2026-09-15. The harness side is one run of `compare-ols` and one of
`bench_stats.py`. Four regressors and an intercept, four lags, clusters of 20 consecutive rows; milliseconds per fit,
best of five, each row including the VIFs on both sides.

| n | HAC, Lodestar | HAC, `statsmodels` wall / cpu | ratio, wall | cluster, Lodestar | cluster, `statsmodels` wall / cpu | ratio, wall |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 1,000 | **0.097 ms** | 1.508 / 1.507 ms | **15.61** | **0.035 ms** | 1.537 / 1.537 ms | **43.37** |
| 10,000 | **0.995 ms** | 6.683 / 6.683 ms | **6.72** | **0.352 ms** | 6.892 / 6.891 ms | **19.56** |
| 100,000 | **10.646 ms** | 74.696 / 1,189.086 ms | **7.02** | **4.326 ms** | 76.800 / 1,223.598 ms | **17.75** |

What each covariance costs beside the ordinary fit, from `HacClusterBenchmarks`,
`BenchmarkDotNet` 0.14.0 default job:

| rows | Nonrobust | HC0 | HAC, 4 lags | Cluster |
| ---: | ---: | ---: | ---: | ---: |
| 100 | 3.94 μs, 2.59 KB | 5.49 μs, 8.58 KB | 10.92 μs, 12.73 KB | 4.81 μs, 9.54 KB |
| 10,000 | 259.9 μs, 79.93 KB | 556.3 μs, 472.72 KB | 1,212.5 μs, 863.67 KB | 489.1 μs, 565.00 KB |

HAC's filling runs once per lag, so four lags cost about five HC0 fillings.

HC0 and HC1 do not compute the leverages they never read. HC2 and HC3 do, and the two pairs sit
about 15 % apart at both sizes for that reason (`RobustCovarianceBenchmarks`: 5.52 and 5.60 μs
against 6.45 and 6.39 at 100 rows, 552 and 567 μs against 644 and 636 at 10,000).

## GLM offsets and exposure against statsmodels (issue #787)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#41-glm-offsets-and-exposure-against-statsmodels-issue-787).
Accord's GLM takes no offset, so the incumbent is `statsmodels` 0.15.0 through the cross-language
harness. Machine: the AMD Ryzen 7 8700G named above, on 2026-09-15; one run of `compare-glm` and one of `bench_stats.py`. A Poisson fit of the
corpus's counts, four regressors and an intercept, the exposure `1 + row % 3`; milliseconds per fit, best of five.

| n | Lodestar | `statsmodels`, wall / cpu | ratio, wall |
| ---: | ---: | ---: | ---: |
| 1,000 | **0.287 ms** | 1.659 / 1.658 ms | **5.79** |
| 10,000 | **3.299 ms** | 6.546 / 6.545 ms | **1.98** |
| 100,000 | **45.520 ms** | 70.576 / 1,099.885 ms | **1.55** |

What the exposure costs, from `GlmOffsetBenchmarks`, `BenchmarkDotNet` 0.14.0 default job:

| rows | Poisson | with an exposure | Allocated |
| ---: | ---: | ---: | ---: |
| 200 | 21.46 μs | 39.14 μs | 43.66 → 75.84 KB |
| 20,000 | 2,923.9 μs | 5,243.6 μs | 5,003.5 → 8,131.0 KB |

Most of the difference is the null deviance. With an offset it is a second, intercept-only IRLS fit, as the
reference computes it; without one it is the deviance at the response mean.

**The loop every fit shares is unaffected by the offset**: `GlmBenchmarks`' logistic rows read
22.71 and 30.93 μs at 200 rows (one and three regressors) and 222.05 and 297.03 μs at 2,000, and
`GlmPoissonBenchmarks` 189.0 to 207.9 μs across mean counts of 5 to 5,000,000.

## The multinomial logit against Accord and statsmodels (issue #788)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#42-the-multinomial-logit-against-accord-and-statsmodels-issue-788).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, default job, one run. Three regressors
and an intercept,
three categories.

| rows | [`MultinomialLogit.Fit`](../../docs/reference/stats-regression/mnlogit/multinomiallogit-fit.md) | Accord `LowerBoundNewtonRaphson`, tolerance `1e-10` | Accord / Lodestar | Allocated, Lodestar / Accord |
| ---: | ---: | ---: | ---: | ---: |
| 200 | **90.22 μs** | 556.05 μs | **6.16** | 21.63 KB / 1,466.83 KB |
| 2,000 | **877.42 μs** | 4,507.78 μs | **5.14** | 134.13 KB / 12,015.21 KB |

Accord's standard errors come from the lower-bound Hessian its algorithm iterates on, and are 33% to 43% from the ones
this fit and `statsmodels` report. The race compares coefficients only.

Against `statsmodels` 0.15.0 through `compare-glm`, one run of each side: the corpus's four regressors and an intercept,
the category the count modulo three, milliseconds per fit, best of five.

| n | Lodestar | `statsmodels`, wall / cpu | ratio, wall |
| ---: | ---: | ---: | ---: |
| 1,000 | **0.537 ms** | 7.203 / 7.202 ms | **13.42** |
| 10,000 | **5.758 ms** | 40.385 / 40.378 ms | **7.01** |
| 100,000 | **58.563 ms** | 407.374 / 1,853.258 ms | **6.96** |

Part of the difference is the null log-likelihood: `statsmodels` refits the constant-only model with Nelder–Mead and
BFGS, where this fit takes the closed form that refit approximates (decision 0004).

## Instrumental variables against linearmodels (issue #1155)

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#61-instrumental-variables-against-linearmodels-issue-1155).
**No .NET library estimates one**, free or commercial, so `linearmodels` 7.0 on numpy 2.5.3 is the only
incumbent. Machine: the AMD Ryzen 7 8700G named above, .NET 10.0.12, under the repository's machine
lock: the Python side 2026-09-25 11:06 to 11:10 UTC, the C# side 11:29 to 11:31 UTC, after the kernel
change below and the code review's fixes. Milliseconds per fit, best of five, each producing the whole table — first stage, Shea's
R² and the overidentification test — with three exogenous regressors and a constant, two endogenous
regressors and four instruments.

| rows | fit | Lodestar | `linearmodels`, wall / cpu | ratio, cpu |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | 2SLS, robust | **1.04 ms** | 27.9 / 27.9 ms | **26.8** |
| 1,000 | LIML, robust | **1.14 ms** | 28.5 / 28.5 ms | **25.1** |
| 1,000 | GMM, robust weight | **1.05 ms** | 28.1 / 28.1 ms | **26.9** |
| 1,000 | 2SLS, kernel | **5.67 ms** | 30.1 / 30.1 ms | **5.3** |
| 1,000 | 2SLS, clustered | **1.01 ms** | 29.3 / 29.3 ms | **28.9** |
| 10,000 | 2SLS, robust | **10.7 ms** | 131.2 / 262.4 ms | **23.6** |
| 10,000 | LIML, robust | **11.8 ms** | 132.2 / 264.4 ms | **21.5** |
| 10,000 | GMM, robust weight | **10.9 ms** | 132.2 / 264.2 ms | **23.5** |
| 10,000 | 2SLS, kernel | **59.9 ms** | 133.6 / 266.8 ms | **4.4** |
| 10,000 | 2SLS, clustered | **10.1 ms** | 140.0 / 280.0 ms | **26.9** |
| 100,000 | 2SLS, robust | **121 ms** | 1,886 / 19,146 ms | **154.6** |
| 100,000 | LIML, robust | **125 ms** | 1,800 / 18,581 ms | **144.7** |
| 100,000 | GMM, robust weight | **126 ms** | 1,676 / 17,650 ms | **135.2** |
| 100,000 | 2SLS, kernel | **2,257 ms** | 2,441 / 24,318 ms | **10.8** |
| 100,000 | 2SLS, clustered | **111 ms** | 1,790 / 18,794 ms | **166.5** |

**How it reads.** Ahead on every row, by 11× to 28× in elapsed time outside the kernel covariance.
At 100,000 rows `linearmodels` spends ten times more processor time than elapsed time, on numpy's
threaded BLAS; this runs on one thread, which is why the cpu ratio is the honest one there.

**The kernel covariance is the close row**, 1.08× in elapsed time at 100,000 rows: the automatic
bandwidth reaches 236 lags there, and the fit, its two first-stage regressions each repeating it,
sums a `k × k` cross-product per lag. The lags were first summed two products per element, 2,872 ms
and 0.85× the reference; summing each lag's cross-product once and adding it with its transpose,
the reference's own order, took it to 2,257 ms.
