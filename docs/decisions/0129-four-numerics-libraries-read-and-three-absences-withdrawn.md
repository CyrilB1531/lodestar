---
status: accepted
supersedes: []
amends: ["0105", "0116"]
applies: ["0074", "0075"]
---
# 0129 — Four .NET numerics libraries read, and three absence claims withdrawn

**Status:** accepted · **Date:** 2026-09-14 · **Amends:** [0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md), [0116](0116-the-pca-gap-is-the-explained-variance-not-the-projection.md) · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)

## Context

[#676](https://github.com/CyrilB1531/lodestar/issues/676) listed five .NET numerics libraries this
repository had never read: NumFlat, Meta.Numerics, Numerics.NET, Extreme Optimization and
ILNumerics. [Decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) says a
gap is claimed on what a package exports, never on its README, and
[0096](0096-ordinary-least-squares-earns-its-own-package.md) and
[0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) each replaced
one of this project's own claims by doing so. The issue asked for the same reading here, recorded
even where the answer is uncomfortable.

It is uncomfortable. Three sentences this repository publishes are false as written.

## The reading

Read on **2026-09-14** with `tools/survey.cs`
([decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)); the
member count is the surveyor's first line, accessors excluded. Every licence was read from the
package, per [decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md).

```bash
dotnet run tools/survey.cs -- Meta.Numerics 4.2.0 'PrincipalComponent|LjungBox|Autocovar'
dotnet run tools/survey.cs -- Numerics.NET 10.7.0 'PartialAutocorrelation|Kpss|VarianceInflation'
dotnet run tools/survey.cs -- ILNumerics.Toolboxes.MachineLearning 7.4.62 'pca$'
dotnet run tools/survey.cs -- NumFlat 1.3.4 'EigenValues|Regression'
```

| | Meta.Numerics 4.2.0 | Numerics.NET 10.7.0 | ILNumerics 7.4.62 | NumFlat 1.3.4 |
| --- | --- | --- | --- | --- |
| published | **2025-07-14** | 2026-07-21 | 2026-08-25 | 2026-07-18 |
| licence, from the package | **MS-PL**, `<license type="expression">` | commercial: `License.md`, an ExoAnalytics Inc. agreement, `requireLicenseAcceptance` | commercial: `LICENSE.txt`, ILNumerics GmbH's agreement, whose free plan excludes commercial use; `requireLicenseAcceptance` | MIT, `<license type="expression">` |
| `lib/` | **`netstandard2.0`** | `net462`, **`netstandard2.0`**, `net8.0`, `net9.0`, `net10.0` | `net461`, `netstandard2.1`, `net8.0` | `net8.0` |
| assembly read | `Meta.Numerics.dll`: 177 types, 1,644 members | `Numerics.NET.dll`: 905 types, 13,839 members | `ILNumerics.Toolboxes.Statistics.dll`: 513 types, 3,644 members; `ILNumerics.Toolboxes.MachineLearning.dll`: 312 types, 2,070 members | `NumFlat.dll`: 123 types, 938 members, as 0116 recorded |

**The five are four.** `Numerics.NET`'s own package description reads *"formerly Extreme
Optimization Numerical Libraries for .NET"*, and `Extreme.Numerics` stopped at 8.1.25 on
2024-05-28. **And Meta.Numerics is not six years stale**: the issue saw 4.1.4 of 2020-08-21, and
4.2.0 followed on 2025-07-14.

### What each exports that this repository had called absent

| subject | Meta.Numerics | Numerics.NET | ILNumerics | NumFlat |
| --- | --- | --- | --- | --- |
| variance a principal component explains | `PrincipalComponent.VarianceFraction`, `.CumulativeVarianceFraction` | `PrincipalComponent.ProportionOfVariance`, `.CumulativeProportionOfVariance` | `MachineLearning.pca(A, outWeights, outCenter, outScores)` | `PrincipalComponentAnalysis.EigenValues` |
| autocorrelation | `TimeSeries.Autocovariance` | `TimeSeriesFunctions.AutocorrelationFunction` | none | none |
| partial autocorrelation | none | `TimeSeriesFunctions.PartialAutocorrelationFunction` | none | none |
| Ljung-Box | `TimeSeries.LjungBoxTest` | `Tests.LjungBoxTest` | none | none |
| stationarity | none | `AugmentedDickeyFullerTest`, `KpssTest` | none | none |
| hypothesis tests | `Univariate.StudentTTest`, `MannWhitneyTest`, `KruskalWallisTest`, `KolmogorovSmirnovTest`, `OneWayAnovaTest`, `ShapiroFranciaTest`; `Bivariate.WilcoxonSignedRankTest`; `BinaryContingencyTableOperations.FisherExactTest`; `ContingencyTable<,>.PearsonChiSquaredTest` | a `Statistics.Tests` namespace, Shapiro-Wilk, Anderson-Darling and Friedman among it | distributions and descriptive statistics, no test | none |
| regression inference | `Parameter.Estimate` as an `UncertainValue` (`Uncertainty`, `ConfidenceInterval`); `GeneralLinearRegressionResult.F`, `.RSquared`, `.Anova`. No adjusted R-squared, no VIF, and `Parameter` carries no test of its own | `Parameter<T>.StandardError`, `.Statistic`, `.PValue`, `.GetConfidenceInterval`; `RegressionModel<T>.AdjustedRSquared`, `.FStatistic`; `LinearRegressionModel.VarianceInflationFactors` | none | coefficients, intercept and prediction only |

Searched for by name and found in **none** of the five assemblies above: Kaplan-Meier, Nelson-Aalen,
the log-rank test, proportional hazards, censoring, conformal prediction, and mixed or random
effects. The only `Hazard` members are Meta.Numerics' distribution hazard functions.

## What is now false as written

1. **"Below `net8.0` nothing in .NET could say how many components a projection should keep."**
   [0116](0116-the-pca-gap-is-the-explained-variance-not-the-projection.md) said the explained
   variance *"has no incumbent on `netstandard2.0`"*, and
   [`docs/reference/decomposition/factorization.md`](../reference/decomposition/factorization.md),
   `docs/migration/sklearn.md`, `README.md`'s incumbent table and both benchmark pages repeated it.
   Meta.Numerics reports it on `netstandard2.0` under MS-PL, and Numerics.NET does under a
   commercial licence. 0116 read the two PCA incumbents a reader was being pointed at, and the
   sentence reached further than the reading.
2. **"No maintained .NET package carries" the time-series diagnostics.** 0105 said *"what nobody
   maintained ships is the apparatus for deciding whether a series may be modelled at all"*, and
   `docs/migration/statsmodels.md` said the same. Numerics.NET ships the autocorrelation and partial
   autocorrelation functions, Ljung-Box, ADF and KPSS, and published this July. Meta.Numerics ships
   the autocovariance and Ljung-Box.
3. **Accord.Statistics is "the one .NET library that did carry" hypothesis tests**, and there is
   *"nothing maintained to benchmark against"* — `docs/guides/hypothesis-testing.md`. Meta.Numerics
   carries eight of the ten families `Lodestar.Stats` ships, on `netstandard2.0`, and Numerics.NET more.

A fourth is false in a smaller way. `README.md` says the standard errors, t and p values, intervals,
adjusted R-squared, F test and VIF of a regression *"are in none of them"*. Numerics.NET exports
every one; Meta.Numerics exports the standard error, the interval and the F test.

## What still holds

- **Survival and conformal prediction have no .NET incumbent**, commercial ones included. `README.md`'s
  items 4 and 7 and [decision 0099](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)
  stand.
- **The free inference table is still incomplete.** Meta.Numerics stops before the adjusted
  R-squared, the VIF and a test per coefficient, which is what 0096 called the gap: *"a maintained,
  permissively licensed, framework-free OLS table"*. NumFlat returns coefficients only.
- **A free partial autocorrelation, stationarity test or seasonal decomposition** exists only in
  `Cortex.TimeSeries`, which 0105 already recorded and set aside.
- **NumFlat changes nothing 0116 did not already record.** Its PCA is measured
  ([#701](https://github.com/CyrilB1531/lodestar/issues/701)), its regression has no inference,
  and its four clustering algorithms against `Lodestar.Cluster`'s one are
  [#681](https://github.com/CyrilB1531/lodestar/issues/681)'s measurement. That answers #676's
  last criterion: NumFlat is already a measured incumbent for `Decomposition`, and #681 makes it
  one for `Cluster`.

## Decision

**Nothing already shipped is withdrawn, and every sentence above is corrected where it was
written.** 0105 and 0116 are amended on their absence claims; their decisions stand.

- **A commercial incumbent is named, not delegated to.** A migration row recommends what a reader
  can install without buying a licence, as 0105 did for `Dew.Stats`. Numerics.NET is now named
  beside each subject it covers, with its licence, so a reader who already holds one knows it
  answers the question.
- **Meta.Numerics is named as a free incumbent and delegated to for nothing yet.**
  `PrincipalComponentVariance`, `SerialCorrelation` and the `Lodestar.Stats` families stay. What
  they add over it is checkable rather than asserted: agreement with scikit-learn, `statsmodels`
  and `scipy` at `1e-9` through a frozen corpus, confidence bands and a partial autocorrelation it
  does not export, and an Apache-2.0 span API. What they do not add is availability below
  `net8.0`, and no document says so any more.
- **Meta.Numerics is measured before any number is claimed against it**:
  [#756](https://github.com/CyrilB1531/lodestar/issues/756) adds it to `bench/` for the explained
  variance and the hypothesis tests it shares with `Lodestar.Stats`.

## Options that lost

- **Deprecate `PrincipalComponentVariance` in favour of Meta.Numerics.** Had this reading preceded
  [0119](0119-the-explained-variance-lives-in-lodestar-decomposition.md), 0116's reopening
  condition might have been met by a migration row. It did not, and the type shipped in
  `Lodestar.Decomposition` 0.3.0 with a frozen scikit-learn corpus that Meta.Numerics has never been
  held to. Retiring it on a member name, before #756 has checked that the two agree, would repeat
  the mistake this record corrects in the opposite direction.
- **Correct only the ADR.** #676's third criterion refuses it, and so does the reason 0074 exists:
  a reader meets the claim in the migration page and the README, not in a decision record.
- **Leave the commercial libraries unnamed, since nothing is delegated to them.** An absence claim
  that is true only once commercial software is excluded has to say so, or it is false.

## Consequences

- `README.md`'s item 6 and its `Lodestar.Decomposition` and `Lodestar.Stats` incumbent rows name
  what these libraries export.
- The PCA pages — `docs/migration/sklearn.md`, `docs/reference/decomposition/factorization.md` and
  its `PrincipalComponentVariance` page, `docs/equivalence.md`'s projection row, `bench/README.md`
  section 30 and `docs/guides/performance.md` — name Meta.Numerics beside NumFlat and ML.NET.
  `docs/guides/hypothesis-testing.md` lists the three libraries that carry tests, and
  `docs/migration/statsmodels.md`'s hypothesis-test row stops pointing at Math.NET, which ships none.
- `docs/migration/statsmodels.md`'s time-series row splits and stops reading *being written*: #617
  shipped the serial-correlation half, and
  [#671](https://github.com/CyrilB1531/lodestar/issues/671) holds the rest, where Numerics.NET's
  ADF and KPSS are now on record.
- [#621](https://github.com/CyrilB1531/lodestar/issues/621) inherits one line of evidence: none of
  these four exports a mixed or random-effects member.
