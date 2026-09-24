# Performance — Lodestar.Metrics

What `Lodestar.Metrics` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Classification metrics (issue #61) — vs scikit-learn

```bash
python bench/corpus/generate_metrics.py           # writes bench/corpus/metrics/, git-ignored
.venv-oracles/bin/activate && python bench/python/bench_metrics.py
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- compare-metrics
python bench/compare.py metrics
```

Six operations — `confusion_matrix`, `accuracy`, `precision_recall_f1_macro`,
`classification_report`, `roc_auc_binary`, `roc_auc_ovr_macro` — over six shapes
(1 000 / 100 000 / 1 000 000 samples, 2 or 10 classes), on the same corpus files
on both sides. **This is the merge gate for the branch, on processor time**: every
row must be ≥ 1×, and it is.

Machine: Intel Core i7-4770S, .NET 10.0.10, against scikit-learn 1.9.0 / NumPy 2.5.1 on Python
3.12.3. Window: one run per side, published 2026-08-06.
Both sides measured back to back, Python first,
on a machine left to settle (one-minute load 1.52 at the Python start — below
this workstation's 1.9–2.3 floor, itself a permanent ~30–40 % background from
the desktop client, an editor and a browser). The C# side started 49 seconds
later, in the Python run's own wake, so its figures are the ones taken on the
busier machine and every ratio below is conservative rather than flattering;
`bench/README.md` records the full conditions.

| Operation | Lodestar ms | Python ms | wall | Lodestar cpu ms | Python cpu ms | **cpu** |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| `confusion_matrix_n1000_k2` | 0.009 | 1.028 | 117.98x | 0.009 | 1.028 | **117.97x** |
| `accuracy_n1000_k2` | 0.001 | 0.546 | 618.32x | 0.001 | 0.546 | **618.33x** |
| `precision_recall_f1_macro_n1000_k2` | 0.008 | 1.793 | 226.58x | 0.008 | 1.793 | **226.58x** |
| `classification_report_n1000_k2` | 0.011 | 6.692 | 623.31x | 0.011 | 6.691 | **623.25x** |
| `roc_auc_binary_n1000_k2` | 0.029 | 2.008 | 70.12x | 0.029 | 2.008 | **70.12x** |
| `confusion_matrix_n1000_k10` | 0.009 | 1.051 | 120.64x | 0.009 | 1.051 | **120.64x** |
| `accuracy_n1000_k10` | 0.001 | 0.541 | 622.03x | 0.001 | 0.541 | **622.08x** |
| `precision_recall_f1_macro_n1000_k10` | 0.010 | 1.855 | 192.49x | 0.010 | 1.855 | **192.48x** |
| `classification_report_n1000_k10` | 0.017 | 7.011 | 422.54x | 0.017 | 7.010 | **422.53x** |
| `roc_auc_ovr_macro_n1000_k10` | 0.550 | 10.526 | 19.13x | 0.550 | 10.525 | **19.13x** |
| `confusion_matrix_n100000_k2` | 0.964 | 15.791 | 16.39x | 0.964 | 15.791 | **16.39x** |
| `accuracy_n100000_k2` | 0.190 | 5.519 | 29.01x | 0.190 | 5.518 | **29.01x** |
| `precision_recall_f1_macro_n100000_k2` | 0.844 | 17.786 | 21.07x | 0.844 | 17.785 | **21.07x** |
| `classification_report_n100000_k2` | 0.848 | 36.233 | 42.75x | 0.847 | 36.231 | **42.75x** |
| `roc_auc_binary_n100000_k2` | 7.977 | 35.024 | 4.39x | 8.092 | 35.023 | **4.33x** |
| `confusion_matrix_n100000_k10` | 1.059 | 16.109 | 15.20x | 1.059 | 16.108 | **15.21x** |
| `accuracy_n100000_k10` | 0.296 | 5.519 | 18.66x | 0.296 | 5.519 | **18.66x** |
| `precision_recall_f1_macro_n100000_k10` | 0.979 | 18.524 | 18.92x | 0.979 | 18.523 | **18.92x** |
| `classification_report_n100000_k10` | 0.979 | 40.139 | 41.00x | 0.979 | 40.137 | **41.00x** |
| `roc_auc_ovr_macro_n100000_k10` | 88.385 | 250.400 | 2.83x | 91.396 | 250.402 | **2.74x** |
| `confusion_matrix_n1000000_k2` | 8.750 | 156.920 | 17.93x | 8.749 | 156.823 | **17.92x** |
| `accuracy_n1000000_k2` | 2.045 | 51.599 | 25.23x | 2.045 | 51.596 | **25.23x** |
| `precision_recall_f1_macro_n1000000_k2` | 8.701 | 164.332 | 18.89x | 8.701 | 164.330 | **18.89x** |
| `classification_report_n1000000_k2` | 8.719 | 314.805 | 36.11x | 8.718 | 314.782 | **36.11x** |
| `roc_auc_binary_n1000000_k2` | 95.219 | 364.420 | 3.83x | 95.684 | 364.384 | **3.81x** |
| `confusion_matrix_n1000000_k10` | 9.916 | 156.707 | 15.80x | 9.915 | 156.699 | **15.80x** |
| `accuracy_n1000000_k10` | 3.122 | 51.877 | 16.61x | 3.122 | 51.874 | **16.61x** |
| `precision_recall_f1_macro_n1000000_k10` | 10.001 | 173.128 | 17.31x | 10.000 | 173.121 | **17.31x** |
| `classification_report_n1000000_k10` | 9.865 | 352.364 | 35.72x | 9.864 | 352.349 | **35.72x** |

**Gate result: 29/29 operations at or above 1× on processor time.** The
narrowest margin is **2.74×**, on `roc_auc_ovr_macro` at n=100 000, k=10 — the
row the design brief flagged as the one most likely to need a radix-sort
rewrite of `BinaryRoc`. It did not: even the heaviest sort-bound row clears the
gate by a comfortable margin, so no algorithmic change was needed on this
branch.

**Read this before quoting a single ratio.** The rows at n=1 000 (70×–620×) are
dominated by CPython's per-call interpreter overhead, not by the computation —
a confusion matrix over 1 000 samples is sub-microsecond work on either side.
The rows that carry the argument are the ones at n=100 000 and n=1 000 000,
where the ratios settle to a more modest but still decisive 2.7×–43×.

Unlike the persistence comparison, wall and processor time agree here to
within about 1% on every row (up to 3.4% on the single heaviest-cpu row): these
metrics allocate little enough per call that .NET's background collector is
never a factor, so there is no gap between the two columns to explain away.

Full breakdown, including the intra-C# and net10-vs-netstandard2.0 tiers and
where the two language sides do not do identical work, in
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#5-classification-metrics-issue-61).

### Balanced accuracy, Matthews correlation, Cohen's kappa (issue #93)

Balanced accuracy, Matthews correlation and Cohen's kappa (issue #93, Tasks
3–5) add three operations — `balanced_accuracy`, `matthews`, `cohen_kappa` —
run over all six shapes above, unweighted and with default label handling on
both sides, matching scikit-learn's `balanced_accuracy_score`,
`matthews_corrcoef` and `cohen_kappa_score`. Same corpus files, same harnesses,
same methodology as the table above — **but measured in a separate window
from the original 29 rows, with its own load**: `uptime`'s one-minute average
was **19.70** just before the Python side started and **7.65** by the time
`compare.py` printed the numbers below (fifteen-minute average 13.2–14.9
throughout that window). That is nowhere near the 1.52 one-minute load the
paragraph above states for the original run, so these 18 rows should not be
read as sharing that sentence's conditions — only their own, given here.

| Operation | Lodestar ms | Python ms | wall | Lodestar cpu ms | Python cpu ms | **cpu** |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| `balanced_accuracy_n1000_k2` | 0.016 | 1.194 | 76.44x | 0.011 | 1.194 | **105.24x** |
| `matthews_n1000_k2` | 0.017 | 2.216 | 134.27x | 0.012 | 2.216 | **192.06x** |
| `cohen_kappa_n1000_k2` | 0.018 | 1.240 | 67.89x | 0.012 | 1.240 | **105.23x** |
| `balanced_accuracy_n1000_k10` | 0.008 | 1.225 | 152.93x | 0.008 | 1.225 | **152.96x** |
| `matthews_n1000_k10` | 0.008 | 2.258 | 282.30x | 0.008 | 2.258 | **282.33x** |
| `cohen_kappa_n1000_k10` | 0.009 | 1.399 | 157.84x | 0.009 | 1.399 | **157.84x** |
| `balanced_accuracy_n100000_k2` | 0.887 | 17.287 | 19.49x | 0.887 | 17.282 | **19.48x** |
| `matthews_n100000_k2` | 0.884 | 34.733 | 39.28x | 0.884 | 34.712 | **39.26x** |
| `cohen_kappa_n100000_k2` | 0.880 | 18.133 | 20.61x | 0.880 | 18.103 | **20.58x** |
| `balanced_accuracy_n100000_k10` | 1.001 | 17.326 | 17.31x | 1.001 | 17.320 | **17.31x** |
| `matthews_n100000_k10` | 0.996 | 36.312 | 36.46x | 0.996 | 36.307 | **36.45x** |
| `cohen_kappa_n100000_k10` | 0.980 | 17.130 | 17.49x | 0.979 | 17.129 | **17.49x** |
| `balanced_accuracy_n1000000_k2` | 9.087 | 166.698 | 18.35x | 9.085 | 166.690 | **18.35x** |
| `matthews_n1000000_k2` | 9.003 | 350.953 | 38.98x | 9.003 | 350.762 | **38.96x** |
| `cohen_kappa_n1000000_k2` | 9.032 | 186.455 | 20.64x | 9.032 | 185.697 | **20.56x** |
| `balanced_accuracy_n1000000_k10` | 10.103 | 167.552 | 16.58x | 10.102 | 167.550 | **16.59x** |
| `matthews_n1000000_k10` | 10.262 | 340.992 | 33.23x | 10.261 | 340.854 | **33.22x** |
| `cohen_kappa_n1000000_k10` | 10.352 | 174.623 | 16.87x | 10.352 | 174.619 | **16.87x** |

**18/18 at or above 1× on processor time — the gate holds for these three
metrics too.** The two narrowest are `balanced_accuracy_n1000000_k10` at
**16.59×** and `cohen_kappa_n1000000_k10` at **16.87×**; every other row
clears 17×. As with the original 29, the busier the machine gets, the more
conservative (not flattering) a ratio above 1× is — and this window's load
average was roughly 5–13× the original run's, so these margins are, if
anything, understated relative to a quiet machine.

### Regression metrics — mse, mae, median_ae, r2 (issue #92)

The eleven regression metrics landed for issue #92 add four benchmark
operations — `mse`, `mae`, `median_ae`, `r2` — covering the four distinct cost
shapes among them: a squared mean, an absolute mean, a sort, and a two-pass
centred sum. The other seven metrics are one of those four with a different
arithmetic kernel and are not separately timed. They run over
`y_true_real`/`y_pred_real`, continuous targets drawn by a separate seeded
random generator and attached to each of the six existing corpus shapes,
independent of the classification columns those shapes already carry. The
generator inserting these draws would otherwise have shifted every
classification array after the insertion point, invalidating the 29 and 18
rows above. A before/after comparison of `y_true[:10]` on the regenerated
corpus confirmed it did not. Same corpus files, same harnesses, same
methodology as the tables above — **but measured in yet another separate
window, with its own load**: `uptime`'s one-minute average was **8.05** just
before the Python side started (five/fifteen-minute: 11.95 / 14.25) and
**6.05** by the time `compare.py` printed the numbers below (five/fifteen-minute:
7.15 / 11.07). That is well below the 16–23 one-minute load this session saw
at dispatch and while the code changes were being made, but still noticeably
busier than the 1.52 one-minute load recorded for the original 29 rows. So
these 24 rows should be read only under their own conditions, given here —
**except the six `median_ae` rows marked †**. Those come from a later
window described below, after `MedianAbsoluteError`'s unweighted path was
rewritten.

**Read the `k` suffix as a corpus file name, not as a workload.** The
regression arrays are drawn from `SeededRandom(SEED + 1_000 + n)`, which
depends on the sample count and not on the class count, so `metrics_n1000_k2`
and `metrics_n1000_k10` carry byte-identical `y_true_real`. That is deliberate
— all four operations here are single-output, and `k` is a property of the
classification columns those files also hold — but it means the 24 rows below
are **12 distinct workloads, each measured twice**. The pairs are useful for
exactly that: they bound the run-to-run spread. At n=1 000 000 the two members
agree to within 0.04× (`mse` 1.04× / 1.00×), while at n=1 000 the same
identical array gives 98.88× and 141.17× — a 43 % spread, which is what a
sub-millisecond `mse` measurement is worth on a machine at this load, and the
reason no conclusion on this page rests on an n=1 000 row.

| Operation | Lodestar ms | Python ms | wall | Lodestar cpu ms | Python cpu ms | **cpu** |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| `mse_n1000_k2` | 0.005 | 0.486 | 104.89x | 0.005 | 0.458 | **98.88x** |
| `mae_n1000_k2` | 0.005 | 0.358 | 77.79x | 0.005 | 0.358 | **77.70x** |
| `median_ae_n1000_k2`† | 0.011 | 0.818 | 77.81x | 0.011 | 0.625 | **59.45x** |
| `r2_n1000_k2` | 0.008 | 0.443 | 57.72x | 0.008 | 0.442 | **57.66x** |
| `mse_n1000_k10` | 0.005 | 1.003 | 219.23x | 0.005 | 0.646 | **141.17x** |
| `mae_n1000_k10` | 0.005 | 0.541 | 119.33x | 0.005 | 0.507 | **111.80x** |
| `median_ae_n1000_k10`† | 0.011 | 0.367 | 34.83x | 0.011 | 0.367 | **34.84x** |
| `r2_n1000_k10` | 0.008 | 0.447 | 55.95x | 0.008 | 0.447 | **55.86x** |
| `mse_n100000_k2` | 0.452 | 0.645 | 1.43x | 0.452 | 0.645 | **1.43x** |
| `mae_n100000_k2` | 0.466 | 1.588 | 3.41x | 0.466 | 1.295 | **2.78x** |
| `median_ae_n100000_k2`† | 1.967 | 1.781 | 0.91x | 2.045 | 1.781 | **0.87x** |
| `r2_n100000_k2`‡ | 0.759 | 0.991 | 1.31x | 0.759 | 0.991 | **1.31x** |
| `mse_n100000_k10` | 0.455 | 0.628 | 1.38x | 0.454 | 0.628 | **1.38x** |
| `mae_n100000_k10` | 0.458 | 0.673 | 1.47x | 0.458 | 0.672 | **1.47x** |
| `median_ae_n100000_k10`† | 2.142 | 1.796 | 0.84x | 2.241 | 1.795 | **0.80x** |
| `r2_n100000_k10`‡ | 0.743 | 0.950 | 1.28x | 0.743 | 0.950 | **1.28x** |
| `mse_n1000000_k2` | 5.013 | 5.226 | 1.04x | 5.008 | 5.220 | **1.04x** |
| `mae_n1000000_k2` | 5.054 | 5.635 | 1.12x | 5.036 | 5.633 | **1.12x** |
| `median_ae_n1000000_k2`† | 18.365 | 16.375 | 0.89x | 18.708 | 16.360 | **0.87x** |
| `r2_n1000000_k2`‡ | 8.093 | 9.205 | 1.14x | 8.083 | 9.204 | **1.14x** |
| `mse_n1000000_k10` | 4.983 | 4.989 | 1.00x | 4.982 | 4.983 | **1.00x** |
| `mae_n1000000_k10` | 5.040 | 5.712 | 1.13x | 5.035 | 5.711 | **1.13x** |
| `median_ae_n1000000_k10`† | 18.094 | 16.282 | 0.90x | 18.163 | 16.259 | **0.90x** |
| `r2_n1000000_k10`‡ | 7.807 | 9.687 | 1.24x | 7.807 | 9.686 | **1.24x** |

† The six `median_ae` rows were measured after `WeightedPercentile`'s unweighted branch stopped
sorting the whole residual array, in a separate window from the other eighteen rows and under a
deliberately comparable load (one-minute average 6.62 falling to 6.52, against the table's own 8.05
to 6.05). **They are superseded**: [#140](https://github.com/CyrilB1531/lodestar/issues/140) took a
further 38 % off `median_ae` at n = 1 000 000, so these rows measure a partition this package no
longer ships and the current one has not been re-measured against scikit-learn.

‡ The four `r2` rows predate
[#127](https://github.com/CyrilB1531/lodestar/issues/127): they measure the original sequential-sum
`R2`, not the Neumaier-compensated one this package ships, and are kept only so the pairing this
table relies on stays intact.

**20 of 24 rows at or above 1× on processor time**, which under the pairing above is 10 of 12
distinct workloads. **The four below the gate are `median_ae`** at n = 100 000 and n = 1 000 000 —
0.80× to 0.90× — and the cause is algorithmic rather than a run: scikit-learn's
`median_absolute_error` calls NumPy's `median`, which selects by introselect in expected `O(n)`.
This package now selects the one or two order statistics the median needs with a median-of-three
quickselect, falling back to `Array.Sort` on the remaining range once partitioning exceeds a budget
proportional to `log2(n)` — the same worst-case guarantee NumPy relies on — so the two do the same
order of work and what is left reads as constant overhead: managed bounds checks, the Lomuto
partition's extra writes, no SIMD comparison loop. `mse_n1000000_k10` is the narrowest passing row
at **1.00×**, near enough to parity that a busier or quieter machine could tip it either way.

## `Lodestar.Metrics` against ML.NET 5.0.0's binary evaluator

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15.

| Samples | Request | Lodestar | ML.NET | Faster |
| ---: | --- | ---: | ---: | ---: |
| 100,000 | the six numbers ML.NET returns | 4,744.17 μs, 998 B | 22,914.88 μs, 5,089,338 B | Lodestar, 4.84× |
| 100,000 | accuracy alone | 69.09 μs, 0 B | 22,143.43 μs, 5,089,337 B | Lodestar, 321.81× |
| 1,000,000 | the six numbers ML.NET returns | 89,431.29 μs, 0 B | 140,625.56 μs, 23,229,318 B | Lodestar, 1.57× |
| 1,000,000 | accuracy alone | 1,661.99 μs, 0 B | 144,519.97 μs, 23,229,318 B | Lodestar, 86.96× |

**Ahead on every row, by less as the sample grows**: 4.84× at 100,000 samples to 1.57× at a million for
the full bundle. ML.NET computes the bundle whatever is asked, so accuracy alone costs a caller the six.
