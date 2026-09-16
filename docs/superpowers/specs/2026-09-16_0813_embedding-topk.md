# 0813 — `EmbeddingIndex.Search` by a bounded heap

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#813](https://github.com/CyrilB1531/lodestar/issues/813), found by a performance review of `main`.

## Change

`EmbeddingIndex.Search` keeps the best k in a bounded heap instead of sorting every score, allocating k results rather than the whole index.

## Measured

| vectors (384 dimensions, k = 10) | `main` | fix |
| ---: | ---: | ---: |
| 10,000 | 833 µs, 80 KB | 369 µs, 1.6 KB |
| 100,000 | 10.4 ms, 783 KB | **3.89 ms, 1.6 KB** |

Same total order (score descending, index ascending); a k past half the index still sorts. A differential test checks every k against the full ranking. `EmbeddingSearchBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
