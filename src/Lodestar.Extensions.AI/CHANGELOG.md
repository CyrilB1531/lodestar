# Changelog — Lodestar.Extensions.AI

What changed in `Lodestar.Extensions.AI`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- The `Lodestar.Embeddings` dependency floor rises from 0.6.0 to 0.8.0, the release that forwards its data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

## [0.1.1] — 2026-09-24

### Changed

- `Microsoft.Extensions.AI.Abstractions` moves from 10.9.0 to 10.10.0. ([#622](https://github.com/CyrilB1531/lodestar/issues/622), [`8603bb01`](https://github.com/CyrilB1531/lodestar/commit/8603bb01))
- The `Lodestar.Embeddings` dependency floor rises from 0.5.0 to 0.6.0. ([#682](https://github.com/CyrilB1531/lodestar/issues/682), [`afc1909d`](https://github.com/CyrilB1531/lodestar/commit/afc1909d))

### Fixed

- The generation reference page places the package in the interop tier, per decision 0089, where it called it a satellite. ([#949](https://github.com/CyrilB1531/lodestar/issues/949))

## [0.1.0] — 2026-09-10

### Added

- **`Lodestar.Extensions.AI` 0.1.0 — the ONNX embedding path, behind `Microsoft.Extensions.AI`'s own interface.** `OnnxEmbeddingGenerator` implements `IEmbeddingGenerator<string, Embedding<float>>` over an `OnnxTextEmbedder` and a `BatchEncoder`, so a Semantic Kernel pipeline or any `Microsoft.Extensions.AI` chain can hold Lodestar embeddings without knowing Lodestar. It adds **no arithmetic**: every vector is what `OnnxTextEmbedder.EmbedBatch` returned for that text, which is why the suite asserts identity with that overload exactly rather than within a tolerance, and why this package carries no oracle corpus of its own. It is the **second satellite**, and it exists rather than being a second dependency on `Lodestar.Onnx` because [decision 0076](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0076-a-core-package-carries-no-external-dependency.md) says an external dependency earns its own package named for it — so a caller who wants inference and not the AI abstractions still restores nothing extra. Three things are stated rather than left to be discovered: the returned task is **already completed**, since the model runs in the calling process and `Task.Run` would move the same CPU without telling the caller anything true; `EmbeddingGenerationOptions.Dimensions` is **checked, not honoured**, because an ONNX model's width is fixed at export, and a width the model provably does not produce is refused instead of silently ignored; and the generator **takes ownership** of the embedder, because `IEmbeddingGenerator` is `IDisposable` and a consumer holding it through the interface cannot see that disposing would otherwise leave a native session open. `GetService` answers for the metadata, for the generator itself, and for the underlying `OnnxTextEmbedder` — the only way to reach the single-sequence entry point through an interface that has no shape for it. ([#570](https://github.com/CyrilB1531/lodestar/issues/570))
