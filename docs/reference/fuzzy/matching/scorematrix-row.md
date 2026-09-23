# ScoreMatrix.Row

One query's scores against every choice, without copying.

<!-- docs-declaration -->

```csharp
public ReadOnlySpan<double> Row(int row)
```

**Parameters** — `row` is the query's index.

**Returns** — a window onto that row of the matrix, `Columns` values wide; empty when there were
no choices.

**Exceptions** — `ArgumentOutOfRangeException` when `row` is outside the matrix.

**Example** — the second query's scores.

```csharp
using Lodestar.Fuzzy;

string[] queries = ["new york", "boston"];
string[] choices = ["new york mets", "boston red sox", "atlanta braves"];

ReadOnlySpan<double> boston = Process.Cdist(queries, choices).Row(1);

int width = boston.Length;                       // => 3
double best = Math.Round(boston[1], 4);          // => 60
```

**Remarks — a window, not a copy.** The storage is row-major, so a row is contiguous and this
hands back a span over it; [`ToArray`](scorematrix-toarray.md) is what copies. A row of a matrix
with no columns is an empty span rather than a refusal, which is the shape the reference answers
an empty choice list with.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ScoreMatrix`](scorematrix.md), [`ScoreMatrix.ToArray`](scorematrix-toarray.md).
