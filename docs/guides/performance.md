# Performance

One comparison per capability: this library against the library a reader would otherwise
reach for, with that library's version, the machine the pair ran on and the window the run
took. Both sides are checked to return the same answers before either is timed — the
agreement checks, the corpora and every command are
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md)'s
subject, and this page carries only what came out.

A before/after of this repository's own code is not here. It is an argument about one
commit, and it belongs in the pull request that made it.

## How to read a row

- **A ratio above 1 means this library is faster**, unless the table's own header says
  otherwise; each table states its direction.
- **A named machine is not a shared runner.** Every row below names the processor, the
  operating system and the runtime it ran on. Where a row comes from a hosted runner or a
  session container it says so, and is read as a ratio: such a machine's absolutes are not
  comparable between nights, or with another machine's.
- **A cross-language row is processor time as well as wall time** where the Python side can
  use more than one core, because a wall-clock lead over a multi-threaded reference is a
  different claim from a lead per core.
- **Every baseline here is single-threaded** unless the row says otherwise.

## Lodestar.Text

### [`IndelPattern`](../reference/text/distances/indelpattern.md) against FuzzySharp (issue #1130)

Full method, the corpora that cost the handle and the defect the agreement check found:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#59-one-patterns-table-held-rather-than-rebuilt-issue-1130).
Machine: the AMD Ryzen 7 8700G named below, on 2026-09-24. `BenchmarkDotNet` 0.14.0, default job.
One query against 64 texts; `length` is the part that differs, the shared affixes are on top of it.

| the texts | length | Lodestar, held | Lodestar, pairwise | FuzzySharp 6.0.0 |
| --- | ---: | ---: | ---: | ---: |
| unrelated | 4 | **347.7 ns** | 763.9 ns | 3,104.7 ns |
| unrelated | 16 | **842.4 ns** | 1,811.8 ns | 4,942.5 ns |
| unrelated | 32 | **1,443.0 ns** | 3,318.8 ns | 7,853.3 ns |
| sharing 12 + 12 | 4 | **1,280.7 ns** | 1,484.9 ns | 4,378.9 ns |
| sharing 12 + 12 | 16 | **1,746.5 ns** | 2,472.9 ns | 5,627.5 ns |
| sharing 12 + 12 | 32 | **2,363.3 ns** | 3,978.1 ns | 8,403.8 ns |
| sharing a 45 suffix | 4 | **2,098.5 ns** | 2,289.7 ns | 4,669.3 ns |
| sharing a 45 suffix | 16 | **2,608.4 ns** | 3,342.1 ns | 6,373.8 ns |
| sharing a 45 suffix | 32 | **4,654.0 ns** | 4,834.3 ns | 9,057.9 ns |
| sharing 28 + 28 | 4 | 2,561.2 ns | **2,362.6 ns** | 4,653.3 ns |
| sharing 28 + 28 | 16 | 3,776.9 ns | **3,328.8 ns** | 6,726.6 ns |
| sharing 28 + 28 | 32 | 5,064.0 ns | **4,898.9 ns** | 9,560.6 ns |
| sharing 45 + 45 | 4 | 3,949.0 ns | **3,344.1 ns** | 5,581.9 ns |
| sharing 45 + 45 | 16 | 4,685.6 ns | **4,346.0 ns** | 7,641.9 ns |
| sharing 45 + 45 | 32 | 6,135.0 ns | **5,901.3 ns** | 10,186.3 ns |
| past the table, 70 + 70 | 4 | **4,580.0 ns** | 4,601.9 ns | 7,001.1 ns |
| past the table, 70 + 70 | 16 | **5,504.4 ns** | 5,728.7 ns | 8,989.9 ns |
| past the table, 70 + 70 | 32 | **6,803.1 ns** | 7,181.3 ns | 11,815.0 ns |

**Against the .NET incumbent, over all eighteen rows: the held form is 1.41× to 8.93× and the
pairwise loop 1.52× to 4.06×.** `Raffinert.FuzzySharp` publishes no held form: its scorer takes
two strings, so its row is one call per text, which is the same absence the score matrix records.

**Against this package's own pairwise loop it is 0.43 to 0.96 where the texts share little or
share one end, and 1.03 to 1.18 where they share a long run at both.** Those six rows are the
cost of the design. The table spans the whole pattern, so it cannot drop a common prefix and
suffix; where the pattern spans two words a sixteen-unit probe at either end sends the pair back
to the pairwise call, which is what holds the 45 + 45 rows to 1.04–1.18 against 1.56–2.14
unguarded. Over one word the probe is not taken — scanning the affix costs about what the trim
saves — and the 28 + 28 row at length 4, the one that would say otherwise, reads 1.08.

**Allocation: 40 bytes against the incumbent's 5,120.** The handle is one object and a pooled
table; the incumbent materialises per call. The pairwise loop allocates nothing at all, which is
the one column it wins.

### [`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md) against rapidfuzz

[`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md) against rapidfuzz
3.14.6 on Python 3.12, through the cross-language harness over `bench/corpus/pairs.json` — the same
committed corpus and the same ns/pair methodology on both sides. Method, corpora and which bucket
reaches which kernel:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#the-levenshtein-corpora-and-which-one-reaches-what).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: one harness run per side on 2026-09-14.

| Alphabet, length | rapidfuzz | Lodestar | |
| --- | ---: | ---: | --- |
| Latin, 128 | 994.2 ns | **536.4 ns** | **1.85× C# faster** |
| Latin, 512 | 8,711.5 ns | **7,535.7 ns** | **1.16× C# faster** |
| CJK, 128 | 1,671.8 ns | **1,245.1 ns** | **1.34× C# faster** |
| CJK, 512 | 14,632.2 ns | **11,393.5 ns** | **1.28× C# faster** |

**Ahead on both alphabets at both lengths.** The Latin buckets take Myers' blocked kernel two words
at a time; the CJK buckets leave Latin-1 once their affixes are trimmed — 993 of 1,000 pairs at 128
and all 1,000 at 512 — and take the row-major kernel with the side table that carries symbols above
U+00FF. Buckets of 8 and 64 do not reach the blocked kernel at all; at those lengths rapidfuzz pays
per-call interop overhead and this package is further ahead.

### `Indel` and `fuzz.ratio` against rapidfuzz

`Indel` is `len(a) + len(b) - 2·LCS`, so what runs is
[`Lcs.SubsequenceLength`](../reference/text/distances/lcs-subsequencelength.md) — and that is also
what `fuzz.ratio`, every `process.extract` and every blocking deduplication pass runs. Same corpus,
same harness and the same rapidfuzz 3.14.6 as the table above.

Machine: the AMD Ryzen 7 8700G named above. Window: the harness's `compare-indel`, one run per side on 2026-09-13, the C#
side published once over three rounds; the range below is the three rounds' spread.

| Alphabet, length | rapidfuzz | Lodestar | |
| --- | ---: | ---: | --- |
| Latin, 128 | 258.2 ns | **229.3 – 230.7 ns** | **1.12× C# faster** |
| Latin, 512 | **2,712.8 ns** | 3,085 – 3,103 ns | 1.14× Python faster |
| CJK, 128 | 1,127.8 ns | **776 – 817 ns** | **1.38× C# faster** |
| CJK, 512 | 9,423.5 ns | **5,222 – 5,277 ns** | **1.79× C# faster** |

**Latin 512 is the one row behind**, and what it pays there is the four-word group's stored carry
and the equality table's clear; the CJK rows, which take a different route, are ahead by more the
longer the input gets.

### [`TfidfVectorizer`](../reference/text/vectorizers/tfidfvectorizer.md) against ML.NET 5.0.0's `FeaturizeText`

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15.

| Documents | Lodestar | `FeaturizeText` | Ratio |
| ---: | ---: | ---: | ---: |
| 200 | 5.057 ms, 5.13 MB | 28.875 ms, 28.80 MB | 5.71 |
| 1,000 | 22.294 ms, 24.92 MB | 244.044 ms, 325.26 MB | 10.95 |

**This is not like-for-like**, and section 15 of `bench/README.md` counts the difference.
`FeaturizeText` adds character n-grams, so it produces 70,307 non-zero features at 200 documents
where this package stores 7,996, and 351,217 at 1,000 against 39,974. Per non-zero feature produced,
that is 0.63 μs against 0.41 μs at 200 documents, ML.NET ahead, and 0.56 μs against 0.69 μs at 1,000.
The claim is the sparse representation, not a faster kernel.

### BM25 against LuceneSharp (issue #677)

[`Bm25Index`](../reference/text/search/bm25index.md) against LuceneSharp.Core 26.8.4415's
`BM25Similarity`, by `Bm25Benchmarks`: one query term, the top ten, over a seeded corpus of 500
terms and 40 tokens a document. Method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#21-bm25-against-lucenesharp-issue-573)
section 21.

**Both sides return the same ten documents in the same order**, checked at 1,000 and at 20,000
documents before either was timed. This package's `K1` is 1.5 and Lucene's 1.2; with one term and
documents of equal length the ranking follows the term's frequency alone, whatever `K1` is.

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14 — the query rows from the
run that measured both sides after [#751](https://github.com/CyrilB1531/lodestar/issues/751)
replaced `Top`'s sort with a bounded heap, the text-to-ranking rows from the run before it, which
that change does not touch because the row is the vectorizer's.

| Documents | Row | Lodestar | LuceneSharp | Faster |
| ---: | --- | ---: | ---: | --- |
| 1,000 | query only | **1.223 μs**, 424 B | 1.943 μs | Lodestar, 1.6× |
| 20,000 | query only | 15.166 μs, 424 B | **10.614 μs** | Lucene, 1.4× |
| 1,000 | text to ranking | 9,280.8 μs, 21,362.8 KB | **4,451.2 μs**, 1,346.0 KB | Lucene, 2.1× |
| 20,000 | text to ranking | 228,889 μs ± 23,916, 418,588.8 KB | **86,301 μs**, 21,897.3 KB | Lucene, 2.7× |

**The query splits by size.** What is left at 20,000 documents is the pass over every document's
score, 20,000 comparisons against the heap's root; Lucene never reads a document without the term,
and skips postings blocks whose best score cannot enter its top ten, as its design has it. The
query no longer allocates in proportion to the corpus.

**The text-to-ranking row is the vectorizer's**, not the index's. Each phase, split with a
`Stopwatch` on the same machine and corpus, lowest of several runs:

| Documents | [`CountVectorizer.FitTransform`](../reference/text/vectorizers/countvectorizer-fittransform.md) | `new Bm25Index(counts)` | [`Bm25Index.Score`](../reference/text/search/bm25index-score.md) |
| ---: | ---: | ---: | ---: |
| 1,000 | 9.39 ms, 20.4 MB | 0.360 ms, 0.46 MB | 1.3 μs |
| 20,000 | 175.6 ms, 399.6 MB | 3.233 ms, 8.97 MB | 10.1 μs |

**For a caller who already holds the matrix** — one who has vectorized for something else, TF-IDF
or a classifier — building `Bm25Index` and answering `q` queries costs `0.36 ms + 1.22 μs·q` at
1,000 documents against Lucene's `4.45 ms + 1.94 μs·q`, cheaper at every `q`. At 20,000 documents
it is `3.23 ms + 15.2 μs·q` against `86.3 ms + 10.6 μs·q`, which crosses near 18,000 queries. From
text Lucene stays ahead from the first query. These crossings mix `Stopwatch` build figures with
`BenchmarkDotNet` query figures, so read them as orders of magnitude.

### BK-tree vs a length-filtered scan (issue #526)

**No .NET package publishes a BK-tree**, so the alternative is the loop a caller would otherwise
write: `BkTreeBenchmarks.LengthFilteredScan`, a linear scan that skips any word whose length
already rules it out, then calls
[`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md) on what survives.
**Both arms materialise and sort the same shape of result** — a `List<BkTreeMatch>` ordered by
distance ascending — so neither pays a cost the other is exempt from. 20,000 words per shape, 200
queries drawn from the corpus. **Building the tree runs in `[GlobalSetup]`**, which BenchmarkDotNet
excludes from every measured iteration: the scan has no structure to build, so a reader weighing
the `k = 1` win should price the build in separately.

```bash
python3 bench/corpus/generate_dictionary.py
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BkTree*'
```

Machine: 4-core Intel Xeon Processor @ 2.10GHz (BenchmarkDotNet's own header; the physical CPU
behind this virtualized host is not otherwise identified), Ubuntu 24.04.4 LTS, .NET SDK 10.0.111,
.NET 10.0.11 runtime — a hosted session container, not a dedicated benchmark machine, so **these
ratios are directional rather than exact**. Window: one `BenchmarkDotNet` run, default job,
2026-09-02, no other load on the container, 6 min 16 s across 16 benchmarks.

| Method | Radius | Shape | Mean | Ratio | RatioSD | Allocated | Alloc Ratio |
| --- | ---: | --- | ---: | ---: | ---: | ---: | ---: |
| `LengthFilteredScan` | 1 | clustered | 153.01 ms | 1.00 | 0.02 | 27.25 KB | 1.00 |
| `TreeWithinDistance` | 1 | clustered | **90.91 ms** | **0.59** | 0.02 | 103.75 KB | 3.81 |
| `LengthFilteredScan` | 1 | uniform | 161.09 ms | 1.00 | 0.02 | 23.86 KB | 1.00 |
| `TreeWithinDistance` | 1 | uniform | **84.03 ms** | **0.52** | 0.02 | 116.56 KB | 4.88 |
| `LengthFilteredScan` | 2 | clustered | **223.35 ms** | 1.00 | 0.01 | 103.44 KB | 1.00 |
| `TreeWithinDistance` | 2 | clustered | 352.15 ms | 1.58 | 0.02 | 260.13 KB | 2.51 |
| `LengthFilteredScan` | 2 | uniform | **234.14 ms** | 1.00 | 0.03 | 54.65 KB | 1.00 |
| `TreeWithinDistance` | 2 | uniform | 315.79 ms | 1.35 | 0.06 | 193.09 KB | 3.53 |
| `LengthFilteredScan` | 3 | clustered | **279.42 ms** | 1.00 | 0.02 | 949.90 KB | 1.00 |
| `TreeWithinDistance` | 3 | clustered | 489.30 ms | 1.75 | 0.04 | 1366.63 KB | 1.44 |
| `LengthFilteredScan` | 3 | uniform | **286.40 ms** | 1.00 | 0.02 | 741.56 KB | 1.00 |
| `TreeWithinDistance` | 3 | uniform | 476.27 ms | 1.66 | 0.07 | 1153.80 KB | 1.56 |
| `LengthFilteredScan` | 4 | clustered | **327.14 ms** | 1.00 | 0.03 | 5113.56 KB | 1.00 |
| `TreeWithinDistance` | 4 | clustered | 569.75 ms | 1.74 | 0.06 | 7216.20 KB | 1.41 |
| `LengthFilteredScan` | 4 | uniform | **326.21 ms** | 1.00 | 0.01 | 5514.13 KB | 1.00 |
| `TreeWithinDistance` | 4 | uniform | 584.62 ms | 1.79 | 0.07 | 7964.50 KB | 1.44 |

Ratio is `Mean(TreeWithinDistance) / Mean(LengthFilteredScan)`, BenchmarkDotNet's own baseline
column: below 1 the tree is faster, above 1 it is slower than never having built it. **Only the
`k = 1` rows are below 1**, and the gap widens with radius rather than closing — 0.52/0.59
(uniform/clustered) at `k = 1` against 1.79/1.74 at `k = 4`.

**Pruning is not the whole cost.** An instrumented run in the same window counted raw distance
computations, tree over scan, at 0.33/0.34 (`k = 1`), 0.79/0.86, 0.92/0.95 and 0.96/0.93 (`k = 4`):
the tree does compute fewer distances at every radius. What that count cannot see is per-node
traversal — a `Dictionary<int, Node>` lookup, a `Stack<Node>` push and a call through the stored
`Func<string, string, int>` metric delegate, against the scan's one array read and one integer
subtraction through a static method the JIT inlines. That fixed cost is cheap at `k = 1`, where the
tree visits a third as many nodes as the scan has candidates, and dominates from `k = 2`, where it
visits most of the corpus one node at a time instead of one array element at a time.

The allocation column carries the same story: the tree starts at **3.81× to 4.88×** the scan at
`k = 1`, where a `Stack<Node>` costs more than a 20-word hit list, and falls to **1.41× to 1.44×**
by `k = 4` as the shared result list comes to dominate both sides.
[`docs/guides/dictionary-lookup.md`](dictionary-lookup.md) carries the reader-facing version: the
tree is worth using at `k = 1`, and a length-filtered scan is the better answer past it.

## Lodestar.Fuzzy

### [`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md) against Fastenshtein 1.0.12, Quickenshtein 1.5.1 and F23.StringSimilarity 7.0.1

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

### `Fuzz` against Raffinert.FuzzySharp 6.0.0

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15.

| Operation | Lodestar | FuzzySharp | Faster |
| --- | ---: | ---: | ---: |
| [`Fuzz.Ratio`](../reference/fuzzy/matching/fuzz-ratio.md) | 62.18 ns, 0 B | 129.69 ns, 80 B | Lodestar, 2.09× |
| [`Fuzz.PartialRatio`](../reference/fuzzy/matching/fuzz-partialratio.md) | 452.36 ns, 0 B | 5,767.01 ns, 160 B | Lodestar, 12.75× |
| [`Fuzz.TokenSetRatio`](../reference/fuzzy/matching/fuzz-tokensetratio.md) | 653.21 ns, 1,448 B | 1,159.41 ns, 1,944 B | Lodestar, 1.78× |
| [`Fuzz.WRatio`](../reference/fuzzy/matching/fuzz-wratio.md) | 1,346.20 ns, 2,760 B | 2,938.48 ns, 3,128 B | Lodestar, 2.18× |

**Ahead on all four**, and allocating less on each.

### The score matrix against FuzzySharp and `rapidfuzz` (issue #1123)

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

[`Process.Cdist`](../reference/fuzzy/matching/process-cdist.md) costs **exactly** what the same
double loop written by hand against [`Fuzz.Ratio`](../reference/fuzzy/matching/fuzz-ratio.md)
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
[`Fuzz.Ratio`](../reference/fuzzy/matching/fuzz-ratio.md) rebuilds it per pair. On `Ratio` that fixed cost is most of what a cell costs and the reference is ~7× ahead;
on `WRatio`, which inspects its input and computes several sub-ratios, the same fixed cost is
amortised and **the two are level**. Two rows of the same two libraries on the same corpus say
more than any cross-language loop could.
[Issue #1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries the hoisting, which
needs new `Lodestar.Text` API and the release order that comes with it.

**A cold probe nearly went into this document.** One unwarmed `Stopwatch` pass over 200 × 200 read
25.5 ms against the reference's 0.5 — 51×, all of it JIT. The harness and BenchmarkDotNet agree
with each other at 1.58 ms and 1.97 ms and not with it.

### The score matrix under a cutoff, against `rapidfuzz` (issue #1134)

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
is the per-pair equality table [`Fuzz.Ratio`](../reference/fuzzy/matching/fuzz-ratio.md) rebuilds
where `cdist` builds it once per query, which
no cutoff can remove — only the hoisting
[#1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries, and the `Lodestar.Text`
release order it needs.

**The bound holds for the Indel ratio alone**, so the rows above are the default scorer and there
is no `WRatio` row to put beside them: `partial_ratio("cat", "the cat sat on the mat")` is 100
against a ceiling of 24, and a scorer passed by the caller has every pair scored.

## Lodestar.Embeddings

### SentencePiece and WordPiece encode, against Microsoft.ML.Tokenizers (issue #713)

Full method, and the check that both sides return the same ids:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0 runs of `TokenizerIncumbentBenchmarks`, **default
job**, on 2026-09-13, encoding all 5 000 documents of the corpus per operation.
Microsoft.ML.Tokenizers 2.0.0. `spiece_30k.model` is a unigram model.

| Model | Lodestar | Microsoft.ML.Tokenizers | Allocated, Lodestar | Allocated, Microsoft.ML.Tokenizers |
| --- | ---: | ---: | ---: | ---: |
| [`WordPieceTokenizer`](../reference/embeddings/tokenization/wordpiecetokenizer.md) | **20.26 ms** | 29.96 ms | 8.71 MB | **3.55 MB** |
| [`SentencePieceTokenizer`](../reference/embeddings/tokenization/sentencepiecetokenizer.md) | **26.21 ms** | 29.51 ms | 5.44 MB | **3.09 MB** |

**Both encoders run ahead of the incumbent**, and the ids agree with it over the whole corpus. Each
allocates more than the incumbent does: what is left is the result lists, the normalized text and,
for WordPiece, the lowercased copy — a matched token is the vocabulary's own string rather than a
new one.

Both encoders find every piece starting at a position in one walk of a double-array trie, rather
than probing a hash table once per candidate substring.

**What the trie costs the loader, which decision 0004 makes the product.** Measured with a
`Stopwatch` over the same two vocabularies on the same machine, not by BenchmarkDotNet:

| Constructor | First call | Warm |
| --- | ---: | ---: |
| `new SentencePieceTokenizer(vocabulary)` | 35.1 ms | 5.6–7.3 ms |
| `new WordPieceTokenizer(vocabulary)` | 31.8 ms | 6.6–8.1 ms |

Construction is **6–7 ms warm and about 35 ms on the first call in a process**, paid once per
tokenizer; where the first call's extra time goes was not measured apart. Reading
`spiece_30k.model` itself takes 14 ms on the same run.

### Byte-level BPE encode, against its unigram baseline and Microsoft.ML.Tokenizers (issue #673)

Full method, and the check that both sides return the same ids:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-14, of `BpeBenchmarks`,
`BpeScalingBenchmarks` and `TokenizerIncumbentBenchmarks`. Every document of the corpus is encoded
per operation, with `tokenizer_30k_bpe.json`'s byte-level model. Microsoft.ML.Tokenizers 2.0.0.

| Row | Mean | Allocated |
| --- | ---: | ---: |
| [`BpeTokenizer`](../reference/embeddings/tokenization/bpetokenizer.md) | **58.33 ms** | **28.47 MB** |
| `SentencePieceTokenizer`, the `Unigram` baseline | 17.34 ms | 5.43 MB |
| `CodeGenTokenizer`, Microsoft.ML.Tokenizers | 148.73 ms | 59.08 MB |
| Lodestar against it, `TokenizerIncumbentBenchmarks` | **57.95 ms, 2.57× faster** | |

**Byte-level BPE is 2.57× faster than the incumbent**, and 3.36× the cost of this package's own
unigram baseline, which is the cheaper model rather than the faster implementation. The ids agree
with the incumbent's over the whole corpus.
[Decision 0005](../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)
was taken on these numbers.

**The largest single step was a hash function.** The merge ranks were a `Dictionary<long, int>`
keyed by the two ids packed into a `long`, and `long.GetHashCode` is the exclusive or of the two
halves, so every pair whose ids share an exclusive or collided — `(1, 2)` with `(2, 1)`, `(0, 3)`
among them. An open-addressing table with a Fibonacci hash removed two thirds of what was left.

**What compiling the split pattern costs.** Matching it over the corpus takes 17 ms interpreted and
5 compiled. Code generation moves to the tokenizer's first encode, about 2 ms, measured with a
`Stopwatch`; building the tokenizer stays at 5 to 6 ms warm either way. The `netstandard2.0` build
compiles it too, and that cost was not measured on .NET Framework.

**One token with no split point**, `BpeScalingBenchmarks`:

| Length | Mean | Allocated |
| ---: | ---: | ---: |
| 512 | 10.20 μs | 7.48 KB |
| 1,024 | 21.49 μs | 14.53 KB |
| 2,048 | 48.49 μs | 28.58 KB |
| 4,096 | 152.17 μs | 56.63 KB |

The step from 2,048 to 4,096 costs 3.1×, where the others cost about 2×. It is one step, not a
curve: a `Stopwatch` taken further measured each doubling from 4,096 to 32,768 characters at 2.0
to 2.2×.

### [`VectorMath.Dot`](../reference/embeddings/search/vectormath-dot.md) against `TensorPrimitives`, by vector width (issue #754)

`tensor-primitives` (`bench/README.md` section 14) on the machine below, pinned to one core, three
conditions interleaved over five runs of nine. Median of the five medians, in ms, over
10,000 × 384 floats. Ratios above 1 mean `TensorPrimitives` is faster.

| row | `Vector512` on | `Vector512` off | AVX-512 off |
| --- | ---: | ---: | ---: |
| `ours_dot_knn` | 0.863 | 1.232 | 0.955 |
| `tp_dot_knn` | 6.222 | 3.860 | 3.946 |
| `ours_cosine_knn` | 1.803 | 2.055 | 1.814 |
| `tp_cosine_knn` | 10.795 | 6.130 | 6.363 |
| `ours_one_sweep` | 0.563 | 0.545 | 0.351 |
| `tp_one_sweep` | 0.456 | 0.387 | 0.369 |
| `index_search` | 3.734 | 3.787 | 3.692 |
| **dot ratio** | **0.14** | **0.32** | **0.24** |
| **cosine ratio** | **0.17** | **0.34** | **0.29** |

**Our kernel is ahead on the kNN pattern at every width**, 3–7×, and one long sweep is at parity.
Turning the 512-bit path off halves `TensorPrimitives`' time without changing the direction.
[Decision 0004](../decisions/0004-what-is-written-here-and-what-is-delegated.md)
records it against the hosted runner's opposite reading in
[0004](../decisions/0004-what-is-written-here-and-what-is-delegated.md).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 16 logical and 8 physical cores, Ubuntu 26.04.1
LTS, .NET 10.0.12 runtime. 2026-09-16.

### Saving and loading an index, against numpy

The nightly's `compare-persistence` of 2026-09-12 has Lodestar at 0.13–0.34× numpy on every
uncompressed index row. **Most of that is not code on either side: it is whose allocator keeps a
freed 15–20 MB block's pages.** glibc raises its mmap threshold after the first free of a block
that size, so a `Harness.Measure` loop of `np.load` refills warm pages; the .NET GC decommits a
dead large-object region, so every iteration of the C# loop page-faults its 15 MB again.

**Conditions.** AMD Ryzen 7 8700G, 8 cores / 16 threads, Ubuntu 26.04.1, .NET 10.0.12, numpy
2.5.3 on Python 3.12, a workstation shared with other sessions under the machine lock, one-minute
load average 2.2–4.1. The probes are a `Harness.Measure`-shaped Stopwatch loop, median of five
rounds, and are not BenchmarkDotNet; read their ratios.

Each side, with its allocator's retention turned off or on:

| row | default | retention flipped | flip |
| --- | ---: | ---: | --- |
| `np.load`, `BytesIO` | 1.073 ms | 7.276 ms | `MALLOC_MMAP_THRESHOLD_=65536`, pages fresh |
| `np.load`, file | 1.728 ms | 2.798 ms | the same |
| [`EmbeddingIndex.Load`](../reference/embeddings/search/embeddingindex-load.md), memory | 4.409 ms | 2.236 ms | `DOTNET_GCRetainVM=1`, pages kept |
| [`EmbeddingIndex.Load`](../reference/embeddings/search/embeddingindex-load.md), `MemoryStream` | 5.207 ms | 3.177 ms | the same |
| [`EmbeddingIndex.Load`](../reference/embeddings/search/embeddingindex-load.md), file | 6.484 ms | 4.348 ms | the same |
| 15.36 MB `memcpy` into a fresh `float[]` | 4.531 ms | 1.239 ms | the same |
| the save row's own `new MemoryStream(capacity)` plus one `memcpy`, no library code | 3.911 ms | 1.543 ms | the same |

- **numpy pays 6.8× on its load the moment its pages are fresh**, and a `memcpy` into a fresh
  `float[]` costs 3.7× one into kept pages. A caller who loads one index once pays those faults
  on both sides; only a loop of loads collects the difference, and the harness is a loop.
- **The save rows charge the C# side for the harness's sink.** `new MemoryStream(indexArtifact.Length)`
  allocates and zeroes 20.6 MB inside the timed window: 3.911 ms of the 4.057 ms
  `embedding_index_save` measures here, against 0.568 ms for the whole chunked base64 encode.
  `io.BytesIO()` grows on pages glibc kept.
- **What remains with retention on both sides is the format's own work**, which
  [decision 0001](../decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
  already prices: a JSON scan of the 20.6 MB document with its 10 000 ids (0.581 ms), a base64
  decode (0.760 ms into warm pages), and the finite scan numpy does not promise (0.235 ms).
  `embedding_index_ingest_npy` stays the like-for-like row.

Two levers inside that remainder were measured and **not taken**: a `Vector512` exponent-bits
finite scan, 0.235 to 0.167 ms, is 1.5% of a load; decoding and scanning in L2-sized slices,
3.711 to 3.655 ms, is noise.

### Compressing an index (issue #378)

The artifact is base64 inside JSON, which spends eight bits to carry six, so it is about 1.33× the
raw block. Deflate takes that back almost exactly — base64 is the one expansion a general-purpose
coder undoes perfectly. The question is what the time costs.

Machine: Intel Core i7-4770S, .NET 10.0.10, a synthetic 4,000 × 384 index through the real `Save`
and `Load`. Window: warmed, median of 7, the five modes interleaved in one window, published
2026-08-20.

| | bytes | × size | save | × save | load | × load |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| plain | 8 231 006 | 1.000 | 10.2 ms | 1.00 | 7.9 ms | 1.00 |
| gzip `Fastest` | 6 257 079 | 0.760 | 270.8 ms | **26.67×** | 56.5 ms | 7.19× |
| gzip `Optimal` | 6 151 764 | 0.747 | 382.1 ms | **37.62×** | 46.6 ms | 5.92× |
| brotli `Fastest` | 6 074 449 | **0.738** | 37.4 ms | 3.68× | 40.8 ms | 5.19× |
| brotli `Optimal` | 6 069 780 | 0.737 | 122.1 ms | 12.02× | 38.3 ms | 4.87× |

**The size claim holds exactly**: 0.747 × 1.333 = 0.996 of the raw block. Level 9 gives the same
bytes as level 6, so there is nothing to tune, and **deflate is dominated on all three axes** —
brotli `Fastest` is smaller than gzip `Optimal` and writes it seven times faster.

**Against numpy, compressed**, from the nightly's own rows at 10,000 × 384 on a hosted runner —
ratios only, per that page's warning, since a shared runner's absolutes are not comparable:

| operation | C# cpu | bytes | Python cpu | Python bytes |
| --- | ---: | ---: | ---: | ---: |
| `embedding_index_save` | 5.949 ms | 20 589 007 | 1.337 ms | 15 360 128 |
| `embedding_index_save_gzip` | 456.995 ms | 15 251 458 | 638.992 ms | 14 022 374 |
| `embedding_index_load` | 5.519 ms | 20 589 007 | 1.327 ms | 15 360 128 |
| `embedding_index_load_gzip` | 81.774 ms | 15 251 458 | 72.368 ms | 14 022 374 |

- **Compressed, this package is ahead of numpy on the write and level with it on the read** —
  1.40× on `savez_compressed`, 0.88× coming back. The deflate coder is the same on both sides, so
  what is compared is what each side hands it.
- **Compression closes most of the format gap.** Uncompressed the artifact is 1.34× numpy's block,
  the expansion
  [decision 0001](../decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
  priced; compressed it is 1.09×.
- **The price grows with the artifact**, which is the opposite of what would make it worth paying:
  0.741× the size for 76.8× the save and 14.8× the load on the corpus, against 37.62× and 5.92× for
  `Optimal` on the 8 MB synthetic index. The indexes big enough for 26 % of a disk to matter are
  the ones where compressing costs most.

### Batched embedding — what the number is, and what it is not

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BatchEmbedding*' --inProcess
```

**Read this before quoting the ratio.** The model is `tiny_embedder.onnx`: one
Gather node over a 64 × 4 table, because weights are never committed
(`CONTRIBUTING.md`) and a real encoder is a hundred megabytes. Its arithmetic is
free. So what is measured is the per-sequence cost that batching removes — graph
dispatch, thread-pool wake-up, tensor wrapping — and none of the matrix
multiplication a real encoder adds to *both* sides. This is an upper bound on the
speed-up, not the speed-up.

Machine: Intel Core i7-4770S (Haswell, 4 physical cores), Ubuntu 24.04, .NET 10.0.10, X64 RyuJIT
AVX2. Window: four runs in one window — two per target, alternating — published 2026-08-06; the
run's own date was not recorded. Full job, `[MemoryDiagnoser]`, `InProcessEmitToolchain`. Corpus
of 1 to 61 words per text, sub-batch 8. `UnitLoop` is the baseline — one `Embed`
call per text, which is what the guide's three lines amounted to before
`EmbedBatch` existed. **Two runs, both shown**, because BenchmarkDotNet's `±`
describes dispersion inside one process and not reproducibility across processes.

| Texts | `UnitLoop` | `EmbedBatch` | ratio | `EmbedBatchBucketed` | ratio | allocated vs baseline |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 1 | 9.9 / 11.0 µs | 10.6 / 10.8 µs | 1.06 / 0.99 | 10.0 / 11.1 µs | 1.00 / 1.01 | 1.12 |
| 8 | 168 / 180 µs | 105 / 107 µs | 0.62 / 0.60 | 101 / 106 µs | 0.60 / 0.59 | 0.95 |
| 32 | 628 / 672 µs | 360 / 381 µs | 0.57 / 0.57 | 346 / 349 µs | 0.55 / 0.52 | 0.94 / 0.91 |
| 128 | 2 439 / 2 602 µs | 1 482 / 1 460 µs | 0.61 / 0.56 | 1 370 / 1 356 µs | 0.56 / 0.52 | 0.94 / 0.91 |

**Batching removes about 40 % of the wall clock** from 8 texts upward and stays
there — 0.56–0.62 across every pairing — as the per-call overhead is amortized
over the whole sub-batch. At a single text there is nothing to amortize and the
two paths are a wash: 1.06 in one run, 0.99 in the next, which is a way of
saying this benchmark cannot tell them apart there rather than that either wins.

**Bucketing is a different story, and the honest answer is smaller.** It engages
only when the corpus spans more than one sub-batch, so the rows at 1 and 8 above
run the *identical* code in both columns — they are the control, and what they
differ by is this harness's noise floor: 1–3 % on seven of the eight control
measurements taken here, with one outlier at 5.7 %. At 128 texts bucketing is
ahead by 5.8–7.5 % in all four pairings, and ahead at 32 in all four as well.
The sign is consistent where the magnitude alone would not be decisive. What is
decisive is the allocation column, which is counted rather than sampled:
1 764 KB → 1 697 KB at 128 texts, in every run. That is padding genuinely not
written. On a model doing real work that padding would be matrix multiplication
not performed, and the time would follow; this model cannot show it, so the
claim stops here.

**The two builds are level here, and that is the structurally correct answer.** The same benchmark
against the `netstandard2.0` assemblies puts that side 1.4–5.7 % *ahead* of net10 in all four
pairings (355 / 365 µs against 360 / 381 at 32 texts; 1 418 / 1 419 against 1 482 / 1 460 at 128) —
inside the harness's noise, but consistent in direction. `Pooling` guards its `Vector<T>` branch
with `accumulator.Length >= Vector<float>.Count`, and `tiny_embedder.onnx` has a hidden size of 4
(`EMBEDDING_DIM` in `tools/build_tiny_models.py`) against `Vector<float>.Count` of 8 under AVX2, so
on net10 the guard is false and both builds run the same scalar tail loop. This benchmark cannot
resolve a difference between the targets, and reports that instead of a number. Where the vector
path does engage it is worth 4×–7×, in `VectorMath` over 384–1024 dimensions
([`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md) section 2).
The one difference it does resolve is counted rather than timed: the unit-loop path allocates 0.6 %
more on `netstandard2.0` (1 887 KB against 1 875 at 128 texts), identically in both runs, while the
two batch paths allocate byte for byte the same on both targets.

```bash
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*BatchEmbedding*'
```

`--inProcess` on the first command and not on the second is the point. The `netstandard2.0` project
pins `InProcessEmitToolchain` in its `Program.cs` — it has to, or BenchmarkDotNet's generated
project re-resolves the `ProjectReference` and silently restores the net10.0 build — so the flag is
what puts the net10 side on the same toolchain. Without it the two commands measure the same code
two different ways.

**Conditions.** The four runs behind the table were taken back to back in one window, alternating
net10 and `netstandard2.0`, with the one-minute load average between 5.1 and 5.9 on 8 logical cores
— the editor's language servers and the session driving the runs are part of that load and cannot
be excluded from inside it. Both columns pay it equally, so the table is internally comparable; it
is not comparable to figures taken on this machine in a quieter state, and the ratios travel
between such sets while the absolute microseconds do not.

## Lodestar.Metrics

### Classification metrics (issue #61) — vs scikit-learn

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

#### Balanced accuracy, Matthews correlation, Cohen's kappa (issue #93)

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

#### Regression metrics — mse, mae, median_ae, r2 (issue #92)

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

### `Lodestar.Metrics` against ML.NET 5.0.0's binary evaluator

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

## Lodestar.Decomposition

### Truncated SVD and NMF against ML.NET 5.0.0's `ProjectToPrincipalComponents`

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15, and the decomposition class in
section 16.

A 2,000 × 500 term-document matrix at 2% density, rank 20:

| Row | Mean | Ratio |
| --- | ---: | ---: |
| `TruncatedSvd`, over the sparse matrix | 17.58 ms | 1.00 |
| `Nmf`, capped at 50 iterations | 118.72 ms | 6.76 |
| ML.NET's centred PCA, over the dense twin | **14.85 ms** | 0.85 |

**ML.NET's PCA is 1.18× faster, and it is a different decomposition**: centred and dense against
uncentred and sparse. Section 16 of `bench/README.md` says what is checked instead of agreement, and
decision 0004 why the gap that matters is the explained variance, which ML.NET does not report.

### The variance principal components explain, against NumFlat (issue #701)

Full method and what the pair does and does not compare:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#30-the-variance-principal-components-explain-against-numflat-issue-701).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: one `BenchmarkDotNet` run, **default job**, on 2026-09-13, 8 benchmarks. NumFlat
1.3.4. Both sides were checked to return the same spectrum before either was timed.

| Shape | [`PrincipalComponentVariance`](../reference/decomposition/factorization/principalcomponentvariance.md) | NumFlat `PrincipalComponentAnalysis` | NumFlat / Lodestar | Allocated, Lodestar | Allocated, NumFlat |
| --- | ---: | ---: | ---: | ---: | ---: |
| 200 × 10 | 14.74 μs | 15.35 μs | 1.04 | 2.38 KB | 1.94 KB |
| 2,000 × 10 | 82.53 μs | 111.83 μs | **1.36** | 2.38 KB | 1.94 KB |
| 2,000 × 50 | 1,977.65 μs | 1,634.81 μs | **0.83** | 42.07 KB | 40.06 KB |
| 100 × 200 | 7,834.02 μs | 9,306.55 μs | **1.19** | 318.27 KB | 628.54 KB |

**This package is faster on three shapes of four and slower on one**, while NumFlat's row also
computes the eigenvectors and the mean. The one it loses is the one where the eigen solve
dominates: 50 × 50 is where a one-sided Jacobi, several sweeps of `O(p³)`, falls behind a
tridiagonal solver. At 10 columns the Gram matrix is most of the work, and centring each row into
a buffer of one row's width keeps it in cache.

The wide block is solved through the 100 × 100 Gram matrix of its rows rather than the 200 × 200
one of its columns, which is where the 1.19 and half the allocation come from.

The path to these numbers is in
[decision 0003](../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md): a
Jacobi solve over the whole centred block measured 13× slower than NumFlat at 2,000 × 50 before
the Gram route replaced it. **Below `net8.0` the second row is Meta.Numerics**: NumFlat does not install there and ML.NET
reports no eigenvalue, and [its section](#metanumerics-against-lodestarstats-and-principalcomponentvariance-issue-756)
has the numbers.

### The factorization applied to unseen rows (issue #1124)

Full method and why both losses are run:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#57-the-factorization-applied-to-unseen-rows-against-scikit-learn-issue-1124).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `BenchmarkDotNet` 0.14.0, default job;
2,000 × 500 at 2% density for the fit, 200 unseen rows for the transform.

**There is no .NET incumbent**: neither ML.NET nor NumFlat publishes a non-negative factorization,
so the row that matters is what a batch of unseen rows costs against the fit it is scored on.

| operation | cost | against the fit |
| --- | ---: | ---: |
| [`TruncatedSvd.Fit`](../reference/decomposition/factorization/truncatedsvd-fit.md), rank 20 | 11.527 ms | — |
| [`Nmf.Fit`](../reference/decomposition/factorization/nmf-fit.md), rank 20, 50 updates | 66.647 ms | 1.00 |
| [`Nmf.Transform`](../reference/decomposition/factorization/nmf-transform.md), 200 unseen rows | **2.102 ms** | **0.032** |

**A batch of unseen rows costs about a thirtieth of the fit**, which is the number a caller sizing
a scoring loop needs. It iterates — it is a factorization with `H` held fixed, not a projection —
so it is not free the way
[`TruncatedSvd.Transform`](../reference/decomposition/factorization/truncatedsvd-transform.md)'s
multiply is; it is also not another fit.

Against `scikit-learn` 1.9.1 through `compare-nmf-transform`, one run of each side, milliseconds
per operation, best of five, `solver='mu'` and `tol=0.0` pinned so both compute the same thing:

| unseen rows | loss | Lodestar | `scikit-learn`, wall / cpu | ratio, wall | ratio, cpu |
| ---: | --- | ---: | ---: | ---: | ---: |
| 100 | Frobenius | 1.264 ms | **0.345** / 0.345 ms | 0.27 | 0.27 |
| 500 | Frobenius | 4.054 ms | **0.705** / 0.705 ms | 0.17 | 0.17 |
| 2,000 | Frobenius | **14.708 ms** | 2.567 / 40.553 ms | 0.17 | **2.75** |
| 100 | Kullback-Leibler | **0.732 ms** | 7.400 / 7.400 ms | **10.11** | **10.11** |
| 500 | Kullback-Leibler | **3.299 ms** | 16.913 / 16.912 ms | **5.13** | **5.13** |
| 2,000 | Kullback-Leibler | **13.015 ms** | 72.064 / 1,007.735 ms | **5.54** | **77.22** |

**The two losses are two different computations, and the table is two different answers.** The
Frobenius update is three dense products; scikit-learn hands them to a parallel BLAS, which is why
it is 5.9× ahead on elapsed time at 2,000 rows and **2.75× behind on processor time** — it spends
40.6 ms of CPU to finish in 2.6 ms. This runs on one thread and spends 14.7 ms of CPU to finish in
14.7 ms.

**The Kullback-Leibler update is where the sparse representation pays.** The ratio `X / WH` is
needed only where `X` is stored, which is 2% of the cells here; scikit-learn densifies `W H` and
pays for all of them. That is 5.5× on elapsed time and **77× on processor time** at 2,000 rows —
it burns a full second of CPU for 72 ms of wall clock.

A caller choosing between the two losses on a sparse corpus should know that the one this package
is faster at is also the one a term-document matrix wants: Kullback-Leibler fits a Poisson noise
model, which is what counts are.

## Lodestar.Cluster

### k-means against NumFlat and Meta.Numerics (issue #681)

Full method, the two classes and why only one is like-for-like:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#34-k-means-against-numflat-and-metanumerics-issue-681).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: one `BenchmarkDotNet` 0.14.0 run, **default job**, on 2026-09-14, 15 benchmarks.
NumFlat 1.3.4, Meta.Numerics 4.2.0. Every pair was checked to return the same centres before either
side was timed; the largest difference was zero.

**Lloyd's iterations from the same centres** — the like-for-like pair:

| Shape (rows × features × k) | Iterations | [`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) | NumFlat `KMeans.Update` | NumFlat / Lodestar | Allocated, Lodestar | Allocated, NumFlat |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 10,000 × 2 × 8 | 101 | 19.21 ms | 70.34 ms | **3.66** | 98.92 KB | 96.40 KB |
| 10,000 × 16 × 16 | 4 | 6.51 ms | 9.87 ms | **1.52** | 88.70 KB | 13.58 KB |
| 50,000 × 8 × 32 | 8 | 64.02 ms | 127.26 ms | **1.99** | 410.24 KB | 36.39 KB |

**This package is ahead on every shape, 1.52× to 3.66×**, and allocates more on the two wider ones:
its current and previous labels are one `int` per row each, 80 KB at 10,000 rows, where NumFlat's
allocation grows with the iteration count instead — which is why the 101-iteration row is level.

**A whole fit, k-means++ included**, under each library's own stopping rule:

| Shape | [`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) | NumFlat `KMeans` | Meta.Numerics `MeansClustering` | NumFlat / Lodestar | Meta.Numerics / Lodestar |
| --- | ---: | ---: | ---: | ---: | ---: |
| 10,000 × 2 × 8 | 0.41 ms | 3.10 ms | 1.69 ms | 7.49 | 4.09 |
| 10,000 × 16 × 16 | 3.93 ms | 21.53 ms | 54.62 ms | 5.48 | 13.90 |
| 50,000 × 8 × 32 | 19.98 ms | 260.88 ms | 408.07 ms | 13.06 | 20.42 |

**Read this table as what a caller pays, not as a kernel ratio.** On these well-separated blobs
scikit-learn's scaled tolerance stops this package after one or two iterations, and neither incumbent
reports how many it ran. The second table is the one that compares arithmetic; this one says that a
default fit here is 4× to 20× cheaper, and part of that is when it decides to stop.

**Meta.Numerics is the comparison that holds below `net8.0`**, and the only one: NumFlat does not
install there.

### DBSCAN against NumFlat and `Dbscan` (issue #759)

Full method, the two classes and what the agreement check refused:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#48-dbscan-against-numflat-and-dbscan-issue-759).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET 10.0.12 runtime, AVX-512. Window: one
`BenchmarkDotNet` 0.14.0 run, **`--job short`**, on 2026-09-16, 10 benchmarks. NumFlat 1.3.4,
`Dbscan` 3.0.0. Every shape was checked to return the same *partition* — cluster numbering
ignored — on all the libraries in its table before any was timed.

**Planar, where all three compete:**

| Shape (rows × features × blobs) | radius | [`Dbscan.Fit`](../reference/cluster/partitioning/dbscan-fit.md) | NumFlat `DbScan.Fit` | `Dbscan` 3.0.0 | NumFlat / Lodestar | Dbscan / Lodestar |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 5,000 × 2 × 5 | 1.0 | 63.70 ms | 287.31 ms | 157.27 ms | **4.51** | **2.47** |
| 20,000 × 2 × 10 | 1.0 | 889.93 ms | 9,716.85 ms | 2,365.60 ms | **10.92** | **2.66** |

**Above two features, where only NumFlat follows:**

| Shape | radius | [`Dbscan.Fit`](../reference/cluster/partitioning/dbscan-fit.md) | NumFlat `DbScan.Fit` | NumFlat / Lodestar |
| --- | ---: | ---: | ---: | ---: |
| 10,000 × 8 × 8 | 3.0 | 413.80 ms | 1,536.70 ms | **3.71** |
| 5,000 × 16 × 8 | 5.0 | 193.20 ms | 518.30 ms | **2.68** |

**Ahead on every shape, 2.47× to 10.92×, and allocating 2.89× to 5.46× less**: 13.98 MB against
NumFlat's 66.59 MB and `Dbscan`'s 40.40 MB at 5,000 planar points, 111.97 MB against 531.88 MB and
372.11 MB at 20,000.

**`Dbscan` 3.0.0 has no row in the second table because it has no entry point there.** Its `Point`
carries `X` and `Y` and nothing else, which is
[decision 0004](../decisions/0004-what-is-written-here-and-what-is-delegated.md)'s whole
argument for writing this class — the gap is reach below `net8.0`, and the speed is a second
finding rather than the claim.

**One measured defect in an incumbent, and it is not ours.** NumFlat 1.3.4 marks a non-core sample
noise permanently when the ascending scan reaches it before any cluster has been grown, where the
original algorithm and scikit-learn let a later expansion take it as a border sample. Nine points
in one dimension separate them, and at sixteen features with a radius of 3 it costs 1,017 rows of
5,000 — where this package matches scikit-learn exactly. `bench/README.md` section 48 has the
reproducer and the alternative explanation that was tested and refused.

### Agglomerative clustering against `Aglomera` (issue #760)

Full method, what was measured before any code and why the benchmark runs on blobs:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#49-agglomerative-clustering-against-aglomera-issue-760).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET 10.0.12 runtime, AVX-512. Window: one
`BenchmarkDotNet` 0.14.0 run, **`--job short`**, on 2026-09-16, 16 benchmarks, with nothing else
running — a first run that shared the machine with the oracle generator was discarded. `Aglomera`
1.1.1. Every combination was checked to return the same merge heights in order, and the same
partition at the cut, before either side was timed.

| samples | linkage | [`AgglomerativeClustering.Fit`](../reference/cluster/partitioning/agglomerativeclustering-fit.md) | `Aglomera` `GetClustering` | Aglomera / Lodestar | allocated, Lodestar | allocated, Aglomera |
| ---: | --- | ---: | ---: | ---: | ---: | ---: |
| 500 | ward | 2.03 ms | 48.91 ms | **24.14** | 1.04 MB | 64.19 MB |
| 500 | complete | 1.98 ms | 61.70 ms | **31.21** | 1.04 MB | 83.11 MB |
| 500 | average | 1.92 ms | 56.28 ms | **29.37** | 1.04 MB | 59.88 MB |
| 500 | single | 0.45 ms | 87.45 ms | **194.69** | 0.06 MB | 81.29 MB |
| 1,500 | ward | 17.44 ms | 1,110.66 ms | **63.70** | 8.97 MB | 573.62 MB |
| 1,500 | complete | 16.98 ms | 1,209.18 ms | **71.23** | 8.97 MB | 747.36 MB |
| 1,500 | average | 17.28 ms | 1,224.95 ms | **70.91** | 8.97 MB | 537.15 MB |
| 1,500 | single | 3.53 ms | 2,081.80 ms | **589.97** | 0.18 MB | 732.69 MB |

**Ahead on every row, 24× to 590×, and the gap widens with the sample count** — the three
nearest-neighbour-chain linkages go from 24–31× at 500 to 64–71× at 1,500. Single linkage holds no
distance matrix at all, 185 KB where `Aglomera` allocates 733 MB.

**These rows are the only ones where the comparison is fair.** Blobs have no tied merge heights; on
data that does, `Aglomera` breaks ties the other way and can build a different tree, and its Ward
height is reported as `d²/2`. The speed is a second finding — the reason this class exists is that
`Aglomera`'s answer is not scikit-learn's.

## Lodestar.Preprocessing

### The splitters against ML.NET and scikit-learn (issue #762)

Full method and what cannot be made to agree:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#44-the-splitters-against-mlnet-and-scikit-learn-issue-762).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, default job, one run. Five folds,
three classes at 60 / 30 / 10.

**ML.NET's split is lazy**, so it appears twice: the call, and the call followed by reading which
rows each fold holds. Only the second is a split a caller can fit on, and it is the one that scales
with the data — the construction is flat in the row count because it builds ten wrappers and stops.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 10,000 | five folds, constructed | **52.52 μs** | 79.19 μs | 1.51 |
| 10,000 | five folds, rows read | **52.52 μs** | 3,330.43 μs | **63.44** |
| 10,000 | train/test, constructed | **3.11 μs** | 15.70 μs | 5.05 |
| 10,000 | train/test, rows read | **3.11 μs** | 592.81 μs | **190.55** |
| 100,000 | five folds, constructed | **695.48 μs** | 78.35 μs | 0.11 |
| 100,000 | five folds, rows read | **695.48 μs** | 25,527.38 μs | **36.71** |
| 100,000 | train/test, constructed | **56.81 μs** | 16.30 μs | 0.29 |
| 100,000 | train/test, rows read | **56.81 μs** | 4,454.57 μs | **78.42** |

Stratifying costs 136.77 μs at 10,000 rows and 1,489.37 μs at 100,000 — 2.6× and 2.1× the plain fold
cut. **ML.NET has no row to compare it against**: `CrossValidationSplit` and `TrainTestSplit` never
stratify ([dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396), open
since 2019).

Allocation, per operation: 273.94 KB against ML.NET's 149.07 KB at 10,000 rows for the fold cut —
**this returns every index of every fold, where ML.NET returns views that allocate again on each
read**, so the two columns count different things and the larger one is the one holding the answer.

Against `scikit-learn` 1.9.0 through `compare-splitters`, one run of each side, milliseconds per
split, best of five:

| n | operation | Lodestar | `scikit-learn`, wall / cpu | ratio, wall |
| ---: | --- | ---: | ---: | ---: |
| 10,000 | `KFold` | **0.050 ms** | 0.057 / 0.057 ms | **1.14** |
| 10,000 | `StratifiedKFold` | **0.135 ms** | 0.445 / 0.445 ms | **3.30** |
| 10,000 | train/test | **0.003 ms** | 0.073 / 0.073 ms | **25.70** |
| 100,000 | `KFold` | **0.798 ms** | 2.145 / 2.145 ms | **2.69** |
| 100,000 | `StratifiedKFold` | **1.657 ms** | 5.718 / 5.717 ms | **3.45** |
| 100,000 | train/test | **0.149 ms** | 0.194 / 0.194 ms | **1.30** |
| 1,000,000 | `KFold` | **12.273 ms** | 14.919 / 14.917 ms | **1.22** |
| 1,000,000 | `StratifiedKFold` | **20.507 ms** | 48.187 / 48.178 ms | **2.35** |
| 1,000,000 | train/test | **0.517 ms** | 1.323 / 1.323 ms | **2.56** |

The million-row rows move about ±20% run to run on this machine — `KFold` there read 10.0 ms once
and 12.0 to 12.4 ms in the four runs after, on unchanged code. The ratios below 100,000 rows are
stable to the third decimal across every run.

`TrainTest` writes the two index halves straight out: an identity read comes out ascending already,
so neither half is sorted and no order array is filled.

### The scalers against ML.NET's normalizers (issue #763)

Full method, and why `fixZero: false` is passed:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#45-the-scalers-against-mlnets-normalizers-issue-763).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, one invocation per iteration,
five warmups and twenty iterations. Ten features per row; ML.NET's estimator is lazy, so it appears both
as a fit and as a fit whose values are read back.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | min-max, fit + transform | **68.91 μs** | 225.28 μs (fit only) | **3.27** |
| 1,000 | min-max, values read | **68.91 μs** | 1,208.41 μs | **17.54** |
| 1,000 | robust, values read | **1,189.85 μs** | 3,521.85 μs | **2.96** |
| 20,000 | min-max, fit + transform | **1,106.62 μs** | 3,614.47 μs (fit only) | **3.27** |
| 20,000 | min-max, values read | **1,106.62 μs** | 9,674.44 μs | **8.74** |
| 20,000 | robust, values read | **10,430.85 μs** | 15,245.13 μs | **1.46** |

`MaxAbsScaler` costs 65.21 μs and 1,116.54 μs at the two sizes and **has no ML.NET row**: its
normalizers offer mean-variance, min-max, log-mean-variance, robust scaling, binning and L_p norm,
and none of them is "divide by the largest absolute value" — `NormalizeMinMax(fixZero: true)` comes
closest and is a different transform.

Both sides scale the same column to the same values, checked row by row to `1e-6` before either was
timed — single precision, which is what ML.NET's pipeline carries.

**Where the cost is.** `RobustScaler` is about nine times `MinMaxScaler` here and allocates twice as
much: a percentile has to order its column, so it sorts each feature once where the other two take a
single pass. `numpy.percentile` partitions rather than sorting, which is the same asymptotic work
with smaller constants and is where to look if that row ever needs to be cheaper. Allocation, at
20,000 rows: 1,563.96 KB for min-max against ML.NET's 1,133.80 KB, and 3,126.26 KB for robust
against 4,821.33 KB.

### The encoders and the imputer against ML.NET (issue #764)

Full method, and the shape that had to be corrected first:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#46-the-encoders-and-the-imputer-against-mlnet-issue-764).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, one invocation per iteration,
five warmups and twenty iterations. Twenty categories; ML.NET's estimator is lazy, so it appears as a fit
and as a fit whose values are read back.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | one-hot, fit + transform | **136.37 μs** | 195.66 μs (fit only) | **1.43** |
| 1,000 | one-hot, **values read** | **136.37 μs** | 728.92 μs | **5.35** |
| 1,000 | impute, **values read** | **84.09 μs** | 650.84 μs | **7.74** |
| 20,000 | one-hot, fit + transform | **3,672.62 μs** | 2,083.19 μs (fit only) | 0.57 |
| 20,000 | one-hot, **values read** | **3,672.62 μs** | 5,384.35 μs | **1.47** |
| 20,000 | impute, **values read** | **925.39 μs** | 4,670.33 μs | **5.05** |

[`Encoders.Ordinal`](../reference/preprocessing/encoding/encoders-ordinal.md) costs 133.73 μs and 2,875.13 μs at the two sizes — about three quarters of the
one-hot encoding, at a **tenth** of the allocation (16.70 KB against 165.24 KB at 1,000 rows), since
it produces one column rather than one per category. ML.NET's `MapValueToKey` is its counterpart and
is not measured here: it maps to a key type inside the pipeline rather than to a number a caller
holds.

**The correction, stated because the first table was wrong.** `OneHotEncoding` takes **one named
column** where this package's encoders take a matrix of however many features, so the first run
encoded four features here against one there — four times the work for the same row, reported as
**2.8× slower** at 20,000. One column on both sides is the comparison above. `ReplaceMissingValues`
does take a vector column, so the imputer rows compare four features against four.

**Allocation** is where the two differ most on the read: 165.24 KB against 413.76 KB at 1,000 rows,
and 3,282.43 KB against 1,212.64 KB at 20,000 — this package materialises every encoded column as a
`double`, where ML.NET's cursor yields rows one at a time and never holds the matrix.

### The feature transformers against ML.NET and scikit-learn (issue #1122)

Full method and what each row does and does not compare:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#55-the-feature-transformers-against-mlnet-and-scikit-learn-issue-1122).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `BenchmarkDotNet` 0.14.0, one invocation
per iteration, five warmups and twenty iterations. Four features; ML.NET's estimators are lazy, so
each appears as a fit whose rows are read.

**Only two of the seven have an incumbent in .NET at all.** `NormalizeLpNorm` scales a row to unit
norm as [`Normalizer`](../reference/preprocessing/transforming/normalizer.md) does; `NormalizeBinning`
cuts a feature into bins but emits a position in `[0, 1]` rather than the bin, so the two price the
same traversal and not the same answer.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | unit-norm rows, **values read** | **28.01 μs** | 717.09 μs | **25.60** |
| 1,000 | five bins, fit + transform, **values read** | **566.79 μs** | 1,525.96 μs | **2.69** |
| 20,000 | unit-norm rows, **values read** | **313.21 μs** | 4,816.59 μs | **15.38** |
| 20,000 | five bins, fit + transform, **values read** | **8,657.46 μs** | 18,112.77 μs | **2.09** |

The other five have nothing to race. Their own numbers at 20,000 rows and four features, so the
shape of the cost is on the record:
[`PolynomialFeatures.Transform`](../reference/preprocessing/transforming/polynomialfeatures-transform.md) 950.72 μs,
[`QuantileTransformer.Fit`](../reference/preprocessing/transforming/quantiletransformer-fit.md) 7,173.56 μs and its
transform 10,806.04 μs,
[`PowerTransformer.Fit`](../reference/preprocessing/transforming/powertransformer-fit.md) 32,350.17 μs against a
transform of 1,085.94 μs, and
[`KnnImputer.Transform`](../reference/preprocessing/encoding/knnimputer-transform.md) 38,939.98 μs over its own fixed
2,000 rows. **[`PowerTransformer`](../reference/preprocessing/transforming/powertransformer.md)'s fit is thirty times its transform** — it maximises a
log-likelihood by Brent's method per feature — and [`KnnImputer`](../reference/preprocessing/encoding/knnimputer.md) is the one member here that is
quadratic in the rows, which is why it is pinned and why the type itself refuses past 100 million
distance terms.

Against `scikit-learn` 1.9.1 and `numpy` 2.5.3 through `compare-transformers`, one run of each
side, milliseconds per operation, best of five. **Ratios above 1 mean Lodestar is faster; several
are below it, and that is the finding.**

| n | operation | Lodestar | `scikit-learn`, wall / cpu | ratio, wall | ratio, cpu |
| ---: | --- | ---: | ---: | ---: | ---: |
| 10,000 | `Normalizer` | **0.188 ms** | 0.196 / 0.196 ms | **1.04** | **1.03** |
| 10,000 | `PolynomialFeatures` | 0.588 ms | **0.492** / 0.492 ms | 0.84 | 0.83 |
| 10,000 | `KBinsDiscretizer`, fit | **0.964 ms** | 1.147 / 1.147 ms | **1.19** | **1.19** |
| 10,000 | `KBinsDiscretizer`, transform | **0.261 ms** | 2.661 / 2.660 ms | **10.19** | **8.12** |
| 10,000 | `QuantileTransformer`, fit | **1.034 ms** | 2.312 / 2.311 ms | **2.24** | **2.24** |
| 10,000 | `QuantileTransformer`, transform | 1.106 ms | **0.982** / 0.982 ms | 0.89 | 0.89 |
| 10,000 | `PowerTransformer`, fit | **14.035 ms** | 38.040 / 38.033 ms | **2.71** | **2.71** |
| 10,000 | `PowerTransformer`, transform | 0.569 ms | **0.453** / 0.452 ms | 0.80 | 0.79 |
| 10,000 | `KnnImputer`, transform | 28.056 ms | **16.730** / 200.865 ms | 0.60 | **7.13** |
| 10,000 | `LabelEncoder` | 0.683 ms | **0.445** / 0.445 ms | 0.65 | 0.65 |
| 100,000 | `Normalizer` | 1.211 ms | **1.067** / 1.067 ms | 0.88 | 0.69 |
| 100,000 | `PolynomialFeatures` | **4.289 ms** | 4.539 / 4.533 ms | **1.06** | 0.87 |
| 100,000 | `KBinsDiscretizer`, fit | 9.622 ms | **4.619** / 4.619 ms | 0.48 | 0.45 |
| 100,000 | `KBinsDiscretizer`, transform | **3.252 ms** | 10.622 / 10.620 ms | **3.27** | **2.61** |
| 100,000 | `QuantileTransformer`, fit | **9.903 ms** | 11.505 / 11.503 ms | **1.16** | **1.08** |
| 100,000 | `QuantileTransformer`, transform | 10.161 ms | **9.145** / 9.143 ms | 0.90 | 0.86 |
| 100,000 | `PowerTransformer`, fit | **147.715 ms** | 272.278 / 272.239 ms | **1.84** | **1.69** |
| 100,000 | `PowerTransformer`, transform | 4.838 ms | **3.062** / 3.061 ms | 0.63 | 0.58 |
| 100,000 | `KnnImputer`, transform | 29.016 ms | **13.597** / 163.092 ms | 0.47 | **5.63** |
| 100,000 | `LabelEncoder` | 8.846 ms | **4.674** / 4.674 ms | 0.53 | 0.51 |

**Where this package wins, it wins on the work rather than on the loop.** The discretizer's
transform is a binary search per value against a `numpy.digitize` that builds an index array; the
power fit is Brent's method against `scipy.optimize.brent` driven from Python, which is why the fit
is ahead and the transform — one `Math.Pow` per value against a vectorised `numpy.power` — is
behind.

**Where it loses, it loses to vectorised C, and the losses are where a single array operation does
the whole job**: [`PolynomialFeatures`](../reference/preprocessing/transforming/polynomialfeatures.md) multiplies column by column
there, [`LabelEncoder`](../reference/preprocessing/encoding/labelencoder.md) is one `numpy.unique`, and the quantile map is one `numpy.interp`. Roughly a factor of two at 100,000
rows, which is what a scalar managed loop costs against SIMD C over a contiguous array.

**[`KnnImputer`](../reference/preprocessing/encoding/knnimputer.md) is the row to read twice.** It is behind on elapsed time and **5.6× ahead on
processor time**, because `scikit-learn`'s pairwise distances run on every core through joblib and
this runs on one: the reference spends 163 ms of CPU to finish in 13.6 ms. This one is a
parallelism gap, not an arithmetic one, and it is the honest candidate for its own `perf/` issue.

[`KBinsDiscretizer`](../reference/preprocessing/transforming/kbinsdiscretizer.md)'s fit at 100,000 rows is the other
one: it sorts each column with
`Array.Sort` against numpy's introsort over a contiguous buffer, and 0.48× is about the constant
factor that costs.

## Lodestar.Stats

### Lodestar.Stats against Accord.Statistics (issue #1121) — the variance, proportion and fit tests

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

**[`Binomial.Test`](../reference/stats/tests/binomial-test.md) is 3.4× ahead at a hundred trials
and 445× at ten thousand, allocating 48 bytes against 200 kilobytes.** That is a difference in
what is computed, not in how well: `Accord` sums the binomial mass term by term, which is `O(n)`
and allocates an array of it, where this evaluates the regularized incomplete beta that sum *is*,
in constant time and constant space. The gap therefore widens with every further order of
magnitude.

[`Levene.Test`](../reference/stats/tests/levene-test.md) is 2.0× ahead at a hundred values and
9.9× at ten thousand. Its default centre is the median, which it takes by Hoare selection rather
than by sorting — `O(n)` against `O(n log n)`, and at this scale the sort was the test.
[`Bartlett.Test`](../reference/stats/tests/bartlett-test.md) is a steady 1.9× to 2.0× at both
sizes, on half the allocation.

**[`AndersonDarling.Test`](../reference/stats/tests/andersondarling-test.md) is the one row behind
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

### Lodestar.Stats against Meta.Numerics (issue #1120) — the three correlation tests

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

**[`KendallTau.Test`](../reference/stats/tests/kendalltau-test.md) is the headline: 2.0× ahead at
100 pairs and 126× at 10,000, allocating nothing at either size.** That is not a constant factor.
The definition counts concordant and discordant pairs in a double loop; this orders the pairs by
the first sample and counts the inversions of the second with a merge sort, so the work grows as
`n log n` where `Meta.Numerics` grows as `n²` — at ten thousand pairs, a hundred and thirty
thousand comparisons against fifty million. The gap therefore widens with every further order of
magnitude rather than closing.

[`Spearman.Test`](../reference/stats/tests/spearman-test.md) is 1.17× ahead at 100 and 1.36× at
10,000, while allocating 2.0× and 2.5× more: it materialises both rank arrays where
`Meta.Numerics` works off one sorted copy.

**[`Pearson.Test`](../reference/stats/tests/pearson-test.md) is the one row behind — 1.25× at 100
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
([decision 0005](../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)),
not an optimisation, and it has not been taken. This package allocates nothing here where
`Meta.Numerics` allocates 168 B per call.

### Lodestar.Stats against Accord.Statistics (issue #442)

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
[`TTest.Independent`](../reference/stats/tests/ttest-independent.md) (30× at 100 samples, narrowing
to 3.2× at 10,000, as `Accord`'s fixed per-call overhead is amortised over more work) and on
[`MannWhitney.Test`](../reference/stats/tests/mannwhitney-test.md) (5.1× at 100, 2.9× at 10,000,
allocating 61-62% less at both sizes — both sides take the guarded asymptotic path at 10,000, past
`MannWhitney`'s own `20_000`-product exact-method bound). Those two `MannWhitney` rows predate
[#711](https://github.com/CyrilB1531/lodestar/issues/711), after which the ranking no longer
allocates the pooled arrays; that allocation column measures a path this package no longer ships
and has not been re-measured against `Accord`. `Accord` is faster on
[`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) (roughly 380 ns against 294 ns, flat with
sample size since a 2×2 table has four cells regardless of how many observations produced it) — the
one family where this package's richer result (`Chi2ContingencyResult` carries the expected-value
table; `Accord`'s `ChiSquareTest` does not expose one) costs more than it buys at this shape.

**The chi-square row has since reversed, on a different machine.** The tail reads an integer or
half-integer degree of freedom up to 100 as a finite sum rather than iterating the incomplete
gamma's continued fraction, and
[`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) reads **66.52 ns** at 100 samples and
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

### Meta.Numerics against Lodestar.Stats and PrincipalComponentVariance (issue #756)

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
| [`FisherExact.Test`](../reference/stats/tests/fisherexact-test.md), 2×2 table | **256 ns** | **3.49×** |
| [`KolmogorovSmirnov.TwoSample`](../reference/stats/tests/kolmogorovsmirnov-twosample.md), n = m = 100 | **1,544 ns** | **1.47×** |

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

| shape | [`PrincipalComponentVariance.Compute`](../reference/decomposition/factorization/principalcomponentvariance-compute.md) | Meta.Numerics `PrincipalComponentAnalysis` | ratio | allocated |
| --- | ---: | ---: | ---: | ---: |
| 200 × 10 | **15.42 μs** | 565.95 μs | **36.70** | 2.38 KB / 329.71 KB |
| 2,000 × 10 | **86.25 μs** | 69,772.02 μs | **808.97** | 2.38 KB / **31,414.98 KB** |
| 2,000 × 50 | **1,953.85 μs** | 337,025.46 μs | **172.50** | 42.07 KB / 32,054.48 KB |

Both report the same first component's variance fraction, checked to `1e-9` relative before either
was timed. **Meta.Numerics refuses the fourth shape**: 100 rows by 200 features raises
`InsufficientDataException`, where this package and NumFlat both answer — so the wide matrix has no
Meta.Numerics row at all rather than a slow one.

## Lodestar.Stats.Regression

### Lodestar.Stats.Regression against Accord.Statistics (issue #566)

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
[`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md)
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

### Lodestar.Stats.Regression's generalized linear model against Accord.Statistics (issue #678)

Full method, and why the untyped `GeneralizedLinearRegression` is the one driven:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#27-lodestarstatsregressions-generalized-linear-model-against-accordstatistics-issue-616).
**Accord.Statistics 3.8.0 is archived.** Its last package shipped on 2017-10-19 and the repository
was archived on 2020-11-18 ([`docs/migration/README.md`](../migration/README.md)). A lead over it is
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
[`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md) call the
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

### The least-squares pipeline against Math.NET Numerics (issue #782)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#36-weighted-least-squares-against-mathnet-numerics-and-the-least-squares-pipeline-under-it-issue-782).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime, AVX-512.
Window: one `BenchmarkDotNet` 0.14.0 run, **default job**, on 2026-09-15. `MathNet.Numerics` 5.0.0, `Accord.Statistics` 3.8.0. Every pair returned the same slope before it
was timed.

#### Weighted least squares against `WeightedRegression.Weighted`

| n | [`WeightedLeastSquares.Fit`](../reference/stats-regression/wls/weightedleastsquares-fit.md) | Math.NET | Math.NET / Lodestar | Allocated, Lodestar | Allocated, Math.NET |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 200 | **6.650 μs** | 9.865 μs | **1.48** | 3.35 KB | 59.08 KB |
| 2,000 | **54.601 μs** | 54.718 μs | 1.00 | 17.41 KB | 509.88 KB |
| 20,000 | **0.591 ms** | 0.923 ms | **1.56** | 158.41 KB | 5,033.56 KB |
| 200,000 | **5.718 ms** | 11.072 ms | **1.94** | 1,564.58 KB | 50,035.09 KB |

**Level at 2,000 rows and faster everywhere else, while computing the whole table** — Math.NET
returns the coefficients alone — and allocating 17× to 32× less.

#### Ordinary least squares against `MultipleRegression.QR` and Accord

| n | [`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md) | Math.NET | Accord | Math.NET / Lodestar | Allocated, Lodestar |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 100 | **3.938 μs** | 24.096 μs | 135.345 μs | **6.12** | 2.57 KB |
| 10,000 | **262.402 μs** | 860.391 μs | 2,677.370 μs | **3.28** | 79.91 KB |

At 10,000 rows the robust covariances on top of that fit cost 0.628–0.631 ms for HC0 to HC3, and
6.27–6.31 μs at 100 rows (`RobustCovarianceBenchmarks`).

### Generalized least squares against Math.NET Numerics (issue #771)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#38-generalized-least-squares-against-mathnet-numerics-issue-771).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-15, the run taken after GLS was
rebased on the shared least-squares pipeline of
[#782](https://github.com/CyrilB1531/lodestar/issues/782).
`MathNet.Numerics` 5.0.0. Four regressors and an intercept, AR(1) errors at 0.6; each pair returned the same
slope within `1e-9` before it was timed.

| n | [`GeneralizedLeastSquares.Fit`](../reference/stats-regression/gls/generalizedleastsquares-fit.md) | Math.NET, Cholesky and normal equations | Math.NET / Lodestar | Allocated, Lodestar | Allocated, Math.NET |
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

### The negative binomial GLM against statsmodels (issue #781)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#37-the-negative-binomial-glm-against-statsmodels-issue-781).
**No free .NET library fits this family at parity** — Accord's IRLS is right only for canonical links, and commercial
libraries are not timed under a trial licence — so the incumbent is `statsmodels` 0.15.0, through the cross-language
harness. Machine: the AMD Ryzen 7 8700G named above, on 2026-09-15; the Python side is one run of
`bench_stats.py` on the same machine. Milliseconds per fit, best of five, `α = 1`, four regressors
and an intercept. The coefficients agree to 1.3e-14 relative or better with the same iteration
counts.

| n | [`GeneralizedLinearModel.Fit`](../reference/stats-regression/glm/generalizedlinearmodel-fit.md) | `statsmodels`, wall | `statsmodels`, cpu | statsmodels / Lodestar, wall |
| ---: | ---: | ---: | ---: | ---: |
| 1,000 | **0.404 ms** | 1.437 ms | 1.437 ms | **3.56** |
| 10,000 | **4.136 ms** | 5.604 ms | 5.603 ms | **1.35** |
| 100,000 | **45.520 ms** | 74.138 ms | 1,174.297 ms | **1.63** |

The log-likelihood reads `lnΓ(1/α)` once rather than once per row — a few dozen logarithms per fit.
At 100,000 rows `statsmodels` reaches LAPACK through numpy's threads and spends sixteen times the
processor time it takes in wall clock; the `wall` column is still the comparison.

### The Gamma GLM against statsmodels, and the IRLS loop it widened (issue #770)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#39-the-gamma-glm-against-statsmodels-issue-770).
**No free .NET library fits this family** — Accord's generalized linear regression returned NaN coefficients on the
inverse link — so the incumbent is `statsmodels` 0.15.0, through the cross-language harness.
Machine: the AMD Ryzen 7 8700G named above. Window: `BenchmarkDotNet` 0.14.0 and the harness,
**default job**, on 2026-09-15; the Python side one run of `bench_stats.py`. Log link, four
regressors and an intercept; milliseconds per fit, best of five.

| n | [`GeneralizedLinearModel.Fit`](../reference/stats-regression/glm/generalizedlinearmodel-fit.md), Gamma | `statsmodels`, wall | `statsmodels`, cpu | statsmodels / Lodestar, wall |
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

### HAC and cluster-robust covariances against statsmodels (issue #775)

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

### GLM offsets and exposure against statsmodels (issue #787)

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

### The multinomial logit against Accord and statsmodels (issue #788)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#42-the-multinomial-logit-against-accord-and-statsmodels-issue-788).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, default job, one run. Three regressors
and an intercept,
three categories.

| rows | [`MultinomialLogit.Fit`](../reference/stats-regression/mnlogit/multinomiallogit-fit.md) | Accord `LowerBoundNewtonRaphson`, tolerance `1e-10` | Accord / Lodestar | Allocated, Lodestar / Accord |
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

## Lodestar.Stats.TimeSeries

### Stationarity and seasonal decomposition against Cortex.TimeSeries (issue #671)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#35-stationarity-and-seasonal-decomposition-against-cortextimeseries-issue-671).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-15. `Cortex.TimeSeries` 1.1.0. Each
pair returned the same statistic before it was timed.

| n | function | [`Lodestar.Stats.TimeSeries`](../reference/stats-timeseries/stationarity-tests.md) | `Cortex.TimeSeries` | Cortex / Lodestar | Allocated, Lodestar | Allocated, Cortex |
| ---: | --- | ---: | ---: | ---: | ---: | ---: |
| 200 | ADF, lag 4 | **6.987 μs** | 15.819 μs | **2.26** | 20.68 KB | 16.18 KB |
| 200 | KPSS, level | 2.219 μs | 2.328 μs | 1.05 | 1.69 KB | 3.43 KB |
| 200 | decomposition, period 12 | **0.810 μs** | 1.499 μs | **1.85** | 6.50 KB | 8.16 KB |
| 2,000 | ADF, lag 4 | **101.382 μs** | 181.199 μs | **1.79** | 203.51 KB | 142.76 KB |
| 2,000 | KPSS, level | 59.554 μs | 60.223 μs | 1.01 | 15.75 KB | 31.55 KB |
| 2,000 | decomposition, period 12 | **8.253 μs** | 15.391 μs | **1.86** | 62.75 KB | 78.48 KB |

**The augmented Dickey-Fuller test is 1.79× to 2.26× faster than Cortex's.** Its lag search fits
through
[`OrdinaryLeastSquares.Estimate`](../reference/stats-regression/ols/ordinaryleastsquares-estimate.md),
which applies the reflections to the response and stops at the coefficients, their standard errors
and the residual sum of squares, rather than forming Q explicitly and pricing variance inflation
factors and a Student quantile per coefficient to hand back one t statistic. It allocates more than
Cortex at 2,000 points: the design and its column-major copy. The p-value stays what the test is
for — Cortex's is clamped at `0.01`, where MacKinnon's surface gives `1.1e-9` to `9.5e-4` on the
regressions checked.

**The decomposition is 1.85× faster**: the moving average is a running
sum over each window's interior rather than a weighted pass over the whole window, `O(n)` against
`O(n·period)`. **KPSS is level**, with half the allocation; its cost is the lagged products of the
long-run variance, which both sides compute.

### Lodestar.Stats' serial-correlation diagnostics against Cortex.TimeSeries (issue #617)

Full method, and why the partial autocorrelations of the two libraries differ:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#28-lodestarstatstimeseriess-serial-correlation-diagnostics-against-cortextimeseries-issue-617).

Machine: the AMD Ryzen 7 8700G named above. Window: one `BenchmarkDotNet` run, **default job**, on 2026-09-13,
12 benchmarks, 20 lags throughout.

| Method | SampleSize | Mean | Ratio | Allocated |
| --- | ---: | ---: | ---: | ---: |
| `LodestarAutocorrelation` | 200 | 18.582 μs | 1.00 | 1,000 B |
| `CortexAutocorrelation` | 200 | 7.308 μs | 0.39 | 192 B |
| `LodestarPartialAutocorrelation` | 200 | 18.829 μs | 1.01 | 1,192 B |
| `CortexPartialAutocorrelation` | 200 | 7.696 μs | 0.41 | 768 B |
| `LodestarLjungBox` | 200 | 8.407 μs | 0.45 | 720 B |
| `CortexLjungBox` | 200 | 7.375 μs | 0.40 | 224 B |
| `LodestarAutocorrelation` | 2,000 | 88.121 μs | 1.00 | 1,000 B |
| `CortexAutocorrelation` | 2,000 | 76.709 μs | 0.87 | 192 B |
| `LodestarPartialAutocorrelation` | 2,000 | 88.041 μs | 1.00 | 1,192 B |
| `CortexPartialAutocorrelation` | 2,000 | 77.123 μs | 0.88 | 768 B |
| `LodestarLjungBox` | 2,000 | 77.513 μs | 0.88 | 720 B |
| `CortexLjungBox` | 2,000 | 76.915 μs | 0.87 | 224 B |

**The gap is one function call.** Lodestar's ACF and PACF are 11.3 and 11.1 μs slower at 200
points and 11.4 and 10.9 μs slower at 2,000 — constant while the series grows tenfold, which a
kernel difference could not be. Both compute the confidence band `Cortex.TimeSeries` does not,
through one `NormalQuantile` call, and the Ljung-Box pair, which has no band, is 1.14× at 200
points and **level at 2,000**. Subtracting the 11.5 μs quantile leaves 7.1 μs against Cortex's
7.3 at 200 points and 76.6 μs against 76.7 at 2,000. The allocation difference is the band and
the result record that carries it, and
[#709](https://github.com/CyrilB1531/lodestar/issues/709) confirmed it by removing that cost:
without the bisection the pairs are level.

### The vector autoregression against statsmodels (issue #786)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#43-the-vector-autoregression-against-statsmodels-issue-786).
Nothing in .NET estimates a VAR, so the incumbent is `statsmodels` 0.15.0 through `compare-var`.
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16; one run of each side. A two-variable series at lag 2, milliseconds per fit, best of five.

| n | [`VectorAutoregression.Fit`](../reference/stats-timeseries/var/vectorautoregression-fit.md) | `statsmodels`, wall / cpu | ratio, wall |
| ---: | ---: | ---: | ---: |
| 1,000 | **0.042 ms** | 1.263 / 1.263 ms | **30.23** |
| 10,000 | **0.485 ms** | 8.565 / 8.564 ms | **17.65** |
| 100,000 | **5.391 ms** | 79.616 / 79.609 ms | **14.77** |

What the fit costs by shape, from `VectorAutoregressionBenchmarks`, `BenchmarkDotNet` 0.14.0 default job:

| observations | variables | lags | mean | Allocated |
| ---: | ---: | ---: | ---: | ---: |
| 500 | 2 | 1 | 14.76 μs | 60.53 KB |
| 500 | 2 | 4 | 49.28 μs | 132.84 KB |
| 500 | 5 | 1 | 72.24 μs | 206.76 KB |
| 500 | 5 | 4 | 422.41 μs | 590.05 KB |
| 5,000 | 2 | 1 | 244.97 μs | 587.95 KB |
| 5,000 | 2 | 4 | 829.14 μs | 1,293.21 KB |
| 5,000 | 5 | 1 | 911.36 μs | 2,000.01 KB |
| 5,000 | 5 | 4 | 4,701.02 μs | 5,548.08 KB |

Both axes multiply: a system of `K` variables at lag `p` fits `K` least squares over a design of `1 + K·p` columns.

## Lodestar.Gpu

### Lodestar.Gpu — four kernels against their CPU paths (issue #444)

Measured 2026-09-10, on the one machine `bench/README.md`'s GPU gate
says a GPU figure has to be published beside:

| | |
| --- | --- |
| GPU | **NVIDIA GeForce RTX 5070 Ti**, 15 877 MB, CUDA SM_120 |
| CPU | AMD Ryzen 7 8700G, 8 physical / 16 logical cores, AVX-512 |
| OS / runtime | Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 |
| ILGPU | 1.5.3, CUDA backend |
| BenchmarkDotNet job | `--job short` — 3 iterations, 3 warmups, 1 launch |

**Read this before the numbers.** Three caveats travel with every table below.

1. **The baseline is single-threaded.** bench/README.md's GPU gate asks a kernel to be priced against *this
   repository's own path*, and none of those paths is parallel. A `Parallel.For` over eight cores
   would close a large part of every gain here, most of all Myers'. These are honest against the
   gate as written; they are not a claim about a parallel CPU implementation.
2. **`--job short` is three iterations.** The `GpuResident` rows sit at 0.7–7 % standard error,
   which carries a verdict. Some `GpuFromHost` rows do not — one shows a confidence interval wider
   than its own mean — and those are read as orders of magnitude, not as figures.
3. **The first run of these benchmarks measured the wrong device**, and the numbers below are the
   second. `Context.GetPreferredDevice(preferCPU: false)` returned an OpenCL runtime that executes
   on the processor — ILGPU enumerated `cpu-skylake-avx512-AMD Ryzen 7 8700G` as an OpenCL device
   beside the CUDA card — and a check on the accelerator *type* called it a GPU. `GpuContext` now
   orders the devices itself and asks OpenCL for the device type, and the device's name is a
   column in every table so the mistake cannot repeat silently.

#### Tiled cosine + top-k, against [`EmbeddingIndex.Search`](../reference/embeddings/search/embeddingindex-search.md)

384 dimensions, top-10, seeded corpus. `GpuResident` is the gate row: query transfer and result
read-back are inside the measurement, the corpus is not.

| Documents | Queries | `SimdBaseline` | `GpuResident` | gain | `GpuFromHost` |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 10 000 | 1 | 759.4 µs | 219.2 µs | 3.5× | 11.9 ms |
| 100 000 | 1 | 9.32 ms | 1.41 ms | **6.6×** | 86.2 ms |
| 10 000 | 256 | 197.5 ms | 18.4 ms | **10.8×** | 29.9 ms |
| 100 000 | 256 | 2 356.9 ms | 178.2 ms | **13.2×** | 278.6 ms |

**Clears the 5–10× gate at three sizes of four**, and misses at the smallest corpus with a single
query — which is where the crossing point belongs and why the sizes bracket it.

**`GpuFromHost` is the argument for residency, and it is brutal.** At 100 000 documents and one
query it is **9.25× slower than the CPU path**: 147 MB of corpus uploaded to answer one question.
At 256 queries the same upload amortises and the row returns to 8.5× faster than the CPU. Below
roughly a hundred queries per corpus, a GPU package that uploads per call is a pessimisation.

Allocation is the secondary win: 434 KB against the SIMD path's 200 MB at the largest size, a
ratio of 0.002, because the kernel returns *k* hits rather than a score per document.

#### Sparse-dense product, against [`CsrMatrix.Multiply`](../reference/abstractions/sparse/csrmatrix-multiply.md)

20 000 terms at 0.2 % density, double precision on both sides.

| Documents | Width | `CpuBaseline` | `GpuResident` | gain |
| ---: | ---: | ---: | ---: | ---: |
| 5 000 | 64 | 7.92 ms | 4.54 ms | 1.74× |
| 5 000 | 256 | 38.4 ms | 11.1 ms | 3.45× |
| 50 000 | 64 | 71.5 ms | 8.25 ms | **8.66×** |
| 50 000 | 256 | 377.5 ms | 20.0 ms | **18.9×** |

`bench/README.md` predicted this kernel might miss the gate on FP64, since a consumer card runs
double precision at a fraction of single. **That was wrong, and the reason is worth keeping**:
sparse-dense is bound by memory bandwidth, not by the double-precision units, so the reduced FP64
rate never becomes the constraint. It clears the gate at both realistic vectorizer sizes.

The cost it does carry is allocation — **5× the CPU path** at the small sizes, from the staged
upload and the downloaded result.

#### Myers bit-parallel edit distance, against [`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md)

24-character pattern, 26-letter alphabet, seeded corpus.

| Texts | Text length | `CpuBaseline` | `GpuResident` | gain | `GpuFromHost`, 2026-09-10 | `GpuFromHost`, 2026-09-17 |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 10 000 | 32 | 1 600.7 µs | 57.4 µs | **27.9×** | 2 943 µs | 421 µs |
| 10 000 | 256 | 7 585.8 µs | 121.6 µs | **62.4×** | 21 277 µs | 1.65 ms |
| 200 000 | 32 | 32.07 ms | 220.2 µs | **145.7×** | 55.5 ms | 5.27 ms |
| 200 000 | 256 | 153.4 ms | 1.19 ms | **129.4×** | 450.0 ms | 37.3 ms |

**This was written expecting a failure and is the largest gain of the four.** The reasoning behind
the prediction — that Myers is bit-parallel on both sides, so the CPU already spends tens of
nanoseconds per short pair — was true and did not settle it: the baseline is *one thread* and the
kernel is tens of thousands, over pairs with no dependency between them. Caveat 1 above bites
hardest here.

`GpuFromHost` runs **4.1× to 6.5× below the CPU path**: renaming the batch is a 64 KB code table on
the host rather than a dictionary probe per character
([#853](https://github.com/CyrilB1531/lodestar/issues/853)). That column was measured on 2026-09-17,
same machine and `--job short`, with a `CpuBaseline` of 1.73 ms, 8.08 ms, 34.4 ms and 162 ms in the
same window; the other columns are the 2026-09-10 run.

#### MinHash signatures, against [`MinHash.Signature`](../reference/text/similarity/minhash-signature.md)

24 tokens per document over a vocabulary of 5 000, seeded corpus, the same coefficients on both
sides. Three rows because the two sides divide the work differently: the CPU path hashes and
minimises in one pass, while the kernel takes hashes the host already computed.

| Documents | Permutations | `CpuBaseline` | `GpuResident` | gain | `GpuWithHashing` | gain |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 5 000 | 64 | 63.2 ms | 0.95 ms | 66× | 49.6 ms | **1.27×** |
| 5 000 | 128 | 79.8 ms | 1.94 ms | 41× | 50.8 ms | **1.57×** |
| 50 000 | 64 | 637.1 ms | 15.1 ms | 42× | 503.2 ms | **1.27×** |
| 50 000 | 128 | 779.6 ms | 23.2 ms | 34× | 537.4 ms | **1.45×** |

**`GpuWithHashing` is the row that matters, and it is the one that misses the gate.** The
minimisation is 34× to 66× faster on the accelerator; a caller starting from tokens sees 1.27× to
1.57×, because hashing is most of the work and it stays on the host. This kernel clears the GPU gate on the part it took and misses it on the part a caller experiences.

Located rather than disappointing: the next move is to hash on the accelerator, which is a separate
kernel because parity requires the first four bytes of SHA-1 little-endian and a device
implementation agreeing bit for bit is its own piece of work. Until then the kernel earns its place
for a caller who **already holds hashes** — sketching one corpus under several permutation sets, for
instance, where hashing is paid once and minimisation many times.

#### What residency buys across two operations

Two sparse-dense products; `RoundTripped` is the baseline because the question is what an
intermediate costs when it crosses the bus, not whether the accelerator wins.

| Rows | Width | `RoundTripped` | `Chained` | gain | `CpuBaseline` |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 2 000 | 32 | 1 234.7 µs | 552.6 µs | 2.23× | 1 795.9 µs |
| 2 000 | 128 | 5 372.1 µs | 1 830.1 µs | 2.94× | 8 325.9 µs |
| 20 000 | 32 | 1 990.5 µs | 1 602.4 µs | 1.24× | 6 324.3 µs |
| 20 000 | 128 | 6 288.9 µs | 3 490.4 µs | 1.80× | 25 519.8 µs |

`bench/README.md` predicted the gap would be roughly flat in `Rows`. **It is not — it shrinks**,
2.23× to 1.24× at width 32, and the arithmetic never predicted otherwise: a fixed transfer against
work that grows with `Rows` is a falling *share* of the total. The prediction in `Width` holds.

The reading for a caller inverts the usual intuition: **residency is worth most where the work is
smallest.** A large job amortises a round trip on its own; a small one does not.
