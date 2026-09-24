# Performance

One comparison per capability: this library against the library a reader would otherwise
reach for, with that library's version, the machine the pair ran on and the window the run
took. Both sides are checked to return the same answers before either is timed — the
agreement checks, the corpora and every command are
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md)'s
subject, and this page carries only what came out.

A before/after of this repository's own code is not here. It is an argument about one
commit, and it belongs in the pull request that made it.

## How to read a row

- **A ratio above 1 means this library is faster**, unless the table's own header says
  otherwise; each table states its direction.
- **A named machine is not a shared runner.** Every row below names the processor, the
  operating system and the runtime it ran on. Where a row comes from a hosted runner or a
  session container it says so, and is read as a ratio: such a machine's absolutes are not
  comparable between nights, or with another machine's.
- **A cross-language row is processor time as well as wall time** where the Python side can
  use more than one core, because a wall-clock lead over a multi-threaded reference is a
  different claim from a lead per core.
- **Every baseline here is single-threaded** unless the row says otherwise.

## Packages

Each package's comparisons live beside its project ([#1133](https://github.com/CyrilB1531/lodestar/issues/1133)).

| package | comparisons |
| --- | --- |
| [`Lodestar.Abstractions`](../../src/Lodestar.Abstractions/performance.md) | none yet |
| [`Lodestar.Cluster`](../../src/Lodestar.Cluster/performance.md) | measured |
| [`Lodestar.Conformal`](../../src/Lodestar.Conformal/performance.md) | none yet |
| [`Lodestar.Decomposition`](../../src/Lodestar.Decomposition/performance.md) | measured |
| [`Lodestar.Embeddings`](../../src/Lodestar.Embeddings/performance.md) | measured |
| [`Lodestar.Extensions.AI`](../../src/Lodestar.Extensions.AI/performance.md) | none yet |
| [`Lodestar.Extensions.MathNet`](../../src/Lodestar.Extensions.MathNet/performance.md) | none yet |
| [`Lodestar.Extensions.VectorData`](../../src/Lodestar.Extensions.VectorData/performance.md) | none yet |
| [`Lodestar.Fuzzy`](../../src/Lodestar.Fuzzy/performance.md) | measured |
| [`Lodestar.Gpu`](../../src/Lodestar.Gpu/performance.md) | measured |
| [`Lodestar.Metrics`](../../src/Lodestar.Metrics/performance.md) | measured |
| [`Lodestar.Onnx`](../../src/Lodestar.Onnx/performance.md) | none yet |
| [`Lodestar.Preprocessing`](../../src/Lodestar.Preprocessing/performance.md) | measured |
| [`Lodestar.Stats`](../../src/Lodestar.Stats/performance.md) | measured |
| [`Lodestar.Stats.Regression`](../../src/Lodestar.Stats.Regression/performance.md) | measured |
| [`Lodestar.Stats.TimeSeries`](../../src/Lodestar.Stats.TimeSeries/performance.md) | measured |
| [`Lodestar.Survival`](../../src/Lodestar.Survival/performance.md) | none yet |
| [`Lodestar.Text`](../../src/Lodestar.Text/performance.md) | measured |
