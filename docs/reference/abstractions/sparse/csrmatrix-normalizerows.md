# CsrMatrix.NormalizeRows

Divide every row by its own norm, in place.

<!-- docs-declaration -->

```csharp

public void NormalizeRows(SparseNorm norm)
```

**Parameters** — `norm` is which norm to divide by: [`SparseNorm.L1`](sparsenorm.md) for a row
that sums to `1`, or `SparseNorm.L2` for a row of unit Euclidean length.

**Returns** — nothing. **The matrix is modified in place**, which is what makes it cheap and what
makes it a trap: a caller holding another reference to the same matrix sees the change.

**Exceptions** — `ArgumentOutOfRangeException` when `norm` is not a defined `SparseNorm`, as
`sklearn.preprocessing.normalize` refuses an unknown norm. `InvalidOperationException` when a
stored value is `NaN` or infinite, which scikit-learn's input check refuses too. Either way the
matrix is left untouched.

**Example** — after normalizing, every row's norm is `1`.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;

string[] docs = ["the cat eats", "the dog eats", "the cat and the dog"];

CsrMatrix unit = new CountVectorizer().FitTransform(docs);
unit.NormalizeRows(SparseNorm.L2);
double length = unit.RowL2Norm(0);  // => 1

CsrMatrix shares = new CountVectorizer().FitTransform(docs);
shares.NormalizeRows(SparseNorm.L1);
double mass = shares.RowL1Norm(2);  // => 1
```

**Remarks** — in place rather than returning a copy, because the matrix is the large object in
this namespace and copying it to change every value would double the memory for no benefit. Where
a copy is wanted, transform the corpus again — vectorizing is cheaper than it looks, and the
alternative is a copy nobody asked for on the common path.

A row that is entirely zero has no norm to divide by and is **left alone** rather than producing
`NaN`. That is scikit-learn's choice too, and it is the reason an empty document does not poison a
matrix.

A row of finite values whose norm overflows to `+∞` is divided to zeros, as scikit-learn's
`inplace_csr_row_normalize_l2` divides it.

[`TfidfVectorizer`](../../text/vectorizers/tfidfvectorizer.md) already normalizes, by
[`TfidfOptions.Norm`](../../text/vectorizers/tfidfoptions.md); calling this on its output with the same norm changes
nothing beyond rounding, and with the other norm rescales every row.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SparseNorm`](sparsenorm.md), [`CsrMatrix.RowL2Norm`](csrmatrix-rowl2norm.md),
[`TfidfOptions`](../../text/vectorizers/tfidfoptions.md).
