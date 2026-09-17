# Splitters.TrainTest

Holds out the last rows, as `train_test_split(shuffle=False)` does.

<!-- docs-declaration -->

```csharp
public static TrainTestSplit TrainTest(int sampleCount, double testFraction)
public static TrainTestSplit TrainTest(int sampleCount, double testFraction, ReadOnlySpan<int> order)
```

**Parameters** — `sampleCount` is how many rows there are, at least two. `testFraction` is the share
to hold out, strictly inside `(0, 1)`. `order` is a permutation of the rows whose first
`ceil(sampleCount · testFraction)` entries are held out; an empty span holds out the last rows instead.

**Returns** — a `TrainTestSplit` carrying the training and test indices, each ascending.

**Exceptions** — `ArgumentOutOfRangeException` when `sampleCount` is below two, when `testFraction`
is `NaN` or outside `(0, 1)`, or when the fraction rounds up to every row and leaves nothing to fit
on. `ArgumentException` when `order` is neither empty nor a permutation of the rows.

**Example** — a quarter of ten rows, then the same fraction read in another order.

```csharp
using Lodestar.Preprocessing;

TrainTestSplit split = Splitters.TrainTest(sampleCount: 10, testFraction: 0.25);

// ceil(10 × 0.25) = 3, and they are the last three rows.
string test = string.Join(",", split.TestIndices);    // => 7,8,9
string train = string.Join(",", split.TrainIndices);  // => 0,1,2,3,4,5,6

int[] order = [9, 4, 1, 7, 0, 3, 6, 8, 2, 5];
// With an order the test rows are its first three, as ShuffleSplit takes permutation[:n_test].
string held = string.Join(",", Splitters.TrainTest(10, 0.25, order).TestIndices);  // => 1,4,9
```

**Remarks** — the held-out count is `ceil(sampleCount · testFraction)`, which is the reference's
rounding; at `n = 10` and `0.25` it is three rows, not two.

**The order's head is held out, the unshuffled split's tail.** That asymmetry is the reference's own:
`train_test_split(shuffle=False)` tests on the last rows, while `ShuffleSplit` — which
`train_test_split` calls when it shuffles — tests on `permutation[:n_test]`. So passing the
permutation scikit-learn drew reproduces its split, and the identity order holds out the **first**
rows, not the same rows as the two-argument overload.

**Stratification is not offered here.** The reference refuses `stratify` with `shuffle=False`
outright, and with a caller-supplied permutation the honest version is
[`Splitters.StratifiedKFold`](splitters-stratifiedkfold.md) with `foldCount = round(1 / testFraction)`,
taking one fold as the test set: at a quarter that is four folds, and fold 0 is the held-out rows.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TrainTestSplit`](traintestsplit.md), [`Splitters.KFold`](splitters-kfold.md).
