# CsrMatrix.CreateUnchecked

Builds a matrix from raw arrays without the structural pass the constructor runs — for a producer
whose arrays are valid by construction.

<!-- docs-declaration -->

```csharp
public static CsrMatrix CreateUnchecked(int rowCount, int columnCount, double[] values, int[] columnIndices, int[] rowPointers)
```

**Parameters** — `rowCount` and `columnCount` are the shape. `values` holds the stored entries row by
row, `columnIndices` the column of each, and `rowPointers` where each row starts in `values`,
followed by the total — `rowCount + 1` entries.

**Returns** — a [`CsrMatrix`](csrmatrix.md) that shares the three arrays rather than copying them.

**Exceptions** — `ArgumentNullException` when an array is null. `ArgumentOutOfRangeException` when
a dimension is negative. `ArgumentException` when `rowPointers` is not `rowCount + 1` long, or
`values` and `columnIndices` differ in length.

**Example** — two rows built by hand, the same matrix the constructor would have validated.

```csharp
using Lodestar.Abstractions;

CsrMatrix matrix = CsrMatrix.CreateUnchecked(2, 3, [1.0, 2.0, 3.0], [0, 2, 1], [0, 2, 3]);

int rows = matrix.RowCount;                  // => 2
double first = matrix.RowL1Norm(0);          // => 3
```

**Remarks** — **unvalidated, and that is the whole difference from the constructor.** The length and
null checks run; the per-entry pass does not. Row pointers that decrease, a column index at or past
`columnCount`, or a pointer past the stored values are accepted here and surface later — as a wrong
product, or as an `IndexOutOfRangeException` from whichever member reads the bad entry first.

Reach for it only when the arrays come from your own code and are correct by the way they were
built: [`CountVectorizer`](../../text/vectorizers/countvectorizer.md) and the other vectorizers call
it on arrays they have just filled. Anything read from a file, a network or a caller goes through
[the constructor](csrmatrix.md), which refuses every malformed shape.

Public since 0.2.0. It was internal before, reached by `Lodestar.Text` through an
`InternalsVisibleTo` this package no longer grants
([decision 0003](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)); the
signature did not change, so a `Lodestar.Text` built against 0.1.x still binds to it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CsrMatrix`](csrmatrix.md), [`CsrMatrix.ToDense`](csrmatrix-todense.md),
[`CountVectorizer`](../../text/vectorizers/countvectorizer.md).
