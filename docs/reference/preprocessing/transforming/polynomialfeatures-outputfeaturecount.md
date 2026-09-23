# PolynomialFeatures.OutputFeatureCount

How many terms an expansion produces, without producing one.

<!-- docs-declaration -->

```csharp
public static int OutputFeatureCount(int featureCount, PolynomialFeaturesOptions options = null)
```

**Parameters** — `featureCount` is how many values each input row carries. `options` chooses the
degree, the interaction restriction and the bias; `null` takes the reference's defaults.

**Returns** — the number of columns [`Transform`](polynomialfeatures-transform.md) would emit.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the degree
is negative. `ArgumentException` when the degree is 0 and the bias is off, which leaves no term.

**Example** — four features at degree three, before deciding whether to expand them.

```csharp
using Lodestar.Preprocessing;

var options = new PolynomialFeaturesOptions { Degree = 3 };

int columns = PolynomialFeatures.OutputFeatureCount(featureCount: 4, options);   // => 35
```

**Remarks — ask this before expanding a wide matrix.** The expansion allocates every column of
every row at once, and the column count is a binomial coefficient: it grows faster than a reader
expects, and a matrix that fits comfortably in memory can have an expansion that does not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PolynomialFeatures.Transform`](polynomialfeatures-transform.md),
[`PolynomialFeatures.FeatureNames`](polynomialfeatures-featurenames.md), the
[Python equivalence table](../../../equivalence.md).
