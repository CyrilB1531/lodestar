# GeneralizedLinearModel.Fit

Fits one model and reports its inference table.

<!-- docs-declaration -->

```csharp
public static GlmSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, GlmFamily family, GlmOptions options = null)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one value per row of `design`: `0` or `1` for
`GlmFamily.Binomial`, a count for `GlmFamily.Poisson` and `GlmFamily.NegativeBinomial`, a positive value
for `GlmFamily.Gamma`. `featureCount`
is how many regressors each row carries. `family` is the response distribution, with the link statsmodels
defaults it to. `options` chooses the intercept, the confidence level, the IRLS budget and the negative
binomial's `α` and the link; `null` fits an intercept at 0.95 with the
100-iteration, `1e-8` defaults.

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
IRLS did not converge and `GlmOptions.ThrowOnNonConvergence` says throw.

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
