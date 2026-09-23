# Encoders

Fits the categorical encoders.

<!-- docs-declaration -->

```csharp
public static class Encoders
```

**Example** — one feature of three categories, one column each.

```csharp
using Lodestar.Preprocessing;

string[] values = ["b", "a", "c", "a"];

OneHotEncoder<string> encoder = Encoders.OneHot(values, featureCount: 1);

string categories = string.Join(",", encoder.Categories[0]);  // => a,b,c
string first = string.Join(",", encoder.Transform(["b"]));    // => 0,1,0
```

**Remarks** — a static factory rather than a `Fit` on each encoder, for two reasons. A public static
member on a generic type is what CA1000 refuses; and inference reads better —
`Encoders.OneHot(values, 1)` against `OneHotEncoder<string>.Fit(values, 1)`. It is the shape
[`Splitters`](../splitting/splitters.md) already has in this package.

**One element type per call**, which is what a 2-D array carries in the reference too: a caller with
a string column and an integer column makes two calls and concatenates the results. The type decides
the category order — strings sort by code point, integers as numbers — and that order decides the
columns.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoder`](onehotencoder.md), [`OrdinalEncoder`](ordinalencoder.md),
[`LabelEncoder`](labelencoder.md), [`SimpleImputer`](simpleimputer.md), the
[encoding index](../encoding.md).

## Members

| Member | What it does |
| --- | --- |
| [`Encoders.OneHot`](encoders-onehot.md) | Fits a one-hot encoder on a row-major matrix of categories. |
| [`Encoders.Ordinal`](encoders-ordinal.md) | Fits an ordinal encoder on the same. |
| [`Encoders.Label`](encoders-label.md) | Fits a label encoder on one column of labels. |
