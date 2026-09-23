# KnnImputer

Fills each missing value from the rows most like the one it is missing from, at
`sklearn.impute.KNNImputer` parity.

<!-- docs-declaration -->

```csharp
public sealed class KnnImputer
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on.
`OutputFeatureCount` is how many values a filled row carries, and `KeptFeatures` says which
feature each one is — **fewer than `FeatureCount` when a feature was missing from every fitted
row**, since there is then nothing to impute it from and nothing to impute it with.

**Example** — a `NaN` in each of two rows, filled from the two nearest donors.

```csharp
using Lodestar.Preprocessing;

double[] rows =
[
    1.0, 2.0, double.NaN,
    3.0, 4.0, 3.0,
    double.NaN, 6.0, 5.0,
    8.0, 8.0, 7.0,
];

KnnImputer imputer = KnnImputer.Fit(
    rows, featureCount: 3, new KnnImputerOptions { NeighbourCount = 2 });

double[] filled = imputer.Transform(rows);

double firstGap = filled[2];   // => 4
double secondGap = filled[6];  // => 5.5
```

**Remarks — this is the imputer that reads the row, where
[`SimpleImputer`](simpleimputer.md) reads the column.** A mean fills every gap in a feature with
the same number; this one asks which other rows resemble the one with the gap and answers with
theirs, so a row that looks like the small ones gets a small value and a row that looks like the
large ones gets a large one. Above, the missing third feature of the first row becomes 4 — the
mean of its two nearest donors — not 5, the column mean.

**Distances are `nan_euclidean`.** The squared differences are taken over the coordinates both
rows have and scaled up by the fraction that were missing, so a pair sharing two features of four
is not automatically nearer than a pair sharing all four. It is the reference's own metric.

**It costs a distance between every pair**, so the work is
`rows × fitted rows × features` — which is why the call is refused past a measured ceiling rather
than run for however long that takes. [`Transform`](knnimputer-transform.md) has the number.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KnnImputerOptions`](knnimputeroptions.md),
[`NeighbourWeights`](neighbourweights.md), [`SimpleImputer`](simpleimputer.md), the
[encoding index](../encoding.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`KnnImputer.Fit`](knnimputer-fit.md) | Keeps the fitted rows, which are the donors. |
| [`KnnImputer.Transform`](knnimputer-transform.md) | Fills every missing value from its nearest donors. |
