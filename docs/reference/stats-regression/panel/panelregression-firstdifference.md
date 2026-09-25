# PanelRegression.FirstDifference

Fits least squares on the differences between adjacent periods of each entity,
`FirstDifferenceOLS(y, x).fit()`.

<!-- docs-declaration -->

```csharp
public static PanelSummary FirstDifference(PanelDesign design, PanelOptions options)
```

<!-- docs-declaration -->

```csharp
public static PanelSummary FirstDifference(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
```

`options` is required, since the default adds the constant a difference removes: pass
`WithIntercept = false`. The second overload clusters by the caller's labels, which must not change
between two rows that are differenced.

**Parameters** — `design` is the panel, with no constant column. `clusters` is one label per row.
`options` chooses the covariance.

**Returns** — a [`PanelSummary`](../paneldata/panelsummary.md) over the differenced rows:
`ObservationCount` counts the differences, one fewer per entity, and one fewer again per gap.

**Exceptions** — `ArgumentOutOfRangeException` when `ExogenousCount` is below one or the confidence level is outside
(0, 1). `ArgumentException` when a block's length is not its width times the rows, when two rows
share an entity and a period, when no residual degree of freedom is left, when a Bartlett or Parzen
bandwidth reaches past the periods, or when the options set a value this estimator does not read.
`ArgumentNullException` when the clustered overload gets no `options`. `ArgumentException` too when the regressors hold a constant or
`WithIntercept` is set, when there are fewer than two periods, when a cluster label changes between
differenced rows, or when the options cluster by period.

**Example** — the same panel, differenced.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.FirstDifference(design, new PanelOptions { WithIntercept = false });

double slope = summary.Coefficients[0];      // => 1.551418439…
double error = summary.StandardErrors[0];    // => 0.0574956411…
int differences = summary.ObservationCount;  // => 12
```

**Remarks** — a difference is taken only between adjacent periods of the sorted period labels; a
missing year leaves its two neighbours undifferenced. **The reference reads its period axis in the
order periods are first seen**, so an unbalanced panel whose first entity misses a period differences
the wrong pairs there; the sorted axis is used here, and the two agree whenever the first entity is
complete.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression.FixedEffects`](panelregression-fixedeffects.md).
