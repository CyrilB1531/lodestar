# Splitters.RepeatedStratifiedKFold

Repeats shuffled stratified k-fold, as `RepeatedStratifiedKFold` does.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> RepeatedStratifiedKFold(ReadOnlySpan<int> labels, int foldCount, int repeatCount, long randomState)
```

**Parameters** — `labels` is one class label per row. `foldCount` is how many folds each repeat
cuts. `repeatCount` is how many repeats, at least one. `randomState` is scikit-learn's
`random_state`, in `[0, 2³² − 1]`.

**Returns** — `repeatCount · foldCount` folds, repeat by repeat, each index list ascending.

**Exceptions** — `ArgumentOutOfRangeException` when a count or `randomState` is out of range.
`ArgumentException` when `foldCount` is above every class's count.

**Example** — twelve rows in three classes, three folds, two repeats.

```csharp
using Lodestar.Preprocessing;

int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

IReadOnlyList<FoldSplit> folds = Splitters.RepeatedStratifiedKFold(labels, foldCount: 3, repeatCount: 2, randomState: 0);

int count = folds.Count;                                        // => 6
string first = string.Join(",", folds[0].TestIndices);          // => 2,4,7,11
string fourth = string.Join(",", folds[3].TestIndices);         // => 1,5,6,10
```

**Remarks** — each repeat is [`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md) with a seed,
drawing from the one generator the repeats share.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.RepeatedKFold`](splitters-repeatedkfold.md),
[`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md).
