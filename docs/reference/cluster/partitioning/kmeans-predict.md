# KMeans.Predict

Assigns unseen samples to the fitted centres.

<!-- docs-declaration -->

```csharp
public int[] Predict(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to assign, row-major, with `FeatureCount` values per row.

**Returns** — one cluster index per row.

**Exceptions** — `ArgumentException` when `samples` holds no row, or a partial one.

**Example** — rows the fit never saw.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];
KMeans model = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] });

int[] unseen = model.Predict([0.5, 0.5, 9.5, 10.5]);

int low = unseen[0];   // => 0
int high = unseen[1];  // => 1
```

**Remarks** — nothing is refitted: this is the assignment step alone, over the centres already
found. Passing the fitted samples back reproduces `Labels` exactly, which is not a coincidence but
the contract — when the loop stops on the centre shift rather than on the labels, a final assignment
runs so the two agree.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeans.Fit`](kmeans-fit.md), [`KMeans`](kmeans.md).
