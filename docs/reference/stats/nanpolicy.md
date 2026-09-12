# NanPolicy

What a test does with a `NaN` in its input; scipy's `nan_policy`.

<!-- docs-declaration -->

```csharp
public enum NanPolicy
```

**Fields** — `Propagate` carries the `NaN` into the statistic and the p-value, which is scipy's
default and this package's behaviour before the parameter existed. `Raise` refuses the input with
`ArgumentException`. `Omit` drops the missing observations and tests on the rest.

**Example** — the same sample under two policies.

```csharp
using Lodestar.Stats;

double[] sample = [1.0, 2.0, double.NaN, 4.0, 5.0, 6.0, 7.0];

TestResult carried = ShapiroWilk.Test(sample);
bool isNaN = double.IsNaN(carried.Statistic);  // => True

TestResult dropped = ShapiroWilk.Test(sample, NanPolicy.Omit);
bool isReal = double.IsNaN(dropped.Statistic);  // => False
```

**Remarks** — offered on the eleven entry points whose scipy counterpart takes `nan_policy`, and
on no others: `chi2_contingency`, `fisher_exact` and `false_discovery_control` do not take it, so
`ChiSquare.Contingency`, `FisherExact.Test` and the `MultipleComparisons` methods do not either.

`Omit` drops **pairs** where the two inputs are aligned — `TTest.Paired`, `Wilcoxon.Paired`, and
`ChiSquare.GoodnessOfFit` when an expectation is given — and values everywhere else. Dropping each
sample independently would silently change what a paired test is testing.

Omission is a filter, not a second policy: a family's own guards run afterwards on what survives,
so `ShapiroWilk.Test` still refuses fewer than three observations and `KruskalWallis.Test` still
refuses a fully tied pool. Decision 0116 has the reasoning.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ShapiroWilk.Test`](tests/shapirowilk-test.md), [`TTest.Paired`](tests/ttest-paired.md).
