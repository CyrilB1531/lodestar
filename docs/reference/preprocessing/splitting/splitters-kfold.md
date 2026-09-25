# Splitters.KFold

Cuts the rows into contiguous folds, as `KFold(n_splits=foldCount)` does without shuffling.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount)
public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount, ReadOnlySpan<int> order)
public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount, long randomState)
```

**Parameters** — `sampleCount` is how many rows there are. `foldCount` is how many folds to cut, at
least two and at most `sampleCount`. `order` is a permutation of `0..sampleCount−1` the rows are
read in; an empty span reads them in order, which is what the two-argument overload passes.
`randomState` is scikit-learn's `random_state`, in `[0, 2³² − 1]`: it selects
`KFold(shuffle=True, random_state=randomState)`, numpy's generator replayed.

**Returns** — one `FoldSplit` per fold, in order, each index list ascending.

**Exceptions** — `ArgumentOutOfRangeException` when `sampleCount` is below two, or `foldCount` is
below two or above `sampleCount`. `ArgumentException` when `order` is neither empty nor a
permutation of the rows — a repeated index, an index out of range, or a length that is not
`sampleCount`. `ArgumentOutOfRangeException` when `randomState` is outside numpy's range.

**Example** — ten rows into three folds, and the same ten read in a permuted order.

```csharp
using Lodestar.Preprocessing;

IReadOnlyList<FoldSplit> folds = Splitters.KFold(sampleCount: 10, foldCount: 3);

// 10 = 3 + 3 + 3 + 1, and the leftover row goes to the first fold, not the last.
string first = string.Join(",", folds[0].TestIndices);   // => 0,1,2,3
string second = string.Join(",", folds[1].TestIndices);  // => 4,5,6
string third = string.Join(",", folds[2].TestIndices);   // => 7,8,9

// Reading the rows in another order cuts the same block sizes out of that order.
int[] order = [9, 4, 1, 7, 0, 3, 6, 8, 2, 5];
string shuffled = string.Join(",", Splitters.KFold(10, 3, order)[0].TestIndices);  // => 1,4,7,9

// scikit-learn's KFold(3, shuffle=True, random_state=42), fold for fold.
string seeded = string.Join(",", Splitters.KFold(10, 3, randomState: 42)[0].TestIndices);  // => 0,1,5,8
```

**Remarks** — the first `sampleCount % foldCount` folds take one extra row. That is the reference's
placement and it is worth stating, because the other plausible rule — giving the remainder to the
last folds — agrees with it only when the division is exact.

Every row is held out by exactly one fold and fitted on by the other `foldCount − 1`, permuted or
not; that is asserted rather than assumed.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md), [`FoldSplit`](foldsplit.md).
