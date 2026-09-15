# AutocorrelationOptions

What an autocorrelation may be told.

<!-- docs-declaration -->

```csharp
public sealed record AutocorrelationOptions
```

**Properties** — `Adjusted` says whether lag `k` divides by `n - k` rather than by `n`; `false` by
default, which is the reference's, and the estimator that keeps the sequence positive
semi-definite. `BartlettConfidenceInterval` says whether the band widens with the lag by
Bartlett's formula; `true` by default, which is the reference's — `false` gives the flat band a
correlogram usually draws. `ConfidenceLevel` is the two-sided level the band is reported at; `0.95`
by default.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)` — checked where the level is set, not where a band three functions later would otherwise
reach the caller at the wrong width with no exception naming it.

**Example** — the estimator, the band shape, and the level: three independent choices on the same
call.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult biased = SerialCorrelation.Autocorrelation(series, lagCount: 4);
AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
    series, lagCount: 4, new AutocorrelationOptions { Adjusted = true });

double r1Biased = Math.Round(biased.Values[1], 4);      // => 0.5922
double r1Adjusted = Math.Round(adjusted.Values[1], 4);  // => 0.646
```

**Remarks** — [`SerialCorrelation.PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md)
reads only `ConfidenceLevel` from this type: `Adjusted` and `BartlettConfidenceInterval` have no
meaning for a partial autocorrelation, so nothing here is silently ignored without saying so.

**`BartlettConfidenceInterval = false` widens lag zero's band, which the default narrows to a
point.** Bartlett's formula is exactly zero at lag zero, so the default band's `[1, 1]` there
matches the reference; the flat band applies its `1/n` variance uniformly, lag zero included,
because the reference's own non-Bartlett variance is a scalar rather than a per-lag formula.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult flatBand = SerialCorrelation.Autocorrelation(
    series, lagCount: 4, new AutocorrelationOptions { BartlettConfidenceInterval = false });

double lower0 = Math.Round(flatBand.ConfidenceLower[0], 4);  // => 0.4342
double upper0 = Math.Round(flatBand.ConfidenceUpper[0], 4);  // => 1.5658
```

Being a `record`, two option sets with the same three values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.Autocorrelation`](serialcorrelation-autocorrelation.md),
[`SerialCorrelation.PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md),
[`AutocorrelationResult`](autocorrelationresult.md), the
[Python equivalence table](../../../equivalence.md).
