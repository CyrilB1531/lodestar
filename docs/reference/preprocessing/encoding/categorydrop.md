# CategoryDrop

Which category, if any, [`OneHotEncoder`](onehotencoder.md) leaves without a column.

<!-- docs-declaration -->

```csharp
public enum CategoryDrop
```

**Fields** — `None` gives every category a column, `First` drops each feature's first, and
`IfBinary` drops the first only where the feature has exactly two — `drop=None`, `"first"` and
`"if_binary"`.

**Example** — dropping the first category of a three-category feature.

```csharp
using Lodestar.Preprocessing;

OneHotEncoder<string> encoder = Encoders.OneHot(
    ["a", "b", "c"], 1, new OneHotEncoderOptions { Drop = CategoryDrop.First });

int columns = encoder.EncodedFeatureCount;                 // => 2
string first = string.Join(",", encoder.Transform(["a"])); // => 0,0
```

**Remarks** — dropping one category is what a linear model with an intercept needs: with every
category carrying a column, the columns of a feature sum to 1 and are collinear with the intercept.
`IfBinary` is the compromise the reference offers — drop where a single column says everything, keep
where it does not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoderOptions`](onehotencoderoptions.md).
