# Splitters.StratifiedTrainTest

Holds out a share that keeps each label's proportion, as `train_test_split(stratify=y)` does.

<!-- docs-declaration -->

```csharp
public static TrainTestSplit StratifiedTrainTest(ReadOnlySpan<int> labels, double testFraction, long randomState)
```

**Parameters** — `labels` is one class label per row; every class needs at least two rows.
`testFraction` is the share to hold out, strictly inside `(0, 1)`. `randomState` is scikit-learn's
`random_state`, in `[0, 2³² − 1]`.

**Returns** — a `TrainTestSplit` of `ceil(n · testFraction)` test rows and the rest for training,
each index list ascending.

**Exceptions** — `ArgumentOutOfRangeException` when there are fewer than two rows, when
`testFraction` is outside `(0, 1)` or rounds up to every row, or when `randomState` is out of range.
`ArgumentException`, in the reference's order, when a class has a single row, or when either side
would hold fewer rows than there are classes.

**Example** — twelve rows in three classes, a quarter held out.

```csharp
using Lodestar.Preprocessing;

int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

TrainTestSplit split = Splitters.StratifiedTrainTest(labels, testFraction: 0.25, randomState: 42);

// One row of each class: 3 test rows split 6 : 3 : 3 as closely as whole rows allow.
string held = string.Join(",", split.TestIndices);   // => 3,6,11
string fit = string.Join(",", split.TrainIndices);   // => 0,1,2,4,5,7,8,9,10
```

**Remarks** — `StratifiedShuffleSplit`'s algorithm: each class's share of the training rows, then of
the test rows, from the approximate mode of the multivariate hypergeometric; then each class's rows,
ascending, read through a permutation and cut there. **Where two classes' remainders tie, the
reference picks which one gets the extra row at random**, which is why this takes the seed rather
than a permutation: no permutation a caller could pass carries that choice
([decision 0008](../../../decisions/0008-numpy-s-legacy-generator-is-replayed.md)).

There is no unseeded form. The reference refuses `stratify` with `shuffle=False`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.TrainTest`](splitters-traintest.md),
[`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md), [`TrainTestSplit`](traintestsplit.md).
