# SimpleImputer.Transform

Fills the missing values of a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to fill, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, with every `NaN` replaced.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or an infinity.

**Example** — the statistic is the fit's, not the transform's.

```csharp
using Lodestar.Preprocessing;

double[] fitted = [1.0, 2.0, 3.0];

SimpleImputer imputer = SimpleImputer.Fit(fitted, featureCount: 1);

// The mean of 1, 2 and 3 — the row being filled has no say in it.
string filled = string.Join(",", imputer.Transform([double.NaN, 99.0]));  // => 2,99
```

**Remarks** — the width never changes, which is the divergence
[`Fit`](simpleimputer-fit.md) explains: the reference's default returns one column fewer when a
feature was empty at fit time.

Never writes to the input.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimpleImputer`](simpleimputer.md), [`SimpleImputerOptions`](simpleimputeroptions.md).
