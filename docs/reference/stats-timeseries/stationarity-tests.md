# Stationarity tests — `Lodestar.Stats.TimeSeries`

Two tests that answer the question a correlogram raises and cannot settle: may this series be
modelled as it stands, or does it need differencing or detrending first? Both at `statsmodels`
0.15.0 parity.

| test | its null | entry point |
| --- | --- | --- |
| augmented Dickey-Fuller | the series has a unit root | [`Stationarity.AugmentedDickeyFuller`](stationarity-tests/stationarity-augmenteddickeyfuller.md) |
| KPSS | the series is stationary around a level or a line | [`Stationarity.Kpss`](stationarity-tests/stationarity-kpss.md) |

**The nulls are opposite, so a reader runs both.** A small ADF p-value rejects a unit root; a small
KPSS p-value rejects stationarity. Rejecting one and not the other points the same way from two
sides; rejecting both or neither says the series is too short, or its trend is not what the test
assumed.

| type | carries |
| --- | --- |
| [`Stationarity`](stationarity-tests/stationarity.md) | the two tests |
| [`DickeyFullerOptions`](stationarity-tests/dickeyfulleroptions.md) | the trend terms, the lag rule and the maximum lag |
| [`DickeyFullerResult`](stationarity-tests/dickeyfullerresult.md) | the statistic, MacKinnon's p-value and critical values, the lag used |
| [`KpssOptions`](stationarity-tests/kpssoptions.md) | the null's trend and the lag window rule |
| [`KpssResult`](stationarity-tests/kpssresult.md) | the statistic, its tabulated p-value, and whether that p-value was clamped |
| [`TrendTerms`](stationarity-tests/trendterms.md) | constant, linear trend, quadratic trend, or nothing |
| [`LagSelection`](stationarity-tests/lagselection.md) | Akaike, Schwarz, the t statistic, or a fixed lag |
| [`KpssLagRule`](stationarity-tests/kpsslagrule.md) | Hobijn's automatic window, Schwert's legacy one, or a fixed one |
| [`PValueBound`](stationarity-tests/pvaluebound.md) | which way the truth lies when a p-value is the end of its table |

The [Python equivalence table](../../equivalence.md) maps each `statsmodels` call to its
counterpart here.
