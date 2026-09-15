# Seasonality — `Lodestar.Stats.TimeSeries`

Classical decomposition of a series into a trend, a seasonal pattern and a residual, by moving
averages, at `statsmodels` 0.15.0's `seasonal_decompose` parity.

| type | carries |
| --- | --- |
| [`SeasonalDecomposition`](seasonality/seasonaldecomposition.md) | [`SeasonalDecomposition.Decompose`](seasonality/seasonaldecomposition-decompose.md), the one entry point |
| [`SeasonalDecompositionOptions`](seasonality/seasonaldecompositionoptions.md) | the model, the filter's sides and the trend extrapolation |
| [`SeasonalComponents`](seasonality/seasonalcomponents.md) | the three components, each the length of the series |
| [`SeasonalModel`](seasonality/seasonalmodel.md) | additive or multiplicative |

The period is required: the reference infers it from a pandas index, which a span does not have.
STL, the loess-based decomposition, is a different algorithm and is not here.

The [Python equivalence table](../../equivalence.md) maps each `statsmodels` call to its
counterpart here.
