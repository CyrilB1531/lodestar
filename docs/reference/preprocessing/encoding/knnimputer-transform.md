# KnnImputer.Transform

Fills every missing value from its nearest donors among the fitted rows.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to fill, row-major,
[`FeatureCount`](knnimputer.md) values per row, where a `NaN` is a missing value.

**Returns** — a new matrix, [`OutputFeatureCount`](knnimputer.md) values per row; the input is
never written to.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or an infinity.
`ArgumentOutOfRangeException` when the number of distances needed exceeds the ceiling below.

**Example** — the same two gaps, weighted two ways.

```csharp
using Lodestar.Preprocessing;

double[] rows =
[
    1.0, 2.0, double.NaN,
    3.0, 4.0, 3.0,
    double.NaN, 6.0, 5.0,
    8.0, 8.0, 7.0,
];

KnnImputer uniform = KnnImputer.Fit(
    rows, 3, new KnnImputerOptions { NeighbourCount = 2 });

KnnImputer weighted = KnnImputer.Fit(
    rows, 3, new KnnImputerOptions { NeighbourCount = 2, Weights = NeighbourWeights.Distance });

double byMean = uniform.Transform(rows)[2];     // => 4
double byDistance = Math.Round(weighted.Transform(rows)[2], 4);   // => 3.6667
```

**Remarks — the call is refused past 100 million distance terms.** The cost is
`rows × fitted rows × features`, and on a Ryzen 7 8700G a hundred million terms over one feature
take about 0.35 s and the same count over ten features about 1.6 s (measured 2026-09-23). Past
that the call throws rather than running for however long it would take — the same stance
`MannWhitney` takes on its exact table. Split the receiving rows into batches to go further; the
fitted donors are the expensive half and they are shared.

**A donor is a fitted row that has the missing feature**, not simply a near row. The nearest rows
are ranked by `nan_euclidean` distance, and the first `NeighbourCount` of them that actually carry
the feature being filled are the ones averaged — so a gap in a rare feature can draw on donors
further away than the nominal neighbours.

**A value with no donor at all falls back to the feature's mean over the fitted rows**, which is
the reference's behaviour. A feature with no value anywhere in the fitted matrix was already
dropped at [`Fit`](knnimputer-fit.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KnnImputer.Fit`](knnimputer-fit.md),
[`NeighbourWeights`](neighbourweights.md), [`KnnImputer`](knnimputer.md).
