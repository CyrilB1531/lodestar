# NpyBlock.Equals

Value equality over the elements and the shape.

<!-- docs-declaration -->

```csharp
public bool Equals(NpyBlock other)
```

**Parameters** — `other` is the block to compare against.

**Returns** — `bool`, true when both blocks hold the same number of elements with equal values in
the same order, and declare the same shape.

**Example** — two reads of the same bytes.

```csharp
using Lodestar.Embeddings.Persistence;

using var buffer = new MemoryStream();
NpyFile.Write(buffer, [1f, 2f, 3f, 4f], 2, 2);
byte[] bytes = buffer.ToArray();

NpyBlock first = NpyFile.Read(new ReadOnlyMemory<byte>(bytes));
NpyBlock second = NpyFile.Read(new ReadOnlyMemory<byte>(bytes));

bool equal = first.Equals(second);  // => True
```

**Remarks** — a `record struct` over `ReadOnlyMemory<float>` and a list would compare both by
reference, so two reads of one file were unequal
(the equality rule
is why this one is written out). Elements compare the way `float.Equals` does, so `NaN` equals
`NaN`. `OwnedArray` is not compared: who owns the array is not part of the block's value.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NpyBlock`](npyblock.md), [`NpyBlock.GetHashCode`](npyblock-gethashcode.md).
