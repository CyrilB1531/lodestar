# LjungBoxOptions

What a Ljung-Box test may be told.

<!-- docs-declaration -->

```csharp
public sealed record LjungBoxOptions
```

**Properties** — `ModelDegreesOfFreedom` is how many parameters the model whose residuals these
are consumed; `0` by default, for a raw series — each lag's degrees of freedom is its own lag less
this. `BoxPierce` says whether the Box-Pierce statistic is reported beside Ljung-Box; `false` by
default.

**Exceptions** — `ArgumentOutOfRangeException` when `ModelDegreesOfFreedom` is negative — a model
cannot consume a negative number of parameters, checked where the value is set.

**Example** — the same lag costs two degrees of freedom against a fitted model, so its p-value
moves even though the statistic itself does not.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult raw = SerialCorrelation.LjungBox(series, lagCount: 4);
LjungBoxResult residuals = SerialCorrelation.LjungBox(
    series, lagCount: 4, new LjungBoxOptions { ModelDegreesOfFreedom = 2 });

double rawStatistic = Math.Round(raw.Statistics[3], 4);        // => 11.7928
double residualStatistic = Math.Round(residuals.Statistics[3], 4);  // => 11.7928
double rawP = Math.Round(raw.PValues[3], 4);                    // => 0.019
double residualP = Math.Round(residuals.PValues[3], 4);         // => 0.0027
```

**Remarks** — Box-Pierce is `n·Σ r_k²`, where Ljung-Box is `n·(n + 2)·Σ r_k²/(n − k)` — the factor
`(n + 2)/(n − k)` is at least 1 at every lag, so Box-Pierce is the smaller of the two; it is here
because the reference offers it and a reader comparing an old paper's numbers needs it.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult result = SerialCorrelation.LjungBox(
    series, lagCount: 4, new LjungBoxOptions { BoxPierce = true });

double ljungBox4 = Math.Round(result.Statistics[3], 4);          // => 11.7928
double boxPierce4 = Math.Round(result.BoxPierceStatistics[3], 4);  // => 8.6989
```

Being a `record`, two option sets with the same two values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.LjungBox`](serialcorrelation-ljungbox.md),
[`LjungBoxResult`](ljungboxresult.md), the [Python equivalence table](../../../equivalence.md).
