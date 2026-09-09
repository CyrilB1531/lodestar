# Distributions

The tail probabilities and the quantile a caller holding its own statistic needs.

<!-- docs-declaration -->

```csharp
public static class Distributions
```

**Example** — a coefficient's two-sided p-value and its 95% multiplier, the two numbers a
regression table prints beside an estimate.

```csharp
using Lodestar.Stats;

// A t of 2.0 on 12 residual degrees of freedom.
double twoSided = 2.0 * Distributions.StudentSf(2.0, 12.0);   // => 0.0686550…
double multiplier = Distributions.StudentQuantile(0.975, 12.0); // => 2.1788128296672298

// And the overall F test of a model with two regressors and twenty residual df.
double overall = Distributions.FisherSf(4.0, 2.0, 20.0);        // => 0.0345716…
```

**Remarks** — these are the four members `Lodestar.Stats.Regression` needs, and no more. The
machinery underneath — log-gamma, the incomplete beta and gamma, the normal tail — stays internal;
[`decisions/0081`](../../../decisions/0081-the-stats-numerical-layer-stays-internal.md) says why,
and the [index page](../tails.md) says what publishing these four cost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TTest`](../tests/ttest.md), [`OneWayAnova`](../tests/onewayanova.md),
the [distributions index](../tails.md).

## Members

| Member | What it does |
| --- | --- |
| [`Distributions.FisherSf`](distributions-fishersf.md) | The upper tail of the *F* distribution. |
| [`Distributions.StudentQuantile`](distributions-studentquantile.md) | The value a Student's *t* falls below with a given probability. |
| [`Distributions.StudentSf`](distributions-studentsf.md) | The upper tail of Student's *t*. |
