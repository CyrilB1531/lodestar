# LabelEncoder.Transform

Reads labels as codes.

<!-- docs-declaration -->

```csharp
public int[] Transform(ReadOnlySpan<T> labels)
```

**Parameters** — `labels` is one column of labels to encode.

**Returns** — one code per label: its index in [`Classes`](labelencoder.md).

**Exceptions** — `ArgumentException` when `labels` holds a null, or a label this encoder was not
fitted on.

**Example** — the fitted column, and one value on its own.

```csharp
using Lodestar.Preprocessing;

string[] cities = ["paris", "lyon", "paris", "nice"];

LabelEncoder<string> encoder = Encoders.Label<string>(cities);

string all = string.Join(",", encoder.Transform(cities));      // => 2,0,2,1
int one = encoder.Transform(["nice"])[0];                      // => 1
```

**Remarks — an unseen label is refused, and there is no option to encode it anyway.** The
reference refuses it too, and here that is the only reasonable answer: a target column's codes
name classes, and a class nobody fitted has no code that means anything. That is the difference
from [`OneHotEncoder`](onehotencoder.md), which offers
[`UnknownCategory.Zeros`](unknowncategory.md) because a feature can legitimately meet a value the
training set did not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LabelEncoder.InverseTransform`](labelencoder-inversetransform.md),
[`LabelEncoder`](labelencoder.md).
