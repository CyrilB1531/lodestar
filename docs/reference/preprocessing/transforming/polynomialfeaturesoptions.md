# PolynomialFeaturesOptions

What [`PolynomialFeatures`](polynomialfeatures.md) expands, and how far.

<!-- docs-declaration -->

```csharp
public sealed record PolynomialFeaturesOptions
```

**Properties** — `Degree` is the highest total power a term may reach; scikit-learn's `degree`,
default 2. `InteractionOnly` keeps only terms in which every feature appears at most once;
scikit-learn's `interaction_only`, default `false`. `IncludeBias` emits the constant term;
scikit-learn's `include_bias`, default `true`.

**Example** — interactions without powers and without a bias.

```csharp
using Lodestar.Preprocessing;

var options = new PolynomialFeaturesOptions
{
    Degree = 2,
    InteractionOnly = true,
    IncludeBias = false,
};

string[] names = PolynomialFeatures.FeatureNames(3, options);

string joined = string.Join("|", names);   // => x0|x1|x2|x0 x1|x0 x2|x1 x2
```

**Remarks — degree 0 with the bias off is refused**, on all three members: the pair leaves no
term, and the reference raises rather than returning a matrix with no columns.

**`order` is absent.** scikit-learn's third parameter chooses numpy's memory layout,
C-contiguous or Fortran-contiguous, which changes how the array is stored and not what it holds.
There is one layout here, and it is the row-major one every member of this package uses.

Being a `record`, two option sets with the same three values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PolynomialFeatures.Transform`](polynomialfeatures-transform.md), the
[Python equivalence table](../../../equivalence.md).
