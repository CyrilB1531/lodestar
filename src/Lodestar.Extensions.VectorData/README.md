# Lodestar.Extensions.VectorData

An in-process `Microsoft.Extensions.VectorData` store: collections of records with
vector search over Lodestar's `EmbeddingIndex`, keyword search over `Bm25Index`, and hybrid search
fusing the two by reciprocal rank. Nothing to deploy: the store lives in the process that uses
it.

`Note` is any record class carrying a key, a text and a vector property.

## Install

```bash
dotnet add package Lodestar.Extensions.VectorData
```

## Example

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> FuseAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "an elephant crossed the river", Embedding = new float[] { 0f, 0f, 1f } },
    ]);

    List<VectorSearchResult<Note>> hits = await notes
        .HybridSearchAsync(new float[] { 1f, 0f, 0f }, ["elephant", "zebra"], 2)
        .ToListAsync();
    return string.Join(",", hits.Select(hit => hit.Record.Id));   // c,a
}
```

## Parity

Its search sits on `Lodestar.Embeddings` and `Lodestar.Text`, which carry the parity;
the store follows the `Microsoft.Extensions.VectorData` contracts.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A interop package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Embeddings` 0.8.0 or later
- `Lodestar.Text` 0.7.0 or later
- `Microsoft.Extensions.VectorData.Abstractions` 10.10.0 or later

## Documentation

- Reference: [extensions-vectordata/store](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/extensions-vectordata/store.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.VectorData/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.VectorData/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
