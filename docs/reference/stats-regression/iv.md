# Instrumental variables — `Lodestar.Stats.Regression`

One entry point, [`InstrumentalVariables`](iv/instrumentalvariables.md), for a regressor that is
correlated with the error — measured with error, chosen by the unit being measured, or determined
jointly with the response. Least squares is biased there, and no amount of data removes the bias;
an instrument that moves the regressor and touches the response only through it does. Two-stage
least squares, LIML and two-step GMM are fitted with the whole table `linearmodels` reports: the
estimates and their errors under four covariances, the first-stage diagnostics that say whether the
instruments are strong, and the overidentification test that says whether they agree.

## Why this exists

No .NET library publishes an instrumental-variables estimator with its inference table: Math.NET
Numerics, Accord and ML.NET fit least squares and stop
([decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)). The
estimators are closed forms, so they are replayed against `linearmodels` at `1e-9` from a frozen
corpus rather than approximated.

## Types

| Type | What it is |
| --- | --- |
| [`InstrumentalVariables`](iv/instrumentalvariables.md) | Fits 2SLS, LIML and two-step GMM and builds the table. |

The data it takes and returns — [`IvDesign`](instrumental/ivdesign.md),
[`IvOptions`](instrumental/ivoptions.md), [`IvSummary`](instrumental/ivsummary.md) and the rest —
live in [`Lodestar.Stats.Regression.Instrumental`](instrumental.md).

## See also

- [Ordinary least squares](ols.md) — the estimator this one corrects.
- [Regression inference](../../guides/regression-inference.md) — reading the table.
- [Python → C# equivalence](../../equivalence.md).
