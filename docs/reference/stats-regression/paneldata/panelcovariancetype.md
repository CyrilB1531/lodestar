# PanelCovarianceType

How a panel fit estimates the covariance of its coefficients: `linearmodels`' panel four.

<!-- docs-declaration -->

```csharp
public enum PanelCovarianceType
```

**Members** — `Unadjusted` assumes one variance for every error, and is the default. `Robust` is
White's estimator. `Clustered` lets errors correlate inside a cluster — the entity, the period, the
caller's labels, or two of them at once — and treats clusters as independent. `Kernel` is Driscoll
and Kraay's: the scores summed by period, then weighted across periods by
[`PanelOptions.Kernel`](paneloptions.md), so errors may correlate across entities and along time.

**Example** — the same fit under two covariances.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary robust = PanelRegression.FixedEffects(
    design, new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Robust });
PanelSummary clustered = PanelRegression.FixedEffects(
    design, new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Clustered, ClusterEntity = true });

double robustError = robust.StandardErrors[1];        // => 0.0412274910…
double clusteredError = clustered.StandardErrors[1];  // => 0.0533951997…
```

**Remarks** — **the clustered covariance takes no `G/(G−1)` factor**, as the reference's does not;
two-way clustering is `S₀ + S₁ − S₀₁`, which need not be positive definite. Every covariance scales
by `n/(n − effects − k)` when debiased and `n/(n − effects)` otherwise.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelOptions`](paneloptions.md), [`KernelType`](../common/kerneltype.md).
