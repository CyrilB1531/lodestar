# Fuzz.TokenSetRatio

The words as sets, so extra words on one side stop counting against it.

<!-- docs-declaration -->

```csharp
public static double TokenSetRatio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double TokenSetRatio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`, computed over the intersection and the two differences of
the word sets.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. `ArgumentException` when the two strings hold more than 63,455 distinct code points above U+0020, which is what a `char` can rank one unit per code point; [`Fuzz.Ratio`](fuzz-ratio.md) answers such a pair, these scorers do not yet.

**Example** — one side carrying words the other does not.

```csharp
using Lodestar.Fuzzy;

string query = "mariners vs angels";
string candidate = "los angeles angels vs seattle mariners";

double subset = Fuzz.TokenSetRatio(query, candidate);  // => 100
```

**Remarks** — `100`, because every word of the shorter side appears in the longer one. That is the
most forgiving of the seven and the easiest to misuse: it will score `100` for a query that is a
**subset** of a candidate, however much else that candidate says.

Right for "does this short label refer to this long one" — a team name against a full fixture, a
brand against a product title. Wrong for deduplication, where two records differing by several
words are usually two things.

Duplicated words do not help: a set counts a word once, so `"the the cat"` and `"the cat"` compare
as equal sets.

A side with no words — empty, or whitespace alone — scores `0` against anything, the other side
included when it has none either, as in rapidfuzz: an empty set is not a subset match.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0002](../../../decisions/0002-unicode-comparison-unit.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.TokenSortRatio`](fuzz-tokensortratio.md),
[`Fuzz.PartialTokenSetRatio`](fuzz-partialtokensetratio.md).
