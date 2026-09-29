# OnnxTextEmbedder feeds a static export its fixed axes

**Issues:** [#1258](https://github.com/CyrilB1531/lodestar/issues/1258).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

A static export — quantized or mobile — fixes `input_ids` at, say, `[batch, 128]` or `[1, 128]`.
`OnnxTextEmbedder` read a fixed sequence axis as the longest input, but `BatchEncoder.Pad` pads a
sub-batch to its own longest row, so `EmbedBatch(["hello"])` fed `[1, 3]`, and a fixed batch axis of
1 met the default `BatchSize` of 32: ONNX Runtime refused every call.

## Decisions

- **Shaped in `Lodestar.Onnx`, at the one call every path goes through**, not in `BatchEncoder`:
  the satellite builds against the published `Lodestar.Embeddings`, and `Embed` never reaches the
  encoder. Each row is widened to the fixed sequence axis with masked positions, and the rows run
  in chunks of the fixed batch axis, the last filled with masked rows whose outputs are dropped.
- **The padded id is 0**: a masked position is never read unmasked, and the mean leaves it out, so
  the vectors are the dynamic export's exactly.
- **A sequence past a fixed axis is refused, not truncated**, under the caller's parameter —
  `inputIds`, `options`, `encoder` or `batch` — rather than left to ONNX Runtime's message.
- **A dynamic export takes the path it took**: two comparisons decide there is nothing to reshape.

## Verification

- `tiny_embedder_static.onnx`, new in `tools/build_tiny_models.py`: `tiny_embedder.onnx` with its
  axes fixed at `[2, 16]`. `FixedAxesTests` embeds three texts, which run as two chunks, and a
  single sequence, which fills a chunk with a masked row, and asserts the dynamic export's vectors
  exactly; on `main` both throw ONNX Runtime's "invalid dimensions".
- Review A found a row past the fixed axis reaching ONNX Runtime undocumented, and a remark still
  promising no sub-batching; both fixed here, the first pinned by a test.
