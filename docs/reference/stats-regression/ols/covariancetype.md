# CovarianceType

How [`OrdinaryLeastSquares`](ordinaryleastsquares.md) estimates the covariance of its estimates.

<!-- docs-declaration -->

```csharp
public enum CovarianceType
```

**Members** — `Nonrobust` is one variance for every error, `σ²(XᵀX)⁻¹`, and the default. `Hc0` is
White's estimator, weighting each row by its squared residual. `Hc1` scales `Hc0` by `n / (n - k)`.
`Hc2` divides each squared residual by `1 - hᵢᵢ`, the row's own leverage, and `Hc3` by
`(1 - hᵢᵢ)²` — the jackknife approximation, and the conservative one. `Hac` is Newey and West's
estimator for errors correlated along the row order: the scores of rows up to
[`HacLags`](olsoptions.md) apart enter with Bartlett weights `1 - l / (L + 1)`. `Cluster` lets errors
correlate inside a cluster and treats clusters as independent; it needs the
[`Fit` overload that takes labels](ordinaryleastsquares-fit.md).

**Example** — a response whose spread grows with the regressor is what these exist for.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0];
double[] response =
    [2.1, 4.3, 5.7, 8.4, 9.6, 13.1, 13.4, 17.9, 17.2, 22.8, 20.9, 26.4, 24.1, 31.6];

OlsSummary ordinary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);
OlsSummary robust = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1,
    new OlsOptions { CovarianceType = CovarianceType.Hc3 });

double slope = ordinary.Coefficients[1];        // => 2.1052747252747253
double same = robust.Coefficients[1];           // => 2.1052747252747253
double confident = ordinary.StandardErrors[1];  // => 0.10685448183492335
double honest = robust.StandardErrors[1];       // => 0.14211839172191734
```

**Returns** — the estimate does not move. `Hc3` reports a standard error **33% larger** on the
slope above, which is the ordinary one having been too sure of itself.

**Remarks** — **choosing anything but `Nonrobust` changes the distribution the tests are read
against**, because `statsmodels` does the same. [`OlsSummary.TStatistics`](olssummary.md) becomes
*z* against the normal rather than *t* against Student's,
[`OlsSummary.PValues`](olssummary.md) and the interval multiplier follow it, and
[`OlsSummary.CovarianceType`](olssummary.md) is what says which was used.

That has a consequence worth seeing before it surprises you. On the data above the ordinary
p-value for the slope is `1.66e-10` and the robust one is `1.20e-49` — **smaller, on a wider
standard error.** Nothing is wrong: the wider error divides a *z* of 14.8, and the normal tail
there is far thinner than Student's on twelve degrees of freedom. A robust covariance is not a
more cautious p-value, it is a differently derived one.

[`OlsSummary.FPValue`](olssummary.md) stays on the F distribution either way, even though the
coefficient tests move. That asymmetry is statsmodels' and is reproduced rather than tidied.

**Rows in time order** — `Hac` is the one that reads the order of the rows.

```csharp
using Lodestar.Stats.Regression;

double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0,
                   11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0, 20.0];
double[] response = [2.43, 3.16, 4.05, 4.97, 5.77, 4.73, 5.77, 7.8, 8.67, 9.68,
                     9.7, 10.86, 11.12, 10.9, 11.98, 14.32, 15.33, 14.54, 15.6, 19.15];

OlsSummary white = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1, new OlsOptions { CovarianceType = CovarianceType.Hc0 });
OlsSummary neweyWest = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1,
    new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 2 });
OlsSummary none = OrdinaryLeastSquares.Fit(
    design, response, featureCount: 1,
    new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 0 });

double hc0 = Math.Round(white.StandardErrors[1], 6);         // => 0.03883
double lagged = Math.Round(neweyWest.StandardErrors[1], 6);  // => 0.032722
double zero = Math.Round(none.StandardErrors[1], 6);         // => 0.03883
```

With no lags HAC is HC0, and the lags are what it adds. They can move the standard error either way:
a lag's cross-products are as signed as the residuals they multiply. `Hac` applies no
degrees-of-freedom correction by default and `Cluster` applies `G / (G - 1) · (n - 1) / (n - k)`;
[`SmallSampleCorrection`](olsoptions.md) switches either, as statsmodels' `use_correction` does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsOptions`](olsoptions.md), [`OlsSummary`](olssummary.md),
[`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md).
