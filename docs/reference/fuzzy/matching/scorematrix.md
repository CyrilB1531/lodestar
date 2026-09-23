# ScoreMatrix

A score for every pair of two collections — what [`Process.Cdist`](process-cdist.md) answers with.

<!-- docs-declaration -->

```csharp
public readonly struct ScoreMatrix
```

**Properties** — `Rows` is how many queries were scored, `Columns` how many choices each was
scored against.

**Example** — the shape, one cell, and one row.

```csharp
using Lodestar.Fuzzy;

string[] queries = ["new york", "boston"];
string[] choices = ["new york mets", "boston red sox", "atlanta braves"];

ScoreMatrix scores = Process.Cdist(queries, choices);

int cells = scores.Rows * scores.Columns;              // => 6
double first = Math.Round(scores[0, 0], 4);            // => 76.1905

ReadOnlySpan<double> boston = scores.Row(1);
double bostonBest = Math.Round(boston[1], 4);          // => 60
```

**Remarks — a type rather than a bare `double[]`, because the shape is not an input here.** Every
bulk member of `Lodestar.Preprocessing` takes a `featureCount` beside its span, so a caller
already holds the width; `Cdist` derives it from the two lists, and handing back an array alone
would make the caller carry the column count next to it.

**The storage is row-major**, the layout every bulk member in this repository uses: cell
`[row, column]` lives at `row * Columns + column`, and [`ToArray`](scorematrix-toarray.md) hands
it over in that order. [`Row`](scorematrix-row.md) is a window onto it and copies nothing.

**Equality is on the storage, not the cells.** Two matrices are equal when they are the same
matrix; comparing two sets of scores element-wise is what `Row` is for. That keeps `Equals` `O(1)`
on a type whose whole point is to be large.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Process.Cdist`](process-cdist.md), the [matching index](../matching.md).

## Members

| Member | What it does |
| --- | --- |
| [`ScoreMatrix.Equals`](scorematrix-equals.md) | Whether two handles are the same matrix. |
| [`ScoreMatrix.GetHashCode`](scorematrix-gethashcode.md) | The shape's hash. |
| [`ScoreMatrix.Row`](scorematrix-row.md) | One query's scores, without copying. |
| [`ScoreMatrix.ToArray`](scorematrix-toarray.md) | The whole matrix, row-major. |
