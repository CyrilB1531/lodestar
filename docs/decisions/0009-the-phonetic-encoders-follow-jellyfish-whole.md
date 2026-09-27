---
status: accepted
supersedes: []
amends: ["0005", "0007"]
applies: []
---
# 0009 — The phonetic encoders follow jellyfish whole

**Status:** accepted · **Date:** 2026-09-27

## Context

[`0005`](0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md) holds Soundex,
NYSIIS, Metaphone and the match rating to `jellyfish` 1.2.1, and draws the scope of that parity
short of jellyfish's whole behaviour: Metaphone is proven **on real words**, jellyfish's answers on
degenerate letter sequences being *its C implementation's, of no value to reproduce*.
[`0007`](0007-the-deliberate-divergences.md) records a second boundary as a deliberate divergence:
jellyfish measures a match rating codex in UTF-8 **bytes** for the truncation, the length-gap check
and the minimum-rating lookup, and the C# measured characters.

The pre-1.0 review of 2026-09-26 found the boundary drawn in the wrong place
([#1194](https://github.com/CyrilB1531/lodestar/issues/1194),
[#1195](https://github.com/CyrilB1531/lodestar/issues/1195),
[#1198](https://github.com/CyrilB1531/lodestar/issues/1198)). The divergences were not confined to
non-words: 54 of 5,402 real ASCII words differed in Metaphone (`Fletcher`, `Mitchell`, `Highness`),
9 in NYSIIS (`Reeves`, `Fischer`), and an apostrophe — `Keats's`, `O'Brien` — changed the Soundex,
NYSIIS and match rating answers of every possessive in the dictionary, because the C# dropped what
jellyfish reads. The implementation behind jellyfish 1.2.1 is Rust, not C, and its rules are
visible in behaviour: a full uppercase mapping (`ß` is `SS`), a decomposition or a grapheme cluster
as each encoder takes one, and every other character standing where it is.

## Decision

**Soundex, NYSIIS, Metaphone and the match rating reproduce `jellyfish` 1.2.1 on every input, not
on real words only.** The input is uppercased by the full case mapping; Soundex and Metaphone read
its NFKD decomposition, NYSIIS and the match rating its extended grapheme clusters; a character no
rule names is kept where it stands, and acts there as jellyfish's rules make it act. The match
rating measures its codex in UTF-8 bytes, as jellyfish does, and `Compare` answers `null` where
jellyfish answers `None` for a name the codex refuses.

This amends `0005`'s phonetics row: the Metaphone corpus is no longer the boundary of the claim,
and degenerate sequences are inside it. It withdraws `0007`'s match rating row: byte length is no
longer a divergence, so `Codex("並丝七世")` is jellyfish's `並丝七丝七世`. The rules are
reproduced from what jellyfish answers, as [`0002`](0002-provenance-and-the-allowed-references.md)
allows, and each is proven by its oracle, not by the source.

## Consequences

- **Two Unicode facts .NET does not expose are tables in `Lodestar.Text`**: the scalars whose full
  uppercase differs from `char.ToUpperInvariant`, and the Other_Alphabetic marks
  `char::is_alphabetic` admits. Both were measured over every scalar against jellyfish 1.2.1 and
  Python's `unicodedata` 15.0; a scalar Unicode assigned after that follows the runtime's tables,
  and a grapheme boundary follows the runtime's segmentation, which on .NET Framework predates
  extended clusters.
- **A differential test replaces the real-word boundary**: 134,334 inputs — the dictionary and
  30,000 random strings over apostrophes, spaces, `ß`, combining marks, emoji and letter numbers —
  agree with jellyfish on all four encoders, and 33,003 pairs on the comparison. The corpora gain
  the names and the non-ASCII cases that used to sit outside the claim.
- **Metaphone's corpus, `metaphone.json`, is no longer special**: it stays because its names cover
  the rules, not because it bounds what is promised.
