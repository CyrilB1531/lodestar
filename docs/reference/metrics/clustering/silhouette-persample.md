# Silhouette.PerSample

The score of each sample, from the samples themselves.

<!-- docs-declaration -->

```csharp
public static double[] PerSample(ReadOnlySpan<int> labels, ReadOnlySpan<double> features, int featureCount)
```

**Parameters** — as `Silhouette.Score`: `labels`, the samples `features` row-major, and
`featureCount`.

**Returns** — `double[]`, one value per sample in the order the samples were given.

**Exceptions** — `ArgumentException` when the inputs disagree in size, when a feature is not
finite — "Input X contains NaN." or its infinity counterpart, `check_X_y`'s sentences — and when the number of
distinct labels falls outside `[2, n - 1]` — scikit-learn's own bound, carried with its own
sentence: `Number of labels is 1. Valid values are 2 to n_samples - 1 (inclusive)`. Also
`ArgumentException`, naming `labels`, when the `n × k` per-cluster distance sums over `n` samples
and `k` clusters need more cells than one array holds ([#1480](https://github.com/CyrilB1531/lodestar/issues/1480)).

**Example** — the mean hides which sample is misplaced; this does not.

```csharp
using Lodestar.Metrics;

double[] features = [0.0, 0.0, 0.2, 0.1, 4.0, 4.0, 4.2, 3.9, 0.1, 0.3];
int[] labels = [0, 0, 1, 1, 1];

double[] scores = Silhouette.PerSample(labels, features, 2);
double stranger = scores[4];   // => -0.9501…
```

**Remarks** — the negative value is the point. Sample 4 was labelled into the far cluster while
sitting among the near one, and no mean would have told you which sample to look at.

A cluster holding one sample scores that sample `0.0` rather than dividing by zero: there is no
other member to be close to, so it is neither well nor badly placed. That is scikit-learn's answer,
measured.

**Applies to** — net10.0, netstandard2.0.

**See also** — `Silhouette.Score`, `Silhouette.PerSampleFromDistances`, the [Python equivalence table](../../../equivalence.md).
