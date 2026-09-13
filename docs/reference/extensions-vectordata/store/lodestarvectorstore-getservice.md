# LodestarVectorStore.GetService

Answers for the store's metadata.

<!-- docs-declaration -->

```csharp
public object GetService(Type serviceType, object serviceKey = null)
```

**Parameters** — `serviceType` is the service being asked for. `serviceKey` names a keyed
registration; nothing here is registered under one, so a keyed lookup answers `null`.

**Exceptions** — `ArgumentNullException` when `serviceType` is null.

**Returns** — a `VectorStoreMetadata` whose `VectorStoreSystemName` is `lodestar` when `serviceType`
is that type and `serviceKey` is `null`; `null` for anything else.

**Example** — identifying the provider through the abstraction.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

using VectorStore store = new LodestarVectorStore();
var about = (VectorStoreMetadata)store.GetService(typeof(VectorStoreMetadata));

string system = about.VectorStoreSystemName;  // => lodestar
```

**Remarks** — service resolution is how `Microsoft.Extensions.VectorData` lets a consumer learn which
provider it holds without referencing the provider's types. `VectorStoreName` is not set: an
in-process store has no database name to report.

A new metadata object is returned on each call. A `null` `serviceType` throws, as the abstraction's
contract for `GetService` says it must.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.GetService`](lodestarvectorstorecollection-getservice.md),
[`LodestarVectorStore`](lodestarvectorstore.md).
