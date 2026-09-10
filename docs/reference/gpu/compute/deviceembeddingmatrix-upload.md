# DeviceEmbeddingMatrix.Upload

Uploads a row-major block of vectors to the accelerator.

<!-- docs-declaration -->

```csharp
public static DeviceEmbeddingMatrix Upload(GpuContext context, ReadOnlySpan<float> rows, int count, int dimension, bool normalize = true)
```

**Parameters** — `context` is the accelerator to upload to. `rows` holds `count` × `dimension`
values, row-major and contiguous. `normalize` L2-normalizes each row on upload, which is the
default and what makes cosine a dot product.

**Returns** — `DeviceEmbeddingMatrix`, owning device memory the caller disposes.

**Exceptions** — `ArgumentNullException` when `context` is null; `ArgumentOutOfRangeException` when
`count` or `dimension` is below 1; `ArgumentException` when `rows` is not exactly the block.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceEmbeddingMatrix.Upload(
    context, [3f, 4f, 0f, 1f], count: 2, dimension: 2);

int held = matrix.Count;  // => 2
```

**Remarks** — the staging copy is deliberate: normalization happens on the host, on a copy, so the
caller's array is never modified. That costs one pass and one allocation per upload, which is paid
once per corpus rather than once per query.

**Normalizing twice is the mistake to avoid.** If the vectors are already unit length, pass
`normalize: false`: re-normalizing is harmless arithmetically but costs a pass, and a caller who
mixes normalized and unnormalized corpora in one index gets scores that are not comparable.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceEmbeddingMatrix`](deviceembeddingmatrix.md).
