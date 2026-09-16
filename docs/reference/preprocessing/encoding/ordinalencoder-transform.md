# OrdinalEncoder.Transform

Encodes a row-major matrix as category indices.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<T> values)
```

**Parameters** — `values` is the categories to encode, row-major, with `FeatureCount` per row.

**Returns** — a new array of the same length, each value the index of its category.

**Exceptions** — `ArgumentException` when `values` holds no row, a partial one, a null, or a category
the fit never saw.

**Example** — two features, each coded against its own categories.

```csharp
using Lodestar.Preprocessing;

string[] values = ["b", "x", "a", "y", "c", "x"];

OrdinalEncoder<string> encoder = Encoders.Ordinal(values, featureCount: 2);

string codes = string.Join(",", encoder.Transform(values));  // => 1,0,0,1,2,0
```

**Remarks** — an unseen category is refused rather than coded: the reference's default raises too,
and its `use_encoded_value` needs a value outside the codes that nothing here asks for yet.

Returns `double[]` rather than `int[]` so the result feeds a scaler or a model without a conversion.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinalEncoder`](ordinalencoder.md), [`Encoders.Ordinal`](encoders-ordinal.md).
