# OnnxTextEmbedder's chunk buffers are not pooled

**Issues:** [#1598](https://github.com/CyrilB1531/lodestar/issues/1598).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Onnx` after #1596 found the `Embed` page pricing a static export's chunk
buffers as rented from `ArrayPool`, rounded up to a power of two. `Run` allocates `chunkIds` and
`chunkMask` fresh, at their exact size, on every chunked call; only the unpadded copies `Embed` and
`RunBatch` make are pooled.

## Decisions

- **The page follows the code.** The ids and mask are pooled when the input is fed as it is, and
  allocated at exact size each call when the embedder chunks, the pooled copies held alongside.
  Pooling the chunk was not taken up here: this issue is the page's, and a pool would keep the
  largest chunk a process ever made.
- **The allocation's comment follows #1589**: the refusal there only fires with one axis fixed,
  since the constructor refuses two fixed axes whose product passes one array; two smaller fixed
  axes still reach the chunk path.
