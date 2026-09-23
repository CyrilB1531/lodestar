# Encoders.Label

Fits a label encoder on one column of labels.

<!-- docs-declaration -->

```csharp
public static LabelEncoder<T> Label<T>(ReadOnlySpan<T> labels) where T : IComparable<T>, IEquatable<T>
```

**Type parameter** — `T` is the label type; `string` and `int` are the two the reference takes.

**Parameters** — `labels` is one column of labels.

**Returns** — a fitted [`LabelEncoder`](labelencoder.md).

**Exceptions** — `ArgumentException` when `labels` is empty or holds a null.

**Example** — the codes and how many rows were read.

```csharp
using Lodestar.Preprocessing;

string[] cities = ["paris", "lyon", "paris", "nice"];

LabelEncoder<string> encoder = Encoders.Label<string>(cities);

int rows = encoder.SampleCount;                        // => 4
string classes = string.Join(",", encoder.Classes);    // => lyon,nice,paris
```

**Remarks — one column, not a matrix**, which is the one difference from
[`Encoders.Ordinal`](encoders-ordinal.md): the reference's `LabelEncoder` takes a 1-D array and
its `OrdinalEncoder` a 2-D one, because the first is for a target and the second for features.
There is no `featureCount` parameter here for that reason.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LabelEncoder`](labelencoder.md), [`Encoders`](encoders.md),
[`Encoders.Ordinal`](encoders-ordinal.md).
