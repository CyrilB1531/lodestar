# BinomialResult.ProportionConfidenceInterval

The confidence interval for the proportion.

<!-- docs-declaration -->

```csharp
public (double Low, double High) ProportionConfidenceInterval(double level = 0.95, ProportionInterval method = ProportionInterval.Exact)
```

**Parameters** — `level` is the confidence level, strictly between 0 and 1. `method` chooses which
interval to compute, defaulting to [`ProportionInterval.Exact`](proportioninterval.md) as scipy's
`proportion_ci` does.

**Returns** — `(double Low, double High)`: the interval around the observed proportion. A
one-sided test's interval is half-open, and the far bound is the proportion's own limit, `0` or
`1`, rather than a wider number — a proportion cannot leave that range. So is a two-sided
interval's bound when every trial succeeded or none did.

**Exceptions** — `ArgumentOutOfRangeException` when `level` is `NaN` or outside `(0, 1)`, or
`method` is not one of the three.

**Example** — the same seven of twenty, under two of the three methods.

```csharp
using Lodestar.Stats;

BinomialResult result = Binomial.Test(7, 20, 0.5);
(double exactLow, double exactHigh) = result.ProportionConfidenceInterval();
(double wilsonLow, double wilsonHigh) =
    result.ProportionConfidenceInterval(0.95, ProportionInterval.Wilson);

double clopperPearson = Math.Round(exactLow, 4);   // => 0.1539
double wilson = Math.Round(wilsonLow, 4);          // => 0.1812
```

**Remarks — "exact" names the derivation, not the coverage.** Clopper-Pearson inverts the binomial
tails themselves, so it is guaranteed never to cover *less* than the level asked for — and because
the binomial is discrete, it usually covers more, which makes it wider than it needs to be.
Wilson inverts the normal approximation instead: it is close to the level on average and can fall
below it for particular counts. Neither is wrong; they answer "at least this much coverage" and
"about this much coverage", and which one a reader wants depends on what a wrong interval costs.

**A bound the data pins exactly is reported as the limit, not computed.** With every trial a
success there is no upper bound to estimate, and the interval runs to `1`.

```csharp
using Lodestar.Stats;

(double low, double high) = Binomial.Test(20, 20, 0.5).ProportionConfidenceInterval();

double lower = Math.Round(low, 4);   // => 0.8316
double upper = high;                 // => 1
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Binomial.Test`](binomial-test.md), [`ProportionInterval`](proportioninterval.md),
[`PearsonResult.ConfidenceInterval`](pearsonresult-confidenceinterval.md), the
[Python equivalence table](../../../equivalence.md).
