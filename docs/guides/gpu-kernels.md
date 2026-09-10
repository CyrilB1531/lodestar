# GPU kernels

`Lodestar.Gpu` puts four operations on an accelerator through
[ILGPU](https://github.com/m4rs-mt/ILGPU). It is **additive**: no package under `src/` depends on
it, so the SIMD and scalar paths the rest of Lodestar ships remain the complete answer on every
target framework.

This guide is about **when the accelerator is worth using**, which is a narrower question than
whether it is faster. [`docs/guides/performance.md`](performance.md) has the figures;
`bench/README.md` sections 21 to 25 have how they were taken.

```bash
dotnet add package Lodestar.Gpu
```

## The one rule: upload once, sweep many times

Every kernel here takes a **resident** operand, and that is not an optimisation — it is the
difference between a speed-up and a pessimisation. The upload is one call —
[`DeviceEmbeddingMatrix.Upload`](../reference/gpu/compute/deviceembeddingmatrix-upload.md) for
vectors, [`DeviceSparseMatrix.Upload`](../reference/gpu/compute/devicesparsematrix-upload.md) for a
CSR matrix, [`DeviceDenseBlock.Upload`](../reference/gpu/compute/devicedenseblock-upload.md) for a
dense operand — and every query after it is cheap.

```csharp
using Lodestar.Gpu.Compute;

const int documents = 2_000;
const int dimension = 8;
float[] rows = new float[documents * dimension];
for (int i = 0; i < rows.Length; i++)
{
    rows[i] = (i % 7) + 1;
}

using var context = GpuContext.Create();
using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, documents, dimension);
var kernel = new TiledCosineTopK(context);

// The corpus crossed the bus once. Every query after this is cheap.
float[] queries = rows[..(4 * dimension)];
IReadOnlyList<IReadOnlyList<GpuSearchResult>> hits = kernel.Search(matrix, queries, 4, 10);

int answered = hits.Count;  // => 4
```

Measured on an RTX 5070 Ti: a hundred thousand documents and one query ran **6.6× faster** than
[`EmbeddingIndex.Search`](../reference/embeddings/search/embeddingindex-search.md) with the matrix already resident — and **9.25× slower** when the same
corpus was uploaded for that one query. The crossing point is around a hundred queries per corpus.

**If your corpus changes on every request, do not use this package.** That is not a limitation to
work around; it is the workload an accelerator is bad at, and the SIMD path is the right answer.

## Which device is actually running

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create();

string device = context.DeviceName;
bool graphics = context.IsHardwareGpu;
bool named = device.Length > 0;  // => True
```

**Read `IsHardwareGpu`, never the accelerator type.** An OpenCL runtime installed for a processor
reports `AcceleratorType.OpenCL` and executes on the CPU — and asked for a non-CPU device, ILGPU
returned exactly that ahead of a CUDA card on the machine these numbers come from. A whole
benchmark run was published against a processor before anyone noticed, which is why `GpuContext`
orders the devices itself and why the device's name belongs beside any figure you publish.

[`GpuContext.Create`](../reference/gpu/compute/gpucontext-create.md) with `preferCpu: true` forces
ILGPU's CPU accelerator. Use it in tests: it proves a kernel **correct** where there is no graphics
hardware, which is a different question from whether it is **faster**.

## Choosing a kernel

| You have | Use | Clears the 5–10× gate |
| --- | --- | --- |
| an embedding corpus and a batch of queries | [`TiledCosineTopK`](../reference/gpu/compute/tiledcosinetopk.md) | at 100 000 rows, or at 256 queries |
| a CSR matrix and a dense block | [`TiledSparseDenseProduct`](../reference/gpu/compute/tiledsparsedenseproduct.md) | at 50 000 rows |
| one pattern and a batch of strings | [`BitParallelEditDistance`](../reference/gpu/compute/bitparalleleditdistance.md) | at every size measured |
| two operations to compose | [`DeviceDenseBlock`](../reference/gpu/compute/devicedenseblock.md) | a different ratio — see below |

The edit-distance kernel is the largest gain and carries the loudest caveat: its baseline,
[`Levenshtein.Distance`](../reference/text/distances/levenshtein-distance.md), is bit-parallel but **single-threaded**, so a `Parallel.For` over the CPU
path would close much of the gap. Read that row as "parallelism helps a lot here", not as a claim
against a parallel CPU implementation.

## Chaining, and why it is worth most on small work

A kernel returning `double[]` has already paid a device-to-host copy, so the next one pays a
host-to-device copy undoing it. `DeviceDenseBlock` is the type that avoids both.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create();
var kernel = new TiledSparseDenseProduct(context);

// A two-by-two diagonal, held on the device and applied twice.
using var diagonal = DeviceSparseMatrix.Upload(
    context, [0, 1, 2], [0, 1], [2.0, 3.0], rowCount: 2, columnCount: 2);

using var operand = DeviceDenseBlock.Upload(context, [1.0, 0.0, 0.0, 1.0], 2, 2);
using DeviceDenseBlock inner = kernel.Multiply(diagonal, operand);
using DeviceDenseBlock outer = kernel.Multiply(diagonal, inner);  // nothing crossed the bus
double[] answer = outer.Download();                               // one copy, whole chain

double squared = answer[0];  // => 4
```

Measured at **1.24× to 2.94×** faster than the same two products with a download and re-upload
between them — **and the gain is largest where the work is smallest.** That inverts the usual
intuition, and the arithmetic is why: a fixed transfer against work that grows is a falling share
of the total, so a large job amortises a round trip on its own.

## What this package does not do

No approximate index, no Block-Max WAND, no query language. `TiledCosineTopK` is exhaustive — it
scores every row and selects the best *k*. Brute force is what an accelerator is good at, and an
approximate structure would spend its parallelism on branching.

It also targets `net10.0` and `netstandard2.1` and **not** `netstandard2.0`, because ILGPU
publishes no such asset
([decisions 0101](../decisions/0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md)
and [0103](../decisions/0103-lodestar-gpu-ships-netstandard2-1-beside-net10.md)). A .NET Framework
caller reaches the SIMD paths, which lose nothing by this package existing.

**See also** — [the reference pages](../reference/gpu/compute.md),
[what was measured](performance.md).
