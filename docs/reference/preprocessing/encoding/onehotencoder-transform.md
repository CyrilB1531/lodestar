# OneHotEncoder.Transform

Encodes a row-major matrix of categories.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<T> values)
```

**Parameters** — `values` is the categories to encode, row-major, with `FeatureCount` per row.

**Returns** — a new array of `rows × EncodedFeatureCount` values, each 0 or 1.

**Exceptions** — `ArgumentException` when `values` holds no row, a partial one, or a null; or when it
holds a category the fit never saw and the encoder was fitted with
[`UnknownCategory.Refuse`](unknowncategory.md).

**Example** — an ignored unknown and a dropped first category produce the same row.

```csharp
using Lodestar.Preprocessing;

var options = new OneHotEncoderOptions
{
    Drop = CategoryDrop.First,
    Unknown = UnknownCategory.Ignore,
};

OneHotEncoder<string> encoder = Encoders.OneHot(["a", "b", "c"], 1, options);

string dropped = string.Join(",", encoder.Transform(["a"]));      // => 0,0
string unknown = string.Join(",", encoder.Transform(["zzz"]));    // => 0,0
```

**Remarks** — **a row of zeros means either**, and nothing distinguishes them. That collision is the
reference's, not this package's invention, and it is the reason to think twice before combining the
two options.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoder`](onehotencoder.md), [`OneHotEncoderOptions`](onehotencoderoptions.md).
