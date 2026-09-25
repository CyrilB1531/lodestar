# IvFirstStage

How strongly the instruments predict one endogenous regressor: a row of `first_stage.diagnostics`.

<!-- docs-declaration -->

```csharp
public sealed class IvFirstStage
```

**Properties** — `RSquared` is the endogenous regressor's R² on the exogenous regressors and the
instruments. `PartialRSquared` is the instruments' alone, once both sides are purged of the
exogenous regressors. `SheaRSquared` is Shea's partial R², which also accounts for the other
endogenous regressors. `InstrumentTest` is the joint test that the instruments' first-stage
coefficients are zero, an [`IvTest`](ivtest.md).

**Example** — two instruments that explain the endogenous regressor almost entirely.

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

IvFirstStage first = InstrumentalVariables.TwoStageLeastSquares(design).FirstStage[0];

double partial = first.PartialRSquared;               // => 0.9933…
double statistic = first.InstrumentTest.Statistic;    // => 2046.159…
```

**Remarks** — the first-stage regressions are reported under the fit's own covariance, **the
bandwidth it chose included**, as the reference's are. The instruments' test is an F under an
unadjusted covariance and a χ² under the others, whatever `Debiased` says.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IvSummary`](ivsummary.md).
