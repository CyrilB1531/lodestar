# GpuContext.Create

Opens the best available device, or ILGPU's CPU accelerator.

<!-- docs-declaration -->

```csharp
public static GpuContext Create(bool preferCpu = false)
```

**Parameters** — `preferCpu` forces the CPU accelerator. That is what a machine with no GPU runs,
and what proves a kernel *correct* where the 5–10× gate cannot be evaluated at all.

**Returns** — `GpuContext`, owning an accelerator the caller disposes.

**Exceptions** — `InvalidOperationException` when no device could be opened.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var forced = GpuContext.Create(preferCpu: true);

bool graphics = forced.IsHardwareGpu;  // => False
```

**Remarks** — **`Context.GetPreferredDevice` is deliberately not used.** Measured: asked for a
non-CPU device it returned an OpenCL runtime executing on the processor, ahead of a CUDA card, so
every figure a benchmark produced would have been the CPU's. This orders the devices itself — CUDA
first, then OpenCL on hardware whose device type is a GPU, then anything else.

Where no graphics device exists the CPU accelerator is opened rather than throwing, because a
correctness suite has to run on a machine without one. Check `IsHardwareGpu` before reading a
timing; a functional result is the same either way.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`GpuContext`](gpucontext.md).
