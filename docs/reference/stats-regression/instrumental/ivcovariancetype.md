# IvCovarianceType

How an instrumental-variables fit estimates the covariance of its coefficients: `linearmodels`'
four.

<!-- docs-declaration -->

```csharp
public enum IvCovarianceType
```

**Members** — `Unadjusted` assumes one variance for every error, `s²·(x̂ᵀx̂)⁻¹`. `Robust` is
White's estimator over the projected regressors, and the default. `Kernel` lets the errors be
correlated along the row order as well, weighting lags by [`IvOptions.Kernel`](ivoptions.md) up to
its bandwidth. `Clustered` lets errors correlate inside a cluster and treats clusters as
independent; it needs the overloads that take labels.

**Example** — the same fit under two covariances.

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

int[] clusters = [1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6];
IvSummary robust = InstrumentalVariables.TwoStageLeastSquares(design);
IvSummary clustered = InstrumentalVariables.TwoStageLeastSquares(
    design, clusters, new IvOptions { CovarianceType = IvCovarianceType.Clustered });

double robustError = robust.StandardErrors[2];        // => 0.0310248100…
double clusteredError = clustered.StandardErrors[2];  // => 0.0192193492…
```

**Remarks** — its own type rather than [`CovarianceType`](../ols/covariancetype.md), whose HC1 to
HC3 have no counterpart in the reference. **The clustered covariance takes its small-sample factor
`G/(G−1)·(n−1)/(n−k)` only when `Debiased` is set**, as the reference's does; `statsmodels` applies
the same factor by default. Under every choice the coefficients are read against the normal unless
`Debiased` is set, including `Unadjusted`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IvOptions`](ivoptions.md), [`KernelType`](../common/kerneltype.md).
