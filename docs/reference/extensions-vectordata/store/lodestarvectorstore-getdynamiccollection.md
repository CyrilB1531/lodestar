# LodestarVectorStore.GetDynamicCollection

Refuses: this store serves typed records only.

<!-- docs-declaration -->

```csharp
public VectorStoreCollection<object, Dictionary<string, object>> GetDynamicCollection(string name, VectorStoreCollectionDefinition definition)
```

**Parameters** — `name` would name the collection and `definition` would describe its schema. Neither
is read.

**Returns** — never returns.

**Exceptions** — `NotSupportedException`, always.

**Example** — the refusal, and what it says.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

using var store = new LodestarVectorStore();
string reason;
try
{
    store.GetDynamicCollection("notes", new VectorStoreCollectionDefinition());
    reason = "served";
}
catch (NotSupportedException error)
{
    reason = error.Message;
}

string refusal = reason;  // => This store serves typed records only.…
```

**Remarks** — the base class declares this member abstract, so it is present; it refuses rather than
returning a collection that would then refuse every filter. A dynamic record is a
`Dictionary<string, object>`, and a filter in this package is an `Expression<Func<TRecord, bool>>`
compiled against the record's **properties** — a dictionary has none to bind to. Interpreting the
expression over dictionary lookups instead is a second filter path, and the typed key the collection
is built on disappears with it.

Declare a record type and call
[`LodestarVectorStore.GetCollection`](lodestarvectorstore-getcollection.md) instead.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore`](lodestarvectorstore.md).
