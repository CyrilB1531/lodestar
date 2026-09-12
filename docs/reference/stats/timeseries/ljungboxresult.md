# LjungBoxResult

A Ljung-Box test at each lag, indexed from lag 1.

<!-- docs-declaration -->

```csharp
public sealed class LjungBoxResult
```

**Properties** — `Statistics` is the Ljung-Box statistic cumulated to each lag. `PValues` is its
chi-square p-value, `NaN` where no degree of freedom is left. `BoxPierceStatistics` is the
Box-Pierce statistic, empty unless [`LjungBoxOptions.BoxPierce`](ljungboxoptions.md) asked for it.
`BoxPiercePValues` is Box-Pierce's p-value, empty unless it was asked for. `DegreesOfFreedom` is
the lag less the model's parameters, which may be zero or negative.

**Example** — five parallel lists rather than a list of five-field rows, because a caller plots a
column.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult result = SerialCorrelation.LjungBox(
    series, lagCount: 4, new LjungBoxOptions { BoxPierce = true });

int lastLagDf = result.DegreesOfFreedom[3];                       // => 4
double lastStatistic = Math.Round(result.Statistics[3], 4);       // => 11.7928
double lastBoxPierce = Math.Round(result.BoxPierceStatistics[3], 4);  // => 8.6989
```

**Remarks — there is no public constructor.** A result is what
[`SerialCorrelation.LjungBox`](serialcorrelation-ljungbox.md) returns, never something a caller
assembles by hand.

`BoxPierceStatistics` and `BoxPiercePValues` are empty lists, not lists of `NaN`, when
[`LjungBoxOptions.BoxPierce`](ljungboxoptions.md) was left at its `false` default — a reader
indexing them by mistake gets an `IndexOutOfRangeException` naming the omission rather than a
silent `NaN`.

A class rather than a record, for the reason [`AutocorrelationResult`](autocorrelationresult.md)
gives: a record's equality would compare these five lists by reference.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.LjungBox`](serialcorrelation-ljungbox.md),
[`LjungBoxOptions`](ljungboxoptions.md), the [Python equivalence table](../../../equivalence.md).
