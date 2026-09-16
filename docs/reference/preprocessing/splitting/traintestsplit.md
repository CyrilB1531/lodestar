# TrainTestSplit

A single train and test split of the rows.

<!-- docs-declaration -->

```csharp
public sealed class TrainTestSplit
```

**Properties** — `TrainIndices` is the rows to fit on and `TestIndices` the rows held out, both
ascending and both `IReadOnlyList<int>`. The two are disjoint and cover every row.

**Example** — a fifth of twenty rows.

```csharp
using Lodestar.Preprocessing;

TrainTestSplit split = Splitters.TrainTest(sampleCount: 20, testFraction: 0.2);

int fitted = split.TrainIndices.Count;             // => 16
string held = string.Join(",", split.TestIndices); // => 16,17,18,19
```

**Remarks** — the same shape as [`FoldSplit`](foldsplit.md) and a different type on purpose: a
train/test split is one split, not the first of several, and returning a one-element fold list would
invite a loop that runs once.

There is no constructor: a `TrainTestSplit` comes from
[`Splitters.TrainTest`](splitters-traintest.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.TrainTest`](splitters-traintest.md), [`FoldSplit`](foldsplit.md).
