# QuantileOutput

Which distribution [`QuantileTransformer`](quantiletransformer.md) maps its ranks onto.

<!-- docs-declaration -->

```csharp
public enum QuantileOutput
```

**Values** — `Uniform` maps onto the unit interval; scikit-learn's `'uniform'`, and the default.
`Normal` maps onto the standard normal, through its quantile function; `'normal'`.

**Example** — the two ends of a column, on the normal output.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer normal = QuantileTransformer.Fit(
    skew, 1, new QuantileTransformerOptions { Output = QuantileOutput.Normal });

double[] ends = normal.Transform([1.0, 100.0]);

double low = ends[0];    // => -5.199337582605575
double high = ends[1];   // => 5.19933758270342
```

**Remarks — `Normal` is `Uniform` composed with the normal quantile function**, so the two carry
the same information and differ in what a downstream model sees: a uniform column has no tails
and a normal one does, which matters to anything that assumes normality — a linear model's
residuals, a Gaussian naive Bayes, a distance in a space where the extremes should count for
more.

**The ends are clipped rather than infinite.** The normal quantile of 0 is `-∞`, which no model
can use, so both ends stop at the quantile of `1e-7` one ulp in — about ±5.199. That is the
reference's own threshold, and the asymmetry in the last digits above is the quantile function
being evaluated at two arguments that are not exact mirrors.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformerOptions`](quantiletransformeroptions.md),
[`QuantileTransformer.Transform`](quantiletransformer-transform.md).
