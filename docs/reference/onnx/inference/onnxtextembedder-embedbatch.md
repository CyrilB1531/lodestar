# OnnxTextEmbedder.EmbedBatch

A vector per text: one session run per sub-batch of `BatchSize`, and per chunk of a static export's
fixed batch axis.

<!-- docs-declaration -->

```csharp
public float[][] EmbedBatch(IEnumerable<string> texts, EncodingOptions options = null, CancellationToken cancellationToken = default)
public float[][] EmbedBatch(IEnumerable<string> texts, BatchEncoder encoder, CancellationToken cancellationToken = default)
public float[][] EmbedBatch(EncodedBatch batch, CancellationToken cancellationToken = default)
```

**Parameters** — `texts` are the strings to embed. `options` tunes the encoding — the special-token
template, the truncation strategy, the maximum length, the batch size and whether to sort by
length; padding is not an option: each sub-batch is padded to its own longest sequence, or to a static
export's fixed axis. `encoder` is a `BatchEncoder`
you have already configured, for when the model's tokenizer is not the default. `batch` is an
`EncodedBatch` you encoded yourself. `cancellationToken`
abandons the run.

**Returns** — `float[][]`, one vector of `Dimension` per input text, in input order.

**Exceptions** — `ArgumentNullException` when `texts`, `encoder` or `batch` is null.
`ArgumentException` when the encoder refuses a text — as it refuses one over `MaxLength` under
`TruncationStrategy.None` — naming `texts`, as the first text past a configured length longer than the
template is under a configured `only_second` truncation, which cuts no single text, or a configured `stride` equal
to the room that length leaves beside the template, on which tokenizers fails to cut it; at or below the template
both cut every text to the template or none, as sentence-transformers does
([#1780](https://github.com/CyrilB1531/lodestar/issues/1780)); or, from the overload taking `options`, when
[`BatchEncoder`](../../embeddings/tokenization/batchencoder.md) refuses them; and when a sequence is
past [`MaxSequenceLength`](onnxtextembedder.md) — a static export's fixed sequence axis or the
position-embedding table — naming `options`, `encoder` or `batch`
([#1423](https://github.com/CyrilB1531/lodestar/issues/1423)), or when the batch times the
sequence — one fixed by the model, the other brought by the call — makes one chunk more cells
than the runtime's largest array holds, `0x7FEFFFFF` before .NET 6, naming the same three
([#1555](https://github.com/CyrilB1531/lodestar/issues/1555),
[#1581](https://github.com/CyrilB1531/lodestar/issues/1581)); below that bound the memory the
chunk takes is what [`Embed`](onnxtextembedder-embed.md) describes.
`InvalidOperationException` from the overload taking only texts, when the
embedder was built without a tokenizer: the other two overloads are the way to supply one. Both text
overloads throw it when the sentence-transformers configuration or the `modules.json` in the Transformer module's folder — the model's, or the parent of the `onnx/` it sits in — cannot be read — a directory, unreadable, longer than 256 MiB, opening with a byte-order mark or not UTF-8 —, is not JSON, holds an integer of more than 4300 digits, as Python's `json` refuses, or is not what sentence-transformers loads: a configuration that is not a JSON object, or whose `processing_kwargs` is not one, a `model_args`, `model_kwargs`, `tokenizer_args`, `processor_kwargs`, `config_args` or `config_kwargs` in which `trust_remote_code` cannot be looked up and removed, or which is kept and is not an object, a `modality_config` that is not an object, stands beside no `module_output_name` or holds a modality entry that is not an object naming `method` and `method_output_name`, a `transformer_task` sentence-transformers does not know, a `processing_kwargs` bucket — `text`, `common`, `image`, `audio` or `video` — `dict.update` refuses, a `null`, `0` or `false` `common` among them, a key that is not a string in `text` or `common`, a `truncation` transformers does not know, a `stride` tokenizers cannot take or that passes the room the length leaves beside the template ([#1780](https://github.com/CyrilB1531/lodestar/issues/1780)), a `modules.json` that is not a list of modules with a string path and type or lists a path outside the model's folder, or a `max_seq_length`, `model_max_length` or `max_length` that is neither `null` nor an integer of at least 1, `true` and a zero among them. A key the module's constructor does not take, which sentence-transformers refuses, is read past
([#1257](https://github.com/CyrilB1531/lodestar/issues/1257)). All
three throw it when the model output is not shaped for the batch it was fed, or declares its first two
axes as the input's two swapped ([#1424](https://github.com/CyrilB1531/lodestar/issues/1424)), and `NotSupportedException` when its elements
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
]);   // a null MaxLength: max_seq_length beside the model, else 256
```

**Remarks** — three overloads for one operation, and the choice is about **who owns the
tokenizer**. The first owns it for you and is right when the model ships a standard vocabulary.
The second takes an encoder you built, for a model whose tokenizer differs. The third takes an
already-encoded batch, for when encoding happened elsewhere — on another thread, or once for
several models.

Batching is not merely convenient. A session run has a fixed cost, so embedding a hundred texts
one at a time costs a hundred of those; the vectors are identical either way, and the time is not.

**A static export is fed its own shape.** Where the model fixes its batch or sequence axis, as a
quantized or mobile export does, each row is padded to the fixed sequence length with masked
positions the mean leaves out, and the rows run in chunks of the fixed batch size, the last filled
with masked rows whose vectors are dropped; the vectors are the ones a dynamic export of the same
model gives ([#1258](https://github.com/CyrilB1531/lodestar/issues/1258)). A sequence past the fixed
axis cannot be fed, and is refused rather than truncated: a `MaxLength` above it names `options`, an
encoder whose own `MaxLength` produces one names `encoder`, a batch holding one names `batch`. The same holds for
[`Embed`](onnxtextembedder-embed.md), and for a sequence past the position-embedding table a
dynamic export's `MaxSequenceLength` is read from.

**Where a null `MaxLength` truncates.** At the length sentence-transformers configures, read when a text
overload first runs, from the configuration sentence-transformers reads for the model's Transformer module — the folder holding the
model, or the `onnx/` it sits in, when a `modules.json` lists it there, at `""` in its own or under its name in its
parent's — the first of the seven legacy names Python finds true, a `model_max_length` in its `tokenizer_args` or
`processor_kwargs` over its `max_seq_length`, and a `max_length` or a `truncation` that cuts nothing in its
`processing_kwargs`, its `text` and `common` buckets merged key by key, `common`'s winning, over both — and at 256 when none states one — the length the [constructor](onnxtextembedder.md) describes —
never past [`MaxSequenceLength`](onnxtextembedder.md): the model's declared sequence axis when
fixed, else the positions its position-embedding table can index, 512 for `all-MiniLM-L6-v2`.
That model's export states 256, so the example cuts where sentence-transformers cuts. A
configured length past the table is capped at it, where sentence-transformers feeds it and fails
in the graph. A model with neither a fixed axis nor a readable table takes the configured length
or 256 as it is. Both text overloads cut alike: the one taking a `BatchEncoder` applies the same
default to an encoder with a null `MaxLength`, through
[`WithDefaultMaxLength`](../../embeddings/tokenization/batchencoder-withdefaultmaxlength.md), and
leaves the encoder you passed unchanged; a `MaxLength` the encoder sets is kept
([#1257](https://github.com/CyrilB1531/lodestar/issues/1257)).

**Lowercasing.** When that file's `do_lower_case` is true — by Python's truth, so `1` or a non-empty
string count and `0`, `""` or `null` do not — both text overloads lowercase each text before encoding it,
as sentence-transformers does by prepending the tokenizers' `Lowercase` normalizer: each code point's own
lowercase, `İ` to `i̇`, no final sigma. Divergence: a special token written into the text in capitals,
such as `[SEP]`, is lowercased with it, where the tokenizer would match it before its normalizer
([#1257](https://github.com/CyrilB1531/lodestar/issues/1257)).

Order is preserved, so the *n*th vector belongs to the *n*th text however the batch was padded
internally.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OnnxTextEmbedder.Embed`](onnxtextembedder-embed.md),
`BatchEncoder`,
`EncodedBatch`.
