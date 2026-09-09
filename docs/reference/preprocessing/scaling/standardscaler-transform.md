# StandardScaler.Transform

Standardises a row-major sample matrix with the fitted statistics.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to transform, row-major, with `FeatureCount` values per
row. It need not be the matrix the scaler was fitted on.

**Returns** — a new array of the same length. The input is never written to.

**Exceptions** — `ArgumentException` when `samples` holds no row, or a partial one.

**Example** — fit on training rows, apply to unseen ones.

```csharp
using Lodestar.Preprocessing;

double[] training = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];
StandardScaler scaler = StandardScaler.Fit(training, featureCount: 2);

// A row the scaler never saw, standardised with the training statistics.
double[] unseen = scaler.Transform([3.0, 10.0]);

double centred = unseen[0];   // => 0.5345224838248487
double constant = unseen[1];  // => 0
```

**Remarks** — the statistics come from the fit and are not recomputed here, which is the whole point
of the two-step shape: a validation set standardised with its own mean has been told something about
itself that the training set did not know.

**What the two steps do is decided by the options, not by which statistics exist.** With
`WithMean = false` the scaler still carries a `Mean` — see [`StandardScaler`](standardscaler.md)'s
table — and this method still does not subtract it.

A feature whose `Scale` was forced to 1 comes through centred but unscaled. That is the intended
outcome: its spread was below what the variance computation could resolve, so there is nothing to
divide by that would not be noise.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScaler.Fit`](standardscaler-fit.md),
[`StandardScaler.InverseTransform`](standardscaler-inversetransform.md).
