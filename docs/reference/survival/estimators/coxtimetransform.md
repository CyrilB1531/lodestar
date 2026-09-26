# CoxTimeTransform

The time scale [`CoxProportionalHazards.TestProportionalHazards`](coxproportionalhazards-testproportionalhazards.md)
correlates the scaled Schoenfeld residuals with: lifelines' `time_transform`.

<!-- docs-declaration -->

```csharp
public enum CoxTimeTransform { Rank, KaplanMeier, Identity, Log }
```

**Members** — `Rank`, the default, is each event's rank among the fit's sorted subjects, lifelines'
`"rank"`. `KaplanMeier` is one less the pooled Kaplan-Meier estimate at the event, `"km"`, R's
default. `Identity` is the event time itself and `Log` its logarithm.

**Example** — the same fit under two time scales.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double rank = CoxProportionalHazards.TestProportionalHazards(design, months, died, fit)[0].Statistic;
double km = CoxProportionalHazards.TestProportionalHazards(
    design, months, died, [], [], fit, CoxTimeTransform.KaplanMeier)[0].Statistic;

double byRank = Math.Round(rank, 6);   // => 0.962064
double byCurve = Math.Round(km, 6);    // => 0.77799
```

**Remarks** — lifelines defaults to the rank, which performs well against the others (Park and
Hendry, 2015); R's `cox.zph` defaults to the Kaplan-Meier scale. Under `Log` a zero duration gives a
`NaN` statistic, as it does in lifelines.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxProportionalHazards.TestProportionalHazards`](coxproportionalhazards-testproportionalhazards.md).
