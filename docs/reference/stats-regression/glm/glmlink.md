# GlmLink

The link a `GeneralizedLinearModel` maps its linear predictor to the mean through.

<!-- docs-declaration -->

```csharp
public enum GlmLink
```

**Members** — `Default` is each family's statsmodels default: logit for `Binomial`, log for `Poisson`
and `NegativeBinomial`, inverse for `Gamma`. `Log` is `η = log μ`. `Inverse` is `η = 1/μ`, statsmodels'
`InversePower`.

**Example** — the same positive, skewed response through Gamma's two links: the log link's slope reads
as a growth rate, and it estimates a smaller dispersion here than the inverse link does.

```csharp
using Lodestar.Stats.Regression;

double[] x = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0];
double[] cost = [2.1, 1.8, 3.5, 2.9, 4.8, 5.5, 4.9, 7.8, 6.9, 9.4];

GlmSummary inverse = GeneralizedLinearModel.Fit(x, cost, 1, GlmFamily.Gamma);
GlmSummary log = GeneralizedLinearModel.Fit(x, cost, 1, GlmFamily.Gamma, new GlmOptions { Link = GlmLink.Log });

double growth = Math.Round(log.Coefficients[1], 4);            // => 0.1718
double inverseDispersion = Math.Round(inverse.Dispersion, 4);  // => 0.0526
double logDispersion = Math.Round(log.Dispersion, 4);          // => 0.0317
```

**Remarks** — **not every family takes every link here.** `Gamma` takes `Inverse` or `Log`; the two count
families take `Log`, their default; `Binomial` takes only its logit. Any other pairing is refused by
[`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md) on `options`, rather than fitted.

Under `Inverse`, nothing keeps `1/η` positive, which is why statsmodels warns when it builds that link.
A Gamma fit whose mean reaches zero or below is refused at the iteration it happens, where the reference
carries on with `|μ|` in its variance; `Log` keeps every mean positive.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GlmFamily`](glmfamily.md), [`GlmOptions`](glmoptions.md).
