# Distributions.StudentCdf

Student's *t* lower tail: `P(T ≤ t)`.

<!-- docs-declaration -->

```csharp
public static double StudentCdf(double t, double df)
```

**Parameters** — `t` is the statistic; `NaN` answers `NaN`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.cdf(t, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included.

**Example** — a one-sided lower p-value on ten degrees of freedom.

```csharp
using Lodestar.Stats;

double lower = Distributions.StudentCdf(-2.0, 10.0);   // => 0.036694…
```

**Remarks** — The upper tail of `−t`, by the law's symmetry, so a small lower tail keeps its digits.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentSf`](distributions-studentsf.md), [`Distributions.StudentQuantile`](distributions-studentquantile.md).
