# CountVectorizer.SaveAsync

Write a fitted vectorizer out without blocking the caller.

<!-- docs-declaration -->

```csharp
public Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
```

**Parameters** — `destination` is a writable stream, left open. `cancellationToken` cancels the
write.

**Returns** — `Task`, completing when the vectorizer has been written.

**Exceptions** — `InvalidOperationException` when nothing has been fitted yet. `ArgumentException`,
the JSON writer's own, as 0.7.0 raised it, when a vocabulary term, the token pattern or a stop word
is beyond what the writer can write. Both are refused before anything is written
([#1618](https://github.com/CyrilB1531/lodestar/issues/1618),
[#1646](https://github.com/CyrilB1531/lodestar/issues/1646)). `NotSupportedException`, the stream's
own, when it cannot be written to, raised by the write once the artifact is composed, as 0.7.0
raised it. `IndexOutOfRangeException`, the writer's own as well, for a string whose escaped form
passes its buffer, as 0.7.0 raised it. `ArgumentNullException` for a null stream.
`OperationCanceledException` when cancelled.

**Example** — the same round trip as [`Save`](countvectorizer-save.md), asynchronously.

```csharp
using Lodestar.Text.Vectorization;

static async Task<int> RoundTripAsync()
{
    var cv = new CountVectorizer();
    cv.Fit(["the cat eats", "the dog eats"]);

    using var buffer = new MemoryStream();
    await cv.SaveAsync(buffer);
    buffer.Position = 0;

    CountVectorizer restored = await CountVectorizer.LoadAsync(buffer);
    return restored.Transform(["the cat"]).ColumnCount;
}

int columns = RoundTripAsync().GetAwaiter().GetResult();  // => 4
```

The `GetAwaiter().GetResult()` is only what lets a synchronous example drive an async one; in
async code the call is simply `await`.

**Remarks** — there is no `SaveAsync(string path)` overload to match
[`Save`](countvectorizer-save.md)'s: opening the file is the caller's, and a `FileStream` created
with `useAsync: true` is what makes the asynchrony reach the disk rather than stopping at a
buffer.

Worth using when the destination is a network stream or a large file, and not otherwise — a
vocabulary is small, and the synchronous overload avoids the machinery.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CountVectorizer.Save`](countvectorizer-save.md),
[`CountVectorizer.LoadAsync`](countvectorizer-loadasync.md).
