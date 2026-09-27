# OneWayAnova.Test

Compares the means of two or more groups.

<!-- docs-declaration -->

```csharp
public static TestResult Test(double[][] groups)
```

<!-- docs-declaration -->

```csharp
public static TestResult Test(NanPolicy nanPolicy, double[][] groups)
```

The policy comes first because the groups are a `params` array and C# allows no parameter after
one — the shape `string.Join` uses, for the same reason.

**Parameters** — `groups` are the samples to compare, at least two, each holding at least one
value, and at least one holding more than one — `scipy.stats.f_oneway` takes its samples the same
way, one array per group, which `groups` is `params` for. `nanPolicy` says what to do with a
`NaN`; scipy's `nan_policy`, defaulting to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the F statistic, and the upper-tail p-value.

**Exceptions** — `ArgumentException` when there are fewer than two groups, a group is empty, or
every group holds exactly one value.

**Example** — three shifts, fifteen measurements in all.

```csharp
using Lodestar.Stats;

double[] morning = [12.0, 14.0, 11.0, 13.0, 15.0];
double[] afternoon = [16.0, 15.0, 18.0, 17.0, 14.0];
double[] evening = [21.0, 19.0, 22.0, 20.0, 23.0];

TestResult result = OneWayAnova.Test(morning, afternoon, evening);

double f = Math.Round(result.Statistic, 4);   // => 32.6667
double p = Math.Round(result.PValue, 8);      // => 1.396E-05
```

**Remarks — a fully degenerate input answers `NaN`, not an exception.** Groups that are each
internally constant, and constant at the *same* value, leave no variation to compare. Constancy is
read off the values, as scipy's `f_oneway` does, not off the sums of squares: three copies of
`0.1` average to `0.10000000000000002`, which leaves the within-group sum at `1e-33` rather than
zero.

```csharp
using Lodestar.Stats;

TestResult degenerate = OneWayAnova.Test([5.0, 5.0], [5.0, 5.0]);

bool isNaN = double.IsNaN(degenerate.Statistic);   // => True
bool alsoNaN = double.IsNaN(OneWayAnova.Test([0.1, 0.1, 0.1], [0.1, 0.1, 0.1]).Statistic);   // => True
```

No variation within and none between has no ratio, and scipy's own `f_oneway` returns the same
`NaN` on the same input — the honest answer, not a guard this package chose to skip. Compare
[`KruskalWallis.Test`](kruskalwallis-test.md), which *throws* on the analogous all-tied input: an
ANOVA on constants is a well-formed question with an undefined answer, where the rank-based
statistic's inputs there are provably meaningless rather than merely indeterminate.

Groups each constant at *different* values are a perfect separation: the statistic is
`PositiveInfinity` and the p-value an exact `0`, as scipy's.

```csharp
using Lodestar.Stats;

TestResult separated = OneWayAnova.Test([0.1, 0.1, 0.1], [0.2, 0.2, 0.2]);

bool infinite = double.IsPositiveInfinity(separated.Statistic);   // => True
double p = separated.PValue;                                      // => 0
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KruskalWallis.Test`](kruskalwallis-test.md) for the rank-based counterpart,
[`TTest.Independent`](ttest-independent.md), [`MultipleComparisons`](multiplecomparisons.md) for
correcting the many pairwise tests an ANOVA's rejection invites, the
[Python equivalence table](../../../equivalence.md).
