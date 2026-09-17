# Fuzz.WRatio

Picks among the other scorers by inspecting the input.

<!-- docs-declaration -->

```csharp
public static double WRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double WRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`, a weighted best among the scorers it judges applicable.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. `ArgumentException` when the two strings hold more than 63,455 distinct code points above U+0020, which is what a `char` can rank one unit per code point; [`Fuzz.Ratio`](fuzz-ratio.md) answers such a pair, these scorers do not yet.

**Example** — the same typo `Ratio` scores, reached by a different route.

```csharp
using Lodestar.Fuzzy;

double score = Fuzz.WRatio("apple pie", "appel pie");  // => 88.88888888888889
```

**Remarks** — the answer to "which scorer" when the input is not known in advance. It compares the
lengths, tries the applicable scorers, and weights the partial ones down so that a fragment match
does not automatically beat a whole-string one.

The same value as [`Ratio`](fuzz-ratio.md) here, because on two strings of equal length there is
nothing for the partial scorers to add. On a pair where one side is much longer they diverge, and
that divergence is the point of the type.

It is rapidfuzz's `WRatio` and the weights are rapidfuzz's; the number is reproduced rather than
invented, which matters because the weights are not derived from anything — they are a choice that
library made.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0002](../../../decisions/0002-unicode-comparison-unit.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.Ratio`](fuzz-ratio.md), [`Fuzz.PartialRatio`](fuzz-partialratio.md),
[the matching index](../matching.md).
