# `Lodestar.Stats.TimeSeries`

Three serial-correlation diagnostics, at `statsmodels` 0.15.0 parity: the sample autocorrelation
function, its partial counterpart, and the Ljung-Box portmanteau test — a correlogram, and the
test that says whether it matters.

| diagnostic | what it asks | entry point |
| --- | --- | --- |
| autocorrelation | how strongly does the series echo itself at each lag? | [`SerialCorrelation.Autocorrelation`](timeseries/serialcorrelation-autocorrelation.md) |
| partial autocorrelation | the same, with the shorter lags' influence on longer ones removed | [`SerialCorrelation.PartialAutocorrelation`](timeseries/serialcorrelation-partialautocorrelation.md) |
| Ljung-Box | is any of it more than noise? | [`SerialCorrelation.LjungBox`](timeseries/serialcorrelation-ljungbox.md) |

All three are static methods on [`SerialCorrelation`](timeseries/serialcorrelation.md), each taking
a `ReadOnlySpan<double>` series and a required lag count. The reference defaults the lag count, and
to two *different* rules depending on which function is asked — `min(10·log10(n), n - 1)` for
`acf`, `min(10·log10(n), n/2 - 1)` for `pacf` — so this asks rather than silently picking one; pass
either rule deliberately to reproduce a reference plot.

| result | carries | returned by |
| --- | --- | --- |
| [`AutocorrelationResult`](timeseries/autocorrelationresult.md) | the sequence and its band, indexed from lag 0 | [`SerialCorrelation.Autocorrelation`](timeseries/serialcorrelation-autocorrelation.md), [`SerialCorrelation.PartialAutocorrelation`](timeseries/serialcorrelation-partialautocorrelation.md) |
| [`LjungBoxResult`](timeseries/ljungboxresult.md) | five lists cumulated lag by lag, indexed from lag 1 | [`SerialCorrelation.LjungBox`](timeseries/serialcorrelation-ljungbox.md) |

Two option records choose what a call is told; neither has a public constructor of its own to
speak of beyond the defaults, and both are validated where a property is set rather than where it
is later read.

| option | chooses |
| --- | --- |
| [`AutocorrelationOptions`](timeseries/autocorrelationoptions.md) | the estimator, the band shape, and its confidence level |
| [`LjungBoxOptions`](timeseries/ljungboxoptions.md) | the model's parameter count, and whether Box-Pierce is reported beside Ljung-Box |

The [Python equivalence table](../../equivalence.md) maps each `statsmodels` call to its
counterpart here.
