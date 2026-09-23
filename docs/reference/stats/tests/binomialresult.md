# BinomialResult

A binomial test's result: the observed proportion and the p-value.

<!-- docs-declaration -->

```csharp
public sealed record BinomialResult(double Statistic, double PValue)
```

**Properties** — `Statistic` is the observed proportion of successes, `successes / trials`.
`PValue` is the p-value on the requested tail.

**Example** — the proportion and its interval come from one call, not two.

```csharp
using Lodestar.Stats;

BinomialResult result = Binomial.Test(7, 20, 0.5);
(double low, double high) = result.ProportionConfidenceInterval();

double estimate = Math.Round(result.Statistic, 4);   // => 0.35
double lower = Math.Round(low, 4);                   // => 0.1539
double upper = Math.Round(high, 4);                  // => 0.5922
```

The interval is wide because twenty trials is not much: the data is consistent with a coin that
comes up heads 15% of the time and with one that does so 59% of the time.

**Remarks** — alongside the two public properties, a `BinomialResult` privately carries the counts
it was given and the tail that was asked for. They are internal because a caller only reaches them
through [`ProportionConfidenceInterval`](binomialresult-proportionconfidenceinterval.md), which
needs all three: the counts set the interval and the tail decides whether it is half-open. scipy
keeps `k`, `n` and `alternative` on its own `BinomTestResult` for the same purpose.

Being a `record`, two results with the same proportion and p-value are equal — the internal fields
take part in that equality too, so two results from different trial counts are not equal even when
their two numbers coincide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Binomial.Test`](binomial-test.md), [`TestResult`](testresult.md),
[`PearsonResult`](pearsonresult.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`BinomialResult.ProportionConfidenceInterval`](binomialresult-proportionconfidenceinterval.md) | The confidence interval for the proportion. |
