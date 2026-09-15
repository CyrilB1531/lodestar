# PValueBound

Whether a tabulated p-value was clamped at the end of its table, and which way the truth lies.

<!-- docs-declaration -->

```csharp
public enum PValueBound
```

**Fields** — `None` means the statistic fell inside the table and the p-value is interpolated.
`ActualIsSmaller` means the statistic is at or past the table's smallest p-value, which came back,
so the true p-value is smaller. `ActualIsGreater` means the statistic is at or before the table's
largest p-value, which came back, so the true p-value is greater.

**Example** — a narrow window pushes the statistic past the 1 % critical value.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

var fixedWindow = new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = 2 };

PValueBound automatic = Stationarity.Kpss(walk).PValueBound;           // => None
PValueBound narrow = Stationarity.Kpss(walk, fixedWindow).PValueBound;  // => ActualIsSmaller
```

**Remarks** — the reference raises an `InterpolationWarning` exactly when it returns an end of the
table; this is that warning as a value.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KpssResult`](kpssresult.md), [`Stationarity.Kpss`](stationarity-kpss.md).
