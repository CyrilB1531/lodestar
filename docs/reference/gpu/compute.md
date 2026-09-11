# GPU kernels — `Lodestar.Gpu.Compute`

Four kernels over ILGPU, and the device-resident types they read and write. Everything here is
**additive**: no package under `src/` depends on this one, so the SIMD and scalar paths the rest
of Lodestar ships stay the complete answer on every target framework
([decision 0101](../../decisions/0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md)).

Two facts run through the whole namespace, and knowing them saves reading every entry.

- **Residency is the point, not an optimisation.** A kernel that uploads its corpus per call is
  *slower* than the CPU path it replaces — measured at 9.25× slower for a hundred thousand
  documents answering one query. The `Device*` types exist so a corpus crosses the bus once and is
  swept many times, and [`docs/guides/performance.md`](../../guides/performance.md) has the figures.
- **A kernel parameter must be blittable**, so nothing here takes a `string` or a Lodestar type.
  Text is renamed to symbol codes on the host; matrices are taken as spans and dimensions. That is
  also why this package carries no inter-package edge in either direction.

## The types

| Type | What it does |
| --- | --- |
| [`GpuContext`](compute/gpucontext.md) | Opens a device and holds the accelerator its kernels load onto. |
| [`TiledCosineTopK`](compute/tiledcosinetopk.md) | Sweeps a resident matrix with a batch of queries: cosine, then top-k. |
| [`TiledSparseDenseProduct`](compute/tiledsparsedenseproduct.md) | A resident CSR matrix times a dense block, tiled through shared memory. |
| [`BitParallelEditDistance`](compute/bitparalleleditdistance.md) | Myers' edit distance from one pattern to a batch of strings. |
| [`TiledMinHashSignatures`](compute/tiledminhashsignatures.md) | MinHash signatures for a batch, one thread per permutation. |
| [`DeviceEmbeddingMatrix`](compute/deviceembeddingmatrix.md) | A row-major embedding matrix held across many queries. |
| [`DeviceSparseMatrix`](compute/devicesparsematrix.md) | A CSR matrix held across many products. |
| [`DeviceDenseBlock`](compute/devicedenseblock.md) | A dense block held **between** two operations, which is what makes a chain. |
| [`DeviceTextBlock`](compute/devicetextblock.md) | A batch of strings renamed to a dense alphabet and held on the device. |
| [`DeviceTokenHashes`](compute/devicetokenhashes.md) | One document's token hashes per row, held flat. |
| [`MinHashScheme`](compute/minhashscheme.md) | Which permutation family a signature is built from. |
| [`GpuSearchResult`](compute/gpusearchresult.md) | One hit from a device sweep: a row index and its score. |

## Which device runs, and how to be sure

`GpuContext.Create()` orders the devices itself rather than asking ILGPU for a preferred one.
**That is not a stylistic choice**: asked for a non-CPU device, ILGPU returned an OpenCL runtime
that executes on the processor — a machine here enumerated `cpu-skylake-avx512-AMD Ryzen 7 8700G`
as an OpenCL device beside a CUDA card. A check on the accelerator's *type* called that a GPU.

So read [`IsHardwareGpu`](compute/gpucontext.md), not the accelerator type, and publish
[`DeviceName`](compute/gpucontext.md) beside any figure. `Create(preferCpu: true)` forces ILGPU's
CPU accelerator, which is what proves a kernel **correct** where there is no graphics hardware —
a different question from whether it is **faster**, which
[decision 0102](../../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md) answers on a
named machine.

## What this namespace does not offer

No approximate index, no Block-Max WAND, no query language. `TiledCosineTopK` is exhaustive: it
scores every row and selects the best *k*. That is the trade this package makes — brute force is
what a GPU is good at, and an approximate structure would spend its parallelism on branching.

**See also** — [the performance guide](../../guides/performance.md) for what each kernel measured,
and `bench/README.md` sections 21 to 25 for how. Every type here applies to net10.0 and
netstandard2.1; its own page says so.
