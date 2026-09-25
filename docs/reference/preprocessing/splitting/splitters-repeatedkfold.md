# Splitters.RepeatedKFold

Repeats shuffled k-fold, as `RepeatedKFold` does.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> RepeatedKFold(int sampleCount, int foldCount, int repeatCount, long randomState)
```

**Parameters** — `sampleCount` is how many rows there are. `foldCount` is how many folds each repeat
cuts. `repeatCount` is how many repeats, at least one. `randomState` is scikit-learn's
`random_state`, in `[0, 2³² − 1]`.

**Returns** — `repeatCount · foldCount` folds, repeat by repeat, each index list ascending — the
order the reference yields them in.

**Exceptions** — `ArgumentOutOfRangeException` when a count or `randomState` is out of range.

**Example** — six rows, three folds, two repeats.

```csharp
using Lodestar.Preprocessing;

IReadOnlyList<FoldSplit> folds = Splitters.RepeatedKFold(sampleCount: 6, foldCount: 3, repeatCount: 2, randomState: 0);

int count = folds.Count;                                            // => 6
string firstRepeat = string.Join(",", folds[0].TestIndices);        // => 2,5
string secondRepeat = string.Join(",", folds[3].TestIndices);       // => 1,3
```

**Remarks** — one generator serves every repeat, so repeat `r` reads the rows in the generator's
`r`-th permutation, exactly as the reference hands one `RandomState` to each `KFold` it builds.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.KFold`](splitters-kfold.md),
[`Splitters.RepeatedStratifiedKFold`](splitters-repeatedstratifiedkfold.md).
