# BatchEncoder.WithDefaultMaxLength

An encoder that caps at a given length where this one sets no `MaxLength` of its own.

<!-- docs-declaration -->

```csharp
public BatchEncoder WithDefaultMaxLength(int maxLength)
```

**Parameters** — `maxLength` is the cap to take, special tokens included; typically a model's own
limit.

**Returns** — a `BatchEncoder` with that cap and every other option unchanged. It is this same
instance when [`EncodingOptions.MaxLength`](encodingoptions.md) is already set, or when the template
alone is longer than `maxLength`. The encoder it was called on is never changed.

**Exceptions** — `ArgumentOutOfRangeException` when `maxLength` is below `1`.

**Example** — the cap taken only where none was set.

```csharp
using Lodestar.Embeddings.Tokenization;

var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
{
    ["[UNK]"] = 0, ["text"] = 3, ["[CLS]"] = 4, ["[SEP]"] = 5, ["[PAD]"] = 6,
};
var tokenizer = new WordPieceTokenizer(
    vocab, unkToken: "[UNK]", continuationPrefix: "##", maxCharsPerWord: 100, lowercase: true);

BatchEncoder open = new(tokenizer);
BatchEncoder capped = open.WithDefaultMaxLength(4);

int length = capped.Encode("text text text text").Length;   // => 4
int uncut = open.Encode("text text text text").Length;      // => 6
```

**Remarks** — this is how [`OnnxTextEmbedder.EmbedBatch`](../../onnx/inference/onnxtextembedder-embedbatch.md)
gives an encoder with a null `MaxLength` the length a sentence-transformers export configures,
else 256, capped at the model's own limit
([#1257](https://github.com/CyrilB1531/lodestar/issues/1257)).

A template longer than `maxLength` leaves nothing to cut, so this method does not refuse it; a
row past the model's limit is refused when it is run, and a call with no texts has nothing to
refuse.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BatchEncoder`](batchencoder.md), [`EncodingOptions`](encodingoptions.md).
