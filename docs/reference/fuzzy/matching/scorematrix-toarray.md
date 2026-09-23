# ScoreMatrix.ToArray

The whole matrix, row-major.

<!-- docs-declaration -->

```csharp
public double[] ToArray()
```

**Returns** — a copy of every score, cell `[row, column]` at `row * Columns + column`.

**Example** — the flat layout, and where a cell sits in it.

```csharp
using Lodestar.Fuzzy;

string[] queries = ["new york", "boston"];
string[] choices = ["new york mets", "boston red sox", "atlanta braves"];

ScoreMatrix scores = Process.Cdist(queries, choices);
double[] flat = scores.ToArray();

int cells = flat.Length;                                          // => 6
double atOneOne = Math.Round(flat[(1 * scores.Columns) + 1], 4);  // => 60
```

**Remarks — a copy, on purpose.** The matrix is immutable and hands out no reference to its own
storage; a caller who wants to read without copying takes [`Row`](scorematrix-row.md), which is a
span over it.

**Row-major is the layout every bulk member in this repository takes** — `Lodestar.Preprocessing`'s
transformers, `Lodestar.Cluster`'s fits — so this array goes straight into one of them without a
transpose.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ScoreMatrix`](scorematrix.md), [`ScoreMatrix.Row`](scorematrix-row.md).
