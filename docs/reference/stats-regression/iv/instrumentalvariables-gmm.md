# InstrumentalVariables.Gmm

Fits two-step GMM, `IVGMM(y, exog, endog, instruments, weight_type=…).fit()`.

<!-- docs-declaration -->

```csharp
public static IvSummary Gmm(IvDesign design, IvOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static IvSummary Gmm(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
```

The second overload takes the labels a clustered covariance or a clustered weight reads.

**Parameters** — `design` is the response, the regressors and the instruments. `clusters` is one
label per row. `options` chooses the covariance, the second step's weight and the intercept;
`null` takes the reference's defaults, a robust weight among them.

**Returns** — an [`IvSummary`](../instrumental/ivsummary.md) whose `Overidentification` is
Hansen's J, and whose `Kappa` is `null`.

**Exceptions** — `ArgumentOutOfRangeException` when a column count is out of range.
`ArgumentException` when a block's length is not its column count times the rows, when there are
fewer instruments than endogenous regressors, when no residual degree of freedom is left, when the
options ask for a cluster covariance or weight without labels, when `Fuller` is set, or when the regressors or
instruments are collinear. `ArgumentNullException` when the clustered overload gets no `options`.

**Example** — the default robust weight, and Hansen's J.

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

IvSummary gmm = InstrumentalVariables.Gmm(design);

double effect = gmm.Coefficients[2];                 // => 1.88441634…
double j = gmm.Overidentification!.Statistic;        // => 5.4959041…
double p = gmm.Overidentification.PValue;            // => 0.019061068…
```

**Remarks** — the first step weights by `(ZᵀZ/n)⁻¹`, which is two-stage least squares; the second
by the inverse of the chosen score covariance at the first step's residuals — `iter_limit=2`, the
reference's default. Iterated GMM is not written: it moves between tolerances, so no corpus pins it.
A kernel weight or covariance without a bandwidth takes `n − 2` here, as the reference's
`KernelWeightMatrix` does, not Newey and West's rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables.TwoStageLeastSquares`](instrumentalvariables-twostageleastsquares.md),
[`IvOptions`](../instrumental/ivoptions.md).
