# KernelType

The lag window a kernel covariance weights its autocovariances with: the instrumental-variables
kernel covariance and weight, and the panels' Driscoll-Kraay covariance.

<!-- docs-declaration -->

```csharp
public enum KernelType
```

**Members** — `Bartlett` is Newey and West's triangle, `1 − j/(m+1)`, and the default. `Parzen` is
Gallant's cubic window, truncated at the bandwidth. `QuadraticSpectral` is Andrews' window, which
weights every lag in the sample rather than stopping at the bandwidth.

**Example** — the automatic bandwidth depends on the kernel.

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

IvSummary bartlett = InstrumentalVariables.TwoStageLeastSquares(
    design, new IvOptions { CovarianceType = IvCovarianceType.Kernel });

int? lag = bartlett.Bandwidth;   // => 1
```

**Remarks** — with no bandwidth given, 2SLS and LIML choose one by Newey and West's (1994) rule on
the scores summed over every column but the constant; GMM takes `n − 2`, as the reference does. A
quadratic spectral window costs a pass per lag of the sample, so it is quadratic in the rows.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IvCovarianceType`](../instrumental/ivcovariancetype.md), [`IvOptions`](../instrumental/ivoptions.md).
