# statsmodels → .NET

**Verdict: decide.** The foundation (linear regression, distributions, basic
tests) exists, and **the OLS summary table is now native** — see the row below.
The rest of **rich econometrics** (detailed GLMs, ARIMA/SARIMAX, mixed models)
still has **no** good .NET equivalent, and remains a candidate for native code
*if your usage justifies it*.

| statsmodels need | .NET |
| --- | --- |
| Linear regression, least squares | **Math.NET Numerics** (`Fit`, `MultipleRegression`) — the estimate only |
| `OLS(...).fit()` with its summary table | **native**: [`Lodestar.Stats.Regression`](../reference/stats-regression/ols.md) |
| Distributions, basic hypothesis tests | **Math.NET** (`Distributions`). ⛔ *not* Accord.NET — last package October 2017, last commit November 2020; [README](README.md#unmaintained-and-why-that-is-stated-with-dates) |
| Advanced GLMs, time series, mixed models | ⚠️ **gap** — write or work around ([#338](https://github.com/CyrilB1531/lodestar/issues/338)) |

```csharp
using MathNet.Numerics;

// OLS y = a + b·x
(double a, double b) = Fit.Line(xs, ys);
double r2 = GoodnessOfFit.RSquared(xs.Select(x => a + b * x), ys);
```

## Pitfalls

- **Math.NET returns the estimate, not the inference.** Read through a
  `MetadataLoadContext`, every regression entry point in `MathNet.Numerics` 5.0.0
  returns coefficients, and `GoodnessOfFit` adds five whole-model scalars. Nothing
  in its 5 333 exported members is a coefficient p-value, a confidence interval, an
  adjusted R-squared, an overall F or a VIF. That gap is what
  [`Lodestar.Stats.Regression`](../reference/stats-regression/ols.md) fills, at
  `statsmodels` parity; [`decisions/0096`](../decisions/0096-ordinary-least-squares-earns-its-own-package.md)
  has the reading.
- **Time series.** Nothing equivalent to `SARIMAX`/`statespace`: either restrict
  the scope, or make it a native lot of its own.

> Before any native development beyond the OLS table, weigh the **real need**: a
> regression plus a few tests is often enough, and that much now ships.

*Guide to be expanded as real needs arise.*
