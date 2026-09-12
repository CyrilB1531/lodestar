# SerialCorrelation.PartialAutocorrelation

The partial autocorrelation function, with its confidence band.

<!-- docs-declaration -->

```csharp
public static AutocorrelationResult PartialAutocorrelation(ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions options = null)
```

**Parameters** — `series` are the observations, in time order. `lagCount` is how many lags past
zero to report, at most half the series length — required rather than defaulted, for the reason
[`Autocorrelation`](serialcorrelation-autocorrelation.md) gives; the reference defaults it to
`min(10·log10(n), n/2 - 1)` here, and to `min(10·log10(n), n - 1)` for `acf`. `options` is read for
[`AutocorrelationOptions.ConfidenceLevel`](autocorrelationoptions.md) only — `Adjusted` and
`BartlettConfidenceInterval` have no meaning for a partial autocorrelation and are ignored.

**Returns** — [`AutocorrelationResult`](autocorrelationresult.md): the reflection coefficients at
each lag from zero upward, and a flat band around them.

**Exceptions** — `ArgumentException` when `series` holds fewer than two points, is constant, or
carries a non-finite value; or when `lagCount` is below one or above half the series length.

**Example** — the same sawtooth series [`Autocorrelation`](serialcorrelation-autocorrelation.md)
reads, and its first four partial lags.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult result = SerialCorrelation.PartialAutocorrelation(series, lagCount: 4);

double p1 = Math.Round(result.Values[1], 4);              // => 0.646
double p3 = Math.Round(result.Values[3], 4);              // => -0.6651
double lower3 = Math.Round(result.ConfidenceLower[3], 4);  // => -1.2309
double upper3 = Math.Round(result.ConfidenceUpper[3], 4);  // => -0.0993
```

**Remarks** — `ywadjusted` (also spelled `yw`, `ywa`, `yw_adjusted`) is the reference's own default
and the only method shipped; the reference offers seven other estimators — `ywm`, `ols`,
`ols-inefficient`, `ols-adjusted`, `ld`, `ldbiased`, `burg` — across four families. Yule-Walker with
the adjusted autocovariance, solved by the Levinson-Durbin recursion, is what a caller gets by not
choosing, and it is the reference's default too. Widening this to an options enum is a later
change, not a reason to hold this one back.

**The band does not use Bartlett's formula.** Its variance is `1/n` at every lag past zero —
Quenouille's result, that a partial autocorrelation past the true order is asymptotically
`N(0, 1/n)` — unlike [`Autocorrelation`](serialcorrelation-autocorrelation.md)'s band, which
widens with the lag by default. Lag zero's interval is fixed at the point `[1, 1]` regardless of
`ConfidenceLevel`, which is what the reference also reports.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult result = SerialCorrelation.PartialAutocorrelation(series, lagCount: 4);

bool lagZeroIsAPoint = result.ConfidenceLower[0] == result.ConfidenceUpper[0];   // => True
double bandWidth = Math.Round(result.ConfidenceUpper[1] - result.ConfidenceLower[1], 4);  // => 1.1316
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.Autocorrelation`](serialcorrelation-autocorrelation.md),
[`SerialCorrelation.LjungBox`](serialcorrelation-ljungbox.md),
[`AutocorrelationOptions`](autocorrelationoptions.md), the
[Python equivalence table](../../../equivalence.md).
