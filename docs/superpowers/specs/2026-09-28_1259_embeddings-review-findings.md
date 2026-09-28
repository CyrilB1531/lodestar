# Lodestar.Embeddings' tokenizer.json Unigram path follows tokenizers

**Issues:** [#1259](https://github.com/CyrilB1531/lodestar/issues/1259),
[#1260](https://github.com/CyrilB1531/lodestar/issues/1260).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The review of `main` at `9b142ab2` found the `tokenizer.json` Unigram path answering as
sentencepiece where the file is `tokenizers`':

1. `SentencePieceTokenizer` hard-coded `remove_extra_whitespaces`, so `"a  b"` collapsed to
   `['▁a', '▁b']` where `tokenizers` keeps `['▁a', '▁', '▁b']`, and a trailing space was trimmed
   (#1259). Measuring it showed a second half: `tokenizers`' `Metaspace` splits before each `▁`, so
   two unknown characters either side of a space stay two tokens, where one Viterbi pass over the
   whole text fused them — on `bench/corpus/vocabs/tokenizer_30k_unigram.json`, `"cat a"` came out
   `['▁ca', 't▁a']`.
2. `PrecompiledNormalizer` implements sentencepiece's longest-match walk. `tokenizers`'
   `spm_precompiled` walks by grapheme and replaces a cluster under six UTF-8 bytes whole by the
   shortest rule matching its start, dropping the marks after it: XLM-R's `nmt_nfkc` on NFD
   `"phở"` is `phở` to sentencepiece and `phơ` to `tokenizers` (#1260).

## Decisions

- **Align the `tokenizer.json` path with `tokenizers`, keep the `.model` path on sentencepiece.**
  The issue left #1260 open between aligning and recording the divergence; the project aligns
  with the reference each format names. The two formats now part on NFD text exactly as the two
  libraries do, and `docs/equivalence.md` says so.
- **`SentencePieceVocabulary` carries the two Metaspace settings**, `RemoveExtraWhitespaces`
  (default on, a `.model`'s) and `SplitsAtMetaSymbol` (default off); the `tokenizer.json` loader
  sets them off and on. The split is a stop in the Viterbi walk — no piece crosses a later `▁` — and
  an unknown run does not fuse across one, which is the segmentation without a second pass.
- **`PrecompiledNormalizer.ByGrapheme`**, internal, gives the loader `tokenizers`' reading of the
  same charsmap; equality tells the two readings apart. The graphemes are segmented here rather than
  by `StringInfo`, whose netstandard2.0 rules are not UAX #29's; only clusters under six bytes need
  exact boundaries, since a longer one is read character by character either way.

## Verification

- `tokenizer_json.json` gains eight Unigram cases appended last (runs of spaces, leading, trailing,
  tab, and uncovered characters beside spaces); `precompiled_grapheme.json` is new, 303 strings
  normalized by `tokenizers.normalizers.Precompiled` over `xlmr_fairseq.model`'s charsmap.
- Random differentials against `tokenizers` 0.23.2: 2,000 whitespace-heavy texts on the 30k Unigram
  file, `main` 1,617 mismatches, this branch 0; 5,003 base-plus-marks texts, a third in NFD, on a
  Unigram `tokenizer.json` built from XLM-R's vocabulary and `nmt_nfkc` charsmap, `main` 2,210,
  this branch 0.
- A/B/A against `main` at `5b9ce3fc` on the developer machine (BenchmarkDotNet): the Unigram
  `tokenizer.json` encode 7 % faster and allocating 8.19 MB where it allocated 5.43 MB — more
  tokens, every space kept and no unknown run fused across one; the `.model` normalizer and encode
  and the incumbent comparisons within 2 %.
