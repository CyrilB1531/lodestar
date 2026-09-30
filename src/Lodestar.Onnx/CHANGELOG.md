# Changelog — Lodestar.Onnx

What changed in `Lodestar.Onnx`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- The `Lodestar.Embeddings` dependency floor rises from 0.6.0 to 0.8.0, the release that forwards its data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

### Fixed

- The `OnnxTextEmbedder.Embed` page says a static export's chunk buffers are allocated at their exact size on every call rather than pooled. ([#1598](https://github.com/CyrilB1531/lodestar/issues/1598))
- `OnnxTextEmbedder` refuses, when it is opened and under `modelPath`, a model whose fixed batch and sequence axes alone make one chunk larger than one array, where every call was refused under an argument no value could fix, and the `Embed` page gives what a half-precision or pooled output costs. ([#1589](https://github.com/CyrilB1531/lodestar/issues/1589), [#1590](https://github.com/CyrilB1531/lodestar/issues/1590))
- `OnnxTextEmbedder.EmbedBatch` over texts names `options` when a static chunk passes one array, where it named `encoder`, and the pages state the bound as `Array.MaxLength` cells and what a chunk below it costs. ([#1581](https://github.com/CyrilB1531/lodestar/issues/1581), [#1582](https://github.com/CyrilB1531/lodestar/issues/1582))
- `OnnxTextEmbedder.MaxSequenceLength` throws `ObjectDisposedException` after `Dispose` as every other member does, and a static export whose fixed axes make one chunk larger than one array is refused with an `ArgumentException` before allocating it. ([#1552](https://github.com/CyrilB1531/lodestar/issues/1552), [#1553](https://github.com/CyrilB1531/lodestar/issues/1553), [#1554](https://github.com/CyrilB1531/lodestar/issues/1554), [#1555](https://github.com/CyrilB1531/lodestar/issues/1555))
- `OnnxTextEmbedder.MaxSequenceLength` falls back, on a symbolic sequence axis, to the rows of the graph's position-embedding table net of a RoBERTa-style padding offset, so `EmbedBatch` truncates to the model's limit where it truncated nothing and a longer text failed inside ONNX Runtime. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- `OnnxTextEmbedder` refuses a model output that is not `[batch, sequence, dim]` or `[batch, dim]` for the batch it fed, naming the output and both shapes, where a transposed output was pooled over the wrong rows. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- `OnnxTextEmbedder` widens a float16 or bfloat16 output to float before pooling, and refuses any other element type by name with `NotSupportedException`, where both failed with an invalid cast. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- `OnnxTextEmbedder` refuses a null input name under its own parameter before opening the model, names `attentionMask` for spans of different lengths, and `EmbedBatch` documents the refusals it passes on. ([#1343](https://github.com/CyrilB1531/lodestar/issues/1343), [#1344](https://github.com/CyrilB1531/lodestar/issues/1344), [#1345](https://github.com/CyrilB1531/lodestar/issues/1345))
- `OnnxTextEmbedder`'s tokenizer constructor documents the exceptions it throws. ([#1377](https://github.com/CyrilB1531/lodestar/issues/1377))
- `OnnxTextEmbedder` feeds a static export its fixed batch and sequence axes, padding each row with masked positions and running the rows in chunks of the fixed batch size, where ONNX Runtime refused every batch it was handed. ([#1258](https://github.com/CyrilB1531/lodestar/issues/1258))
- `OnnxTextEmbedder` refuses a sequence or a `MaxLength` past a position-embedding table's `MaxSequenceLength` by name, where it failed inside ONNX Runtime. ([#1423](https://github.com/CyrilB1531/lodestar/issues/1423))
- `OnnxTextEmbedder` refuses an output whose declared axes swap the input's batch and sequence, which sizes alone missed when the two are as long. ([#1424](https://github.com/CyrilB1531/lodestar/issues/1424))
- The reference pages, README, guide and test README say what `EmbedBatch` runs and takes, what `Dimension` returns, what the constructors take and throw, and what the parity is replayed against. ([#1425](https://github.com/CyrilB1531/lodestar/issues/1425), [#1426](https://github.com/CyrilB1531/lodestar/issues/1426), [#1427](https://github.com/CyrilB1531/lodestar/issues/1427), [#1428](https://github.com/CyrilB1531/lodestar/issues/1428), [#1429](https://github.com/CyrilB1531/lodestar/issues/1429), [#1430](https://github.com/CyrilB1531/lodestar/issues/1430), [#1431](https://github.com/CyrilB1531/lodestar/issues/1431), [#1432](https://github.com/CyrilB1531/lodestar/issues/1432))
- `OnnxTextEmbedder.Dimension` throws `ObjectDisposedException` after `Dispose`, named in full on both targets, and the position-table reader refuses a packed run past any tensor's rank rather than growing without bound. ([#1521](https://github.com/CyrilB1531/lodestar/issues/1521), [#1526](https://github.com/CyrilB1531/lodestar/issues/1526), [#1527](https://github.com/CyrilB1531/lodestar/issues/1527))
- `OnnxTextEmbedder`'s XML, pages and `docs/equivalence.md` row name the axis-swap refusal, ONNX Runtime's exception for a model it cannot read, the version that took spans and the one name not refused, and the tests pin #1423's three other refusals. ([#1522](https://github.com/CyrilB1531/lodestar/issues/1522), [#1523](https://github.com/CyrilB1531/lodestar/issues/1523), [#1524](https://github.com/CyrilB1531/lodestar/issues/1524), [#1525](https://github.com/CyrilB1531/lodestar/issues/1525), [#1528](https://github.com/CyrilB1531/lodestar/issues/1528))

## [0.1.1] — 2026-09-24

### Changed

- `Microsoft.ML.OnnxRuntime` moves from 1.28.0 to 1.30.0. ([#622](https://github.com/CyrilB1531/lodestar/issues/622), [`8603bb01`](https://github.com/CyrilB1531/lodestar/commit/8603bb01))
- The `Lodestar.Embeddings` dependency floor rises from 0.5.0 to 0.6.0. ([#682](https://github.com/CyrilB1531/lodestar/issues/682), [`afc1909d`](https://github.com/CyrilB1531/lodestar/commit/afc1909d))

### Fixed

- `OnnxTextEmbedder`'s constructors dispose the ONNX Runtime session they opened when they throw, check the tokenizer before opening one, and name `outputName` for an undeclared output. ([#903](https://github.com/CyrilB1531/lodestar/issues/903))

## [0.1.0] — 2026-09-08

### Added

- **First release, 0.1.0: ONNX inference, and the satellite tier's first member.** One type, `OnnxTextEmbedder`, moved verbatim from `Lodestar.Embeddings` into namespace `Lodestar.Onnx` — every package sets `RootNamespace` equal to its `PackageId`, and the rename is also what let the split land without colliding with the copy published in `Lodestar.Embeddings` 0.4.0 and 0.5.0. It depends on `Lodestar.Embeddings` 0.5.0 for the tokenizers, the encoding options and the pooling it feeds a session with, and on `Microsoft.ML.OnnxRuntime` 1.28.0, which no other package in the repository now references. Ships `net10.0;netstandard2.0` like the rest. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))
