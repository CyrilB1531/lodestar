# DeviceSparseMatrix.Upload

Uploads a CSR matrix to the accelerator.

<!-- docs-declaration -->

```csharp
public static DeviceSparseMatrix Upload(GpuContext context, ReadOnlySpan<int> rowPointers, ReadOnlySpan<int> columnIndices, ReadOnlySpan<double> values, int rowCount, int columnCount)
```

**Parameters** — `context` is the accelerator to upload to. `rowPointers` holds `rowCount` + 1
offsets into the other two. `columnIndices` holds one column per stored value, ascending inside a
row. `values` holds the stored values in the same order. `rowCount` and `columnCount` are the
matrix's shape; `columnCount` is not derived from the indices, because a matrix may have trailing
columns no row stores anything in.

**Returns** — `DeviceSparseMatrix`, owning three device buffers the caller disposes.

**Exceptions** — `ArgumentNullException` when `context` is null; `ArgumentOutOfRangeException` when
a dimension is below 1; `ArgumentException` when the three arrays do not describe one CSR matrix.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceSparseMatrix.Upload(
    context, [0, 2, 3], [0, 1, 1], [1.0, 2.0, 3.0], rowCount: 2, columnCount: 2);

int stored = matrix.NonZeroCount;  // => 3
```

**Remarks** — three shapes are checked because each has its own failure. `rowPointers` of the wrong
length silently shifts every row; `columnIndices` and `values` of different lengths reads past one
of them; and a final offset that disagrees with the value count means the last row is truncated
without anything noticing.

**Column indices ascending inside a row is a precondition, not a check.** The kernel relies on it
only to read contiguous memory, so an unsorted row produces the same arithmetic more slowly rather
than a wrong answer — but it is what `CsrMatrix` guarantees and what a caller building the arrays
by hand has to preserve.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceSparseMatrix`](devicesparsematrix.md).
