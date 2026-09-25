# IvOptions

What an instrumental-variables fit estimates, and how it reports it.

<!-- docs-declaration -->

```csharp
public sealed record IvOptions
```

**Properties** — `WithIntercept` prepends a constant column to the exogenous regressors; `true` by
default, and it has no counterpart in the reference, where a constant is a column the caller
supplies. [`CovarianceType`](ivcovariancetype.md) is the covariance of the estimates; `Robust` by
default, the reference's. `Debiased` scales by `n/(n − k)` and reads the tests against t and F
rather than the normal and χ²; `false` by default. [`Kernel`](../common/kerneltype.md) and `Bandwidth` are a
kernel covariance's; Bartlett's and `null` by default, `null` choosing the bandwidth by Newey and
West's rule. `ConfidenceLevel` is the intervals' level, strictly inside (0, 1); 0.95 by default.
`Fuller` is LIML's `α`; 0 by default. `GmmWeightType`, `GmmWeightKernel` and
`GmmWeightBandwidth` choose GMM's second-step weight; robust, Bartlett's and `null`, which is
`n − 2` there.

**Example** — a kernel covariance at a chosen bandwidth.

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

IvSummary automatic = InstrumentalVariables.TwoStageLeastSquares(
    design, new IvOptions { CovarianceType = IvCovarianceType.Kernel });
IvSummary fixedLag = InstrumentalVariables.TwoStageLeastSquares(
    design, new IvOptions { CovarianceType = IvCovarianceType.Kernel, Bandwidth = 2 });

int? chosen = automatic.Bandwidth;                 // => 1
double error = fixedLag.StandardErrors[2];         // => 0.0232681079…
```

**Remarks** — the estimators check the options, not the record: a negative bandwidth, an undeclared
covariance or kernel, and a confidence level outside (0, 1) are refused by the fit, and so is **any
option that fit would not read** — a Fuller `α` on anything but LIML, a `GmmWeight` setting on
anything but GMM, a kernel or bandwidth without a kernel covariance or weight to read it. A Bartlett
or Parzen bandwidth of `n` or more is refused as the reference refuses it; the quadratic spectral
kernel reads every lag whatever its bandwidth.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables`](../iv/instrumentalvariables.md), [`IvSummary`](ivsummary.md).
