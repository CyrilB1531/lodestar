# Fuzz.Ratio

Edit-distance similarity over the whole of both strings.

<!-- docs-declaration -->

```csharp
public static double Ratio(string a, string b)
```

<!-- docs-declaration -->

```csharp
public static double Ratio(string a, string b, TextElement element)
```

The second overload compares over `element`. At `TextElement.CodePoint` a character outside the
Basic Multilingual Plane counts once, tokens split on rapidfuzz's own whitespace and sort by code
point, which is rapidfuzz's score on any string; `TextElement.Utf16Unit` is the first overload.

**Parameters** — `a` and `b` are the strings to compare. Order does not change the answer. `element` is the unit compared, in the second overload only.

**Returns** — `double` in `[0, 100]`. `100` when identical, `0` when they share nothing.

**Exceptions** — `ArgumentOutOfRangeException` when `element` is not a declared value. This scorer has no upper bound on the alphabet: past the 63,455 code points a `char` can rank it compares the code points themselves.

**Example** — a transposition, and the two extremes.

```csharp
using Lodestar.Fuzzy;

double typo = Fuzz.Ratio("apple pie", "appel pie");  // => 88.88888888888889
double same = Fuzz.Ratio("abc", "abc");  // => 100
double nothing = Fuzz.Ratio("abc", "xyz");  // => 0
```

The same call over code points, on two emoji that share their high surrogate.

```csharp
using Lodestar.Fuzzy;
using Lodestar.Text;

double units = Fuzz.Ratio("\U0001F600", "\U0001F601");                           // => 50
double codePoints = Fuzz.Ratio("\U0001F600", "\U0001F601", TextElement.CodePoint);  // => 0
```

**Remarks** — this is the scorer to reach for when the two strings are **the same kind of thing**:
two spellings of a name, two renderings of an address. It compares everything, which is its
strength and its limit.

Length is what breaks it. `Fuzz.Ratio("apple", "an apple a day")` is low, not because the strings
disagree but because most of the second is absent from the first — and
[`PartialRatio`](fuzz-partialratio.md) is the answer to that shape.

An emoji is two UTF-16 units, so the first overload scores two different emoji that share a high
surrogate as half alike, where rapidfuzz scores them `0`; pass `TextElement.CodePoint` when the text
can leave the BMP, as [decision 0001](../../../decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md) offers on
every algorithm it affects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Fuzz.PartialRatio`](fuzz-partialratio.md),
[`Fuzz.TokenSortRatio`](fuzz-tokensortratio.md).
