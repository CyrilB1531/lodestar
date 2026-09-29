# TokenizationResult

Tokens and ids, from encoding one string.

<!-- docs-declaration -->

```csharp
public sealed record TokenizationResult
```

**Properties** — `Tokens` are the token strings and `Ids` their ids. They are the **same length**
and in the same order, so `Tokens[i]` is what `Ids[i]` stands for.

**Example** — the two halves of one encoding.

```csharp
using Lodestar.Embeddings.Tokenization;

var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
{
    ["[UNK]"] = 0, ["token"] = 1, ["##ize"] = 2, ["text"] = 3,
};
var tokenizer = new WordPieceTokenizer(
    vocab, unkToken: "[UNK]", continuationPrefix: "##", maxCharsPerWord: 100, lowercase: true);

TokenizationResult encoded = tokenizer.Encode("tokenize text");

int tokens = encoded.Tokens.Count;  // => 3
int firstId = encoded.Ids[0];  // => 1
```

**Remarks** — the tokens are carried alongside the ids because they are what makes a tokenizer
debuggable. When a model behaves oddly, reading the tokens is how you find that the text was cut
where you did not expect, or that half of it became unknown tokens; the ids alone say nothing a
human can check.

Only the ids go to the model. The tokens cost the encoding a list of strings, and that is the
deliberate trade — the alternative is [`WordPieceTokenizer.EncodeToIds`](wordpiecetokenizer-encodetoids.md),
which skips them.

Being a `record`, two results with the same tokens and ids are equal.

`Tokens` and `Ids` are the tokenizer's own lists, taken and exposed as they are, not copied: a
caller casting one back to its list and writing to it changes this result and what it equals, and
every holder of the same list, a `with` copy included ([#1305](https://github.com/CyrilB1531/lodestar/issues/1305)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ISubwordTokenizer.Encode`](isubwordtokenizer-encode.md),
[`EncodedBatch`](encodedbatch.md).

## Members

| Member | What it does |
| --- | --- |
| [`TokenizationResult.Equals`](tokenizationresult-equals.md) | Value equality over the tokens and ids. |
| [`TokenizationResult.GetHashCode`](tokenizationresult-gethashcode.md) | A hash consistent with it. |
