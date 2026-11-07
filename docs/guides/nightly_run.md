# Nightly benchmark run

<!-- nightly-baseline: 3e48e17c68bff96a3371172656990689433f479e -->
<!-- nightly-owed: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
<!-- nightly-cut: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `3e48e17c68bff96a3371172656990689433f479e`
- Previous run: `3e48e17c68bff96a3371172656990689433f479e`
- Runner: Linux / X64 (GitHub hosted)

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
