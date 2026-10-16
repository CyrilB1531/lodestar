# Student's quantile takes its density constant from the cached log B

**Issues:** [#1690](https://github.com/CyrilB1531/lodestar/issues/1690).
**Status:** written with the work, 2026-10-13.
**Date:** 2026-10-13.

`main` below is 724c7912, the commit this work started from.

## The problem

The Review B after #1688 timed `Distributions.StudentQuantile(1e-12, 500)` at 242.2 ns, against
229.2 on `main` before #1688 and 226.8 in Lodestar.Stats 0.5.0: 1.057 times, past the 1.05 the
project counts, on a path #1688 had not benchmarked. Measured again from the sources, `main` lost
about 12 ns a call from `df = 100` to `1999` at both `p = 0.975` and `p = 1e-12`: a constant, not a
per-step cost. #1688 added `LogBeta(df/2, ½)` to `StudentTail`'s constructor beside the two
log-gammas its density constant already took. Where Newton needs one or two tail evaluations, which
is most quantiles from `df = 100`, the one evaluation the cache saved did not pay for it.

## Decisions

- **One logarithm of the beta function for both.** The density's constant
  `Γ((df+1)/2) / (Γ(df/2) √(df π))` is `1 / (√df · B(df/2, ½))`, since `Γ(½) = √π`, so
  `StudentTail` now takes `_logNormalizer = −log B(df/2, ½) − ½ log df` from the `log B` it caches,
  instead of two log-gammas of its own. The cache is now taken for every df: from `df/2 = 1000` it
  replaces those log-gammas rather than adding to them, which was the fifth #1688 avoided there.
- **A huge df gets its digits back.** The old constant cancelled: off by `8e-11` at df `1e5`, `2e-4`
  at `1e12` and 18 in logs at `1e16`, while `-log B - ½ log df` stays within `1e-15`. Newton's last
  step is scaled by the density, so from df `1e16` the old quantiles strayed up to `7e-9` from scipy
  (`StudentIsf(0.025, 1e16)` read 1.9599639979722603 against 1.9599639845400545) and took ten to
  twenty times as long. They now match scipy 1.18.1 to `1e-14`, pinned at df `1e16`, `1e17` and
  `1e300`. The final step's dependence on the density stays: with a constant this accurate it moves
  the root by no more than its tolerance.
- **The far-tail leading term takes the same `log B`.** `FarStudentTail`, past `|t| = 1e50`, formed
  `log B(df/2, ½)` from three log-gammas, cancelling as the constant did; it now takes the
  inversion's cached value, or `LogBeta`'s.
- **Two defects the review found on `main`, fixed here.**
  - An infinite df is the normal law in `Beta.StudentSf` and `Beta.StudentQuantile`, as it already
    was in the public `Distributions` members. Welch's df overflows to `+∞` when its squared
    variance terms underflow (101 values of `±1e-80`), and `TTest.Independent` threw from its
    p-value and its interval; it now matches scipy's `ttest_ind`: statistic −70.714, p 0, interval
    (−1.028e-79, −9.72e-80).
  - The two limits of Student's law are taken in closed form. From df `1e20` it is the normal law
    as `Normal.Sf` computes it: the tails differ by `(t⁴−2t²−1)/4df` relative, under `6e-15` below
    `t = 38.6`, less than `Normal.Sf`'s own rounding of `z/√2` there (`1.1e-13` at `t = 37`). Past df
    `1e305`, `t²/(df+t²)` went subnormal near the median and lost up to `1.9e-7`. To df `1e-12` the
    tail is its first order in df, `½ − (df/2) asinh(t/√df)`, from `I_x(a, ½) = 1 + a(log x − 2 log(1 +
    √(1 − x))) + O(a²)`, equal to a 400-digit series to the last bit at df `1e-12`, `1e-16` and `5e-324`,
    and the quantile its inverse, `√df sinh(2(½ − p)/df)`, whose sinh overflows past every double: to
    df `1e-20` every quantile but the median is infinite. A first cut answered one half to df `1e-20`
    and left the general path above it, whose last ulp crossed one half, so the tail rose with `t` at
    df `1.1e-20`. A subnormal df read a tail of zero, and df `5e-324`, whose half rounds to zero, threw
    from a private parameter.
  - Between those limits, at a small df the power-law seed overflows, and Newton, started from 1,
    settled on 636242490419.0393 for every `p` at df `1e-300`. When the seed overflows and the tail
    at `double.MaxValue` still exceeds `p`, the root lies past every double and the answer is `+∞`.
    scipy answers finite values there that are roots of no tail (`t.isf(0.4, 1e-300)` = 6703.9), so
    infinity is the answer pinned, and `docs/equivalence.md` says so.
  - Just above df `1e-20` the tail read above one half and rose with `t`: `log B(a, ½)` is about
    `−log a`, and its rounding cost up to 40 ulps of the tail. Below a shape of `0.1` the fraction
    and the far-tail term divide by that shape as `exp(−log(s B))`, which has no such term; the
    inversion caches it. `log(s B) = log Γ(1+s) − (log Γ(x+s) − log Γ(x))` is taken to `s ε`
    absolute: the first by its Taylor series to `s²⁰`, the second shifted past 10 by `log1p(s/x)`
    steps and then by Stirling with the corrections' difference term by term, since the log-gammas
    themselves hold only `1e-15` absolute near one; `Gamma` holds the correction's seven
    coefficients once, as constants. The incomplete beta, the large-shape expansion included, is
    clamped to `[0, 1]`, which a few ulps of rounding left: F's tail read `−2e-15` at a shape of
    `1e-19`. The tail still moves by an ulp or two non-monotonically near `t = √df`
    below df `1e-15` (65 steps on a 1.3× sweep, against 266 on `main`), where it is one half to
    those ulps; resolving it would take the complement `1 − I` by a series of its own.
  - Near one half at such a df, Newton settled on non-roots (`1e-25` at `p = 0.49999999999999917`,
    df `1.13e-18`). Below df 1.05, past which it underflows, the quantile compares `p` with the tail
    at the largest double before inverting, whatever the seed, once `p` is under that tail's bound
    `x^a / 2`, `x = df/MaxValue²`, which holds since `a B(a, ½) ≥ 1`; evaluating the tail itself at
    every df below 2 cost df 1 11%. The inversion's fallback moves the exponent on both sides of 1:
    growth is the largest of `√low`, `2 low` and `low²`, and the midpoint is `√(low·high)`, where
    doubling a tiny bound, or halving from 1 toward a root of `1e-25`, took most of the 200
    iterations; `√low` alone never passed 1, which `TailInversionTests` holds.
  - From df `1e20` the quantile is scipy's `ndtri`, as at an infinite df, not the root of
    `Normal.Sf`, from which it differs by up to `1e-13`, `Normal.Sf`'s own rounding of `z/√2`.
  - F and chi-squared refuse a df whose half underflows (df `5e-324`) under the public parameter's
    name, after main's positivity checks: the shape it stands for is not a double, and rounding it up
    doubles a tail of order the shape.
  - The F inversion answered non-roots where the root lies outside the doubles; Student and
    chi-squared had gained an end check, F had not. `FisherFromTail` now compares the target with the
    tail at the smallest positive `f` and at `MaxValue`, below a shape of `0.1` only, past which that
    tail is within `e^-74` of its limit. On 6,000 random inverse tails, dfs in `[1e-6, 1e3]`, 2,108
    failed their own tail on `main`; one does here, its root a subnormal `f` of 15 bits. The root by
    the limit is tried only once the inversion lands below the smallest normal, where trying it
    first cost every quantile 170 ns.
  - A half of the F split below the smallest normal double takes its limit, by one rule,
    `Beta.TakesLimit`, for the tail, the density and the root: `y^s / (s B)` while `z = other · y`,
    `d₁f/2` or `d₂/2f`, is below `1e-17`, the terms it drops being of order `z`; past it the other
    shape is above `4e290` and the tail is the chi-squared limit `P(s, z)`, off by order `1/other`.
    The first form alone read `FisherSnedecorSf(1, 1, 1e308)` as `0.202` for `0.317`; restricted to
    small `z`, it left `FisherSnedecorCdf(1e-16, 1, 1e308)` to the incomplete beta, which read `0`.
    The root by the limit is kept where it holds the target closer than the inversion's, since its
    leading term divides by a shape. The F inversion looks at its ends only below a shape of `0.1`
    and only when its answer is not a root, each end on the tail the target keeps whole, and strictly:
    at shapes `1e-310` the tail at the largest double is one half to rounding, and the median is 1.
    The ends are `f`'s and stay in `FisherFromTail`: moved into `BetaQuantile`, as `x = ε` and
    `1 − x = ε`, they answered 155 of 6,000 random inverse tails wrongly, since at small shapes
    `1 − x` at `f = MaxValue` is far above the smallest double.
  - At subnormal shapes `log Γ` took the reflection, whose `π / sin(πx)` overflowed below
    `1.75e-308`; below a shape of `0.1` it is now `log Γ(1+x) − log x` by the series. In the shift of
    `LogGammaIncrement`, `log(x + s) − log x` cancelled at a tiny `x`; it is `log(s/x) + log1p(x/s)`.
    A zero residual steps zero in all four inversions, where `0 · ∞` at a density of `e^-714` sent the
    fallback to the smallest double; the beta seed's verdict that a root underflows divides by a
    shape, so the tail at the smallest double must exceed the target too. F's density at the origin
    for a numerator df of 2 is 1 exactly, `B(1, b) = 1/b`, where `log B` had left it `5e-14` off.
  - The beta inversion's seed took `log B` from three log-gammas, which cancelled at a shape of
    `5e199`, and a mean rounding to one put Newton on the bracket's end; its fallback, while the lower
    bound was zero, divided by eight, 200 iterations short of a root of `1e-200`. `Beta.LogBeta`, a
    seed below one and a geometric fallback against the smallest double bring F's inverse tail at a
    denominator df from `1e200` to `1.7e308` to scipy's `1.0000434271174663`, where `main` answered
    `+∞`.
  - `StudentLogOnePlusSquare` is `log1p(t/df · t)`, the density's form on `main`, with logs where
    that ratio overflows: a series and `log(1 + r²)`, shared for a while, lost `5e-11` at df `1e7`.
  - `Unclamped` and `ByFraction` are inlined: as calls they cost an F tail 5% at (10, 30); it is now
    within 4% of `main` there, a Student quantile faster from df 1 (160 against 200 ns at df 1) and an
    F quantile 1.7 to 3.6 times faster.
  - Student's density and its inversion share `Beta.StudentLogOnePlusSquare`, where each had its own
    overflow guard. Below df `1e20` the density is about `2e-15` from the normal one it then becomes,
    as the tail is; the cached small-shape constant `StudentTail` holds is not carried into
    `BetaQuantile`, whose F quantiles at small shapes are already 3.4 to 4.7 times faster than `main`.
  - Chi-squared below a shape of `0.1` had the same cancellation: `Q` was `1 − P`, and the prefactor
    took `log Γ(a)`, about `−log a`. `ChiSquaredSf(1, 2e-300)` read `−2e-14` for `6e-301`. With
    `P = x^a/Γ(1+a)·(1 + a T)`, `T = Σ (−x)ⁿ/((a+n) n!)`, `Q = −expm1(u) − a T eᵘ`,
    `u = a log x − log Γ(1+a)`, below `x = a + 1`, and `P = eᵘ(1 + a T)` the same way, where its
    series began at `1/a` and overflowed below a shape of `5.6e-309`; the prefactor is
    `a x^a e^(−x)/Γ(1+a)`, and
    `log Γ(1+a)` the series `Gamma.LogGammaOnePlus`, moved there from `Beta`. Over 72 points of a
    400-digit grid with a shape from `1e-300` to `0.1`, the worst is `5e-15`; from `0.1`, `1 − P`
    still costs up to `3e-14`, as on `main`. The tail and the density are 1.3 to 1.8 times faster
    there (64 to 49 ns, 43 to 24 ns at df 0.05).
  - The gamma and beta quantiles restarted their bracket's search when a converged step rounded onto
    a bound, where `TailInversion` already settled: up to 77 tail evaluations on `main`, at every df,
    for 1 to 21 now. A chi-squared quantile is 3.8 times faster at df 30 (805 to 213 ns) and an F
    quantile 4.7 times at df 0.05 (1720 to 369 ns); none is slower (Stopwatch A/B/A/B).
    A step under tolerance alone accepted non-roots near a pole of the density
    (`FisherSnedecorIsf(0.7, 30, 1e-8)` read `1.2e307`, whose tail is `0.9999964`), so the three
    inversions settle on a bound only with the log residual below `1e-12` too.
  - Where `d₁f / (d₁f + d₂)` or its complement underflows to zero, the F tail and density are taken
    from `log y` as `y^s / (s B(s, other))`, exact to a relative `y` below `1e-308`: the zero read as
    the support's end, and `FisherSnedecorSf(1e-320, 1e-8, 1)` was `1` for `3.8e-6`. The chi-squared
    upper-tail solve answers `0` where the tail at the smallest double is below `p`, as the lower
    tail's seed already did, where it walked down to `2e-258` for `ChiSquaredIsf(1e-6, 1e-10)`.
  - Past the mode with the second shape `b` below `0.1`, `1 − I_y(b, a)` cancelled: F's tail was
    `1.7e-8` off at `FisherSnedecorSf(0.5, 1e-8, 5)` on `main`, read zero at a shape of `1e-300`,
    and `1.4e-12` off just above a shape of `1e-3`. From `B_y(b, a) = Σ (1−a)ₙ/n! y^(n+b)/(n+b)`,
    with `δ = log(b B) − b log y`, `I_x(a, b) = −expm1(−δ) − b S e^(−δ)`, `S` the rest of the sum,
    which needs no subtraction from one. Against a 400-digit series over 280 F and Student points
    with a shape from `1e-300` to `0.1`, the worst is now `1.9e-15` (`3.3e-14` at a value of
    `5e-62`, where `main`'s fraction prefactor is the same); scipy 1.18.1 is within `4.3e-15`
    there, and is what the test pins. Below shape `0.1` an F tail costs 1.3 to 1.7 times (126 to 184 ns
    at `(0.1, 5)`), while Student's tail (116 to 81 ns), quantile (216 to 145 ns) and density (57 to 42 ns) at df `0.05` are
    faster, Student's `log(a B(a, ½))` taken from three log-gammas on every path, tail, quantile cache,
    far tail and density alike, so each inverts the others' function (Stopwatch A/B/A/B against
    `main`); F keeps the increment, which its reflected side needs to `s ε`, in the inversion's cache
    and its uncached tail alike. Student's tail at df 10 is faster, 189 against 199 ns. Above it an F tail is
    within 4% of `main`, 63.9 against 61.6 ns at `(2, 20)`, and the quantile faster at every df measured.
  - `StudentQuantile(0.5, df)` answered `−0`, the negation of the internal `+0`; it answers `+0`, as
    scipy and `NormalQuantile` do.
  - Student's density constant stays written twice, in `Densities.Student` and in the inversion.
    Taking the density's from `LogBeta` too made `StudentPdf` 1.3 to 2 times slower (54 to 71 ns at
    df 5.5, 29 to 58 ns at df 30, Stopwatch, 2e6 calls), so it keeps its two log-gammas. It now takes
    the normal density from df `1e20`, as the tail does, and below df `0.2` the inversion's constant
    from `log(a B)`, where df `5e-324` threw. Past `|t|/√df = 1e8` it takes `log(1 + t²/df)` as
    `2 log(|t|/√df)`, where `t²/df` overflowed and `StudentPdf(1e140, 1e-30)` read 0 for `5e-171`,
    as scipy's does.
  - `StudentTail` keeps four fields: a fifth, caching `½ log df` for the far density, stopped the JIT
    promoting the struct and cost 6% at df 3 (Stopwatch A/B/A/B), so the far branch takes the log.
  - For df below one a root in the top binade came back infinite: the density's `r = x/√df`
    overflowed, and so did the bisection's midpoint once both bounds passed half the largest double.
    Both are taken without overflowing now.
- **Both shapes large: Temme's uniform expansion.** The fraction needs about `√a` terms at the mean,
  so past a shape of `1e10` its million stopped short: `FisherSnedecorSf(1, 1e18, 1e18)` read
  `0.6956` for `0.5` on `main`, and `1e155` read `NaN`. From both shapes `1e5`, `UniformBeta` takes
  `I_x(a, b) = ½ erfc(−η√(s/2)) − K e^(−sη²/2)(h₀/s + h₁/s²)` (Temme, 1996, 11.3.3; DLMF 8.18.ii),
  `h₀` and `h₁` by their series in `t = δ/min(p, q)` near the mean, where the direct forms cancel,
  and directly past `|t| = 0.05`; in `δ` the series carried `1/q¹⁵` and overflowed at `q = 1e-24`.
  Against a 60-digit continued fraction over 33 shape pairs from `1e3` to `1e20`, both orders, at
  six points from the mean to 25 standard deviations, it holds `2e-13` from `1e5` (at `(1e20, 1e5)`
  and 25 deviations, a tail of `3e-296`, it and the fraction both sit `1.4e-11` off), where `main`'s
  fraction drifted to `6.4e-11` at `(1e7, 1e10)` and the large-shape expansion to `7.9e-12` at
  `(1e7, 1e14)`; at shapes of `1e3` its dropped term costs `5e-9`, where the fraction keeps `7e-14`,
  so `1e5` is the threshold, and the uniform expansion is tried before the large-shape one.
- **The prefactor against the exact mean.** `a log(x/p) + b log(y/q)` is taken as
  `a L(D/p) + b L(−D/q)`, `L(t) = log(1 + t) − t`, with `D = x − a/(a + b)` exact: the rounded
  `p = a/c` is corrected by `(a − c p − e p)/c`, `e` the rounding of `c = a + b` by Knuth's two-sum and
  `a − c p` by a fused multiply-add, or Dekker's product on netstandard2.0 (`src/Shared/ExactProduct.cs`,
  compiled into Stats alone). Against the rounded `p` the linear terms left `(a+b) δ ε`, `1e-12` of a
  tail at `(3e6, 1e7)` on `main`; without `e`, at `(5e29, 3e32)` the exponent came out positive and
  F's tail fell from `0.25` to `0` as `f` grew. `D` is taken on the side of the smaller of `x` and `y`,
  so exchanging the shapes and points negates it exactly, and the uniform expansion's sign and
  exponent come from the same `D`: a zero sign beside a nonzero exponent read both tails as one.
  F's split rounds `x` through the same sum as the mean, so for F's tail and density `D` is formed
  from `f − 1` instead, `(f − 1)(x/f) d₂/(d₁ + d₂)`: from `x` it put the law's centre thousands of
  standard deviations off past shapes of `1e30`, and `FisherSnedecorSf(1, 1e100, 1e60)` read 0. `1 + t`
  is taken against the same mean, as a factor, so the log branch agrees with `t`; as a difference it
  cost `6e-6` of F's tail at `(1e-10, 30, 100)`. On netstandard2.0 Dekker's product trades a
  factor of `2^600` between its operands where a split would overflow, scales a product past the
  largest double down with the minuend, and one near the subnormals up, matching the fused
  multiply-add at the edges as well.
- **The inversions at huge and tiny shapes.** Measured by a random sweep of 72,000 quantiles and inverse
  tails, F, chi-squared and Student, shapes log-uniform from `1e-300` to `1e300`, each checked against
  its own tail within 1,024 ulps (8 where the law is narrower than `1e-12`), and its implied relative
  error where the tail is flat:
  - Newton's density cancelled `log B` or `log Γ`, of order `a log a`, against `a log x`: 5 nats at
    shapes `1e15`, so the F median at `(2e15, 2e15)` read `0.99999938` on `main`, 14 standard deviations
    off, and `ChiSquaredQuantile(0.3, 2e15)` `1.2e-10` off. From a shape of `1e5` the beta inversion
    takes the density from the prefactor's exponent and the gamma one from Stirling's.
  - The step tolerance, `1e-13` in `log x`, is held to `1e-8` of the law's relative width once that is
    below `1e-5`, past shapes near `1e10`, where a fixed one outgrew the law. Where the law is narrower
    than `1 − x` resolves, the F quantile refines the small side, which a root above `1e-3` skipped.
  - The large-shape expansion chose `P` or `Q` by `w ≥ b`, wrong below a shape of `0.1`, where
    `P(b, w)` is near one far below `b`: `FisherSnedecorCdf(1, 8e106, 8e-52)` read `2.8e-15` for
    `5e-50`. The F root by the half's limit accepted an `f` that underflowed to zero, whose half is
    zero too: `FisherSnedecorQuantile(1.3e-204, 1e189, 4.6e-207)` read `0` for `1.17e44`, against a
    50-digit chi-squared limit, which scipy's `f.ppf` reads as `2e-88`.
  - Newton's residual `log target − log value` rounds by an ulp of the log, and a slope under one
    carries that into `x`; where `|log target / slope|` passes 4 it is taken as `log(target/value)`.
    The gamma inversion's growth past an unbounded root is `TailInversion`'s, `max(√x, 2x, x²)`, where
    four times per step left `ChiSquaredIsf(1.6e-284, 5.2e-228)` far short of `249.2`; `main` read
    `1e-323` there.
  - `erfc` at zero kept the interpolant's `1 − 1.6e-15` in `Normal.ErfcOfSquareRoot`, which `Erfc`
    already special-cased: `FisherSnedecorSf(1, 2e5, 2e5)` read `0.4999999999999839` on `main`.
  - The three inversions end by one routine, `TailInversion.Finish`, where the gamma and beta ones had
    their own copies. A Newton step under an ulp tries the neighbouring double, a bracket of two
    neighbours settles on the upper one, the smallest `x` that passed the target, unless it is the
    bracket's unevaluated end; a floor of two ulps on the step tolerance, tried first, left a
    chi-squared quantile narrower than an ulp four past its crossing. At a narrow law the tail
    moves more per ulp than the `1e-12` residual a bound otherwise needs, and F's quantile at
    `(1e-3, 1e12, 2e12)` bisected 30 times. A small side narrower than the step tolerance reaches, its
    mean under `0.49`, is solved on that side from its normal approximation, and one narrower than an
    ulp is walked to from its mean; nearer one half `1 − x` loses nothing, and choosing the side by the
    mean alone had clamped `FisherSnedecorQuantile(0.95, 2e10, 2e10)` to 1. The gamma seed is
    Wilson-Hilferty from a shape of `1e5`, where the leading term, which drops `e^(−x)`, seeded near `a/e`.
  - `erfc` is erf's six-term series below `0.04`, where the interpolant, which swings within 20 ulps of
    it, sits 8 below, so the switch steps down; at `0.01` it stepped up and `NormalSf` rose. The
    incomplete beta clamps rounding into `[0, 1]`; answering NaN past `1e-13` of it, tried so a wrong path
    would show, would have turned a hypothesis test's p-value into NaN for rounding, and the sweeps
    above hold every path inside instead. Past an exponent of 750 the uniform expansion answers its
    limit, where `2 exponent / s` overflowed past `9e307` and a df of `1e306` read NaN.
  - At a law narrower than `1e-12` in `log F`, the F quantile walks from the beta's root to where F's
    own tail crosses, at most eight ulps each way: `f = d₂y/(d₁(1 − y))` rounds three times, and at
    `(0.144, 4.7e118, 4.5e282)` the quantile's lower tail read 0.
  - Costs, Stopwatch A/B/A/B against `main` on the same machine: an F quantile at `(2, 20)` 223 to
    141 ns, at `(100, 300)` 4.9 to 1.6 µs, at `(2e10, 2e10)` 5.7 to 3.0 µs, at `(1e-3, 1e12, 2e12)` 3.3
    to 1.5 µs and at `(0.3, 1e40, 2e40)` 4.5 to 0.56 µs; a chi-squared quantile at df 4 484 to 228 ns
    and at df `1e30` 4.6 to 0.2 µs; an F tail at `(100, 300)` within 2%, for the exact prefactor. The
    beta inversion takes `log B`, both `log(s B)` and `log target` once per solve, and none of them
    where both shapes reach the uniform expansion, which reads none.
- **Values move inside Newton's tolerance, not in accuracy.** The density sets only the length of
  Newton's steps, so a different rounding of its constant can stop the inversion at a neighbouring
  root within tolerance. Over 147 df from 0.01 to 1e12, by 139 p from 5e-324 to `1 − 1e-16`,
  quantile and inverse upper tail, 1,848 of 20,433 lines differ from `main`, by 1 to about 9,000
  ulps (about 1e-12 relative, in the deepest tails). Judged by scipy 1.18.1's `t.cdf` and `t.sf`
  where it is reliable, `main` is closer on 690 and this change on 666, with 455 ties. Every oracle
  corpus still matches at `1e-9`. One of #1688's eleven pinned bits, `(1e-20, 20)`, moved one ulp
  and is pinned at its new value; the CHANGELOG says a quantile can move by a few ulps. Cyril chose
  this over a lazily taken, bit-identical cache, which would have saved less (2026-10-13).
- **Measured with BenchmarkDotNet** 0.14.0, short job, AMD Ryzen 7 8700G, .NET 10.0.12. The
  sources of `main` before #1688, `main` and this change were compiled into one harness each and run
  in A/B/A order twice. The figures are the mean of the two runs, in ns:

  | df | p | before #1688 | `main` | this change |
  | --- | --- | ---: | ---: | ---: |
  | 3 | 0.975 | 748 | 631 | 558 |
  | 3 | 1e-12 | 319 | 266 | 213 |
  | 30 | 0.975 | 616 | 603 | 547 |
  | 30 | 1e-12 | 638 | 567 | 496 |
  | 100 | 0.975 | 418 | 430 | 385 |
  | 100 | 1e-12 | 388 | 363 | 309 |
  | 500 | 0.975 | 492 | 505 | 455 |
  | 500 | 1e-12 | 229 | 240 | 193 |
  | 1999 | 0.975 | 500 | 514 | 460 |
  | 1999 | 1e-12 | 222 | 236 | 185 |
  | 2000 | 0.975 | 183 | 188 | 187 |
  | 2000 | 1e-12 | 198 | 200 | 199 |
  | 4000 | 0.975 | 179 | 181 | 173 |
  | 4000 | 1e-12 | 184 | 184 | 185 |
  | 1e5 | 0.975 | 159 | 166 | 160 |
  | 1e5 | 1e-12 | 169 | 176 | 163 |
  | 1e8 | 0.975 | 155 | 157 | 152 |
  | 1e8 | 1e-12 | 166 | 170 | 163 |

  From `df = 2000`, where the cache is new, this change stays within 1.02 times `main` before
  #1688.

- **Carried from the Review B after #1688:**
  - the Stats CHANGELOG entry #1686 lacked;
  - `bench/README.md` §31's first sentence, whose subject was a file path rather than #709;
  - `Version.props`' note that Stats has no inter-package edge.
- **Carried from the Review B after #1698:** the blank line PEP 8 wants before a top-level
  definition in `tools/tests/test_nightly_budget.py`.
