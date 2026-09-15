# Stationarity

Whether a series may be modelled as it stands: the unit-root test and its complement.

<!-- docs-declaration -->

```csharp
public static class Stationarity
```

**Example** — a drifting series, which the augmented Dickey-Fuller test cannot call stationary and
KPSS calls non-stationary.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

double adfP = Math.Round(Stationarity.AugmentedDickeyFuller(walk).PValue, 4);  // => 0.9986
double kpssP = Math.Round(Stationarity.Kpss(walk).PValue, 4);                  // => 0.0128
```

**Remarks** — the two nulls are opposite. The augmented Dickey-Fuller test assumes a unit root and
KPSS assumes stationarity, so a small p-value from each points the other way: here ADF cannot reject
a unit root and KPSS rejects stationarity, and the two agree that the series needs differencing.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stationarity tests index](../stationarity-tests.md),
[`SerialCorrelation`](../correlation/serialcorrelation.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`Stationarity.AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md) | The augmented Dickey-Fuller test, against the null of a unit root. |
| [`Stationarity.Kpss`](stationarity-kpss.md) | The KPSS test, against the null of stationarity around a level or a line. |
