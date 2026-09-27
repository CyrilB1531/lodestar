# Metaphone.Encode

The Metaphone code of one word.

<!-- docs-declaration -->

```csharp
public static string Encode(ReadOnlySpan<char> value)
public static string Encode(string value)
```

**Parameters** — `value` is a single word, or several separated by spaces. Case does not matter:
the word is uppercased by the full case mapping and decomposed (NFKD), so `é` reads as `E` and `ß`
as `SS`. A character no rule names — an apostrophe, a digit — writes nothing, but it still stands
where it is, so it parts a doubled letter: `Keats's` keeps both of its `S` sounds. The `string`
overload forwards to the span one.

**Returns** — `string`, a variable-length code drawn from `B X S K J T F H L M N P R 0 W Y`, with
a space between the codes of two words, or the empty string when nothing in `value` sounds.

**Exceptions** — `ArgumentNullException` when `value` is `null` (the `string` overload only; a
`ReadOnlySpan<char>` cannot be null). An empty string is accepted and encodes to the empty string.

**Example** — the digraphs that make the alphabet look strange.

```csharp
using Lodestar.Text.Phonetics;

string th = Metaphone.Encode("Thomas");  // => 0MS
string sh = Metaphone.Encode("Christina");  // => XRSTN
string silent = Metaphone.Encode("Knight");  // => NT
```

**Remarks** — `0` and `X` are not placeholders. `0` is the "th" sound and `X` the "sh" sound, both
written as single characters so a code stays one character per sound. This is why a Metaphone code
must never be shown to a user: `0MS` is correct and unreadable.

`Knight` → `NT` is the behaviour that distinguishes this from the other two encoders, and it is
also the reason to prefer it for ordinary English words over `Soundex.Encode`. Silent `k`, `w`,
`g` and `b` are all handled, along with `GH`, `DGE`, `-TION` and the rest of the spellings English
uses for sounds it does not write plainly.

`T` goes silent before `CH`, and `GH` before a consonant: `Fletcher` is `FLXR` and `Highness` is
`HNS`. Every rule is jellyfish's, on any input at all — a random letter sequence such as
`xhdzhumzj` encodes to jellyfish's own `XHTSHMSJ` — which is what
[decision 0009](../../../decisions/0009-the-phonetic-encoders-follow-jellyfish-whole.md) settles.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Metaphone`](metaphone.md), [`Soundex.Encode`](soundex-encode.md),
[the phonetics index](../phonetics.md).
