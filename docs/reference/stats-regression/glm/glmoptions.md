# GlmOptions

What a `GeneralizedLinearModel` fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record GlmOptions
```

**Properties** — `WithIntercept` prepends a column of ones; `true` by default. `ConfidenceLevel`
is the two-sided level the intervals are reported at; `0.95` by default. `MaximumIterations` is
how many IRLS iterations are allowed; `100` by default, which is the reference's own budget.
`Tolerance` is the absolute bound on the change in deviance between iterations — the reference's
`atol` with its `rtol` left at zero; `1e-8` by default. `ThrowOnNonConvergence` says whether a fit
that did not converge throws instead of returning; `true` by default.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, when `MaximumIterations` is below one, or when `Tolerance` is not above zero. Each is
thrown where the setting is set, not where the fit reads it: a budget of zero would otherwise skip
the IRLS loop entirely and reach the caller as a table of `0/0`.

**Example** — a wider level widens both ends without moving the estimate, the same way it does for
[`OlsOptions`](../ols/olsoptions.md).

```csharp
using Lodestar.Stats.Regression;

double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

GlmSummary ninetyFive = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Binomial);
GlmSummary ninetyNine = GeneralizedLinearModel.Fit(
    design, response, 1, GlmFamily.Binomial, new GlmOptions { ConfidenceLevel = 0.99 });

double narrow = ninetyFive.ConfidenceLower[1];  // => -0.5746058300782455
double wide = ninetyNine.ConfidenceLower[1];    // => -1.1366351826479073
```

**Remarks** — **turning `ThrowOnNonConvergence` off does not fix a bad fit; it lets you inspect
one.** A non-converged inference table is plausible and wrong — enormous standard errors and
p-values that read like p-values. Set it to `false` to freeze a non-convergent case in a corpus, or
to look at one, and read [`GlmSummary.Converged`](glmsummary.md) before anything else in the table
it returns.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md),
[`GlmSummary`](glmsummary.md).
