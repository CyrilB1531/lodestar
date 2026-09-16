# Encoders.Ordinal

Fits an ordinal encoder on a row-major matrix of categories.

<!-- docs-declaration -->

```csharp
public static OrdinalEncoder<T> Ordinal<T>(ReadOnlySpan<T> values, int featureCount) where T : IComparable<T>, IEquatable<T>
```

**Type parameters** — `T` is the category type; `string` and `int` are the two the reference takes.

**Parameters** — `values` is the categories, row-major: `featureCount` per row. `featureCount` is
how many values each row carries.

**Returns** — a fitted [`OrdinalEncoder<T>`](ordinalencoder.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `values` holds no row, a partial one, or a null.

**Example** — integers encode by their rank among the distinct values, not by their own magnitude.

```csharp
using Lodestar.Preprocessing;

int[] values = [10, 2, 33, 2];

OrdinalEncoder<int> encoder = Encoders.Ordinal(values, featureCount: 1);

string codes = string.Join(",", encoder.Transform(values));  // => 1,0,2,0
```

**Remarks** — no options: the reference's `handle_unknown="use_encoded_value"` needs a value outside
the codes, and nothing here asks for one yet — an unseen category is refused, which is the
reference's own default.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OrdinalEncoder`](ordinalencoder.md), [`Encoders.OneHot`](encoders-onehot.md).
