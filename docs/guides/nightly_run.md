# Nightly benchmark run

<!-- nightly-baseline: d294b0dd903e83f6f31a701c2d1a31ff6843cd14 -->
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

- Commit: `d294b0dd903e83f6f31a701c2d1a31ff6843cd14`
- Previous run: `d294b0dd903e83f6f31a701c2d1a31ff6843cd14`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `CosineTopKSelectionBenchmarks`

### Lodestar.Gpu.Benchmarks.CosineTopKSelectionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Documents | K    | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Allocated   | Alloc Ratio |
|------------- |---------- |----- |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|------------:|------------:|
| **SimdBaseline** | **10000**     | **10**   | **CPUAc(...) GPU) [26]** |       **9.871 ms** |      **1.6159 ms** |     **0.0886 ms** |     **1.00** |    **0.01** |    **26.02 KB** |        **1.00** |
| GpuResident  | 10000     | 10   | CPUAc(...) GPU) [26] |  27,407.531 ms |  7,134.8206 ms |   391.0836 ms | 2,776.82 |   40.53 |  1519.13 KB |       58.39 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **100**  | **CPUAc(...) GPU) [26]** |      **10.183 ms** |      **9.7291 ms** |     **0.5333 ms** |     **1.00** |    **0.06** |    **37.27 KB** |        **1.00** |
| GpuResident  | 10000     | 100  | CPUAc(...) GPU) [26] |  28,312.787 ms |  7,955.9133 ms |   436.0905 ms | 2,785.47 |  129.09 |  1541.63 KB |       41.37 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **1000** | **CPUAc(...) GPU) [26]** |      **14.496 ms** |      **0.9244 ms** |     **0.0507 ms** |     **1.00** |    **0.00** |   **149.77 KB** |        **1.00** |
| GpuResident  | 10000     | 1000 | CPUAc(...) GPU) [26] |  32,521.166 ms | 10,589.3107 ms |   580.4359 ms | 2,243.45 |   35.34 |   1765.7 KB |       11.79 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **10**   | **CPUAc(...) GPU) [26]** |     **138.959 ms** |      **6.1147 ms** |     **0.3352 ms** |     **1.00** |    **0.00** |    **26.25 KB** |        **1.00** |
| GpuResident  | 100000    | 10   | CPUAc(...) GPU) [26] | 269,907.774 ms | 56,422.9601 ms | 3,092.7328 ms | 1,942.36 |   19.70 | 39277.98 KB |    1,496.30 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **100**  | **CPUAc(...) GPU) [26]** |     **142.469 ms** |     **53.7920 ms** |     **2.9485 ms** |     **1.00** |    **0.03** |     **37.5 KB** |        **1.00** |
| GpuResident  | 100000    | 100  | CPUAc(...) GPU) [26] | 273,266.665 ms | 15,571.3204 ms |   853.5166 ms | 1,918.62 |   34.39 | 39299.82 KB |    1,048.00 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **1000** | **CPUAc(...) GPU) [26]** |     **148.846 ms** |     **25.9228 ms** |     **1.4209 ms** |     **1.00** |    **0.01** |      **150 KB** |        **1.00** |
| GpuResident  | 100000    | 1000 | CPUAc(...) GPU) [26] | 278,611.111 ms | 26,581.9880 ms | 1,457.0485 ms | 1,871.93 |   17.71 | 39525.48 KB |      263.50 |

<!-- markdownlint-enable MD060 -->

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
