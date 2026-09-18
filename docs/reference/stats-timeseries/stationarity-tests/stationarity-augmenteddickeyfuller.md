# Stationarity.AugmentedDickeyFuller

The augmented Dickey-Fuller test, against the null of a unit root.

<!-- docs-declaration -->

```csharp
public static DickeyFullerResult AugmentedDickeyFuller(ReadOnlySpan<double> series, DickeyFullerOptions options = null)
```

**Parameters** — `series` are the observations, in time order. `options` sets the trend terms, the
lag rule and its maximum, or null for the reference's defaults: a constant, Akaike's criterion, and
Schwert's maximum lag.

**Returns** — [`DickeyFullerResult`](dickeyfullerresult.md): the statistic, MacKinnon's p-value and
critical values, and the lag the regression used.

**Exceptions** — `ArgumentException` when `series` carries a non-finite value, is constant, lies on
one straight line — a deterministic trend, with no stochastic component to test — is too short
for its trend terms and default lag, or builds a lagged design with no unique least-squares
solution at the lag used or at any lag the search tries; or when `options` asks for a maximum lag above `n/2 − terms − 1` or one that leaves the
widest regression no degree of freedom.

**Example** — a drifting series of 24 points. The lag search keeps three lagged differences, so the
regression fits 20 rows.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(walk);

double statistic = Math.Round(result.Statistic, 4);             // => 1.9817
double p = Math.Round(result.PValue, 4);                        // => 0.9986
int usedLag = result.UsedLag;                                   // => 3
int rows = result.ObservationCount;                             // => 20
double fivePercent = Math.Round(result.CriticalValues[1], 4);   // => -3.0216
double akaike = Math.Round(result.InformationCriterion, 4);     // => -37.8399
```

**Remarks** — the regression is `Δx[t]` on the lagged level `x[t−1]`, the lagged differences and
the trend terms; the statistic is the lagged level's t statistic, and each fit is
[`OrdinaryLeastSquares.Estimate`](../../stats-regression/ols/ordinaryleastsquares-estimate.md) from `Lodestar.Stats.Regression`, which stops short of the inference
table a lag search never reads.

**The lag search fits every candidate over the same rows**, `n − maxLag − 1` of them, so their
criteria compare; only the chosen lag is refitted over its own, longer sample. That is the
reference's rule, and fitting each candidate over its longest sample instead changes which lag wins
on short series.

**The p-value is MacKinnon's (1994) response surface** — exactly `1` above the table and exactly `0`
below it — and the critical values his 2010 ones, for the refitted row count.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult trend = Stationarity.AugmentedDickeyFuller(
    walk,
    new DickeyFullerOptions
    {
        Regression = TrendTerms.ConstantAndTrend,
        LagSelection = LagSelection.Fixed,
        MaxLag = 1,
    });

double statistic = Math.Round(trend.Statistic, 4);  // => -8.9132
double p = Math.Round(trend.PValue, 4);             // => 0
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.Kpss`](stationarity-kpss.md),
[`DickeyFullerOptions`](dickeyfulleroptions.md), [`DickeyFullerResult`](dickeyfullerresult.md), the
[Python equivalence table](../../../equivalence.md).
