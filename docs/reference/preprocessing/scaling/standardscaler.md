# StandardScaler

Centres each feature on its mean and scales it to unit variance.

<!-- docs-declaration -->

```csharp
public sealed class StandardScaler
```

**Properties** — `FeatureCount` is how many values a row carries, and `SampleCount` how many rows
the scaler was fitted on (scikit-learn's `n_samples_seen_`). `Mean`, `Variance` and `Scale` are the
fitted statistics, each **nullable**, because the reference reports `None` for the ones a given
option pair does not compute — see the table below.

**Example** — fit on a matrix, then standardise it.

```csharp
using Lodestar.Preprocessing;

// Row-major: two features per row, three rows. The second is constant.
double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];

StandardScaler scaler = StandardScaler.Fit(samples, featureCount: 2);

double firstMean = scaler.Mean![0];      // => 2.3333333333333335
double constantScale = scaler.Scale![1]; // => 1

double[] standardised = scaler.Transform(samples);
double first = standardised[0];          // => -1.069…
```

**Remarks** — **which statistics exist depends on both options, and not in the obvious way.** This
is scikit-learn's behaviour, reproduced rather than tidied:

| `WithMean` | `WithStd` | `Mean` | `Variance` | `Scale` |
| --- | --- | --- | --- | --- |
| `true` | `true` | fitted | fitted | fitted |
| `false` | `true` | **fitted** | fitted | fitted |
| `true` | `false` | fitted | `null` | `null` |
| `false` | `false` | `null` | `null` | `null` |

Turning centring off still computes a mean; only turning **both** steps off drops it. A scaler that
reports a `Mean` is therefore not necessarily a scaler that subtracts one — what
[`StandardScaler.Transform`](standardscaler-transform.md) does is decided by the options, not by
which statistics came back.

The second feature above is constant, and its `Scale` is `1` rather than `0`. The rule behind that
is not `variance == 0`, and it is worth reading before trusting a value near the noise floor:
[`StandardScaler.Fit`](standardscaler-fit.md) has it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScalerOptions`](standardscaleroptions.md), the
[feature scaling index](../scaling.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`StandardScaler.Fit`](standardscaler-fit.md) | Fits a scaler on a row-major sample matrix. |
| [`StandardScaler.InverseTransform`](standardscaler-inversetransform.md) | Undoes `Transform`, returning values on the original scale. |
| [`StandardScaler.Transform`](standardscaler-transform.md) | Standardises a matrix with the fitted statistics. |
