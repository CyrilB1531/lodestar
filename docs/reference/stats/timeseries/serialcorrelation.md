# SerialCorrelation has moved

`SerialCorrelation` and its four companion types ship in `Lodestar.Stats.TimeSeries` from that
package's first release, and never shipped in a `Lodestar.Stats` release. The namespace did not
change, so a `using Lodestar.Stats.TimeSeries;` compiles as it did; only the package reference does.

The reference lives at [`SerialCorrelation`](../../stats-timeseries/correlation/serialcorrelation.md).
This page stays because an accepted decision record links here, and a decision record is not edited.
