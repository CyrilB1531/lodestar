# IndelPattern.NormalizedSimilarity

`1 - NormalizedDistance`, and — multiplied by 100 — exactly rapidfuzz's `fuzz.ratio`.

<!-- docs-declaration -->

```csharp
public double NormalizedSimilarity(ReadOnlySpan<char> text)
```

**Parameters** — `text` is the string to scan against the pattern.

**Returns** — `double` in `[0, 1]`, larger meaning more alike. Two empty inputs give `1`.

**Exceptions** — `ObjectDisposedException` when the handle has been disposed.

**Example** — the ratio every candidate scores against one query.

```csharp
using Lodestar.Text.Distances;

using IndelPattern query = IndelPattern.For("state");

double s = query.NormalizedSimilarity("taste".AsSpan());   // => 0.8
```

**Remarks** — this is the member a bulk `fuzz.ratio` workload wants: multiply by 100 and the
numbers are rapidfuzz's, with the pattern's table built once instead of once per candidate.

Nothing here preprocesses. `"Kitten"` and `"kitten"` score below `1`, exactly as
[`Indel.NormalizedSimilarity`](indel-normalizedsimilarity.md) has them — normalize before building
the handle, once, rather than per text.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IndelPattern`](indelpattern.md),
[`Indel.NormalizedSimilarity`](indel-normalizedsimilarity.md),
[`Fuzz.Ratio`](../../fuzzy/matching/fuzz-ratio.md).
