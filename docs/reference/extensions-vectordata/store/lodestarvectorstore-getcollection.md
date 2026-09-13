# LodestarVectorStore.GetCollection

The collection held under a name, created on first request.

<!-- docs-declaration -->

```csharp
public VectorStoreCollection<TKey, TRecord> GetCollection<TKey, TRecord>(string name, VectorStoreCollectionDefinition definition = null) where TKey : notnull where TRecord : class
```

**Parameters** — `name` is the collection's name, and the key the store holds it under. `definition`
is an explicit schema; `null` reads the key, vector and data attributes on `TRecord` instead.

**Returns** — a [`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md), typed as the
abstraction's `VectorStoreCollection<TKey, TRecord>`. Cast it to `IKeywordHybridSearchable<TRecord>`
to reach [`LodestarVectorStoreCollection.HybridSearchAsync`](lodestarvectorstorecollection-hybridsearchasync.md).

**Exceptions** — `ArgumentNullException` when `name` is null. `ArgumentException` when `name` is
already held over a different `TKey` or `TRecord`, or when the schema is unusable: no key property,
no vector property, or a vector property that does not hold `ReadOnlyMemory<float>`.

**Example** — the same name twice is the same collection, and asking for it does not create it.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> AskTwiceAsync()
{
    using var store = new LodestarVectorStore();
    VectorStoreCollection<string, Note> first = store.GetCollection<string, Note>("notes");
    VectorStoreCollection<string, Note> second = store.GetCollection<string, Note>("notes");

    bool before = await store.CollectionExistsAsync("notes");
    await first.EnsureCollectionExistsAsync();
    bool after = await store.CollectionExistsAsync("notes");

    return $"{ReferenceEquals(first, second)} {before} {after}";
}

string seen = AskTwiceAsync().GetAwaiter().GetResult();  // => True False True
```

**Remarks** — **the first request decides the schema.** A second call with the same name returns the
collection already held, and its `definition` is not read: a store cannot hold two schemas under one
name, and quietly rebuilding the first would drop its records. A different `TKey` or `TRecord` is
refused rather than ignored, because the cast that would follow could only fail somewhere less
helpful.

The collection is **created, not made to exist**. It answers `false` to
[`LodestarVectorStore.CollectionExistsAsync`](lodestarvectorstore-collectionexistsasync.md) until
[`LodestarVectorStoreCollection.EnsureCollectionExistsAsync`](lodestarvectorstorecollection-ensurecollectionexistsasync.md)
or a write, which is the abstraction's contract for a server-backed store and holds here too.

Every collection takes the [`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md) the store
was constructed with; a collection wanting different ones is constructed directly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore`](lodestarvectorstore.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
