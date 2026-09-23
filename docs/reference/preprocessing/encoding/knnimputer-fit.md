# KnnImputer.Fit

Keeps the fitted rows, which are the donors every later call draws from.

<!-- docs-declaration -->

```csharp
public static KnnImputer Fit(ReadOnlySpan<double> samples, int featureCount, KnnImputerOptions options = null)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row, where a `NaN`
is a missing value; the span is read, never modified. `options` chooses how many donors to average
and how to weight them; `null` takes the reference's defaults, five donors weighted equally.

**Returns** — a fitted [`KnnImputer`](knnimputer.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the
neighbour count is below one. `ArgumentException` when `samples` holds no row, a partial one, or
an infinity.

**Example** — a feature missing from every row is dropped rather than filled.

```csharp
using Lodestar.Preprocessing;

double[] rows =
[
    1.0, double.NaN, 2.0,
    3.0, double.NaN, 4.0,
];

KnnImputer imputer = KnnImputer.Fit(rows, featureCount: 3);

int width = imputer.OutputFeatureCount;                    // => 2
string kept = string.Join(",", imputer.KeptFeatures);      // => 0,2
```

**Remarks — the fit keeps the rows, it does not summarise them.** Every donor is held, so the
imputer's memory is the fitted matrix itself and the work all happens in
[`Transform`](knnimputer-transform.md). That is the reference's design too, and it is what makes
the cost a product of both row counts rather than of one.

**Dropping an all-missing feature is the reference's `keep_empty_features=False`**, its default:
there is no donor to supply a value and no column statistic to fall back on, so the column is
removed from the output rather than filled with a fiction. [`KeptFeatures`](knnimputer.md) is how
a caller maps a filled row back onto the original feature indices.

**An infinity is refused where a `NaN` is not.** A `NaN` is the missing marker; an infinity is a
value that would make every distance through it infinite, so it is rejected at the door rather
than silently deciding which donors are nearest.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KnnImputer.Transform`](knnimputer-transform.md),
[`KnnImputerOptions`](knnimputeroptions.md).
