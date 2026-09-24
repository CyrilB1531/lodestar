# Lodestar.Onnx

Runs a transformer encoder exported to ONNX and pools its token outputs into a sentence
embedding, through ONNX Runtime. It was split out of `Lodestar.Embeddings` so that tokenizing,
pooling or searching never restores a native runtime; install this package only when you run the
model in-process.

The model file is yours: this repository never ships model weights.

## Install

```bash
dotnet add package Lodestar.Onnx
```

## Example

```csharp
using Lodestar.Onnx;

using var embedder = new OnnxTextEmbedder("model.onnx");

long[] ids = [101, 2054, 2003, 102];
long[] mask = [1, 1, 1, 1];

float[] vector = embedder.Embed(ids, mask);
```

## Parity

Embeddings are replayed against the same model run through ONNX Runtime's Python
binding.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A satellite package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Embeddings` 0.8.0 or later
- `Microsoft.ML.OnnxRuntime` 1.30.0 or later

## Documentation

- Guide: [onnx](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/onnx.md)
- Reference: [onnx/inference](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/onnx/inference.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Onnx/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Onnx/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
