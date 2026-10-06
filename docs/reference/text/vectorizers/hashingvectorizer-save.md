# HashingVectorizer.Save

Write the options out, which is all there is to write.

<!-- docs-declaration -->

```csharp
public void Save(Stream destination)
public void Save(string path)
```

**Parameters** — `destination` is a writable stream, left open for the caller to dispose; `path`
is a file to create or overwrite.

**Exceptions** — `ArgumentException`, the JSON writer's own, as 0.7.0 raised it, when the token
pattern or a stop word is beyond what the writer can write. `InvalidDataException` when `MinDf` or
`MaxDf` is not finite, which the constructor does not refuse. Both are refused before a byte is
written: once the writer has accepted the stream, or once the file is open
([#1618](https://github.com/CyrilB1531/lodestar/issues/1618),
[#1622](https://github.com/CyrilB1531/lodestar/issues/1622),
[#1646](https://github.com/CyrilB1531/lodestar/issues/1646)). `ArgumentException` too when the
stream cannot be written to, refused before the options' strings are checked
([#1641](https://github.com/CyrilB1531/lodestar/issues/1641)). `IndexOutOfRangeException`, the
writer's own as well, for a string whose escaped form passes its buffer, as 0.7.0 raised it.
`ArgumentNullException` for a null stream or path. `IOException` from the stream or file system.

**Example** — a round trip that carries the settings and nothing else.

```csharp
using Lodestar.Text.Vectorization;

var hv = new HashingVectorizer(new HashingVectorizerOptions { NumFeatures = 16 });

using var buffer = new MemoryStream();
hv.Save(buffer);
buffer.Position = 0;

HashingVectorizer restored = HashingVectorizer.Load(buffer);
int columns = restored.NumFeatures;  // => 16
```

**Remarks** — there is no fit to save, so this writes the
[options](hashingvectorizeroptions.md) alone, and unlike its counterparts it **never throws for
being unfitted** — there is no such state.

Whether it is worth saving at all is a fair question, and the answer is that it keeps the three
vectorizers interchangeable: code that persists a fitted model works unchanged when the model is
this one. The file is small.

The write is not bounded the way a load is. A stop-word list past a default of
[`ArtifactLoadOptions`](../persistence/artifactloadoptions.md) — a word over 1024 characters, more
than a million words — saves, and loads only with that bound raised.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`HashingVectorizer.Load`](hashingvectorizer-load.md),
[`HashingVectorizer.SaveAsync`](hashingvectorizer-saveasync.md).
