# DeviceTextBlock.Upload

Renames a batch against a pattern's alphabet and uploads it.

<!-- docs-declaration -->

```csharp
public static DeviceTextBlock Upload(GpuContext context, string pattern, IReadOnlyList<string> texts)
```

**Parameters** — `context` is the accelerator to upload to. `pattern` is the string whose
characters define the alphabet; the batch is renamed against it and can only be scored against it.
`texts` are the strings to rename, none null.

**Returns** — `DeviceTextBlock`, owning two device buffers the caller disposes.

**Exceptions** — `ArgumentNullException` when an argument, or one of the texts, is null;
`ArgumentException` when `texts` is empty, or the pattern holds more than 255 distinct characters.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceTextBlock.Upload(context, "abc", ["abc", "abd", ""]);

int held = block.Count;  // => 3
```

**Remarks** — **the block remembers the alphabet it was renamed with**, and
[`BitParallelEditDistance.Distance`](bitparalleleditdistance-distance.md) refuses a pattern holding
a character that alphabet never saw. Without that check such a character would silently rename to
the reserved code, the kernel would treat it as matching nothing, and the answer would come back
plausible and wrong.

An empty string in the batch is legal and scores the pattern's length, which is the right answer.
An empty *batch* is refused, because there is nothing to launch a kernel over.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTextBlock`](devicetextblock.md).
