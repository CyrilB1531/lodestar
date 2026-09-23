# PolynomialFeatures.Transform

Expands a row-major matrix into its polynomial terms.

<!-- docs-declaration -->

```csharp
public static double[] Transform(ReadOnlySpan<double> samples, int featureCount, PolynomialFeaturesOptions options = null)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row; the span is
read, never modified. `options` chooses the degree, whether to keep only interactions, and
whether to emit the bias; `null` takes the reference's defaults, which are degree two with the
bias and without the interaction restriction.

**Returns** — a new matrix,
[`OutputFeatureCount`](polynomialfeatures-outputfeaturecount.md) values per row.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, the degree is
negative, or the expansion would need more than `int.MaxValue` values. `ArgumentException` when
`samples` holds no row, a partial one, or a non-finite value; or when the degree is 0 and the bias
is off, which leaves no term at all — the reference refuses that pair in as many words.

**Example** — two features at the default degree, which is what a linear model needs to see a
curve or an interaction.

```csharp
using Lodestar.Preprocessing;

double[] row = [3.0, 4.0];

double[] expanded = PolynomialFeatures.Transform(row, featureCount: 2);

double bias = expanded[0];        // => 1
double squareOfFirst = expanded[3];  // => 9
double product = expanded[4];     // => 12
```

The six columns are `1`, `x0`, `x1`, `x0²`, `x0·x1`, `x1²` — which is what
[`FeatureNames`](polynomialfeatures-featurenames.md) says, and the order a coefficient vector
will come back in.

**Remarks — the count grows fast, which is the whole cost of this transformer.** Four features at
degree three is thirty-five columns; eight at degree four is four hundred and ninety-five. Ask
[`OutputFeatureCount`](polynomialfeatures-outputfeaturecount.md) before expanding a wide matrix,
because the expansion allocates all of it.

**[`PolynomialFeaturesOptions.InteractionOnly`](polynomialfeaturesoptions.md) keeps the products
and drops the powers**, which is what a caller wants when the question is whether two features act
together rather than whether one of them curves.

```csharp
using Lodestar.Preprocessing;

var options = new PolynomialFeaturesOptions
{
    Degree = 2,
    InteractionOnly = true,
    IncludeBias = false,
};

string[] names = PolynomialFeatures.FeatureNames(3, options);

string last = names[5];   // => x1 x2
int count = names.Length; // => 6
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PolynomialFeaturesOptions`](polynomialfeaturesoptions.md),
[`PolynomialFeatures.FeatureNames`](polynomialfeatures-featurenames.md), the
[Python equivalence table](../../../equivalence.md).
