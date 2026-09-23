# KBinsDiscretizer.Transform

Reads each value as the bin it falls in.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to transform, row-major,
[`FeatureCount`](kbinsdiscretizer.md) values per row.

**Returns** — a new matrix, [`OutputFeatureCount`](kbinsdiscretizer.md) values per row: the bin's
index under [`BinEncoding.Ordinal`](binencoding.md), or one column per bin of each feature with
exactly one of them set under `OneHot`.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or a non-finite value.

**Example** — one age, one-hot across four bins.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

KBinsDiscretizer bins = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions { BinCount = 4 });

int width = bins.OutputFeatureCount;                        // => 4
string row = string.Join(",", bins.Transform([44.0]));      // => 0,0,1,0
```

**Remarks — a value outside the fitted range is clamped, not refused.** Anything below the first
edge lands in bin 0 and anything above the last in the last bin, which is the reference's
behaviour: the edges describe where the fitted data was, and a later row is allowed to be outside
it.

**The one-hot layout is per feature, concatenated.** Three features with four, two and five bins
give an eleven-column row whose blocks start at 0, 4 and 6 — so a caller reading a particular
feature's block needs the running sum of [`BinCounts`](kbinsdiscretizer.md), not
`feature × BinCount`, because a feature can end with fewer bins than it asked for.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizer.Fit`](kbinsdiscretizer-fit.md),
[`KBinsDiscretizer.InverseTransform`](kbinsdiscretizer-inversetransform.md),
[`BinEncoding`](binencoding.md).
