# OnnxEmbeddingGenerator.Dispose

Disposes the embedder this generator was given.

<!-- docs-declaration -->

```csharp
public void Dispose()
```

**Example** — `using` is the whole of it.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Extensions.AI;
using Lodestar.Onnx;
using Microsoft.Extensions.AI;

var tokenizer = new WordPieceTokenizer(VocabTxtLoader.Load("vocab.txt"));

using (IEmbeddingGenerator<string, Embedding<float>> generator = new OnnxEmbeddingGenerator(
    new OnnxTextEmbedder("model.onnx", tokenizer), new BatchEncoder(tokenizer)))
{
    // The model session is closed at the end of the scope.
}
```

**Remarks** — **this generator owns the embedder it was handed**, so disposing it closes the native
model session. That is a stronger promise than a wrapper usually makes, and it is the right one
here: `IEmbeddingGenerator` is `IDisposable`, and a consumer holding this through the interface has
no way to learn that disposing it would leave a session open. A caller who wants to keep an embedder
alive past the generator builds a second embedder rather than sharing one.

The session is **native memory**, not managed, so it is not reclaimed by a garbage collection and a
forgotten generator holds the model until the process ends.

Disposing twice is safe. Calling
[`OnnxEmbeddingGenerator.GenerateAsync`](onnxembeddinggenerator-generateasync.md) afterwards is not,
and throws.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxEmbeddingGenerator`](onnxembeddinggenerator.md).
