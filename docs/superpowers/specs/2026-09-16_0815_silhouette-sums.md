# 0815 — `Silhouette.PerSample` without the distance matrix

**Status:** **retrospective** — written 2026-09-16, after the measurement it records.

Issue: [#815](https://github.com/CyrilB1531/lodestar/issues/815), found by a performance review of `main`.

## Change

`Silhouette.PerSample` sums each pair's distance per cluster instead of holding the n × n distance matrix.

## Measured

| n (16 features, 8 clusters) | `main` | fix |
| ---: | ---: | ---: |
| 2,000 | 23.7 ms, 30.5 MB | 19.8 ms, 150 KB |
| 5,000 | 146 ms, 191 MB | **103 ms, 372 KB** |

Each sum still receives its terms by ascending index: bit-identical on 1,750 scores. `SilhouetteBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
