# OnnxEmbeddingGenerator

Presents an `OnnxTextEmbedder` as an `IEmbeddingGenerator<string, Embedding<float>>`.

<!-- docs-declaration -->

```csharp
public sealed class OnnxEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
```

**Constructor** — takes the embedder to adapt, the `BatchEncoder` it should tokenize with, and
optionally a model identifier to report. **It takes ownership of the embedder**: disposing the
generator disposes the session.

**Example** — the shape of a call. It is not executed: see below.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed (CONTRIBUTING.md, ADR 0003) -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Extensions.AI;
using Lodestar.Onnx;
using Microsoft.Extensions.AI;

var tokenizer = new WordPieceTokenizer(VocabTxtLoader.Load("vocab.txt"));

using IEmbeddingGenerator<string, Embedding<float>> generator = new OnnxEmbeddingGenerator(
    new OnnxTextEmbedder("model.onnx", tokenizer), new BatchEncoder(tokenizer), "my-model");

var about = generator.GetService(typeof(EmbeddingGeneratorMetadata)) as EmbeddingGeneratorMetadata;
```

**Remarks** — **every fence on this page and its members is `docs-run: skip`**, and that is not an
oversight. A running example would need a model of tens of megabytes, and weights are never
committed to this repository — [`decisions/0003`](../../../decisions/0003-provenance-and-licensing.md)
is the rule, and the packaging sample declares the same exclusion for the same reason. The fences
are still **compiled** against the packed package, so a renamed member still fails CI.

**The work is synchronous.** The interface is asynchronous because most implementations of it call
a service over a network; this one runs a model in the calling process, so
[`OnnxEmbeddingGenerator.GenerateAsync`](onnxembeddinggenerator-generateasync.md) does its work on
the caller's thread and hands back an already-completed task. Wrapping it in `Task.Run` would move
the same CPU to a pool thread and tell the caller nothing true.

**Ownership is taken, not shared.** `IEmbeddingGenerator` is `IDisposable`, and a consumer holding
this through the interface has no way to learn that disposing it would leave a native session open.
So [`OnnxEmbeddingGenerator.Dispose`](onnxembeddinggenerator-dispose.md) disposes the embedder. A
caller who wants to keep the embedder alive builds a second one.

**Applies to** — net10.0, netstandard2.0.

**See also** — the [ONNX inference reference](../../onnx/inference.md), the
[semantic search guide](../../../guides/embeddings.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`OnnxEmbeddingGenerator.Dispose`](onnxembeddinggenerator-dispose.md) | Disposes the embedder this generator was given. |
| [`OnnxEmbeddingGenerator.GenerateAsync`](onnxembeddinggenerator-generateasync.md) | Embeds each text into one normalized vector. |
| [`OnnxEmbeddingGenerator.GetService`](onnxembeddinggenerator-getservice.md) | Answers for the services this generator can hand out. |
