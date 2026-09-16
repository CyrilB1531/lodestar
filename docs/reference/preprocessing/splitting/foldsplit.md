# FoldSplit

One fold of a cross-validation split: which rows train, and which are held out.

<!-- docs-declaration -->

```csharp
public sealed class FoldSplit
```

**Properties** — `TrainIndices` is the rows this fold fits on and `TestIndices` the rows it holds
out, both ascending and both `IReadOnlyList<int>`. Together they are every row, each exactly once.

**Example** — the two sides of one fold.

```csharp
using Lodestar.Preprocessing;

FoldSplit fold = Splitters.KFold(sampleCount: 10, foldCount: 5)[2];

string held = string.Join(",", fold.TestIndices);   // => 4,5
int fitted = fold.TrainIndices.Count;               // => 8
```

**Remarks** — indices rather than rows. A splitter that copied the data would decide the caller's
layout for them, and the indices are what a fit and a score both take:
`scaler.Fit(Gather(samples, fold.TrainIndices), featureCount)` is the caller's own gather, over
whatever container the data actually lives in.

There is no constructor: a `FoldSplit` comes from [`Splitters`](splitters.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.KFold`](splitters-kfold.md),
[`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md), [`TrainTestSplit`](traintestsplit.md).
