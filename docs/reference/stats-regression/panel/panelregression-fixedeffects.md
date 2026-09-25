# PanelRegression.FixedEffects

Fits least squares with the chosen effects projected out,
`PanelOLS(y, x, entity_effects=…, time_effects=…).fit()`.

<!-- docs-declaration -->

```csharp
public static PanelSummary FixedEffects(PanelDesign design, PanelOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static PanelSummary FixedEffects(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
```

The second overload clusters by the caller's labels, alone or beside the entity or the period: its
`options` must ask for [`PanelCovarianceType.Clustered`](../paneldata/panelcovariancetype.md).

**Parameters** — `design` is the response, the regressors and each row's entity and period, as a
[`PanelDesign`](../paneldata/paneldesign.md). `clusters` is one label per row. `options` chooses the
effects, the covariance and the intercept; `null` takes the reference's defaults, which fit pooled
least squares.

**Returns** — a [`PanelSummary`](../paneldata/panelsummary.md) whose `PoolabilityTest` is the F
test that the effects are zero, `null` without effects.

**Exceptions** — `ArgumentOutOfRangeException` when `ExogenousCount` is below one or the confidence level is outside
(0, 1). `ArgumentException` when a block's length is not its width times the rows, when two rows
share an entity and a period, when no residual degree of freedom is left, when a Bartlett or Parzen
bandwidth reaches past the periods, or when the options set a value this estimator does not read.
`ArgumentNullException` when the clustered overload gets no `options`. `ArgumentException` too when clustering would run three ways.

**Example** — entity and time effects, clustered by entity.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.FixedEffects(design, new PanelOptions
{
    EntityEffects = true,
    TimeEffects = true,
    CovarianceType = PanelCovarianceType.Clustered,
    ClusterEntity = true,
});

double slope = summary.Coefficients[1];                 // => 1.804761904…
double error = summary.StandardErrors[1];               // => 0.1789687707…
int residualDf = summary.ResidualDegreesOfFreedom;      // => 8
double pooled = summary.PoolabilityTest!.Statistic;     // => 91.29621733…
```

**Remarks** — one effect is removed by demeaning; two are removed by demeaning the larger dimension
and projecting out the other's dummies, as the reference does. With a constant the grand means go
back in, so the constant survives. The effects count against the residual degrees of freedom, and
against the covariance's scaling too — **unless a single effect is nested in the clusters**, which
already absorb it: clustered by entity, entity effects are not counted twice.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression.RandomEffects`](panelregression-randomeffects.md),
[`PanelSummary`](../paneldata/panelsummary.md).
