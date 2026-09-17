# Fuzz.TokenSetRatio

The words as sets, so extra words on one side stop counting against it.

<!-- docs-declaration -->

```csharp
public static double TokenSetRatio(string a, string b)
```

**Parameters** — `a` and `b` are the strings to compare.

**Returns** — `double` in `[0, 100]`, computed over the intersection and the two differences of
the word sets.

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

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.TokenSortRatio`](fuzz-tokensortratio.md),
[`Fuzz.PartialTokenSetRatio`](fuzz-partialtokensetratio.md).
