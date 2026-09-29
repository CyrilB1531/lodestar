# CsrMatrix.ToDense

The same matrix with its zeros written out, for inspection.

<!-- docs-declaration -->

```csharp
public double[,] ToDense()
```

**Returns** — `double[,]` of `RowCount × ColumnCount`, every cell present, the stored values in
their places and zeros everywhere else.

**Exceptions** — `InvalidOperationException` when the matrix has more cells than a
two-dimensional array holds — `uint.MaxValue` in all, `0x7FFFFFC7` along one side — rather than
failing inside the allocation. A 50,000-square matrix is within that; it still needs 20 GB
([#1412](https://github.com/CyrilB1531/lodestar/issues/1412)).

**Example** — the cell that says `the` appears twice in the third document.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;

string[] docs = ["the cat eats", "the dog eats", "the cat and the dog"];
var cv = new CountVectorizer();
CsrMatrix counts = cv.FitTransform(docs);

// Features are sorted: and, cat, dog, eats, the — so "the" is column 4.
double[,] dense = counts.ToDense();
double theInThird = dense[2, 4];  // => 2
```

**Remarks** — **allocates `RowCount × ColumnCount` doubles**, which is exactly the array the sparse
layout exists to avoid. On the three-document corpus above that is fifteen cells; on a real corpus
it is the number that made sparsity necessary in the first place. Reach for it to look at a small
matrix, to hand a small one to something that wants a rectangular array, or in a test — not as a
step in a pipeline.

To read one cell without materialising the rest, walk
[`RowPointers`](csrmatrix.md) and `ColumnIndices` directly; to reduce the whole matrix against a
vector, [`Multiply`](csrmatrix-multiply.md) does it without densifying.

A column stored twice in one row is written out as the sum of its entries, which is what
[`Multiply`](csrmatrix-multiply.md) computes from the same row and what scipy's `toarray()` returns.

A stored `NaN` or infinity is written out as it is, not refused, as `toarray()` writes it.

**On .NET Framework the cap is lower.** The bound is .NET's `Array.MaxLength`; .NET Framework
refuses a `double` array past 2 GB, or past `0x7FEFFFFF` elements under `gcAllowVeryLargeObjects`,
so a result between its cap and this one fails there with the runtime's own out-of-memory error,
raised before any memory is taken.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CsrMatrix`](csrmatrix.md), [`CsrMatrix.Multiply`](csrmatrix-multiply.md).
