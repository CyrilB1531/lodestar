# Changelog — Lodestar.Onnx

What changed in `Lodestar.Onnx`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- The `Lodestar.Embeddings` dependency floor rises from 0.6.0 to 0.8.0, the release that forwards its data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

## [0.1.1] — 2026-09-24

### Changed

- `Microsoft.ML.OnnxRuntime` moves from 1.28.0 to 1.30.0. ([#622](https://github.com/CyrilB1531/lodestar/issues/622), [`8603bb01`](https://github.com/CyrilB1531/lodestar/commit/8603bb01))
- The `Lodestar.Embeddings` dependency floor rises from 0.5.0 to 0.6.0. ([#682](https://github.com/CyrilB1531/lodestar/issues/682), [`afc1909d`](https://github.com/CyrilB1531/lodestar/commit/afc1909d))

### Fixed

- `OnnxTextEmbedder`'s constructors dispose the ONNX Runtime session they opened when they throw, check the tokenizer before opening one, and name `outputName` for an undeclared output. ([#903](https://github.com/CyrilB1531/lodestar/issues/903))

## [0.1.0] — 2026-09-08

### Added

- **First release, 0.1.0: ONNX inference, and the satellite tier's first member.** One type, `OnnxTextEmbedder`, moved verbatim from `Lodestar.Embeddings` into namespace `Lodestar.Onnx` — every package sets `RootNamespace` equal to its `PackageId`, and the rename is also what let the split land without colliding with the copy published in `Lodestar.Embeddings` 0.4.0 and 0.5.0. It depends on `Lodestar.Embeddings` 0.5.0 for the tokenizers, the encoding options and the pooling it feeds a session with, and on `Microsoft.ML.OnnxRuntime` 1.28.0, which no other package in the repository now references. Ships `net10.0;netstandard2.0` like the rest. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))
