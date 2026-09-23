# LabelEncoder.InverseTransform

Reads codes back as labels.

<!-- docs-declaration -->

```csharp
public T[] InverseTransform(ReadOnlySpan<int> codes)
```

**Parameters** — `codes` are indices into [`Classes`](labelencoder.md).

**Returns** — one label per code.

**Exceptions** — `ArgumentOutOfRangeException` when a code is negative or past the last class.

**Example** — a classifier's predictions, read back as what they name.

```csharp
using Lodestar.Preprocessing;

string[] cities = ["paris", "lyon", "paris", "nice"];

LabelEncoder<string> encoder = Encoders.Label<string>(cities);

string predicted = string.Join(",", encoder.InverseTransform([2, 0]));   // => paris,lyon
```

**Remarks — this is the round trip the transformers in this package cannot offer**, and the
reason the encoder is worth holding onto after fitting: nothing was approximated on the way in,
so the codes and the labels are the same information under two spellings. A model that reports
class 2 is reporting Paris, and this is what says so.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LabelEncoder.Transform`](labelencoder-transform.md),
[`LabelEncoder`](labelencoder.md).
