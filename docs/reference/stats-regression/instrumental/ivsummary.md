# IvSummary

An instrumental-variables fit's inference table and diagnostics, as `linearmodels` reports them.

<!-- docs-declaration -->

```csharp
public sealed class IvSummary
```

**Properties** — the per-coefficient lists run constant, exogenous, endogenous: `Coefficients`,
`StandardErrors`, `TStatistics`, `PValues`, `ConfidenceLower` and `ConfidenceUpper`.
[`CovarianceType`](ivcovariancetype.md), `Debiased`, `Bandwidth` (the kernel's, as given or as
chosen, else `null`) and `ConfidenceLevel` echo how they were computed. `HasConstant` says whether
the regressors hold a constant, which centres `RSquared`; `AdjustedRSquared` and
`ResidualDegreesOfFreedom` follow. `ModelTest` is the joint test that every coefficient but the
constant is zero, an [`IvTest`](ivtest.md). `Kappa` is the `k`-class parameter, `null` for GMM.
`FirstStage` holds one [`IvFirstStage`](ivfirststage.md) per endogenous regressor.
`Overidentification` is Sargan's test for 2SLS and LIML and Hansen's J for GMM, `null` when the
model is just identified.

**Example** — the whole-model half of the table.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

double[] response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
double[] exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
double[] endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
double[] instruments =
    [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6,
     2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];

var design = new IvDesign(response, exogenous, 1, endogenous, 1, instruments, 2);

IvSummary summary = InstrumentalVariables.TwoStageLeastSquares(
    design, new IvOptions { Debiased = true });

double fit = summary.RSquared;                   // => 0.9961817721…
double overall = summary.ModelTest!.Statistic;   // => 1414.83778…
int? denominator = summary.ModelTest.DenominatorDegreesOfFreedom;  // => 9
double exogenousP = summary.PValues[1];          // => 0.3990498032…
```

**Remarks** — **p-values are the precise tail.** The reference computes `2 − 2·cdf`, which returns
zero below `1e-16`; this returns the tail itself, and the two agree to `1e-15` absolute. Under
`Debiased` the model test is an F and the coefficients read Student's t with
`ResidualDegreesOfFreedom`; otherwise a χ² and the normal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables`](../iv/instrumentalvariables.md), [`IvFirstStage`](ivfirststage.md).
