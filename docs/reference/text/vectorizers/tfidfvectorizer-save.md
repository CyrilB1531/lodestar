# TfidfVectorizer.Save

Write a fitted vectorizer out, vocabulary and document frequencies together.

<!-- docs-declaration -->

```csharp
public void Save(Stream destination)
public void Save(string path)
```

**Parameters** — `destination` is a writable stream, left open for the caller to dispose; `path`
is a file to create or overwrite.

**Exceptions** — `InvalidOperationException` when nothing has been fitted yet, or when the idf
weights make a base64 block within two mebibytes of the most the JSON writer holds in one buffer, from
201,129,984 weights, or when a vocabulary term, the token pattern or a stop word is beyond what the
JSON writer can write. `InvalidDataException` when an idf weight is not finite. An unfitted vectorizer
is refused before the path is opened; the rest before a byte is written — once the writer has accepted
the stream, or once the file is open — so a read-only stream or a path that opening refuses is refused
first, as it always was, and a file is left empty rather than half written
([#1617](https://github.com/CyrilB1531/lodestar/issues/1617),
[#1618](https://github.com/CyrilB1531/lodestar/issues/1618)). `ArgumentException` when the stream cannot be written to, refused once the vectorizer is known fitted, before its strings are checked ([#1641](https://github.com/CyrilB1531/lodestar/issues/1641)). `ArgumentNullException` for a null
stream or path. `IOException` from the stream or file system.

**Example** — saving and restoring, with the weights intact.

```csharp
using Lodestar.Text.Vectorization;

var tv = new TfidfVectorizer();
tv.Fit(["the cat eats", "the dog eats"]);

using var buffer = new MemoryStream();
tv.Save(buffer);
buffer.Position = 0;

TfidfVectorizer restored = TfidfVectorizer.Load(buffer);
string first = restored.GetFeatureNames()[0];  // => cat
```

**Remarks** — what is written is both halves of the fit: the vocabulary **and** the document
frequencies. That second half is the reason saving matters more here than for
[`CountVectorizer`](countvectorizer.md) — frequencies are a property of the training corpus, and
a corpus that is gone cannot be measured again.

The stream overload leaves `destination` open, so a vectorizer can be one entry in a larger
archive.

The write is not bounded the way a load is. A vocabulary past a default of
[`ArtifactLoadOptions`](../persistence/artifactloadoptions.md) — a token over 1024 characters, more
than a million features — saves, and loads only with that bound raised.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TfidfVectorizer.Load`](tfidfvectorizer-load.md),
[`TfidfVectorizer.SaveAsync`](tfidfvectorizer-saveasync.md).
