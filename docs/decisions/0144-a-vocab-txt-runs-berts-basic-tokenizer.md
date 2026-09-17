---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0144 — A vocab.txt runs BERT's BasicTokenizer, by default

**Status:** accepted · **Date:** 2026-09-17

## Context

`VocabTxtLoader` plus `WordPieceTokenizer` is documented as the route for stock BERT, because a
BERT `tokenizer.json` declares a `BertNormalizer` and a `BertPreTokenizer` that
[`TokenizerJsonLoader.LoadWordPiece`](../reference/embeddings/persistence/tokenizerjsonloader-loadwordpiece.md) refuses. The route did not run those steps either: it split
text as `pre_tokenizers.Whitespace()` does and lowercased with nothing else
([#883](https://github.com/CyrilB1531/lodestar/issues/883)).

Measured on the `bert-base-uncased` vocabulary against `tokenizers` 0.23.2 with
`BertNormalizer(clean_text, handle_chinese_chars, strip_accents=None, lowercase=True)` and
`BertPreTokenizer`, **12 of 18 inputs gave different ids**. `café`, `naïve résumé` and `Ångström`
became `[UNK]` where BERT reads `cafe`, `naive resume` and `ang ##strom`; `wait...` kept `...`
whole where BERT cuts three `.`; `中文字` stayed one word; a zero-width space or a soft hyphen became
`[UNK]` where BERT drops it.

## Decision

`WordPieceVocabulary` gains `BasicTokenization`. When it is set, `WordPieceTokenizer` normalizes
every stretch of text the raw added tokens left as `BertNormalizer` does (NUL, U+FFFD and the
Cc, Cf, Cs, Co and Cn characters but tab, newline and return dropped; whitespace to a space; CJK
ideographs padded; the NFD form's nonspacing marks stripped, then lowercased, when `Lowercase` is
set), and splits it as `BertPreTokenizer` does (whitespace, and each punctuation character on its
own). **`VocabTxtLoader` sets it**, so the route documented for BERT tokenizes as BERT does
without a flag. `TokenizerJsonLoader` leaves it off, since the pipelines it accepts are the
`Whitespace` ones.

## Options refused

**An opt-in setting, off by default.** It keeps every existing call's ids, and leaves the route
the documentation calls stock BERT producing `[UNK]` for every accented word of an uncased model
unless the caller knows to ask. The maintainer chose to change the behaviour while the package is
at 0.x.

**Documentation only**, dropping the stock-BERT claim. The gap is the loader's apparatus, which
is what this project exists to close ([0068](0068-the-tokenizer-gap-is-the-loader-not-the-encode-kernel.md)).

## Consequences

- Ids change on the `vocab.txt` route for any text with punctuation runs, accents under an uncased
  model, CJK ideographs, control or format characters. `CHANGELOG.md` says so under *Changed*.
  `vocab with { BasicTokenization = false }` restores the previous split.
- `vocab_txt.json` replays the pipeline on a cased and an uncased model, the eighteen measured
  inputs included.
- `TokenizerJsonLoader` still refuses a full `BertNormalizer` and a `BertPreTokenizer`. Accepting
  them now needs only the loader to set the flag, which is a separate change.
