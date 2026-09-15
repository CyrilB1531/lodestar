# MultinomialLogit.Fit

Fits one model and reports its inference table.

<!-- docs-declaration -->

```csharp
public static MultinomialLogitSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<int> response, int featureCount, MultinomialLogitOptions options = null)
```

**Parameters** — `design` is the regressors, row-major: `featureCount` values per row, with no constant column of your
own. `response` is one category label per row: any integers, in any order, at least two distinct. `featureCount` is
how many regressors each row carries. `options` chooses the intercept, the confidence level and Newton's budget and
tolerance; `null` takes the reference's defaults.

**Returns** — [`MultinomialLogitSummary`](multinomiallogitsummary.md): the coefficients per non-reference category with
their standard errors, z statistics, p-values and intervals, and the whole-model table.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is below one. `ArgumentException` when `design` is
not a whole number of rows or `response` has another length, when the response holds fewer than two distinct labels,
when no residual degree of freedom is left, or when the Hessian is not positive definite. A category the regressors
separate perfectly and a regressor that is a combination of the others both land there; `statsmodels` returns NaN
coefficients for the first and reports `converged`. `InvalidOperationException` when Newton spends its budget and
[`MultinomialLogitOptions.ThrowOnNonConvergence`](multinomiallogitoptions.md) says throw.

**Example** — the second category's equation, and the test against the constant-only model.

```csharp
using Lodestar.Stats.Regression;

double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23,
                   -0.96, 1.6, 0.2, -1.73, -0.08, -1.16, -0.63, -0.49, -0.71, 0.55, -0.06, -0.59,
                   0.41, 0.83, -1.64, -0.26, -0.98, -0.17, -1.29, 0.02, -0.04, -0.3, -1.05, -0.4];
int[] response = [2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2,
                  0, 0, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2];

MultinomialLogitSummary fit = MultinomialLogit.Fit(design, response, 1);

int reference = fit.Categories[0];                               // => 0
double slope = Math.Round(fit.Coefficients[1][1], 6);            // => -7.040761
double error = Math.Round(fit.StandardErrors[1][1], 6);          // => 2.76465
double ratio = Math.Round(fit.LikelihoodRatio, 6);               // => 33.328409
```

`Coefficients[1]` is the equation of `Categories[2]`, label `2`, against label `0`: as the regressor rises, a row moves
away from `2` towards `0`.

**Remarks** — **the categories are the labels sorted by value**, and the smallest is the reference, as statsmodels
sorts them. Relabelling while keeping the order changes nothing.

`NullLogLikelihood` is the closed form `Σ nⱼ·log(nⱼ/n)`. statsmodels refits the constant-only model with an optimiser
and lands up to `3e-10` from it, so `PseudoRSquared` and `LikelihoodRatio` agree with the reference to about that. The
χ² tail amplifies the gap in `LikelihoodRatioPValue` to about `1e-8`
([decision 0136](../../../decisions/0136-the-multinomial-logit-is-written-and-the-ordered-model-is-not.md)).

`ModelDegreesOfFreedom` is `(K − 1)·(J − 1)` whether or not an intercept was fitted, where `K` is the number of columns
fitted: the reference's `df_model`, which the likelihood-ratio p-value reads.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MultinomialLogitSummary`](multinomiallogitsummary.md),
[`MultinomialLogitOptions`](multinomiallogitoptions.md).
