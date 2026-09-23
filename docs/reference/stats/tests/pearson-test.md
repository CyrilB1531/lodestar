# Pearson.Test

Correlates two paired samples.

<!-- docs-declaration -->

```csharp
public static PearsonResult Test(ReadOnlySpan<double> x, ReadOnlySpan<double> y, Alternative alternative = Alternative.TwoSided, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `x` and `y` are the two samples, of the same length and at least two values;
both spans are read, never modified. `alternative` says which tail the p-value covers.
`nanPolicy` says what to do with a `NaN`, defaulting to [`NanPolicy.Propagate`](../nanpolicy.md);
[`NanPolicy.Omit`](../nanpolicy.md) drops the pair, not the value, so the two samples stay
aligned.

**Returns** — `PearsonResult`: the coefficient in `[-1, 1]`, the p-value, and a
[`ConfidenceInterval`](pearsonresult-confidenceinterval.md) the result can be asked for
afterwards. A sample that is constant has no correlation defined, and both numbers are `NaN`.

**Exceptions** — `ArgumentException` when the samples differ in length, when either holds fewer
than two values, or when `nanPolicy` is [`NanPolicy.Raise`](../nanpolicy.md) and either sample
holds a `NaN`.

**Example** — eight tasting panels: how much sugar the recipe carried, against the sweetness
score the panel gave it.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

PearsonResult result = Pearson.Test(sugar, sweetness);

double r = Math.Round(result.Statistic, 4);   // => 0.9778
double p = Math.Round(result.PValue, 6);      // => 2.7E-05
```

**Remarks — it measures a line, and a monotone relationship that is not one reads lower.** The
dose below rises with the effect at every step, so both rank tests answer exactly `1`; the last
dose is five times the one before it, and a coefficient that measures distances rather than
order reports two thirds of a relationship that is in fact perfect.

```csharp
using Lodestar.Stats;

double[] dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 40.0];
double[] effect = [2.0, 3.5, 4.0, 5.5, 6.0, 7.5, 8.0, 9.0];

double linear = Math.Round(Pearson.Test(dose, effect).Statistic, 4);      // => 0.6749
double monotone = Math.Round(Spearman.Test(dose, effect).Statistic, 4);   // => 1
```

Neither answer is wrong: they are answers to different questions, and the choice between them is
the choice of which question was being asked. The [hypothesis-testing
guide](../../../guides/hypothesis-testing.md) has the longer version.

**The p-value comes from a beta distribution, not from a Student *t*.** Under the null, `r` is
distributed as a beta on `[-1, 1]` with equal shape parameters `n/2 - 1`, and that is what is
evaluated. The algebraically equal `t = r · sqrt((n-2)/(1-r²))` form agrees to every digit that
matters in the middle of the range and loses them in the far tail, where `1 - r²` cancels — which
is exactly where the frozen corpus reaches.

**Two pairs answer `1` whatever the data.** With two observations `r` can only be `+1` or `-1`,
so the two-sided p-value is `1`; scipy documents the same limit rather than evaluating a density
that is not defined there.

**Unlike `scipy.stats.pearsonr`, this takes a `nanPolicy`.** Scipy's `pearsonr` is the one
correlation function left without the parameter, and its own pull request says why: the result
object it returns stores `x` and `y`, which complicated the change. `PearsonResult` stores
neither. The default reproduces scipy exactly — a `NaN` in either sample gives `(NaN, NaN)` — and
the other two settings are reachable only by writing something scipy has no syntax for. The
[Python equivalence table](../../../equivalence.md) carries the row.

```csharp
using Lodestar.Stats;

double[] sugar = [4.0, 6.5, double.NaN, 9.0, 7.5, 3.0, 8.0, 6.0];
double[] sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

PearsonResult omitted = Pearson.Test(sugar, sweetness, nanPolicy: NanPolicy.Omit);

double r = Math.Round(omitted.Statistic, 4);   // => 0.9767
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Spearman.Test`](spearman-test.md), [`KendallTau.Test`](kendalltau-test.md),
[`PearsonResult`](pearsonresult.md), the [Python equivalence table](../../../equivalence.md).
