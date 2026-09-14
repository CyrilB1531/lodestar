---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0126 — The nightly reports a ratio that steps past its own noise, or drifts over ten days

**Status:** accepted · **Date:** 2026-09-14

## Context

[#672](https://github.com/CyrilB1531/lodestar/issues/672): three benchmark ratios got worse over three
weeks and nothing turned red, because nothing compared one night with the next.

- `docs/guides/nightly_run.md` is overwritten every run.
- `benchmark_latest.md` carries old sections forward.
- The run's artefact expires after seven days.

The three were found by walking the page's git history by hand:

- [#673](https://github.com/CyrilB1531/lodestar/issues/673): BPE against its unigram baseline, 0.97 to 1.72, a step.
- [#674](https://github.com/CyrilB1531/lodestar/issues/674): the code-point Indel at length 512, +21%
  over ten days, a drift no single night shows.
- [#675](https://github.com/CyrilB1531/lodestar/issues/675): the code-point Indel at length 20, 2.18 to 4.82, a step.

This record decides what counts as a movement worth reporting on a hosted runner, and how it
reaches a maintainer. The mechanism is `tools/nightly_series.py`, and the series is
`bench/nightly/ratios.csv`.

## The series the thresholds were measured on

Every BenchmarkDotNet `Ratio` cell, rebuilt from two sources:

- **the git history of `docs/guides/nightly_run.md`**, 24 revisions;
- **the wiki's own history of the page**, which adds four runs of commits on `main` whose pull
  requests never merged, the night of 2026-09-01 among them.

Two further wiki runs measured branch commits that no longer exist, and were left out.

The result is **3,415 readings over 23 runs, 18 of them full**, from 2026-08-19 to 2026-09-13.

**Only ratios are kept.** A hosted runner is a different VM every night, so an absolute mean is
not comparable across nights. A ratio against a baseline measured in the same minute is.

**The runners are not one machine.** Five processor models served those 26 days: AMD EPYC 7763
and 9V74, and three Intel Xeons. The CPU is recorded with every reading.

**The noise is heavy and uneven.** Measure a reading against the median of its key's previous five:

| percentile | 50th | 75th | 90th | 95th | 99th |
| --- | ---: | ---: | ---: | ---: | ---: |
| relative deviation | 1.0% | 5.6% | 14.4% | 26.1% | 92.8% |

The tail is concentrated in nanosecond kernels: `LevenshteinCodePointBenchmarks`, `LcsGateBenchmarks`
and `IndelBenchmarks`. A whole-corpus encode barely moves between VMs.

## Decision

**A reading is a movement when either rule fires.**

- **Step.** Its ratio differs from the median of its key's last five readings by more than
  **30%**, or by more than **four times that key's own noise**, whichever is larger. A key's
  noise is the median of its earlier readings' deviations from the medians before them.
- **Drift.** The median of its key's last three readings, tonight's included, differs by more
  than **20%** from the median of the last five readings at least **ten days** older.

A key needs three earlier readings before it is compared at all.

**A movement is new unless the key's previous reading was already one.** After a step, the median
lags for several nights, and the same movement is not headlined again each night.

**It reaches a maintainer three ways, and never fails the job.**

- **The page.** A section at the end of `nightly_run.md` lists the new movements, then the
  persisting ones.
- **The job.** The same section goes to the run's summary, and each new movement raises one `::warning::`.
- **The pull request.** Its body repeats the count.

The page is the product of the run. A red job would withhold the numbers the movement was found in,
and [#464](https://github.com/CyrilB1531/lodestar/issues/464) already separated "the measurement
failed" from "the publish failed" for that reason.

**Only a run on `main` records.** A branch compares against the series and adds nothing to it.
Each run folds in the wiki's history first, so a night whose pull request is still open is not
lost.

## The measurement behind the numbers

Each option below was replayed run by run over the series. "New" counts movements headlined as new.
"First caught" is the first new movement on each regression's key.

| option | new, total | new per full run, median / max | #673 | #674 | #675 |
| --- | ---: | ---: | --- | --- | --- |
| step past a flat 15% | 134 | 2 / 26 | 08-20 (noise) | 08-20 (noise) | 08-26 |
| step past a flat 30% | 53 | 1 / 15 | 09-01 | 08-20 (noise) | 08-26 |
| step past max(30%, 4 × noise) | 44 | 0.5 / 13 | 09-01 | **missed** | 08-26 |
| **step past max(30%, 4 × noise), or drift past 20% over ten days** | **68** | **1 / 21** | **09-01** | **08-31, drift** | **08-26** |
| step past max(20%, 4 × noise), or drift past 20% | 88 | 2 / 21 | 09-01 | 08-31, drift | 08-26 |
| step past max(30%, 4 × noise), or drift past 30% | 59 | 1 / 13 | 09-01 | 08-31, drift | 08-26 |

**The decided option is the cheapest that catches all three, on the run each appeared, for its
own reason.** The flat thresholds raise #673 or #674 on 2026-08-20, from noise, before either
regression existed.

- **#673** is caught on 2026-09-01, the first run after `8de0da96`, the commit its issue names.
- **#675** is caught on 2026-08-26.
- **#674** is caught on 2026-08-31, by the drift rule.

**The larger drift threshold lost on #674's own number.** A 30% drift catches #674 in this series
only because two early wiki readings sit low, at 28.6 and 29.5. The drift the issue measured
against a steady baseline is 21%, and a 20% threshold is the one that would catch it without them.

## Consequences

- `bench/nightly/ratios.csv` is committed, and grows by one run per night on `main`.
- A night of real change raises many movements: 21 on one run above, in a week of merged
  performance work. The rule reports that a ratio moved. Whether its baseline or its comparand
  moved, and whether that was wanted, is still a reader's job, and the section says so.
- A new processor model moves some ratios on its own, as #675's short lengths did on 2026-09-13.
  Such a movement is reported like any other. The CPU column is there to explain it, not to hide it.
- **Revisit these numbers** when the series is long enough to be measured again. The table above
  is 18 full runs from the busiest three weeks this repository has had.
