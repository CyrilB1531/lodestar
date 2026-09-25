# Splitters.GroupKFold

Cuts folds that never split a group, as `GroupKFold` does.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> GroupKFold(ReadOnlySpan<int> groups, int foldCount)
public static IReadOnlyList<FoldSplit> GroupKFold(ReadOnlySpan<int> groups, int foldCount, long randomState)
```

**Parameters** — `groups` is one group label per row, any integers. `foldCount` is how many folds to
cut, at least two and at most the number of distinct groups. `randomState` is scikit-learn's
`random_state`, in `[0, 2³² − 1]`: it selects `GroupKFold(shuffle=True)`.

**Returns** — one `FoldSplit` per fold, in order, each index list ascending. Every row of a group is
held out by the same fold.

**Exceptions** — `ArgumentOutOfRangeException` when there are fewer than two rows, when `foldCount`
is below two or above the row count, or when `randomState` is outside numpy's range.
`ArgumentException` when `foldCount` is above the number of distinct groups, which the reference
refuses too.

**Example** — four groups of three, two, one and four rows, over two folds.

```csharp
using Lodestar.Preprocessing;

int[] groups = [1, 1, 1, 2, 2, 3, 4, 4, 4, 4];

// The largest group first, each to the fold with fewest rows so far: 4 → fold 0, 1 → fold 1, 2 → 1, 3 → 0.
IReadOnlyList<FoldSplit> folds = Splitters.GroupKFold(groups, foldCount: 2);
string first = string.Join(",", folds[0].TestIndices);   // => 5,6,7,8,9
string second = string.Join(",", folds[1].TestIndices);  // => 0,1,2,3,4

// Shuffled with scikit-learn's own seed: the distinct groups are permuted, then cut into runs.
string seeded = string.Join(",", Splitters.GroupKFold(groups, 2, randomState: 0)[0].TestIndices);  // => 5,6,7,8,9
```

**Remarks** — unshuffled, the folds balance **rows**: groups go largest first, each to the fold
holding the fewest rows, the first such fold on a tie. Groups of equal size go in descending label
order, which is what the reference's reversed stable sort gives. Shuffled, they balance **groups**:
the distinct groups, ascending, are permuted by numpy's generator and cut by `np.array_split`, the
first `groupCount % foldCount` runs one group longer, whatever their sizes.

Both are replayed index for index from `tests/oracles/preprocessing_splitters_seeded.json`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.StratifiedGroupKFold`](splitters-stratifiedgroupkfold.md),
[`Splitters.KFold`](splitters-kfold.md), [`FoldSplit`](foldsplit.md).
