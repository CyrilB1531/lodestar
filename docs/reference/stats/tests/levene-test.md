# Levene.Test

Compares the spread of two or more groups.

<!-- docs-declaration -->

```csharp
public static TestResult Test(double[][] groups)
```

<!-- docs-declaration -->

```csharp
public static TestResult Test(Center center, double proportionToCut, NanPolicy nanPolicy, double[][] groups)
```

The centre and the policy come first because the groups are a `params` array and C# allows no
parameter after one — the shape [`OneWayAnova.Test`](onewayanova-test.md) uses, for the same
reason.

**Parameters** — `groups` are the samples to compare, at least two, each holding at least one
value; `scipy.stats.levene` takes its samples the same way, one array per group, which `groups`
is `params` for. `center` says which centre each group's deviations are measured from, defaulting
to [`Center.Median`](center.md) as scipy's does. `proportionToCut` is how much of each end
[`Center.Trimmed`](center.md) drops — scipy's `proportiontocut`, `0.05` by default and ignored by
the other two centres. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`, defaulting
to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the W statistic, and the upper-tail p-value.

**Exceptions** — `ArgumentException` when there are fewer than two groups, a group is empty, or
`nanPolicy` is [`NanPolicy.Raise`](../nanpolicy.md) and a group holds a `NaN`.
`ArgumentOutOfRangeException` when `proportionToCut` would trim a group away entirely.

**Example — three machines filling the same bottle, and the reason this test exists.** One-way
ANOVA says their means are indistinguishable. It is entitled to say that only if their variances
match, and they do not: the second machine is all over the place.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

double meansAgree = Math.Round(OneWayAnova.Test(first, second, third).PValue, 4);  // => 0.9654
double spreadsDiffer = Math.Round(Levene.Test(first, second, third).PValue, 10);   // => 2.4E-08
```

A reader who ran only the ANOVA would report that the three machines agree. They agree on where
they aim and disagree on how well they hold it, which for a bottling line is the whole question —
and it is the assumption the ANOVA's own p-value rests on.

**Remarks — the default centre is the median, which is Brown and Forsythe's test rather than
Levene's own.** scipy defaults the same way and for the same reason: centring on the mean lets one
outlier inflate every deviation in its group, and the median does not notice it. The two give
different statistics on the same data.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

double median = Math.Round(Levene.Test(first, second, third).Statistic, 4);
double mean = Math.Round(
    Levene.Test(Center.Mean, 0.05, NanPolicy.Propagate, first, second, third).Statistic, 4);

double brownForsythe = median;   // => 45.3383
double levenesOwn = mean;        // => 46.7262
```

**[`Center.Trimmed`](center.md) drops `int(n × proportionToCut)` values from each end**, which is
`scipy.stats.trim_mean`'s rule and truncates rather than rounds. At five values and the `0.05`
default that is zero values, so `Trimmed` and [`Center.Mean`](center.md) agree exactly — scipy's
behaviour, not an approximation of it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bartlett.Test`](bartlett-test.md) for the same question under normality,
[`OneWayAnova.Test`](onewayanova-test.md), [`Center`](center.md), the
[Python equivalence table](../../../equivalence.md).
