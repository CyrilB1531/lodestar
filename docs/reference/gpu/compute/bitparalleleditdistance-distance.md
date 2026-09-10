# BitParallelEditDistance.Distance

The edit distance from one pattern to every string in a resident batch.

<!-- docs-declaration -->

```csharp
public int[] Distance(string pattern, DeviceTextBlock texts)
```

**Parameters** — `pattern` is the string the batch was renamed against, between 1 and 64
characters. `texts` is that resident batch.

**Returns** — `int[]`, one distance per string, in the batch's own order.

**Exceptions** — `ArgumentNullException` when an argument is null; `ArgumentException` when the
pattern is empty or longer than 64, or when the batch was renamed against a different alphabet.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceTextBlock.Upload(context, "abc", ["abc", "abd", "xyz"]);

int[] distances = new BitParallelEditDistance(context).Distance("abc", block);

int exact = distances[0];  // => 0
int oneEdit = distances[1];  // => 1
```

**Remarks** — a **global** edit distance, not an approximate search. The difference is one bit: the
horizontal carry shifts in a trailing `1`, which is the boundary condition making the first row of
the dynamic program grow with the text. Without it the text may begin anywhere, and the two agree
often enough that the defect reads as a rounding — measured, a pattern of `"aaaa"` against a text of
`"aaa"` answered 0 where the answer is 1.

The equality table is built on the host from the pattern and uploaded per call: 256 entries, which
is negligible beside the batch. **The batch is what must not be re-uploaded** — see
[`DeviceTextBlock`](devicetextblock.md) for what that costs.

A pattern holding a character the batch's alphabet never saw is **refused**, not silently treated as
unmatched.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`BitParallelEditDistance`](bitparalleleditdistance.md).
