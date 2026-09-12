# AutocorrelationResult

An autocorrelation sequence and the band around it, indexed by lag.

<!-- docs-declaration -->

```csharp
public sealed class AutocorrelationResult
```

**Properties** — `Values` is the correlation at each lag, from 0 — where it is always 1 — upwards.
`ConfidenceLower` is the lower end of the band, centred on `Values` rather than on zero.
`ConfidenceUpper` is the upper end.

**Example** — a correlogram: the value at each lag, bracketed by its own band.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult result = SerialCorrelation.Autocorrelation(series, lagCount: 4);

int lagCount = result.Values.Count;                        // => 5
double lag2 = Math.Round(result.Values[2], 4);             // => 0.568
double lag2Lower = Math.Round(result.ConfidenceLower[2], 4);  // => -0.1701
```

**Remarks — there is no public constructor.** A result is what
[`SerialCorrelation.Autocorrelation`](serialcorrelation-autocorrelation.md) and
[`SerialCorrelation.PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md) return,
never something a caller assembles by hand — the three lists are what a real computation
produces, not a promise a constructor could leave broken.

A class rather than a record: a record's equality would compare these three lists by reference,
so two results holding the same numbers would compare unequal
([#668](https://github.com/CyrilB1531/lodestar/issues/668)). Nobody compares two correlograms, so
this promises no equality at all rather than a broken one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.Autocorrelation`](serialcorrelation-autocorrelation.md),
[`SerialCorrelation.PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md),
[`AutocorrelationOptions`](autocorrelationoptions.md), the
[Python equivalence table](../../../equivalence.md).
