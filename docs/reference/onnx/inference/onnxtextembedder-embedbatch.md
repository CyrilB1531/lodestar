# OnnxTextEmbedder.EmbedBatch

A vector per text, in one session run.

<!-- docs-declaration -->

```csharp
public float[][] EmbedBatch(IEnumerable<string> texts, EncodingOptions options = null, CancellationToken cancellationToken = default)
public float[][] EmbedBatch(IEnumerable<string> texts, BatchEncoder encoder, CancellationToken cancellationToken = default)
public float[][] EmbedBatch(EncodedBatch batch, CancellationToken cancellationToken = default)
```

**Parameters** — `texts` are the strings to embed. `options` tunes the encoding — padding,
truncation, the maximum length. `encoder` is a `BatchEncoder`
you have already configured, for when the model's tokenizer is not the default. `batch` is an
`EncodedBatch` you encoded yourself. `cancellationToken`
abandons the run.

**Returns** — `float[][]`, one vector of `Dimension` per input text, in input order.

**Exceptions** — `InvalidOperationException` from the overload taking only texts, when the
embedder was built without a tokenizer: the other two overloads are the way to supply one. All
three throw it when the model output is not shaped for the batch it was fed, and `NotSupportedException` when its elements
are not float, float16 or bfloat16, as [`Embed`](onnxtextembedder-embed.md) does.
`OperationCanceledException` when `cancellationToken` is already cancelled, or is cancelled
between sub-batches. `ObjectDisposedException` after
[`Dispose`](onnxtextembedder-dispose.md), for the reason
[`Embed`](onnxtextembedder-embed.md) gives.

**Example** — the shortest path from strings to vectors.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Onnx;

var tokenizer = new WordPieceTokenizer(VocabTxtLoader.Load("vocab.txt"));
using var embedder = new OnnxTextEmbedder("model.onnx", tokenizer);

float[][] vectors = embedder.EmbedBatch(
[
    "the cat sat on the mat",
    "a dog lay on the rug",
],
new EncodingOptions { MaxLength = 256 });   // max_seq_length, as sentence-transformers truncates
```

**Remarks** — three overloads for one operation, and the choice is about **who owns the
tokenizer**. The first owns it for you and is right when the model ships a standard vocabulary.
The second takes an encoder you built, for a model whose tokenizer differs. The third takes an
already-encoded batch, for when encoding happened elsewhere — on another thread, or once for
several models.

Batching is not merely convenient. A session run has a fixed cost, so embedding a hundred texts
one at a time costs a hundred of those; the vectors are identical either way, and the time is not.

**Where a null `MaxLength` truncates.** The overload taking `EncodingOptions` takes it from
[`MaxSequenceLength`](onnxtextembedder.md): the model's declared sequence axis when fixed, else the
positions its position-embedding table can index — 512 for `all-MiniLM-L6-v2`. That is the
model's hard limit, not sentence-transformers' own: it truncates to `max_seq_length` from
`sentence_bert_config.json` (256 for that model), which no ONNX graph carries, so a text between
the two lengths embeds differently. Passing `max_seq_length` as `MaxLength`, as the example does,
matches sentence-transformers exactly. A model with neither a fixed axis nor a readable table is
not truncated. The overload taking a `BatchEncoder` truncates exactly as that encoder does.

Order is preserved, so the *n*th vector belongs to the *n*th text however the batch was padded
internally.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxTextEmbedder.Embed`](onnxtextembedder-embed.md),
`BatchEncoder`,
`EncodedBatch`.
