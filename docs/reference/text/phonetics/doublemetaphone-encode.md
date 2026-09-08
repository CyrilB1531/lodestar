# DoubleMetaphone.Encode

The two Double Metaphone codes of one word.

<!-- docs-declaration -->

```csharp
public static DoubleMetaphoneCode Encode(ReadOnlySpan<char> value)
public static DoubleMetaphoneCode Encode(string value)
```

**Parameters** — `value` is a single word. Only its ASCII letters are read; case does not matter,
and everything else — accents, non-Latin scripts, digits, punctuation, spacing — is dropped rather
than rejected. The `string` overload forwards to the span one.

**Returns** — [`DoubleMetaphoneCode`](doublemetaphonecode.md), a pair of variable-length codes
drawn from `A F H J K L M N P R S T X 0`. `Primary` is the empty string when `value` holds no
ASCII letter; `Secondary` is the empty string when the word has no alternate pronunciation.

**Exceptions** — `ArgumentNullException` when `value` is `null` (the `string` overload only; a
`ReadOnlySpan<char>` cannot be null). An empty string is accepted and encodes to two empty codes.

**Example** — one reading, then two.

```csharp
using Lodestar.Text.Phonetics;

DoubleMetaphoneCode thomas = DoubleMetaphone.Encode("Thomas");
string only = thomas.Primary;  // => TMS
string none = thomas.Secondary;  // =>

DoubleMetaphoneCode knuth = DoubleMetaphone.Encode("Knuth");
string th = knuth.Primary;  // => N0
string t = knuth.Secondary;  // => NT
```

**Remarks** — match on **either** code. Two words are phonetic candidates when any of their codes
agree, so the test is four comparisons and not one; taking `Primary` alone throws away the reason
to use this encoder over [`Metaphone`](metaphone.md).

`Knuth` above is the ordinary shape of an alternate: `TH` is `0` to an English ear and a plain `T`
to a German one, and the encoder declines to choose. `Wright` encodes to `RT` with no alternate —
the silent `W` is not a disagreement, just English spelling, which this encoder models the same
way `Metaphone.Encode` does.

**Accents are dropped, not folded.** `élan` encodes as `lan` does, to `LN`, because the reference
keeps ASCII letters only. That is a real limitation on non-English names rather than a rounding of
one, and it is pinned by the corpus rather than asserted here, so what the reference does stays
the reference's to say.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DoubleMetaphone`](doublemetaphone.md),
[`DoubleMetaphoneCode`](doublemetaphonecode.md), [`Metaphone.Encode`](metaphone-encode.md),
[the phonetics index](../phonetics.md).
