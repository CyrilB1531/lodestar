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

**Every splitter is scikit-learn's, fold for fold and index for index**, replayed from
`tests/oracles/preprocessing_splitters.json` and `preprocessing_splitters_seeded.json`. A shuffled one
takes scikit-learn's own `random_state`, or a permutation — the [splitting index](../splitting.md)
has why both.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`FoldSplit`](foldsplit.md), [`TrainTestSplit`](traintestsplit.md), the
[splitting index](../splitting.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`Splitters.KFold`](splitters-kfold.md) | Cuts the rows into contiguous folds. |
| [`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md) | Cuts folds that keep each class's share. |
| [`Splitters.TrainTest`](splitters-traintest.md) | Holds out the last rows as a test set, or a shuffled share. |
| [`Splitters.StratifiedTrainTest`](splitters-stratifiedtraintest.md) | Holds out a shuffled share that keeps each class's proportion. |
| [`Splitters.GroupKFold`](splitters-groupkfold.md) | Cuts folds that never split a group. |
| [`Splitters.StratifiedGroupKFold`](splitters-stratifiedgroupkfold.md) | Cuts group folds that keep each class's share. |
| [`Splitters.TimeSeries`](splitters-timeseries.md) | Cuts forward-chaining splits over time-ordered rows. |
| [`Splitters.RepeatedKFold`](splitters-repeatedkfold.md) | Repeats shuffled k-fold. |
| [`Splitters.RepeatedStratifiedKFold`](splitters-repeatedstratifiedkfold.md) | Repeats shuffled stratified k-fold. |
