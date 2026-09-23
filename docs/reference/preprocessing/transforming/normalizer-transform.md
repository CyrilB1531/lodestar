# Normalizer.Transform

Scales each row of a row-major matrix to unit norm.

<!-- docs-declaration -->

```csharp
public static double[] Transform(ReadOnlySpan<double> samples, int featureCount, RowNorm norm = RowNorm.L2)
```

<!-- docs-declaration -->

```csharp
public static CsrMatrix Transform(CsrMatrix matrix, RowNorm norm = RowNorm.L2)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row; the span is
read, never modified. `matrix` is the sparse form, also read and never modified. `norm` says
which norm each row is scaled by.

**Returns** — a new matrix of the same shape; the sparse overload returns a new `CsrMatrix` with
the same stored positions, since scaling a row by a positive number turns no stored value into a
zero that was not one already.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or `norm` is
not a defined value. `ArgumentException` when `samples` holds no row, a partial one, or a
non-finite value.

**Example** — three documents as word counts, each scaled to unit Euclidean length, which is what
makes a dot product between two of them a cosine.

```csharp
using Lodestar.Preprocessing;

double[] counts = [2.0, 0.0, 1.0, 0.0, 4.0, 4.0, 1.0, 1.0, 1.0];

double[] scaled = Normalizer.Transform(counts, featureCount: 3);

double first = Math.Round(scaled[0], 4);    // => 0.8944
double second = Math.Round(scaled[4], 4);   // => 0.7071
```

**Remarks — a row of zeros is left alone rather than divided by its own norm.** There is no
direction to preserve, and the reference makes the same choice; the row comes back as zeros
rather than as `NaN`s.

```csharp
using Lodestar.Preprocessing;

double[] withEmptyRow = [3.0, 4.0, 0.0, 0.0];

double[] scaled = Normalizer.Transform(withEmptyRow, featureCount: 2);

double emptyStaysEmpty = scaled[2];   // => 0
```

**The sparse overload returns a new matrix rather than scaling one in place.** `CsrMatrix`
carries a `NormalizeRows` that mutates, which is the right shape for a caller who owns the matrix
and wants it changed; it is the wrong shape here, where every member leaves its input alone so a
caller can transform the same data twice. It also carries no maximum norm, which this does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RowNorm`](rownorm.md), [`StandardScaler.Transform`](../scaling/standardscaler-transform.md)
for the column-wise counterpart, the [Python equivalence table](../../../equivalence.md).
