# Distributions.StudentQuantile

The value a Student's *t* falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double StudentQuantile(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `−∞` and `1` answers `+∞`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.ppf(p, df)`, except where the root lies past the largest double: `±∞` there, but the largest double of that sign where its rounded tail is `p`, or its smaller one the smaller of `p` and one less `p`, as [`StudentIsf`](distributions-studentisf.md) says (#1690).

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, or when `df` is not
positive. `NaN` is refused on both.

**Example** — the multiplier a 95% interval asks for.

```csharp
using Lodestar.Stats;

double multiplier = Distributions.StudentQuantile(0.975, 12.0);  // => 2.1788128296672284

// It is the value whose upper tail is the other 2.5%.
double tail = Distributions.StudentSf(multiplier, 12.0);          // => 0.02500000000000002

// An interval is then estimate ± multiplier × standard error.
double halfWidth = multiplier * 0.4;                              // => 0.871525…
```

**Remarks** — **this is the quantile, not the inverse survival function**, and the distinction is
not pedantic. The inverse survival function solves `P(T > x) = p` and carries the opposite sign;
the two are related by the distribution's symmetry about zero. Under that convention this method
would have returned `-2.18` where every printed table shows `+2.18`, and an interval built on it
would have been reflected through its own estimate.

**The endpoints answer the two infinities**, as scipy's do. They were refused until #1158, which
aligned every quantile of this class on the reference's support ends.

A point past the largest double, such as the `1e-300` point on half a degree of freedom near `1e600`, is `±∞` rather than `±1.8e308`. **It searches for the smallest double whose lower tail before its last rounding reaches `p`**, but where its root lies past every double, `±∞` there as above unless at the largest double of that sign [`StudentCdf`](distributions-studentcdf.md) rounds to `p`, or the smaller tail there, as [`StudentSf`](distributions-studentsf.md) rounds it, to the smaller of `p` and one less `p`, which answers that double ([#1746](https://github.com/CyrilB1531/lodestar/issues/1746)), judged on the smaller tail, one less `p` past one half. **The contract is 8 ulps** (Cyril, 2026-10-29): the tail at the answer lies within 8 ulps of its target, or the answer lies next to the crossing where one ulp of the statistic moves the tail further, past the 14 to 15 digits Algorithm 708 certifies; the smallest double reaching it is the method's aim, not its promise. The tail is the regularized incomplete beta to 106 bits, rounded once, within 8 ulps of its exact value and monotone in the statistic to within that, and Newton steps on it settle the inversion's root, with a search of the doubles within 128 ulps of where they land, then of the root, then from the root however far; to df `1e-12` it is the first order's closed-form root, and from `1e20` the normal law's; at `p = 0.5` it is `0`, the median; where `p` and the inversion's root's tail are both subnormal it answers that root, the exact tail telling nothing finer than a subnormal's granule ([#1745](https://github.com/CyrilB1531/lodestar/issues/1745)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentSf`](distributions-studentsf.md),
[`Distributions.FisherSnedecorSf`](distributions-fishersnedecorsf.md).
