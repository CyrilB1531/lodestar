# Performance — Lodestar.Fuzzy

What `Lodestar.Fuzzy` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## [`Levenshtein.Distance`](../../docs/reference/text/distances/levenshtein-distance.md) against Fastenshtein 1.0.12, Quickenshtein 1.5.1 and F23.StringSimilarity 7.0.1

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15.

| Length | Lodestar | Fastenshtein | Quickenshtein | F23.StringSimilarity |
| ---: | ---: | ---: | ---: | ---: |
| 8 | **15.71 ns**, 0 B | 46.11 ns, 56 B | 48.27 ns, 0 B | 103.42 ns, 128 B |
| 64 | **178.57 ns**, 0 B | 3,339.85 ns, 280 B | 727.63 ns, 0 B | 5,443.03 ns, 576 B |
| 512 | **7,097.59 ns**, 0 B | 239,725.19 ns, 2,072 B | 19,810.70 ns, 0 B | 452,681.32 ns, 4,160 B |

**Ahead of all three at every length**: 2.8× to 4.1× against Quickenshtein, the closest, and 2.9× to
63.8× against the other two. Neither Lodestar nor Quickenshtein allocates.

## `Fuzz` against Raffinert.FuzzySharp 6.0.0

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15.

| Operation | Lodestar | FuzzySharp | Faster |
| --- | ---: | ---: | ---: |
| [`Fuzz.Ratio`](../../docs/reference/fuzzy/matching/fuzz-ratio.md) | 62.18 ns, 0 B | 129.69 ns, 80 B | Lodestar, 2.09× |
| [`Fuzz.PartialRatio`](../../docs/reference/fuzzy/matching/fuzz-partialratio.md) | 452.36 ns, 0 B | 5,767.01 ns, 160 B | Lodestar, 12.75× |
| [`Fuzz.TokenSetRatio`](../../docs/reference/fuzzy/matching/fuzz-tokensetratio.md) | 653.21 ns, 1,448 B | 1,159.41 ns, 1,944 B | Lodestar, 1.78× |
| [`Fuzz.WRatio`](../../docs/reference/fuzzy/matching/fuzz-wratio.md) | 1,346.20 ns, 2,760 B | 2,938.48 ns, 3,128 B | Lodestar, 2.18× |

**Ahead on all four**, and allocating less on each.

## The score matrix against FuzzySharp and `rapidfuzz` (issue #1123)

Full method and what each row does and does not compare:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#56-the-score-matrix-against-fuzzysharp-and-rapidfuzz-issue-1123).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `BenchmarkDotNet` 0.14.0, default job.
Phrases of three words and an index, so no pair scores 100 by accident.

**The .NET incumbent publishes no matrix call at all**, checked by reflection:
`Raffinert.FuzzySharp.Process` exports `ExtractAll`, `ExtractTop`, `ExtractSorted` and
`ExtractOne`, and nothing that takes two collections. Its row is therefore `ExtractAll` once per
query — the loop that library forces.

| size | operation | Lodestar | FuzzySharp 6.0.0 | FuzzySharp / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 50 × 50 | score matrix, `Ratio` | **89.86 μs** | 255.91 μs | **2.85** |
| 200 × 200 | score matrix, `Ratio` | **1,974.46 μs** | 4,459.54 μs | **2.26** |

**Allocation is the wider margin**: 19.55 KB against 301.95 KB at 50 a side, and 312.62 KB against
4,723.45 KB at 200 — **15×**, because the matrix is one array where a per-query `ExtractAll`
materialises a result object per pair.

[`Process.Cdist`](../../docs/reference/fuzzy/matching/process-cdist.md) costs **exactly** what the same
double loop written by hand against [`Fuzz.Ratio`](../../docs/reference/fuzzy/matching/fuzz-ratio.md)
costs — 1.00× and 1.01× at the two sizes, byte for byte the same allocation. It is worth saying
plainly: this member buys the shape, the cutoff applied where the score is produced, and one
allocation instead of a list per query. It does not buy speed over what a caller could already
write.

Against `rapidfuzz` 3.14.6 through `compare-cdist`, one run of each side, milliseconds per
operation, best of five, `dtype=np.float64` and `workers=1` on the Python side so both compute the
same thing:

| n a side | scorer | Lodestar | `rapidfuzz` | ratio |
| ---: | --- | ---: | ---: | ---: |
| 50 | `Ratio` | 0.088 ms | **0.018 ms** | 0.21 |
| 50 | `WRatio` | **1.125 ms** | 1.148 ms | **1.02** |
| 200 | `Ratio` | 1.575 ms | **0.229 ms** | 0.15 |
| 200 | `WRatio` | **17.319 ms** | 17.658 ms | **1.02** |
| 500 | `Ratio` | 9.393 ms | **1.383 ms** | 0.15 |
| 500 | `WRatio` | **104.467 ms** | 107.631 ms | **1.03** |

**The gap is the bulk shape, and the two scorers prove it.** `cdist` builds each query's
bit-parallel equality table once and scans every choice against it, where
[`Fuzz.Ratio`](../../docs/reference/fuzzy/matching/fuzz-ratio.md) rebuilds it per pair. On `Ratio` that fixed cost is most of what a cell costs and the reference is ~7× ahead;
on `WRatio`, which inspects its input and computes several sub-ratios, the same fixed cost is
amortised and **the two are level**. Two rows of the same two libraries on the same corpus say
more than any cross-language loop could.
[Issue #1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries the hoisting, which
needs new `Lodestar.Text` API and the release order that comes with it.

**A cold probe nearly went into this document.** One unwarmed `Stopwatch` pass over 200 × 200 read
25.5 ms against the reference's 0.5 — 51×, all of it JIT. The harness and BenchmarkDotNet agree
with each other at 1.58 ms and 1.97 ms and not with it.

## The score matrix under a cutoff, against `rapidfuzz` (issue #1134)

Full method, the corpus rule and what the cutoff does to each side:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#58-the-cutoff-that-skips-a-pair-rather-than-reporting-it-issue-1134).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `compare-cdist`, one run of each side,
milliseconds per operation, best of five, `dtype=np.float64` and `workers=1` so both compute the
same thing. Phrases of one to six words and an index, which is what gives a length bound anything
to reject — the three-word corpus above holds every phrase to within a word of every other.

| n a side | cutoff | Lodestar | `rapidfuzz` 3.14.6 | ratio |
| ---: | ---: | ---: | ---: | ---: |
| 50 | none | 0.080 ms | **0.015 ms** | 0.19 |
| 50 | 90 | 0.033 ms | **0.015 ms** | **0.47** |
| 200 | none | 1.460 ms | **0.185 ms** | 0.13 |
| 200 | 90 | 0.624 ms | **0.185 ms** | **0.30** |
| 500 | none | 8.335 ms | **1.096 ms** | 0.13 |
| 500 | 90 | 3.416 ms | **1.098 ms** | **0.32** |

**A cutoff of 90 buys this package 2.3× to 2.4× and buys the reference nothing.** `rapidfuzz`
reads 0.185 ms against 0.185 and 1.096 against 1.098 with the cutoff and without it — inside its
own noise. The Indel distance is at least the difference in lengths, so a pair whose lengths alone
miss the cutoff cannot reach it; that rejection is what this package now takes, and the deficit
against the reference halves — 0.13× becomes 0.30× at 200 a side and 0.32× at 500.

**It does not close the gap, and the reason is the one section #1123 already named.** What remains
is the per-pair equality table [`Fuzz.Ratio`](../../docs/reference/fuzzy/matching/fuzz-ratio.md) rebuilds
where `cdist` builds it once per query, which
no cutoff can remove — only the hoisting
[#1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries, and the `Lodestar.Text`
release order it needs.

**The bound holds for the Indel ratio alone**, so the rows above are the default scorer and there
is no `WRatio` row to put beside them: `partial_ratio("cat", "the cat sat on the mat")` is 100
against a ceiling of 24, and a scorer passed by the caller has every pair scored.
