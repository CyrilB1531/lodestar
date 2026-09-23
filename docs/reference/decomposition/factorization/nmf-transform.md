# Nmf.Transform

`W` for an unseen matrix, with this fit's `H` held fixed.

<!-- docs-declaration -->

```csharp
public double[] Transform(CsrMatrix matrix)
```

**Parameters** — `matrix` holds the rows to factorize against this fit, and must have exactly
`FeatureCount` columns. It does not have to be the matrix that was fitted, and usually is not.

**Returns** — `double[]`, row-major and `ComponentCount` wide: `matrix.RowCount × ComponentCount`
values, row `i`'s mix of components starting at `i * ComponentCount`.

**Exceptions** — `ArgumentNullException` when `matrix` is null. `ArgumentException` when `matrix`
has no row, does not have `FeatureCount` columns, or holds a negative value, a `NaN` or an
infinity.

**Example** — one unseen document scored against the fit from
[`Nmf.Fit`](nmf-fit.md)'s own example.

```csharp
using Lodestar.Abstractions;
using Lodestar.Decomposition;

CsrMatrix matrix = new(
    4, 3,
    [3.0, 1.0, 2.0, 1.0, 1.0, 4.0, 2.0, 3.0],
    [0, 1, 0, 2, 1, 2, 0, 2],
    [0, 2, 4, 6, 8]);

Nmf fitted = Nmf.Fit(matrix, 2);

// A document the fit never saw, over the same three terms.
CsrMatrix unseen = new(1, 3, [2.0, 5.0], [0, 2], [0, 2]);

double[] weights = fitted.Transform(unseen);

double firstComponent = Math.Round(weights[0], 3);    // => 2.046
double secondComponent = Math.Round(weights[1], 3);   // => 0.835
```

**Remarks — this iterates where [`TruncatedSvd.Transform`](truncatedsvd-transform.md)
multiplies.** Applying a row to an orthonormal basis is a product; applying it to a non-negative
one is another factorization, with `H` held fixed and only `W` free. That is why this member
costs what a small fit costs rather than what a matrix product costs, and why it honours the loss,
the iteration cap and the tolerance the fit was given.

**The settings are the fit's, and cannot be overridden.** The reference replays its estimator's
own parameters in `transform`, so the fitted object remembers them. A `Transform` that took
options would let a caller score under settings the model was never fitted under, which reads as
a feature and behaves as a trap.

**`W` starts from the mean, not from a draw.** The reference fills it with
`sqrt(matrix.mean() / ComponentCount)` — the mean over *every* cell, the structural zeros of a
sparse matrix included — and no `random_state` reaches it. That determinism is what lets this be
frozen against scikit-learn at `1e-9`, and re-implementing it by hand agreed to `1.7e-16`; the
[Python equivalence table](../../../equivalence.md) carries the measurement.

**A row of zeros comes back as zeros**, and a matrix of them comes back empty of weight: the mean
of an all-zero matrix is zero, so `W` starts at zero and a multiplicative update leaves it there.
The reference answers the same way rather than refusing.

**`H` is not touched.** The fit's `Components` are the same after a transform as before it, which
is what "held fixed" means and what makes a fitted `Nmf` reusable across as many batches as a
caller has.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Nmf`](nmf.md), [`Nmf.Fit`](nmf-fit.md),
[`TruncatedSvd.Transform`](truncatedsvd-transform.md), the
[factorization index](../factorization.md), the
[Python equivalence table](../../../equivalence.md).
