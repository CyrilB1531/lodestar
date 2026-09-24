using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Stats.TimeSeries still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.KpssLagRule))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.LagSelection))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.PValueBound))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.SeasonalModel))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.TrendTerms))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TimeSeries.VarOptions))]
