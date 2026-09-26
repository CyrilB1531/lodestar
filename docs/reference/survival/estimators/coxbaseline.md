# CoxBaseline

One stratum's baseline of a Cox fit: Breslow's hazard at each time, accumulated, and the survival it
implies. [`CoxSummary.Baselines`](coxsummary.md) holds one per stratum.

<!-- docs-declaration -->

```csharp
public sealed record CoxBaseline(int Stratum, double[] Times, double[] Hazard, double[] CumulativeHazard, double[] Survival)
```

**Properties** — `Stratum` is the stratum's label, zero for an unstratified fit. `Times` are every
distinct duration of the fit, ascending, as lifelines indexes its baseline. `Hazard` is the baseline
hazard at each, zero where the stratum has no event; `CumulativeHazard` its running sum; `Survival`
`exp(−CumulativeHazard)`.

**Example** — the first steps of an unstratified baseline.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxBaseline baseline = CoxProportionalHazards.Fit(design, months, died, featureCount: 2).Baselines[0];

double firstTime = baseline.Times[0];                          // => 2
double atFive = Math.Round(baseline.CumulativeHazard[2], 6);   // => 0.237236
```

**Remarks** — the baseline is that of a subject at the covariates' **means**, as lifelines centres
them; a subject's own cumulative hazard is this one times its partial hazard `exp((x − mean) · β)`,
which is what [`CoxSummary.PredictCumulativeHazard`](coxsummary-predictcumulativehazard.md) computes.
Equality compares the label and the four arrays element by element.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md).

## Members

| Member | What it does |
| --- | --- |
| [`CoxBaseline.Equals`](coxbaseline-equals.md) | Compares the label and the four arrays, element by element. |
| [`CoxBaseline.GetHashCode`](coxbaseline-gethashcode.md) | A hash consistent with it. |
