# OneHotEncoder.FeatureNames

The name of each column the encoder produces, `get_feature_names_out`.

<!-- docs-declaration -->

```csharp
public string[] FeatureNames(IReadOnlyList<string> inputFeatures = null)
```

**Parameters** — `inputFeatures` is one name per input feature; `null` names them `x0`, `x1`, …, as
the reference does for an array.

**Returns** — `feature_category` for each column, in the columns' order, and
`feature_infrequent_sklearn` for a feature's infrequent column.

**Exceptions** — `ArgumentException` when `inputFeatures` does not hold `FeatureCount` names.

**Example** — two frequent categories and the column the rest share.

```csharp
using Lodestar.Preprocessing;

OneHotEncoder<string> encoder = Encoders.OneHot(
    ["a", "a", "a", "b", "b", "c", "d"], 1, new OneHotEncoderOptions { MinFrequency = 2 });

string names = string.Join(",", encoder.FeatureNames(["colour"]));  // => colour_a,colour_b,colour_infrequent_sklearn
```

**Remarks** — a category is written as Python's `str` writes it: a string as it is, an integer in
invariant digits, a floating value as `1.0` or `1e-05`. A dropped column has no name, as it has
no column.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoder`](onehotencoder.md), [`OneHotEncoderOptions`](onehotencoderoptions.md).
