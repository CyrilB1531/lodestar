# OneHotEncoderOptions

Which category [`OneHotEncoder`](onehotencoder.md) drops, and what it does with an unseen value.

<!-- docs-declaration -->

```csharp
public sealed record OneHotEncoderOptions
```

**Properties** — `Drop` chooses which category loses its column, and `Unknown` what happens to a
value the fit never saw — `sklearn.preprocessing.OneHotEncoder`'s `drop` and `handle_unknown`, same
defaults. `MinFrequency` makes a category seen fewer times infrequent, and `MinFrequencyShare` one
seen in fewer than that share of the rows, strictly between 0 and 1: `min_frequency` as an integer
and as a float, one setting, so the two are refused together. `MaxCategories` caps each feature's
columns, its infrequent one included — `max_categories`. All three are `null` by default, which
groups nothing.

**Example** — `IfBinary` drops the first category only where the feature has exactly two.

```csharp
using Lodestar.Preprocessing;

var options = new OneHotEncoderOptions { Drop = CategoryDrop.IfBinary };

OneHotEncoder<string> binary = Encoders.OneHot(["y", "n", "y"], 1, options);
OneHotEncoder<string> three = Encoders.OneHot(["a", "b", "c"], 1, options);

int binaryColumns = binary.EncodedFeatureCount;  // => 1
int threeColumns = three.EncodedFeatureCount;    // => 3
```

**Example** — at most two columns: the most frequent category, and one for the rest.

```csharp
using Lodestar.Preprocessing;

var capped = new OneHotEncoderOptions { MaxCategories = 2 };
OneHotEncoder<string> encoder = Encoders.OneHot(["a", "a", "a", "b", "b", "c", "d"], 1, capped);

int columns = encoder.EncodedFeatureCount;                            // => 2
string grouped = string.Join(",", encoder.InfrequentCategories[0]!);  // => b,c,d
```

**Remarks** — past `MaxCategories` the least frequent categories join the infrequent ones, ranked
by a stable sort of the counts, so a tie keeps the sorted category order, as the reference's does.
`CategoryDrop.First` drops the first column **after** grouping, and `CategoryDrop.IfBinary` counts
the grouped columns. The drop and unknown settings interact: an ignored unknown encodes to all zeros, and so does a
dropped first category, so a row of zeros means either. [`Transform`](onehotencoder-transform.md)
says so with the example.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CategoryDrop`](categorydrop.md), [`UnknownCategory`](unknowncategory.md).
