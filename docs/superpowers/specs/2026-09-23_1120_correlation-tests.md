# Correlation tests: Pearson, Spearman and Kendall's tau

**Issue:** [#1120](https://github.com/CyrilB1531/lodestar/issues/1120).
**Status:** written before the work, 2026-09-23.
**Date:** 2026-09-23.

## The problem

Ten hypothesis-test families ship in `Lodestar.Stats` — t-test, Wilcoxon, Mann-Whitney,
Kruskal-Wallis, one-way ANOVA, chi-square, Fisher exact, Kolmogorov-Smirnov, Shapiro-Wilk and the
multiple-comparison corrections — and not one of them answers *are these two variables related?*
`docs/equivalence.md` carries no row for `pearsonr`, `spearmanr` or `kendalltau`, so the question is
neither written here, nor delegated to another .NET library, nor recorded as a void. It is
undecided, and a reader arriving from `scipy.stats` meets the hole immediately.

## What was measured

### The incumbent is Meta.Numerics, not Math.NET

The issue reads the gap as "Math.NET Numerics computes the three coefficients and returns no
p-value, so the gap is the test rather than the statistic". That is not the state of .NET.
`Meta.Numerics` 4.2.0 — MS-PL, `netstandard2.0`, the incumbent
[#756](https://github.com/CyrilB1531/lodestar/issues/756) already measures eight of the ten shipped
families against, and already a `PackageReference` in `bench/Lodestar.Stats.Benchmarks` — exports
`Bivariate.PearsonRTest`, `Bivariate.SpearmanRhoTest` and `Bivariate.KendallTauTest`, p-values
included. Read out of the restored `netstandard2.0` asset, the way `bench/README.md` §18 resolves
`Accord`'s names rather than trusting a plan.

So the gap is not existence. It is the reading
[decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md) already made for
the ten families in its own words: *"the gap is not existence: it is agreement with scipy at `1e-9`
through a frozen corpus, and a span API under Apache-2.0"*. Concretely, against Meta.Numerics: the
three alternatives, tau-b **and** tau-c, the exact small-sample branch, scipy's tie handling, the
Fisher-z interval, `NanPolicy`, and `ReadOnlySpan<double>` at the boundary.

No new decision record is written for this. Decision 0004's row covers the reasoning unchanged, and
a record for three functions would be a record for code rather than for an axis.

### `pearsonr` has no `nan_policy`, and the exclusion is deliberate

Measured against the pinned scipy 1.18.1, and checked against the published documentation of
1.14.0, 1.15.0, 1.16.0, 1.18.0 and the 2.0.0.dev branch. Every one of the five documents the same
signature — `pearsonr(x, y, *, alternative='two-sided', method=None, axis=0)` — and the string
`nan_policy` appears on none of the five pages. On 1.18.1,
`pearsonr(x, y, nan_policy='omit')` raises `TypeError: pearsonr() got an unexpected keyword
argument 'nan_policy'`, and a `NaN` in either input returns `(nan, nan)`.

It is not an oversight upstream. Scipy pull request
[scipy/scipy#22155](https://github.com/scipy/scipy/pull/22155), *"ENH: stats: add
axis/nan_policy/keepdims/etc. support to correlation tests"*, merged 2024-12-30, added the parameter
to the correlation family and named its exceptions in its own description: *"Exceptions (right now)
are `pearsonr` because its return object stores `x` and `y` (complicating things)"*. `spearmanr` and
`kendalltau` take `nan_policy`; `pearsonr` was left out, and is still out on the development branch
fourteen months later.

The reason upstream is an obstacle in scipy's own result object, not a statement about the
statistic. That is what makes this the one place where this package moves ahead of the reference
rather than behind it — see *The one deliberate divergence* below.

### The Kendall variants differ in the statistic and in nothing else

Read out of scipy's own source and confirmed by replay:

| fact | measured |
| --- | --- |
| tau-b | `(C − D) / sqrt(tot − xtie) / sqrt(tot − ytie)`, two divisions rather than one root of a product |
| tau-c | `2(C − D) / (n² · (m − 1) / m)`, `m = min(distinct x, distinct y)` |
| both | clamped into `[-1, 1]` after the division |
| the p-value | **identical between the variants** — it reads `C − D`, which the normalisation does not touch |
| `method='exact'` with ties | `ValueError: Ties found; exact method cannot be used.` for both variants |
| `method='auto'` | exact when there is no tie **and** (`n <= 33` **or** `min(dis, tot − dis) <= 1`); asymptotic otherwise |
| either input fully tied | `(nan, nan)`, returned rather than raised |

On a tied eight-point sample: tau-b `0.857321409974`, tau-c `0.875`, p-value `0.00615326469337` for
both. Scipy's own documentation states the rule the replay confirms — the variants *"differ only in
how they are normalized to lie within the range -1 to 1; the hypothesis tests (their p-values) are
identical"*.

Two consequences for this package. First, the variant selects a normalisation at the end of one
shared computation, so it is a parameter rather than a second entry point. Second, **`ExactMethod.Exact`
on tied input must refuse**, which is the opposite of `MannWhitney`, where scipy computes an exact
p-value on tied data and this package matches it. Both rows go in `docs/equivalence.md`, or a reader
finds an inconsistency that is scipy's and reads it as ours.

### Two places the reference is wrong in a way it does not state

Both found while freezing the corpus, both decision 0007's first family, and both recorded here
because a corpus cannot hold a case the reference produces no value for.

**`kendalltau(x, y, method='asymptotic')` at two pairs raises `ZeroDivisionError`.** The
variance's last term is `x0·y0 / (9 n (n-1) (n-2))`, and at `n = 2` both sides are zero; the
division is unguarded and escapes. Nothing in scipy's documentation states a lower size bound for
the asymptotic branch, so it is not a refusal scipy defends. The IEEE answer is taken instead and
the statistic, well defined there, still comes back.

**`spearmanr(x, y, nan_policy='omit')` can answer a correlation above 1, and then invert its own
conclusion.** The omit path routes through `mstats_basic`'s masked arrays, which do not clip. On
five pairs holding one aligned `NaN` it answers `rho=1.0000000000000002` and `pvalue=1.0` — no
evidence of association, for perfectly monotone data — because the Student argument divides by a
`1 - rho` that has gone negative and is clipped to zero. The same four surviving pairs passed to
`spearmanr` directly give `(1.0, 0.0)`. This is the more serious of the two: it changes a
conclusion rather than an edge case. Omission is a filter, so the filtered answer is what the
corpus states.

### Scipy counts discordant pairs in O(n log n)

`kendalltau`'s docstring: *"Although a naive implementation has O(n^2) complexity, this
implementation uses a Fenwick tree to do the computation in O(n log(n)) complexity."* The count of
discordant pairs is an inversion count, so a merge sort answers it in the same order. The naive
double loop would lose the benchmark at `n = 10,000` by three orders of magnitude before any
constant factor is argued about, so the inversion count is written as a merge sort from the start
rather than as a follow-up optimisation.

### Pearson's p-value comes from a beta distribution, not from a t

Scipy's notes give the null density of `r` as a beta on `[-1, 1]` with equal shape parameters
`a = b = n/2 - 1`, and compute `p = 2 · dist.cdf(-|r|)`. The algebraically equal `t` form,
`t = r · sqrt((n-2)/(1-r²))` against Student with `n - 2` degrees of freedom, loses bits exactly
where the corpus goes — the far tail, where `1 - r²` cancels. `Internal/Beta.cs` already carries the
regularized incomplete beta, so the beta form costs nothing to reach.

`n = 2` is the documented special case: both possible values of `r` are `±1`, and the two-sided
p-value is `1`.

## What ships

```csharp
namespace Lodestar.Stats;

public enum KendallVariant { TauB, TauC }

public static class Pearson
{
    public static PearsonResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        NanPolicy nanPolicy = NanPolicy.Propagate);
}

public sealed record PearsonResult(double Statistic, double PValue)
{
    public (double Low, double High) ConfidenceInterval(double level = 0.95);
}

public static class Spearman
{
    public static TestResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        NanPolicy nanPolicy = NanPolicy.Propagate);
}

public static class KendallTau
{
    public static TestResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        KendallVariant variant = KendallVariant.TauB,
        ExactMethod method = ExactMethod.Auto,
        NanPolicy nanPolicy = NanPolicy.Propagate);
}
```

`variant` sits next to `alternative` because both say *what is being asked*, where `method` and
`nanPolicy` say *how it is computed*; scipy's own order is keyword-only and carries no meaning to
preserve.

`PearsonResult.ConfidenceInterval` mirrors `TTestResult.ConfidenceInterval`: a method rather than a
property because it takes a level, the interval through the Fisher z transform that
`scipy.stats.pearsonr(...).confidence_interval()` returns by default, and a one-sided alternative
gives a half-open interval whose far bound is `-1` or `1` rather than a narrower two-sided one.

`Spearman.Test` takes no `ExactMethod`: scipy has no exact branch for `spearmanr` at all — it is the
t approximation on `n - 2` degrees of freedom, always — and offering a parameter with one legal
value would be an API this package would then have to keep.

## The one place this moves ahead of the reference

**`Pearson.Test` takes `NanPolicy`, where `scipy.stats.pearsonr` does not.**

`NanPolicy`'s own remarks state the rule this bends: the parameter is offered on the entry points
whose scipy counterpart takes it, and on no others, so a reader can tell from the reference page
alone whether a call has it, without knowing scipy's version history.

It is bent here on purpose. Scipy's exclusion of `pearsonr` is not a statement about the Pearson
correlation: pull request [scipy/scipy#22155](https://github.com/scipy/scipy/pull/22155) says the
obstacle is the shape of scipy's own `PearsonRResult`, which stores `x` and `y` for its bootstrap
and permutation methods. `PearsonResult` stores neither, so the obstacle does not exist here.
Refusing the parameter would import a constraint from another library's internals into this one's
public API, and would leave the three correlation calls disagreeing with each other for a reason no
caller can see.

**This is an extension, not a divergence in
[decision 0007](../../decisions/0007-the-deliberate-divergences.md)'s sense**, and the distinction
decides which rules apply. That record governs members that answer something *different* from the
reference, and it names "a parameter's presence" among the things reproduced rather than diverged
from. Nothing here answers differently: `NanPolicy.Propagate` is the default, and on the default the
result is scipy's exactly — a `NaN` in either input gives `(NaN, NaN)`. On every input
`scipy.stats.pearsonr` can express, the two calls agree at the corpus tolerance. `Raise` and `Omit`
are reachable only by a caller who writes something scipy has no syntax for, so no migrated script
can land on them by accident.

`Omit` drops the **pairs** in which either coordinate is missing, which is what `spearmanr` and
`kendalltau` already do on paired input, so the three agree with each other as well.

The `docs/equivalence.md` row states all of it — that the default reproduces scipy exactly, that the
parameter is an addition, and why, naming the scipy pull request — so a reader meets the extension
in the mapping table rather than in a signature.

## What is not written

- `spearmanr`'s matrix mode. Scipy's `spearmanr(a)` on a 2-D array returns a correlation matrix and
  a matrix of p-values; only the paired 1-D call has a counterpart here. `Lodestar.Stats` publishes
  scalar tests, and a matrix result is a different shape of answer.
- The `axis` and `keepdims` parameters, which are numpy's broadcasting contract rather than a
  statistical choice.
- `method=PermutationMethod(...)` / `MonteCarloMethod(...)` on `pearsonr`, and `BootstrapMethod` on
  its interval. Resampling is a facility, not a test, and nothing in this package has one yet.
- Kendall's tau-a, which scipy does not publish separately either: tau-b and tau-c both reduce to it
  when there is no tie.
- `scipy.stats.weightedtau`, `somersd` and `pointbiserialr`, which no caller has asked for.

## Proof

Three corpora, frozen from scipy 1.18.1 per
[decision 0005](../../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md),
compared at `1e-9` — the statistic absolutely, the p-value relatively, as
`StatsOracleAsserts` already splits them.

`tests/oracles/stats_pearson.json` — each sample replayed on the three alternatives, plus the
confidence interval at 0.90/0.95/0.99, `n = 2`, a perfect positive and a perfect negative
correlation, a constant input (scipy warns and returns `NaN`), a far-tail case whose p-value reaches
below `1e-15`, and the `NanPolicy` cases, which are **not** replayed from scipy for `Omit` and
`Raise` — scipy cannot produce them — but stated against the same call on the pairwise-filtered
input, the way `MultipleComparisons.Bonferroni`'s corpus states a definition scipy has no call for.

`tests/oracles/stats_spearman.json` — untied and tied samples, averaged ranks, the three
alternatives, `nan_policy` on all three settings, and a fully tied input.

`tests/oracles/stats_kendall.json` — the cross product of `variant` in {b, c}, `method` in {auto,
asymptotic, exact} and the three alternatives over an untied and a tied sample, with the tied-exact
cells marked `raises`; `n` on both sides of the 33 threshold; the `min(dis, tot - dis) <= 1` corner
that sends a large untied sample down the exact branch; a fully tied input returning `(NaN, NaN)`;
and the `nan_policy` cases.

## Implementation order

Written here rather than in a tracked plan file: `docs/superpowers/plans/` stays empty
([#1104](https://github.com/CyrilB1531/lodestar/issues/1104)), and the checkbox instrument lives
beside the work.

1. **The shared internals.** `Internal/Correlation.cs` holds the product-moment coefficient both
   Pearson and Spearman need; `Internal/Inversions.cs` the merge-sort inversion count;
   `Internal/Concordance.cs` the dense ranks, tie sums and discordant count Kendall reads;
   `Internal/KendallExact.cs` the exact null distribution; `Internal/Fisher.cs` the `atanh` that
   `netstandard2.0` has no `Math.Atanh` for. **No new paired-input validator**:
   `Internal/NanFilter.ApplyAligned` already drops pairs rather than values, and `Wilcoxon.Paired`
   already uses it, so the three call it the same way rather than growing a fifth spelling.
2. **`Pearson.cs` and `PearsonResult`** — the centred sums, the beta tail through
   `Internal/Beta.RegularizedIncomplete`, the `n = 2` and constant-input special cases, and the
   Fisher-z interval on the result.
3. **`Spearman.cs`** — `Ranks.AverageWithTies` on each input, then the Student tail.
4. **`KendallTau.cs`** — one pass producing `C − D` and the tie sums, the two normalisations behind
   `KendallVariant`, the asymptotic normal tail, and the exact branch behind a measured ceiling
   past which `Exact` refuses and `Auto` falls back — the `MannWhitney` precedent.
5. **The corpora** — three generators in `tools/generate_oracles.py`, regenerated from `/var/tmp`
   with `.venv-oracles`, the generator's own exit code read rather than a pipeline's.
6. **The suites** — `PearsonOracleTests`, `SpearmanOracleTests`, `KendallTauOracleTests`,
   `CorrelationEdgeTests` for what no corpus case can hold, and `InversionsTests` against the
   quadratic definition the merge sort replaces.
7. **The four gates** — a `docs/equivalence.md` row per call; nine reference pages and the index;
   four samples; and `CorrelationBenchmarks.cs` against Meta.Numerics 4.2.0.
8. **The local gates** — build and test on both target frameworks, `dotnet format`, every
   `tools/check_*.py`, `pytest tools/tests`, markdownlint, the doc snippets, and the isolated
   sample build.

## Risks

- **The exact Kendall table's cost.** `MannWhitneyCounts` is bounded at a product of 20,000 after a
  measurement. The Kendall table is `O(n² log n)`-ish in the same shape, and `Auto` only ever asks
  for it at `n <= 33` or at a degenerate tail, so the ceiling binds `Exact` alone. The bound is
  measured before it is written down, not chosen round.
- **The far tail of the beta.** The corpus deliberately reaches a p-value below `1e-15`; if
  `Internal/Beta`'s incomplete beta cannot hold `1e-9` relative there, the corpus case is the finding
  and the fix is in `Beta`, not a widened tolerance.
- **Meta.Numerics' Kendall may be another variant.** Which one it computes, and whether its p-value
  is two-sided only, is resolved against the restored assembly before the pair is timed. A pair that
  cannot be made to agree is printed and recorded rather than quietly dropped.
