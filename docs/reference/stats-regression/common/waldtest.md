# WaldTest

A test statistic with its p-value and the degrees of freedom it is read against, shared by the
instrumental-variables and panel estimators.

<!-- docs-declaration -->

```csharp
public sealed record WaldTest(double Statistic, double PValue, int DegreesOfFreedom, int? DenominatorDegreesOfFreedom)
```

**Properties** — `Statistic` is the statistic. `PValue` is its upper-tail probability.
`DegreesOfFreedom` is the numerator's. `DenominatorDegreesOfFreedom` is the denominator's when the
statistic is an F, and `null` when it is a χ².

**Example** — Sargan's test on two instruments for one endogenous regressor.

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

WaldTest sargan = InstrumentalVariables.TwoStageLeastSquares(design).Overidentification!;

double statistic = sargan.Statistic;          // => 7.0786454…
int degrees = sargan.DegreesOfFreedom;        // => 1
bool chiSquared = sargan.DenominatorDegreesOfFreedom is null;  // => True
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IvSummary`](../instrumental/ivsummary.md), [`IvFirstStage`](../instrumental/ivfirststage.md).
