# ProportionInterval

Which interval a binomial proportion is reported with.

<!-- docs-declaration -->

```csharp
public enum ProportionInterval { Exact, Wilson, WilsonCorrected }
```

**Members** — `Exact` is Clopper-Pearson, inverted from the binomial tails; scipy's `'exact'`, and
the default on both sides. `Wilson` is Wilson's score interval, scipy's `'wilson'`.
`WilsonCorrected` is Wilson's with the half-unit continuity correction, scipy's `'wilsoncc'`.

**Example** — the three on the same seven of twenty, widest last.

```csharp
using Lodestar.Stats;

BinomialResult result = Binomial.Test(7, 20, 0.5);

double exact = Math.Round(result.ProportionConfidenceInterval().Low, 4);
double wilson = Math.Round(
    result.ProportionConfidenceInterval(0.95, ProportionInterval.Wilson).Low, 4);

double clopperPearson = exact;   // => 0.1539
double score = wilson;           // => 0.1812
```

**Remarks — the three differ only where the question is hard, which is small samples.** At a
thousand trials they agree to three decimals and the choice does not matter. At twenty they
disagree by two percentage points, and at five they disagree by more than the estimate is worth.
Clopper-Pearson is conservative by construction — it inverts a discrete distribution, so its
coverage is at least the level and usually above it. Wilson is calibrated on average and can dip
below. The continuity correction pushes Wilson back toward Clopper-Pearson, buying coverage at the
cost of width.

**Applies to** — net10.0, netstandard2.0.

**See also** —
[`BinomialResult.ProportionConfidenceInterval`](binomialresult-proportionconfidenceinterval.md),
[`Binomial.Test`](binomial-test.md), the [Python equivalence table](../../../equivalence.md).
