# CovarianceType

How [`OrdinaryLeastSquares`](ordinaryleastsquares.md) estimates the covariance of its estimates.

<!-- docs-declaration -->

```csharp
public enum CovarianceType
```

**Members** — `Nonrobust` is one variance for every error, `σ²(XᵀX)⁻¹`, and the default. `Hc0` is
White's estimator, weighting each row by its squared residual. `Hc1` scales `Hc0` by `n / (n - k)`.
`Hc2` divides each squared residual by `1 - hᵢᵢ`, the row's own leverage, and `Hc3` by
`(1 - hᵢᵢ)²` — the jackknife approximation, and the conservative one.

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

double slope = ordinary.Coefficients[1];        // => 2.1052747252747257
double same = robust.Coefficients[1];           // => 2.1052747252747257
double confident = ordinary.StandardErrors[1];  // => 0.10685448183492333
double honest = robust.StandardErrors[1];       // => 0.14211839172191723
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

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OlsOptions`](olsoptions.md), [`OlsSummary`](olssummary.md),
[`OrdinaryLeastSquares.Fit`](ordinaryleastsquares-fit.md).
