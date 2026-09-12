# SerialCorrelation.LjungBox

The Ljung-Box test for serial dependence, cumulated lag by lag.

<!-- docs-declaration -->

```csharp
public static LjungBoxResult LjungBox(ReadOnlySpan<double> series, int lagCount, LjungBoxOptions options = null)
```

**Parameters** — `series` are the observations, or a model's residuals, in time order. `lagCount`
is the highest lag to test, at most one below the series length. `options` sets the model's
parameter count and whether Box-Pierce is reported beside Ljung-Box, or null for the defaults.

**Returns** — [`LjungBoxResult`](ljungboxresult.md): five lists of length `lagCount`, indexed from
lag 1.

**Exceptions** — `ArgumentException` when `series` holds fewer than two points, is constant, or
carries a non-finite value; or when `lagCount` is below one or reaches `series.Length`.

**Example** — the same sawtooth series
[`Autocorrelation`](serialcorrelation-autocorrelation.md) reads, tested to lag 4.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult result = SerialCorrelation.LjungBox(series, lagCount: 4);

double q4 = Math.Round(result.Statistics[3], 4);  // => 11.7928
double p4 = Math.Round(result.PValues[3], 4);     // => 0.019
```

**Remarks** — `Q(m) = n·(n + 2) · Σ_{k=1..m} r[k]² / (n - k)`, on `r` the biased autocorrelation
(`Adjusted = false`), the same estimator [`Autocorrelation`](serialcorrelation-autocorrelation.md)
computes by a direct sum rather than the reference's `fft=True` default — the two agree to
`4.44e-16` on this branch's own fixtures, well inside the `1e-9` the oracle corpus compares at.

**A lag with no degrees of freedom left answers `NaN`, not an exception.** Each lag's degrees of
freedom is the lag itself less
[`LjungBoxOptions.ModelDegreesOfFreedom`](ljungboxoptions.md); once that reaches zero or below, the
statistic is still reported but the p-value is `NaN` — the reference does the same rather than
raising, and only `ModelDegreesOfFreedom` itself being negative is refused.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult result = SerialCorrelation.LjungBox(
    series, lagCount: 4, new LjungBoxOptions { ModelDegreesOfFreedom = 2 });

int dfAtLag2 = result.DegreesOfFreedom[1];              // => 0
bool noDegreesOfFreedomLeft = double.IsNaN(result.PValues[1]);  // => True
double pAtLag4 = Math.Round(result.PValues[3], 4);      // => 0.0027
```

**Box-Pierce is the same sum without the `n/(n - k)` weight** — smaller, and less powerful in a
short series — reported alongside Ljung-Box only when
[`LjungBoxOptions.BoxPierce`](ljungboxoptions.md) asks for it.

```csharp
using Lodestar.Stats.TimeSeries;

double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

LjungBoxResult result = SerialCorrelation.LjungBox(
    series, lagCount: 4, new LjungBoxOptions { BoxPierce = true });

double boxPierce4 = Math.Round(result.BoxPierceStatistics[3], 4);  // => 8.6989
double boxPierceP4 = Math.Round(result.BoxPiercePValues[3], 4);    // => 0.0691
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SerialCorrelation.Autocorrelation`](serialcorrelation-autocorrelation.md),
[`LjungBoxOptions`](ljungboxoptions.md), [`LjungBoxResult`](ljungboxresult.md), the
[Python equivalence table](../../../equivalence.md).
