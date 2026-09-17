# Splitters.StratifiedKFold

Cuts the rows into folds that keep each label's share, as `StratifiedKFold` does without shuffling.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> StratifiedKFold(ReadOnlySpan<int> labels, int foldCount)
public static IReadOnlyList<FoldSplit> StratifiedKFold(ReadOnlySpan<int> labels, int foldCount, ReadOnlySpan<int> order)
```

**Parameters** — `labels` carries one class label per row; any integers, and the row count is
`labels.Length`. `foldCount` is how many folds to cut, at least two and at most the row count.
`order` is a permutation of the rows to read them in; an empty span reads them in order.

**Returns** — one `FoldSplit` per fold, in order, each index list ascending.

**Exceptions** — `ArgumentOutOfRangeException` when the row count is below two, or `foldCount` is
below two or above it. `ArgumentException` when `order` is not a permutation of the rows, or when
`foldCount` is greater than **every** class's count, which would leave a fold with nothing to hold
out.

**Example** — twelve rows in three classes, three folds.

```csharp
using Lodestar.Preprocessing;

int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

IReadOnlyList<FoldSplit> folds = Splitters.StratifiedKFold(labels, foldCount: 3);

// Two rows of class 0 and one of each other class, in every fold.
string first = string.Join(",", folds[0].TestIndices);   // => 0,1,6,9
string second = string.Join(",", folds[1].TestIndices);  // => 2,3,7,10
string third = string.Join(",", folds[2].TestIndices);   // => 4,5,8,11
```

**Remarks** — fold `i` takes as many rows of class `c` as the sorted labels hold at positions `i`,
`i + foldCount`, `i + 2·foldCount`, and so on. That slicing is the reference's own, and it is not
the same as filling the earliest folds first: a class of two rows over three folds lands in folds 0
and **2**.

**A fold count above the smallest class is allowed, and costs that class a fold.** scikit-learn warns
here rather than refusing, and a warning is not a return value, so what it would have said is this:

```csharp
using Lodestar.Preprocessing;

// Class 1 has one row and cannot reach three folds.
IReadOnlyList<FoldSplit> sparse = Splitters.StratifiedKFold([0, 0, 0, 1], foldCount: 3);

string withBoth = string.Join(",", sparse[0].TestIndices);  // => 0,3
string withoutIt = string.Join(",", sparse[1].TestIndices); // => 1
```

Folds 1 and 2 hold out no row of class 1, so a score computed on them says nothing about that class.
Only when `foldCount` is above **every** class count is the input refused, because then some fold
holds out nothing at all.

**The class order is first appearance, not label value.** The reference encodes its classes by
ranking each label's first index, and the allocation above reads that order, so `[1,1,1,1,1,0,0,0,2,2]`
does not split the way `[0,0,0,1,1,1,1,1,2,2]` does. Reproduced deliberately: one frozen case in
`tests/oracles/preprocessing_splitters.json` separates the two rules. With an `order`, first
appearance is counted in that order, which is what the reference does when handed the labels read
that way.

**An order does not reproduce `StratifiedKFold(shuffle=True)`.** The reference shuffles each class's
list of folds, not the rows, so no permutation of the rows reaches its draw. What `order` gives is the
unshuffled folds over the rows read in that order — scikit-learn's `StratifiedKFold` over `y[order]`,
mapped back to row numbers.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.KFold`](splitters-kfold.md), [`FoldSplit`](foldsplit.md).
