# Distributions.StudentIsf

The `t` with `P(T > t) = p`: the inverse of `StudentSf`.

<!-- docs-declaration -->

```csharp
public static double StudentIsf(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `+∞` and `1` answers `−∞`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.isf(p, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when `df` is not positive.

**Example** — the upper 2.5% point on twelve degrees of freedom.

```csharp
using Lodestar.Stats;

double upper = Distributions.StudentIsf(0.025, 12.0);   // => 2.178812…
```

**Remarks** — The mirror of [`StudentQuantile`](distributions-studentquantile.md). **It reaches past `1.3e154`**: the Cauchy's (`df = 1`) point at `1e-300` is `1/(π·1e-300) = 3.18e299`, which an overflowing `t²` used to cap (#1158).

A point past the largest double, such as the `1e-300` point on half a degree of freedom near `1e600`, is `±∞` rather than `±1.8e308`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md), [`Distributions.StudentSf`](distributions-studentsf.md).
