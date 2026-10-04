# HashingVectorizer.SaveAsync

Write the options out without blocking the caller.

<!-- docs-declaration -->

```csharp
public Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
```

**Parameters** — `destination` is a writable stream, left open. `cancellationToken` cancels the
write.

**Returns** — `Task`, completing when the options have been written.

**Exceptions** — `ArgumentException`, the JSON writer's own, as 0.7.0 raised it, when the token
pattern or a stop word is beyond what the writer can write. `InvalidDataException` when `MinDf` or
`MaxDf` is not finite, which the constructor does not refuse. Both are refused before anything is
written ([#1618](https://github.com/CyrilB1531/lodestar/issues/1618),
[#1622](https://github.com/CyrilB1531/lodestar/issues/1622),
[#1646](https://github.com/CyrilB1531/lodestar/issues/1646)). `NotSupportedException`, the stream's
own, when it cannot be written to, raised by the write once the artifact is composed, as 0.7.0
raised it. `IndexOutOfRangeException`, the writer's own as well, for a string whose escaped form
passes its buffer, as 0.7.0 raised it. `ArgumentNullException` for a null stream.
`OperationCanceledException` when cancelled.

**Example** — the round trip, asynchronously.

```csharp
using Lodestar.Text.Vectorization;

static async Task<int> RoundTripAsync()
{
    var hv = new HashingVectorizer(new HashingVectorizerOptions { NumFeatures = 16 });

    using var buffer = new MemoryStream();
    await hv.SaveAsync(buffer);
    buffer.Position = 0;

    HashingVectorizer restored = await HashingVectorizer.LoadAsync(buffer);
    return restored.NumFeatures;
}

int columns = RoundTripAsync().GetAwaiter().GetResult();  // => 16
```

The `GetAwaiter().GetResult()` is only what lets a synchronous example drive an async one; in
async code the call is simply `await`.

**Remarks** — what is written is a handful of settings, so the asynchronous overload buys little
here beyond keeping this type interchangeable with the other two. As with them, there is no
`string path` overload: opening the file with `useAsync: true` belongs to the caller.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`HashingVectorizer.Save`](hashingvectorizer-save.md),
[`HashingVectorizer.LoadAsync`](hashingvectorizer-loadasync.md).
