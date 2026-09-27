# JaroWinkler.Similarity

Computes `Jaro.Similarity` and then raises it in proportion to how many of the first four
characters the two strings share.

<!-- docs-declaration -->

```csharp
public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, double prefixWeight = 0.1, TextElement element = TextElement.Utf16Unit)
```

**Parameters** — `a` and `b` are the two strings to compare. `prefixWeight` is how much each
shared leading character is worth, `0.1` by default, which is jellyfish's value and is also
available as the constant `JaroWinkler.DefaultPrefixWeight`; it must lie in `[0, 1]`, rapidfuzz's
range for the same parameter. `element` says what counts as one character; pass
`TextElement.CodePoint` for parity with jellyfish outside the Basic Multilingual Plane.

**Returns** — `double` in `[0, 1]`, larger meaning more alike.

**Exceptions** — `ArgumentOutOfRangeException` when `prefixWeight` is below `0`, above `1`, or
`NaN`.

**Example** — a shared `DI` prefix lifts a middling Jaro score.

```csharp
using Lodestar.Text.Distances;

double s = JaroWinkler.Similarity("DIXON", "DICKSONX");   // => 0.8133…
```

**Remarks** — prefer this to plain `Jaro` for names, and to `Levenshtein` for both: people
mistype and abbreviate the ends of names far more often than the beginnings, so agreement on the
first few characters really is evidence. It is the standard choice for surname matching in record
linkage, which is what it was built for.

Two behaviours regularly surprise a caller, and both are jellyfish's, kept on purpose. The boost
is
applied only when the underlying Jaro score is already above `0.7`, so a pair that shares a prefix
but little else gets no lift at all and reads as identical to plain `Jaro`. And only the first
four
characters ever count, however long the shared prefix runs.

A `prefixWeight` above `0.25` could lift four shared characters past `1`, so the score is capped
there, as rapidfuzz caps it: at `0.5`, `JaroWinkler.Similarity("MARTHA", "MARHTA")` reads `1`
rather than `1.0277…`.

**Applies to** — net10.0, netstandard2.0.

**See also** — `JaroWinkler.Distance`, `Jaro.Similarity`,
the [Python equivalence table](../../../equivalence.md).
