# LodestarVectorStoreCollection.GetService

Answers for the collection's metadata.

<!-- docs-declaration -->

```csharp
public object GetService(Type serviceType, object serviceKey = null)
```

**Parameters** — `serviceType` is the service being asked for. `serviceKey` names a keyed
registration; nothing here is registered under one, so a keyed lookup answers `null`.

**Returns** — a `VectorStoreCollectionMetadata` whose `VectorStoreSystemName` is `lodestar` and whose
`CollectionName` is the collection's `Name`, when `serviceType` is that type and `serviceKey` is
`null`; `null` for anything else.

**Example** — the collection's name, reached through the abstraction.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

using VectorStoreCollection<string, Note> notes = new LodestarVectorStoreCollection<string, Note>("notes");
var about = (VectorStoreCollectionMetadata)notes.GetService(typeof(VectorStoreCollectionMetadata));

string described = $"{about.VectorStoreSystemName}/{about.CollectionName}";  // => lodestar/notes
```

**Remarks** — the base class's member, and `IKeywordHybridSearchable<TRecord>`'s, are the same
method here, so a consumer holding either interface gets the same answer. `VectorStoreName` is not
set, since a collection constructed directly belongs to no store.

A new metadata object is returned on each call. A `null` `serviceType` answers `null` rather than
throwing.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore.GetService`](lodestarvectorstore-getservice.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
