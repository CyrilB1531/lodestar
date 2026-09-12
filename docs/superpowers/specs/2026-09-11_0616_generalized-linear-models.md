# 0616 — Generalized linear models, with the inference table

**Status:** accepted, 2026-09-11. Written before the work.

**Issue:** [#616](https://github.com/CyrilB1531/lodestar/issues/616), the link-function half of
[#338](https://github.com/CyrilB1531/lodestar/issues/338).

## Problem

`Lodestar.Stats.Regression` fits a model whose response is a real number. It cannot fit one whose
response is a count, a proportion or a category — that needs a link function, and
[decision 0096](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) put it on
this issue's side of the line in its own Consequences.

The capability exists in .NET three times over, and
[decision 0104](../../decisions/0104-generalized-linear-models-are-written-natively.md) read all
three through a `MetadataLoadContext` before the claim was written down. Each is disqualified for a
different reason, and none of them is "nobody did it":

| checked | measured | why it is not the answer |
| --- | --- | --- |
| `Accord.Statistics` 3.8.0 | 561 types, 5 620 members | has the whole stack — `GeneralizedLinearRegression`, eleven `ILinkFunction`s, `LogisticRegressionAnalysis` with standard errors, Wald tests, odds ratios and deviance. **LGPL-2.1**, archived 2017-10-19. [Decision 0003](../../decisions/0003-provenance-and-licensing.md) refuses it on its licence, not its age |
| `Microsoft.ML` 5.0.0 | 77 types in `StandardTrainers`, 220 members | `CoefficientStatistics` is **logistic only**; `PoissonRegressionModelParameters` exposes no statistics at all. No intervals, no odds ratios, no AIC, no dispersion, no link abstraction — and its standard errors need a native MKL |
| `cs-glm` 1.0.1 | 21 types, 112 members | **installs no assembly**: `lib/net461/Release/` is not a lib asset path. Reproduced 2026-09-11 with `tools/survey.cs`, which reports NU1202 against `net10.0` |
| `MathNet.Numerics` 5.0.0 | 336 types, 5 707 members | `Distributions.Poisson` and `Distributions.Logistic` are distributions, not a fitter. No link, no IRLS |

Our own repository was checked too: `Lodestar.Metrics`' `PoissonDeviance`, `GammaDeviance` and
`TweedieDeviance` **evaluate a GLM's predictions and fit nothing**, and
`Lodestar.Decomposition`'s `NmfBetaLoss` is NMF's beta-loss. A reader who greps for "deviance" and
concludes the domain is half-built is reading three metrics.

So the shape of the gap is 0096's again, one link function along: .NET has the **estimate** and not
the **inference**. `Microsoft.ML` will fit a logistic regression and will not tell you whether a
coefficient is distinguishable from zero.

## Scope

The smallest set that stands together and is useless apart:

- **IRLS**, over the Householder QR `Lodestar.Decomposition` already publishes;
- **two families with their canonical links** — binomial/logit and Poisson/log;
- **the summary table**, which is what makes this inference rather than prediction.

Explicitly **not** in this brick, each its own follow-up: negative binomial, Gamma and inverse
Gaussian families; grouped binomial responses; non-canonical links; multinomial and ordinal
responses; offsets and exposure; mixed effects; regularised fits.

## Public surface — four types

`Fit` mirrors `OrdinaryLeastSquares.Fit` member for member, because a reader of one should learn
nothing new from the other: the same flat row-major span, the same `featureCount`, the same
optional options object.

```csharp
namespace Lodestar.Stats.Regression;

public enum GlmFamily { Binomial, Poisson }

public sealed class GlmOptions
{
    public bool WithIntercept { get; init; } = true;
    public double ConfidenceLevel { get; init; } = 0.95;
    public int MaximumIterations { get; init; } = 100;
    public double Tolerance { get; init; } = 1e-8;
    public bool ThrowOnNonConvergence { get; init; } = true;
}

public static class GeneralizedLinearModel
{
    public static GlmSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        GlmFamily family,
        GlmOptions? options = null);
}

public sealed class GlmSummary
{
    public IReadOnlyList<double> Coefficients { get; }
    public IReadOnlyList<double> StandardErrors { get; }
    public IReadOnlyList<double> ZStatistics { get; }
    public IReadOnlyList<double> PValues { get; }
    public IReadOnlyList<double> ConfidenceLower { get; }
    public IReadOnlyList<double> ConfidenceUpper { get; }
    public double Deviance { get; }
    public double NullDeviance { get; }
    public double Dispersion { get; }
    public double LogLikelihood { get; }
    public double Akaike { get; }
    public int ResidualDegreesOfFreedom { get; }
    public bool HasIntercept { get; }
    public bool Converged { get; }
    public int Iterations { get; }
    public double DevianceChange { get; }
}
```

**`GlmFamily` is a closed enum**, not an interface. A public `IGlmFamily` would be a contract in a
core package's first version, and IRLS cannot check that a supplied family is internally consistent
— an incoherent one produces a plausible inference table rather than an error. Adding a member is
not a breaking change, so the follow-up families cost nothing here.

**`Dispersion` is published although it is 1 for both families in this brick.** Binomial and Poisson
fix it by definition and `statsmodels` reports `scale=1` for them. The field exists so the shape of
the summary does not change when Gamma lands, and it is documented as what it is rather than
omitted as uninteresting.

## Non-convergence

IRLS does not always converge. Perfect separation in a logistic fit sends coefficients to infinity;
`statsmodels` returns the table with a Python warning and a `converged=False` on the result.

**`ThrowOnNonConvergence` defaults to true.** A non-converged inference table is plausible and
wrong — enormous standard errors, p-values that read like p-values — and this repository has been
bitten by that shape three times in other clothes: a `--filter` matching nothing and exiting zero,
a pinned report name collapsing 32 files into one, a coverage figure of 0% that meant "no data".
The default refuses to hand back numbers nobody asked to check.

`GlmSummary` carries `Converged`, `Iterations` and `DevianceChange` anyway, for two reasons that
are not "belt and braces": the oracle replays a non-converged case with the flag off and has to
compare those fields like any others, and a caller who deliberately turns the throw off needs
somewhere to read what happened. The message when it throws names the tolerance and the change
reached, so the number that failed is in the exception rather than in a debugger.

## The solve

Per iteration, with `g` the link and `V` the variance function:

```text
eta = X beta                     mu = g^-1(eta)
z   = eta + (y - mu) * g'(mu)    W  = 1 / (V(mu) * g'(mu)^2)
beta <- argmin  sum W (z - X beta)^2
```

The weighted least squares is solved by **scaling the rows by `sqrt(W)` and calling
`QrDecomposition.Householder`** — the same call `OrdinaryLeastSquares.Fit` makes, unchanged. No
weighted overload is added to `Lodestar.Decomposition`, and the normal equations are not used, for
the reason that package's own comment already gives.

The covariance is `phi * (X' W X)^-1`, and through the QR of the scaled design `X̃' X̃ = R' R`, so
`(X' W X)^-1 = R^-1 R^-T` — which is the identity `OrdinaryLeastSquares` computes today. The GLM
reuses that arithmetic rather than restating it.

**Convergence is the oracle's criterion, read from its source rather than described.**
`statsmodels` stops when successive deviances satisfy `numpy.allclose`, whose test is the sum of an
absolute and a relative term:

```text
|D_i - D_{i+1}|  <=  atol + rtol * |D_{i+1}|
```

The relative term is switched off. `GLM.fit` reads `atol = kwargs.get("atol")` and
`rtol = kwargs.get("rtol", 0.0)`, then substitutes `tol` for `atol` when it was not given — so with
`tol_criterion="deviance"`, `maxiter=100` and `tol=1e-8`, the default path is
`|D_i - D_{i+1}| <= 1e-8` and nothing else. `Tolerance` is therefore an absolute bound on the change
in deviance, not a mixed one; offering `rtol` is a second option nothing has asked for.
`MaximumIterations` is 100 for the same reason — a smaller budget would make this report a
non-convergence where the oracle reports a fit, and the corpus could not freeze the difference.

`DevianceChange` is the left side of that inequality at the last iteration, so a caller who turns
the throw off reads the quantity that failed rather than a proxy for it, and the exception message
carries both sides.

## `LogGamma`, which the AIC needs

Poisson's log-likelihood is `sum(y log(mu) - mu - logGamma(y + 1))`. The third term is what makes
the AIC comparable to `statsmodels`', and **`Gamma.LogGamma` is `internal` in `Lodestar.Stats`** —
[decision 0095](../../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
names it explicitly among the members that stay so:

> `RegularizedIncomplete`, **`LogGamma`**, `RegularizedP` […] stay internal. Nothing has asked for
> them, and 0081's asymmetry holds in both directions: publishing later is always available,
> unpublishing never is.

That asymmetry is
[decision 0081](../../decisions/0081-the-stats-numerical-layer-stays-internal.md)'s, and it is why
this is a decision rather than a convenience: the member can be published when a caller needs it and
cannot be withdrawn afterwards.

Something has now asked. `InternalsVisibleTo` on `Lodestar.Stats` reaches only its two test
assemblies, so a GLM in `Lodestar.Stats.Regression` cannot see it. This is therefore an input to
the placement question below rather than a detail:

- **in `Lodestar.Stats.Regression`** — `Distributions.LogGamma` is published from `Lodestar.Stats`
  under 0095's own rule, and that package takes a minor version;
- **in `Lodestar.Stats`** — the internal is reachable and nothing is published, but the
  hypothesis-test package starts fitting regressions.

Binomial's log-likelihood needs no such term for 0/1 responses: the binomial coefficient is
`log C(1, y) = 0`. It would return with a grouped response, which is a follow-up.

## Placement, deliberately deferred

[Decision 0104](../../decisions/0104-generalized-linear-models-are-written-natively.md) left the
package unnamed, and this spec does not name it either. The constraints are known:
[0076](../../decisions/0076-a-core-package-carries-no-external-dependency.md) allows a split only
for a distinct dependency profile, audience or cadence, and by that test a GLM has none — same zero
dependencies, same readers, same release rhythm as the OLS beside it. Against that, #616 measures
the fixed cost of a seventeenth package on #566: five hard-coded pack loops, two release
allow-lists, `Version.props`, the `EXPECTED` entry, `FLOORS` rows, a `*.NetStandard.Tests` mirror
with every `Lodestar.*` dependency pinned by `ProjectReference`, a `wiki-map.json` entry, reference
pages, a sample per public class, equivalence rows, a guide and a bench section.

**The work starts in `Lodestar.Stats.Regression`, and the decision is taken on the measurement
before anything is published.** What is measured: the added source lines, the added public members,
and whether `LogGamma` had to be published — a package that forces a neighbour's surface open is
one argument for being its own thing, and one argument against.

**The decision point is before the version bump, never after.** A published type cannot move
without a deprecation, so deferring past the release is not deferring, it is choosing.

## Oracle

`statsmodels` 0.15.0, BSD-3-Clause, already in `tools/requirements.lock.txt` since #566 — no lock
churn and no Python-floor question. `GLM(y, X, family=sm.families.Binomial()).fit()` and its
Poisson counterpart, with `.params`, `.bse`, `.tvalues`, `.pvalues`, `.conf_int()`, `.deviance`,
`.null_deviance`, `.scale`, `.llf`, `.aic`, `.converged` and `.fit_history['iteration']`.

Two corpora, one per family, in the shape #645 established for the two MinHash schemes: separate
blocks rather than a moved one, so a family added later grows the file instead of rewriting it.

Floats compare at the `1e-9` the suites use, **except p-values, which compare relatively** — a p-value of `1e-17` and one of `2e-17` differ by
`1e-17` absolutely and by a factor of two, and only the second reading is about the algorithm.
[Decision 0081](../../decisions/0081-the-stats-numerical-layer-stays-internal.md) records that and
names where it lives: `StatsOracleAsserts` in `tests/Lodestar.Stats.Tests/Oracles/`, which compares
a p-value at `1e-9` relative and a statistic at `1e-9` absolute in one helper. **These tests reuse
it rather than restating the tolerance**, so a change to the rule reaches the GLM without anyone
remembering to carry it.

One case per corpus is deliberately **non-converged** — a separable logistic design — generated with
`ThrowOnNonConvergence` off, so `Converged`, `Iterations` and `DevianceChange` are frozen like any
other value rather than asserted by hand.

## Testing

- The two oracle corpora, replayed member by member against the summary.
- The non-convergence throw, asserted separately because the corpus records the other branch.
- A refusal per malformed input, matching `OrdinaryLeastSquares`' existing set: a design and
  response of mismatched length, a `featureCount` below one, fewer rows than parameters.
- A binomial response outside `{0, 1}` is refused. `statsmodels` accepts a proportion there and
  means something different by it; accepting it silently would give a caller a grouped-binomial
  answer to a question they asked in unit form.
- Both target frameworks, through the `*.NetStandard.Tests` mirror, which is what makes the
  `netstandard2.0` assembly executed rather than merely compiled.

## Benchmarks

A `bench/README.md` section against `Accord.Statistics` 3.8.0. LGPL-2.1 bars it from `src/` and not
from `bench/`, which is the precedent
[decision 0096](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) set for the
OLS table — a comparison against the library we refused is worth more than one against nothing, and
its archived date is in the row rather than in a footnote.

## Documentation owed, in the same commit as the code

`docs/equivalence.md` rows for each public member, per CLAUDE.md's rule that a row lands with its
function; reference pages under `docs/reference/stats/` for every new public type; a sample per
public class, per decision 0041; a guide entry if the link function needs one, which the first
draft will show.
