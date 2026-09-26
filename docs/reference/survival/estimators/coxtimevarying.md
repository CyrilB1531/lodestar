# CoxTimeVarying

The Cox model with covariates that change over time, one row per interval: lifelines'
`CoxTimeVaryingFitter`.

<!-- docs-declaration -->

```csharp
public static class CoxTimeVarying
```

**Example** — six subjects, three of whom change their covariate partway.

```csharp
using Lodestar.Survival;

double[] exposure = [0.5, 1.5, -0.3, 0.0, 0.8, -1.2, 1.1, 2.0, -0.5];
double[] starts = [0, 4, 0, 0, 6, 0, 0, 3, 0];
double[] stops = [4, 9, 7, 6, 12, 10, 3, 8, 5];
bool[] ends = [false, true, true, false, true, false, false, true, true];

CoxSummary fit = CoxTimeVarying.Fit(exposure, starts, stops, ends, featureCount: 1);

double beta = Math.Round(fit.Coefficients[0], 6);   // => 0.14208
```

**Remarks** — each row is an interval `(start, stop]` over which a subject's covariates held, and its
event flag says whether the subject's event ends it. The fit is the proportional hazards one on these
risk sets: at each event time, every interval spanning it. Reference behaviour is
`lifelines.CoxTimeVaryingFitter` 0.30.3. Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`CoxProportionalHazards`](coxproportionalhazards.md).

## Members

| Member | What it does |
| --- | --- |
| [`CoxTimeVarying.Fit`](coxtimevarying-fit.md) | Fits the model on start-stop intervals. |
