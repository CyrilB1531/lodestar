# PrecompiledNormalizer.FromCharsMap

Build one from a model's compiled trie.

<!-- docs-declaration -->

```csharp
public static PrecompiledNormalizer FromCharsMap(byte[] charsMap)
```

**Parameters** — `charsMap` is the `precompiled_charsmap` blob from a `spiece.model` or a
`tokenizer.json` normalizer.

**Returns** — `PrecompiledNormalizer` ready to [`Normalize`](precompilednormalizer-normalize.md).

**Exceptions** — `ArgumentNullException` when `charsMap` is null. `InvalidDataException` when the
blob is not a usable charsmap: too short to carry its own trie size, or declaring a trie that is
empty, not a whole number of 4-byte units, or longer than the bytes after the header.

**Example** — the refusals, which are what a caller can actually reach without a model file.

```csharp
using Lodestar.Embeddings.Tokenization;

string tooShort = "";
try { PrecompiledNormalizer.FromCharsMap([]); }
catch (InvalidDataException e) { tooShort = e.Message; }

string emptyTrie = "";
try { PrecompiledNormalizer.FromCharsMap([0, 0, 0, 0]); }
catch (InvalidDataException e) { emptyTrie = e.Message; }

bool bothRefused = tooShort.Length > 0 && emptyTrie.Length > 0;  // => True
```

**Remarks** — the blob is copied, so writing to the caller's array afterwards changes neither what
the normalizer does nor what it equals ([#1342](https://github.com/CyrilB1531/lodestar/issues/1342)).

A charsmap cannot be synthesised, which is why the example above shows what happens
when you try. Four zero bytes are a well-formed *header* declaring a trie of nothing, and that is
refused separately from a blob too short to have a header at all — two different ways a file can
be wrong, and the messages say which.

Refusing rather than accepting an empty trie matters: a normalizer that silently does nothing
would leave text unfolded and the model would see input it was not trained on, with no error
anywhere.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PrecompiledNormalizer`](precompilednormalizer.md),
[`PrecompiledNormalizer.Normalize`](precompilednormalizer-normalize.md).
