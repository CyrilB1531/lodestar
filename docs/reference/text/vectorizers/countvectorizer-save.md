# CountVectorizer.Save

Write a fitted vectorizer out, so the vocabulary survives the process.

<!-- docs-declaration -->

```csharp
public void Save(Stream destination)
public void Save(string path)
```

**Parameters** — `destination` is a writable stream, left open for the caller to dispose; `path`
is a file to create or overwrite.

**Exceptions** — `InvalidOperationException` when nothing has been fitted yet, or when a vocabulary
term, the token pattern or a stop word is beyond what the JSON writer can write — about 166 million
characters, fewer where it escapes — refused before a byte is written: once the writer has accepted
the stream, or once the file is open, so a path opening refuses is refused first ([#1618](https://github.com/CyrilB1531/lodestar/issues/1618)). `ArgumentNullException` for a null stream or path. `IOException` from the stream or file
system.

**Example** — round-tripping through memory.

```csharp
using Lodestar.Text.Vectorization;

var cv = new CountVectorizer();
cv.Fit(["the cat eats", "the dog eats"]);

using var buffer = new MemoryStream();
cv.Save(buffer);
buffer.Position = 0;

CountVectorizer restored = CountVectorizer.Load(buffer);
int columns = restored.Transform(["the cat"]).ColumnCount;  // => 4
```

**Remarks** — what is written is the **fit**: the vocabulary and the options that produced it. A
vectorizer restored from it counts a corpus exactly as the original would, which is the point —
the alternative is refitting on training data that may no longer be around.

Saving before fitting throws rather than writing an empty vocabulary, because a file that loads
into a vectorizer which drops every term is worse than no file.

The stream overload leaves `destination` open. That is deliberate: it lets a vectorizer be one
part of a larger archive.

The write is not bounded the way a load is. A vocabulary past a default of
[`ArtifactLoadOptions`](../persistence/artifactloadoptions.md) — a token over 1024 characters, more
than a million features — saves, and loads only with that bound raised.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CountVectorizer.Load`](countvectorizer-load.md),
[`CountVectorizer.SaveAsync`](countvectorizer-saveasync.md).
