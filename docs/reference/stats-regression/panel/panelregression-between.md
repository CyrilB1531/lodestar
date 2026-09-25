# PanelRegression.Between

Fits least squares on the entity means, `BetweenOLS(y, x).fit()`.

<!-- docs-declaration -->

```csharp
public static PanelSummary Between(PanelDesign design, PanelOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static PanelSummary Between(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
```

The second overload clusters the entities by the caller's labels, which must be constant within
each entity.

**Parameters** — `design` is the panel. `clusters` is one label per row. `options` chooses the
covariance and the intercept; `null` takes the reference's defaults.

**Returns** — a [`PanelSummary`](../paneldata/panelsummary.md) over one row per entity:
`ObservationCount` is the entity count.

**Exceptions** — `ArgumentOutOfRangeException` when `ExogenousCount` is below one or the confidence level is outside
(0, 1). `ArgumentException` when a block's length is not its width times the rows, when two rows
share an entity and a period, when no residual degree of freedom is left, when a Bartlett or Parzen
bandwidth reaches past the periods, or when the options set a value this estimator does not read.
`ArgumentNullException` when the clustered overload gets no `options`. `ArgumentException` too when a cluster label varies inside an entity,
or when the options ask for Driscoll-Kraay or for clusters by entity or period.

**Example** — the slope across entities rather than within them.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.Between(design);

double slope = summary.Coefficients[1];      // => 2.686235759…
double error = summary.StandardErrors[1];    // => 0.1019030481…
int rows = summary.ObservationCount;         // => 4
```

**Remarks** — each entity counts once whatever its number of periods, as the reference weighs them.
Clustered with no labels, each entity is its own cluster.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression.FixedEffects`](panelregression-fixedeffects.md).
