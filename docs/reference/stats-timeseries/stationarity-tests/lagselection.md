# LagSelection

How the augmented Dickey-Fuller test chooses its lag order.

<!-- docs-declaration -->

```csharp
public enum LagSelection
```

**Fields** — `Akaike` takes the smallest Akaike criterion, the reference's `"AIC"` and the default.
`Schwarz` takes the smallest Schwarz criterion, `"BIC"`. `TStatistic` walks down from the maximum
and stops at the first lag whose own t statistic reaches 1.645, `"t-stat"`. `Fixed` searches
nothing and uses the maximum lag as the lag, `autolag=None`.

**Example** — a fixed lag reports no criterion.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult fixedLag = Stationarity.AugmentedDickeyFuller(
    walk, new DickeyFullerOptions { LagSelection = LagSelection.Fixed, MaxLag = 1 });

int usedLag = fixedLag.UsedLag;                                     // => 1
bool noCriterion = double.IsNaN(fixedLag.InformationCriterion);     // => True
```

**Remarks** — a tie between two criteria keeps the shorter lag, as the reference's `min` over
`(criterion, lag)` pairs does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DickeyFullerOptions`](dickeyfulleroptions.md),
[`DickeyFullerResult`](dickeyfullerresult.md).
