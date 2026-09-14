---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0095"]
---
# 0130 — Mixed models have no .NET incumbent, and are not written until a caller needs one

**Status:** accepted · **Date:** 2026-09-14 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)

## Context

[#338](https://github.com/CyrilB1531/lodestar/issues/338) had three subjects.
[Decision 0104](0104-generalized-linear-models-are-written-natively.md) took the link function
and [0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) the time
index. Mixed and hierarchical models were read by neither — 0104 says so in its own consequences —
and [#621](https://github.com/CyrilB1531/lodestar/issues/621) exists so that closing #338 did not
drop them: `statsmodels`' `MixedLM`, fixed and random effects, random intercepts and slopes,
nested and crossed grouping.

The issue put two things on record before anything was read. **The reading was owed** under
[decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), which twice found
the naive expectation wrong. **And the verdict was allowed to be no**: the subject is the hardest
of the three, and its audience is the narrowest.

## The searches

nuget.org, 2026-09-14:

| query | hits | anything that fits a mixed model |
| --- | ---: | --- |
| `mixed effects`, `mixed-effects` | 1 | none — an org-chart widget |
| `mixed model`, `linear mixed` | 33, 25 | none — mixed-integer programming solvers, and web or ORM libraries |
| `hierarchical model` | 41 | none — UI tree and pivot controls, and object-model libraries |
| `multilevel model`, `random effects`, `repeated measures`, `hierarchical linear`, `bayesian hierarchical` | 0 | — |
| `lme4`, `MixedLM`, `glmm` | 0 | — |
| `REML` | 1 | none — a Roslyn analysis server |
| `variance components` | 1 | `Dew.Stats` 6.3.10, whose description matches on *"variance and covariance analysis"* and names no mixed model |
| `anova` | 7 | `Accord.Statistics`, `CenterSpace.NMath.Stats`, `Dew.Stats` — read below |

## The reading

Every package below was read with `tools/survey.cs`
([decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)) against
one pattern, and every licence from the package, per
[decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md):

```bash
dotnet run tools/survey.cs -- <package> <version> \
  '(Mixed|RandomEffect|FixedEffect|VarianceComponent|Reml|Multilevel|Hierarch|RandomIntercept|RandomSlope|Satterthwaite|KenwardRoger)'
```

| package | published | licence, from the package | surface | what the pattern returns |
| --- | --- | --- | --- | --- |
| `Accord.Statistics` 3.8.0 | 2017-10-19 | LGPL-2.1 | 561 types, 5,620 members | `TwoWayAnovaModel.Mixed` — a two-way ANOVA with one random factor, by mean squares |
| `MathNet.Numerics` 5.0.0 | 2022-04-03 | MIT | 336 types, 5,707 members | two mixed *partial derivatives* |
| `Meta.Numerics` 4.2.0 | 2025-07-14 | MS-PL | 177 types, 1,644 members | nothing |
| `NumFlat` 1.3.4 | 2026-07-18 | MIT | 123 types, 938 members | nothing |
| `Numerics.NET` 10.7.0 | 2026-07-21 | commercial | 905 types, 13,839 members | `HierarchicalClusterAnalysis` and an MKL loader; its `AnovaModel` has no random factor |
| `ILNumerics.Toolboxes.Statistics`, `.MachineLearning` 7.4.62 | 2026-08-25 | commercial | 513 + 312 types | nothing ([decision 0129](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)) |
| `CenterSpace.NMath.Standard.Linux.X64` 8.0.0.39, assembly `NMath` | 2026-09-10 | commercial, `license.txt` | 681 types, 10,698 members | mixed-integer programming; with `Anova` added, `OneWayRanova` and `TwoWayRanova` — repeated-measures ANOVA, by mean squares |
| `Dew.Stats.Core` 6.3.10 | 2025-12-15 | commercial, `DewStatsLicense.txt` | 51 types, 727 members | nothing; with `Anova` added, `ANOVA1` and `ANOVA2` |
| `Microsoft.ML.Probabilistic` 0.4.2504.701 (Infer.NET) and `.Compiler` | 2025-04-07 | MIT | 700 + 251 types, 8,173 + 1,734 members | nothing |
| `Microsoft.ML` 5.0.0 | 2025-11-11 | MIT | read for 0104 on 2026-09-11 | no random-effects concept |

`Dew.Stats.Core` is the managed edition of `Dew.Stats`; the Windows edition was not loaded, and its
description names ANOVA, ANCOVA and no mixed model.

**Nothing in .NET fits a linear mixed model**, free or commercial. The nearest things are classical
ANOVA with a random or repeated factor — Accord, NMath — which is the balanced-design special case
a mixed model exists to replace. Infer.NET is the one honest caveat: its `Variable` API can
*express* a hierarchical model by hand, a Gaussian per group drawn from a Gaussian over groups, and
infers it by message passing. That is a Bayesian modelling language, not an estimator with
statsmodels' output, and it is named so it is not mistaken for an absence.

So the gap is real, and wider than any 0074 has recorded: 0096 and 0105 each found the computation
somewhere and the apparatus missing. Here neither exists.

## What parity would mean

`statsmodels` 0.15.0 is already in `tools/requirements.lock.txt`, and one fit settles more than
any argument. A random intercept and a random slope over 20 groups of 10, fitted by REML:

```text
              Coef. Std.Err.   z    P>|z| [0.025 0.975]
Intercept     1.318    0.228  5.774 0.000  0.871  1.766
x             1.879    0.080 23.583 0.000  1.723  2.036
Group Var     0.927    0.354
Group x x Cov 0.002    0.102
x Var         0.000    0.119
```

Two things follow.

**The inference is `z`, not Satterthwaite or Kenward-Roger.** `statsmodels` reports Wald tests
against the normal distribution. Across its source, Kenward-Roger appears nowhere and Satterthwaite
only in `stats/oneway.py`, `stats/weightstats.py`, `stats/multicomp.py` and
`stats/nonparametric.py` — the unequal-variance corrections of classical tests. The part #621 feared
most is not part of this reference: parity with `MixedLM` needs REML or ML, the profiled
likelihood and its Hessian, and nothing from `lmerTest`.

**The estimate belongs to the optimizer, and the oracle gate cannot hold it.** The slope's variance
sits on its boundary, and `statsmodels` warns *"The MLE may be on the boundary of the parameter
space"*. The same data refitted with each `method` it accepts:

| `method` | intercept | `x` | REML log-likelihood | warning |
| --- | ---: | ---: | ---: | --- |
| `lbfgs`, the default | 1.3184803 | 1.8794272 | −319.141886 | on the boundary |
| `powell` | 1.3184989 | 1.8794401 | −319.138960 | on the boundary |
| `nm` | 1.3184784 | 1.8795097 | −319.139331 | failed to converge |
| `bfgs`, `cg` | 1.2991528 | 1.8402714 | −335.284634 | failed to converge |

The default does not reach the best likelihood `statsmodels` itself finds on this data, and two
solvers stop 16 log-likelihood units away. The oracle gate compares at `1e-9`
([decision 0073](0073-the-oracle-gate-compares-numbers-not-bytes.md)); here the reference disagrees
with itself at `1e-5` on a coefficient and `3e-3` on the likelihood. A corpus would freeze one
optimizer's path, and a correct implementation reaching a better optimum would fail it.

Moving away from the boundary narrows the disagreement without closing it. A random intercept alone,
over 30 groups of 10 with a group variance near 1.7, converges under all five solvers with no
warning, and they agree on the REML log-likelihood to `3e-8`, on the coefficients to `2e-7`, and on
the group variance only to `1.4e-4` (`1.73992` to `1.74006`): the likelihood is flat along the
variance, so every solver stops where its own tolerance says the surface is level. A variance
shrinking to zero is the ordinary case, not an edge, and even the interior one is not pinned to
`1e-9` by the reference's defaults.

## Decision

**Mixed models are not written now.** The void is recorded, scoped, and left for a caller, under
[decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s rule that
what ships is what a caller needs rather than what a gap permits.

It is not refused forever, and the reason is not that .NET has an answer — it has none. It is that
the lot would be the largest in `Lodestar.Stats.Regression`, its audience is the narrowest of
the three subjects in #338, and its conformance cannot be proven the way everything else in this repository is
proven. Writing it on the strength of the gap alone would ship the one estimator here whose
numbers no corpus stands behind.

**What reopens it:** a caller who needs `MixedLM`'s table from .NET. The lot it would be is
written down so it does not have to be rediscovered:

- linear mixed models only — random intercepts and slopes over one grouping factor, variance
  components for nested ones — by REML and ML on the profiled likelihood, with `statsmodels`'
  `z`-based table;
- **a conformance method decided before any code**: the log-likelihood held to the best the
  reference's solvers reach, and the parameters to the tolerance that likelihood's curvature
  allows rather than to one solver's stopping point — a divergence from the `1e-9` gate that
  `docs/equivalence.md` and a decision would both have to carry;
- generalized linear mixed models, crossed random effects beyond variance components, and
  Satterthwaite or Kenward-Roger degrees of freedom out of scope: `statsmodels` fits the first
  only by Bayesian approximation (`BinomialBayesMixedGLM`, `PoissonBayesMixedGLM`), and names the
  last only for one-way ANOVA and two-sample tests, never for a mixed model;
- placement answered then, against
  [decision 0076](0076-a-core-package-carries-no-external-dependency.md) and the two-level naming
  rule. `Lodestar.Stats.Regression` is the obvious candidate and the answer is not taken here,
  because nothing is being placed.

## Options that lost

- **Write it as the next lot.** The gap is genuine, and this project's thesis is to write where
  .NET is empty. It lost on the conformance table above, not on effort: an estimator whose
  reference disagrees with itself cannot be held to the tolerance every other estimator here
  meets, and choosing a weaker one belongs to the lot that has a caller to judge it against.
- **Delegate to Infer.NET.** First-party and MIT, and it can express the model. It cannot print
  the table: no REML, no standard errors from the observed information, no `z` test. A migration
  row sending a `MixedLM` user to a message-passing compiler would be a row that fails them.
- **Delegate to a commercial ANOVA.** NMath's repeated-measures ANOVA answers the balanced design
  a mixed model generalises, and nothing more. Naming it is a courtesy to a reader with that
  design; delegating to it would be a claim it does not meet.

## Consequences

- `docs/migration/statsmodels.md`'s mixed-models row stops reading *"the only one still unread"*:
  it reads **no .NET package**, with this record, the Infer.NET and repeated-measures caveats, and
  the reopening condition.
- #621 closes with this record. #338 loses its last unread subject.
