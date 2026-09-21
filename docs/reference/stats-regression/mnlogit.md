# Multinomial logit — `Lodestar.Stats.Regression`

One entry point, [`MultinomialLogit.Fit`](mnlogit/multinomiallogit-fit.md), for a response with more than two
unordered categories. Each category but the smallest label gets its own equation against that reference, and the fit
reports what `statsmodels`' `MNLogit(...).fit()` summary holds: the coefficients per equation with their standard
errors, z statistics, p-values and intervals, the log-likelihood, McFadden's pseudo-R², the likelihood-ratio test,
AIC and BIC.

**Why it is not a GLM family.** The fit is Newton-Raphson on the analytic score and Hessian, not IRLS. Its table is
a matrix, one row per equation, where [`GlmSummary`](glm/glmsummary.md)'s is a vector.

**The ordered model is not here.** `statsmodels`' `OrderedModel` differentiates its likelihood numerically, and does
not reproduce its own answer at the corpus tolerance ([decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)).

## Types

| Type | What it is |
| --- | --- |
| [`MultinomialLogit`](mnlogit/multinomiallogit.md) | Fits the model by Newton-Raphson and builds the table. |
| [`MultinomialLogitSummary`](mnlogit/multinomiallogitsummary.md) | The coefficients per equation, their inference, and the whole-model numbers. |
| [`MultinomialLogitOptions`](mnlogit/multinomiallogitoptions.md) | The intercept, the confidence level, and Newton's budget and tolerance. |

## See also

- [Generalized linear models](glm.md) — the binomial logit is this model at two categories.
- [statsmodels → .NET](../../migration/statsmodels.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
