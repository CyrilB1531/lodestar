# ScoreMatrix.Equals

Whether two handles are the same matrix.

<!-- docs-declaration -->

```csharp
public bool Equals(ScoreMatrix other)
public bool Equals(object obj)
```

**Parameters** — `other`, or `obj`, is the matrix to compare with. The `object` overload answers
`false` for anything that is not a `ScoreMatrix`.

**Returns** — `true` when both handles carry the same shape and the same storage.

**Example** — a copy of the handle is the same matrix; a second call is not.

```csharp
using Lodestar.Fuzzy;

string[] queries = ["new york"];
string[] choices = ["new york mets", "boston red sox"];

ScoreMatrix scores = Process.Cdist(queries, choices);
ScoreMatrix sameHandle = scores;
ScoreMatrix scoredAgain = Process.Cdist(queries, choices);

bool copied = sameHandle.Equals(scores);        // => True
bool recomputed = scoredAgain.Equals(scores);   // => False
```

**Remarks — this compares the handles, not the cells.** A score matrix is a result a caller reads,
not a value they match on, and comparing two of them element-wise is what
[`Row`](scorematrix-row.md) is for. Keeping it a reference comparison also keeps it `O(1)` on a
type whose whole purpose is to be large — a cell-by-cell `Equals` on a 500 × 500 matrix would walk
a quarter of a million doubles to answer a question nobody asked.

**Two matrices of equal scores are therefore not equal**, as the example shows. That is the
deliberate reading; a caller who wants the scores compared takes `ToArray` on both and compares
those.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ScoreMatrix`](scorematrix.md), [`ScoreMatrix.Row`](scorematrix-row.md).
