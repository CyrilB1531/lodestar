# LogRank.MultiGroup

Compares the survival of several groups at once: lifelines' `multivariate_logrank_test`.

<!-- docs-declaration -->

```csharp
public static LogRankResult MultiGroup(ReadOnlySpan<double> durations, ReadOnlySpan<int> groups, ReadOnlySpan<bool> eventObserved, LogRankOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static LogRankResult MultiGroup(ReadOnlySpan<double> durations, ReadOnlySpan<int> groups, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, LogRankOptions options = null)
```

The second overload weighs each subject, lifelines' `weights=`.

**Parameters** — `durations` holds one non-negative duration per subject and `eventObserved` its
flag. `groups` holds one label per subject, any integers, at least two distinct. `weights` is one
positive, finite weight per subject, or empty for ones. `options` chooses the weighting, its
exponents and the truncation; `null` runs the log-rank test.

**Returns** — a [`LogRankResult`](logrankresult.md) on one degree of freedom fewer than the groups.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, a
duration is negative or `NaN`, fewer than two groups appear, or `weights` is neither empty nor one
positive, finite value per subject. `ArgumentOutOfRangeException` when `options` names no
weighting, or holds a negative or non-finite exponent, or a negative or `NaN` truncation.

**Example** — three groups that do not differ.

```csharp
using Lodestar.Survival;

double[] durations = [5, 8, 12, 3, 9, 15, 4, 6, 20, 11, 7, 14];
int[] groups = [1, 1, 1, 2, 2, 2, 3, 3, 3, 1, 2, 3];
bool[] observed = [true, true, false, true, true, true, true, true, false, true, false, true];

LogRankResult result = LogRank.MultiGroup(durations, groups, observed);

double statistic = Math.Round(result.Statistic, 6);   // => 0.159352
double p = Math.Round(result.PValue, 6);              // => 0.923415
int df = result.DegreesOfFreedom;                     // => 2
```

**Remarks** — at each distinct duration every group's observed events are set against the share of
the pooled events its risk set would carry, and the statistic is that difference vector over its
hypergeometric covariance, for every group but the last. **The covariance is read through a
pseudo-inverse**, as lifelines reads it through `numpy.linalg.pinv`, so a group nobody is at risk in
by the first event leaves the others' comparison standing rather than failing. Which group is left
out does not change the statistic.

**A weight of two is the subject written twice**, in every risk set and every event count — except in
the pooled curve Fleming-Harrington weighs by, which lifelines builds from the subjects unweighted,
and so does this. A time whose pooled risk set has fallen below one, possible only with fractional
weights, adds nothing to the covariance, as in lifelines.

Reference behaviour is `lifelines.statistics.multivariate_logrank_test` 0.30.3, matched with and
without weights under every weighting.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank.Pairwise`](logrank-pairwise.md), [`LogRank.Test`](logrank-test.md),
[`LogRankOptions`](logrankoptions.md).
