# GeneralizedLinearModel.Fit

Fits one model and reports its inference table.

<!-- docs-declaration -->

```csharp
public static GlmSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, GlmFamily family, GlmOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static GlmSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> offset, ReadOnlySpan<double> exposure, int featureCount, GlmFamily family, GlmOptions options = null)
```

The second overload adds a term to the linear predictor that carries no coefficient: `offset` as given,
`exposure` as its logarithm. Either span may be empty, and both empty is the first overload.

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one value per row of `design`: `0` or `1` for
`GlmFamily.Binomial`, a count for `GlmFamily.Poisson` and `GlmFamily.NegativeBinomial`, a positive value
for `GlmFamily.Gamma`. `featureCount`
is how many regressors each row carries. `family` is the response distribution, with the link statsmodels
defaults it to. `options` chooses the intercept, the confidence level, the IRLS budget and the negative
binomial's `α` and the link; `null` fits an intercept at 0.95 with the
100-iteration, `1e-8` defaults. `offset` is one finite value per row, and `exposure` one finite value
above zero per row, for a log link only; pass an empty span for either one you do not use.

**Returns** — the fitted model, with its standard errors, z statistics, p-values, confidence
intervals, deviance and the rest of the table `GlmSummary` carries.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is below one, or when `family`
is not a declared `GlmFamily` member; a setting outside its own range throws from
[`GlmOptions`](glmoptions.md) itself. `ArgumentException` when `design` is empty or is not a whole
number of rows, when the lengths disagree, when a response value is outside its family — a
`Binomial` response that is not `0` or `1`, a count one that is negative, fractional or
infinite, or a `Gamma` one that is not finite and above zero — when a count response is zero in every row, when no residual degree of
freedom is left, when the design is rank deficient and the weighted least squares has no unique
solution, or when `GlmOptions.NegativeBinomialAlpha` is set for a family other than
`NegativeBinomial`, when `GlmOptions.Link` names a link the family is not fitted through here, or when
the inverse link takes a `Gamma` mean to zero or below during IRLS. `InvalidOperationException` when
IRLS did not converge and `GlmOptions.ThrowOnNonConvergence` says throw. The second overload also
throws `ArgumentException` when `offset` or `exposure` is not empty and has a different length, or when
`exposure` is given with a link other than log (a binomial fit, or `Gamma` through the inverse link),
and `ArgumentOutOfRangeException` when an offset is not finite or an exposure is not finite and above
zero. statsmodels raises `ValueError` for each.

A `Poisson` count has no upper bound: the log-likelihood's `log(y!)` reads a table below 256 and a
Stirling series above it, in constant time, where statsmodels evaluates `gammaln(y + 1)`. Until
[#665](https://github.com/CyrilB1531/lodestar/issues/665) a count above one million was refused,
since the table then grew with the largest count.

An all-zero `Poisson` response is refused on both sides, for the same reason stated differently:
its likelihood is maximised at minus infinity, so a fit would report where the tolerance stopped
rather than an estimate. statsmodels raises `ValueError` from the first deviance evaluation.

**Example** — the same design fit through both families reads differently: `Poisson`'s count
response through the log link, against `Binomial`'s `{0, 1}` one through the logit link.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
double[] response = [2.0, 3.0, 6.0, 8.0, 10.0, 13.0, 14.0, 17.0];

GlmSummary fit = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Poisson);

double slope = fit.Coefficients[1];        // => 0.25511965967535943
double error = fit.StandardErrors[1];      // => 0.05644539163614572
double significance = fit.PValues[1];      // => 6.19095855462…
```

**Rates** — counts over unequal exposures, the model a claims or incidence table asks for.

```csharp
using Lodestar.Stats.Regression;

double[] risk = [0.0, 1.0, 2.0, 3.0, 0.5, 1.5, 2.5, 3.5, 0.2, 1.2, 2.2, 3.2];
double[] claims = [1.0, 4.0, 1.0, 12.0, 3.0, 4.0, 7.0, 3.0, 2.0, 3.0, 9.0, 5.0];
double[] years = [1.0, 2.5, 0.5, 4.0, 3.0, 1.5, 2.0, 0.75, 3.5, 1.25, 2.75, 1.0];

GlmSummary rate = GeneralizedLinearModel.Fit(risk, claims, [], years, 1, GlmFamily.Poisson);
GlmSummary count = GeneralizedLinearModel.Fit(risk, claims, 1, GlmFamily.Poisson);

double perYear = Math.Round(rate.Coefficients[1], 6);      // => 0.462111
double error = Math.Round(rate.StandardErrors[1], 6);      // => 0.130564
double ignored = Math.Round(count.Coefficients[1], 6);     // => 0.350862
double baseline = Math.Round(Math.Exp(rate.Coefficients[0]), 6);  // => 0.945009
```

`exposure` enters as `log(years)` with its coefficient fixed at one, so the intercept is a rate:
0.945 claims a year at zero risk. Leaving the exposure out reads the rows' unequal years as part of the
risk effect and reports 0.351 where the rate model reports 0.462.

With either term given, `NullDeviance` is the deviance of the intercept-only model **refitted with the
same term**, as statsmodels computes it, rather than the deviance at the response mean. That is also
true of an offset of zeros, which fits the same coefficients as no offset but takes that refit.

**Remarks** — **the intercept is not a column you supply.** `WithIntercept` prepends it, so
`Coefficients[0]` is the intercept and the regressors follow in the design's own order, exactly as
[`OrdinaryLeastSquares.Fit`](../ols/ordinaryleastsquares-fit.md) does.

A fit that does not converge is refused by default. Set
[`GlmOptions.ThrowOnNonConvergence`](glmoptions.md) to `false` to inspect one instead — read
[`GlmSummary.Converged`](glmsummary.md) before anything else in the table it returns, because a
non-converged inference table is plausible and wrong.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GlmSummary`](glmsummary.md), [`GlmOptions`](glmoptions.md),
[`GlmFamily`](glmfamily.md).
