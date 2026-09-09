# Distributions.StudentSf

The upper tail of Student's *t*: `P(T > t)`.

<!-- docs-declaration -->

```csharp
public static double StudentSf(double t, double df)
```

**Parameters** — `t` is the statistic. `df` is the degrees of freedom, which must be positive.

**Returns** — `scipy.stats.t.sf(t, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included.

**Example** — one tail, and the two-sided p-value a regression table prints.

```csharp
using Lodestar.Stats;

double upper = Distributions.StudentSf(2.0, 12.0);   // => 0.0343275…
double twoSided = 2.0 * upper;                        // => 0.0686550…

// Symmetric about zero, so the two tails of the same magnitude sum to one.
double whole = upper + Distributions.StudentSf(-2.0, 12.0);  // => 1
```

**Remarks** — **one tail, not two.** A regression reports `2 × StudentSf(|t|, df)` beside each
coefficient, and doubling is the caller's step rather than this method's, because a one-sided test
exists and would be wrong to double.

Accurate into the far tail, which is the reason this member is published rather than left to a
caller's own approximation: the corpus behind it reaches `3.1e-24` and is compared relatively, so
an implementation returning zero there fails rather than passing on an absolute tolerance.

`df` of `NaN` is refused rather than propagated. It fails every ordering, so a guard written as
`df <= 0` would have let it through and returned a `NaN` probability from a public method.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md),
[`Distributions.FisherSf`](distributions-fishersf.md), [`TTest`](../tests/ttest.md).
