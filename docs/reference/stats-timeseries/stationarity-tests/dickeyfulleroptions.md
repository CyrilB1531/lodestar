# DickeyFullerOptions

What an augmented Dickey-Fuller test may be told.

<!-- docs-declaration -->

```csharp
public sealed record DickeyFullerOptions
```

**Properties** — `Regression` is the regression's deterministic terms, a
[`TrendTerms`](trendterms.md); `Constant` by default. `LagSelection` is how the lag order is
chosen, a [`LagSelection`](lagselection.md); `Akaike` by default. `MaxLag` is the largest lag the
search considers, or the lag itself under `LagSelection.Fixed`; null by default, which takes
Schwert's `ceil(12·(n/100)^¼)` capped at `n/2 − terms − 1`.

**Exceptions** — `ArgumentOutOfRangeException` when `MaxLag` is negative, or `Regression` or
`LagSelection` is a value its enum does not declare, checked where the value is set.

**Example** — Schwarz's criterion picks the same lag as Akaike's on this series, and reports its own
value.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult schwarz = Stationarity.AugmentedDickeyFuller(
    walk, new DickeyFullerOptions { LagSelection = LagSelection.Schwarz });

int usedLag = schwarz.UsedLag;                                  // => 3
double criterion = Math.Round(schwarz.InformationCriterion, 4);  // => -34.6446
```

**Remarks** — without a constant the table changes too: `TrendTerms.None` reads MacKinnon's
no-constant surface, which has no upper cut-off, so a large positive statistic reads a p-value close
to `1` rather than exactly `1`.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult bare = Stationarity.AugmentedDickeyFuller(
    walk,
    new DickeyFullerOptions { Regression = TrendTerms.None, LagSelection = LagSelection.Fixed, MaxLag = 2 });

double statistic = Math.Round(bare.Statistic, 4);            // => 3.9128
double p = Math.Round(bare.PValue, 4);                       // => 1
double onePercent = Math.Round(bare.CriticalValues[0], 4);   // => -2.6804
```

Being a `record` of value types, two option sets with the same three values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md),
[`DickeyFullerResult`](dickeyfullerresult.md), the [Python equivalence table](../../../equivalence.md).
