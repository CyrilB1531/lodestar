# PolynomialFeatures.FeatureNames

Each term's name, in the order the columns come out.

<!-- docs-declaration -->

```csharp
public static string[] FeatureNames(int featureCount, PolynomialFeaturesOptions options = null)
```

**Parameters** — `featureCount` is how many values each input row carries. `options` chooses the
degree, the interaction restriction and the bias; `null` takes the reference's defaults.

**Returns** — one name per output column, in the reference's own spelling: `1` for the bias,
`x0` for a feature, `x0^2` for a power, `x0 x1` for a product.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the degree
is negative. `ArgumentException` when the degree is 0 and the bias is off, which leaves no term.

**Example** — the default expansion of two features.

```csharp
using Lodestar.Preprocessing;

string[] names = PolynomialFeatures.FeatureNames(featureCount: 2);

string joined = string.Join("|", names);   // => 1|x0|x1|x0^2|x0 x1|x1^2
```

**Remarks — this is the only way to read a coefficient back.** The expansion returns an array of
doubles with nothing to say which column is which, and the order is not obvious past degree two:
`x0^2 x1` comes before `x0 x1^2`, because the terms are ordered by their feature indices and not
by their powers. A caller that fits a model on the expansion and prints its coefficients should
print these beside them.

The names are composed invariantly, so a French or an Arabic culture produces the same strings as
an English one: a name here is an identifier to match on, not a number to read.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PolynomialFeatures.Transform`](polynomialfeatures-transform.md),
[`PolynomialFeatures.OutputFeatureCount`](polynomialfeatures-outputfeaturecount.md), the
[Python equivalence table](../../../equivalence.md).
