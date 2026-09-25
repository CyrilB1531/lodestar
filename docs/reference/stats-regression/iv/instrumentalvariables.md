# InstrumentalVariables

Instrumental-variables regression: two-stage least squares, LIML and two-step GMM, at
`linearmodels` parity.

<!-- docs-declaration -->

```csharp
public static class InstrumentalVariables
```

**Example** — one exogenous regressor, one endogenous, two instruments.

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

IvSummary summary = InstrumentalVariables.TwoStageLeastSquares(design);

double effect = summary.Coefficients[2];     // => 1.9054825408…
double error = summary.StandardErrors[2];    // => 0.0310248100…
double strength = summary.FirstStage[0].PartialRSquared;  // => 0.9933…
```

**Remarks** — the coefficients run as `linearmodels` reports them: the constant when
[`IvOptions.WithIntercept`](../instrumental/ivoptions.md) adds it, the exogenous regressors, then
the endogenous ones. **The default covariance is robust**, as the reference's `fit()` is, where
[`OrdinaryLeastSquares`](../ols/ordinaryleastsquares.md) defaults to the unadjusted one as
`statsmodels` does: each follows its own reference.

The three estimators share one design and one options record, and **an option the fit would not
read is refused rather than ignored**: [`IvOptions.Fuller`](../instrumental/ivoptions.md) outside
LIML, the `GmmWeight` options outside GMM, and a kernel or bandwidth without the kernel covariance
or weight that reads it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IvSummary`](../instrumental/ivsummary.md), [`IvOptions`](../instrumental/ivoptions.md),
the [instrumental-variables index](../iv.md).

## Members

| Member | What it does |
| --- | --- |
| [`InstrumentalVariables.TwoStageLeastSquares`](instrumentalvariables-twostageleastsquares.md) | Fits two-stage least squares. |
| [`InstrumentalVariables.Liml`](instrumentalvariables-liml.md) | Fits limited-information maximum likelihood, with Fuller's correction. |
| [`InstrumentalVariables.Gmm`](instrumentalvariables-gmm.md) | Fits two-step GMM with a chosen weight matrix. |
