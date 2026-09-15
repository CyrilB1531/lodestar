# `Lodestar.Stats`

The package-level reference for `Lodestar.Stats`: the ten families of hypothesis test live in
[their own section](stats/tests.md), and the tail distributions they lean on in
[theirs](stats/tails.md). The time-series diagnostics ship in their own package,
[`Lodestar.Stats.TimeSeries`](stats-timeseries/correlation.md). One type sits above both
sections, because it is shared by entry points across the package rather than belonging to any
single test.

## Types

| Type | What it is |
| --- | --- |
| [`NanPolicy`](stats/nanpolicy.md) | What a test does with a `NaN` in its input. |
