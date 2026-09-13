# PrincipalComponentVariance

How much of a dense matrix's variance each principal component explains — the numbers a scree plot
is drawn from, and the ones a component count is chosen on.

<!-- docs-declaration -->

```csharp
public sealed class PrincipalComponentVariance
```

**Example** — three features, two of which move together.

```csharp
using Lodestar.Decomposition;

// Five samples of three features; the third is nearly twice the first.
double[] matrix =
[
    1.0, 2.0, 2.1,
    2.0, 1.0, 3.9,
    3.0, 4.0, 6.2,
    4.0, 3.0, 7.9,
    5.0, 5.0, 10.1,
];

PrincipalComponentVariance variance =
    PrincipalComponentVariance.Compute(matrix, rowCount: 5, columnCount: 3);

int components = variance.ComponentCount;                            // => 3
double first = variance.ExplainedVarianceRatio[0];                   // => 0.95191178727706…
double firstTwo = variance.CumulativeExplainedVarianceRatio[1];      // => 0.99999072700625…
```

**Remarks** — **this is not a PCA.** There are no components to read and nothing to project onto:
the projection is delegated, to ML.NET's `ProjectToPrincipalComponents` on any target or to
NumFlat's `PrincipalComponentAnalysis` on `net8.0` and above
([`decisions/0116`](../../../decisions/0116-the-pca-gap-is-the-explained-variance-not-the-projection.md)).
What neither answers below `net8.0` is how many of those components are worth keeping, and that is
all this type does. Compute it here, choose the count off `CumulativeExplainedVarianceRatio`, and
hand the count to whichever projection you use.

**Dense, and the input's own scale.** Nothing is standardised: a feature measured in thousands
dominates one measured in units, which is PCA's behaviour and scikit-learn's. Standardise first with
`Lodestar.Preprocessing`'s [`StandardScaler`](../../preprocessing/scaling/standardscaler.md) when the features' units are not comparable. A
`CsrMatrix` is refused by construction rather than densified, because centring fills it in
([`decisions/0119`](../../../decisions/0119-the-explained-variance-lives-in-lodestar-decomposition.md)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PrincipalComponentVariance.Compute`](principalcomponentvariance-compute.md),
[`TruncatedSvd`](truncatedsvd.md), the [Python equivalence table](../../../equivalence.md).

## Properties

| Property | What it holds |
| --- | --- |
| `SampleCount` | `int` — rows of the matrix, each one sample. |
| `FeatureCount` | `int` — columns of the matrix, each one feature. |
| `ComponentCount` | `int` — the smaller of the two: a block of `n` samples has at most `n` components however many features it has. |
| `TotalVariance` | `double` — the sum of the columns' sample variances, which is what the components share out. |
| `ExplainedVariance` | `IReadOnlyList<double>` — the variance along each component, largest first, over `n − 1`. |
| `ExplainedVarianceRatio` | `IReadOnlyList<double>` — each of those over `TotalVariance`. |
| `CumulativeExplainedVarianceRatio` | `IReadOnlyList<double>` — the running sum of the ratios, ending at one. |

## Members

| Member | What it does |
| --- | --- |
| [`PrincipalComponentVariance.Compute`](principalcomponentvariance-compute.md) | Computes the variance each principal component of a row-major matrix explains. |
