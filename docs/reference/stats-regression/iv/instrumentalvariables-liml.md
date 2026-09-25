# InstrumentalVariables.Liml

Fits limited-information maximum likelihood, `IVLIML(y, exog, endog, instruments, fuller=α).fit()`.

<!-- docs-declaration -->

```csharp
public static IvSummary Liml(IvDesign design, IvOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static IvSummary Liml(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
```

The second overload is the cluster-robust fit, as for
[`InstrumentalVariables.TwoStageLeastSquares`](instrumentalvariables-twostageleastsquares.md).

**Parameters** — `design` is the response, the regressors and the instruments. `clusters` is one
label per row. `options` chooses the covariance, Fuller's `α` and the intercept; `null` takes the
reference's defaults.

**Returns** — an [`IvSummary`](../instrumental/ivsummary.md) whose `Kappa` is the `k`-class
parameter the fit used.

**Exceptions** — `ArgumentOutOfRangeException` when a column count is out of range.
`ArgumentException` when a block's length is not its column count times the rows, when there are
fewer instruments than endogenous regressors, when no residual degree of freedom is left, when the
options ask for a cluster covariance or weight without labels, when `Fuller` is not finite, or when the regressors or
instruments are collinear. `ArgumentNullException` when the clustered overload gets no `options`.

**Example** — LIML, then Fuller's correction at `α = 1`.

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

IvSummary liml = InstrumentalVariables.Liml(design);
IvSummary fuller = InstrumentalVariables.Liml(design, new IvOptions { Fuller = 1.0 });

double kappa = liml.Kappa!.Value;          // => 2.4365392…
double lowered = fuller.Kappa!.Value;      // => 2.3115392…
double effect = liml.Coefficients[2];      // => 1.9021995…
```

**Remarks** — `κ` is the smallest eigenvalue of the LIML problem: `E = [y, X_endog]` purged of
the exogenous regressors, against the same purged of the instruments. Fuller lowers it by
`α/(n − L)`, here `1/8`. A just-identified model has `κ = 1` and fits two-stage least squares
exactly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables.TwoStageLeastSquares`](instrumentalvariables-twostageleastsquares.md),
[`IvSummary`](../instrumental/ivsummary.md).
