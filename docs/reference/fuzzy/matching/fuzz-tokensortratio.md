# Fuzz.TokenSortRatio

The words sorted before comparing, so their order stops counting.

<!-- docs-declaration -->

```csharp
public static double TokenSortRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double TokenSortRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`, the [`Ratio`](fuzz-ratio.md) of the two strings after each
is split into words, sorted and rejoined.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value.

**Example** — the same words in a different order.

```csharp
using Lodestar.Fuzzy;

double reordered = Fuzz.TokenSortRatio("new york mets", "mets new york");  // => 100
```

**Remarks** — `100`, where [`Ratio`](fuzz-ratio.md) on the same pair is much lower. Word order is
the difference, and for names, addresses and titles it usually carries no meaning — "Smith, John"
and "John Smith" are one person.

It still counts **extra** words against you: a side with a word the other lacks scores lower, which
is the difference from [`TokenSetRatio`](fuzz-tokensetratio.md).

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0002](../../../decisions/0002-unicode-comparison-unit.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.TokenSetRatio`](fuzz-tokensetratio.md), [`Fuzz.Ratio`](fuzz-ratio.md).
