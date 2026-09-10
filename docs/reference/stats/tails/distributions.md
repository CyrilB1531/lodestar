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

// And a log-rank test's p-value, on one degree of freedom.
double logRank = Distributions.ChiSquaredSf(3.84, 1.0);          // => 0.0500…

// The large-sample multiplier, where a Student one has no degrees of freedom to take.
double large = Distributions.NormalQuantile(0.975);              // => 1.959963…
```

**Remarks** — these are the members a second package has asked for, and no more: four for
`Lodestar.Stats.Regression` under
[`decisions/0095`](../../../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md),
the chi-squared tail for a log-rank test under
[`decisions/0097`](../../../decisions/0097-the-chi-squared-tail-joins-the-published-four.md), and the
normal quantile for that curve's confidence bounds under
[`decisions/0098`](../../../decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md). The
machinery underneath — log-gamma, the incomplete beta and gamma, the normal tail — stays internal;
[`decisions/0081`](../../../decisions/0081-the-stats-numerical-layer-stays-internal.md) says why,
and the [index page](../tails.md) says what publishing them cost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TTest`](../tests/ttest.md), [`OneWayAnova`](../tests/onewayanova.md),
the [distributions index](../tails.md).

## Members

| Member | What it does |
| --- | --- |
| [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md) | The upper tail of the chi-squared distribution. |
| [`Distributions.FisherSf`](distributions-fishersf.md) | The upper tail of the *F* distribution. |
| [`Distributions.NormalQuantile`](distributions-normalquantile.md) | The value a standard normal falls below with a given probability. |
| [`Distributions.StudentQuantile`](distributions-studentquantile.md) | The value a Student's *t* falls below with a given probability. |
| [`Distributions.StudentSf`](distributions-studentsf.md) | The upper tail of Student's *t*. |
