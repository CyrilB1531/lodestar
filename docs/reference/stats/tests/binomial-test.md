# Binomial.Test

Tests a count of successes against a stated probability.

<!-- docs-declaration -->

```csharp
public static BinomialResult Test(int successes, int trials, double probability = 0.5, Alternative alternative = Alternative.TwoSided)
```

**Parameters** — `successes` is the count observed, between zero and `trials`. `trials` is how
many there were, at least one. `probability` is the probability of success under the null, in
`[0, 1]`, defaulting to `0.5` as `scipy.stats.binomtest` does. `alternative` says which tail the
p-value covers.

**Returns** — `BinomialResult`: the observed proportion, the p-value, and a
[`ProportionConfidenceInterval`](binomialresult-proportionconfidenceinterval.md) the result can be
asked for afterwards.

**Exceptions** — `ArgumentOutOfRangeException` when `trials` is not positive, `successes` lies
outside `[0, trials]`, or `probability` lies outside `[0, 1]`.

**Example** — seven heads in twenty tosses of a coin assumed fair.

```csharp
using Lodestar.Stats;

BinomialResult result = Binomial.Test(successes: 7, trials: 20, probability: 0.5);

double proportion = Math.Round(result.Statistic, 4);   // => 0.35
double p = Math.Round(result.PValue, 6);               // => 0.263176
```

Seven of twenty is well inside what a fair coin produces; nothing here is evidence against it.

**Remarks — exact means exact at any size, which is the whole reason this exists beside
[`ChiSquare.GoodnessOfFit`](chisquare-goodnessoffit.md).** The p-value is a sum of binomial
probabilities rather than a chi-squared approximation of one, so it stays right where the
approximation is worst: few trials, or a probability near zero or one. The cost is that the
two-sided p-value has to decide which outcomes on the far side are "at least as extreme", and two
outcomes whose probabilities differ only in the last bits are counted as equally extreme — the
same `1 + 1e-7` relative margin [`FisherExact.Test`](fisherexact-test.md) uses, and scipy's own.

**The two-sided p-value is not twice the one-sided one.** The binomial is discrete and, unless
`probability` is `0.5`, asymmetric; doubling a tail would be wrong in both directions at once.

```csharp
using Lodestar.Stats;

BinomialResult twoSided = Binomial.Test(7, 20, 0.5);
BinomialResult greater = Binomial.Test(7, 20, 0.5, Alternative.Greater);

double both = Math.Round(twoSided.PValue, 6);    // => 0.263176
double upper = Math.Round(greater.PValue, 6);    // => 0.942341
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BinomialResult`](binomialresult.md),
[`ChiSquare.GoodnessOfFit`](chisquare-goodnessoffit.md),
[`FisherExact.Test`](fisherexact-test.md), the [Python equivalence table](../../../equivalence.md).
