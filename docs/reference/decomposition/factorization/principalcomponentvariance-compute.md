# PrincipalComponentVariance.Compute

Computes the variance each principal component of a row-major matrix explains.

<!-- docs-declaration -->

```csharp
public static PrincipalComponentVariance Compute(ReadOnlySpan<double> matrix, int rowCount, int columnCount)
```

**Parameters** — `matrix` is the matrix, row-major: `columnCount` values per row, one row per
sample. It is copied, not centred in place. `rowCount` is how many samples it has, at least two
because a sample variance divides by `n − 1`; `columnCount` is how many features, at least one.

**Returns** — the variance, the ratio and the cumulative curve, one entry per component, largest
first.

**Exceptions** — `ArgumentOutOfRangeException` when `rowCount` is below two or `columnCount` is not
positive. `ArgumentException` when `matrix` does not hold `rowCount × columnCount` values, holds a
`NaN` or an infinity, or has no variance at all because every column is constant.

**Example** — fewer samples than features caps the component count at the samples.

```csharp
using Lodestar.Decomposition;

// Two samples of three features.
double[] matrix = [1.0, 0.0, 2.0, 0.0, 1.0, 2.0];

PrincipalComponentVariance variance =
    PrincipalComponentVariance.Compute(matrix, rowCount: 2, columnCount: 3);

int components = variance.ComponentCount;               // => 2
double first = variance.ExplainedVarianceRatio[0];      // => 1
```

**Remarks** — **the last component of a wide block is empty.** Centring removes one degree of
freedom, so `n` samples span at most `n − 1` directions, and the `n`-th component explains nothing.
scikit-learn keeps it at `min(n_samples, n_features)` and so does this; it reads as a zero at the
tail of `ExplainedVarianceRatio`, not as a missing entry.

**A matrix with no variance is refused.** scikit-learn divides by the zero total and answers `NaN`
for every ratio; a scree plot of `NaN` tells a caller nothing they could act on, so this throws
instead and names the reason.

The eigenvalues are taken from the Gram matrix of the centred block, the smaller of `XᵀX` and
`XXᵀ`. That squares the condition, as scikit-learn's own `covariance_eigh` solver does for tall
blocks, and costs absolute accuracy near `ε · λ₁` — below anything a ratio can show. Against
scikit-learn's `svd_solver="full"` the frozen corpus agrees at `1e-9`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PrincipalComponentVariance`](principalcomponentvariance.md),
[`QrDecomposition.Householder`](qrdecomposition-householder.md), the
[Python equivalence table](../../../equivalence.md).
