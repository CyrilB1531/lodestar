# BitParallelEditDistance

Myers' bit-parallel edit distance, one pattern against a batch of strings.

<!-- docs-declaration -->

```csharp
public sealed class BitParallelEditDistance
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceTextBlock.Upload(context, "kitten", ["sitting", "kitten"]);
var kernel = new BitParallelEditDistance(context);

int[] distances = kernel.Distance("kitten", block);

int first = distances[0];  // => 3
int second = distances[1];  // => 0
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`BitParallelEditDistance.Distance`](bitparalleleditdistance-distance.md) | The distance from one pattern to every string |

**Properties** — `MaxPatternLength` is the constant 64, the longest pattern one machine word carries.

**Remarks** — **the parallelism is across pairs, not inside one.** Myers already collapses a
dynamic-programming row into a single machine word, so there is nothing left to widen within a
comparison: one thread runs one whole distance in registers, and the kernel wins by running tens of
thousands at once.

That shape is why this measured the **largest gain of the four kernels, 28× to 146×** — and why the
caveat matters: the baseline is [`Levenshtein.Distance`](../../text/distances/levenshtein-distance.md), which is bit-parallel but **single-threaded**.
A `Parallel.For` over the CPU path would close much of that gap.

Limited to a 64-character pattern. The blocked formulation beyond that carries state a thread would
have to loop over, which is a second kernel rather than a wider one.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTextBlock`](devicetextblock.md), [the namespace index](../compute.md).
