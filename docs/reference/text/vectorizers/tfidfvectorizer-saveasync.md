# TfidfVectorizer.SaveAsync

Write a fitted vectorizer out without blocking the caller.

<!-- docs-declaration -->

```csharp
public Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
```

**Parameters** — `destination` is a writable stream, left open. `cancellationToken` cancels the
write.

**Returns** — `Task`, completing when the vectorizer has been written.

**Exceptions** — `InvalidOperationException` when nothing has been fitted yet, or when the idf
weights make a base64 block within two mebibytes of the most the JSON writer holds in one buffer, from
201,129,984 weights, or
when a vocabulary term, the token pattern or a stop word is beyond what the JSON writer
can write.
`InvalidDataException` when an idf weight is not finite. All are refused before anything is written
([#1617](https://github.com/CyrilB1531/lodestar/issues/1617), [#1618](https://github.com/CyrilB1531/lodestar/issues/1618)). `ArgumentNullException` for a null
stream. `OperationCanceledException` when cancelled.

**Example** — the round trip, asynchronously.

```csharp
using Lodestar.Text.Vectorization;

static async Task<string> RoundTripAsync()
{
    var tv = new TfidfVectorizer();
    tv.Fit(["the cat eats", "the dog eats"]);

    using var buffer = new MemoryStream();
    await tv.SaveAsync(buffer);
    buffer.Position = 0;

    TfidfVectorizer restored = await TfidfVectorizer.LoadAsync(buffer);
    return restored.GetFeatureNames()[0];
}

string first = RoundTripAsync().GetAwaiter().GetResult();  // => cat
```

The `GetAwaiter().GetResult()` is only what lets a synchronous example drive an async one; in
async code the call is simply `await`.

**Remarks** — as with [`CountVectorizer.SaveAsync`](countvectorizer-saveasync.md) there is no
`string path` overload: opening the file with `useAsync: true` is the caller's decision, and
hiding it here would hide whether the asynchrony reaches the disk.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TfidfVectorizer.Save`](tfidfvectorizer-save.md),
[`TfidfVectorizer.LoadAsync`](tfidfvectorizer-loadasync.md).
