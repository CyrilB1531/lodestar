# ScoreMatrix.GetHashCode

The shape's hash.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash combining [`Rows`](scorematrix.md) and [`Columns`](scorematrix.md).

**Example** — the same handle hashes the same.

```csharp
using Lodestar.Fuzzy;

ScoreMatrix scores = Process.Cdist(["new york"], ["new york mets", "boston red sox"]);
ScoreMatrix sameHandle = scores;

bool agree = scores.GetHashCode() == sameHandle.GetHashCode();   // => True
```

**Remarks — the shape alone, on purpose.** Two matrices of the same shape collide, which costs a
hash bucket and never a wrong answer:
[`Equals`](scorematrix-equals.md) settles it on the storage. Hashing the cells would walk the
whole matrix to place it in a dictionary, which is work a caller of this type is not asking for.

Hand-rolled rather than `HashCode.Combine`, which netstandard2.0 does not carry and no polyfill
in this repository supplies.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ScoreMatrix`](scorematrix.md), [`ScoreMatrix.Equals`](scorematrix-equals.md).
