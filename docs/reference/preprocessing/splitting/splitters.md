# Splitters

Cross-validation and train/test splits over row indices.

<!-- docs-declaration -->

```csharp
public static class Splitters
```

**Example** — three stratified folds over twelve rows in three classes.

```csharp
using Lodestar.Preprocessing;

// Six rows of class 0, three of class 1, three of class 2.
int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

IReadOnlyList<FoldSplit> folds = Splitters.StratifiedKFold(labels, foldCount: 3);

string held = string.Join(",", folds[0].TestIndices);  // => 0,1,6,9
string fit = string.Join(",", folds[0].TrainIndices);  // => 2,3,4,5,7,8,10,11
```

**Remarks** — every fold above holds two rows of class 0 and one of each other class, which is what
stratifying buys: [`Splitters.KFold`](splitters-kfold.md) on the same twelve rows would hand fold 0
four rows of class 0 and nothing else.

**The unshuffled splitters are scikit-learn's, fold for fold and index for index**, replayed from
`tests/oracles/preprocessing_splitters.json`. The shuffled ones take the permutation as an argument
rather than a seed — the [splitting index](../splitting.md) has why, and which shuffled splits a
permutation reproduces.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`FoldSplit`](foldsplit.md), [`TrainTestSplit`](traintestsplit.md), the
[splitting index](../splitting.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`Splitters.KFold`](splitters-kfold.md) | Cuts the rows into contiguous folds. |
| [`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md) | Cuts folds that keep each class's share. |
| [`Splitters.TrainTest`](splitters-traintest.md) | Holds out the last rows as a test set. |
