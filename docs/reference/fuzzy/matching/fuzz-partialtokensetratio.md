# Fuzz.PartialTokenSetRatio

Word sets, compared over the best-matching window.

<!-- docs-declaration -->

```csharp
public static double PartialTokenSetRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double PartialTokenSetRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`: the word sets are formed, then
[`PartialRatio`](fuzz-partialratio.md) is applied.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. `ArgumentException` when the two strings hold more than 63,455 distinct code points above U+0020, which is what a `char` can rank one unit per code point; [`Fuzz.Ratio`](fuzz-ratio.md) answers such a pair, these scorers do not yet.

**Example** — a subset of the words, scored as a fragment.

```csharp
using Lodestar.Fuzzy;

string query = "mariners vs angels";
string candidate = "los angeles angels vs seattle mariners";

double score = Fuzz.PartialTokenSetRatio(query, candidate);  // => 100
```

**Remarks** — **the most forgiving of the seven**, and the one to justify before using. It ignores
word order, ignores extra words, and scores the best window rather than the whole — three
allowances at once, so a `100` here says much less than a `100` from [`Ratio`](fuzz-ratio.md).

It earns its place on messy, human-entered text where every one of those three differences is
noise. On clean data it will merge records that should stay apart.

A side with no words — empty, or whitespace alone — scores `0`, as in rapidfuzz, so a blank query
does not match every candidate.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0002](../../../decisions/0002-unicode-comparison-unit.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.TokenSetRatio`](fuzz-tokensetratio.md), [`Fuzz.WRatio`](fuzz-wratio.md).
