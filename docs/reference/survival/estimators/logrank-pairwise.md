# LogRank.Pairwise

Runs the two-sample test on every pair of groups: lifelines' `pairwise_logrank_test`.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<PairwiseLogRankResult> Pairwise(ReadOnlySpan<double> durations, ReadOnlySpan<int> groups, ReadOnlySpan<bool> eventObserved, LogRankOptions options = null)
```

**Parameters** — `durations` holds one non-negative duration per subject and `eventObserved` its
flag. `groups` holds one label per subject, any integers, at least two distinct. `options` chooses
the weighting, its exponents and the truncation; `null` runs the log-rank test.

**Returns** — one [`PairwiseLogRankResult`](pairwiselogrankresult.md) per pair of labels, the labels
ascending and each pair lower label first: lifelines' row order.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, a
duration is negative or `NaN`, or fewer than two groups appear. `ArgumentOutOfRangeException` when
`options` names no weighting, or holds a negative or non-finite exponent, or a negative or `NaN`
truncation.

**Example** — the three groups of the multi-group page, two at a time.

```csharp
using Lodestar.Survival;

double[] durations = [5, 8, 12, 3, 9, 15, 4, 6, 20, 11, 7, 14];
int[] groups = [1, 1, 1, 2, 2, 2, 3, 3, 3, 1, 2, 3];
bool[] observed = [true, true, false, true, true, true, true, true, false, true, false, true];

IReadOnlyList<PairwiseLogRankResult> pairs = LogRank.Pairwise(durations, groups, observed);

int count = pairs.Count;                                    // => 3
int second = pairs[1].GroupB;                               // => 3
double statistic = Math.Round(pairs[1].Result.Statistic, 6);   // => 0.087424
```

**Remarks** — each pair is tested on its own two groups alone: the other groups are out of its risk
sets and, under Fleming-Harrington, out of its pooled curve. **No correction for multiple comparisons
is applied**, as lifelines applies none; with many groups, scale the p-values before reading them.
Subject weights are not taken, as lifelines' pairwise test takes none.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank.MultiGroup`](logrank-multigroup.md), [`PairwiseLogRankResult`](pairwiselogrankresult.md).
