# IndelPattern.For

Builds a handle over a pattern, renting the equality table every later scan reads.

<!-- docs-declaration -->

```csharp
public static IndelPattern For(string pattern)
public static IndelPattern For(ReadOnlySpan<char> pattern)
```

**Parameters** — `pattern` is the one side every scan compares against.

**Returns** — an [`IndelPattern`](indelpattern.md) to
[dispose](indelpattern-dispose.md) once the last text has been scanned.

**Exceptions** — `ArgumentNullException` when `pattern` is null.

**Example** — one query against a list, the table built once.

```csharp
using Lodestar.Text.Distances;

string[] candidates = ["kitten", "sitting", "mitten", "bitten"];
using IndelPattern query = IndelPattern.For("kitten");

double best = candidates.Max(c => query.NormalizedSimilarity(c.AsSpan()));   // => 1
int worst = candidates.Max(c => query.Distance(c.AsSpan()));                 // => 5
```

**Remarks** — the span overload copies, because a handle outlives the call that built it and a
`ReadOnlySpan<char>` cannot be held. Hand it a `string` where you have one and no copy is made.

The table is rented from `ArrayPool<ulong>`, so a handle that is never disposed leaks nothing but
does force the pool to allocate a replacement. A `using` statement is the intended shape.

Building a handle for a single comparison is a loss: [`Indel.Distance`](indel-distance.md) takes
two strings and does not have to fill a table that outlives the call.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IndelPattern`](indelpattern.md), [`Indel`](indel.md),
[`IndelPattern.Dispose`](indelpattern-dispose.md).
