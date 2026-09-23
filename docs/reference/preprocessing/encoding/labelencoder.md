# LabelEncoder

Gives each distinct label a code, at `sklearn.preprocessing.LabelEncoder` parity.

<!-- docs-declaration -->

```csharp
public sealed class LabelEncoder<T> where T : IComparable<T>, IEquatable<T>
```

**Type parameter** — `T` is the label type; `string` and `int` are the two the reference takes.

**Properties** — `SampleCount` is how many labels it was fitted on. `Classes` is the distinct
labels in sorted order — the reference's `classes_` — and the code of a label is its index here.

**Example** — four rows, three cities.

```csharp
using Lodestar.Preprocessing;

string[] cities = ["paris", "lyon", "paris", "nice"];

LabelEncoder<string> encoder = Encoders.Label<string>(cities);

string classes = string.Join(",", encoder.Classes);            // => lyon,nice,paris
string codes = string.Join(",", encoder.Transform(cities));    // => 2,0,2,1
```

**Remarks — this is for the target column, not for the features.** It takes one column and the
reference says so in as many words; a feature matrix goes through
[`OrdinalEncoder`](ordinalencoder.md), which is the same idea over several columns at once and
returns doubles rather than indices. The distinction is worth keeping because the two have
different jobs downstream: a target's codes are class labels a classifier reports back, and a
feature's codes are values a model computes with.

**The order is the sort order of `T`**, so strings sort by code point as numpy's do and integers
as numbers. It is not the order of first appearance, which is what makes the codes reproducible
across a reordered file.

There is no `Fit` on this type: [`Encoders.Label`](encoders-label.md) is the factory, for the
reason [`Encoders`](encoders.md) states.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Encoders.Label`](encoders-label.md), [`OrdinalEncoder`](ordinalencoder.md), the
[encoding index](../encoding.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`LabelEncoder.InverseTransform`](labelencoder-inversetransform.md) | Reads codes back as labels. |
| [`LabelEncoder.Transform`](labelencoder-transform.md) | Reads labels as codes. |
