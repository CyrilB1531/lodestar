# VocabTxtLoader

Reads a BERT-style `vocab.txt`: one token per line, the id being the line number.

<!-- docs-declaration -->

```csharp
public static class VocabTxtLoader
```

**Example** — the route for a stock BERT checkpoint.

<!-- docs-run: skip - the file is a model artifact, and model artifacts are never committed (CONTRIBUTING.md) -->

```csharp
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

WordPieceVocabulary vocab = VocabTxtLoader.Load("bert-base-uncased/vocab.txt", lowercase: true);
var tokenizer = new WordPieceTokenizer(vocab);
```

**Remarks** — this is **the** route for stock BERT, not a fallback. The vocabulary it returns has
[`WordPieceVocabulary.BasicTokenization`](../tokenization/wordpiecevocabulary.md) set, so the tokenizer
runs BERT's BasicTokenizer ahead of WordPiece as `transformers.BertTokenizer` does: control characters
dropped, CJK ideographs padded, each punctuation character a token of its own, and accents stripped
when `lowercase` is set. `café` in an uncased checkpoint is `cafe`, not `[UNK]`. A HuggingFace BERT
`tokenizer.json` declares the same steps as a `BertPreTokenizer` and a full `BertNormalizer`, which
[`TokenizerJsonLoader.LoadWordPiece`](tokenizerjsonloader-loadwordpiece.md) still refuses.
[Decision 0005](../../../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md) has why the
route changed.

The format records nothing but the tokens, so everything else is a parameter —
[`Load`](vocabtxtloader-load.md) has the three that matter and why `lowercase` is the dangerous
one.

Reference behaviour is `transformers.BertTokenizer`'s vocabulary loading, including two quirks of
the Python loop it reproduces deliberately; `docs/equivalence.md`'s loader row names them.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TokenizerJsonLoader`](tokenizerjsonloader.md),
[`ArtifactLoadOptions`](artifactloadoptions.md), [the persistence index](../persistence.md).

## Members

| Member | What it does |
| --- | --- |
| [`VocabTxtLoader.Load`](vocabtxtloader-load.md) | Reads a `vocab.txt` into a WordPiece vocabulary. |
| [`VocabTxtLoader.LoadAsync`](vocabtxtloader-loadasync.md) | The same, asynchronously. |
