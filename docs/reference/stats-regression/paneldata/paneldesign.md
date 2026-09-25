# PanelDesign

A panel's data: the response, the row-major regressors, and each row's entity and period.

<!-- docs-declaration -->

```csharp
public readonly ref struct PanelDesign
```

**Properties** — `Response` is one value per row. `Exogenous` is the row-major regressors,
`ExogenousCount` values per row, with no constant column of its own:
[`PanelOptions.WithIntercept`](paneloptions.md) adds it. `Entities` and `Periods` are one integer
label per row, any values; their order is what matters, not their spacing.

**Example** — the same data described once and fitted twice.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

int rows = design.Response.Length;                                                   // => 16
double within = PanelRegression.FixedEffects(design, new PanelOptions { EntityEffects = true }).Coefficients[1];  // => 1.514261266…
double random = PanelRegression.RandomEffects(design).Coefficients[1];               // => 1.653028044…
```

**Remarks** — a `ref struct`, so the spans are read in place and a design lives no longer than the
call it is passed to. The constructor checks nothing; the estimators refuse a block whose length is
not its width times the rows, and two rows with the same entity and period.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression`](../panel/panelregression.md).
