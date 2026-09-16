# RobustScaler

Centres each feature on its median and scales it by an interpercentile range.

<!-- docs-declaration -->

```csharp
public sealed class RobustScaler
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on. `Centre` is each
feature's median and `Scale` the range it divides by, **each nullable and null exactly when its own
step is off** — a simpler mapping than [`StandardScaler`](standardscaler.md)'s, where turning
centring off still fits a mean.

**Example** — one column with an outlier three decades out.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 2.0, 3.0, 4.0, 5000.0];

RobustScaler scaler = RobustScaler.Fit(samples, featureCount: 1);

double centre = scaler.Centre![0];  // => 3
double scale = scaler.Scale![0];    // => 2

double outlier = scaler.Transform(samples)[4];  // => 2498.5
```

**Remarks** — **this is the scaler for data with outliers.** The median and the quartile range above
move with the bulk of the column: the same five values give `StandardScaler` a mean of 1002 and a
standard deviation of about 1999, so every ordinary value lands near −0.5 and the shape of the data
is lost. Here they land at −1, −0.5, 0 and 0.5, and the outlier is visibly an outlier at 2498.5.

The percentiles interpolate linearly, `numpy.percentile`'s default — see
[`RobustScaler.Fit`](robustscaler-fit.md), which is also where the convention is compared against
the two other quantiles this repository ships.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScalerOptions`](robustscaleroptions.md), the [feature scaling index](../scaling.md),
the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`RobustScaler.Fit`](robustscaler-fit.md) | Fits a scaler on a row-major sample matrix. |
| [`RobustScaler.InverseTransform`](robustscaler-inversetransform.md) | Undoes `Transform`. |
| [`RobustScaler.Transform`](robustscaler-transform.md) | Centres and scales a matrix. |
