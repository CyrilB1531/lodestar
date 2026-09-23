# Spearman.Test

Correlates the ranks of two paired samples.

<!-- docs-declaration -->

```csharp
public static TestResult Test(ReadOnlySpan<double> x, ReadOnlySpan<double> y, Alternative alternative = Alternative.TwoSided, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `x` and `y` are the two samples, of the same length; both spans are read, never
modified. `alternative` says which tail the p-value covers. `nanPolicy` says what to do with a
`NaN`; scipy's `nan_policy`, defaulting to [`NanPolicy.Propagate`](../nanpolicy.md), and
[`NanPolicy.Omit`](../nanpolicy.md) drops the pair rather than the value.

**Returns** — `TestResult`: rho, and the p-value. Fewer than two pairs, or a constant sample,
leaves the correlation undefined and both numbers are `NaN`; so is the p-value alone at exactly
two pairs, where the Student distribution has no degrees of freedom left.

**Exceptions** — `ArgumentException` when the samples differ in length, or when `nanPolicy` is
[`NanPolicy.Raise`](../nanpolicy.md) and either sample holds a `NaN`.

**Example** — the same eight tasting panels [`Pearson.Test`](pearson-test.md) measures.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

TestResult result = Spearman.Test(sugar, sweetness);

double rho = Math.Round(result.Statistic, 4);   // => 0.9762
double p = Math.Round(result.PValue, 6);        // => 3.3E-05
```

**Remarks — ties take the mid-rank, as `scipy.stats.rankdata` gives them.** Six values of which
two are equal occupy ranks 1 to 6, and the tied pair takes the mean of the two places it spans
rather than an arbitrary one of them. That is what keeps rho symmetric under a relabelling of
equal observations.

**There is no exact branch, because scipy has none.** `spearmanr` approximates with
`t = rho · sqrt((n-2)/((1+rho)(1-rho)))` on `n - 2` degrees of freedom at every sample size, and
so does this; a parameter with one legal value would be an API with nothing behind it.
[`KendallTau.Test`](kendalltau-test.md) is the one of the three that does enumerate, and is the
better reading on a short untied sample for exactly that reason.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Pearson.Test`](pearson-test.md), [`KendallTau.Test`](kendalltau-test.md), the
[Python equivalence table](../../../equivalence.md).
