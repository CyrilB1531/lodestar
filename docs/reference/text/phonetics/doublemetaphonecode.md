# DoubleMetaphoneCode

A word's two Double Metaphone codes.

<!-- docs-declaration -->

```csharp
public readonly record struct DoubleMetaphoneCode
```

**Example** — the two codes, and what an empty alternate means.

```csharp
using Lodestar.Text.Phonetics;

DoubleMetaphoneCode smyth = DoubleMetaphone.Encode("Smyth");
string primary = smyth.Primary;  // => SM0
string secondary = smyth.Secondary;  // => XMT

DoubleMetaphoneCode wright = DoubleMetaphone.Encode("Wright");
string one = wright.Primary;  // => RT
string alternate = wright.Secondary;  // =>
```

**Remarks** — `Secondary` is empty when the word has one reading, which is most words. The
reference repeats the primary there; empty is the convention `jellyfish`, `metaphone` and
`phonetics` share, and
[decision 0075](../../../decisions/0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)
takes it for this API so that "has an alternate" is a property a caller can test rather than a
comparison it has to make.

Being a `record struct`, two codes compare equal by value, which makes the pair usable as a
dictionary key when grouping a corpus by sound. That is not the same as a phonetic match: equality
here is *both* codes agreeing, while two words are candidates when **any** of their codes agree.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DoubleMetaphone.Encode`](doublemetaphone-encode.md),
[`DoubleMetaphone`](doublemetaphone.md), [the phonetics index](../phonetics.md).
