# NeighbourWeights

How [`KnnImputer`](knnimputer.md) weights the donors it averages.

<!-- docs-declaration -->

```csharp
public enum NeighbourWeights
```

**Values** — `Uniform` counts every donor the same; scikit-learn's `'uniform'`, and the default.
`Distance` counts each donor as the reciprocal of its distance; `'distance'`.

**Example** — the same gap, filled both ways.

```csharp
using Lodestar.Preprocessing;

double[] rows =
[
    1.0, 2.0, double.NaN,
    3.0, 4.0, 3.0,
    double.NaN, 6.0, 5.0,
    8.0, 8.0, 7.0,
];

var uniform = new KnnImputerOptions { NeighbourCount = 2 };

double flat = KnnImputer.Fit(rows, 3, uniform).Transform(rows)[2];   // => 4

KnnImputer weighted = KnnImputer.Fit(
    rows, 3, uniform with { Weights = NeighbourWeights.Distance });

double nearer = Math.Round(weighted.Transform(rows)[2], 4);          // => 3.6667
```

**Remarks — `Distance` matters when the donors are not equally close**, which is most of the
time: above, the nearer donor carries a 3 and the further one a 5, so weighting pulls the filled
value from 4 down to 3.67. Prefer it when the rows are genuinely comparable on a metric scale,
and the default when they are not — an unweighted mean of the nearest few is the more robust
answer on a matrix whose features were not scaled.

**A donor at distance zero takes the whole weight.** A row identical on every shared feature
gives an infinite reciprocal, and the reference resolves that by averaging only the exact matches
and ignoring the rest; so does this.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KnnImputerOptions`](knnimputeroptions.md),
[`KnnImputer.Transform`](knnimputer-transform.md).
