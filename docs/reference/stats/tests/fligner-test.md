# Fligner.Test

Compares the spread of two or more groups, `scipy.stats.fligner`.

<!-- docs-declaration -->

```csharp
public static TestResult Test(double[][] groups)
```

<!-- docs-declaration -->

```csharp
public static TestResult Test(Center center, double proportionToCut, NanPolicy nanPolicy, double[][] groups)
```

The centre and the policy come first because the groups are a `params` array, as for
[`Levene.Test`](levene-test.md).

**Parameters** — `groups` are the samples, at least two. `center` is which centre each group's
deviations are measured from, [`Center.Median`](center.md) by default as scipy's.
`proportionToCut` is how much of each end [`Center.Trimmed`](center.md) drops, `0.05` by default.
`nanPolicy` is scipy's `nan_policy`, [`NanPolicy.Propagate`](../nanpolicy.md) by default.

**Returns** — `TestResult`: the χ² statistic and its upper-tail p-value on `k − 1` degrees of
freedom. Both are `NaN` where a group is empty, a `NaN` propagates, or every deviation ties, as
scipy answers.

**Exceptions** — `ArgumentException` when there are fewer than two groups, or `nanPolicy` is
[`NanPolicy.Raise`](../nanpolicy.md) and a group holds a `NaN`. `ArgumentOutOfRangeException`
when `proportionToCut` would trim a group away entirely.

**Example** — the three bottling machines [`Levene.Test`](levene-test.md) compares, by ranks.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

TestResult result = Fligner.Test(first, second, third);

double statistic = Math.Round(result.Statistic, 6);   // => 16.66687
double pValue = Math.Round(result.PValue, 8);         // => 0.00024035
```

**Remarks** — the deviations are ranked across every group together, and each rank `r` becomes the
normal score `Φ⁻¹(r / (2(N + 1)) + 1/2)`; the statistic compares the groups' mean scores. The
centre is the median by default, as for Levene's test, and scipy's `proportiontocut` trims by the
same truncating rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Levene.Test`](levene-test.md), [`Bartlett.Test`](bartlett-test.md),
[`Center`](center.md).
