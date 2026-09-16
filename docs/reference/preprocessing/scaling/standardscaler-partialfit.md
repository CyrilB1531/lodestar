# StandardScaler.PartialFit

Folds another batch into the mean, the variance and the scale, as `partial_fit` does.

<!-- docs-declaration -->

```csharp
public StandardScaler PartialFit(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the next batch, row-major, with `FeatureCount` values per row.

**Returns** — a new scaler summarising every batch seen so far; the one it was called on is unchanged.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one.

**Example** — two batches, and the fit they add up to.

```csharp
using Lodestar.Preprocessing;

double[] first = [1.0, 2.0, 4.0];
double[] second = [8.0, 16.0];

StandardScaler scaler = StandardScaler.Fit(first, featureCount: 1).PartialFit(second);

int seen = scaler.SampleCount;   // => 5
double mean = scaler.Mean![0];   // => 6.2
```

**Remarks** — **the statistics are the ones a single fit over the concatenation gives**, to a couple
of units in the last place: the update is `_incremental_mean_and_var`'s, Chan, Golub and LeVeque's
parallel form, correction term included. Measured, the reference's own two paths differ by at most
`3.3e-16` relative, and the corpus freezes both to say so.

**It returns a new scaler and leaves this one alone**, which is the one place the spelling differs
from `partial_fit`. `scaler = scaler.PartialFit(batch)` reads the same, and a fitted object here
never changes answer under a reader.

`SampleCount` stays one number where the reference's `n_samples_seen_` becomes an array once a `NaN`
appears — these scalers refuse a non-finite value, so the per-feature counts cannot diverge.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScaler`](standardscaler.md), [`StandardScaler.Fit`](standardscaler-fit.md).
