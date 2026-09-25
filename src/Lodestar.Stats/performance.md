# Performance — Lodestar.Stats

What `Lodestar.Stats` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## The four laws' tails and quantiles against Math.NET Numerics and Meta.Numerics (issue #1158)

Full method: [`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#60-the-four-laws-tails-and-quantiles-against-mathnet-numerics-and-metanumerics-issue-1158).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime — a
dedicated machine, not a container. Window: one `BenchmarkDotNet` 0.14.0 run, default job,
2026-09-25 08:04 to 08:13 UTC, across the 24 benchmarks, under the repository's machine lock. The
three libraries' sixteen values were compared before anything was timed: **0 recorded
differences past `1e-9`**.

| operation | Lodestar | Math.NET Numerics | Meta.Numerics |
| --- | ---: | ---: | ---: |
| Student's t lower tail, `t = −2`, 10 df | **178.1 ns** | 185.4 ns | 203.1 ns |
| Student's t quantile, `p = 0.975`, 10 df | **500.5 ns** | 3,331 ns | 746.1 ns |
| F lower tail, `f = 4`, 2 and 20 df | **59.4 ns** | 79.5 ns | 85.3 ns |
| F quantile, `p = 0.95`, 2 and 20 df | 205.7 ns | 1,532 ns | **201.4 ns** |
| chi-squared lower tail, `x = 9.488`, 4 df | 40.9 ns | **36.3 ns** | 43.3 ns |
| chi-squared quantile, `p = 0.95`, 4 df | 377.2 ns | 283.1 ns | **231.8 ns** |
| normal lower tail, `z = −1.96` | **11.1 ns** | 12.6 ns | 45.5 ns |
| normal quantile, `p = 0.975` | 69.4 ns | **17.4 ns** | 116.3 ns |

Nothing on Lodestar's side allocates; Math.NET's t and F quantiles allocate 112 B and 104 B, and
Meta.Numerics' t quantile 288 B.

**How it reads.** On the tails Lodestar is ahead on three of four, and 13% behind Math.NET on the
chi-squared one. The quantiles split. The Student quantile is **6.7×** Math.NET's and **1.5×**
Meta.Numerics', and the F quantile **7.4×** Math.NET's and level with Meta.Numerics', 2% behind.
Two are behind: the chi-squared quantile, **1.6×** slower than Meta.Numerics' and **1.3×** than
Math.NET's, which is the new inverse of the incomplete gamma paying a Newton step per evaluation
of a full incomplete gamma; and the normal quantile, published since 0.4.0, **4.0×** slower than
Math.NET's rational approximation, because it inverts the library's own normal tail rather than
a separate approximation of the quantile. Both are correct to the corpus's `1e-9` across a grid
reaching `1e-300`; whether they should trade an evaluation for speed is its own question.

## Lodestar.Stats against scipy (issue #1162) — Fligner-Killeen, the k-sample Anderson-Darling test, Spearman's matrix and the point-biserial correlation

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#22-lodestarstats-and-lodestarstatsregression-against-scipy-and-statsmodels-issue-595),
the `stats` harness over its seeded corpus. No free .NET library computes any of the four, so
scipy 1.18.1 on numpy 2.5.3 is the incumbent. Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics,
1 CPU, 16 logical and 8 physical cores, .NET 10.0.12, under the repository's machine lock: the
Python side 2026-09-25 22:09 to 22:17 UTC, the C# side 22:25 to 22:27 UTC, one core of sixteen
taken by an unrelated process throughout. Milliseconds per call, best of five.

| rows | test | Lodestar | scipy, wall / cpu | ratio, cpu |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | Fligner-Killeen | **0.046 ms** | 0.312 / 0.312 ms | **6.82** |
| 1,000 | k-sample Anderson-Darling | **0.051 ms** | 0.215 / 0.215 ms | **4.21** |
| 1,000 | Spearman's matrix | **0.063 ms** | 0.337 / 0.337 ms | **5.32** |
| 1,000 | point-biserial | **0.007 ms** | 0.211 / 0.211 ms | **28.83** |
| 10,000 | Fligner-Killeen | **0.824 ms** | 2.00 / 2.00 ms | **2.36** |
| 10,000 | k-sample Anderson-Darling | **1.18 ms** | 3.49 / 3.49 ms | **2.73** |
| 10,000 | Spearman's matrix | **1.28 ms** | 2.81 / 2.81 ms | **2.10** |
| 10,000 | point-biserial | **0.069 ms** | 0.248 / 0.248 ms | **3.61** |
| 100,000 | Fligner-Killeen | **8.34 ms** | 20.7 / 20.7 ms | **2.41** |
| 100,000 | k-sample Anderson-Darling | **9.16 ms** | 42.4 / 42.4 ms | **4.43** |
| 100,000 | Spearman's matrix | **10.1 ms** | 32.3 / 32.3 ms | **3.04** |
| 100,000 | point-biserial | **0.937 ms** | 1.11 / 17.1 ms | **17.94** |

**How it reads.** Ahead on every row. The point-biserial correlation at 100,000 rows is the close
one in elapsed time, 1.18×, where numpy spreads its reduction over threads and spends sixteen
times the processor time doing it.

**Ranking was the cost.** The first reading was behind on nine of these rows: Spearman's matrix
ranked each column once per partner (0.32× at 100,000), the k-sample test sorted with LINQ and
binary-searched each distinct value (0.64×), and Fligner-Killeen refined every normal score by
Newton (0.70×). Ranking each column once, walking the sorted samples beside the pooled values,
reading the scores from Wichura's AS 241 alone and sorting large samples by radix took them to
the table above. The radix sort serves every rank test in the package from 8,192 values up.

## Lodestar.Stats against Accord.Statistics (issue #1121) — the variance, proportion and fit tests

Full method, how `Accord`'s names were resolved, why Friedman has no row and what `Accord`'s
Anderson-Darling refuses:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#54-the-variance-proportion-and-fit-tests-against-accordstatistics-issue-1121).
This section carries only the numbers, per `CLAUDE.md`'s "Where a fact belongs" table.

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime — a
dedicated machine, not a container. Window: one `BenchmarkDotNet` 0.14.0 run, default job,
2026-09-23, 5 min 25 s across the 18 benchmarks (9 rows × 2 sizes), no other load. Every pair's
statistic was asserted equal before anything was timed; the one disagreement is recorded in
`bench/README.md` §54.

| Method | GroupSize | Mean | Allocated |
| --- | ---: | ---: | ---: |
| `Lodestar_Levene` | 100 | 1.161 μs | 5,072 B |
| `Accord_Levene` | 100 | 2.365 μs | 5,480 B |
| `Lodestar_Bartlett` | 100 | 374.9 ns | 80 B |
| `Accord_Bartlett` | 100 | 723.4 ns | 168 B |
| `Lodestar_Binomial` | 100 | 765.4 ns | 48 B |
| `Accord_Binomial` | 100 | 2.586 μs | 2,216 B |
| `Lodestar_AndersonDarling` | 100 | 4.777 μs | 1,000 B |
| `Accord_AndersonDarling` | 100 | 3.659 μs | 1,096 B |
| `Lodestar_ClopperPearson` | 100 | 3.008 μs | 48 B |
| `Lodestar_Levene` | 10,000 | 90.10 μs | 480,272 B |
| `Accord_Levene` | 10,000 | 892.4 μs | 480,681 B |
| `Lodestar_Bartlett` | 10,000 | 36.77 μs | 80 B |
| `Accord_Bartlett` | 10,000 | 73.32 μs | 168 B |
| `Lodestar_Binomial` | 10,000 | 1.173 μs | 48 B |
| `Accord_Binomial` | 10,000 | 522.2 μs | 200,217 B |
| `Lodestar_AndersonDarling` | 10,000 | 845.1 μs | 80,201 B |
| `Accord_AndersonDarling` | 10,000 | refused | — |
| `Lodestar_ClopperPearson` | 10,000 | 22.00 μs | 48 B |

**[`Binomial.Test`](../../docs/reference/stats/tests/binomial-test.md) is 3.4× ahead at a hundred trials
and 445× at ten thousand, allocating 48 bytes against 200 kilobytes.** That is a difference in
what is computed, not in how well: `Accord` sums the binomial mass term by term, which is `O(n)`
and allocates an array of it, where this evaluates the regularized incomplete beta that sum *is*,
in constant time and constant space. The gap therefore widens with every further order of
magnitude.

[`Levene.Test`](../../docs/reference/stats/tests/levene-test.md) is 2.0× ahead at a hundred values and
9.9× at ten thousand. Its default centre is the median, which it takes by Hoare selection rather
than by sorting — `O(n)` against `O(n log n)`, and at this scale the sort was the test.
[`Bartlett.Test`](../../docs/reference/stats/tests/bartlett-test.md) is a steady 1.9× to 2.0× at both
sizes, on half the allocation.

**[`AndersonDarling.Test`](../../docs/reference/stats/tests/andersondarling-test.md) is the one row behind
at a hundred values — 1.31× — and has no counterpart at all at ten thousand**, where `Accord`'s
own p-value conversion throws. The cost here is deliberate: the statistic sums the logarithms of
both normal tails, and this package evaluates them through a log-tail with an asymptotic branch
rather than through a plain CDF, so an observation ten standard deviations out contributes a
number instead of taking the whole statistic to negative infinity. That is worth 1.1 μs on a
hundred values, and it is the difference between a test that survives an outlier and one that
does not.

`Lodestar_ClopperPearson` has no counterpart either — `Accord` exports no interval for a
proportion — so its two rows measure what the exact interval costs on top of the test: about four
times the test at a hundred trials, and nineteen times at ten thousand, all of it in the beta
inversion.

## Lodestar.Stats against Meta.Numerics (issue #1120) — the three correlation tests

Full method, why the corpus is untied, and how `Meta.Numerics`' names were resolved:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#53-the-three-correlation-tests-against-metanumerics-issue-1120).
This section carries only the numbers, per `CLAUDE.md`'s "Where a fact belongs" table.

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime — a
dedicated machine, not a container. Window: one `BenchmarkDotNet` 0.14.0 run, default job,
2026-09-23, 4 min 29 s across the 14 benchmarks (7 rows × 2 sizes), no other load. The three
statistics were asserted equal to `1e-9` before anything was timed, and the p-values agreed too:
**0 recorded differences at either size**.

| Method | PairCount | Mean | Allocated |
| --- | ---: | ---: | ---: |
| `Lodestar_Pearson` | 100 | 646.0 ns | — |
| `MetaNumerics_Pearson` | 100 | 516.7 ns | 168 B |
| `Lodestar_Spearman` | 100 | 1.857 μs | 4,144 B |
| `MetaNumerics_Spearman` | 100 | 2.177 μs | 2,024 B |
| `Lodestar_KendallTau` | 100 | 3.184 μs | — |
| `MetaNumerics_KendallTau` | 100 | 6.349 μs | 152 B |
| `Lodestar_Pearson` | 10,000 | 57.88 μs | — |
| `MetaNumerics_Pearson` | 10,000 | 37.27 μs | 168 B |
| `Lodestar_Spearman` | 10,000 | 947.4 μs | 400,145 B |
| `MetaNumerics_Spearman` | 10,000 | 1.288 ms | 160,427 B |
| `Lodestar_KendallTau` | 10,000 | 1.369 ms | — |
| `MetaNumerics_KendallTau` | 10,000 | 173.2 ms | — |

**[`KendallTau.Test`](../../docs/reference/stats/tests/kendalltau-test.md) is the headline: 2.0× ahead at
100 pairs and 126× at 10,000, allocating nothing at either size.** That is not a constant factor.
The definition counts concordant and discordant pairs in a double loop; this orders the pairs by
the first sample and counts the inversions of the second with a merge sort, so the work grows as
`n log n` where `Meta.Numerics` grows as `n²` — at ten thousand pairs, a hundred and thirty
thousand comparisons against fifty million. The gap therefore widens with every further order of
magnitude rather than closing.

[`Spearman.Test`](../../docs/reference/stats/tests/spearman-test.md) is 1.17× ahead at 100 and 1.36× at
10,000, while allocating 2.0× and 2.5× more: it materialises both rank arrays where
`Meta.Numerics` works off one sorted copy.

**[`Pearson.Test`](../../docs/reference/stats/tests/pearson-test.md) is the one row behind — 1.25× at 100
pairs and 1.55× at 10,000 — and the reason is a deliberate trade rather than an oversight.** The
coefficient here is computed as `scipy.stats.pearsonr` computes it: centre each sample, scale by
its largest deviation, normalise each vector by its own norm, then take the dot product. That is
four passes over the data and two divisions per element. `Meta.Numerics` takes one pass over the
raw moments — `Σx`, `Σx²`, `Σxy` — which is faster and loses significance to cancellation when the
values are large relative to their spread. The four-pass form is what holds `1e-9` against the
frozen corpus into a p-value below `1e-15`, and what lands a perfect relationship on exactly `1`
rather than on `0.9999999999999998`, which would turn an exact-zero p-value into `8.9e-16`.
Vectorising the final dot product would recover about a third of the gap and change the summation
order, so it would change that exact `1`; that is a decision about what this package promises
([decision 0005](../../docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)),
not an optimisation, and it has not been taken. This package allocates nothing here where
`Meta.Numerics` allocates 168 B per call.

## Lodestar.Stats against Accord.Statistics (issue #442)

Full method, correctness cross-check, and how `Accord`'s 2017-era API names were resolved against
the restored package:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#18-lodestarstats-against-accordstatistics-issue-442).
This section carries only the numbers, per the rule for where a fact belongs
(`CLAUDE.md`'s "Where a fact belongs" table).

Machine: Intel Xeon Processor 2.80GHz, 1 CPU, 4 logical and 4 physical cores (BenchmarkDotNet's own
header), Ubuntu 24.04.4 LTS, .NET SDK 10.0.111, .NET 10.0.11 runtime — a hosted session container,
not a dedicated benchmark machine, so **this row is indicative, not authoritative**, the same
caveat every other container row in this document carries;
`docs/guides/performance.md` records a
case where a container read a full 3× slower than the dedicated machine on the same code, so treat
the ratios below as directional rather than exact. Window: one `BenchmarkDotNet` run, `ShortRun`
job — fewer iterations than the default, exact parameters in `bench/README.md` — 2026-09-05, no
other load on the container during the run; total run time 1 min 51 s across the 12 benchmarks
(6 pairs × 2 sample sizes). The short job matters for reading the table: three iterations is enough
to see which side is faster by an order of magnitude, as every row below is, and not enough to
trust the last digit of a ratio.

| Method | SampleSize | Mean | Allocated |
| --- | ---: | ---: | ---: |
| `LodestarWelchT` | 100 | 1.322 μs | — |
| `AccordWelchT` | 100 | 40.10 μs | 392 B |
| `LodestarMannWhitney` | 100 | 11.52 μs | 8,944 B |
| `AccordMannWhitney` | 100 | 58.71 μs | 23,336 B |
| `LodestarChiSquare` | 100 | 380.0 ns | 200 B |
| `AccordChiSquare` | 100 | 293.6 ns | 168 B |
| `LodestarWelchT` | 10,000 | 52.21 μs | — |
| `AccordWelchT` | 10,000 | 166.6 μs | 392 B |
| `LodestarMannWhitney` | 10,000 | 5.078 ms | 880,312 B |
| `AccordMannWhitney` | 10,000 | 14.67 ms | 2,241,217 B |
| `LodestarChiSquare` | 10,000 | 379.7 ns | 200 B |
| `AccordChiSquare` | 10,000 | 295.4 ns | 168 B |

`Lodestar.Stats` is faster on
[`TTest.Independent`](../../docs/reference/stats/tests/ttest-independent.md) (30× at 100 samples, narrowing
to 3.2× at 10,000, as `Accord`'s fixed per-call overhead is amortised over more work) and on
[`MannWhitney.Test`](../../docs/reference/stats/tests/mannwhitney-test.md) (5.1× at 100, 2.9× at 10,000,
allocating 61-62% less at both sizes — both sides take the guarded asymptotic path at 10,000, past
`MannWhitney`'s own `20_000`-product exact-method bound). Those two `MannWhitney` rows predate
[#711](https://github.com/CyrilB1531/lodestar/issues/711), after which the ranking no longer
allocates the pooled arrays; that allocation column measures a path this package no longer ships
and has not been re-measured against `Accord`. `Accord` is faster on
[`ChiSquare.Contingency`](../../docs/reference/stats/tests/chisquare-contingency.md) (roughly 380 ns against 294 ns, flat with
sample size since a 2×2 table has four cells regardless of how many observations produced it) — the
one family where this package's richer result (`Chi2ContingencyResult` carries the expected-value
table; `Accord`'s `ChiSquareTest` does not expose one) costs more than it buys at this shape.

**The chi-square row has since reversed, on a different machine.** The tail reads an integer or
half-integer degree of freedom up to 100 as a finite sum rather than iterating the incomplete
gamma's continued fraction, and
[`ChiSquare.Contingency`](../../docs/reference/stats/tests/chisquare-contingency.md) reads **66.52 ns** at 100 samples and
**63.42 ns** at 10,000 against `Accord`'s 121.7 ns at both, at 168 B on both sides — AMD Ryzen 7
8700G, Ubuntu 26.04.1 LTS, .NET 10.0.12, `BenchmarkDotNet` 0.14.0 default job, 2026-09-13,
`StatsBenchmarks` and `DistributionTailBenchmarks`. Correctness against `scipy` is unchanged or
tighter: 60,001 `erfc` points over `[-6, 27.5]` agree to a relative 7e-15, and 22,500 `chi2.sf`
points at 1 to 250 degrees of freedom to 1.7e-13. The two machines are not comparable cell by
cell; what is comparable is each run's own pair.

**Correctness, not just speed.** All three families were checked against `scipy` on frozen
`tests/oracles/stats_*.json` corpus cases through both implementations; no case disagreed beyond
floating-point noise (the last one or two digits of a `double`, inside the `1e-9` tolerance
`docs/equivalence.md` already uses). `bench/README.md` has the three cases and the exact figures.

## Meta.Numerics against Lodestar.Stats and PrincipalComponentVariance (issue #756)

Full method, and the six p-values that differ with their causes:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#50-metanumerics-against-lodestarstats-and-principalcomponentvariance-issue-756).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, default job, one run.
`Meta.Numerics` 4.2.0, MS-PL, `netstandard2.0`.

**Eight test families.** Ratio above 1 means this package is faster; every statistic was checked to
agree before anything was timed.

| family | n = 100 | n = 10,000 | allocated, Lodestar / Meta.Numerics (n = 10,000) |
| --- | ---: | ---: | ---: |
| Student t, pooled | **2.88** | **4.81** | 0 B / 104 B |
| Mann-Whitney | **1.47** | **5.25** | 0 B / 80,379 B |
| Kruskal-Wallis | **1.32** | **3.44** | 33 B / 120,702 B |
| Kolmogorov-Smirnov | **1.47** | **1.48** | 160,051 B / 80,371 B |
| One-way ANOVA | **3.46** | **3.21** | 32 B / 744 B |
| Wilcoxon signed rank | 0.95 | **1.18** | 80,056 B / 80,176 B |
| Fisher exact | **3.49** | **3.45** | **0 B** / 944 B |
| χ² contingency | **5.95** | **5.94** | 168 B / 968 B |

The Kolmogorov-Smirnov row at n = 10,000 was re-measured on 2026-09-16 after #802 made `Auto` exact
there; it read 1.16 when `Auto` was asymptotic. The other rows are the original run's.

**Two rows were costs in this package rather than differences in what the two libraries compute,
and this comparison is what found them.** Both are now ahead of Meta.Numerics:

| row | Lodestar | against Meta.Numerics |
| --- | ---: | --- |
| [`FisherExact.Test`](../../docs/reference/stats/tests/fisherexact-test.md), 2×2 table | **256 ns** | **3.49×** |
| [`KolmogorovSmirnov.TwoSample`](../../docs/reference/stats/tests/kolmogorovsmirnov-twosample.md), n = m = 100 | **1,544 ns** | **1.47×** |

- **Fisher** walked the hypergeometric probabilities through **nine log-gammas per candidate table**
  — three per binomial coefficient — with the denominator recomputed every iteration. Neighbouring
  probabilities differ by a ratio of four small integers, so the range now costs one exponential and
  O(range) multiplications, anchored at the mode. Still **zero allocation**.
- **Kolmogorov-Smirnov** built an `(n+1)×(m+1)` table even when the samples are the same size, where
  `D` is always a whole number of steps of `1/n` and Hodges' exceedance probability is a closed form:
  O(n) multiplications against O(n·m) cells and n+1 row allocations. Allocation fell from 86,512 B
  to 1,610 B.

**Neither is an approximation, and no default moved.** `ExactMethod.Auto` chooses exactly what it
chose before; the exact branch is simply cheaper. The corpus gained an equal-size pair of 100 and a
100-against-99 pair — the second of which must take the table rather than the closed form — and
both replay `scipy` 1.18.1 at 1e-9. **This package still returns the exact p-value where
Meta.Numerics returns an asymptotic one, and is now faster doing it.**

**The explained variance.** Ratio above 1 means this package is faster.

| shape | [`PrincipalComponentVariance.Compute`](../../docs/reference/decomposition/factorization/principalcomponentvariance-compute.md) | Meta.Numerics `PrincipalComponentAnalysis` | ratio | allocated |
| --- | ---: | ---: | ---: | ---: |
| 200 × 10 | **15.42 μs** | 565.95 μs | **36.70** | 2.38 KB / 329.71 KB |
| 2,000 × 10 | **86.25 μs** | 69,772.02 μs | **808.97** | 2.38 KB / **31,414.98 KB** |
| 2,000 × 50 | **1,953.85 μs** | 337,025.46 μs | **172.50** | 42.07 KB / 32,054.48 KB |

Both report the same first component's variance fraction, checked to `1e-9` relative before either
was timed. **Meta.Numerics refuses the fourth shape**: 100 rows by 200 features raises
`InsufficientDataException`, where this package and NumFlat both answer — so the wide matrix has no
Meta.Numerics row at all rather than a slow one.
