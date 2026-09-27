# Lodestar.Stats review findings before 1.0

**Issues:** [#1243](https://github.com/CyrilB1531/lodestar/issues/1243),
[#1244](https://github.com/CyrilB1531/lodestar/issues/1244),
[#1245](https://github.com/CyrilB1531/lodestar/issues/1245),
[#1246](https://github.com/CyrilB1531/lodestar/issues/1246),
[#1247](https://github.com/CyrilB1531/lodestar/issues/1247),
[#1248](https://github.com/CyrilB1531/lodestar/issues/1248).
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The full-repository review of `main` at `9b142ab2` found six places where `Lodestar.Stats`
answered differently from scipy 1.18.1:

1. `FisherExact.Test` counted a table as extreme within `1 + 1e-7`, `binomtest`'s margin, where
   `fisher_exact` sets `epsilon = 1e-14`. On `[[20, 38], [40, 106]]` the table at `k = 14` is
   likelier than the observed one by `8.4e-8` and was counted: p `0.394` against `0.314`.
2. `OneWayAnova.Test` recognised constant groups by a within-group sum of squares of exactly zero.
   Three copies of `0.1` average to `0.10000000000000002`, the sum is `1.2e-33`, and two such
   identical groups gave `F = 16, p = 0.016` where scipy answers `(NaN, NaN)`.
3. `KruskalWallis.Test` passed H to the regularized Q directly. The values `1` to `148` split into
   two groups with equal rank sums give `H = -5.7e-14`, and the Q threw
   `ArgumentOutOfRangeException`; scipy's `chdtrc` answers NaN.
4. `Correlation.Coefficient`, under `Pearson.Test` and `PointBiserial.Test`, recognised a constant
   sample by a largest deviation of exactly zero, with the same rounding as (2).
5. `BinomialTail.Mass` formed the mass from three log-gammas, each near `2e9` past `1e8` trials,
   so its last bits were `1e-7` of the mass — the margin the two-sided cut compares at.
   `Binomial.Test(49_999_999, 100_000_000)` answered `0.99992` where scipy answers `1`.
6. `ChiSquared.GoodnessOfFit` refused totals more than `1e-8` apart relative to the observed total,
   where `chisquare` allows `sqrt(eps)` relative to the smaller total.

## Decisions

1. **`FisherExact`'s margin is `1e-14`**, scipy's. `Binomial` keeps `1e-7`, which is what
   `binomtest` uses.
2. **Constancy is read off the values, each test the way its scipy counterpart reads it.**
   `OneWayAnova` takes consecutive differences, `f_oneway`'s `diff(x) == 0`: every group constant is
   `+∞` and the pooled sample constant is `NaN`, as `f_oneway` sets them after the arithmetic. The
   difference and not equality, because `inf - inf` is NaN: `[[inf, inf], [1, 1]]` is NaN in scipy,
   and equality would have answered `+∞`. `Correlation.Coefficient` takes equality with the first
   value, `pearsonr`'s `x == x[0]`, so three infinities are constant there. A NaN never counts as
   constant, so it still reaches the arithmetic and answers NaN, as scipy's `nan_policy` wrapper
   answers before the test runs.
3. **A negative H answers a NaN p-value** and is returned as computed, what scipy returns. Clamping
   H at zero would answer `p = 1`, a value scipy does not give.
4. **The binomial mass is Loader's saddle-point form**: Stirling's error terms for `n`, `k` and
   `n - k` and two deviances `x log(x / np) + np - x`, each deviance expanded in
   `(x - np) / (x + np)` near the mean, and the factorials taken directly below 16. Written from
   the paper (Catherine Loader, *Fast and Accurate Computation of Binomial Probabilities*, 2000).
   Over 20,000 random cases up to `2e9` trials it stays within `5.7e-11` of scipy's `binom.pmf`,
   where the log-gammas reached `1.5e-5`; against a 50-digit reference it is closer than scipy.
5. **The chi-squared sum check is scipy's**: `|Σobs − Σexp| / min(Σobs, Σexp) > sqrt(eps)`.

## Verification

- One frozen case per issue, each failing on `main` and passing here: `a near tie that 1e-7 would
  count`, the three constant-group ANOVA cases, `a balanced split whose H rounds below zero`,
  `six pairs, x a repeated 0.1`, the two large binomial counts, and `totals apart by 1.2e-8`.
- A random differential against scipy 1.18.1 over 16,080 cases — 2,979 Fisher tables, 2,969
  ANOVA and 3,132 Kruskal-Wallis group sets (constant groups included), 3,000 Pearson pairs, 2,000
  two-sided binomial counts up to `2e9` trials, 2,000 chi-squared sum checks around the bound:
  1,567 mismatches on `main`, 0 here.

## Rejected

- **Clamping H at zero**, which the issue offered: scipy answers NaN there, and aligning is the
  rule.
- **Porting Boost's `ibeta_derivative`**, which is what scipy's `binom.pmf` calls. The saddle-point
  form is shorter, needs no Lanczos table, and is already more accurate than the reference.
