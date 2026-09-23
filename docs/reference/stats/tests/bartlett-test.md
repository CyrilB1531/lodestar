# Bartlett.Test

Compares the variances of two or more groups.

<!-- docs-declaration -->

```csharp
public static TestResult Test(double[][] groups)
```

<!-- docs-declaration -->

```csharp
public static TestResult Test(NanPolicy nanPolicy, double[][] groups)
```

The policy comes first because the groups are a `params` array and C# allows no parameter after
one.

**Parameters** — `groups` are the samples to compare, at least two, each holding at least two
values, one array per group as `scipy.stats.bartlett` takes them. `nanPolicy` says what to do with
a `NaN`; scipy's `nan_policy`, defaulting to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the T statistic, and the upper-tail p-value.

**Exceptions** — `ArgumentException` when there are fewer than two groups, a group holds fewer
than two values — it would have no variance for the statistic's logarithm to read — or
`nanPolicy` is [`NanPolicy.Raise`](../nanpolicy.md) and a group holds a `NaN`.

**Example** — the same three machines [`Levene.Test`](levene-test.md) measures.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

TestResult result = Bartlett.Test(first, second, third);

double t = Math.Round(result.Statistic, 4);   // => 27.6477
double p = Math.Round(result.PValue, 10);     // => 9.917E-07
```

**Remarks — which of the two variance tests to reach for is a question about the data, not about
precision.** Bartlett assumes each group is normal and is the more powerful test when that holds.
It is also sensitive to departures from normality in a way that is indistinguishable from a
difference in variance, so on a skewed or heavy-tailed sample it reports a difference that is not
there. [`Levene.Test`](levene-test.md) around the median assumes nothing about the shape and is
the safer default; this one is the sharper instrument once normality has itself been checked, with
[`AndersonDarling.Test`](andersondarling-test.md) or [`ShapiroWilk.Test`](shapirowilk-test.md).

**The statistic is floored at zero after its p-value is taken, not before.** Variances that agree
to the last bits can drive the numerator a shade negative; scipy reports the floor together with
the tail the unfloored value produced, and so does this.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Levene.Test`](levene-test.md), [`OneWayAnova.Test`](onewayanova-test.md),
[`AndersonDarling.Test`](andersondarling-test.md), the
[Python equivalence table](../../../equivalence.md).
