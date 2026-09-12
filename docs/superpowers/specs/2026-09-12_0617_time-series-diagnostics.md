# 0617 — Serial correlation: the autocorrelation functions and the Ljung-Box test

**Status:** accepted, 2026-09-12. Written before the work.

Issue: [#617](https://github.com/CyrilB1531/lodestar/issues/617), the time-index half of
[#338](https://github.com/CyrilB1531/lodestar/issues/338).
Reading: [decision 0105](../../decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md).

## Problem

[Decision 0096](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) drew the
line this issue sits behind:

> This package is OLS and its table; anything with a link function or a time index falls on that
> issue's side of the line.

[#616](https://github.com/CyrilB1531/lodestar/issues/616) took the link function. This takes the
time index, and decision 0105 already established what the gap actually is. The naive scope for
"time series in .NET" is a forecaster, and the forecaster is the one thing Microsoft already
ships: `Microsoft.ML.TimeSeries` 5.0.0 carries `ForecastBySsa`, `DetectSeasonality` and six
detectors across 34 types, and **zero** matches for autocorrelation, ACF, PACF, stationarity,
Ljung-Box or decomposition. `Deedle` 8.1.0 returns zero for the same search across 2 132 members:
it is the index, not a model.

So the gap is the apparatus for deciding whether a series may be modelled at all, and whether a
fit left anything behind — the same shape 0096 found, where the estimate was everywhere and the
inference nowhere.

## Scope

Decision 0105 named four subjects. **This spec covers three of them**, and they are one subject
in three functions: *is there serial dependence left in this series, and at which lags?*

- the autocorrelation function, with its confidence band
- the partial autocorrelation function, with its band
- the Ljung-Box test for residual independence, with Box-Pierce beside it

**Explicitly deferred to a second lot**, which is its own issue: the stationarity tests (ADF and
KPSS) and seasonal decomposition into trend, seasonal and residual. That split is not
convenience. ADF's p-value is a MacKinnon response-surface interpolation over a frozen
coefficient table, and seasonal decomposition needs a period and a moving-average filter design —
neither shares a line of arithmetic with the three above, and both would double the lot without
making any of it more useful. The three here deliver something usable on their own: a reader can
diagnose a fitted model's residuals the day this ships.

Also out, and each named so it is not mistaken for an oversight: ARIMA and SARIMAX estimation,
VAR, Kalman filtering and state-space models, panel data, HAC and Newey-West covariances,
the periodogram, and `acf`'s `missing=` handling for gapped series.

## Public surface — one static class, two result records

```csharp
namespace Lodestar.Stats.TimeSeries;   // the namespace, whatever package it lands in

public static class SerialCorrelation
{
    public static AutocorrelationResult Autocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null);

    public static AutocorrelationResult PartialAutocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null);

    public static LjungBoxResult LjungBox(
        ReadOnlySpan<double> series, int lagCount, LjungBoxOptions? options = null);
}
```

`lagCount` is required rather than defaulted. The reference defaults it — `acf` to
`min(10·log10(n), n − 1)` and `pacf` to `min(10·log10(n), n/2 − 1)`, two different rules for two
functions a caller reads side by side — and a silent default that differs between the two is a
trap rather than a convenience. The two rules are documented on the members and in
`docs/equivalence.md` so a caller reproducing the reference's plot can pass them deliberately.

### `AutocorrelationResult`

```csharp
public sealed class AutocorrelationResult
{
    internal AutocorrelationResult() { }

    public IReadOnlyList<double> Values { get; init; } = [];
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];
}
```

All three have length `lagCount + 1` and are indexed by lag, so `Values[0]` is `1.0` — the
reference returns lag zero and dropping it would renumber every index a reader compares against a
statsmodels plot.

The band is **centred on the estimate, not on zero**, which is what the reference returns and is
worth stating because the band a reader draws on a correlogram is usually the zero-centred one.
`ConfidenceLower[0] == ConfidenceUpper[0] == 1.0`: lag zero has no variance.

**A class rather than a record, and that is the decision, not an omission.** A record's
synthesized `Equals` compares reference-typed members by reference, so two results holding the
same numbers in different arrays compare unequal — the trap
[#668](https://github.com/CyrilB1531/lodestar/issues/668) catalogues across a dozen types. The
three ways out were weighed:

- **a named tuple** does not help. `ValueTuple`'s equality goes through
  `EqualityComparer<double[]>.Default`, which is reference equality again, and a tuple carries no
  per-member XML documentation for the reference gate to check.
- **a record with a hand-written `Equals` and `GetHashCode`** is correct but buys a second trap:
  the arrays are handed to the caller, so a hash taken before they write into one does not match
  it afterwards. A hash code that changes under its own dictionary is worse than no equality.
- **no equality at all**, which is what ships. Nobody compares two correlograms. The shape is
  `GlmSummary`'s, which #616 landed for the same reason: `sealed`, init-only properties, an
  `internal` constructor so no caller can build one that a function did not return.

The options types stay records: every member of both is a value type, so their equality compares
what a reader expects. That is the same condition #616 applied when converting `OlsOptions`, and
this spec is a data point for #668 rather than a pre-emption of it.

### `LjungBoxResult`

```csharp
public sealed class LjungBoxResult
{
    internal LjungBoxResult() { }

    public IReadOnlyList<double> Statistics { get; init; } = [];
    public IReadOnlyList<double> PValues { get; init; } = [];
    public IReadOnlyList<double> BoxPierceStatistics { get; init; } = [];
    public IReadOnlyList<double> BoxPiercePValues { get; init; } = [];
    public IReadOnlyList<int> DegreesOfFreedom { get; init; } = [];
}
```

Length `lagCount`, indexed from lag 1 — the reference indexes its frame the same way, and a
Ljung-Box statistic at lag 0 is not defined. The Box-Pierce arrays are empty when
`LjungBoxOptions.BoxPierce` is false, which is the default, matching the reference's own
`boxpierce=False`.

Five parallel lists rather than a list of five-field rows: a caller plots a column, and
`Lodestar.Stats`' existing results are shaped the same way. Same class-not-record reasoning as
above.

### Options

```csharp
public sealed record AutocorrelationOptions
{
    public bool Adjusted { get; init; }                    // default false
    public bool BartlettConfidenceInterval { get; init; } = true;
    public double ConfidenceLevel { get; init; } = 0.95;   // validated in (0, 1)
}

public sealed record LjungBoxOptions
{
    public int ModelDegreesOfFreedom { get; init; }        // default 0, validated >= 0
    public bool BoxPierce { get; init; }                   // default false
}
```

Records with `init`, which is what every options type in this repository is — `KMeansOptions`,
`StandardScalerOptions`, and `GlmOptions` and `OlsOptions` since #616. Every member is a value
type, so the equality a record brings compares what a reader expects.

Validation lives in the accessor, not at the point of use, for the reason `GlmOptions` records:
a setting read three functions later reaches the caller as an arithmetic accident instead of an
exception naming it.

`PartialAutocorrelation` reads only `ConfidenceLevel` from `AutocorrelationOptions`; `Adjusted`
and `BartlettConfidenceInterval` have no meaning for it. That is stated on the member rather than
solved with a second options type carrying one property.

## The arithmetic, read from the reference's source rather than described

Read on **2026-09-12** against `statsmodels` 0.15.0, which is already in the lock since #566.

### Autocorrelation

`acf` divides the autocovariance by its own lag-zero value: `acf[k] = avf[k] / avf[0]`. With
`adjusted=False`, the default, every `avf[k]` divides by `n` rather than by `n − k` — the biased
estimator, which is the one that guarantees a positive semi-definite sequence. `Adjusted = true`
divides by `n − k` instead.

**The band, when `BartlettConfidenceInterval` is true (the default), is Bartlett's formula:**

```text
var[0] = 0
var[1] = 1 / n
var[k] = (1 / n) · (1 + 2·Σ_{j=1..k-1} acf[j]²)      for k >= 2
```

and the interval is `acf[k] ± z_{1-α/2} · sqrt(var[k])`. Turned off, `var[k] = 1/n` at every lag,
which is the flat band a correlogram usually draws.

The reference computes the autocovariance **through an FFT by default** (`fft=True`). This is an
arithmetic-ordering difference, not a definitional one: the direct sum and the FFT agree to the
last few digits, and the corpus compares at `1e-9`, well above that. The implementation is the
direct double loop, because an FFT here would be a dependency or a kernel this lot has no other
use for, and `docs/equivalence.md` records the divergence in ordering with the measured gap.

### Partial autocorrelation

Default method `ywadjusted`: Yule-Walker with the adjusted autocovariance, solved by the
Levinson-Durbin recursion, whose `k`-th reflection coefficient is `pacf[k]`. `pacf[0] = 1.0`.

**The band does not use Bartlett.** The reference takes `var = 1/n` at every lag ≥ 1 —
Quenouille's result, that a partial autocorrelation beyond the true order is asymptotically
`N(0, 1/n)` — and the interval is `pacf[k] ± z_{1-α/2}/sqrt(n)`. Lag zero's interval is the point
`[1, 1]`, which the reference sets explicitly after the fact.

Only `ywadjusted` ships. The reference offers eight more spellings across four methods — `ywm`,
`ols`, `ols-inefficient`, `ols-adjusted`, `ld`, `ldbiased`, `burg` — and none of them is what a
caller gets by not choosing. Adding them is an options enum later, not a reason to widen this lot.

### Ljung-Box

```text
Q(m) = n·(n + 2) · Σ_{k=1..m} r[k]² / (n − k)
p    = chi2.sf(Q(m), m − modelDf)
```

Box-Pierce is the same sum without the correction: `Q_BP(m) = n · Σ_{k=1..m} r[k]²`, on the same
degrees of freedom.

**The `r[k]` here are `acf(x, nlags=m, fft=False)`** — the reference calls its own autocorrelation
with the FFT explicitly off, so the statistic is computed from the direct estimator even though
`acf`'s own default is the FFT one. Our `Autocorrelation` is direct anyway, so the two agree by
construction; the point is that this is checked rather than assumed.

`m − modelDf <= 0` yields `NaN` for that lag in the reference rather than an exception. We match
it: a caller passing `modelDegreesOfFreedom` larger than a lag has asked a question with no
degrees of freedom left, and the answer is `NaN` at that lag and a real value at the ones above.
`ModelDegreesOfFreedom` below zero is refused at the accessor, as the reference refuses it with
`ValueError`.

### The p-values compare relatively

Through `StatsOracleAsserts` in `tests/Lodestar.Stats.Tests/Oracles/`, reused and never restated,
per [decision 0081](../../decisions/0081-the-stats-numerical-layer-stays-internal.md). The chi-square
survival function is `Distributions.ChiSquaredSf`, already published by `Lodestar.Stats` under
[decision 0095](../../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
— this lot needs no new member from it, and the normal quantile the bands need is
`Distributions.NormalQuantile`, published by the same decision.

## Refusals

Each is an `ArgumentException` naming the offending argument, in the register the sibling packages
use — a lowercase sentence fragment naming the value, not a category.

| input | what happens |
| --- | --- |
| `series` shorter than 2 | refused: a correlation needs two points |
| `lagCount` below 1 | refused |
| `lagCount >= series.Length` | refused for `Autocorrelation`; the reference's own default caps at `n − 1` |
| `lagCount > series.Length / 2` | refused for `PartialAutocorrelation`: the Levinson-Durbin recursion has no information past it, and the reference's own default caps at `n/2 − 1` |
| a constant series | refused: `avf[0]` is zero, so every ratio is `0/0`. The reference returns `NaN` with a warning; we refuse, as `KruskalWallis.Test` already does for a fully tied sample |
| `NaN` or infinity in `series` | refused. The reference has a `missing=` parameter this lot does not implement, so accepting one silently would propagate it through every lag |

The constant-series refusal is a deliberate divergence and gets its `docs/equivalence.md` row.
The precedent is in this repository already: `KruskalWallis.Test` refuses a fully tied pooled
sample where scipy returns `(nan, nan)`, and `OneWayAnova.Test` matches scipy's `NaN` for its own
degenerate case. The rule those two encode is that a division carrying no information is refused
rather than performed, and a constant series is exactly that.

## Placement, deliberately deferred

**Not decided here.** The same method [decision 0111](../../decisions/0111-the-generalized-linear-model-does-not-earn-its-own-package.md)
used for #616: write it, measure the lines, the public types and the members, then test the
result against [decision 0076](../../decisions/0076-a-core-package-carries-no-external-dependency.md)'s
three criteria — a distinct dependency profile, a distinct audience, a distinct cadence — and
record the verdict as an ADR before the branch ends.

What is known now: the naming constraint allows `Lodestar.Stats.TimeSeries` and forbids a third
level; core tier carries no external dependency, which is what rules out taking `Deedle` or
`Cortex.TimeSeries` as a dependency rather than as a benchmark; and the fixed cost of a package
is the one #566 measured — five pack loops, both release allow-lists, `Version.props`, the
`EXPECTED` entry, `FLOORS` rows, the `*.NetStandard.Tests` mirror with every `Lodestar.*`
assembly pinned by `ProjectReference`, a `wiki-map.json` entry, reference pages, a sample per
public class, `equivalence.md` rows, a guide and a `bench/README.md` section.

The argument that will decide it, and which the measurement settles rather than opens: this lot's
audience is the person who just fitted a model and wants to know whether the residuals are white
— the same person `Lodestar.Stats.Regression` serves — and its only dependency is a chi-square
tail and a normal quantile that `Lodestar.Stats` already publishes. If the measurement says the
surface is small and the audience is the neighbour's, it stays put, as the GLM did. **The second
lot's arrival changes that arithmetic**, which is a reason to record the measurement now and
revisit rather than to guess in either direction today.

## Oracle

`statsmodels` 0.15.0, BSD-3-Clause, already in the lock — no lock churn. The corpus is
`tests/oracles/stats_timeseries.json`, generated by a `generate_stats_timeseries()` registered in
`tools/generate_oracles.py` beside its siblings.

Counterparts: `statsmodels.tsa.stattools.acf`, `.pacf`, and
`statsmodels.stats.diagnostic.acorr_ljungbox`.

Fixtures, each with its series frozen in the corpus rather than regenerated:

- a stationary AR(1) at φ = 0.7, 40 points — the textbook case, where the ACF decays and the PACF
  cuts off after lag 1
- an MA(1), 60 points — the mirror image, where the ACF cuts off and the PACF decays; a pair of
  fixtures that would both pass against an implementation that confused the two functions
- white noise, 100 points — every lag inside the band, and the case where a wrong band is visible
- a series with a linear trend, 80 points — a slowly decaying ACF, and the case that motivates
  the second lot's stationarity tests
- a seasonal series, period 12, 96 points — a spike at lag 12, which is what `lagCount` past a
  season is for
- a short series, 12 points, at `lagCount = 5` — where `n − k` in the Ljung-Box denominator is
  small enough to matter

Each fixture freezes, per function: the values, both interval ends at 0.95 **and** at 0.99, and
for Ljung-Box the statistics, p-values, Box-Pierce pair and degrees of freedom at
`modelDf = 0` and at `modelDf = 2`. The two confidence levels are not padding: a band computed
with a hard-coded `1.96` passes at 0.95 and fails at 0.99, and that is a mistake worth one line
of corpus.

The Bartlett and flat bands are both frozen, since `BartlettConfidenceInterval` selects between
two formulas rather than scaling one.

## Testing

`tests/Lodestar.<wherever>.Tests/SerialCorrelationOracleTests.cs` replays the corpus, comparing
values relatively at the OLS regime and p-values through `StatsOracleAsserts`. A
`SerialCorrelationEdgeTests.cs` covers every row of the refusals table, asserting the `ParamName`
and a fragment of the message rather than the whole sentence.

Three properties are asserted directly rather than through the corpus, because they hold for
every input and a frozen number would not say so:

- `Values[0] == 1.0` exactly, for both functions
- an AR(1)'s PACF is within the band at every lag past 1, and its ACF is not — the one assertion
  that would catch the two functions being swapped
- Ljung-Box's statistic is non-decreasing in the lag, since it is a cumulative sum of squares

Both target frameworks, through the `*.NetStandard.Tests` mirror that links the same sources.

## Benchmarks

A `bench/README.md` section against `Cortex.TimeSeries` 1.1.0's `AutocorrelationTests` and
`LjungBoxResult`, which is the only permissively licensed .NET package carrying them. Its 240
downloads are a reason to prefer our own, not a reason to skip the measurement — decision 0096's
rule. `Cortex.TimeSeries` goes in `bench/` only; its `Cortex.ML` edge bars it from `src/` under
decision 0076 whatever it exports.

Numbers go in `docs/guides/performance.md` with the machine named. `bench/README.md` carries how
to measure and nothing measured.

## Documentation owed, in the same commit as the code

- a reference page per public type, under the layout `docs/reference/` already uses, with every
  `csharp` fence true — they are compiled and executed, and a trailing `// =>` is an assertion
- a `docs/equivalence.md` row per public member, carrying the four divergences named above: the
  direct autocovariance against the reference's FFT, the refused constant series, the required
  `lagCount` against the reference's two different defaults, and `ywadjusted` as the only method
- a sample per public class in `samples/Lodestar.Sample`, or `check_sample_coverage.py` fails
- `docs/migration/statsmodels.md`: the time-series diagnostics row stops saying "being written"
  for the part that ships, and keeps saying it for the second lot
- `CHANGELOG.md` under `## [Unreleased]`, and a `Version.props` move decided by where the code
  lands
- the ADR recording the placement verdict, with its measurement

## What would make this spec wrong

Stated so a reader can check rather than trust:

1. **The bands.** If `PartialAutocorrelation`'s band were computed with Bartlett's formula it
   would be wrong, and it would still look plausible on a plot. The corpus's two confidence
   levels and the white-noise fixture are what catch it.
2. **The biased denominator.** `adjusted=False` dividing by `n` at every lag looks like a bug to
   anyone who expects an unbiased estimator, and a future reader may "fix" it. The AR(1) fixture
   fails immediately if they do.
3. **The placement deferral.** If the second lot turns out to be three times this one, the
   measurement taken here answers a question about a third of the eventual surface. That is why
   the ADR records the measurement and the reasoning rather than only the verdict.
