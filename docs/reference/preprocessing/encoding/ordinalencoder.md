# OrdinalEncoder

One code per category, at `sklearn.preprocessing.OrdinalEncoder` parity.

<!-- docs-declaration -->

```csharp
public sealed class OrdinalEncoder<T> where T : IComparable<T>, IEquatable<T>
```

**Type parameters** — `T` is the category type; `string` and `int` are the two the reference takes.

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on, and `Categories`
is each feature's categories, sorted. The code of a value is its index in that list.

**Example** — the codes carry the sort order.

```csharp
using Lodestar.Preprocessing;

OrdinalEncoder<string> encoder = Encoders.Ordinal(["b", "a", "c"], featureCount: 1);

string categories = string.Join(",", encoder.Categories[0]);      // => a,b,c
string codes = string.Join(",", encoder.Transform(["c", "a"]));   // => 2,0
```

**Remarks** — **one column in, one column out**, where [`OneHotEncoder`](onehotencoder.md) gives each
category a column of its own. That makes it the cheaper encoding and the more dangerous one: the
codes are numbers, so a model that reads them as numbers reads the sort order as distance — `'c'` is
twice `'b'`, which nothing in the data said.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Encoders.Ordinal`](encoders-ordinal.md), [`OneHotEncoder`](onehotencoder.md).

## Members

| Member | What it does |
| --- | --- |
| [`OrdinalEncoder.Transform`](ordinalencoder-transform.md) | Encodes a matrix as category indices. |
