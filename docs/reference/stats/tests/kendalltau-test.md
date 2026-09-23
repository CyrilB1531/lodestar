# KendallTau.Test

Correlates two paired samples by counting concordant and discordant pairs.

<!-- docs-declaration -->

```csharp
public static TestResult Test(ReadOnlySpan<double> x, ReadOnlySpan<double> y, Alternative alternative = Alternative.TwoSided, KendallVariant variant = KendallVariant.TauB, ExactMethod method = ExactMethod.Auto, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `x` and `y` are the two samples, of the same length; both spans are read, never
modified. `alternative` says which tail the p-value covers. `variant` chooses which normalisation
the statistic carries, and changes nothing else — see [`KendallVariant`](kendallvariant.md).
`method` chooses the exact null distribution, its normal approximation, or a choice between them
by sample size and ties. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`,
defaulting to [`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: tau, and the p-value. Fewer than two pairs, or a sample in which
every value is tied, leaves tau undefined and both numbers are `NaN`.

**Exceptions** — `ArgumentException` when the samples differ in length, when `method` is
[`ExactMethod.Exact`](exactmethod.md) and either sample holds a tie, or when `nanPolicy` is
[`NanPolicy.Raise`](../nanpolicy.md) and either sample holds a `NaN`.
`ArgumentOutOfRangeException` when `method` is [`ExactMethod.Exact`](exactmethod.md) and the null
distribution's table would exceed twenty million cells.

**Example** — the same eight tasting panels the other two measure.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

TestResult result = KendallTau.Test(sugar, sweetness);

double tau = Math.Round(result.Statistic, 4);   // => 0.9286
double p = Math.Round(result.PValue, 6);        // => 0.000397
```

**Remarks — the exact method is refused on a tied sample, where
[`MannWhitney.Test`](mannwhitney-test.md) answers.** The two families differ because their
references do: `mannwhitneyu` builds its table and reads it whatever the data, while
`kendalltau` raises `ValueError: Ties found; exact method cannot be used.` Each is matched
rather than reconciled, and the [Python equivalence table](../../../equivalence.md) carries both
rows so the difference is met in the mapping rather than in a stack trace.

```csharp
using Lodestar.Stats;

double[] judgeA = [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0];
double[] judgeB = [1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0, 5.0];

string message = "nothing was thrown";
try
{
    KendallTau.Test(judgeA, judgeB, method: ExactMethod.Exact);
}
catch (ArgumentException error)
{
    message = error.Message;
}

string what = message;   // => The exact Kendall distribution is defined over untied…
```

[`ExactMethod.Auto`](exactmethod.md) never throws for it: a tie sends it to the asymptotic route,
and so does a table past the twenty-million-cell ceiling, because nothing the caller wrote asked
for an exact answer. Untied, `Auto` is exact up to and including thirty-three observations, or
whenever one tail holds at most a single pair — the two routes disagree on the number, which is
why the choice is a parameter rather than an optimisation.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

TestResult auto = KendallTau.Test(sugar, sweetness);
TestResult asymptotic = KendallTau.Test(sugar, sweetness, method: ExactMethod.Asymptotic);

double exactP = Math.Round(auto.PValue, 6);             // => 0.000397
double approximateP = Math.Round(asymptotic.PValue, 6); // => 0.001297
```

**The discordant pairs are counted in `O(n log n)`, not by the double loop the definition
suggests.** Ordering the pairs by the first sample turns the count into an inversion count of the
second, which a merge sort answers; at ten thousand pairs that is a hundred and thirty thousand
comparisons against the definition's fifty million. scipy reaches the same order through a
Fenwick tree.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Pearson.Test`](pearson-test.md), [`Spearman.Test`](spearman-test.md),
[`KendallVariant`](kendallvariant.md), the [Python equivalence table](../../../equivalence.md).
