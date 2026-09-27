# TokenizerJsonLoader.LoadWordPiece

Reads the WordPiece model a `tokenizer.json` declares.

<!-- docs-declaration -->

```csharp
public static WordPieceVocabulary LoadWordPiece(Stream source, ArtifactLoadOptions options = null)
public static WordPieceVocabulary LoadWordPiece(string path, ArtifactLoadOptions options = null)
```

**Parameters** — `source` is a readable stream, never disposed here; `path` is the file to read. `options` bounds what will be accepted and defaults to
[`ArtifactLoadOptions`](artifactloadoptions.md)'s own defaults.

**Returns** — `WordPieceVocabulary`, with the continuation prefix and the unknown piece read from the file.

**Exceptions** — `ArgumentNullException` for a null source or path. `InvalidDataException`
when the file declares a different model, declares a pipeline this package does not
reproduce, or exceeds a bound in `options` — the message names what was refused and why.

**Example** — a stock BERT `tokenizer.json`, all-MiniLM-L6-v2's for one.

<!-- docs-run: skip - the file is a model artifact, and model artifacts are never committed (CONTRIBUTING.md) -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

WordPieceVocabulary vocab = TokenizerJsonLoader.LoadWordPiece("tokenizer.json");
var template = new SpecialTokenTemplate(vocab.PrefixTokens, vocab.SuffixTokens, "[PAD]");
```

**Remarks** — **A stock HuggingFace BERT `tokenizer.json` loads as BERT's BasicTokenizer.** Its
default `BertNormalizer` — text cleaned, CJK padded, accents stripped exactly when lowercasing —
followed by a `BertPreTokenizer` sets `BasicTokenization`, the pipeline
[`VocabTxtLoader.Load`](vocabtxtloader-load.md) gives a `vocab.txt`. A `BertPreTokenizer` after any
other normalizer is refused, and so is a `BertNormalizer` doing more than lowercasing ahead of a
`Whitespace` pre-tokenizer: those steps are reproduced as one unit or not at all.

The `post_processor` — `TemplateProcessing`'s `single` template, or `BertProcessing` and
`RobertaProcessing` as `cls $A sep` — lands in `PrefixTokens` and `SuffixTokens`. The file's
`truncation` and `padding` are accepted and not read: they are call settings, and
[`EncodingOptions`](../tokenization/encodingoptions.md) carries the length and the template instead,
with a pad token the vocabulary does not.

Unlike [`VocabTxtLoader.Load`](vocabtxtloader-load.md), there is no `lowercase` parameter: the
file says.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`VocabTxtLoader.Load`](vocabtxtloader-load.md),
[`TokenizerJsonLoader.LoadWordPieceAsync`](tokenizerjsonloader-loadwordpieceasync.md),
[`TokenizerJsonLoader`](tokenizerjsonloader.md).
