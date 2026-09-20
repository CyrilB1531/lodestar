# Fuzz.PartialTokenSortRatio

Sorted words, compared over the best-matching window.

<!-- docs-declaration -->

```csharp
public static double PartialTokenSortRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double PartialTokenSortRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`: the words are sorted, then
[`PartialRatio`](fuzz-partialratio.md) is applied.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. `ArgumentException` when the two strings hold more than 63,455 distinct code points above U+0020, which is what a `char` can rank one unit per code point; [`Fuzz.Ratio`](fuzz-ratio.md) answers such a pair, these scorers do not yet.

**Example** — reordered words, scored as a fragment.

```csharp
using Lodestar.Fuzzy;

double score = Fuzz.PartialTokenSortRatio("new york mets", "mets new york");  // => 100
```

**Remarks** — both transformations at once: order stops counting **and** one side may be a
fragment of the other. That is two kinds of forgiveness compounded, so it scores high on pairs a
person would call unrelated — which is why it is worth choosing deliberately rather than reaching
for as a default.

When the input is not known in advance, [`WRatio`](fuzz-wratio.md) chooses among the scorers
instead of applying the most forgiving one unconditionally.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0001](../../../decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.TokenSortRatio`](fuzz-tokensortratio.md),
[`Fuzz.PartialRatio`](fuzz-partialratio.md).
