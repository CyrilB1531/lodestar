# KpssResult

A KPSS test: the statistic, its tabulated p-value and whether that p-value was clamped.

<!-- docs-declaration -->

```csharp
public sealed class KpssResult
```

**Properties** — `Statistic` is the residual partial-sum statistic over the long-run variance.
`PValue` is the p-value against the null of stationarity, interpolated in Kwiatkowski et al.'s table.
`LagCount` is the window the long-run variance used. `CriticalValues` holds the critical values at
10 %, 5 %, 2.5 % and 1 %. `PValueBound` says whether `PValue` is the table's end rather than an
interpolation, and which way the truth lies — a [`PValueBound`](pvaluebound.md).

**Example** — a statistic below the 10 % critical value comes back as the table's largest p-value,
with the direction beside it.

```csharp
using Lodestar.Stats.TimeSeries;

double[] noise = [0.3, -0.5, 0.9, -0.2, 0.1, -0.8, 0.6, -0.4, 0.2, -0.1, 0.7, -0.6];

KpssResult result = Stationarity.Kpss(noise);

double statistic = Math.Round(result.Statistic, 4);  // => 0.3277
double p = result.PValue;                            // => 0.1
PValueBound bound = result.PValueBound;              // => ActualIsGreater
double tenPercent = result.CriticalValues[0];        // => 0.347
```

**Remarks — there is no public constructor.** A result is what
[`Stationarity.Kpss`](stationarity-kpss.md) returns. The reference returns the same end value and
raises an `InterpolationWarning`; a library has no warning channel a caller reads, so the direction is
a property.

A class rather than a record, for the reason
[`AutocorrelationResult`](../correlation/autocorrelationresult.md) gives.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.Kpss`](stationarity-kpss.md), [`KpssOptions`](kpssoptions.md), the
[Python equivalence table](../../../equivalence.md).
