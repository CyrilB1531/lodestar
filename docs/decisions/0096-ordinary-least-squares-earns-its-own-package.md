---
status: accepted
supersedes: []
amends: []
applies: ["0074"]
---
# 0096 — Ordinary least squares earns its own package

**Status:** accepted · **Date:** 2026-09-10

## Context

[#566](https://github.com/CyrilB1531/lodestar/issues/566) asked for OLS **with inference** — not
the estimate, which is a solve, but the standard errors, t statistics, p-values, intervals,
adjusted R-squared, overall F and VIF a `summary()` table holds. It put two questions before any
code: whether that ships as a namespace inside `Lodestar.Stats` or as its own package, and whether
the gap it claims to fill is real.

The second question came with an instruction from
[#442](https://github.com/CyrilB1531/lodestar/issues/442) and
[decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md): read the
incumbent's exported surface through a `MetadataLoadContext`, not its README.

## The reading

`MathNet.Numerics` 5.0.0 (MIT), `lib/netstandard2.0`: **336 exported types, 5 333 members.**

Every regression entry point returns coefficients. `MultipleRegression.QR`, `.Svd`,
`.NormalEquations`, `.DirectMethod` return `Vector<T>`, `Matrix<T>` or `T[]`;
`SimpleRegression.Fit` returns a `(double, double)`; `WeightedRegression.Weighted` and `.Local` the
same shapes. `GoodnessOfFit` adds five whole-model scalars — `RSquared`, `R`,
`CoefficientOfDetermination`, `StandardError(modelled, observed, dof)` and
`PopulationStandardError`.

**Searching the assembly rather than that namespace found the nuance a README would have hidden.**
A coefficient covariance matrix does exist in MathNet:

```text
MathNet.Numerics.Optimization.NonlinearMinimizationResult
  Matrix<T> Correlation
  Matrix<T> Covariance
  Vector<T> StandardErrors
```

It belongs to the **non-linear** least-squares minimisers and is unreachable from
`LinearRegression`. Past it the chain stops even for its own callers: across 5 333 exported members
there is no t statistic, no p-value, no interval on a coefficient, no adjusted R-squared, no
overall F and no VIF.

`Accord.Statistics` 3.8.0: **561 exported types, 4 796 members, none unresolvable.**
`Analysis.MultipleLinearRegressionAnalysis` really does export the whole table — `StandardErrors`,
`Confidences`, `FTest`, `ZTest`, `RSquareAdjusted`, `InformationMatrix`, an ANOVA `Table`, and a
`Coefficients` collection whose row carries `Value`, `StandardError`, `TTest`, `ConfidenceLower`
and `ConfidenceUpper`. No VIF, in either assembly.

Only declarations were read; no method body of either assembly was opened, which is what
[decision 0003](0003-provenance-and-licensing.md) constrains.

**So #442's sentence — "nobody in .NET does inference" — is false as written, and this record
replaces it.** Accord did it, completely, and stopped: `accord-net/framework` is archived, last
release 2017-10-19, last push 2020-11-18, **LGPL-2.1**. MathNet, the maintained option, stops at
the point estimate. The gap is a *maintained, permissively licensed, framework-free* OLS table.

## Decision

**`Lodestar.Stats.Regression`, a new core-tier package**, rather than a namespace inside
`Lodestar.Stats`.

The audience is the argument. `Lodestar.Stats` is ten hypothesis-test families; a caller who wants
a regression table wants none of them, and a caller running a Kruskal-Wallis wants no QR. That is
the split [#427](https://github.com/CyrilB1531/lodestar/issues/427) asks for — *"a distinct
audience, or a distinct release cadence"* — and it is the trigger
[decision 0081](0081-the-stats-numerical-layer-stays-internal.md) wrote its own escape hatch for,
spent in [decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md).

Two edges, both to core packages, nothing external: `Lodestar.Stats` 0.2.0 for the Student and
Fisher tails, `Lodestar.Decomposition` 0.2.0 for the Householder QR. Core tier holds
([decision 0076](0076-a-core-package-carries-no-external-dependency.md)) — and would have failed
immediately had this taken Accord instead, whatever its license said.

**Solved through the QR, not the normal equations.** Forming `XᵀX` squares the condition number,
and the near-collinear designs a VIF exists to report are exactly the ones that costs. The
covariance is read as `σ²R⁻¹R⁻ᵀ`, whose diagonal is the row norms of `R⁻¹`.

## Two things parity turned out to mean

**statsmodels 0.15.0 changed what a VIF is.**
`stats.outliers_influence.variance_inflation_factor` now takes `standardize=True` by default: each
column is centred and scaled to unit spread before the auxiliary regression, with a `std > 1e-10`
mask that exempts the constant column, and the resulting R-squared is clipped to `1 - 1e-15`. With
an intercept this changes nothing — an affine change of the regressors leaves R-squared alone once
a constant absorbs the shift — and the corpus's four intercept cases passed before this was
implemented. **Without an intercept it is the whole answer**: 87.43 on the naive reading against
the reference's 21.0. Reproduced, because the corpus is frozen from the reference and a VIF that
disagrees with `statsmodels` by a factor of four is not a VIF.

**One divergence, deliberate.** A single regressor fitted with no intercept leaves the auxiliary
regression with an empty design, and statsmodels raises a `ValueError` out of numpy.
`OlsSummary.VarianceInflationFactors[0]` is `NaN` there instead. The rest of the table is well
defined — the estimate, its error, its p-value and its interval are all sound — and one undefined
diagnostic should not sink a fit that is otherwise correct. `docs/equivalence.md` carries the row.

## Consequences

- A thirteenth package, with the fixed cost #566 measured: five hard-coded pack lists across
  `ci.yml` and `sonarcloud.yml`, both release workflows' allow-lists, `Version.props`, the
  `EXPECTED` entry in `check_nuspec_dependencies.py`, two `FLOORS` rows in
  `check_version_floor.py`, the `*.NetStandard.Tests` mirror with all four `Lodestar.*` assemblies
  pinned by `ProjectReference` (#529), a `wiki-map.json` entry, reference pages, samples,
  `equivalence.md` rows and a guide.
- `statsmodels` 0.15.0 (BSD-3-Clause) joins `tools/requirements.txt` and the hashed lock, which
  grows by `pandas`, `patsy`, `formulaic` and their leaves. It fails
  [decision 0078](0078-keybert-is-declared-nodeps-not-compiled-into-the-lock.md)'s test for
  `requirements-nodeps.txt`, since pandas is not already on the lock's graph. The 126 existing
  corpora were regenerated against the new graph and are byte-identical.
- **A benchmark against a named incumbent, because the reading found one.** #566 expected this
  section to record that no .NET incumbent exists; the reading above says otherwise, so
  `bench/Lodestar.Stats.Benchmarks` measures against `Accord.Statistics`'
  `MultipleLinearRegressionAnalysis` instead. The plumbing is already there — section 18 of
  `bench/README.md` benchmarks `Lodestar.Stats` against the same package, and a benchmark project
  is not shipped, so the LGPL-2.1 that bars Accord from `src/` does not bar it from `bench/`.
  Being archived is a reason to prefer a maintained implementation, not a reason to skip the
  measurement. **The result inverts with size and is reported that way**: 2.7× faster at 100 rows,
  1.33× slower at 10 000, because Accord's table has no VIF and this one does — four regressors
  mean five Householder factorisations here against Accord's single solve. A caller who wants the
  table without the diagnostic has no way to say so today; that is the next measurement, not a
  defect of this decision.
- [#338](https://github.com/CyrilB1531/lodestar/issues/338) proposes GLMs and econometric
  summaries under the same name. This package is OLS and its table; anything with a link function
  or a time index falls on that issue's side of the line.
