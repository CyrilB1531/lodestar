# 0671 — Stationarity and seasonal decomposition, and the package the time index earns

**Status:** written before the work, 2026-09-15.

Issue: [#671](https://github.com/CyrilB1531/lodestar/issues/671), the second half of the time-index
subject [#338](https://github.com/CyrilB1531/lodestar/issues/338) opened.
Reading: [decision 0105](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md),
as amended by [0129](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0129-four-numerics-libraries-read-and-three-absences-withdrawn.md).
First half: [`2026-09-12_0617_time-series-diagnostics.md`](2026-09-12_0617_time-series-diagnostics.md)
and [decision 0114](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0114-the-serial-correlation-diagnostics-stay-in-lodestar-stats.md).

## Problem

Issue #617 shipped the autocorrelation functions and the Ljung-Box test, and deferred the rest of 0105's
diagnostics on purpose: the augmented Dickey-Fuller test, KPSS, and seasonal decomposition. They
answer the question a correlogram raises and cannot settle — *may this series be modelled as it
stands, or does it need differencing, detrending or deseasonalising first?* — and they are what a
reader runs before anything #617 shipped is meaningful.

Decision 0129 recorded that Numerics.NET exports ADF and KPSS under a commercial licence and
`Cortex.TimeSeries` a free but unadopted `StationarityTests`. Nothing free and maintained carries
them at `statsmodels` parity, so the gap 0105 named stands for this half.

## Scope

- **`adfuller`** — every `regression` (`n`, `c`, `ct`, `ctt`), every `autolag` (`AIC`, `BIC`,
  `t-stat`, `None`), `maxlag` given or defaulted, and the MacKinnon p-value and critical values.
- **`kpss`** — `regression` `c` and `ct`, `nlags` as `auto`, `legacy` or a count, and the p-value
  interpolated over its four-point table, bound included.
- **`seasonal_decompose`** — additive and multiplicative, `two_sided`, `extrapolate_trend`, with the
  default moving-average filter.

**Out**, each named so it is not mistaken for an oversight: STL by loess (`stlnet` ships it under
MIT, and it deserves its own reading), a caller-supplied `filt`, `period` inferred from a pandas
index, `adfuller`'s `store`/`regresults` side outputs, the cointegration tests that reuse MacKinnon's
table for `N > 1`, and the Phillips-Perron and Zivot-Andrews tests.

## Placement — a package, and the first lot moves into it

[Decision 0114](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0114-the-serial-correlation-diagnostics-stay-in-lodestar-stats.md)
kept #617's lot in `Lodestar.Stats` and wrote down what would overturn that: *"a dependency this lot
never needed — ADF's OLS is precisely that."* This lot has it.

`adfuller` fits an ordinary least squares per candidate lag and reads the lagged level's t
statistic and the fit's information criterion. `Lodestar.Stats.Regression`'s
`OrdinaryLeastSquares.Fit` returns exactly the t statistics and the residual standard error those
need, and its published 0.1.0 already does. But `Lodestar.Stats.Regression` depends on
`Lodestar.Stats`, so `Lodestar.Stats` cannot take it. Three resolutions, measured against
[decision 0076](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0076-a-core-package-carries-no-external-dependency.md):

| option | cost | verdict |
| --- | --- | --- |
| **A package, `Lodestar.Stats.TimeSeries`**, with edges to `Lodestar.Stats` 0.4.0 and `Lodestar.Stats.Regression` 0.1.0, and #617's five types moving into it | the fixed cost #566 measured; no type changes namespace | **chosen**: the dependency profile is now distinct, the criterion 0114 found unmet |
| A private least-squares solve inside `Lodestar.Stats` | a second OLS in one repository | refused on [decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s ground that two copies of one computation disagree eventually |
| An edge from `Lodestar.Stats` to `Lodestar.Decomposition` for its QR | every hypothesis-test caller restores `Decomposition` and `Abstractions` | refused on 0114's own ground against moving the lot into `Stats.Regression` |

**The move is free today and not later.** `Lodestar.Stats/v0.4.0` is the last tag and carries none
of #617's types, so moving them before the next `Lodestar.Stats` tag breaks no published caller. The
namespace was chosen for this in 0114: `SerialCorrelation` and its four companions are already
declared in `Lodestar.Stats.TimeSeries`, the new package's `RootNamespace`, so a caller's `using`
is untouched and only the `PackageReference` changes. Splitting the subject across two packages that
share a namespace was 0114's third option and stays refused.

Two edges, both published floors, so `git clone && dotnet build` needs no pack step:
`Lodestar.Stats` 0.4.0 for `Distributions.ChiSquaredSf` and `NormalQuantile`, and
`Lodestar.Stats.Regression` 0.1.0 for `OrdinaryLeastSquares.Fit`. The ADR that records this
supersedes 0114.

## Public surface

```csharp
namespace Lodestar.Stats.TimeSeries;

public static class Stationarity
{
    public static DickeyFullerResult AugmentedDickeyFuller(
        ReadOnlySpan<double> series, DickeyFullerOptions? options = null);

    public static KpssResult Kpss(ReadOnlySpan<double> series, KpssOptions? options = null);
}

public static class SeasonalDecomposition
{
    public static SeasonalComponents Decompose(
        ReadOnlySpan<double> series, int period, SeasonalDecompositionOptions? options = null);
}

public enum TrendTerms { None, Constant, ConstantAndTrend, ConstantAndQuadraticTrend }
public enum LagSelection { Akaike, Schwarz, TStatistic, Fixed }
public enum KpssLagRule { Automatic, Legacy, Fixed }
public enum SeasonalModel { Additive, Multiplicative }
public enum PValueBound { None, ActualIsSmaller, ActualIsGreater }
```

`period` is required, as `lagCount` was in #617: the reference infers it from a pandas index this
repository does not have.

### Options — records of value types

```csharp
public sealed record DickeyFullerOptions
{
    public TrendTerms Regression { get; init; } = TrendTerms.Constant;
    public LagSelection LagSelection { get; init; } = LagSelection.Akaike;
    public int? MaxLag { get; init; }                       // null: ceil(12·(n/100)^¼), capped
}

public sealed record KpssOptions
{
    public TrendTerms Regression { get; init; } = TrendTerms.Constant; // Constant or ConstantAndTrend
    public KpssLagRule LagRule { get; init; } = KpssLagRule.Automatic;
    public int LagCount { get; init; }                      // read only under Fixed
}

public sealed record SeasonalDecompositionOptions
{
    public SeasonalModel Model { get; init; } = SeasonalModel.Additive;
    public bool TwoSided { get; init; } = true;
    public int ExtrapolateTrend { get; init; }              // 0: leave the ends NaN
}
```

`MaxLag` below zero and `ExtrapolateTrend` below zero are refused at the accessor, as the #617
options refuse theirs. `KpssOptions.Regression` refuses `None` and `ConstantAndQuadraticTrend`
there, since KPSS defines neither.

### Results — sealed classes with an internal constructor

The shape #617 settled, and for its reason: a record would compare the lists by reference
([#668](https://github.com/CyrilB1531/lodestar/issues/668)).

```csharp
public sealed class DickeyFullerResult
{
    public double Statistic { get; init; }
    public double PValue { get; init; }
    public int UsedLag { get; init; }
    public int ObservationCount { get; init; }
    public IReadOnlyList<double> CriticalValues { get; init; } = [];   // 1 %, 5 %, 10 %
    public double InformationCriterion { get; init; }                   // NaN under Fixed
}

public sealed class KpssResult
{
    public double Statistic { get; init; }
    public double PValue { get; init; }
    public int LagCount { get; init; }
    public IReadOnlyList<double> CriticalValues { get; init; } = [];   // 10 %, 5 %, 2.5 %, 1 %
    public PValueBound PValueBound { get; init; }
}

public sealed class SeasonalComponents
{
    public IReadOnlyList<double> Trend { get; init; } = [];
    public IReadOnlyList<double> Seasonal { get; init; } = [];
    public IReadOnlyList<double> Residual { get; init; } = [];
}
```

**`PValueBound` is the decision #671 asked for.** Past either end of KPSS's table the reference
returns the end value and warns that the true p-value lies beyond it. A library has no warning
channel a caller reads, so the direction is a property: `ActualIsSmaller` when the statistic is at
or past the 1 % critical value and `0.01` came back, `ActualIsGreater` at or below the 10 % one.

**`InformationCriterion` is `NaN` under `LagSelection.Fixed`**, where the reference returns
`None`. Under `TStatistic` it is the absolute t statistic of the last lag tried, as the reference's
`icbest` is.

## The arithmetic, read from the reference's source

Read on **2026-09-15** against `statsmodels` 0.15.0.

### Augmented Dickey-Fuller

With `k` the number of trend terms (0 to 3) and `n` the series length:

1. `maxlag` defaults to `ceil(12·(n/100)^¼)`, capped at `n//2 − k − 1`; a given one above that cap is
   refused, and a default below zero is refused as too short.
2. `Δx` is the first difference. The regression at lag `p` has, per row `t`, the lagged **level**
   `x[t−1]` and the lagged differences `Δx[t−1] … Δx[t−p]`, over the rows where all of them exist.
3. **Lag search.** Every candidate `p = 0 … maxlag` is fitted over the **same** `n − maxlag − 1`
   rows, so the criteria are comparable. The trend terms are the constant (as the fit's intercept)
   and the columns `t` and `t²`, with `t = 1 … rows`. Akaike is `−2ℓ + 2q` and Schwarz
   `−2ℓ + q·log(rows)`, with `q` the number of estimated parameters including the constant and
   `ℓ = −rows/2 · (log 2π + log(SSR/rows) + 1)`. The smallest wins, and a tie goes to the smaller lag,
   as the reference's `min` over `(criterion, lag)` pairs does. `TStatistic` walks down from
   `maxlag` and stops at the first lag whose last coefficient's `|t| ≥ 1.6448536269514722`, the
   reference's literal, falling through to lag 0.
4. **Refit** at the chosen lag over its own, longer sample, `n − p − 1` rows. The statistic is the
   lagged level's t statistic; `ObservationCount` is that row count.
5. **P-value** — MacKinnon (1994) for one series: `1` above the table's maximum, `0` below its
   minimum, otherwise `Φ(polynomial(statistic))` with the small-p coefficients at or below the
   table's switch point and the large-p ones above it. The constants are frozen as the reference
   writes them — published coefficient times scaling, multiplied at run time exactly as it does:

   | `regression` | max | min | switch | small-p (× 1, 1, 10⁻²) | large-p (× 1, 10⁻¹, 10⁻¹, 10⁻²) |
   | --- | ---: | ---: | ---: | --- | --- |
   | `n` | +∞ | −19.04 | −1.04 | 0.6344, 1.2378, 3.2496 | 0.4797, 9.3557, −0.6999, 3.3066 |
   | `c` | 2.74 | −18.83 | −1.61 | 2.1659, 1.4412, 3.8269 | 1.7339, 9.3202, −1.2745, −1.0368 |
   | `ct` | 0.70 | −16.18 | −2.89 | 3.2512, 1.6047, 4.9588 | 2.5261, 6.1654, −3.7956, −6.0285 |
   | `ctt` | 0.54 | −17.17 | −3.21 | 4.0003, 1.6580, 4.8288 | 3.0778, 4.9529, −4.1477, −5.9359 |

   `Φ` comes from `Distributions.ChiSquaredSf`: `Φ(z) = ½·ChiSquaredSf(z², 1)` below zero and one
   minus that above. `Lodestar.Stats` publishes no normal CDF and this lot does not ask it to.
6. **Critical values** — MacKinnon (2010), `c₀ + c₁/rows + c₂/rows² + c₃/rows³` per level, from:

   | `regression` | 1 % | 5 % | 10 % |
   | --- | --- | --- | --- |
   | `n` | −2.56574, −2.2358, −3.627, 0 | −1.941, −0.2686, −3.365, 31.223 | −1.61682, 0.2656, −2.714, 25.364 |
   | `c` | −3.43035, −6.5393, −16.786, −79.433 | −2.86154, −2.8903, −4.234, −40.04 | −2.56677, −1.5384, −2.809, 0 |
   | `ct` | −3.95877, −9.0531, −28.428, −134.155 | −3.41049, −4.3904, −9.036, −45.374 | −3.12705, −2.5856, −3.925, −22.38 |
   | `ctt` | −4.37113, −11.5882, −35.819, −334.047 | −3.83239, −5.9057, −12.49, −118.284 | −3.55326, −3.6596, −5.293, −63.559 |

The regressions are fitted by `OrdinaryLeastSquares.Fit`, which solves through a Householder QR where
the reference uses a pseudo-inverse. That is an ordering difference on well-conditioned designs, not
a definitional one, and the corpus's `1e-9` holds it. `SSR` is `ResidualStandardError² ·
ResidualDegreesOfFreedom`, which is how the summary exposes it.

### KPSS

1. Residuals: `x − mean(x)` under `c`; under `ct`, `x` less its least-squares line on `t = 1 … n`.
2. Lags: `Legacy` is `min(ceil(12·(n/100)^¼), n − 1)`. `Automatic` is Hobijn et al. (1998) as the
   reference writes it — `covlags = ⌊n^(2/9)⌋`, `s₀` and `s₁` over those lags with each product
   divided by `n/2`, `γ = 1.1447·((s₁/s₀)²)^(1/3)`, lags `⌊γ·n^(1/3)⌋` capped at `n − 1`. `Fixed`
   takes `LagCount`, refused at or above `n`.
3. `η = Σ(cumulative residual sum)² / n²`, and the long-run variance
   `s² = (Σe² + 2·Σ_{i=1..L} (1 − i/(L+1))·Σ e[t]e[t−i]) / n`. The statistic is `η / s²`.
4. P-value: linear interpolation of the statistic over the critical values `0.347, 0.463, 0.574,
   0.739` (`c`) or `0.119, 0.146, 0.176, 0.216` (`ct`) against `0.10, 0.05, 0.025, 0.01`, clamped at
   both ends, with `PValueBound` set as above.

The line under `ct` is a two-parameter least-squares fit computed in closed form rather than through
`OrdinaryLeastSquares.Fit`, which refuses a design with no residual degrees of freedom — a case
`extrapolate_trend` reaches below — and whose intercept, VIF and interval work a line does not need.
This is not the second OLS 0095 refuses: it is the slope and intercept of a line, and the same
private `LineFit` serves decomposition's extrapolation.

### Seasonal decomposition

1. Refused below two full periods (`n < 2·period`) and below `period = 2`; multiplicative refuses a
   value at or below zero.
2. The filter is `[½, 1, …, 1, ½]/period` of length `period + 1` for an even period and
   `[1, …, 1]/period` of length `period` for an odd one. Two-sided: the trend at `t` is the filter
   centred on `t`, with `⌈L/2⌉ − 1` leading and `⌈L/2⌉ − L mod 2` trailing positions `NaN`. One-sided:
   the filter over `x[t−L+1 … t]`, with `L − 1` leading positions `NaN`.
3. `ExtrapolateTrend = e > 0` replaces the `NaN` ends by least-squares lines through the nearest
   `e + 1` defined points — the reference's own window, which **excludes the last defined point at
   the back** (`trend[back_first:back]`), kept as written. A window of one point takes the
   minimum-norm solution `lstsq` returns.
4. Detrend (`x − trend` or `x / trend`), average each phase `i mod period` ignoring `NaN`, and centre
   the averages (subtract their mean, or divide by it). The seasonal component tiles them; the
   residual is `detrended − seasonal`, or `x / seasonal / trend`.

## Refusals

`ArgumentException` naming the argument, in the register #617 used.

| input | where | what happens |
| --- | --- | --- |
| `NaN` or infinity in `series` | all three | refused, as #617 refuses |
| a constant series | ADF, KPSS | refused; ADF's reference raises on it too, and KPSS's divides by a zero variance |
| a series too short for the trend terms, or `MaxLag` above `n//2 − k − 1` | ADF | refused with the bound in the message |
| `LagCount ≥ n` under `Fixed` | KPSS | refused |
| `period < 2`, or `n < 2·period` | decomposition | refused |
| a value `≤ 0` under `Multiplicative` | decomposition | refused |

## Oracle

`statsmodels` 0.15.0, already in the lock. A new corpus, `tests/oracles/stats_stationarity.json`,
from `generate_stats_stationarity()` beside `generate_stats_timeseries()`, calling `adfuller`,
`kpss` and `seasonal_decompose` with `result_object=False` where the reference's future warning asks
for it. Fixtures, each series frozen in the corpus:

- a random walk, 200 points — ADF cannot reject (−2.48 under `c`). KPSS's automatic window widens
  so far on it that KPSS does not reject either (0.207), which is worth freezing: the two nulls do
  not always disagree from opposite sides, and a reader should not be told they do
- a stationary AR(1) at φ = 0.5, 200 points — the mirror
- a trend-stationary series, 150 points — the case `ct` exists for
- white noise, 400 points, at `MaxLag = 0` under `Fixed` — an ADF statistic below the table's
  minimum, so a p-value of exactly `0`
- an explosive AR(1) at φ = 1.03, 80 points — a statistic above the maximum, so exactly `1`
- series whose ADF statistics fall **either side of the switch point** under `c`, so both
  polynomials are exercised near the boundary where confusing them is invisible at 5 %
- KPSS statistics **either side of each end of the table** — the bound in both directions and a case
  just inside each
- a monthly seasonal series, 96 points, additive and multiplicative, two- and one-sided, at
  `ExtrapolateTrend` 0, 1 and `period − 1`; an odd period, 7, over 50 points; and the minimum,
  `period = 2` over 4 points, where the extrapolation window is one point

Every `regression` × every `autolag` for the ADF fixtures, every `nlags` rule for KPSS. Statistics,
critical values and components compare at `1e-9`; p-values through `StatsOracleAsserts` per
[decision 0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md); `UsedLag`,
`ObservationCount`, `LagCount` and `PValueBound` exactly; `NaN` positions exactly.

## Testing

`tests/Lodestar.Stats.TimeSeries.Tests/` takes #617's two test files and adds
`StationarityOracleTests`, `StationarityEdgeTests`, `SeasonalDecompositionOracleTests` and
`SeasonalDecompositionEdgeTests`, with `Lodestar.Stats.TimeSeries.NetStandard.Tests` linking them.
Asserted directly, because they hold for every input:

- additive: `trend + seasonal + residual == series` wherever the trend is defined
- the seasonal averages centre to zero (additive) or one (multiplicative) over a period
- ADF's p-value is non-decreasing in the statistic across the switch point, for every `regression`,
  to within the `3e-8` by which the reference's own large-p cubic turns over below the `ct` and `ctt`
  maxima

## Documentation owed, in the same commit as the code

- the package: `Version.props` at 0.1.0, the `EXPECTED` edges and `FLOORS`, `Lodestar.slnx`, both
  pack loops and every package list the guards check, `wiki-map.json` moving the namespace to the
  new package, the sample project reference
- reference pages for the thirteen new types, and #617's five pages moving with their types
- `docs/equivalence.md` rows, with the divergences: the QR against the pseudo-inverse,
  `PValueBound` for the warning, the refused constant series, `period` required, and `LineFit`
- `docs/migration/statsmodels.md`: the stationarity row stops reading *gap*
- a guide, `docs/guides/time-series-diagnostics.md`, covering both lots
- `CHANGELOG.md`, and `Lodestar.Stats`' own entry saying `SerialCorrelation` moved before any release
  carried it
- the ADR recording the placement, superseding 0114
- a `bench/README.md` section and numbers in `docs/guides/performance.md`, against `Cortex.TimeSeries`'
  `StationarityTests` and `SeasonalDecompose` where they agree first

## What would make this spec wrong

1. **The comparable-sample rule.** Fitting each candidate lag over its own longest sample instead of
   the common one changes which lag wins on every short series, and still looks reasonable. The lag
   search's `UsedLag` in the corpus is what catches it.
2. **The switch point.** Using the large-p polynomial everywhere is off by little near 5 % and by a
   lot in the tail; the fixtures either side of the switch are there for exactly that.
3. **The extrapolation window.** A reader "fixing" `trend[back_first:back]` to include `back` moves
   every extrapolated value at the back end; the corpus at `ExtrapolateTrend = 1` fails on it.
4. **The placement.** If `Lodestar.Stats.Regression` 0.1.0's `Fit` refuses a design this lot
   produces — a trend column collinear with a short sample — the edge buys less than it costs, and
   the plan's first task is the one that finds out.
