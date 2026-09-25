# Encoders.OneHot

Fits a one-hot encoder on a row-major matrix of categories.

<!-- docs-declaration -->

```csharp
public static OneHotEncoder<T> OneHot<T>(ReadOnlySpan<T> values, int featureCount, OneHotEncoderOptions options = null) where T : IComparable<T>, IEquatable<T>
```

**Type parameters** — `T` is the category type; `string` and `int` are the two the reference takes.

**Parameters** — `values` is the categories, row-major: `featureCount` per row. `featureCount` is
how many values each row carries. `options` chooses which category to drop and what to do with an
unseen value; `null` drops none and refuses.

**Returns** — a fitted [`OneHotEncoder<T>`](onehotencoder.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, or when
`options` holds a `Drop` or an `Unknown` that is not a defined value, a `MinFrequency` or a
`MaxCategories` below 1, or a `MinFrequencyShare` outside (0, 1). `ArgumentException` when `values`
holds no row, a partial one, or a null, or when `options` sets both `MinFrequency` and
`MinFrequencyShare`.

**Example** — two features, and the column layout they produce.

```csharp
using Lodestar.Preprocessing;

// Two features per row: the first has three categories, the second two.
string[] values = ["b", "x", "a", "y", "c", "x", "a", "y"];

OneHotEncoder<string> encoder = Encoders.OneHot(values, featureCount: 2);

int columns = encoder.EncodedFeatureCount;                   // => 5
string row = string.Join(",", encoder.Transform(["a", "y"])); // => 1,0,0,0,1
```

**Remarks** — the first feature's columns come first, in its own sorted order, then the second's.
That is the reference's layout, and it is why `EncodedFeatureCount` is worth reading before slicing
the result.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoderOptions`](onehotencoderoptions.md), [`Encoders.Ordinal`](encoders-ordinal.md).
