# Lodestar.Cluster

Clustering over a row-major span: k-means by Lloyd's algorithm, with the initial
centres accepted as an input so a run is reproducible against scikit-learn's own start, plus
DBSCAN and agglomerative clustering. Labels, centres and inertia are the ones scikit-learn
returns.

## Install

```bash
dotnet add package Lodestar.Cluster
```

## Example

```csharp
using Lodestar.Cluster;

// Five points of two features, row-major.
double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

KMeans model = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, -500.0, -500.0] });

int middle = model.Labels[4];   // 2
```

## Parity

Replayed against `sklearn.cluster`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Reference: [cluster/partitioning](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/cluster/partitioning.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Cluster/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Cluster/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
