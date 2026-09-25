# PanelRegression

Panel regression: fixed effects, between, first-difference and random effects, at `linearmodels`
parity.

<!-- docs-declaration -->

```csharp
public static class PanelRegression
```

**Example** — four entities over four years, fitted with entity effects.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.FixedEffects(design, new PanelOptions { EntityEffects = true });

double slope = summary.Coefficients[1];      // => 1.514261266…
double error = summary.StandardErrors[1];    // => 0.0373453132…
double within = summary.RSquaredWithin;      // => 0.9933538938…
```

**Remarks** — the rows may come in any order: they are sorted by entity, then period, before
anything reads them, so a first difference is always taken between adjacent periods and `Theta`
runs in ascending entity label. The coefficients run constant first, when
[`PanelOptions.WithIntercept`](../paneldata/paneloptions.md) adds it, then the regressors. **The
default covariance is unadjusted and `Debiased` is on**, as the reference's `fit()` defaults are,
and **an option the fit would not read is refused rather than ignored**: effects outside
`FixedEffects`, clusters without the clustered covariance, a kernel without Driscoll-Kraay.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelSummary`](../paneldata/panelsummary.md), [`PanelOptions`](../paneldata/paneloptions.md),
the [panel index](../panel.md).

## Members

| Member | What it does |
| --- | --- |
| [`PanelRegression.FixedEffects`](panelregression-fixedeffects.md) | Fits least squares with entity or time effects projected out. |
| [`PanelRegression.Between`](panelregression-between.md) | Fits least squares on the entity means. |
| [`PanelRegression.FirstDifference`](panelregression-firstdifference.md) | Fits least squares on differences between adjacent periods. |
| [`PanelRegression.RandomEffects`](panelregression-randomeffects.md) | Fits the random-effects model by quasi-demeaning. |
