# GpuContext

An ILGPU context and the accelerator its kernels run on.

<!-- docs-declaration -->

```csharp
public sealed class GpuContext : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);

bool graphics = context.IsHardwareGpu;  // => False
bool named = context.DeviceName.Length > 0;  // => True
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`GpuContext.Create`](gpucontext-create.md) | Opens the best available device, or the CPU accelerator |
| [`GpuContext.Dispose`](gpucontext-dispose.md) | Frees the accelerator and then the context, in that order |

**Properties** — `Accelerator` is the ILGPU accelerator kernels load onto. `DeviceName` is the
device's own name, which a published figure has to carry. `IsHardwareGpu` says whether that
device is real graphics hardware.

**Remarks** — **one per process is the intended shape.** Creating an accelerator is expensive and
ILGPU compiles each kernel on first launch, so a context built per call measures a compiler rather
than a kernel. That is also why every kernel type here takes a context and is built once.

**`IsHardwareGpu` is not the same question as the accelerator's type**, and the difference cost a
whole benchmark run. An OpenCL runtime installed for a processor reports `AcceleratorType.OpenCL`
and executes on the CPU; asked for a non-CPU device, ILGPU returned exactly that ahead of a CUDA
card. A check on the type called it a GPU and the report carried a processor's figures under a
graphics heading. This asks OpenCL for the *device* type instead.

Disposing releases the accelerator and then the context, in that order. Every `Device*` type built
against a context must be disposed before it.

**Applies to** — net10.0, netstandard2.1.

**See also** — [the namespace index](../compute.md),
[decision 0102](../../../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md).
