# KruskalWallis.Test

Compares two or more groups by their ranks in the pooled sample.

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

**Parameters** — `groups` are the samples to compare, at least two — `scipy.stats.kruskal` takes its samples the same way, one array per group, which
`groups` is `params` for. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`,
defaulting to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the H statistic, and the upper-tail p-value.

**Exceptions** — `ArgumentException` when there are fewer than two groups. A fully tied pooled
sample, and a null or empty group, given or left by omission, answer `NaN` for both. `ArgumentOutOfRangeException` when an option is outside its enumeration.

**Example** — the same three shifts [`OneWayAnova.Test`](onewayanova-test.md) compares.

```csharp
using Lodestar.Stats;

double[] morning = [12.0, 14.0, 11.0, 13.0, 15.0];
double[] afternoon = [16.0, 15.0, 18.0, 17.0, 14.0];
double[] evening = [21.0, 19.0, 22.0, 20.0, 23.0];

TestResult result = KruskalWallis.Test(morning, afternoon, evening);

double h = Math.Round(result.Statistic, 4);   // => 11.6215
double p = Math.Round(result.PValue, 6);      // => 0.002995
```

**Remarks — a fully tied pooled sample answers `NaN`, as
[`OneWayAnova.Test`](onewayanova-test.md)'s analogous input does.**

```csharp
using Lodestar.Stats;

TestResult tied = KruskalWallis.Test([5.0, 5.0], [5.0, 5.0]);

bool undefined = double.IsNaN(tied.Statistic);   // => True
```

The tie correction this statistic divides by is `1 - (t³ - t) / (n³ - n)` for a tie group
spanning `t` of the `n` pooled values; when every value is tied, `t = n` and the correction is
exactly `0`, so H is `0/0`. Kruskal and Wallis leave that case undefined, and the answer is `NaN`
([decision 0010](../../../decisions/0010-each-algorithm-follows-its-defining-document.md)), where
this used to throw.

**H can round a hair below zero, and the p-value is then `NaN`.** When the groups' mean ranks are
equal, H is zero in exact arithmetic, and rounding can leave it at `-5.7e-14` — the values `1` to
`148` split into two groups with equal rank sums do. The chi-squared tail is undefined below zero,
so the p-value is `NaN`, as scipy's `kruskal` answers on the same split, and the statistic is
returned as computed.

**Under `NanPolicy.Propagate`, a NaN reaches the statistic and the p-value.** The check runs
before ranking — unguarded, `Array.Sort` sorts a NaN to the front and it would take a finite rank
like any other value, the same failure mode [`MannWhitney.Test`](mannwhitney-test.md) shares and
guards against the same way.

Omission runs before this test's own requirements, which are unchanged by it — a pool left fully
tied, or a group left empty, answers exactly as it would if passed directly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneWayAnova.Test`](onewayanova-test.md) for the parametric counterpart,
[`MannWhitney.Test`](mannwhitney-test.md), the [Python equivalence table](../../../equivalence.md).
