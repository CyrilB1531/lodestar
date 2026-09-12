# Factorization — `Lodestar.Decomposition`

Two factorizations of a sparse matrix, and the settings each takes. Both take a `CsrMatrix`
and neither centres it; what separates them is whether the components may be negative. A third
section answers the one PCA question .NET leaves open below `net8.0`, over a dense block.

## Truncated SVD

A term-document matrix has one column per word and one row per document, and almost every entry
is zero. This page factorizes it into a handful of dense components — latent semantic analysis —
**without centring it first**, which is the whole reason the operation exists: centring a sparse
matrix fills it in, and a corpus that fitted in memory as a `CsrMatrix` does not fit as a dense
one. That is also what separates this from PCA, which centres and is therefore a different
answer.

**So PCA's projection is not offered here, and that is a refusal rather than a gap.** For a dense
matrix, reach for ML.NET's `ProjectToPrincipalComponents` on any target this repository supports,
or [NumFlat](https://www.nuget.org/packages/NumFlat)'s `PrincipalComponentAnalysis` on `net8.0`
and above. The one thing neither gives you below `net8.0` — **how much variance each component
explains** — is the section further down.

The factorization is randomized, not exact. A thin random block Ω probes the matrix's range, a
few power iterations sharpen it, and the singular values fall out of a small dense problem whose
size is the rank you asked for rather than the size of the corpus. The cost is that two runs from
different Ω disagree in the last digits, which is why Ω is handled the way it is below.

**Ω is an input, not a seed.** [`TruncatedSvdOptions.RandomMatrix`](factorization/truncatedsvdoptions.md)
takes the block itself, so a run of this package and a run of scikit-learn can be compared entry
by entry instead of hoping two generators agree; `Seed` is there for when you only need
repeatability and draws from this package's own generator, which is not NumPy's. See
[ADR 0072](../../decisions/0072-omega-is-an-input-not-a-seed.md).

| Type | What it is |
| --- | --- |
| [`TruncatedSvd`](factorization/truncatedsvd.md) | A fitted factorization: its components, its singular values, and the projection of new rows onto them. |
| [`TruncatedSvdOptions`](factorization/truncatedsvdoptions.md) | Oversampling, power iterations, the normalizer, and Ω itself. |
| [`PowerIterationNormalizer`](factorization/poweriterationnormalizer.md) | What happens to the probe block between the two products of a power iteration. |

The whole procedure is two calls: [`TruncatedSvd.Fit`](factorization/truncatedsvd-fit.md) on the
corpus, then [`TruncatedSvd.Transform`](factorization/truncatedsvd-transform.md) on whatever you
want projected — including the corpus itself.

> **Nothing above is a topic model.** The components are signed and orthogonal, so a "topic" may
> subtract a word as readily as add one. Non-negative matrix factorization is the decomposition
> that answers that question, and it is the other half of this page.

## Non-negative matrix factorization

The same corpus, factorized into two blocks neither of which holds a negative number: `X ≈ W H`,
where a row of `H` is a component over the terms and a row of `W` is one document's mixture of
those components. Nothing subtracts, so a component is a set of words that occur together and a
document is a recipe rather than a coordinate — which is what makes the output readable in a way
an SVD's is not, and what costs it the SVD's uniqueness. The answer is a local minimum, and the
initialisation decides which one.

| Type | What it is |
| --- | --- |
| [`Nmf`](factorization/nmf.md) | A fitted factorization: `W`, `H`, how many updates ran, and how far the product is from the matrix. |
| [`NmfOptions`](factorization/nmfoptions.md) | The loss, the initialisation, the iteration cap, the tolerance, and Ω. |
| [`NmfBetaLoss`](factorization/nmfbetaloss.md) | What the factorization minimises — a Gaussian noise model or a Poisson one. |
| [`NmfInitialization`](factorization/nmfinitialization.md) | Where the iteration starts — the two NNDSVD variants this package ships. |

## The variance principal components explain

ML.NET's fourteen public PCA members expose the eigenvectors and the mean and no eigenvalue, and
NumFlat's `EigenValues` ships `net8.0` only, so below `net8.0` nothing in .NET could say how many
components a projection should keep
([decision 0116](../../decisions/0116-the-pca-gap-is-the-explained-variance-not-the-projection.md)).
This is that number and nothing more: centre a dense block, take the eigenvalues of its Gram
matrix, and read the share of the total variance each one carries. It takes a row-major span
rather than a `CsrMatrix`, because centring is exactly the step that densifies one —
[decision 0119](../../decisions/0119-the-explained-variance-lives-in-lodestar-decomposition.md)
has why it lives in this package all the same.

| Type | What it is |
| --- | --- |
| [`PrincipalComponentVariance`](factorization/principalcomponentvariance.md) | The variance, the ratio and the cumulative curve, one entry per component. |

## The kernel underneath, now published

The randomized SVD needs an orthonormal basis for a projected block, and this package writes its
own QR rather than take Math.NET — 5.0.0 dates from April 2022 and nothing stable has followed
([`decisions/0059`](../../decisions/0059-phase-0-verifications-two-confirmed-voids-do-not-survive-nuget.md)).
That kernel is published now, because a second package needs it rather than a second copy of it.
The LU and the one-sided Jacobi SVD beside it stay internal: nothing has asked for them, and an
unpublished API can still be published later where the reverse is not true.

| Type | What it is |
| --- | --- |
| [`QrDecomposition`](factorization/qrdecomposition.md) | A thin QR by Householder reflections: the orthonormal factor and the triangular one. |

The whole procedure is one call, [`Nmf.Fit`](factorization/nmf-fit.md), in either of two forms:
hand it a rank and it computes the initialisation, or hand it `W₀` and `H₀` and it runs the
updates on exactly those.

> **The updates are multiplicative, so a zero is permanent.** Every entry of `W` and `H` is scaled
> by a non-negative ratio, which is what keeps both factors non-negative with no projection — and
> what means the sparsity of the answer was chosen by the initialisation, not found in the data.

**See also** — the [Python equivalence table](../../equivalence.md), which maps every member on
these pages to the scikit-learn call it matches.
