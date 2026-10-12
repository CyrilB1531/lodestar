# PowerTransformer

Raises each feature to the power that makes it most nearly normal, at
`sklearn.preprocessing.PowerTransformer` parity.

<!-- docs-declaration -->

```csharp
public sealed class PowerTransformer
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on. `Lambdas` is
each feature's fitted exponent — the reference's `lambdas_`.

**Example** — a right-skewed income column.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

PowerTransformer transformer = PowerTransformer.Fit(income, featureCount: 1);

double lambda = Math.Round(transformer.Lambdas[0], 4);   // => -0.8072

double[] symmetric = transformer.Transform(income);

double smallest = Math.Round(symmetric[0], 4);   // => -1.4543
double largest = Math.Round(symmetric[9], 4);    // => 1.7177
```

**Remarks — this is the gentle counterpart to
[`QuantileTransformer`](quantiletransformer.md).** Both answer "make this column look normal";
this one does it with a single exponent fitted by maximum likelihood, so the order *and* the
relative spacing survive — two values close together stay close together, and a new value outside
the fitted range is transformed by the same formula rather than clamped. The cost is that one
exponent may not be enough: a bimodal column has no power that makes it normal, and the
transformer will still return the best one.

**Conformance here is `1e-5`, not the `1e-9` the rest of this repository meets.** The exponent is
fitted by maximising a log-likelihood whose curvature at the optimum is about 176 against a value
around 443; a double carries that value to roughly `3e-11`, so the data itself pins the exponent
only to about `6e-7`, and moving it that far moves the transformed values by `9.1e-7` relative.
The reference's own optimiser stops at `1.48e-8` on the argument, well inside what the objective
can distinguish. The [Python equivalence table](../../../equivalence.md) carries the arithmetic;
it is the one family in this package held to a stated wider tolerance.

**On a likelihood flat to rounding the exponent is not reproducible.** A column whose spread is
tiny against its offset — large values with a little noise — carries almost nothing about its
exponent, and there numpy's vectorised `exp`, which parts from .NET's by an ulp on some inputs and
some processors, and the `log1p` and `expm1` this package writes, an ulp from the C library's on
others, move where either optimiser stops. Measured against scikit-learn 1.9.1
on 100 columns of `1e10` plus noise, 66 Yeo-Johnson and 29 Box-Cox exponents differed by more than
`1e-5` relative, and their transformed values with them.

**Two rows are the extreme of it.** Their optimum sits at `λ ≈ 0`, scikit-learn's exponents ranging
from `1e-7` to `1e-4`, so each optimiser stops on its rounding noise: on 150 random two-row columns,
72 Yeo-Johnson and 101 Box-Cox transforms part from scikit-learn's by more than `1e-5`, where three
to ten rows part on at most five in 150
([#1680](https://github.com/CyrilB1531/lodestar/issues/1680)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformerOptions`](powertransformeroptions.md),
[`PowerMethod`](powermethod.md), [`QuantileTransformer`](quantiletransformer.md), the
[feature transforming index](../transforming.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`PowerTransformer.Fit`](powertransformer-fit.md) | Fits one exponent per feature by maximum likelihood. |
| [`PowerTransformer.InverseTransform`](powertransformer-inversetransform.md) | Undoes `Transform`. |
| [`PowerTransformer.Transform`](powertransformer-transform.md) | Raises each feature to its fitted power. |
