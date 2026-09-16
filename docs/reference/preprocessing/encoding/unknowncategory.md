# UnknownCategory

What [`OneHotEncoder`](onehotencoder.md) does with a value the fit never saw.

<!-- docs-declaration -->

```csharp
public enum UnknownCategory
```

**Fields** — `Refuse` raises naming the feature, and `Ignore` encodes the value as all zeros —
`handle_unknown="error"` and `"ignore"`, with `Refuse` the default as `"error"` is.

**Example** — the same value under both settings.

```csharp
using Lodestar.Preprocessing;

OneHotEncoder<string> lenient = Encoders.OneHot(
    ["a", "b"], 1, new OneHotEncoderOptions { Unknown = UnknownCategory.Ignore });

string unknown = string.Join(",", lenient.Transform(["zzz"]));  // => 0,0
```

**Remarks** — refusing is the default because the alternative is silent: an unknown category becomes
a row of zeros that a model reads as a category of its own, and no exception marks the point where
the data stopped matching the fit.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneHotEncoderOptions`](onehotencoderoptions.md), [`OneHotEncoder.Transform`](onehotencoder-transform.md).
