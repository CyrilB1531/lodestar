# 0811 — `LogRank.Test` in one sorted walk

**Status:** **retrospective** — written 2026-09-16, after the measurement it records.

Issue: [#811](https://github.com/CyrilB1531/lodestar/issues/811), found by a performance review of `main`.

## Change

`LogRank.Test` sorts each arm once and walks it with the event times, where it rescanned both arms at every time.

## Measured

| n | distinct times | `main` | fix |
| ---: | ---: | ---: | ---: |
| 1,000 | 100% | 606 µs | 17.0 µs |
| 100,000 | 4% | 1.60 s | 8.79 ms |
| 100,000 | 100% | 19.25 s | **10.24 ms** |

Same times, same integer counts, same accumulation order: bit-identical. `SurvivalBenchmarks.LogRankTest`; allocation rises to about 1.7 MB at 100,000 subjects for the sorted copies.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
