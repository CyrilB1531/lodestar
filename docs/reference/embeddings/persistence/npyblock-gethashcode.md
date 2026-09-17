# NpyBlock.GetHashCode

A hash consistent with that equality.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — `int`, over the element count and the number of dimensions rather than the values.

**Example** — equal blocks hash alike.

```csharp
using Lodestar.Embeddings.Persistence;

var left = new NpyBlock(new float[] { 1f, 2f }, [2]);
var right = new NpyBlock(new float[] { 1f, 2f }, [2]);

bool hashesAlike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — a block of embeddings holds millions of floats, and hashing them on every call would
cost what a lookup is meant to save. Two blocks that share a hash are told apart by
[`Equals`](npyblock-equals.md), which reads every element.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NpyBlock.Equals`](npyblock-equals.md).
