# 0887 — The Whitespace pre-tokenizer, over code points

**Status:** **retrospective** — written 2026-09-17, after the measurement it records.

Issue: [#887](https://github.com/CyrilB1531/lodestar/issues/887), found by the systematic review of `main`.

## Problem

`tokenizers.pre_tokenizers.Whitespace()` is `\w+|[^\w\s]+` compiled by Oniguruma, whose Unicode
`\w` is Alphabetic ∪ Mark ∪ `Nd` ∪ `Pc` ∪ Join_Control, over code points. `WordPieceTokenizer` and
`BpePreTokenizer`, for `BpePatterns.Whitespace`, compiled the same string as a .NET `Regex`, whose
`\w` is `L`, `Mn`, `Nd`, `Pc`, over UTF-16 units.

Each code point `c` was run as `"a" + c + "a"` through both. Over the 139,751 code points assigned
from U+0020 to U+2FFFF, `WordPieceTokenizer` disagreed on 79,831:

- 78,008 astral letters, marks and digits, which the regex sees as two `Cs` surrogates — CJK
  Extensions B to G among them;
- 260 `Mc` (Devanagari vowel signs), 13 `Me`, 65 `Nl` (`Ⅻ`), ZWNJ and ZWJ;
- 52 `So` that are Other_Alphabetic: the circled letters U+24B6–U+24E9.

The BPE path disagreed on 1,957 only, because the shadow written for issue #341 already turned an
astral letter or digit into one placeholder — but it also turned an astral `No` into a digit, which
the reference splits (615 of the 1,957).

## Options

- **Widen the class** to `[\w\p{M}]+|…`. Fixes `Mc` and `Me` only: 79,558 differences remain, and no
  .NET character class can tell an astral letter from an astral symbol, since both are surrogates.
- **Extend #341's shadow** to the Whitespace pattern. It would need a placeholder per class and still
  a regex pass over a rebuilt string, to recover what one classification per code point gives.
- **A code-point scanner** (chosen). The pattern is two alternatives of one character class and its
  complement minus whitespace, so its matches are the maximal runs of word characters and of
  "other" characters, which a single forward walk finds without a regex.

## Change

`WhitespaceScanner` classifies each code point: ASCII through a table, a surrogate pair through
`CharUnicodeInfo.GetUnicodeCategory(string, int)` — present on both target frameworks, where `Rune`
is not — and everything else through `char.GetUnicodeCategory`. A word character is a letter, any
of the three marks, `Nd`, `Nl`, `Pc`, U+200C/U+200D, or U+24B6–U+24E9, U+1F130–U+1F149,
U+1F150–U+1F169, U+1F170–U+1F189: the `So` code points the sweep found inside words, all of them
Other_Alphabetic. Whitespace is the `White_Space` property, spelled out rather than read from
`char.IsWhiteSpace`, which follows an older table on .NET Framework and Mono. The sweep showed
exactly those 25 code points split as whitespace, U+180E not among them.

`WordPieceTokenizer` walks the scanner over its normalized segment. `BpePreTokenizer` uses it in
place of a compiled regex wherever a pattern is ordinal-equal to `BpePatterns.Whitespace`, as a
`Split` step's or as the last pattern, so the public string does not change and a caller who
writes it gets the reference's split. A `Split` step declaring that regex in `tokenizers` is also
compiled by Oniguruma, so the scanner is right there too — measured for `Isolated` on
`"a\U00020000a b"`.

## Measured

**Sweep.** Every one of the 1,112,064 Unicode scalar values between two `a`s, through the reflected
`BpePreTokenizer` `BpeTokenizer` builds and through `WordPieceTokenizer.Encode` with a vocabulary
holding every code point and its `##` form, against `tokenizers` 0.23.2 on .NET 10: **0**
differences on either path, so no Unicode-version residue between .NET 10's tables and Oniguruma's
for this split. 50,000 random strings of 1 to 12 code points, 60 % drawn from ASCII and the classes
above, also agree on the BPE path; on the WordPiece path the one string that differs contains
`##^`, which the harness reads back as a continuation piece, not a split difference.

**Oracles.** `bpe.json` and `wordpiece.json` gain nine texts each, appended — Devanagari with vowel
signs, an `Me`, a Roman numeral, ZWJ/ZWNJ inside words, a circled letter, CJK Extension B, an emoji,
`²`. `tools/compare_oracles.py` reports only the case counts moving (20 → 29, 9 → 18); the earlier
cases and the metadata are identical.

**Speed.** Stopwatch, fastest of 10 rounds after 3 warm-up rounds, processes alternated `main`, fix
three times, over `bench/corpus/vocabs/documents.json` (5,000 lowercase ASCII documents) and a
generated prose of the same lengths with capitals, punctuation, digits and Latin-1 accents.
`tokenizer_30k_wordpiece.json` for WordPiece. The load average was 11 from other work throughout;
the checksums (pieces and ids) are identical on both sides.

| path | `main` | fix |
| --- | ---: | ---: |
| `BpePreTokenizer.Split`, documents | 10.46 ms | **7.30 ms** |
| `BpePreTokenizer.Split`, prose | 22.39 ms | **8.71 ms** |
| `WordPieceTokenizer.Encode`, documents | 20.59 ms | **12.70 ms** |
| `WordPieceTokenizer.Encode`, prose | 29.18 ms | **14.85 ms** |
