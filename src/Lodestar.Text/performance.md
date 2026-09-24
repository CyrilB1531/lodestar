# Performance — Lodestar.Text

What `Lodestar.Text` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## [`IndelPattern`](../../docs/reference/text/distances/indelpattern.md) against FuzzySharp (issue #1130)

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

## [`Levenshtein.Distance`](../../docs/reference/text/distances/levenshtein-distance.md) against rapidfuzz

[`Levenshtein.Distance`](../../docs/reference/text/distances/levenshtein-distance.md) against rapidfuzz
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

## `Indel` and `fuzz.ratio` against rapidfuzz

`Indel` is `len(a) + len(b) - 2·LCS`, so what runs is
[`Lcs.SubsequenceLength`](../../docs/reference/text/distances/lcs-subsequencelength.md) — and that is also
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

## [`TfidfVectorizer`](../../docs/reference/text/vectorizers/tfidfvectorizer.md) against ML.NET 5.0.0's `FeaturizeText`

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

## BM25 against LuceneSharp (issue #677)

[`Bm25Index`](../../docs/reference/text/search/bm25index.md) against LuceneSharp.Core 26.8.4415's
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

| Documents | [`CountVectorizer.FitTransform`](../../docs/reference/text/vectorizers/countvectorizer-fittransform.md) | `new Bm25Index(counts)` | [`Bm25Index.Score`](../../docs/reference/text/search/bm25index-score.md) |
| ---: | ---: | ---: | ---: |
| 1,000 | 9.39 ms, 20.4 MB | 0.360 ms, 0.46 MB | 1.3 μs |
| 20,000 | 175.6 ms, 399.6 MB | 3.233 ms, 8.97 MB | 10.1 μs |

**For a caller who already holds the matrix** — one who has vectorized for something else, TF-IDF
or a classifier — building `Bm25Index` and answering `q` queries costs `0.36 ms + 1.22 μs·q` at
1,000 documents against Lucene's `4.45 ms + 1.94 μs·q`, cheaper at every `q`. At 20,000 documents
it is `3.23 ms + 15.2 μs·q` against `86.3 ms + 10.6 μs·q`, which crosses near 18,000 queries. From
text Lucene stays ahead from the first query. These crossings mix `Stopwatch` build figures with
`BenchmarkDotNet` query figures, so read them as orders of magnitude.

## BK-tree vs a length-filtered scan (issue #526)

**No .NET package publishes a BK-tree**, so the alternative is the loop a caller would otherwise
write: `BkTreeBenchmarks.LengthFilteredScan`, a linear scan that skips any word whose length
already rules it out, then calls
[`Levenshtein.Distance`](../../docs/reference/text/distances/levenshtein-distance.md) on what survives.
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
[`docs/guides/dictionary-lookup.md`](../../docs/guides/dictionary-lookup.md) carries the reader-facing version: the
tree is worth using at `k = 1`, and a length-filtered scan is the better answer past it.
