# Splitters.TimeSeries

Cuts forward-chaining splits over time-ordered rows, as `TimeSeriesSplit` does.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<FoldSplit> TimeSeries(int sampleCount, int splitCount, int? testSize = null, int gap = 0, int? maxTrainSize = null)
```

**Parameters** — `sampleCount` is how many rows there are, oldest first. `splitCount` is scikit-learn's
`n_splits`, at least two. `testSize` is the rows in each test block; `null` is
`sampleCount / (splitCount + 1)`, the reference's default. `gap` is how many rows to drop between a
training block and its test block; a negative gap overlaps them, as the reference lets it.
`maxTrainSize` keeps only the most recent rows of each training block; `null` or `0` keeps them all,
since the reference reads `max_train_size=0` as no cap.

**Returns** — one `FoldSplit` per split, oldest first, each index list ascending.

**Exceptions** — `ArgumentOutOfRangeException` when `splitCount` is below two, `testSize` below
one, `maxTrainSize` negative, or `splitCount + 1` above `sampleCount`. The negative cap is a
refusal the reference does not make: it slices one into an empty training block without a word.
`ArgumentException` when the test blocks and the gap leave the first split no training row, the
reference's own refusal, or when the test blocks need more rows than there are — which only a
negative gap lets past that check, and which the reference answers with an empty test fold.

**Example** — ten rows in three splits, then with a gap and a capped training window.

```csharp
using Lodestar.Preprocessing;

IReadOnlyList<FoldSplit> splits = Splitters.TimeSeries(sampleCount: 10, splitCount: 3);
string firstTrain = string.Join(",", splits[0].TrainIndices);  // => 0,1,2,3
string firstTest = string.Join(",", splits[0].TestIndices);    // => 4,5
string lastTrain = string.Join(",", splits[2].TrainIndices);   // => 0,1,2,3,4,5,6,7

IReadOnlyList<FoldSplit> windowed = Splitters.TimeSeries(10, 3, testSize: 2, gap: 1, maxTrainSize: 3);
string window = string.Join(",", windowed[1].TrainIndices);    // => 2,3,4
string held = string.Join(",", windowed[1].TestIndices);       // => 6,7
```

**Remarks** — the test blocks are the last `splitCount · testSize` rows, cut in order, so any rows
the division leaves over go to the first training block. Each split trains on everything before
its block less `gap` rows, cut to the last `maxTrainSize` of them. With a gap of zero or more, no
row a split trains on comes after a row it is tested on.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Splitters.KFold`](splitters-kfold.md), [`FoldSplit`](foldsplit.md).
