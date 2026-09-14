# CoxOptions

What a `CoxProportionalHazards` fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record CoxOptions
```

**Properties** — `ConfidenceLevel` is the two-sided level the intervals are reported at; `0.95` by
default. `MaximumIterations` is how many Newton-Raphson iterations are allowed; `100` by default.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, or when `MaximumIterations` is below one. Each is thrown where the setting is set, not
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

**Remarks** — **there is no intercept and no penalizer.** The baseline hazard absorbs what an
intercept would carry, so a column of ones is collinear with it and is refused. The reference's
penalizer defaults to zero, and a penalised fit is a different estimate with its own corpus.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxProportionalHazards.Fit`](coxproportionalhazards-fit.md),
[`CoxSummary`](coxsummary.md).
