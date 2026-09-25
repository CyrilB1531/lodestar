# Distributions.StudentQuantile

The value a Student's *t* falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double StudentQuantile(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `−∞` and `1` answers `+∞`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.ppf(p, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, or when `df` is not
positive. `NaN` is refused on both.

**Example** — the multiplier a 95% interval asks for.

```csharp
using Lodestar.Stats;

double multiplier = Distributions.StudentQuantile(0.975, 12.0);  // => 2.1788128296672298

// It is the value whose upper tail is the other 2.5%.
double tail = Distributions.StudentSf(multiplier, 12.0);          // => 0.024999999999999998

// An interval is then estimate ± multiplier × standard error.
double halfWidth = multiplier * 0.4;                              // => 0.871525…
```

**Remarks** — **this is the quantile, not the inverse survival function**, and the distinction is
not pedantic. The internal helper it delegates to solves `P(T > x) = p` and therefore carries the
opposite sign; the two are related by the distribution's symmetry about zero. Published under that
helper's own convention, this method would have returned `-2.18` where every printed table shows
`+2.18`, and an interval built on it would have been reflected through its own estimate.

**The endpoints answer the two infinities**, as scipy's do. They were refused until #1158, which
aligned every quantile of this class on the reference's support ends.

A point past the largest double, such as the `1e-300` point on half a degree of freedom near `1e600`, is `±∞` rather than `±1.8e308`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentSf`](distributions-studentsf.md),
[`Distributions.FisherSf`](distributions-fishersf.md).
