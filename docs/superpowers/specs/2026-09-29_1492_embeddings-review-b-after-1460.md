# The Embeddings findings of the Review B after #1397 and #1460

**Issues:** [#1492](https://github.com/CyrilB1531/lodestar/issues/1492) to
[#1506](https://github.com/CyrilB1531/lodestar/issues/1506).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Embeddings` after #1397 and #1460, on `main` at `1cf61032`, found
fifteen defects.

In the code:

- #1436's overflow catch caught a type `GetByteCount` never throws.
- The loaders refused a null stream as `stream`, and `BpeFilesLoader` did so only after reading
  the other file.
- A `.model` defining a piece twice loaded, keeping the last definition, where sentencepiece
  refuses it.
- A negative `maxCharsPerWord` and an undefined truncation strategy were accepted.
- `EncodeBatch` refused an over-large batch under `Pad`'s parameter name rather than its own.

In the documentation:

- Exceptions went undocumented: the trie's, the regex timeout, the charsmap's lazy
  `InvalidDataException`, the constructors' and the path overloads'.
- `ArtifactLoadOptions` misstated what bounds an index.
- The piece limit was misstated.
- The records did not say their collections are live.
- `docs/equivalence.md` lacked a row for #1434.
- Three tests pinned, named or cited the wrong thing.

## Decisions

- **The overflow is a plain `ArgumentException`.** Measured on .NET 10:
  `GetByteCount(new string('一', 800_000_000))` throws `ArgumentException("Conversion buffer
  overflow.")` with no parameter name. Both catches now take `ArgumentException` but exclude
  `EncoderFallbackException`, which derives from it and means a lone surrogate, not a length.
- **Repeated pieces follow sentencepiece's two maps.** Normal, user-defined and unused pieces
  share one map; control, unknown and byte pieces share the other. A repeat within either is
  refused; the same string once in each loads.
- **`byte_fallback` in BPE.** The reference pages say the loader accepts it as decision 0007
  describes. Decision 0005(b)'s BPE row, which says it is refused, predates that support; the
  owner decided to leave the record as written.

## Rejected

- **Widening `CharTrie.Trim`'s `maxBase + alphabet + 1` to `long`.** `Step` checks its bounds, so
  the sum cannot reach an array.
