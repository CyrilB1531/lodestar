# StandardScalerOptions

Which of the two standardisation steps [`StandardScaler`](standardscaler.md) applies.

<!-- docs-declaration -->

```csharp
public sealed record StandardScalerOptions
```

**Properties** — `WithMean` centres each feature on its mean, `WithStd` divides it by its standard
deviation. Both default to `true`, as `sklearn.preprocessing.StandardScaler`'s `with_mean` and
`with_std` do.

**Example** — scale without centring, which is what a caller keeping sparsity asks for.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];

StandardScaler scaler = StandardScaler.Fit(
    samples, featureCount: 2, new StandardScalerOptions { WithMean = false });

// The mean is still fitted — turning centring off does not stop it being computed.
double mean = scaler.Mean![0];              // => 2.3333333333333335

// But it is not subtracted: the first value keeps its own magnitude.
double first = scaler.Transform(samples)[0]; // => 0.8017837257372732
```

**Remarks** — the pair does more than switch two lines of arithmetic: it decides **which fitted
statistics exist at all**, and the mapping is not symmetric. [`StandardScaler`](standardscaler.md)
carries the table.

`WithMean = false` is the option a caller reaches for when centring would be wrong rather than
merely unwanted — subtracting a mean from a matrix whose zeros are meaningful turns every zero into
a non-zero, which is why scikit-learn refuses to centre a sparse matrix at all.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScaler`](standardscaler.md), [`StandardScaler.Fit`](standardscaler-fit.md).
