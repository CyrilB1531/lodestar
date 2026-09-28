# Lodestar.Embeddings findings of the invariant sweep after #1320

**Issues:** [#1322](https://github.com/CyrilB1531/lodestar/issues/1322),
[#1323](https://github.com/CyrilB1531/lodestar/issues/1323),
[#1324](https://github.com/CyrilB1531/lodestar/issues/1324).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The Review B of `Lodestar.Embeddings` after #1320 (spec `2026-09-28_1305_review-b-invariants.md`)
found two allocations sized by caller counts and, in its final open review, one undocumented
exception:

1. `NpyFile.Write` took the whole block as bytes, so past 536 million floats — 1.4 million vectors
   of 384 dimensions — `MemoryMarshal.AsBytes` threw `OverflowException`, and the big-endian path
   allocated the whole block (#1322).
2. `BatchEncoder.Pad` allocated `count × width` twice in unchecked `int` (#1323).
3. `SentencePieceTokenizer.Encode` threw `EncoderFallbackException` on a lone surrogate under a
   Precompiled normalizer and read it as an unknown piece without one (#1324).

## Decisions

- **Every float block is written in 240 KiB slices of floats**, the `.npy` writer and the shared
  base64 writer alike: Review A found the base64 writer taking the whole block as bytes too, and its
  asynchronous twin counting `values.Length * 4` in `int`, which wrote an empty block, silently,
  between 536 million and 1.07 billion floats.
- **Neither writer produces what no read can take back.** `NpyFile.Write` refuses more than
  `int.MaxValue / 4` floats, the one block `Read` holds, and `EmbeddingIndex.Save` a block whose
  base64 is longer than the one byte array `Load` decodes it into; both check before the first byte, and the
  `path` overloads before the file is opened, so a refused write no longer truncates the file it
  would replace.
- **`Pad` sizes its tables through `TableLength.Of`** (#1314), naming `count`.
- **`SentencePieceTokenizer` refuses a lone surrogate up front**, with `ArgumentException`: neither
  `tokenizers` nor sentencepiece can be handed such a string, since a Python `str` holding one does
  not cross into Rust or C++. `BpeTokenizer` keeps its documented contract for the same input and
  `WordPieceTokenizer` its documented drop of it, as `BertNormalizer` treats control characters.
  `BatchEncoder.EncodeAll` and `EncodeBatch` report a refused text under `texts`, with its
  position, where the exception used to name `Encode`'s `text`.

## Verification

- `SweepFindingsTests` round-trips a `.npy` block of three slices and a remainder and an index past
  one base64 slice, synchronously and asynchronously; leaves a file whole on a refused write;
  refuses a lone surrogate on a model with and without a normalizer, and names `texts` from a batch; `BatchEncoderTests` refuses 50,000 windows of
  one 50,000-token sequence before allocating, naming `count`.
