# BpeTokenizer.Encode

Tokens and ids for one string.

<!-- docs-declaration -->

```csharp
public TokenizationResult Encode(string text)
```

**Parameters** — `text` is the string to encode.

**Returns** — [`TokenizationResult`](tokenizationresult.md), the merged symbols and their ids.

**Example** — the merges applied in rank order.

```csharp
using Lodestar.Embeddings.Tokenization;

var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
{
    ["Ġ"] = 0, ["t"] = 1, ["o"] = 2, ["k"] = 3, ["e"] = 4, ["n"] = 5,
    ["to"] = 6, ["ken"] = 7, ["token"] = 8, ["Ġtoken"] = 9, ["ke"] = 10,
};
var merges = new List<MergePair> { new("t", "o"), new("k", "e"), new("ke", "n") };
var model = new BpeVocabulary(vocab, merges)
{
    ByteLevel = true,
    PreTokenizerPattern = BpePatterns.Gpt2,
    PreSplit = null,
};
var tokenizer = new BpeTokenizer(model);

TokenizationResult encoded = tokenizer.Encode("token");

string first = encoded.Tokens[0];  // => to
string second = encoded.Tokens[1];  // => ken
```

**Exceptions** — `ArgumentNullException` when `text` is null. `ArgumentException` when a byte-level vocabulary is missing one of the
256 base alphabet tokens — a broken model rather than ordinary uncovered input — or, once a
normalizer is declared, when an unpaired surrogate falls in a gap, since
`string.Normalize` refuses that before the byte-level re-encoding is reached; and when a
pre-tokenized piece needs more symbols than one array holds, some 700 million characters, which
used to overflow and fail from inside the runtime
([#1436](https://github.com/CyrilB1531/lodestar/issues/1436),
[#1437](https://github.com/CyrilB1531/lodestar/issues/1437)).
`EncoderFallbackException` when an unpaired UTF-16 surrogate is re-encoded to UTF-8: a
byte-level model re-encodes the whole text, and a classic model declaring `byte_fallback`
re-encodes each symbol its vocabulary does not cover. Both are lossless only over well-formed
UTF-16, so they throw rather than substituting. A classic model declaring neither re-encodes
nothing, and an unpaired surrogate through it returns normally.

**Remarks** — `token` is in the vocabulary as a single entry, and the result is still two tokens.
That is not a bug: BPE reaches a symbol only by **merging**, and no rule joins `to` with `ken`.
A vocabulary entry with no path of merges to it is unreachable, which is a real property of hand-built
models and a good reason to check a tokenization rather than assume it.

The pre-tokenizer runs first and merges never cross its boundaries, so
[`BpePatterns`](bpepatterns.md) decides what the merge loop even sees.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BpeTokenizer.Decode`](bpetokenizer-decode.md), [`MergePair`](mergepair.md).
