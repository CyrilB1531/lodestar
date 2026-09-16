# MinMaxScaler

Maps each feature onto a fixed range.

<!-- docs-declaration -->

```csharp
public sealed class MinMaxScaler
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on. `DataMinimum`,
`DataMaximum` and `DataRange` are what it saw; `Scale` and `Minimum` are what
[`Transform`](minmaxscaler-transform.md) multiplies and adds. None is nullable: unlike
[`StandardScaler`](standardscaler.md), every statistic here exists whatever the options say.

**Example** — three rows, two features, the second constant.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 10.0, 3.0, 10.0, 5.0, 10.0];

MinMaxScaler scaler = MinMaxScaler.Fit(samples, featureCount: 2);

double scale = scaler.Scale[0];          // => 0.25
double constantScale = scaler.Scale[1];  // => 1

double[] mapped = scaler.Transform(samples);
double smallest = mapped[0];             // => 0
```

**Remarks** — the first feature spans 1 to 5, so a quarter maps it onto `[0, 1]`. The second never
varies, and its scale is **1 rather than a division by zero**: the rule is `range < 10·eps`, not
`range == 0`, which [`MinMaxScaler.Fit`](minmaxscaler-fit.md) states with the measurement behind it.
A feature the floor catches lands on the bottom of the range.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScalerOptions`](minmaxscaleroptions.md), the [feature scaling index](../scaling.md),
the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`MinMaxScaler.Fit`](minmaxscaler-fit.md) | Fits a scaler on a row-major sample matrix. |
| [`MinMaxScaler.InverseTransform`](minmaxscaler-inversetransform.md) | Undoes `Transform`, and never clips. |
| [`MinMaxScaler.PartialFit`](minmaxscaler-partialfit.md) | Folds another batch into the fitted statistics. |
| [`MinMaxScaler.Transform`](minmaxscaler-transform.md) | Maps a matrix onto the fitted range. |
