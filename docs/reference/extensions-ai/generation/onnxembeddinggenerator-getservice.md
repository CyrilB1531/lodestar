# OnnxEmbeddingGenerator.GetService

Answers for the services this generator can hand out.

<!-- docs-declaration -->

```csharp
public object GetService(Type serviceType, object serviceKey = null)
```

**Parameters** — `serviceType` is the service being asked for. `serviceKey` names a keyed
registration; nothing here is registered under one, so a keyed lookup always answers `null`.

**Returns** — the service, or `null`. Three types are answered: `EmbeddingGeneratorMetadata`,
`OnnxTextEmbedder`, and any type this generator is an instance of — which covers both
`IEmbeddingGenerator<string, Embedding<float>>` and the concrete class.

**Exceptions** — `ArgumentNullException` when `serviceType` is null.

**Example** — reaching the embedder through the abstraction.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Extensions.AI;
using Lodestar.Onnx;
using Microsoft.Extensions.AI;

var tokenizer = new WordPieceTokenizer(VocabTxtLoader.Load("vocab.txt"));

using IEmbeddingGenerator<string, Embedding<float>> generator = new OnnxEmbeddingGenerator(
    new OnnxTextEmbedder("model.onnx", tokenizer), new BatchEncoder(tokenizer));

var embedder = generator.GetService(typeof(OnnxTextEmbedder)) as OnnxTextEmbedder;
float[] one = embedder.Embed([1, 4, 2], [1, 1, 1]);
```

**Remarks** — `OnnxTextEmbedder` is offered on purpose rather than as a leak. The single-sequence
`Embed` takes token ids a caller already holds, and `Microsoft.Extensions.AI` has no shape for that
at all; without this, a consumer holding the abstraction could not reach it. Everything reached this
way is the same object this generator will dispose, so it must not outlive the generator.

`EmbeddingGeneratorMetadata` reports `Lodestar.Onnx` as the provider, the model identifier the
constructor was given — `null` if it was given none, since an ONNX file carries no name to read —
and the model's output width when the model declares a fixed one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxEmbeddingGenerator`](onnxembeddinggenerator.md),
[`OnnxEmbeddingGenerator.GenerateAsync`](onnxembeddinggenerator-generateasync.md).
