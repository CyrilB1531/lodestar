# Distributions

The density, both tails and both inverses of the standard normal, Student's *t*, *F* and
chi-squared laws, as `scipy.stats` gives them.

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

**Remarks** — twenty members, `Pdf`, `Cdf`, `Sf`, `Quantile` (scipy's `ppf`) and `Isf` for each
law. Five were published first because a sibling package asked for them, under
[`decisions/0003`](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md); the
other fifteen followed in #1158, so a caller writing their own test, interval or power
calculation no longer leaves the package for the law underneath it. Each tail is evaluated on the
side that keeps full relative precision, and the log-gamma, incomplete beta and gamma and their
inverses stay internal. The [index page](../tails.md) has the history.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TTest`](../tests/ttest.md), [`OneWayAnova`](../tests/onewayanova.md),
the [distributions index](../tails.md).

## Members

| Member | What it does |
| --- | --- |
| [`Distributions.ChiSquaredCdf`](distributions-chisquaredcdf.md) | The lower tail of the chi-squared distribution. |
| [`Distributions.ChiSquaredIsf`](distributions-chisquaredisf.md) | The inverse of the chi-squared upper tail. |
| [`Distributions.ChiSquaredPdf`](distributions-chisquaredpdf.md) | The chi-squared density. |
| [`Distributions.ChiSquaredQuantile`](distributions-chisquaredquantile.md) | The value a chi-squared falls below with a given probability. |
| [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md) | The upper tail of the chi-squared distribution. |
| [`Distributions.FisherCdf`](distributions-fishercdf.md) | The lower tail of the *F* distribution. |
| [`Distributions.FisherIsf`](distributions-fisherisf.md) | The inverse of the *F* upper tail. |
| [`Distributions.FisherPdf`](distributions-fisherpdf.md) | The *F* density. |
| [`Distributions.FisherQuantile`](distributions-fisherquantile.md) | The value an *F* falls below with a given probability. |
| [`Distributions.FisherSf`](distributions-fishersf.md) | The upper tail of the *F* distribution. |
| [`Distributions.NormalCdf`](distributions-normalcdf.md) | The lower tail of the standard normal. |
| [`Distributions.NormalIsf`](distributions-normalisf.md) | The inverse of the standard normal's upper tail. |
| [`Distributions.NormalPdf`](distributions-normalpdf.md) | The standard normal density. |
| [`Distributions.NormalQuantile`](distributions-normalquantile.md) | The value a standard normal falls below with a given probability. |
| [`Distributions.NormalSf`](distributions-normalsf.md) | The upper tail of the standard normal. |
| [`Distributions.StudentCdf`](distributions-studentcdf.md) | The lower tail of Student's *t*. |
| [`Distributions.StudentIsf`](distributions-studentisf.md) | The inverse of Student's *t* upper tail. |
| [`Distributions.StudentPdf`](distributions-studentpdf.md) | Student's *t* density. |
| [`Distributions.StudentQuantile`](distributions-studentquantile.md) | The value a Student's *t* falls below with a given probability. |
| [`Distributions.StudentSf`](distributions-studentsf.md) | The upper tail of Student's *t*. |
