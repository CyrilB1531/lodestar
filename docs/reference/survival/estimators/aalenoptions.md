# AalenOptions

What an [`AalenAdditive`](aalenadditive.md) fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record AalenOptions
```

**Properties** — `ConfidenceLevel` is the two-sided level the cumulative coefficients' bounds are
reported at; `0.95` by default. `FitIntercept` is lifelines' `fit_intercept`: whether a baseline
column of ones is added after the covariates, `true` by default. `CoefficientPenalizer` is lifelines'
`coef_penalizer`, a ridge penalty on each time's increments, zero by default. `SmoothingPenalizer` is
lifelines' `smoothing_penalizer`, which pulls each time's increments towards the previous time's,
zero by default.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, or when either penalizer is negative, infinite or `NaN`. Each is thrown where the setting
is set, not where the fit reads it.

**Example** — a ridge penalty shrinks the treatment's cumulative coefficient at the last event time.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary plain = AalenAdditive.Fit(treated, months, died, 1);
AalenSummary ridge = AalenAdditive.Fit(
    treated, months, died, 1, new AalenOptions { CoefficientPenalizer = 1.0 });

int last = (plain.EventTimes.Count - 1) * 2;
double unpenalised = Math.Round(plain.CumulativeHazards[last], 6);   // => -2.083333
double penalised = Math.Round(ridge.CumulativeHazards[last], 6);     // => -0.643275
```

**Remarks** — **both penalties act on the scaled covariates**, each at unit sample deviation, so
they do not depend on the units a covariate is recorded in. At each event time the two add to the
diagonal of the normal equations together, and the smoothing penalty adds its share of the previous
time's increments to the right-hand side: at `CoefficientPenalizer = 0`, a large
`SmoothingPenalizer` holds the increments near their previous values, a small one leaves each time
to its own deaths. The intercept is penalised with the rest.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenAdditive.Fit`](aalenadditive-fit.md), [`AalenSummary`](aalensummary.md).
