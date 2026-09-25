# InstrumentalVariables.TwoStageLeastSquares

Fits two-stage least squares, `IV2SLS(y, exog, endog, instruments).fit()`.

<!-- docs-declaration -->

```csharp
public static IvSummary TwoStageLeastSquares(IvDesign design, IvOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static IvSummary TwoStageLeastSquares(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
```

The second overload is the cluster-robust fit: its `options` must ask for
[`IvCovarianceType.Clustered`](../instrumental/ivcovariancetype.md), and no other covariance reads the labels.

**Parameters** — `design` is the response, the exogenous and endogenous regressors and the
excluded instruments, as an [`IvDesign`](../instrumental/ivdesign.md). `clusters` is one label per
row, any integers, naming at least two clusters. `options` chooses the covariance, its kernel and
scaling, and the intercept; `null` takes the reference's defaults.

**Returns** — an [`IvSummary`](../instrumental/ivsummary.md): the table, R², the model test, the
first-stage diagnostics and Sargan's overidentification test.

**Exceptions** — `ArgumentOutOfRangeException` when a column count is out of range.
`ArgumentException` when a block's length is not its column count times the rows, when there are
fewer instruments than endogenous regressors, when no residual degree of freedom is left, when the
options ask for a cluster covariance without labels or set a value this estimator does not read, or
when the regressors or instruments are collinear. `ArgumentNullException` when the clustered
overload gets no `options`.

**Example** — the robust default, and the unadjusted covariance beside it.

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

IvSummary robust = InstrumentalVariables.TwoStageLeastSquares(design);
IvSummary unadjusted = InstrumentalVariables.TwoStageLeastSquares(
    design, new IvOptions { CovarianceType = IvCovarianceType.Unadjusted });

double robustError = robust.StandardErrors[2];          // => 0.0310248100…
double unadjustedError = unadjusted.StandardErrors[2];  // => 0.0345804417…
double sargan = robust.Overidentification!.Statistic;   // => 7.0786454…
```

**Remarks** — the estimate is `(X̂ᵀX̂)⁻¹X̂ᵀy` with `X̂ = P_zX`, and the covariance is
`V⁻¹SV⁻¹/n` over the projected regressors, `S` the chosen estimator's. Sargan's test above rejects
the two instruments' agreement at 1%: one of them, at least, is not excluded from the response.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables.Liml`](instrumentalvariables-liml.md),
[`InstrumentalVariables.Gmm`](instrumentalvariables-gmm.md), [`IvOptions`](../instrumental/ivoptions.md).
