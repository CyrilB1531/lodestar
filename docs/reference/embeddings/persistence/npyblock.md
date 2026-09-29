# NpyBlock

A float block read from a `.npy` file, with the shape it was stored under.

<!-- docs-declaration -->

```csharp
public readonly record struct NpyBlock(ReadOnlyMemory<float> Values, IReadOnlyList<int> Shape)
{
    public float[]? OwnedArray { get; init; }
}
```

**Parameters** — `Values` are the elements in C order, so a matrix reads row by row.
`Shape` has one entry for a vector and two for a matrix. `OwnedArray` is the array the block
owns, or `null` when it borrows one — set by the reader, and settable by a caller, which it then
takes on trust.

**Example** — the shape is the file's, not a guess.

```csharp
using Lodestar.Embeddings.Persistence;

using var buffer = new MemoryStream();
NpyFile.Write(buffer, [1f, 2f, 3f, 4f, 5f, 6f], 2, 3);
buffer.Position = 0;

NpyBlock block = NpyFile.Read(buffer);
string shape = string.Join("x", block.Shape);   // => 2x3
bool adoptable = block.OwnedArray is not null;  // => True
```

**Remarks** — `Values` is `ReadOnlyMemory<float>` rather than an array because the block is the
file's content and not the caller's buffer to grow. `.Span` reads it without copying; `.ToArray()`
takes a copy when one is wanted.

`Shape` is what the file declared and what the read was bounded against — the element count is
checked before it sizes anything, which is the rule every loader here follows.

`Values` and `Shape` are taken and exposed as given, not copied: writing to the caller's memory or
list changes what the block equals, and every holder of it, a `with` copy included
([#1305](https://github.com/CyrilB1531/lodestar/issues/1305)).

**The reader fills `OwnedArray` when it allocated the array `Values` covers.** Reading from a
`Stream` or a path allocates an array nothing else holds, and so does reading a
`ReadOnlyMemory<byte>` on a big-endian host, which swaps into a copy. That array is what a caller
may pass to [`EmbeddingIndex.FromOwnedBlock`](../search/embeddingindex-fromownedblock.md) instead
of copying the block again — the transfer
`docs/guides/performance.md`
puts on the caller. Nothing in Lodestar reads `OwnedArray` itself. A little-endian read of a
`ReadOnlyMemory<byte>` leaves it null, because those values are borrowed and there is no array to
give; so does a block you construct without setting it.

**`OwnedArray` is a public `init` property, and nothing checks it.** A caller may set it to any
array, which the block neither compares nor uses; handing that array to `FromOwnedBlock` then
indexes whatever it holds. A `with` copy that replaces `Values` keeps the old `OwnedArray`, which
no longer holds the block's elements — clear it in the same `with`.
`docs/guides/performance.md`
has why the two reads differ.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NpyFile`](npyfile.md), [`NpyFile.Read`](npyfile-read.md),
[the persistence index](../persistence.md).

## Members

| Member | What it does |
| --- | --- |
| [`NpyBlock.Equals`](npyblock-equals.md) | Value equality over the elements and the shape. |
| [`NpyBlock.GetHashCode`](npyblock-gethashcode.md) | A hash consistent with it. |
