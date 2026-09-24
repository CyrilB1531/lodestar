# Lodestar.Gpu

GPU kernels over device-resident data, through ILGPU: an embedding matrix swept by a
batch of queries with cosine similarity and top-k on the accelerator, sparse and dense products,
and MinHash signatures over tokenized text. Each kernel ships only where it beat this repository's
own CPU path. It targets `net10.0` and `netstandard2.1`, the one package without a
`netstandard2.0` build, because ILGPU publishes none; nothing else depends on it.

## Install

```bash
dotnet add package Lodestar.Gpu
```

## Example

```csharp
using Lodestar.Gpu.Compute;

// preferCpu runs the kernels on ILGPU's CPU accelerator, where no GPU is present.
using var context = GpuContext.Create(preferCpu: true);

bool onGraphicsHardware = context.IsHardwareGpu;   // False
```

## Parity

Measured and checked against this repository's own CPU paths.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A satellite package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.1`:

- `ILGPU` 1.5.3 or later
- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [gpu kernels](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/gpu-kernels.md)
- Reference: [gpu/compute](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/gpu/compute.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Gpu/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Gpu/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
