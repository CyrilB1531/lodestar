# OnnxEmbeddingGenerator.GenerateAsync

Embeds each text into one normalized vector.

<!-- docs-declaration -->

```csharp
public Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, EmbeddingGenerationOptions options = null, CancellationToken cancellationToken = default)
```

**Parameters** — `values` are the texts to embed. `options` carries the request a
`Microsoft.Extensions.AI` caller makes; only `Dimensions` is read, and see the remarks for what
that means. `cancellationToken` abandons the run.

**Returns** — a `GeneratedEmbeddings<Embedding<float>>` holding one embedding per input, in input
order. Each is what `OnnxTextEmbedder.EmbedBatch` returned for that text — mean-pooled and L2
normalized — with nothing added on top.

**Exceptions** — `ArgumentNullException` when `values` is null. `ArgumentException` when `options`
asks for a dimension the loaded model does not produce. `OperationCanceledException` when
`cancellationToken` is already cancelled, or is cancelled between sub-batches — it is the batch path
underneath that observes it, so the point at which it fires is that path's, not this one's.
`ObjectDisposedException` after
[`OnnxEmbeddingGenerator.Dispose`](onnxembeddinggenerator-dispose.md).

**Example** — driving it from a synchronous entry point.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed -->

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

int width = EmbedAsync().GetAwaiter().GetResult();
```

The `GetAwaiter().GetResult()` is only what lets a synchronous example drive an async one; in your
own code, `await` it.

**Remarks** — **the task is already completed when it comes back.** The model runs in this process,
so the work happens on the calling thread and there is nothing to wait for. That is the honest
shape: an implementation that posted the same CPU to the thread pool would return a task the caller
could await without the machine doing less.

**`Dimensions` is checked, not honoured.** An ONNX model's output width is fixed at export, so a
different width cannot be produced; asking for one the model does not produce is refused rather
than silently ignored. The check only fires when the model declares a fixed output axis — most
exports declare a symbolic one, and there the request is left alone, because refusing it would mean
guessing.

**`ModelId` is not read.** It selects among the models a service hosts, and this generator holds
exactly one — the file its embedder was opened on.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxEmbeddingGenerator`](onnxembeddinggenerator.md),
[`OnnxEmbeddingGenerator.GetService`](onnxembeddinggenerator-getservice.md).
