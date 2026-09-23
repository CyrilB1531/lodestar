# The tests around the ten families: Levene, Bartlett, Friedman, the binomial test and Anderson-Darling

**Issue:** [#1121](https://github.com/CyrilB1531/lodestar/issues/1121).
**Status:** written before the work, 2026-09-23.
**Date:** 2026-09-23.

## The problem

Thirteen families ship in `Lodestar.Stats` after
[#1120](https://github.com/CyrilB1531/lodestar/issues/1120), and five `scipy.stats` tests sit
directly beside them with no row in `docs/equivalence.md` — neither written, nor delegated, nor
recorded as a void.

The first two are the sharpest, because **a test that already ships depends on them**.
[`OneWayAnova.Test`](../../reference/stats/tests/onewayanova-test.md) assumes equal variances, and
[`TTest.Independent`](../../reference/stats/tests/ttest-independent.md) assumes them unless
`Variance.Welch` is asked for. The package sells those conclusions today and gives the caller no
way to check the assumption underneath them. `levene` and `bartlett` are that check.

The other three fill gaps of their own: `friedmanchisquare` is the repeated-measures counterpart of
[`KruskalWallis.Test`](../../reference/stats/tests/kruskalwallis-test.md), `binomtest` is the exact
one-sample proportion test where
[`ChiSquare.GoodnessOfFit`](../../reference/stats/tests/chisquare-goodnessoffit.md) answers only the
asymptotic version, and `anderson` is the normality test read beside
[`ShapiroWilk.Test`](../../reference/stats/tests/shapirowilk-test.md).

## What was measured

Every algorithm below was read out of scipy 1.18.1's own source, not recalled, and each formula was
replayed against the installed library before it was written down here.

### The five algorithms

**`bartlett`** — with `k` groups, `Ni` their sizes, `ssq_i` their variances at `ddof=1` and
`Ntot = Σ Ni`:

```text
spsq  = Σ (Ni - 1)·ssq_i / (Ntot - k)
numer = (Ntot - k)·ln(spsq) - Σ (Ni - 1)·ln(ssq_i)
denom = 1 + (1 / (3(k - 1)))·(Σ 1/(Ni - 1) - 1/(Ntot - k))
T     = numer / denom
```

p from the chi-squared upper tail at `k - 1`. **The statistic is clipped to `[0, ∞)` after the
p-value is taken, not before** — so a `T` that rounds negative is reported as `0` with the p-value
its negative value produced. Reproduced in that order.

**`levene`** — each group is centred by `Center`, then the absolute deviations are compared:

```text
Zij   = |x_ij - centre_i|
Zbari = mean_j Zij          Zbar = Σ Ni·Zbari / Ntot
numer = (Ntot - k)·Σ Ni·(Zbari - Zbar)²
denom = (k - 1)·Σ Σ (Zij - Zbari)²
W     = numer / denom
```

p from the F upper tail at `(k - 1, Ntot - k)`. The three centres are the median (scipy's default,
which makes this Brown-Forsythe), the mean (Levene's original), and a trimmed mean —
`scipy.stats.trim_mean`'s rule, `m = int(n · proportiontocut)` values dropped from each end of the
sorted group.

**A corpus trap, measured:** at `n = 5` and the default `proportiontocut = 0.05`,
`int(5 × 0.05) = 0`, so `Center.Trimmed` returns exactly what `Center.Mean` returns. A fixture that
small proves nothing about trimming, so the corpus carries a group long enough for the cut to bite.

**`friedmanchisquare`** — `k` treatments of `n` blocks each, `k ≥ 3`, ranked **within each block**
with ties averaged:

```text
ties = Σ over blocks and tie groups of t(t² - 1)
c    = 1 - ties / (k(k² - 1)·n)
ssbn = Σ over treatments of (rank sum)²
Q    = (12 / (k·n·(k + 1))·ssbn - 3n(k + 1)) / c
```

p from the chi-squared upper tail at `k - 1`.

**`binomtest`** — `less` is `cdf(k)`, `greater` is `sf(k-1)`, and two-sided sums every outcome no
more likely than the observed one, with the same `1 + 1e-7` relative guard
[`FisherExact`](../../reference/stats/tests/fisherexact-test.md) already carries:

```text
d = pmf(k),  rerr = 1 + 1e-7
k < p·n : ix = first index in [ceil(p·n), n] with pmf(ix) <= d·rerr
          y  = n - ix  (+1 when pmf(ix) equals d·rerr exactly)
          p-value = cdf(k) + sf(n - y)
otherwise: ix = last index in [0, floor(p·n)] with pmf(ix) <= d·rerr
          y  = ix + 1
          p-value = cdf(y - 1) + sf(k - 1)
```

clipped at 1. The binary search is scipy's own, and its contract — *the `i` with
`a(i) <= d < a(i+1)`* — is what decides the boundary term.

**`anderson`**, for `dist='norm'` — over `y = sort(x)` and `w = (y - mean) / sd(ddof=1)`:

```text
A² = -N - Σ_{i=1..N} (2i - 1)/N · (ln Φ(w_i) + ln(1 - Φ(w_{N+1-i})))
```

### `binomtest`'s exact interval is a root-find, not a beta quantile

Measured, and it changes what has to be written. scipy does **not** call `beta.ppf` for
Clopper-Pearson. It solves, with `alpha = (1 - level)/2` two-sided and `1 - level` one-sided:

```text
plow  solves  Binomial(n, p).sf(k - 1) = alpha        (0 when k = 0, or alternative 'less')
phigh solves  Binomial(n, p).cdf(k)    = alpha        (1 when k = n, or alternative 'greater')
```

Those are the Clopper-Pearson bounds, and the closed form is the beta quantile —
`plow = Q(alpha; k, n-k+1)`, `phigh = Q(1-alpha; k+1, n-k)` — because a binomial tail *is* a
regularized incomplete beta. So the two routes agree mathematically and neither reproduces the
other bit for bit; `1e-9` is what decides, as it does everywhere else here.

This package takes the closed form, which means **one new numerical routine**:
`Internal/BetaQuantile.cs`, inverting `Beta.RegularizedIncomplete(a, b, x)` in `x`. `Internal/Beta`
has the forward function and `Internal/TailInversion` does not apply — it inverts a tail symmetric
about zero, and this one lives on `(0, 1)` with no symmetry. The inversion is bracketed on `(0, 1)`
from the start, which is what makes bisection-with-Newton safe here where it needed care there.

Wilson's interval is closed-form and needs nothing new, in both the plain and the
continuity-corrected shapes Newcombe (1998) gives and scipy implements.

### `anderson` is mid-removal upstream, and this is the one hard call

On the pinned scipy 1.18.1, `stats.anderson(x, dist='norm')` already emits a `FutureWarning`: since
1.17 a `method` must be chosen, and **from 1.19 `critical_values`, `significance_level` and
`fit_result` disappear**, replaced by a `pvalue`. The issue asks for exactly the shape being
retired.

The consequence is not stylistic. A corpus frozen on that call stops being regenerable the day the
lock moves past 1.19: the *Oracles are reproducible* job would fail on an **exception**, not on a
numeric drift, which is a red `tools/compare_oracles.py` cannot explain.

Two facts about the survivor, both measured:

- `method='interpolate'` returns `SignificanceResult(statistic, pvalue)` and computes the p-value as
  `numpy.interp(A², critical, sig/100)`. `numpy.interp` **clamps at both ends**, so for `norm` the
  p-value can never leave `[0.01, 0.15]`. Beside `ShapiroWilk.Test`, which reaches `1e-15`, that is
  a convenience rather than a measurement.
- The critical values are a published table scaled by the sample size, and nothing else:

  ```text
  _Avals_norm = [0.561, 0.631, 0.752, 0.873, 1.035]      at 15%, 10%, 5%, 2.5%, 1%
  critical    = round(_Avals_norm / (1 + 0.75/N + 2.25/N²), 3)
  ```

  Verified against scipy at `N = 8`, `20` and `100`: the formula reproduces its output exactly.

So the corpus can hold all of it without depending on the shape being removed — the statistic and
the interpolated p-value are replayed from `method='interpolate'`, which survives 1.19, and the
critical values are **stated** from Stephens' constants and the scale above, the way
[`MultipleComparisons.Bonferroni`](../../reference/stats/tests/multiplecomparisons-bonferroni.md)'s
corpus states a definition scipy has no call for. Both forms ship;
`docs/equivalence.md` records the `FutureWarning`, the 1.19 removal and why the corpus is built this
way.

### The incumbents

Read out of the restored `netstandard2.0` assets by reflection, the protocol `bench/README.md` §18
set, never guessed:

| family | incumbent | signature |
| --- | --- | --- |
| Levene | `Accord.Statistics.Testing.LeveneTest` | `(double[][] samples, bool median)`, `(double[][] samples, double percent)` |
| Bartlett | `Accord…BartlettTest` | `(double[][] samples)` |
| binomial | `Accord…BinomialTest` | `(int successes, int trials, double hypothesizedProbability, OneSampleHypothesis alternate)` |
| Anderson-Darling | `Accord…AndersonDarlingTest` | `(double[] sample, IUnivariateDistribution<double> hypothesizedDistribution)` |
| Friedman | **none** | — |

`Meta.Numerics` 4.2.0 — the incumbent [#756](https://github.com/CyrilB1531/lodestar/issues/756) and
[#1120](https://github.com/CyrilB1531/lodestar/issues/1120) measure against — carries none of the
five. `Accord.Statistics` 3.8.0 is archived (November 2020, last published October 2017) and is
already a `bench/`-only `PackageReference`; it is **timed, never read**, decision 0002's rule.

Two consequences. **Friedman has no .NET incumbent at all**, so its comparison goes through
`bench/python/bench_stats.py` against scipy rather than through a BenchmarkDotNet pair. And
**Accord's Anderson-Darling takes the distribution as an argument**, so it is the known-parameters
test where scipy's estimates `loc` and `scale` from the sample; whether the two statistics are the
same quantity when Accord is handed `N(x̄, s)` is asserted before anything is timed, and the pair is
dropped and recorded rather than reported if they disagree.

### Where `NanPolicy` and `Alternative` go

The issue asks for "the same `Alternative` and `NanPolicy` shapes the neighbouring tests already
take" on all five. Measured signatures say otherwise, and decision 0007's rule — the parameter is
offered where the scipy counterpart takes it — decides each one:

| call | `nan_policy` | `alternative` |
| --- | --- | --- |
| `levene(*samples, center, proportiontocut, axis, nan_policy)` | yes | no |
| `bartlett(*samples, axis, nan_policy)` | yes | no |
| `friedmanchisquare(*samples, axis, nan_policy)` | yes | no |
| `binomtest(k, n, p, alternative)` | no | yes |
| `anderson(x, dist, method)` | no | no |

Three of five take `NanPolicy`; one of five takes `Alternative`. The variance and Friedman tests are
upper-tail by construction — there is no other tail for "the variances differ" to point at.

## What ships

```csharp
namespace Lodestar.Stats;

public enum Center { Median, Mean, Trimmed }
public enum ProportionInterval { Exact, Wilson, WilsonCorrected }

public static class Levene
{
    public static TestResult Test(params double[][] groups);
    public static TestResult Test(
        Center center, double proportionToCut, NanPolicy nanPolicy, params double[][] groups);
}

public static class Bartlett
{
    public static TestResult Test(params double[][] groups);
    public static TestResult Test(NanPolicy nanPolicy, params double[][] groups);
}

public static class Friedman
{
    public static TestResult Test(params double[][] treatments);
    public static TestResult Test(NanPolicy nanPolicy, params double[][] treatments);
}

public static class Binomial
{
    public static BinomialResult Test(
        int successes, int trials, double probability = 0.5,
        Alternative alternative = Alternative.TwoSided);
}

public sealed record BinomialResult(double Statistic, double PValue)
{
    public (double Low, double High) ProportionConfidenceInterval(
        double level = 0.95, ProportionInterval method = ProportionInterval.Exact);
}

public static class AndersonDarling
{
    public static AndersonResult Test(ReadOnlySpan<double> x);
}

public sealed record AndersonResult(double Statistic, double PValue, double[] CriticalValues, double[] SignificanceLevels);
```

The policy-first overloads follow `KruskalWallis.Test` and `OneWayAnova.Test`: `params` allows no
parameter after it, so an optional one has to come before. `Binomial` rather than `BinomialTest`,
which would read `BinomialTest.Test`.

`AndersonResult` carries two arrays, so it follows
[`Chi2ContingencyResult`](../../reference/stats/tests/chi2contingencyresult.md) exactly — `CA1819`
and `S2368` suppressed with the same reasoning, and hand-written `Equals` and `GetHashCode`, because
a record whose member compares by reference would call two equal results unequal.

## What is not written

- **`anderson`'s other five distributions.** `expon`, `logistic`, `gumbel_l`, `gumbel_r` and
  `weibull_min` each carry their own table and their own fit — `logistic` solves two equations
  numerically, `weibull_min` and the two Gumbels call a full maximum-likelihood fit. The issue reads
  this as the normality test beside `ShapiroWilk`, and the rest wait on a caller.
- **`method=MonteCarloMethod(...)`** on `anderson`, and `anderson_ksamp`. Resampling is a facility,
  not a test, and nothing here has one.
- **`fligner`**, the third equality-of-variance test, which no caller has asked for.
- **`binomtest`'s array mode**, `axis` and `keepdims`: numpy's broadcasting contract, not a
  statistical choice.

## Proof

Five corpora, frozen from scipy 1.18.1 per
[decision 0005](../../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md),
compared at `1e-9` — the statistic absolutely, the p-value relatively.

`stats_levene.json` — the three centres over balanced, unbalanced and unequal-variance groups, a
group long enough that `Trimmed` differs from `Mean`, two groups and five groups, and the
`nan_policy` arm.

`stats_bartlett.json` — the same group fixtures, plus a set whose variances are equal enough that
`T` lands near zero, which is where the clip is observable.

`stats_friedman.json` — three, four and six treatments; blocks with ties inside them and without;
the `k < 3` refusal; the `nan_policy` arm.

`stats_binomial.json` — the three alternatives over `k` below, at and above `p·n`; `k = 0` and
`k = n`; `p` at `0.5` and away from it; `n` small enough to check every term and large enough to
exercise the search; all three interval methods at 0.90, 0.95 and 0.99, one-sided included, where
one bound is a hard `0` or `1`.

`stats_anderson.json` — the statistic and the interpolated p-value from `method='interpolate'`,
over normal, exponential and tied samples and on both sides of the table's ends, where the p-value
clamps; the critical values **stated** from the formula above and checked against
`method=None`'s output in the generator, so the statement is verified rather than asserted.

`stats_beta_quantile.json` — a corpus of its own for the new inversion, frozen from
`scipy.special.betaincinv` across shapes spanning six orders of magnitude and probabilities into
both tails. The routine has no test family of its own, so without this it would be proven only
indirectly, through binomial intervals that could hide a systematic bias.

## Implementation order

Written here rather than in a tracked plan file: `docs/superpowers/plans/` stays empty
([#1104](https://github.com/CyrilB1531/lodestar/issues/1104)).

1. **`Internal/BetaQuantile.cs`** — invert the regularized incomplete beta on `(0, 1)`, bracketed,
   with its own corpus and its own unit tests beside `StudentQuantileTests`. First, because
   `Binomial`'s interval cannot be written without it and because it is the one piece with no
   precedent in the package.
2. **`Internal/GroupSpread.cs`** — median, mean and `trim_mean`'s trimmed mean over a group, shared
   by `Levene`; and `Internal/BinomialTail.cs` — `pmf`, `cdf` and `sf` from `Internal/Beta` and
   `Internal/Gamma`, shared by `Binomial`'s p-value and its interval.
3. **`Levene.cs` and `Bartlett.cs`** — the pair that repairs an assumption the package already
   sells. F and chi-squared tails are already published on `Distributions`.
4. **`Friedman.cs`** — ranks within each block through `Ranks.AverageWithTies`, tie correction,
   chi-squared at `k - 1`.
5. **`Binomial.cs` and `BinomialResult`** — the three alternatives, the `1 + 1e-7` rule and scipy's
   binary search, then the three interval methods.
6. **`AndersonDarling.cs` and `AndersonResult`** — `A²`, the stated critical values, and the
   interpolated p-value with its clamp.
7. **The corpora** — six generators in `tools/generate_oracles.py`, regenerated from `/var/tmp` with
   `.venv-oracles`, the generator's own exit code read rather than a pipeline's.
8. **The suites** — one oracle replay per family, a shared replay helper for the `nan_policy` arm as
   `Oracles/CorrelationReplay.cs` already does for the three correlation corpora, and an edge suite
   for what no corpus case can hold.
9. **The four gates** — an `equivalence.md` row per call; reference pages for five classes, their
   methods, two result records, two enums and the index; five samples; and the benchmarks.
   **`PackagingGate.Excluded` takes `BinomialResult..ctor` and `AndersonResult..ctor` with
   `ResultRecordCtor`, and `AndersonResult.Equals`/`.GetHashCode` with `RecordPlumbing`** — the gate
   runs only when the sample is executed and reports on `stderr`, which is what
   [#1125](https://github.com/CyrilB1531/lodestar/pull/1125) learned the hard way.
10. **The local gates** — build and test on both target frameworks, `dotnet format`, every
    `tools/check_*.py`, `pytest tools/tests`, markdownlint, the doc snippets, the isolated sample
    build **and run**, each read by its exit code rather than by its output.

## Risks

- **`ln Φ(w)` in the far tail.** `A²` sums logarithms of normal tails, and `Internal/Normal.Sf`
  underflows to an exact zero past about `z = 38`, where the logarithm becomes `-∞` and the
  statistic a `NaN`. A standardised sample rarely reaches it, but nothing forbids it. A
  `Normal.LogSf` with an asymptotic branch is written and tested against the direct form over the
  range where both hold, rather than discovered by a corpus case.
- **The beta inversion's accuracy at extreme shapes.** `1e-9` relative is the bar, and the corpus
  deliberately reaches shapes where the forward function itself is delicate. If the inversion cannot
  hold it there, the finding is the inversion's and the fix is in `BetaQuantile`, not a widened
  tolerance.
- **Accord's Anderson-Darling may not be the same test.** It takes the hypothesised distribution
  rather than fitting one. Whether it can be made comparable is settled before anything is timed,
  and a pair that cannot is recorded rather than quietly reported.
- **Five families in one pull request.** The scale of the ten-family lot
  ([#442](https://github.com/CyrilB1531/lodestar/issues/442)), which is the precedent; the ordering
  above front-loads the two that repair a shipped assumption, so a decision to split later costs the
  later families and not the first two.
