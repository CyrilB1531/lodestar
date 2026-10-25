# Distributions.FisherSnedecorIsf

The `f` with `P(F > f) = p`: the inverse of `FisherSnedecorSf`.

<!-- docs-declaration -->

```csharp
public static double FisherSnedecorIsf(double p, double numeratorDf, double denominatorDf)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `+∞` and `1` answers `0`. `numeratorDf` and `denominatorDf` are the two degrees of freedom, each of which must be positive and finite.

**Returns** — `scipy.stats.f.isf(p, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when either degrees of freedom is not positive.

**Example** — the same 5% critical value, read from the upper tail.

```csharp
using Lodestar.Stats;

double critical = Distributions.FisherSnedecorIsf(0.05, 2.0, 20.0);   // => 3.492828…
```

**Remarks** — **More exact than scipy's at small `p`.** `scipy.stats.f.isf(1e-8, 1, 1)` is `5.0e-9` off in relative terms, measured against the closed form `1/tan²(π·p/2)` and against scipy's own `sf`; this one reproduces both to `4e-15`. **It searches for the smallest double whose upper tail before its last rounding falls to `p`**, judged on the smaller tail, one less `p` past one half. **The contract is 8 ulps** (Cyril, 2026-10-29): the tail at the answer lies within 8 ulps of its target, or the answer lies next to the crossing where one ulp of the statistic moves the tail further, past the 14 to 15 digits Algorithm 708 certifies; the smallest double reaching it is the method's aim, not its promise. The tail is the regularized incomplete beta to 106 bits, rounded once, within 8 ulps of its exact value and monotone in the statistic to within that, up to a harmonic shape `ab/(a + b)` of 1000, the chi-squared limit a half of the split below the normal doubles takes included. Past that shape, below shapes of `1e5` where Temme's expansion takes over, the tail is computed in doubles and steps thousands of ulps between runs of the statistic; there the answer is the inversion's root, which need not reach `p`. Elsewhere Newton steps on the tail before its last rounding settle the root, and a search of the doubles within 128 ulps of where they land, then of the root, then outward from the root, finds the answer. Two exceptions: where `p` and the root's tail are both subnormal it answers that root, the exact tail telling nothing finer than a subnormal's granule; and where the tail moves by less than an ulp of `p` over `2²⁰` doubles of the statistic, as near the median at dfs below about `1e-6`, or the tail is a lifted df's double form, it answers the doubles next to where the Newton steps land or to the inversion's root if the crossing is there, else that root, the search costing some 40 tails ([#1745](https://github.com/CyrilB1531/lodestar/issues/1745)). Below dfs of `6.2e-9`, where the incomplete beta's first order holds at the root it gives for `p`, the search starts from that root, judged there on the caller's own tail rather than the smaller, `1` for the median of equal dfs and `0` where the tail at the smallest double is already past `p` ([#1743](https://github.com/CyrilB1531/lodestar/issues/1743)). **An inversion that overflows past the largest double answers a double** where the smaller tail there reaches its target, `p` or one less `p` as that tail is the caller's or the other: the smallest double reaching it, searched as any other. A root past every double is `+∞` unless at the largest double [`FisherSnedecorSf`](distributions-fishersnedecorsf.md) rounds to `p`, or the smaller tail there, as it or [`FisherSnedecorCdf`](distributions-fishersnedecorcdf.md) rounds it, to the smaller of `p` and one less `p`: that double then, so `FisherSnedecorIsf(FisherSnedecorSf(double.MaxValue, 0.5, 0.5), 0.5, 0.5)` is `double.MaxValue` ([#1746](https://github.com/CyrilB1531/lodestar/issues/1746)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.FisherSnedecorQuantile`](distributions-fishersnedecorquantile.md), [`Distributions.FisherSnedecorSf`](distributions-fishersnedecorsf.md).
