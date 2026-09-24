# Lodestar.Extensions.AI

Exposes Lodestar's ONNX embedding path as `IEmbeddingGenerator<string, Embedding<float>>`,
so an application written against Microsoft.Extensions.AI can use a local model without a service
behind it. It adapts `OnnxTextEmbedder` and `BatchEncoder`; it computes nothing of its own.

## Install

```bash
dotnet add package Lodestar.Extensions.AI
```

## Example

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Extensions.AI;
using Lodestar.Onnx;
using Microsoft.Extensions.AI;

async Task<int> EmbedAsync()
{
    var tokenizer = new WordPieceTokenizer(VocabTxtLoader.Load("vocab.txt"));

    using IEmbeddingGenerator<string, Embedding<float>> generator = new OnnxEmbeddingGenerator(
        new OnnxTextEmbedder("model.onnx", tokenizer), new BatchEncoder(tokenizer));

    GeneratedEmbeddings<Embedding<float>> vectors =
        await generator.GenerateAsync(["a first sentence", "a second one"]);
    return vectors[0].Dimensions;
}
```

## Parity

It adds no computation; `Lodestar.Onnx` and `Lodestar.Embeddings` carry the parity.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A interop package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Onnx` 0.1.0 or later
- `Lodestar.Embeddings` 0.8.0 or later
- `Microsoft.Extensions.AI.Abstractions` 10.10.0 or later

## Documentation

- Reference: [extensions-ai/generation](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/extensions-ai/generation.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.AI/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.AI/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
