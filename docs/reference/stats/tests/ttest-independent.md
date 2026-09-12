# TTest.Independent

Compares the means of two independent samples.

<!-- docs-declaration -->

```csharp
public static TTestResult Independent(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Alternative alternative = Alternative.TwoSided, Variance variance = Variance.Welch, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `a` and `b` are the two samples, each at least two values; both spans are read,
never modified. `alternative` says which tail the p-value covers. `variance` says whether to pool
the two sample variances. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`,
defaulting to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TTestResult`: the t statistic, the p-value, and the degrees of freedom, which are
fractional under `Variance.Welch`.

**Exceptions** — `ArgumentException` when either sample holds fewer than two values, or
`nanPolicy` is `NanPolicy.Raise` and either sample holds a `NaN`.

**Example** — two samples with clearly different means.

```csharp
using Lodestar.Stats;

double[] before = [102.0, 98.0, 110.0, 105.0, 99.0];
double[] after = [95.0, 92.0, 99.0, 91.0, 97.0];

TTestResult result = TTest.Independent(before, after);

bool significant = result.PValue < 0.05;   // => True
```

**Remarks — the default is not scipy's.** This defaults to `Variance.Welch`;
`scipy.stats.ttest_ind` defaults to `equal_var=True`, which is Student's test.
Pooling is only correct when the two populations really share a variance, and a
default that is wrong in the common case costs more than a word at the call
site. Pass `Variance.Equal` for scipy's default. Both are covered by
`tests/oracles/stats_ttest.json`, and the divergence has a row in the
[equivalence table](../../../equivalence.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TTest.Paired`](ttest-paired.md),
[`TTest.OneSample`](ttest-onesample.md),
[`MannWhitney.Test`](mannwhitney-test.md) for the rank-based counterpart, the
[Python equivalence table](../../../equivalence.md).
