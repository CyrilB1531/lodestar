# 0841 — The incomplete beta with one large and one small shape

**Status:** **retrospective** — written 2026-09-17, after the measurement it records; stacked on #837.

Issue: [#841](https://github.com/CyrilB1531/lodestar/issues/841), found while fixing
[#837](https://github.com/CyrilB1531/lodestar/issues/837).

## Change

- **Method.** `I_x(a, b)` with `a` large and `b` small comes from DiDonato and Morris's asymptotic
  expansion (BGRAT, ACM TOMS 1992), written from Temme's derivation (*Special Functions*, 1996,
  §11.3.3). With `x = e^-z` and `N = a + (b - 1)/2`, the integrand becomes
  `e^-Nu u^(b-1) (sinh(u/2)/(u/2))^(b-1)`; its even series integrates into
  `Gamma(a + b) / (Gamma(a) N^b)` times the sum of `c_k (b)_2k N^-2k R(b + 2k, Nz)`, where `R` is
  `Q` or `P`, whichever is under about a half, advanced by the recurrence
  `Q(s + 1, w) = Q(s, w) + w^s e^-w / Gamma(s + 1)`. The `c_k` follow from the Bernoulli series of
  `log(sinh(v)/v)`, and the leading ratio from the Bernoulli-polynomial expansion of two log-gammas
  (DLMF 5.11.8), whose even orders cancel. The sum stops at a relative `3e-16`; 64 terms is a guard
  no grid point came near, the most taken being 22.
- **Region.** The large shape at least 1000, `b^3 <= a^2`, and `x >= 1/2` in its own variable, in
  either orientation. The terms fall by about `b^3 / (24 a^2)` near the mean and
  `(log(x) / 2 pi)^2` away from it, and those two bounds cap both. Below `a = 1000` the continued
  fraction stays: its error there is under `5e-12`, and the expansion was up to 90% slower at
  `a = 100`.
- **The argument's complement.** `RegularizedIncomplete` gains an overload taking `y = 1 - x` as the
  caller formed it. `StudentSf` and `FisherSf` form both halves by division, which replaces
  `StudentSf`'s `1e-9` test of whether `1 - x` was still good enough: at `df = 2e8` and `t = 1`,
  rounding `x` moves `y` by `1e-8` relative.

## Measured

Against exact sums `I_x(a, n) = x^a sum (a)_k (1-x)^k / k!` in 60-digit decimals for integer `b`,
and scipy 1.18.1's `betainc` otherwise, both tails:

| grid | points | #837 worst relative | fix worst relative |
| --- | ---: | ---: | ---: |
| `a` from 1e3 to 1e8, `b` from 0.1 to 50, 15 quantiles | 1,919 | 2.7e-6 | 9.9e-15 |
| `b^3 / a^2` from 0.01 to 3, `a` from 60 to 1e8 | 540 | 4.2e-11 | 1.2e-11 |
| `a` to 1e15, `b` from 1e-8 to 7 | 256 | 5.2e2 | 4.3e-8 |
| shapes 0.1 to 1e5, both tails (broad) | 2,025 | 3.8e-6 | 3.8e-6 |

- On the broad grid no point moved away from its reference by more than `1.2e-13`. Its `3.8e-6` is
  scipy's, at `I = 2.2e-268`, where the exact sum sides with both versions here.
- The second grid's `1.2e-11` is at `b^3 = 3 a^2`, outside the region and unchanged. In it, the
  expansion matched exact sums to `2.3e-13` up to `b = 215,443`, where the fraction was `4.2e-11` off.
- The third grid's `4.3e-8` is at `b = 1e-8`, where `Q(b, w)` is formed as `1 - P` (the fraction
  was `1.3` off there). From `b = 1e-3` it held `6e-13`.
- Through the public tails, 14 points at `df` from `2e5` to `2e8`: `#837` was up to `4.3e-7` off,
  the fix `1e-15`, except two `f.sf` points where scipy is `1.5e-9` and `2.4e-11` from the exact sum
  and the fix is not. `docs/equivalence.md` records the gap.
- Six cases join `stats_distributions.json`, after the existing ones: three `t.sf` at `df = 2e7` and
  `2e8`, and three `f.sf` at `dfd = 2e5` or `dfn = 2`, where scipy is exact. `compare_oracles.py`
  shows only the count and the additions; with them stripped out, all 150 corpora agree.

Stopwatch, ns per call, #837 against the fix, the fastest of three alternated rounds, over 96 calls:
calls outside the region move by -2% to +4%. Inside it the worst is `I(1000, 100, 0.909)`, 262 to
279 (+6%); `I(1000, 0.5, 0.998)` goes 399 to 85, `I(1e8, 1e5, 0.999)` 2,302 to 168,
`t.sf(3, 20000)` 214 to 74.

## Rejected

- **A floor at `a = 100`.** `I(100, 15, x)` three standard deviations below the mean went 109 to
  212 ns, to gain digits on an error already under `5e-12`.
- **Forming the ratio `Gamma(a + b) / (Gamma(a) N^b)` from Stirling's `Gamma*` and
  `log(1 + t) - t`**, as `Front` does. It is as accurate, but its three series put the region's
  lower tail 14% behind the fraction.
- **Deriving `z` from `log(1 + t) - t` on `y`.** The gap between the two complements,
  `(1 - x - y) / x`, is exact at `x >= 1/2` and cost a division instead of a series.
