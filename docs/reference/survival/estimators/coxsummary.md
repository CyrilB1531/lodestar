# CoxSummary

What a Cox proportional hazards fit reports, at `lifelines` parity.

<!-- docs-declaration -->

```csharp
public sealed class CoxSummary
```

**Properties** — the per-covariate lists are parallel and in the design's column order:

- `Coefficients` are the log hazard ratios.
- `StandardErrors` are the square roots of the inverse observed information's diagonal.
- `ZStatistics` is each coefficient over its standard error.
- `PValues` are two-sided, from the normal tail.
- `ConfidenceLower` and `ConfidenceUpper` are each coefficient's interval at `ConfidenceLevel`.
- `HazardRatios`, `HazardRatioLower` and `HazardRatioUpper` are the exponentials of the three above.

For the model:

- `LogLikelihood` is the log partial likelihood at the fit, and `NullLogLikelihood` the same with
  every coefficient zero.
- `LikelihoodRatioStatistic` is twice their difference, and `LikelihoodRatioPValue` its chi-squared
  tail on `LikelihoodRatioDegreesOfFreedom` degrees, one per covariate.
- `ConcordanceIndex` is Harrell's C.
- `ConfidenceLevel` is the level the intervals were built at.

**Example** — the model as a whole, beside one covariate.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary summary = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);

double treatmentP = summary.PValues[1];                      // => 0.14391…
double modelP = summary.LikelihoodRatioPValue;               // => 0.05068…
double concordance = summary.ConcordanceIndex;               // => 0.77380952380952…
int degrees = summary.LikelihoodRatioDegreesOfFreedom;       // => 2
```

**Remarks** — **a hazard ratio is per unit of its covariate.** Above, 4.07 is per milligram of dose,
so the same data coded in grams reads 4.07 to the thousandth power. The p-value does not change with
units; the ratio does.

**The concordance counts a pair of identical linear predictors as one half**, as Harrell's rule
and lifelines' own documentation state. lifelines computes the partial hazard with row-dependent
rounding, so two subjects with identical covariates can compare strictly there. On the ten patients
above it reports 0.7857 where the rule gives 0.7738. The two identical rows are the sixth and the
tenth.

There is **no iteration count**: the reference reaches its answer under a different stopping rule,
so no oracle could check one, and a fit that did not converge throws rather than returning. A class
rather than a record, as `OlsSummary` is: two fitted tables are not values a caller compares.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxProportionalHazards.Fit`](coxproportionalhazards-fit.md).
