# KnnImputerOptions

How many neighbours [`KnnImputer`](knnimputer.md) averages, and how it weights them.

<!-- docs-declaration -->

```csharp
public sealed record KnnImputerOptions
```

**Properties** — `NeighbourCount` is how many donors each missing value is averaged over;
scikit-learn's `n_neighbors`, default 5. `Weights` is whether nearer donors count for more;
`weights`, default [`NeighbourWeights.Uniform`](neighbourweights.md).

**Example** — two donors rather than five, on a four-row matrix.

```csharp
using Lodestar.Preprocessing;

double[] rows =
[
    1.0, 2.0, double.NaN,
    3.0, 4.0, 3.0,
    double.NaN, 6.0, 5.0,
    8.0, 8.0, 7.0,
];

var options = new KnnImputerOptions { NeighbourCount = 2 };

double filled = KnnImputer.Fit(rows, 3, options).Transform(rows)[2];   // => 4
```

**Remarks — the neighbour count is a ceiling, not a requirement.** Asking for five donors on a
matrix where only three rows carry the feature averages those three; nothing is refused and no
row is padded.

**`metric` is absent.** The reference publishes one value for it, `'nan_euclidean'`, and
documents that it is the only one supported — so there is no choice to carry.
`missing_values` is absent with it: `NaN` is the marker, which is the reference's own default and
the only one that needs no sentinel value hunting through a column of doubles. `copy` and
`add_indicator` are absent for the reasons the rest of this package gives — a span goes in and a
new array comes out, and an indicator column is something a caller composes.

**`keep_empty_features` is absent because the default is the only defensible answer here**: a
feature missing from every fitted row is dropped, as [`KnnImputer.Fit`](knnimputer-fit.md)
describes.

Being a `record`, two option sets with the same two values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KnnImputer.Fit`](knnimputer-fit.md),
[`NeighbourWeights`](neighbourweights.md).
