# IndelPattern.NormalizedDistance

The distance scaled into `[0, 1]` by the sum of the two lengths.

<!-- docs-declaration -->

```csharp
public double NormalizedDistance(ReadOnlySpan<char> text)
```

**Parameters** — `text` is the string to scan against the pattern.

**Returns** — `double` in `[0, 1]`, `0` meaning identical. Two empty inputs give `0`.

**Exceptions** — `ObjectDisposedException` when the handle has been disposed.

**Example** — five edits over thirteen units.

```csharp
using Lodestar.Text.Distances;

using IndelPattern query = IndelPattern.For("kitten");

double d = Math.Round(query.NormalizedDistance("sitting".AsSpan()), 4);   // => 0.3846
```

**Remarks** — the same expression [`Indel.NormalizedDistance`](indel-normalizeddistance.md)
evaluates, over the same distance, so the two agree bit for bit.

Use this rather than [`Distance`](indelpattern-distance.md) when scores from pairs of different
lengths have to be compared: an edit count is not comparable across lengths and this is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IndelPattern`](indelpattern.md),
[`Indel.NormalizedDistance`](indel-normalizeddistance.md).
