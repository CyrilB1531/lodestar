# OneHotEncoderOptions

Which category [`OneHotEncoder`](onehotencoder.md) drops, and what it does with an unseen value.

<!-- docs-declaration -->

```csharp
public sealed record OneHotEncoderOptions
```

**Properties** — `Drop` chooses which category loses its column, and `Unknown` what happens to a
value the fit never saw — `sklearn.preprocessing.OneHotEncoder`'s `drop` and `handle_unknown`, same
defaults.

**Example** — `IfBinary` drops the first category only where the feature has exactly two.

```csharp
using Lodestar.Preprocessing;

var options = new OneHotEncoderOptions { Drop = CategoryDrop.IfBinary };

OneHotEncoder<string> binary = Encoders.OneHot(["y", "n", "y"], 1, options);
OneHotEncoder<string> three = Encoders.OneHot(["a", "b", "c"], 1, options);

int binaryColumns = binary.EncodedFeatureCount;  // => 1
int threeColumns = three.EncodedFeatureCount;    // => 3
```

**Remarks** — the two settings interact: an ignored unknown encodes to all zeros, and so does a
dropped first category, so a row of zeros means either. [`Transform`](onehotencoder-transform.md)
says so with the example.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CategoryDrop`](categorydrop.md), [`UnknownCategory`](unknowncategory.md).
