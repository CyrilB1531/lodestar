# GeneralizedLinearModel.Fit

Fits one model and reports its inference table.

<!-- docs-declaration -->

```csharp
public static GlmSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount, GlmFamily family, GlmOptions options = null)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no
constant column of your own. `response` is one value per row of `design`: `0` or `1` for
`GlmFamily.Binomial`, a count for `GlmFamily.Poisson`. `featureCount` is how many regressors each
row carries. `family` is the response distribution, with its canonical link. `options` chooses
the intercept, the confidence level and the IRLS budget; `null` fits an intercept at 0.95 with the
100-iteration, `1e-8` defaults.

**Returns** — the fitted model, with its standard errors, z statistics, p-values, confidence
intervals, deviance and the rest of the table `GlmSummary` carries.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is below one, or when `family`
is not a declared `GlmFamily` member; a setting outside its own range throws from
[`GlmOptions`](glmoptions.md) itself. `ArgumentException` when `design` is empty or is not a whole
number of rows, when the lengths disagree, when a response value is outside its family — a
`Binomial` response that is not `0` or `1`, or a `Poisson` one that is negative, fractional or
above one million — when a `Poisson` response is zero in every row, when no residual degree of
freedom is left, or when the design is rank deficient and the weighted least squares has no unique
solution. `InvalidOperationException` when
IRLS did not converge and `GlmOptions.ThrowOnNonConvergence` says throw.

The Poisson bound is this implementation's and not the reference's: the log-likelihood sums an
exact `log(k!)` table indexed by the largest count, 8 MB at a million and unbounded above it, where
statsmodels evaluates `gammaln(y + 1)` in constant time
([#665](https://github.com/CyrilB1531/lodestar/issues/665)).

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

double slope = fit.Coefficients[1];        // => 0.2551196596753593
double error = fit.StandardErrors[1];      // => 0.0564453916361457
double significance = fit.PValues[1];      // => 6.1909585546285475E-06
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
