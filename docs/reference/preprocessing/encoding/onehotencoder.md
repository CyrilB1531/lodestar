# OneHotEncoder

One column per category, at `sklearn.preprocessing.OneHotEncoder` parity.

<!-- docs-declaration -->

```csharp
public sealed class OneHotEncoder<T> where T : IComparable<T>, IEquatable<T>
```

**Type parameters** — `T` is the category type; `string` and `int` are the two the reference takes.

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on.
`EncodedFeatureCount` is how many columns [`Transform`](onehotencoder-transform.md) produces, which
is fewer than the total number of categories when one is dropped. `Categories` is each feature's
categories, sorted — the reference's `categories_`.

**Example** — a fitted encoder reports what it will produce before it produces it.

```csharp
using Lodestar.Preprocessing;

OneHotEncoder<string> encoder = Encoders.OneHot(["a", "b", "c", "a"], featureCount: 1);

int rows = encoder.SampleCount;          // => 4
int columns = encoder.EncodedFeatureCount; // => 3
```

**Remarks** — fitted through [`Encoders.OneHot`](encoders-onehot.md) rather than a static `Fit`
here: a public static on a generic type is what CA1000 refuses, and the factory infers `T`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoderOptions`](onehotencoderoptions.md), [`OrdinalEncoder`](ordinalencoder.md).

## Members

| Member | What it does |
| --- | --- |
| [`OneHotEncoder.Transform`](onehotencoder-transform.md) | Encodes a matrix as one column per category. |
