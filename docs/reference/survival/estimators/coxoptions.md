# CoxOptions

What a `CoxProportionalHazards` or `CoxTimeVarying` fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record CoxOptions
```

**Properties** — `ConfidenceLevel` is the two-sided level the intervals are reported at; `0.95` by
default. `MaximumIterations` is how many Newton-Raphson iterations are allowed; `100` by default,
and not read by an L1 fit, which keeps lifelines' own limits of 500 steps, 50 for the time-varying
fit.
`Penalizer` is lifelines' `penalizer`, zero by default, and `L1Ratio` its `l1_ratio`, the share of
the penalty that is L1, zero by default. `Robust` asks for the Huber sandwich standard errors,
lifelines' `robust=True`; clusters passed to the fit ask for it too.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, when `MaximumIterations` is below one, when `Penalizer` is negative or not finite, or when
`L1Ratio` lies outside `[0, 1]`. Each is thrown where the setting is set, not
where the fit reads it.

**Example** — a narrower level moves both ends inward without moving the estimate.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary ninetyFive = CoxProportionalHazards.Fit(design, months, died, 2);
CoxSummary ninety = CoxProportionalHazards.Fit(
    design, months, died, 2, new CoxOptions { ConfidenceLevel = 0.9 });

double wide = ninetyFive.ConfidenceLower[0];  // => 0.16213876109…
double narrow = ninety.ConfidenceLower[0];    // => 0.36168950449…
```

**Remarks** — **there is no intercept.** The baseline hazard absorbs what an intercept would carry,
so a column of ones is collinear with it and is refused.

**The penalty is lifelines' elastic net on the standardised coefficients**,
`n · penalizer · Σ (l1 · |βⱼσⱼ| + (1 − l1) · (βⱼσⱼ)² / 2)` with `σⱼ` the covariate's sample standard
deviation, so it does not depend on the units a covariate is recorded in. **A ridge penalty,
`L1Ratio = 0`, is fitted to its optimum**, as the unpenalised fit is. **With an L1 part the answer is
lifelines' own loop**, reproduced step for step: the absolute value is smoothed and sharpened at every
Newton step, so the answer is where lifelines' stopping rules fire rather than a stationary point, and
its default and tightened fits differ by a factor of 29 (#1160). `LogLikelihood` is the penalised one,
as lifelines' `log_likelihood_` is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxProportionalHazards.Fit`](coxproportionalhazards-fit.md),
[`CoxSummary`](coxsummary.md).
