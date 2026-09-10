# DeviceTextBlock

A batch of strings renamed to a dense alphabet and held on the accelerator.

<!-- docs-declaration -->

```csharp
public sealed class DeviceTextBlock : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceTextBlock.Upload(context, "kitten", ["sitting", "mitten"]);

int held = block.Count;  // => 2
int alphabet = DeviceTextBlock.MaxPatternAlphabet;  // => 255
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`DeviceTextBlock.Upload`](devicetextblock-upload.md) | Renames a batch against a pattern's alphabet and uploads it |
| [`DeviceTextBlock.Dispose`](devicetextblock-dispose.md) | Frees the two device buffers |

**Properties** — `Count` is how many strings the block holds. `MaxPatternAlphabet` is the constant
255, how many distinct characters a pattern may hold.

**Remarks** — **a kernel parameter has to be blittable, so nothing here carries a `string`.** The
batch is renamed on the host into one flat array of symbol codes plus the offsets cutting it back
into rows. That renaming is also what lets the equality masks be a 256-entry table rather than a
lookup per character.

A character the pattern does not hold renames to **one reserved code**, because a mask no pattern
position sets is the same mask for all of them. 255 rather than 256 distinct pattern characters for
exactly that reason: one slot is spoken for.

**The renaming is the expensive half, and it is on the host.** Measured: scoring a resident batch
ran 28× to 146× faster than the CPU path, while renaming and uploading per call ran **1.7× to 2.9×
slower** — a pass over every character, in the language the baseline is already written in. Upload
once, score many times, or do not use this kernel.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`BitParallelEditDistance`](bitparalleleditdistance.md),
[the namespace index](../compute.md).
