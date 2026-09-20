# 0844 — Six `Lodestar.Text` kernels without their per-term allocations

**Status:** **retrospective** — written 2026-09-17, after the measurement it records.

Issue: [#844](https://github.com/CyrilB1531/lodestar/issues/844), found by the performance review of `main` (2026-09-16).

## Problem

The review found per-term and per-posting work in `Lodestar.Text` that no result depends on: a string, a regex match and a dictionary per term in the vectorizers, a UTF-8 array (and on netstandard2.0 a hash algorithm) per token in the sketches, BM25's length normalisation recomputed per posting, OSA's full dynamic program with no affix trim, a `params` array per Double Metaphone rule, and a Ratcliff-Obershelp scan that continued past a run that could not grow.

## Change

- The analyzer hands each term to a struct sink as a span; `CountVectorizer` and `HashingVectorizer` count a row in a dense tally reset by its touched columns.
- `MinHash` and `SimHash` hash through one `TokenDigest` per call; the Mersenne modulo is folded without a division, and the affine-32 loop is vectorised.
- `Bm25Index` stores `K1 * (1 - B + B * |D| / avgdl)` per document at construction.
- `Osa` trims the common affixes and runs Hyyrö's (2003) bit-parallel OSA for a Latin-1 UTF-16 pattern of at most 64 units.
- `DoubleMetaphone.StringAt` takes `params ReadOnlySpan<string>`, and `IsSlavoGermanic` compares characters.
- `RatcliffObershelp`'s longest-match scan returns once its run spans the shorter operand.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| [`MinHash.Signature`](../../reference/text/similarity/minhash-signature.md), affine-32, 2,000 documents × 128 | 29.3 ms | **16.3 ms** |
| [`MinHash.Signature`](../../reference/text/similarity/minhash-signature.md), legacy, 2,000 documents × 128 | 33.5 ms | 28.4 ms |
| [`CountVectorizer.FitTransform`](../../reference/text/vectorizers/countvectorizer-fittransform.md), 1,000 documents | 4.51 ms | 2.71 ms |
| [`CountVectorizer.FitTransform`](../../reference/text/vectorizers/countvectorizer-fittransform.md), (1, 2)-grams, 1,000 documents | 7.66 ms | 4.02 ms |
| [`CountVectorizer.FitTransform`](../../reference/text/vectorizers/countvectorizer-fittransform.md), char word-boundary (2, 4), 1,000 documents | 22.8 ms | **8.97 ms** |
| [`HashingVectorizer.Transform`](../../reference/text/vectorizers/hashingvectorizer-transform.md), 1,000 documents | 4.37 ms | 2.69 ms |
| [`Bm25Index`](../../reference/text/search/bm25index.md) from text (count, index, query), 20,000 documents | 196 ms | 77.9 ms |
| [`Bm25Index.Top`](../../reference/text/search/bm25index-top.md), five terms, 20,000 documents | 27.7 µs | 22.2 µs |
| [`Bm25Index.Top`](../../reference/text/search/bm25index-top.md), one term, 1,000 documents | 1.13 µs | 1.19 µs |
| [`Osa.Distance`](../../reference/text/distances/osa-distance.md), UTF-16, length 64 | 5.85 µs | **201 ns** |
| [`Osa.Distance`](../../reference/text/distances/osa-distance.md), code points, length 64 | 5.97 µs | 4.40 µs |
| [`DoubleMetaphone.Encode`](../../reference/text/phonetics/doublemetaphone-encode.md), 1,000 surname-shaped strings | 210 µs, 917 KB | 145 µs, 489 KB |
| [`RatcliffObershelp.Similarity`](../../reference/text/distances/ratcliffobershelp-similarity.md), containment, 64 | 586 ns | 238 ns |

Terms reach the vocabulary or the hash as spans and each row is counted in a dense tally; the sketches reuse one UTF-8 buffer and vectorise the affine-32 loop; BM25 stores each document's length term once; OSA trims affixes and runs Hyyrö's bit-parallel kernel on a Latin-1 pattern of at most 64 units; Double Metaphone passes its candidates as a span; Ratcliff-Obershelp stops a scan whose run already spans the shorter operand. Integer arithmetic, or the same floating-point expression in the same order, so every result is bit-identical and the oracle corpora replay unchanged. `SimilaritySketchBenchmarks`, `VectorizerBenchmarks`, `Bm25Benchmarks`, `OsaBenchmarks`, `DoubleMetaphoneBenchmarks` and `RatcliffObershelpBenchmarks`, pinned to four cores.

## Rejected

- **TextRank stem cache (finding 11).** `TextRank.Extract` measured 1.01× and 0.98× with a 2-5% allocation drop, which is no gain.
- **LSH buckets hashed from the span (finding 12).** `SketchThenVerify` did not improve beyond what its signatures gained (0.98× and 1.07× at 2,000 documents), so the change bought nothing measurable.
- **Q-gram counts and Damerau-Levenshtein dense ids (findings 2 and 7).** Shipped separately in #829 and #828; TextRank's compaction (finding 1) in #816.
- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
