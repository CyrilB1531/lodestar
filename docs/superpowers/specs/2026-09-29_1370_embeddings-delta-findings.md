# The Embeddings findings of the Review B after #1361 and #1362

**Issues:** [#1370](https://github.com/CyrilB1531/lodestar/issues/1370) to
[#1378](https://github.com/CyrilB1531/lodestar/issues/1378).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of what #1361 and #1362 shipped, on `main` at `3088ca29`, found the fixes applied to
some members of a class and not their siblings: `SentencePieceVocabulary` and `BpeVocabulary` still
hashed an absent list by throwing; `BpeTokenizer` and `WordPieceTokenizer` still dereferenced a
missing list or a null added token, and WordPiece took a negative added-token id; `MeanPoolBatch`,
`PrecompiledNormalizer`'s buffers and `EmbeddingIndex`'s id array were bounded at `int` overflow or
not at all rather than at the largest array; and three XML comments were stale or missing.

## Decisions

- **The same helpers as the siblings**: `ValueEquality.Same` and `LengthOf` where the element is
  `IEquatable`, a local loop for the two enum lists, which are not.
- **One up-front check per tokenizer constructor**, refusing under `vocabulary`, rather than a
  guard at each dereference.
- **Every bound is `TableLength.MaxLength`**, computed in `long`; the normalizer takes the
  `GetMaxByteCount` bound only where it fits one array and the exact count otherwise.

## Verification

- `ReviewBFindingsTests` gains four facts, one per behaviour a caller can reach without allocating
  gigabytes; the normalizer's and the id array's bounds are argued, not exercised.
- Review A found `EmbeddingIndex.Load`'s id array growing unclamped too, fixed here, and the null
  `Types`, `PrefixTokens` and `SuffixTokens` branches untested, now covered.
