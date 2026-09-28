# The Onnx, Extensions.AI and Extensions.VectorData findings of the sweep after #1325

**Issues:** [#1338](https://github.com/CyrilB1531/lodestar/issues/1338),
[#1343](https://github.com/CyrilB1531/lodestar/issues/1343),
[#1344](https://github.com/CyrilB1531/lodestar/issues/1344) (its `OnnxTextEmbedder` half),
[#1345](https://github.com/CyrilB1531/lodestar/issues/1345),
[#1346](https://github.com/CyrilB1531/lodestar/issues/1346),
[#1353](https://github.com/CyrilB1531/lodestar/issues/1353),
[#1354](https://github.com/CyrilB1531/lodestar/issues/1354).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The Review B invariant sweep of `Lodestar.Embeddings`' satellites on `main` at `eb4d240a` found
seven gaps. A `LodestarVectorStoreCollection` grew its vector block with an `int` product that
wrapped negative near two million 1,536-wide records. Null names and keys surfaced as
`Dictionary`'s `ArgumentNullException("key")`, and a refused text surfaced under `texts`, a
parameter of the embedder rather than of `GenerateAsync`. No write observed its cancellation token.
Two pages misdescribed what the code does.

## Decisions

- **The growth is bounded in slots**: the largest array divided by the width, refused with
  `InvalidOperationException` once reached, so every `slot × width` offset fits an `int`.
- **A token already cancelled returns a cancelled task and writes nothing**, as a `Task`-returning
  member reports every other failure; a batch upsert checks once it has read the batch and before
  it writes any of it, keeping the all-or-nothing its remarks promise.
- **`GenerateAsync` rethrows a refused text under `values`**, recognising both `texts` and `text`:
  the second is what `Lodestar.Embeddings` before #1324 names, which the published floor still is.
- **The null input names are refused before the model opens**, so no native session is created
  only to be disposed.

## Verification

- `ReviewBFindingsTests` in the Onnx and VectorData suites, and one fact in
  `OnnxEmbeddingGeneratorTests`, pin each finding.
