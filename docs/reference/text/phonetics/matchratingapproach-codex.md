# MatchRatingApproach.Codex

The Match Rating codex of one name.

<!-- docs-declaration -->

```csharp
public static string Codex(string value)
public static string Codex(ReadOnlySpan<char> value)
```

**Parameters** — `value` is a single name: any alphabetic character — a letter, a letter number, or
a mark the Unicode Alphabetic property takes in, such as a Devanagari vowel sign — plus a space is
accepted; a digit, punctuation or any other whitespace is refused rather than ignored. Case does not
matter: the name is uppercased by the full case mapping, so `ß` reads as `SS`. The `string`
overload forwards to the span one.

**Returns** — `string`, an uppercase code, or the empty string when `value` is empty. A code over
six UTF-8 bytes keeps its first three and last three characters.

**Exceptions** — `ArgumentNullException` when `value` is `null` (the `string` overload only).
`ArgumentException` when `value` holds a character that is neither alphabetic nor a space.

**Example** — doubles collapse before non-leading vowels are dropped, and a code over six
characters keeps only its first three and last three.

```csharp
using Lodestar.Text.Phonetics;

string smith = MatchRatingApproach.Codex("Smith");             // => SMTH
string mississippi = MatchRatingApproach.Codex("Mississippi"); // => MSSP
string bhattacharya = MatchRatingApproach.Codex("Bhattacharya");   // => BHTHRY
```

**Remarks** — `Mississippi` reducing to `MSSP` is doubled letters collapsing *before* the
non-leading vowels between them are dropped: the raw sequence `M-I-S-S-I-S-S-I-P-P-I` collapses
its adjacent-letter runs first (`M-I-S-I-S-I-P-I`), and only then loses every vowel but the first
character, whatever it is — a leading vowel is kept, as in `Codex("aeiou")` → `"A"`.

`Bhattacharya` reducing to six characters (`BHTCHRY` would be seven) keeps its first three and its
last three: `BHT` + `HRY`. The six is counted in UTF-8 **bytes**, as `jellyfish` counts it, so a
four-character name outside ASCII can be truncated too, and repeat its middle:
`Codex("並丝七世")` is `並丝七丝七世`, jellyfish's own answer
([decision 0009](../../../decisions/0009-the-phonetic-encoders-follow-jellyfish-whole.md)).

A character that is neither alphabetic nor a space throws instead of being dropped —
`Codex("O'Brien")` and `Codex("Anne-Marie")` both raise `ArgumentException`, naming the character
that stopped them — where [`Compare`](matchratingapproach-compare.md) answers `null` for the same
input. [`Soundex.Encode`](soundex-encode.md), [`Metaphone.Encode`](metaphone-encode.md) and
[`Nysiis.Encode`](nysiis-encode.md) accept it, each by its own rule; all four are jellyfish's.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MatchRatingApproach`](matchratingapproach.md),
[`MatchRatingApproach.Compare`](matchratingapproach-compare.md),
[the phonetics index](../phonetics.md).
