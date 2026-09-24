# Lodestar.Embeddings

Everything around an embedding model except running it. Sub-word tokenizers loaded from
the files a Hugging Face model ships (WordPiece, SentencePiece unigram, BPE and byte-level BPE,
from `tokenizer.json`, `vocab.txt` or a SentencePiece model); batch encoding with padding,
truncation and attention masks; mean pooling and L2 normalization; a SIMD k-nearest-neighbour
`EmbeddingIndex`; and `.npy` reading and writing. It has no native dependency: running an ONNX
encoder is `Lodestar.Onnx`'s job.

## Install

```bash
dotnet add package Lodestar.Embeddings
```

## Example

```csharp
using Lodestar.Embeddings.Tokenization;

var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
{
    ["[UNK]"] = 0, ["token"] = 1, ["##ize"] = 2, ["text"] = 3,
};
var tokenizer = new WordPieceTokenizer(
    vocab, unkToken: "[UNK]", continuationPrefix: "##", maxCharsPerWord: 100, lowercase: true);

TokenizationResult encoded = tokenizer.Encode("tokenize");
int pieces = encoded.Tokens.Count;   // 2: "token", "##ize"
```

## Parity

Token ids are replayed against Hugging Face `tokenizers` and `sentencepiece` on the
vocabularies of real models; pooling and search against numpy.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [embeddings](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/embeddings.md)
- Reference: [embeddings/persistence](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/embeddings/persistence.md)
- Reference: [embeddings/pooling](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/embeddings/pooling.md)
- Reference: [embeddings/search](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/embeddings/search.md)
- Reference: [embeddings/tokenization](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/embeddings/tokenization.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Embeddings/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Embeddings/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
