# KBinsDiscretizer.InverseTransform

Reads each bin back as its centre.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> encoded)
```

**Parameters** — `encoded` is a matrix this discretizer produced, row-major,
[`OutputFeatureCount`](kbinsdiscretizer.md) values per row.

**Returns** — one value per input feature, per row: the centre of the bin each code names.

**Exceptions** — `ArgumentException` when `encoded` holds a partial row or a non-finite value; when
an ordinal code, truncated toward zero, falls outside `[-n, n)` for a feature of `n` bins — the
reference indexes with `astype(np.int64)`, so a negative code counts from the last bin and one past
either end raises `IndexError`; or when a one-hot block is all zeros, which the reference's encoder
refuses. A one-hot block is read by its first largest value, numpy's `argmax`
([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)).

**Example** — the four bin codes read back.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

KBinsDiscretizer bins = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal });

double[] centres = bins.InverseTransform([0.0, 1.0, 2.0, 3.0]);

double firstBin = centres[0];    // => 22
double lastBin = centres[3];     // => 67.5

double roundTrip = bins.InverseTransform(bins.Transform([44.0]))[0];   // => 51
```

**Remarks — this is not a round trip, and cannot be.** The discretizer replaced a measurement
with the group it belongs to, and the only thing left to answer with is where that group sits: 44
comes back as 51, the centre of the bin from 41 to 61. Use it to read a discretized column in the
units it came from, not to recover the column.

The reference's own inverse does the same, and its documentation says so in the same words: the
returned value is the bin's centre.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizer.Transform`](kbinsdiscretizer-transform.md),
[`KBinsDiscretizer`](kbinsdiscretizer.md).
