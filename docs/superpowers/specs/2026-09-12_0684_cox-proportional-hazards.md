# 0684 — The Cox proportional hazards model, and a reference that stops before its maximum

**Status:** written before the work, 2026-09-12. The convergence section was corrected on
2026-09-14, when writing the generator measured it again and refuted it.

**Issue:** [#684](https://github.com/CyrilB1531/lodestar/issues/684), opened on the reading that
`Lodestar.Survival` holds the descriptive half of survival analysis and stops before the method the
field actually uses.

## Problem

`src/Lodestar.Survival/` answers *what does survival look like in these groups* (Kaplan-Meier,
Nelson-Aalen) and *are two curves different* (the log-rank test). It does not answer *by how much
does a covariate change the hazard*, which is the question a dataset with covariates is usually
asked, and which the Cox proportional hazards model is the standard answer to.

The capability search behind
[decision 0099](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)
returned zero NuGet packages for survival analysis on 2026-09-09, and a re-check finds nothing for
Cox either. `scikit-survival` is refused on GPL-3.0-or-later by
[decision 0003](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0003-provenance-and-licensing.md) — a licence refusal, so it cannot
be read as a behaviour reference either. The oracle is `lifelines`, as it already is for the three
members that ship.

## What the issue assumed, measured

Three of the issue's premises were checked rather than taken.

**Nothing needs to go public.** The issue's fifth bullet reserves a place for "whatever tail or
numerical member this needs from `Lodestar.Stats`", to be published under
[decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
rule. Measured: Cox needs a two-sided normal p-value and a normal quantile, and
`Distributions.NormalQuantile` and `Distributions.ChiSquaredSf` are **already public** —
`KaplanMeier.Estimate` uses the first for its log-log bounds and `LogRank.Test` uses the second.
Decision 0095 does not fire a fourth time, and this work publishes nothing new from `Lodestar.Stats`.

**Breslow is refused, and the reason is the oracle rather than a preference.** The issue asks for
"Efron's handling of ties (lifelines' default; Breslow as an option or refused with the reason)".
Read from the signature: `lifelines.CoxPHFitter` 0.30.3 takes **no `ties` parameter at all** —
`__init__` takes `baseline_estimation_method`, `penalizer`, `strata`, `l1_ratio`, `n_baseline_knots`,
`knots` and `breakpoints`, and `baseline_estimation_method` chooses how the *baseline hazard* is
estimated, not how the partial likelihood breaks ties. Efron is the only tie handling lifelines
offers. A Breslow option could therefore ship only with hand-written expectations, against a
repository whose whole conformance argument is frozen reference values
([decision 0073](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0073-the-oracle-gate-compares-numbers-not-bytes.md)). Breslow is
refused on that ground, recorded here so a later contributor does not re-open it as an oversight.

**The proportional-hazards test is a separate function.** `lifelines.statistics.proportional_hazard_test`
is not a method on the fitter; it takes a fitted model and the data, and defaults to
`time_transform="rank"`. That it is separable in the reference is what makes splitting it off a
natural seam rather than an arbitrary one.

## The finding that shapes everything else: lifelines stops before its maximum

This is the measured fact this specification exists to record, and it was found by executing both
sides rather than by reading either.

**What the first draft claimed, and why it was wrong.** It said lifelines' loop discards its last
step, that tightening `fit_options` moves nothing, and that refitting from lifelines' own answer
(`initial_point=`) reaches a fixed point within 9.2e-9, so the corpus would be compared relatively at
1e-7. Measured again while writing the generator, none of that holds. `CoxPHFitter` standardises the
design before its Newton-Raphson, and `initial_point` is read in that standardised space; the loop
also mutates the array it is given. Refitting from `params_` without rescaling measured nothing,
and a correctly rescaled refit stops at exactly the same place as the first fit.

**What does hold.** lifelines stops when the step norm **or the Newton decrement** `g·H⁻¹·g / 2`
falls below `precision` (default `1e-7`), or when the relative change in log-likelihood falls below
`r_precision` (default `1e-9`). The decrement is quadratic in the gradient, so it fires while the
score is still far from zero: at the defaults, lifelines stops with the score near **1e-4**.

Measured against an independent Newton-Raphson on the same Efron partial likelihood, run until the
step is below 1e-14:

| lifelines `fit_options` | worst coefficient, relative, over the five fixtures below |
| --- | --- |
| defaults | **8.6e-6** (heavy ties) |
| `precision=1e-12`, `r_precision=0` | 6.4e-8 (heavy censoring) |
| `precision=1e-14` or `1e-16`, `r_precision=0` | 2.5e-10 (no ties; three covariates) — the decrement still fires |
| **`precision=1e-20`, `r_precision=0`** | **3.1e-13**, with no `ConvergenceWarning` on any fixture |

The log partial likelihood agrees at every setting, to **0.0** at the same β, which is what says
the Efron formula below is the one lifelines evaluates. `batch_mode=True` and `False` give the same
answer, so the residual is the stopping rule and not either of lifelines' two gradient paths.

### Consequence: the Cox corpus is compared at 1e-9, like every other

The generator fits with `fit_options={"precision": 1e-20, "r_precision": 0.0}`, so the frozen values
are the maximum rather than lifelines' default stopping point. Over the whole table the worst
disagreement with the independent Newton-Raphson is **2.2e-12** (p-values, relative) and
**7.4e-13** (z statistics, absolute).

So the replay test uses the repository's usual tolerances: floats at `1e-9` absolute, and p-values
at `1e-9` relative as `OlsOracleTests` and `GlmOracleTests` already do. That relative rule for
p-values comes from [decision 0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md);
[decision 0122](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0122-erfc-is-an-interpolant-sampled-from-the-incomplete-gamma.md)
amends 0081 on how `erfc` is evaluated, not on this. No per-family tolerance is needed, and
`tools/compare_oracles.py` is untouched.

**A lifelines fit at its defaults is not the oracle, and a caller comparing against one should
expect the coefficients to differ in the sixth significant figure.** `docs/equivalence.md` says so.

## Decision

**`CoxProportionalHazards.Fit`, in `Lodestar.Survival`**, beside the three members that ship.

[Decision 0096](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0096-ordinary-least-squares-earns-its-own-package.md) split ordinary
least squares out of `Lodestar.Stats` into its own package on an audience argument — *"a caller who
wants a regression table wants none of them, and a caller running a Kruskal-Wallis wants no QR"*.
That argument does not transfer. The audience for a Cox model is the audience for a Kaplan-Meier
curve: someone holding censored durations, who will usually want both, and who fits the curve first
to look at the data. [Decision 0114](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0114-the-serial-correlation-diagnostics-stay-in-lodestar-stats.md)
is the recent precedent for a member staying in the package whose audience it serves.

```csharp
public static CoxSummary Fit(
    ReadOnlySpan<double> design,
    ReadOnlySpan<double> durations,
    ReadOnlySpan<bool> eventObserved,
    int featureCount,
    CoxOptions? options = null)
```

Row-major `design` with `featureCount` columns, one row per subject, exactly as
`OrdinaryLeastSquares.Fit` takes it. `durations` and `eventObserved` are the pair
`KaplanMeier.Estimate` and `NelsonAalen.Estimate` already take, in that order, so the three members
read alike at a call site.

**No intercept.** A Cox model has none — the baseline hazard absorbs it — so `CoxOptions` carries no
`WithIntercept`, and that absence is a documented difference from `OlsOptions` rather than an
omission.

### What it reports

`CoxSummary`, a sealed class with `init` accessors and an `internal` constructor, following
`OlsSummary` exactly. A class rather than a record: two fitted models are not a thing a caller
compares, so the `with` expression and the value equality
[decision 0113](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0113-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
would then oblige buy nothing.

| member | what it is |
| --- | --- |
| `Coefficients` | the log hazard ratios, in the design's column order |
| `StandardErrors` | the square roots of the diagonal of the inverse observed information |
| `ZStatistics` | each coefficient over its standard error |
| `PValues` | two-sided, as `Distributions.ChiSquaredSf(z * z, 1.0)` |
| `ConfidenceLower`, `ConfidenceUpper` | on `Distributions.NormalQuantile`, at `ConfidenceLevel` |
| `HazardRatios`, `HazardRatioLower`, `HazardRatioUpper` | the exponentials of the three above |
| `LogLikelihood` | the log partial likelihood at the fitted β |
| `NullLogLikelihood` | the same at β = 0 |
| `LikelihoodRatioStatistic` | `2 * (LogLikelihood - NullLogLikelihood)` |
| `LikelihoodRatioPValue` | `Distributions.ChiSquaredSf(statistic, featureCount)` |
| `LikelihoodRatioDegreesOfFreedom` | `featureCount` |
| `ConcordanceIndex` | Harrell's C, on the negated partial hazard |
| `ConfidenceLevel` | the level the two interval lists were built at |

**No iteration count is reported.** It was in an earlier draft of this table and is dropped here:
it is the one member the corpus could not check, since lifelines reaches its answer under a
different stopping rule and a damped step, and a public member no oracle constrains is a member
that can drift silently. A fit that does not converge throws; one that does has nothing to say
about how long it took.

Verified against lifelines: the reported likelihood-ratio statistic **is** `2 * (ll - ll_null)` to
the last bit, its degrees of freedom are the covariate count, and `concordance_index_` equals
`concordance_index(T, -predict_partial_hazard(df), E)` exactly. `NullLogLikelihood` needs no second
fit — the partial likelihood at β = 0 is a direct evaluation.

The two `IReadOnlyList<double>` lists per coefficient are parallel and in the design's own order,
which is `OlsSummary`'s stated contract and needs no second spelling here.

### The two internals

**`Internal/EfronPartialLikelihood.cs`** — the log partial likelihood, its score, and the observed
information, in one pass over the risk sets. Efron's handling of ties is the whole reason this file
is not four lines: at a time carrying `m` events, the `l`-th of them divides by
`S₀ − (l/m)·S̃₀`, where the tilde sums run over the tied events alone. The sample is ordered by
ascending duration with events before censorings at a tie, and the risk set at each distinct time is
the suffix from that time onward, so the three sums are accumulated once rather than recomputed per
event.

**`Internal/Cholesky.cs`** — the p×p solve and the inverse. The observed information is symmetric and
positive definite at a well-posed optimum, so one factorization answers both the Newton step
`H⁻¹U` and the covariance `H⁻¹` whose diagonal the standard errors are. This is not a second copy
of a delicate kernel: no Cholesky exists anywhere in this repository, and the alternative —
an eleventh package edge onto `Lodestar.Decomposition` for the public
`QrDecomposition.Householder` — would apply a general rectangular factorization to a small
symmetric positive-definite matrix, which is more work and a worse inverse.

**A refused factorization is the diagnostic, not an error to route around.** A covariate that
separates the events, or two that are collinear, make the information indefinite or singular; the
factorization fails there, and `Fit` throws an `ArgumentException` naming that cause. lifelines
returns numbers in this situation, behind a warning a caller may not see. This is a deliberate
divergence, and it goes in `docs/equivalence.md` beside the two this package already records.

### Convergence

Undamped Newton-Raphson from β = 0, stopping when the largest absolute component of the step falls
below `1e-10`, capped at 100 iterations. The fixture above reaches machine precision in **four**
iterations, and lifelines' own damping (an initial step of 0.95, adapted) exists for the separated
and near-separated fits that this specification refuses rather than approximates. Exhausting the cap
throws, naming the iteration count — a Cox fit that has not converged has no standard errors worth
reporting, and returning one with a flag invites it to be read as a result.

`CoxOptions` carries `ConfidenceLevel` (0.95, validated strictly inside (0, 1) with the wording
`OlsOptions` already uses) and `MaximumIterations` (100). No `penalizer`: lifelines defaults it to
zero, an unpenalised fit is what the oracle freezes, and ridge-penalised Cox is a second decision
with its own corpus.

## Testing

**`tests/oracles/survival_cox.json`**, frozen from lifelines 0.30.3 at `precision=1e-20` as described above. Per
case: the design, durations and event flags in, and the whole table out.

The fixtures, chosen so that each would catch something a different one would not:

- **heavy ties, two covariates** — one continuous, one binary. Ties are where Efron and Breslow
  part company, so this is the case that proves which is implemented rather than agreeing with
  either.
- **no ties at all** — where Efron and Breslow coincide, so a tie-handling error hides here and the
  fixture above is what exposes it. Present to pin the untied path, not to discriminate.
- **heavy censoring**, around 70% censored, which moves the risk sets without moving the events.
- **a single covariate**, where the p×p algebra degenerates to a scalar and a Cholesky bug would
  otherwise be invisible.
- **three covariates**, the smallest case where the inverse is not a formula.

Two facts are asserted in the C# suite rather than the corpus, because lifelines produces no value
to freeze:

- a design whose covariate separates the events completely — the factorization is refused, and
  `Fit` throws naming separation or collinearity;
- a duplicated covariate column — collinear by construction, refused the same way.

Both are the decision above made checkable, and they are the reason the refusal is a design choice
rather than an accident of arithmetic.

The netstandard2.0 mirror picks these up automatically, as every test file in
`tests/Lodestar.Survival.Tests/` already is.

## Documentation owed, in the same commit as the code

- Reference pages under `docs/reference/survival/estimators/` — the namespace `docs/wiki-map.json`
  declares covered for this package — for `CoxProportionalHazards`, `CoxProportionalHazards.Fit`,
  `CoxSummary` and `CoxOptions`. Their ` ```csharp ` fences execute in CI, so a `// =>` on one is
  checked rather than trusted.
- `docs/guides/survival-analysis.md` gains a Cox section that **leads with the proportional-hazards
  assumption**, the way `docs/guides/conformal.md` leads with exchangeability. This is the issue's
  third bullet at its stated minimum, and the section says plainly what the model assumes, that this
  release does not test it, and which issue will.
- `docs/equivalence.md` rows: `CoxPHFitter.fit` to `CoxProportionalHazards.Fit`, carrying the two
  divergences — that the corpus is lifelines at a tightened stopping rule rather than at its
  defaults, with the reason, and the refusal where lifelines returns numbers.
- One `CHANGELOG.md` entry under `### Lodestar.Survival`, `#### Added`.
- An ADR carrying the decision. **Take its number from `./.next-adr` at the repository root when
  the record is written, never from this line and never by listing `docs/decisions/`.** Listing the
  directory is what produced 0115, 0116 and 0117 each being claimed by another branch while a branch
  that had reserved them was open — three times in three days, costing a rename across every
  citation each time. `.next-adr` reads the working tree, `origin/main`, every other branch, every
  other worktree and every sibling clone matched by origin URL, which is the set a listing cannot
  see. It is untracked and in `.git/info/exclude`, so only `ls -a` at the root finds it.
- `samples/Lodestar.Sample` gains a member reference for each new public member. `Lodestar.Survival`
  is in `check_sample_coverage.py`'s `CONVERTED` list, so the packaging gate counts these — and it
  counts **members**, not types, which is the trap
  [#695](https://github.com/CyrilB1531/lodestar/pull/695) paid for by predicting otherwise.

## Not in scope

- **Schoenfeld residuals and `proportional_hazard_test`.** Their own issue, opened when this lands.
  The guide section above is what stands in the meantime, and it says so.
- **No prediction surface.** No `predict_partial_hazard`, no baseline hazard, no survival curve for
  a new covariate row. The linear predictor exists internally because concordance needs it; exposing
  it is a separate decision, and a baseline hazard is a third one with its own oracle and its own
  choice of estimator.
- **No strata, no time-varying covariates, no entry times, no weights, no cluster-robust variance.**
  Each is a parameter on lifelines' `fit` and each changes what is being estimated.
- **No Breslow ties**, for the measured reason above.
- **No penalizer.**
- **No version bump.** `Lodestar.Survival` sits at its tag; `check_unreleased.py` treats unpublished
  work as the normal state between a merge and a release.
