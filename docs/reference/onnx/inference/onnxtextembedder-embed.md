# OnnxTextEmbedder.Embed

One vector, from token ids you already have.

<!-- docs-declaration -->

```csharp
public float[] Embed(ReadOnlySpan<long> inputIds, ReadOnlySpan<long> attentionMask)
```

**Parameters** — `inputIds` are the token ids for one text, as the model's own tokenizer produces
them. `attentionMask` is the same length, `1` for a real token and `0` for padding.

**Returns** — `float[]` of length `Dimension`, the pooled vector for that text.

**Exceptions** — `ArgumentException` when the two spans differ in length, or are longer than
[`MaxSequenceLength`](onnxtextembedder.md) — a fixed sequence axis or the position-embedding table,
refused here rather than failing inside the graph
([#1423](https://github.com/CyrilB1531/lodestar/issues/1423)); and when the model's fixed batch
times the input's length makes one chunk more than `Array.MaxLength` cells, refused before
allocating it ([#1555](https://github.com/CyrilB1531/lodestar/issues/1555)) — a model fixing both
axes that far is refused when it is opened instead
([#1589](https://github.com/CyrilB1531/lodestar/issues/1589)). Below that bound the chunk is
allocated: about 8 bytes a cell for the ids and 8 for the mask — rented from the pool, which rounds
a length up to a power of two below its largest bucket, and alongside the caller's unpadded copies
when a static export is chunked — and 8 for the token types of a model that declares them, a
buffer each thread keeps for its lifetime at the largest size it has reached. The output ONNX
Runtime returns adds 4 × the dimension a cell for a float output, and for a float16 or bfloat16
output 2 × the dimension plus the `float` array it is widened into, rounded up the same way: at
least 6 × and up to about 10 × the dimension a cell. A pooled `[batch, dim]` output costs that per
row rather than per cell ([#1590](https://github.com/CyrilB1531/lodestar/issues/1590)). An
export declaring axes near the bound can exhaust memory before any refusal, and on .NET Framework,
where one object stops at 2 GB unless `gcAllowVeryLargeObjects` is set, from about 268 million
cells, a `long[]` stopping at `0x7FEFFFFF` elements even when it is set
([#1582](https://github.com/CyrilB1531/lodestar/issues/1582)).
`InvalidOperationException` when the model output is not `[batch, sequence, dim]` (or `[batch, dim]`,
pooled by the graph) for the batch it was fed, or declares its axes as the input's two swapped, which
sizes alone miss when the batch is as long as the sequence
([#1424](https://github.com/CyrilB1531/lodestar/issues/1424)); the message names the output and both
shapes.
`NotSupportedException` when its elements are not float, float16 or bfloat16 — the two
half-precision types are widened to float before pooling.
`ObjectDisposedException` after [`Dispose`](onnxtextembedder-dispose.md) — the type
checks a flag of its own, because reaching a disposed ONNX Runtime session surfaces as a
null dereference from inside it, naming neither the object nor the mistake.

**Example** — ids from a tokenizer, one text at a time.

<!-- docs-run: skip - constructing it loads an ONNX model, and model weights are never committed -->

```csharp
using Lodestar.Onnx;

using var embedder = new OnnxTextEmbedder("model.onnx");

long[] ids = [101, 2054, 2003, 102];
long[] mask = [1, 1, 1, 1];

float[] vector = embedder.Embed(ids, mask);
```

**Remarks** — this is the low-level entry: it takes ids rather than text, so the tokenizer is
yours to choose and yours to match to the model. Mismatching them produces vectors that are
confidently wrong rather than an error, which is the failure worth guarding against — use the
vocabulary that shipped with the model.

For text rather than ids, [`EmbedBatch`](onnxtextembedder-embedbatch.md) takes strings and does the
encoding. It is also the faster path for more than one text: a session run has a fixed cost that a
batch amortises.

The `attentionMask` matters even for a single unpadded text, where it is all ones — the model
reads it, and pooling uses it to ignore padding.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxTextEmbedder.EmbedBatch`](onnxtextembedder-embedbatch.md),
`BatchEncoder`.
