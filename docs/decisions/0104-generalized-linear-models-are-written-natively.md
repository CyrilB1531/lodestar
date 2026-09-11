# 0104 — Generalized linear models are written natively: Accord is LGPL-2.1, ML.NET stops at logistic

**Status:** accepted · **Date:** 2026-09-11

## Context

[#338](https://github.com/CyrilB1531/lodestar/issues/338) proposed a project for "Advanced GLMs,
time series and econometric summaries" and put its own Phase 1 first: *inventory what Math.NET,
ML.NET and Deedle actually cover, and identify the genuine holes as opposed to the ones that are
only a matter of ergonomics.*

One third of that issue closed before this reading began.
[Decision 0096](0096-ordinary-least-squares-earns-its-own-package.md) shipped the OLS summary
table as `Lodestar.Stats.Regression` and drew the boundary in its own Consequences:

> This package is OLS and its table; anything with a link function or a time index falls on that
> issue's side of the line.

This decision takes the link function. The time index is
[decision 0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md).

The instruction is [#427](https://github.com/CyrilB1531/lodestar/issues/427)'s protocol and
[decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md): read the
incumbent's exported surface through a `MetadataLoadContext`, not its README; check the
first-party incumbent and the dominant third-party one; and check our own repository before
declaring a gap.

## The reading

Read on **2026-09-11**. Each package was installed into a throwaway project outside this
repository, its dependency closure resolved with `dotnet publish`, and the target assembly loaded
through a `PathAssemblyResolver` over that closure plus the running runtime's reference
assemblies. A bare `lib/*.dll` cannot be read — the missing-dependency failure 0074 hit on
`Mosaik.Core`.

Member counts below exclude property and event accessors, which are one member counted twice.
On that basis `MathNet.Numerics` 5.0.0 reads **336 exported types** and `Accord.Statistics` 3.8.0
reads **561** — reproducing 0096's type counts exactly. The member totals do not reproduce 0096's,
because 0096 did not state its basis; the counts below are comparable with each other and with
nothing else. That this cannot be settled is [#619](https://github.com/CyrilB1531/lodestar/issues/619).

Only declarations were read. No method body of any assembly was opened, which is what
[decision 0003](0003-provenance-and-licensing.md) constrains.

### `Accord.Statistics` 3.8.0 — the whole thing, and it is LGPL-2.1

561 exported types, 5 620 members. It ships a complete generalized linear model:

```text
Accord.Statistics.Models.Regression.GeneralizedLinearRegression
Accord.Statistics.Models.Regression.LogisticRegression
Accord.Statistics.Models.Regression.MultinomialLogisticRegression
Accord.Statistics.Models.Regression.ProportionalHazards
Accord.Statistics.Models.Regression.Fitting.IterativeReweightedLeastSquares
  ComputeStandardErrors, GetInformationMatrix, Gradient, HasConverged
Accord.Statistics.Links.ILinkFunction
  Absolute, Cauchit, Identity, Inverse, InverseSquared, Logit,
  Log, LogLog, Probit, Sin, Threshold            (ten implementations)
Accord.Statistics.Analysis.LogisticRegressionAnalysis
  Coefficients, StandardErrors, WaldTests, Confidences, OddsRatios,
  Deviance, LogLikelihood, ChiSquare, LikelihoodRatioTests,
  InformationMatrix, GetConfidenceInterval, GetPredictionInterval
Accord.Statistics.Analysis.MultinomialLogisticRegressionAnalysis
Accord.Statistics.Analysis.StepwiseLogisticRegressionAnalysis
```

That is the table `statsmodels` prints, plus Cox regression and stepwise selection. It is also
**LGPL-2.1**, last released **2017-10-19**, `accord-net/framework` archived with its last push
**2020-11-18**.

### `Microsoft.ML` 5.0.0 — inference exists, and it is logistic only

The first-party incumbent, MIT, `dotnet/machinelearning` at 9 358 stars and pushed 2026-09-10.
`Microsoft.ML.StandardTrainers` reads 77 exported types, 220 members. Searching the assembly
rather than the namespace found what a README would have hidden — there *is* coefficient
inference:

```text
Microsoft.ML.Trainers.CoefficientStatistics
  Estimate, StandardError, ZScore, PValue, Index
Microsoft.ML.Trainers.LinearModelParameterStatistics
  GetWeightsCoefficientStatistics, GetBiasStatistics, GetBiasStatisticsForValue
Microsoft.ML.Trainers.ModelStatisticsBase
  Deviance, NullDeviance, ParametersCount, TrainingExampleCount
```

Four things bound it, and together they are the gap.

It is **binary logistic only**. `LbfgsPoissonRegressionTrainer` produces
`PoissonRegressionModelParameters`, which declares **no public member of its own** — a Poisson fit
comes back with no standard error, no p-value and no deviance.

The table is **incomplete even for logistic**: no confidence interval on a coefficient, no odds
ratio, no dispersion, no AIC or BIC, no likelihood-ratio test.

The standard errors are **not in this package**. `ComputeLogisticRegressionStandardDeviation`
declares `ComputeStandardDeviation` and no public constructor; the only concrete implementation is
`ComputeLRTrainingStdThroughMkl`, in `Microsoft.ML.Mkl.Components` 5.0.0, which carries a native
Intel MKL redistributable. Asking ML.NET for a standard error means taking that dependency.

And there is **no link function as a concept** anywhere in the assembly — no `ILinkFunction`, no
family, no way to say "Gamma with a log link".

### `cs-glm` 1.0.1 — the capability is there and the package does not install

The only NuGet hit for `generalized linear model` besides Accord. Its nuspec describes
"Generalized Linear Model in .NET", and it is not empty: the `GlmSharp.dll` it bundles reads 21
exported types and 112 members, with a real stack —

```text
GlmSharp.Glm
  DistributionFamily, GetLinkFunction, Solve, Predict, Statistics, MaxIters, Tol
GlmSharp.GlmDistributionFamily
  Bernouli, Binomial, Categorical, Exponential, Gamma,
  InverseGaussian, Multinomial, Normal, Poisson
GlmSharp.GlmIrls, GlmIrlsQrNewton, GlmIrlsSvdNewton
```

— and it **installs nothing**. Its assemblies sit at `lib/net461/Release/` and `lib/net461/Debug/`;
a subfolder of a target-framework folder is not a lib asset path, so `dotnet add package` followed
by `dotnet publish` copied zero assemblies into the closure. The file above had to be lifted out
of the package cache by hand to be read at all. It is `net461` only, 1 star, frozen 2018-05-04,
and it misspells `Bernouli` in a public enum.

This is the one entry the protocol changed the verdict on. Dismissed on its date it would have
looked like an ordinary stale package; read, it is a package that cannot be consumed as published.

### `MathNet.Numerics` 5.0.0 — distributions, not a fitter

336 exported types, 5 707 members, re-read for this question after 0096 read it for linear
regression. `Distributions.Poisson`, `Distributions.Logistic` and `Distributions.ConwayMaxwellPoisson`
are probability distributions. Across 5 707 members there is no link function, no iteratively
reweighted least squares and no generalized linear fit of any kind.

### The searches that returned nothing

| query, nuget.org, 2026-09-11 | packages |
| --- | --- |
| `negative binomial` | 0 |
| `econometrics` | 0 |

Recorded rather than omitted, which is what
[decision 0099](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)
established an empty search is for.

### Our own repository

Per #427's last clause, checked before the gap was declared:

```text
src/Lodestar.Metrics/PoissonDeviance.cs, GammaDeviance.cs, TweedieDeviance.cs
src/Lodestar.Decomposition/NmfBetaLoss.cs
```

All four are **evaluation, not fitting**. The three deviances are scikit-learn's
`mean_*_deviance` metrics, which score a GLM's predictions once something else has produced them;
`NmfBetaLoss` is the beta-loss family for non-negative matrix factorisation. Nothing here fits a
model, and a reader who greps for `Poisson` should not conclude the domain is half-built.

## Decision

**A generalized linear model with its inference table is written natively**, as
[#616](https://github.com/CyrilB1531/lodestar/issues/616).

The gap is not that nobody did it. Three .NET packages did, and each is disqualified for a
different reason that has nothing to do with quality: Accord's licence, ML.NET's scope, and
`cs-glm`'s packaging. What does not exist is a **maintained, permissively licensed,
framework-free** GLM table — the same sentence 0096 reached for OLS, for the same two of the same
three reasons.

The first lot is the IRLS solver over the Householder QR `Lodestar.Decomposition` already
publishes, two families with their canonical links — binomial/logit and Poisson/log — and the
summary table that makes it inference: coefficients, standard errors, Wald z, p-values, intervals,
deviance, null deviance, dispersion, log-likelihood and AIC.

Explicitly **not** in the first lot: negative binomial, Gamma and inverse Gaussian families;
multinomial and ordinal responses; offsets and exposure; mixed effects; regularised fits. Each is
its own lot, and #616 names them so that the first one is not quietly widened.

**The package is not named here.** Core tier, zero external dependencies, `net10.0;netstandard2.0`
([decision 0076](0076-a-core-package-carries-no-external-dependency.md)); two-level naming, so
`Lodestar.Stats.Glm` is available and `Lodestar.Stats` and `Lodestar.Stats.Regression` are taken.
Whether it earns a package at all is the question 0096 answered on audience, and it is answered
the same way — with a measurement — when #616's spec is written, not before.

## What the licences decide, and what they do not

`Accord.Statistics` is refused on its **licence**, not on its capability and not on its age.
LGPL-2.1 is copyleft and [decision 0003](0003-provenance-and-licensing.md) admits permissive
references only; a core package taking it would also have failed 0076 immediately, whatever the
licence said. Being archived is a reason to prefer a maintained implementation, not the reason it
is refused — 0099's framing, kept.

Two licences could not be read from package metadata, which is the trap
[decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md) named and 0099 paid
again. `cs-glm` publishes a bare GitHub URL rather than an SPDX expression; `cschen1205/cs-glm`
reports MIT. `Deedle` 8.1.0 publishes **no licence field at all** and `fslaborg/Deedle` reports
BSD-2-Clause — recorded here because 0105 relies on it.

And what a licence does not decide: `Accord.Statistics` stays admissible in `bench/`, which ships
nothing. That is 0096's precedent and #616 carries it as an acceptance criterion.

## Consequences

- [#616](https://github.com/CyrilB1531/lodestar/issues/616) is opened with this reading as its
  evidence, on milestone 0.7.0, with a spec owed before any code.
- `docs/migration/statsmodels.md` stops saying "⚠️ gap — write or work around" for GLMs and says
  what was read instead, naming ML.NET's logistic table and its four bounds.
- The oracle is `statsmodels` 0.15.0, already in `tools/requirements.lock.txt` since #566, so this
  lot costs no lock churn and raises no Python-floor question — unlike #566 itself, which pulled
  `pandas`, `patsy` and `formulaic` in.
- A `bench/README.md` section is owed by the lot that ships code, not by this decision. It will
  measure against `Accord.Statistics`, for the reason above.
- The member counts here are not comparable with 0096's, because neither states a basis that the
  other shares. [#619](https://github.com/CyrilB1531/lodestar/issues/619) is where that stops
  being true for the sixth reading.
- #338 is discharged by this decision and 0105 together, and closes: it asked for a
  reconnaissance, a decision per subject and a roadmap, and all three now exist. Its three
  subjects leave as [#616](https://github.com/CyrilB1531/lodestar/issues/616),
  [#617](https://github.com/CyrilB1531/lodestar/issues/617) and
  [#621](https://github.com/CyrilB1531/lodestar/issues/621).
- **Mixed and hierarchical models were not read.** Neither this decision nor 0105 covers them, no
  capability search was run for them, and #621 exists so that closing #338 does not turn an
  unexamined subject into a settled one. Its verdict is allowed to be "not written".
