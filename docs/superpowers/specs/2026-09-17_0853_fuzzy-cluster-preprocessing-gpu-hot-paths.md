# 0853 — Fuzzy scoring and selection, clustering, scaling, encoding and upload

**Status:** **retrospective** — written 2026-09-17, after the measurement it records.

Issue: [#853](https://github.com/CyrilB1531/lodestar/issues/853), found by a performance review of `main`.

## Problem

The review found per-element work that the result does not need in four packages: token-set
ratios building and scoring strings whose scores follow from lengths, `Process` sorting every hit
for a handful, the nearest-neighbour chain recomputing condensed indices and scanning merged slots,
the scalers dividing by the feature count per element, the encoders searching categories per value,
`StratifiedKFold` walking fold shares from zero per row, and the GPU text upload probing a
dictionary per character.

## Change

- `Lodestar.Fuzzy`: the token-set ratio takes two scores from lengths and writes its combined
  strings into one buffer; `WRatio` shares one tokenization; `Process.Extract` keeps a bounded heap
  and `ExtractOne` one scan.
- `Lodestar.Cluster`: the nearest-neighbour chain walks live slots by row offset with the linkage
  chosen per merge; `KMeans` assigns rows past four features over sliced spans.
- `Lodestar.Preprocessing`: `StandardScaler`, `MinMaxScaler` and `MaxAbsScaler` loop by row and
  feature; the encoders look categories up by hash for strings and integral types; `StratifiedKFold`
  keeps a fold cursor per class.
- `Lodestar.Gpu`: `DeviceTextBlock.Upload` renames through a code table.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| [`Fuzz.WRatio`](../../reference/fuzzy/matching/fuzz-wratio.md) | 1.34 µs, 2,760 B | **672 ns, 1,120 B** |
| [`Fuzz.TokenSetRatio`](../../reference/fuzzy/matching/fuzz-tokensetratio.md) | 677 ns, 1,448 B | 539 ns, 896 B |
| [`Process.ExtractOne`](../../reference/fuzzy/matching/process-extractone.md), 100,000 choices | 15.0 ms, 6 MB | **4.07 ms, 0 B** |
| [`Process.Extract`](../../reference/fuzzy/matching/process-extract.md), limit 5 | 14.8 ms, 6 MB | 8.06 ms, 215 B |
| [`KMeans`](../../reference/cluster/partitioning/kmeans.md) Lloyd, 10,000 × 16 × 16 | 6.44 ms | **4.44 ms** |
| [`KMeans`](../../reference/cluster/partitioning/kmeans.md) Lloyd, 50,000 × 8 × 32 | 66.4 ms | 49.5 ms |
| [`AgglomerativeClustering.Fit`](../../reference/cluster/partitioning/agglomerativeclustering-fit.md) complete, 1,500 rows | 16.6 ms | **8.86 ms** |
| [`AgglomerativeClustering.Fit`](../../reference/cluster/partitioning/agglomerativeclustering-fit.md) ward, 1,500 rows | 17.7 ms | 11.1 ms |
| [`StandardScaler`](../../reference/preprocessing/scaling/standardscaler.md) fit and transform, 1,000 × 10 | 49.4 µs | **29.9 µs** |
| [`MinMaxScaler`](../../reference/preprocessing/scaling/minmaxscaler.md) fit and transform, 1,000 × 10 | 36.4 µs | 25.1 µs |
| [`Encoders.Ordinal`](../../reference/preprocessing/encoding/encoders-ordinal.md), 1,000 rows | 51.2 µs | 40.4 µs |
| [`Splitters.StratifiedKFold`](../../reference/preprocessing/splitting/splitters-stratifiedkfold.md), 10,000 samples | 136 µs, 274 KB | 94.2 µs, 313 KB |
| [`DeviceTextBlock.Upload`](../../reference/gpu/compute/devicetextblock-upload.md), 10,000 × 256 characters | 22.0 ms | **1.65 ms** |

Every sum keeps its operands and order, every scan meets its candidates in the same order under the same tie rule, and a hash replaces a search only where equality is exactly a comparison of zero: bit-identical against `main` on the scalers, encoders, splitters, k-means and all four linkages. `GpuFromHost` now runs below the CPU path at every size. k-means keeps `main`'s loop up to four features, where slicing was slower; its `Fit` rows are 1.09× and 0.99×. `FuzzIncumbentBenchmarks`, `ProcessExtractBenchmarks`, `AgglomerativeIncumbentBenchmarks`, `SplitterIncumbentBenchmarks` and `BitParallelEditDistanceBenchmarks`; `KMeansLloydIncumbentBenchmarks`, `ScalerIncumbentBenchmarks` and `EncoderIncumbentBenchmarks` under MediumRun; pinned to four cores.

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 16 logical and 8 physical cores, Ubuntu 26.04.1
LTS, .NET 10.0.12 runtime, `BenchmarkDotNet` 0.14.0. A/B/A: `main`, the fix, `main` again, in one window on
2026-09-17; both `main` runs agreed within 4%. The upload row ran on an NVIDIA GeForce RTX 5070 Ti.

## Rejected

- **A buffer cache on `TiledCosineTopK`.** It makes the class disposable, a public API change, and
  unsafe for concurrent searches.
- **Counting the precomputed DBSCAN neighbourhoods before filling them.** A second n² pass for an
  allocation-only gain.
- **`Process` stopping at a score of 100.** It would stop enumerating a caller's lazy sequence early.
- **A partial-sum exit in the k-means assignment.** Measured slower at every shape: Lloyd 0.53× to
  0.82×, its branch mispredicting on overlapping blobs.
- **Row loops in `RobustScaler` and `SimpleImputer`.** Measured 1.00× to 1.01× and 0.98× to 1.00×
  under MediumRun, so both keep `main`'s loop.
