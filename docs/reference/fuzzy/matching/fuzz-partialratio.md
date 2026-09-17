# Fuzz.PartialRatio

The best-matching window of the longer string.

<!-- docs-declaration -->

```csharp
public static double PartialRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double PartialRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare; which is longer does not matter. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`, the best [`Ratio`](fuzz-ratio.md) over any window of the
longer string as long as the shorter.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. `ArgumentException` when the two strings hold more than 63,455 distinct code points above U+0020, which is what a `char` can rank one unit per code point; [`Fuzz.Ratio`](fuzz-ratio.md) answers such a pair, these scorers do not yet.

**Example** — a short string contained in a longer one.

```csharp
using Lodestar.Fuzzy;

double contained = Fuzz.PartialRatio("apple", "an apple a day");  // => 100
```

**Remarks** — `100`, because `apple` appears verbatim inside the longer string.
[`Ratio`](fuzz-ratio.md) on the same pair is far lower, and both are correct: one asks "are these
the same string", the other "does the short one occur in the long one".

Reach for it when one side is a **fragment** — a search box against titles, a product name against
a description. Do not reach for it when the two are the same kind of thing, because it will happily
score `100` for a short string that matches a small part of a long one and means something else.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0002](../../../decisions/0002-unicode-comparison-unit.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.Ratio`](fuzz-ratio.md),
[`Fuzz.PartialTokenSortRatio`](fuzz-partialtokensortratio.md).
