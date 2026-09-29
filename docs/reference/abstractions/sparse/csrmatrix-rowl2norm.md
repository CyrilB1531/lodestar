# CsrMatrix.RowL2Norm

The Euclidean length of one row.

<!-- docs-declaration -->

```csharp
public double RowL2Norm(int row)
```

**Parameters** — `row` is the zero-based row index.

**Returns** — `double`, the square root of the sum of the squares of that row's stored cells.
Zero for an empty row.

**Exceptions** — `IndexOutOfRangeException` when `row` is negative or not below
`RowCount`. The index reaches the backing array directly, so the array's own exception
is what surfaces rather than a re-wrapped one.

**Example** — three ones give `√3`; the third row's repeated `the` makes it longer.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;

string[] docs = ["the cat eats", "the dog eats", "the cat and the dog"];
CsrMatrix counts = new CountVectorizer().FitTransform(docs);

double first = counts.RowL2Norm(0);   // => 1.7320508075688772
double third = counts.RowL2Norm(2);   // => 2.6457513110645907
```

**Remarks** — this is the norm that matters for similarity. Two rows divided by their L2 norms
have a dot product equal to their cosine similarity, which is why
[`TfidfOptions`](../../text/vectorizers/tfidfoptions.md) normalizes by it and why
[`NormalizeRows(SparseNorm.L2)`](csrmatrix-normalizerows.md) is the usual call before comparing
documents.

A matrix straight out of [`TfidfVectorizer`](../../text/vectorizers/tfidfvectorizer.md) is already L2-normalized, so
every row's norm is `1` and calling this on one is a way to confirm that rather than to learn
something new.

A stored `NaN` or infinity is carried into the norm, not refused: the row's norm is then `NaN` or
`+∞`. The squares are summed as they are, unscaled, so a finite row whose sum of squares overflows —
any value past about `1.34e154`, such as `[1e200]` — reads `+∞` although its true norm is finite.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CsrMatrix.RowL1Norm`](csrmatrix-rowl1norm.md),
[`CsrMatrix.NormalizeRows`](csrmatrix-normalizerows.md), [`SparseNorm`](sparsenorm.md).
