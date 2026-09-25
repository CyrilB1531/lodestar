# OneHotEncoder.TransformSparse

Encodes a row-major matrix of categories into a sparse matrix, `OneHotEncoder(sparse_output=True)`'s
output and the reference's default.

<!-- docs-declaration -->

```csharp
public CsrMatrix TransformSparse(ReadOnlySpan<T> values)
```

**Parameters** — `values` is the categories to encode, row-major, with `FeatureCount` per row.

**Returns** — a `rows × EncodedFeatureCount` `CsrMatrix` holding exactly the ones
[`Transform`](onehotencoder-transform.md) sets: at most one per feature and row, in ascending column
order.

**Exceptions** — `ArgumentException` as for [`Transform`](onehotencoder-transform.md).

**Example** — an infrequent category and an unknown one land in the same column.

```csharp
using Lodestar.Abstractions;
using Lodestar.Preprocessing;

var options = new OneHotEncoderOptions { MinFrequency = 2, Unknown = UnknownCategory.Infrequent };
OneHotEncoder<string> encoder = Encoders.OneHot(["a", "a", "a", "b", "b", "c", "d"], 1, options);

CsrMatrix encoded = encoder.TransformSparse(["a", "c", "zzz"]);

int stored = encoded.NonZeroCount;                          // => 3
string columns = string.Join(",", encoded.ColumnIndices);   // => 0,2,2
```

**Remarks** — a dropped category and an ignored unknown store nothing, as `sparse_output=True`
stores nothing for them. A high-cardinality column is where this matters: the dense encoding is
mostly zeros, and every sparse consumer in these packages — the decompositions, the sparse
scalers — takes a `CsrMatrix`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoder.Transform`](onehotencoder-transform.md), [`OneHotEncoder`](onehotencoder.md).
