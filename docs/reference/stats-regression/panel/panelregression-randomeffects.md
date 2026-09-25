# PanelRegression.RandomEffects

Fits the random-effects model, `RandomEffects(y, x).fit()`: Swamy and Arora's variance components,
then least squares on quasi-demeaned rows.

<!-- docs-declaration -->

```csharp
public static PanelSummary RandomEffects(PanelDesign design, PanelOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static PanelSummary RandomEffects(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
```

The second overload clusters by the caller's labels, alone or beside the entity or the period.

**Parameters** — `design` is the panel. `clusters` is one label per row. `options` chooses the
covariance and the intercept; `null` takes the reference's defaults.

**Returns** — a [`PanelSummary`](../paneldata/panelsummary.md) whose `Theta` holds each entity's
quasi-demeaning weight, and whose variance components are the estimated ones.

**Exceptions** — `ArgumentOutOfRangeException` when `ExogenousCount` is below one or the confidence level is outside
(0, 1). `ArgumentException` when a block's length is not its width times the rows, when two rows
share an entity and a period, when no residual degree of freedom is left, when a Bartlett or Parzen
bandwidth reaches past the periods, or when the options set a value this estimator does not read.
`ArgumentNullException` when the clustered overload gets no `options`. `ArgumentException` too when
there are no more rows than entities plus coefficients, or no more entities than coefficients:
the variance components' degrees of freedom, where the reference divides by zero.

**Example** — the slope, and how much of each entity's mean it removes.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.RandomEffects(design);

double slope = summary.Coefficients[1];      // => 1.653028044…
double theta = summary.Theta![0];            // => 0.6691202669…
double share = summary.Rho!.Value;           // => 0.6703470171…
```

**Remarks** — `σ²ₑ` comes from the within regression over `n − k − N + 1` degrees of freedom and
`σ²ᵤ` from the between one, less `σ²ₑ` over the harmonic mean of the entity sizes, floored at zero.
`θᵢ = 1 − √(σ²ₑ/(Tᵢσ²ᵤ + σ²ₑ))`: zero reduces to pooled least squares, one to fixed effects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression.FixedEffects`](panelregression-fixedeffects.md),
[`PanelSummary`](../paneldata/panelsummary.md).
