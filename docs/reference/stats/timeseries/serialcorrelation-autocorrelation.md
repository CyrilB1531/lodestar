# SerialCorrelation.Autocorrelation

The autocorrelation function, with its confidence band.

<!-- docs-declaration -->

```csharp
public static AutocorrelationResult Autocorrelation(ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions options = null)
```

**Parameters** — `series` are the observations, in time order. `lagCount` is how many lags past
zero to report — required rather than defaulted, because the reference defaults it to two
*different* rules depending on which function is asked: `min(10·log10(n), n - 1)` here, and
`min(10·log10(n), n/2 - 1)` for
[`PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md) — a default that differs
between two functions read side by side is a trap, so pass either rule deliberately to reproduce a
reference plot. `options` chooses the estimator, the band shape and its level, or null for the
defaults.

**Returns** — [`AutocorrelationResult`](autocorrelationresult.md): the correlation at each lag from
zero upward, and the band around it.

**Exceptions** — `ArgumentException` when `series` holds fewer than two points, is constant, or
carries a non-finite value; or when `lagCount` is below one or reaches `series.Length`.

**Example** — a sawtooth series, and its first four lags.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult result = SerialCorrelation.Autocorrelation(series, lagCount: 4);

double r1 = Math.Round(result.Values[1], 4);              // => 0.5922
double lower1 = Math.Round(result.ConfidenceLower[1], 4);  // => 0.0264
double upper1 = Math.Round(result.ConfidenceUpper[1], 4);  // => 1.158
```

**Remarks — the autocovariance is a direct double sum, where the reference's `acf` defaults to
`fft=True`.** Measured on this branch's own fixtures
(`tools/generate_oracles.py`'s `_timeseries_fixtures`) against `statsmodels` 0.15.0 directly: the
largest gap between `acf(x, nlags=m, adjusted=True, fft=False)` and the same call with
`fft=True` is `4.44e-16` — two units in the last place of a `double`, an ordering difference in
how the same sum is accumulated, not a definitional one. The oracle corpus itself is generated at
`fft=False` and compares at `1e-9` ([`docs/equivalence.md`](../../../equivalence.md)), well above
either figure, so nothing here is masked by a looser tolerance than the one that would catch a
real disagreement.

**A constant series is refused.** The reference answers `NaN` with a warning; this throws, as
[`KruskalWallis.Test`](../tests/kruskalwallis-test.md) already refuses a fully tied pooled sample
for the same reason — `avf[0]` is zero, so every ratio would be `0/0`.

```csharp
using Lodestar.Stats.TimeSeries;

string message = "nothing was thrown";
try
{
    SerialCorrelation.Autocorrelation([5.0, 5.0, 5.0], 1);
}
catch (ArgumentException error)
{
    message = error.Message;
}

string what = message;   // => every value is 5…
```

**Adjusted divides by a smaller denominator at every lag past zero.** `Adjusted = true` divides lag
`k` by `n - k` rather than by `n`; lag zero divides by `n` either way, since `n - 0` is `n`.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult biased = SerialCorrelation.Autocorrelation(series, lagCount: 4);
AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
    series, lagCount: 4, new AutocorrelationOptions { Adjusted = true });

double r1Biased = Math.Round(biased.Values[1], 4);      // => 0.5922
double r1Adjusted = Math.Round(adjusted.Values[1], 4);  // => 0.646
```

**The flat, non-Bartlett band applies at lag zero too.** Bartlett's formula — the default — is
exactly zero at lag zero, so `ConfidenceLower[0]` and `ConfidenceUpper[0]` are both `1`, the point
the reference also reports. `BartlettConfidenceInterval = false` uses a flat variance of `1/n` at
*every* lag, lag zero included, because the reference's own non-Bartlett variance is a scalar
applied uniformly — this surprised task 5 of [#617](https://github.com/CyrilB1531/lodestar/issues/617)
enough to earn its own test.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

AutocorrelationResult defaultBand = SerialCorrelation.Autocorrelation(series, lagCount: 4);
AutocorrelationResult flatBand = SerialCorrelation.Autocorrelation(
    series, lagCount: 4, new AutocorrelationOptions { BartlettConfidenceInterval = false });

bool defaultLagZeroIsAPoint = defaultBand.ConfidenceLower[0] == defaultBand.ConfidenceUpper[0];  // => True
bool flatLagZeroIsAPoint = flatBand.ConfidenceLower[0] == flatBand.ConfidenceUpper[0];            // => False
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.PartialAutocorrelation`](serialcorrelation-partialautocorrelation.md),
[`SerialCorrelation.LjungBox`](serialcorrelation-ljungbox.md),
[`AutocorrelationOptions`](autocorrelationoptions.md), the
[Python equivalence table](../../../equivalence.md).
