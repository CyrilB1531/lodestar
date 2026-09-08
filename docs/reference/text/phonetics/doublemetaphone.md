# DoubleMetaphone

Double Metaphone (Lawrence Philips, 2000): one word, **two** codes, so a name that is pronounced
more than one way can match on either.

<!-- docs-declaration -->

```csharp
public static class DoubleMetaphone
```

**Example** — the pair that is the reason this encoder exists.

```csharp
using Lodestar.Text.Phonetics;

DoubleMetaphoneCode smith = DoubleMetaphone.Encode("Smith");
DoubleMetaphoneCode schmidt = DoubleMetaphone.Encode("Schmidt");

string a = smith.Secondary;  // => XMT
string b = schmidt.Primary;  // => XMT
```

**Remarks** — the other four encoders in this namespace answer with one code, which forces a
single verdict on a spelling that has more than one plausible reading. `Smith` is English and
sounds like `SM0`; it is also the anglicisation German speakers write as `Schmidt`, which sounds
like `XMT`. A single code has to pick one and lose the other match. Double Metaphone returns both,
and two words are candidates when **any** of their codes agree — which is what puts `Smith` and
`Schmidt` together above without putting every `S`-word with them.

`Secondary` is empty when a word has only one reading, which is the common case: over the 423-word
corpus this is pinned to, 102 words carry an alternate and 321 do not. Empty means *no alternate*,
not *no code* — compare against `Primary` when it is empty.

The output alphabet is `A F H J K L M N P R S T X 0`, where `X` is "sh" and `0` is "th". Codes are
**not truncated** to four characters: the reference does not truncate, and a caller who wants the
classic four can take them from the front.

Reference behaviour is `doublemetaphone.doublemetaphone` 1.2.
[Decision 0075](../../../decisions/0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)
records why that library rather than `jellyfish` — which exports no Double Metaphone at all — and
the 401-word comparison that showed the two independent permissive implementations agree on every
primary, so the choice freezes an algorithm rather than one library's dialect.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Metaphone`](metaphone.md), [`Nysiis`](nysiis.md),
[the phonetics index](../phonetics.md).

## Members

| Member | What it does |
| --- | --- |
| [`DoubleMetaphone.Encode`](doublemetaphone-encode.md) | The two Double Metaphone codes of one word. |
