# 0812 — The contingency table's cell key, mixed before hashing

**Status:** **retrospective** — written 2026-09-16, after the measurement it records.

Issue: [#812](https://github.com/CyrilB1531/lodestar/issues/812), found by a performance review of `main`.

## Change

The clustering agreement scores spread their contingency cells across the hash table, where a 100 × 100 table's cells shared 128 hash values.

## Measured

| clusters (n = 100,000) | `main` | fix |
| ---: | ---: | ---: |
| 10 | 2.70 ms | 1.38 ms |
| 100 | 20.9 ms | **1.95 ms** |

The key is multiplied by an odd constant modulo 2^64 and inverted when read; insertion order, and so the order `MutualInformation` sums in, is unchanged. `ClusteringAgreementBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
