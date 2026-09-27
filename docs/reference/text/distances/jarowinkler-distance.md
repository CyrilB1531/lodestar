# JaroWinkler.Distance

`1 - Similarity`, for code that wants a distance rather than a score.

<!-- docs-declaration -->

```csharp
public static double Distance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, double prefixWeight = 0.1, TextElement element = TextElement.Utf16Unit)
```

**Parameters** — `a` and `b` are the two strings to compare, `prefixWeight` is what each shared
leading character is worth (`0.1` by default) and `element` says what counts as one character —
all
three exactly as for `Similarity`, which this subtracts from `1`.

**Returns** — `double` in `[0, 1]`, larger meaning less alike.

**Exceptions** — `ArgumentOutOfRangeException` when `prefixWeight` is below `0`, above `1`, or
`NaN`.

**Example** — a swapped pair of letters, forgiven almost entirely because the prefix agrees.

```csharp
using Lodestar.Text.Distances;

double d = JaroWinkler.Distance("MARTHA", "MARHTA");   // => 0.0388…
```

**Remarks** — the same measure as `Similarity`, turned round for code that sorts ascending or
thresholds with "at most". Nothing else differs.

It inherits `Similarity`'s cap: a `prefixWeight` above `0.25` cannot push the similarity past `1`,
so this never drops below zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — `JaroWinkler.Similarity`, `Jaro.Distance`,
the [Python equivalence table](../../../equivalence.md).
