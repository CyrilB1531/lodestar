# Distributions.StudentIsf

The `t` with `P(T > t) = p`: the inverse of `StudentSf`.

<!-- docs-declaration -->

```csharp
public static double StudentIsf(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `+∞` and `1` answers `−∞`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.isf(p, df)`, except where the root lies past the largest double, at a df below `1.05` and `p` near one half or tiny: `±∞` there, but the largest double of that sign where its rounded tail is `p`, or its smaller one the smaller of `p` and one less `p`, where scipy answers finite values that are roots of no tail (`t.isf(0.4, 1e-300)` is `6703.9`), and every `p` but one half from df `1e-20` down (#1690).

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when `df` is not positive.

**Example** — the upper 2.5% point on twelve degrees of freedom.

```csharp
using Lodestar.Stats;

double upper = Distributions.StudentIsf(0.025, 12.0);   // => 2.178812…
```

**Remarks** — The mirror of [`StudentQuantile`](distributions-studentquantile.md). **It reaches past `1.3e154`**: the Cauchy's (`df = 1`) point at `1e-300` is `1/(π·1e-300) = 3.18e299`, which an overflowing `t²` used to cap (#1158). **It searches for the smallest double whose upper tail before its last rounding falls to `p`**, but where its root lies past every double, `±∞` there as above unless at the largest double of that sign [`StudentSf`](distributions-studentsf.md) rounds to `p`, or the smaller tail there, as [`StudentSf`](distributions-studentsf.md) rounds it, to the smaller of `p` and one less `p`, which answers that double ([#1746](https://github.com/CyrilB1531/lodestar/issues/1746)), judged on the smaller tail, one less `p` past one half. **The contract is 8 ulps** (Cyril, 2026-10-29): the tail at the answer lies within 8 ulps of its target, or the answer lies next to the crossing where one ulp of the statistic moves the tail further, past the 14 to 15 digits Algorithm 708 certifies; the smallest double reaching it is the method's aim, not its promise. The tail is the regularized incomplete beta to 106 bits, rounded once, within 8 ulps of its exact value and monotone in the statistic to within that, and Newton steps on it settle the inversion's root, with a search of the doubles within 128 ulps of where they land, then of the root, then from the root however far; to df `1e-12` it is the first order's closed-form root, and from `1e20` the normal law's; at `p = 0.5` it is `0`, the median; where `p` and the inversion's root's tail are both subnormal it answers that root, the exact tail telling nothing finer than a subnormal's granule ([#1745](https://github.com/CyrilB1531/lodestar/issues/1745)).

A point past the largest double, such as the `1e-300` point on half a degree of freedom near `1e600`, is `±∞` rather than `±1.8e308`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md), [`Distributions.StudentSf`](distributions-studentsf.md).
