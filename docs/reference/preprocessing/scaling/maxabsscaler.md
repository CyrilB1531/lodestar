# MaxAbsScaler

Divides each feature by its largest absolute value.

<!-- docs-declaration -->

```csharp
public sealed class MaxAbsScaler
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on.
`MaximumAbsolute` is each feature's largest absolute value and `Scale` is what
[`Transform`](maxabsscaler-transform.md) divides by — the same numbers, except on a feature the
near-constant floor caught.

**Example** — two features, one of them negative throughout.

```csharp
using Lodestar.Preprocessing;

double[] samples = [2.0, -4.0, 1.0, -8.0];

MaxAbsScaler scaler = MaxAbsScaler.Fit(samples, featureCount: 2);

double first = scaler.MaximumAbsolute[0];  // => 2
double second = scaler.Scale[1];           // => 8

double scaled = scaler.Transform(samples)[1];  // => -0.5
```

**Remarks** — **it never subtracts**, so a zero stays a zero. That is the whole reason to prefer it
to [`StandardScaler`](standardscaler.md) on data whose zeros are meaningful: centring turns every
zero into a non-zero, which is why scikit-learn refuses to centre a sparse matrix at all.

The largest absolute value is not the largest value: the second feature above runs from −8 to −4 and
is divided by 8.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScalerOptions`](maxabsscaleroptions.md), the [feature scaling index](../scaling.md),
the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`MaxAbsScaler.Fit`](maxabsscaler-fit.md) | Fits a scaler on a row-major sample matrix. |
| [`MaxAbsScaler.InverseTransform`](maxabsscaler-inversetransform.md) | Undoes `Transform`, and never clips. |
| [`MaxAbsScaler.PartialFit`](maxabsscaler-partialfit.md) | Folds another batch into the fitted statistics. |
| [`MaxAbsScaler.Transform`](maxabsscaler-transform.md) | Divides a matrix by the fitted maxima. |
