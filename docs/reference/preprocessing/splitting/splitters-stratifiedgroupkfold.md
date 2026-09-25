# Splitters.StratifiedGroupKFold

Cuts group folds that keep each label's share, as `StratifiedGroupKFold` does.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> StratifiedGroupKFold(ReadOnlySpan<int> labels, ReadOnlySpan<int> groups, int foldCount)
public static IReadOnlyList<FoldSplit> StratifiedGroupKFold(ReadOnlySpan<int> labels, ReadOnlySpan<int> groups, int foldCount, long randomState)
```

**Parameters** — `labels` is one class label per row. `groups` is one group label per row, as many
as `labels`. `foldCount` is how many folds to cut. `randomState` is scikit-learn's `random_state`,
in `[0, 2³² − 1]`: it selects `StratifiedGroupKFold(shuffle=True)`.

**Returns** — one `FoldSplit` per fold, in order, each index list ascending. No group straddles two
folds.

**Exceptions** — `ArgumentOutOfRangeException` when the row count, `foldCount` or `randomState` is
out of range. `ArgumentException` when the two spans differ in length, when `foldCount` is above
every class's count, or when it is above the number of distinct groups.

**Example** — two classes over four groups, in two folds.

```csharp
using Lodestar.Preprocessing;

int[] labels = [0, 0, 1, 1, 0, 1, 0, 1, 1, 0];
int[] groups = [1, 1, 1, 2, 2, 3, 4, 4, 4, 4];

IReadOnlyList<FoldSplit> folds = Splitters.StratifiedGroupKFold(labels, groups, foldCount: 2);
string first = string.Join(",", folds[0].TestIndices);   // => 0,1,2,6,7,8,9
string second = string.Join(",", folds[1].TestIndices);  // => 3,4,5

string seeded = string.Join(",", Splitters.StratifiedGroupKFold(labels, groups, 2, randomState: 0)[0].TestIndices);  // => 5,6,7,8,9
```

**Remarks** — the reference's greedy search, step for step: groups by descending spread of their
class counts, each to the fold that minimises the mean, over classes, of the spread of the folds'
shares of that class; a fold within `np.isclose` of the best wins if it holds fewer rows. The shuffled
form only changes the order ties between groups are met in.

**The spreads are numpy's to the last bit**, because a tie between two folds decides where a group
goes. numpy sums a row pairwise from its first element and a column row by row; a random
differential run found a five-class case whose fold turns on that order, and it is frozen in the
corpus. A fold count above the smallest class makes the reference warn, and is accepted here.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.GroupKFold`](splitters-groupkfold.md),
[`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md), [`FoldSplit`](foldsplit.md).
